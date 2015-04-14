using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization
{
    public class BaseSyncHandler : COM.TIGER.TASK.Task.JobBaseHandler
    {
        /// <summary>
        /// 数据同步配置节点名称
        /// </summary>
        protected static readonly string SYNC_SECTION_NAME = "synchronization";

        /// <summary>
        /// 数据同步配置
        /// </summary>
        protected static readonly SyncModel SYNC_SECTION_CONFIG = null;

        static BaseSyncHandler()
        {
            SYNC_SECTION_NAME = "synchronization";
            //获取配置文件
            var configreader = new COM.TIGER.TASK.Configuration.DefaultConfigurationReader();
            SYNC_SECTION_CONFIG = configreader.GetSection<SyncModel>(SYNC_SECTION_NAME);
        }

        public override object EventContext
        {
            get { return GetType(); }
        }

        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            var modulename = context.JobInstance.GetType().FullName;
            var config = SYNC_SECTION_CONFIG[modulename];

            var dbfrom = CreateDataHandler(string.IsNullOrWhiteSpace(config.DbFrom) ? SYNC_SECTION_CONFIG.DefaultContext.DbFrom : config.DbFrom);
            var dbtarget = CreateDataHandler(string.IsNullOrWhiteSpace(config.DbTarget) ? SYNC_SECTION_CONFIG.DefaultContext.DbTarget : config.DbTarget);
            var commandText = string.IsNullOrWhiteSpace(config.CommandText) ? SYNC_SECTION_CONFIG.DefaultContext.CommandText : config.CommandText;

            Execute(dbfrom, dbtarget, commandText);
        }

        private COM.TIGER.TASK.DAT.Synchronization.Dao.DataHandler CreateDataHandler(string config)
        {
            var arr = config.Split(',', ';');
            COM.TIGER.TASK.DAT.Synchronization.Dao.DataHandler hd = null;
            switch (arr[0].ToLower())
            {
                case "oracle":
                    hd = new COM.TIGER.TASK.DAT.Synchronization.Dao.OracleHandler(arr[1], arr[2], arr[3]);
                    break;
                default:
                    hd = new COM.TIGER.TASK.DAT.Synchronization.Dao.MSSqlServerHandler(arr[1], arr[2], arr[3], arr[4]);
                    break;
            }
            return hd;
        }

        protected virtual void Execute(COM.TIGER.TASK.DAT.Synchronization.Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        { 
            
        }
    }

    public abstract class BaseSyncHandler<T> : BaseSyncHandler where T : DatBaseModel, new()
    {
        protected TASK.Task.SyncEventArg _eventContext = null;
        public override object EventContext
        {
            get { return _eventContext; }
        }

        protected override void Execute(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected virtual void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Console.WriteLine("## start time {0}----------------------------------------------", DateTime.Now);

            try
            {
                using (System.Data.IDataReader reader = dbFrom.ExecuteReader(dataFromCmdString))
                {
                    while (reader.Read())
                    {
                        var t = Read(reader);
                        _eventContext = new Task.SyncEventArg(t);
                        EventTigger();
                        Executed(t, dbTarget);
                    }
                }
            }
            catch (Exception e) 
            {
                _eventContext = new Task.SyncEventArg(null, Task.SyncEventLevel.EXCEPTION, e, "获取数据信息错误");
                EventTigger();
                Console.WriteLine("错误：{0}", e.Message); 
            }

            Console.WriteLine("## end time {0}----------------------------------------------", DateTime.Now);
        }

        protected virtual T Read(System.Data.IDataReader reader)
        {
            var tp = typeof(T);
            var properties = tp.GetProperties();

            var t = new T();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var property = properties.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                if (!reader.IsDBNull(i) && property != null && property.CanWrite)
                {
                    var obj = reader.GetValue(i);
                    if (obj != null)
                    {
                        property.SetValue(t, obj, null);
                    }
                }
            }

            return t;
        }

        protected virtual T GetEntity(Dao.DataHandler db, string cmdStr)
        {
            try
            {
                var t = default(T);
                using (System.Data.IDataReader reader = db.ExecuteReader(cmdStr))
                {
                    while (reader.Read())
                    {
                        t = Read(reader);
                        break;
                    }
                }
                return t;
            }
            catch (Exception e)
            {
                Console.WriteLine("错误：{0}", e.Message);

                return default(T);
            }
        }

        protected abstract void Executed(T t, Dao.DataHandler dbTarget);

        protected virtual int GetPopulationID(string cardno, Dao.DataHandler db)
        {
            if (string.IsNullOrWhiteSpace(cardno))
                return 0;

            var str = string.Format("select id from Pgis_PopulationBasicInfo where cardno = '{0}'", cardno);
            var obj = db.ExecuteScalar(str);

            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual int CheckParam(Dao.DataHandler db, string name, string code = null, int pid = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                return 0;

            var obj = db.ExecuteScalar(GetParamCmd(name));
            if (obj == null)
            {
                code = code ?? GetCode();
                db.ExecuteNonQuery(InsertParamCmd(pid, code, name));
                obj = db.ExecuteScalar(GetParamCmd(name));
            }

            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual string GetParamCmd(string name)
        {
            return string.Format("select id from PGis_Param where name = '{0}'", name);
        }

        protected virtual string InsertParamCmd(int pid, string code, string name)
        {
            return string.Format("insert into PGis_Param(pid, code, name, disabled) values({0}, '{1}', '{2}', 1)", pid, code, name);
        }

        protected virtual int GetCompanyID(string token, Dao.DataHandler db)
        {
            var str = string.Format("select id from Pgis_Company where token = '{0}'", token);
            var obj = db.ExecuteScalar(str);
            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual int GetHotelID(string token, Dao.DataHandler db)
        {
            var str = string.Format("select id from Pgis_Hotel where token = '{0}'", token);
            var obj = db.ExecuteScalar(str);
            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual int GetSex(string code, Dao.DataHandler db, out string name)
        {
            switch (code)
            {
                case "1":
                    name = "男";
                    break;
                case "2":
                    name = "女";
                    break;
                default:
                    name = "其他";
                    break;
            }

            return CheckParam(db, name, GetCode(), 13);
        }

        protected virtual int CheckAddress(string addr, Dao.DataHandler db)
        {
            if (string.IsNullOrWhiteSpace(addr))
                return 0;

            var obj = db.ExecuteScalar(GetAddressCmd(addr));
            if (obj != null)
                return int.Parse(string.Format("{0}", obj));

            db.ExecuteNonQuery(InsertAddressCmd(addr));
            obj = db.ExecuteScalar(GetAddressCmd(addr));
            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual int CheckArea(Dao.DataHandler db, out string outName, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "其它";

            outName = name;

            var obj = db.ExecuteScalar(GetAreaCmd(name));
            if (obj == null)
            {
                db.ExecuteNonQuery(InsertAreaCmd(name));
                obj = db.ExecuteScalar(GetAreaCmd(name));
            }

            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual int CheckAdministrative(Dao.DataHandler db, out string outName, string name = "其它")
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "其它";
            outName = name;

            var obj = db.ExecuteScalar(GetAdminCmd(name));
            if (obj == null)
            {
                db.ExecuteNonQuery(InsertAdminCmd(db, name));
                obj = db.ExecuteScalar(GetAdminCmd(name));
            }

            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected virtual string GetAdminCmd(string name)
        {
            return string.Format("select id from Pgis_Administrative where name = '{0}'", name);
        }

        protected virtual string InsertAdminCmd(Dao.DataHandler db, string name)
        {
            int areaid = 0;
            string areaname = "";
            areaid = CheckArea(db, out areaname);

            var fields = new string[] 
            {
                "Name", "Code", "PID", "FirstLetter", "AreaID", "AreaName"
            };
            var values = new string[] 
            {
                string.Format("'{0}'", name),
                string.Format("'{0}'", GetCode()),
                "'0'",
                GetFirstLetter(name),
                string.Format("'{0}'", areaid),
                string.Format("'{0}'", areaname)
            };

            return string.Format("insert into Pgis_Administrative({0}) values({1})", string.Join(",", fields), string.Join(",", values));
        }

        protected virtual string GetAreaCmd(string name)
        {
            return string.Format("select id from PGis_Area where name = '{0}'", name);
        }

        protected virtual string InsertAreaCmd(string name)
        {
            var fields = new string[] 
            {
                "pid", "name", "newcode"
            };
            var values = new string[] 
            {
                "'0'", string.Format("'{0}'", name), string.Format("'{0}'", GetCode())
            };

            return string.Format("insert into PGis_Area({0}) values({1})", string.Join(",", fields), string.Join(",", values));
        }

        protected virtual string GetAddressCmd(string addr)
        {
            return string.Format("select id from Pgis_Address where Content = '{0}'", addr);
        }

        protected virtual string InsertAddressCmd(string addr)
        {
            return string.Format("insert into Pgis_Address(Content) values('{0}')", addr);
        }

        protected string GetCode()
        {
            return string.Format("{0}", GetGuid());
        }

        protected long GetGuid()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 获取指定名称的首字母大写组合
        /// eg. 
        ///     '首字母'->'SZM'
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetFirstLetter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "NULL";

            return string.Format("dbo.fun_getPYFirst('{0}')", str);
        }
    }
}

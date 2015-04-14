using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.JCJ_JJDB
{
    public class Job:BaseSyncHandler<Model>
    {
        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            base.ExecuteJob(context);
        }

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            try
            {
                Model.tick = 0;
                Console.WriteLine("三台合一平台数据同步开始 ...");
                var maxnum = GetMax(dbTarget);

                if (!string.IsNullOrWhiteSpace(maxnum))
                {
                    dataFromCmdString = dataFromCmdString.ToLower();
                    if (dataFromCmdString.Contains(" where "))
                        dataFromCmdString = string.Format("{0} and BJSJ > TO_DATE('{1}', 'yyyy-mm-dd hh24:mi:ss')", dataFromCmdString, maxnum);
                    else
                        dataFromCmdString = string.Format("{0} where BJSJ > TO_DATE('{1}', 'yyyy-mm-dd hh24:mi:ss')", dataFromCmdString, maxnum);
                }

                base.Executed(dbFrom, dbTarget, dataFromCmdString);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                _eventContext = new Task.SyncEventArg(this, Task.SyncEventLevel.EXCEPTION, e, "错误");
                EventTigger();
            }
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            try
            {
                Console.WriteLine(t.ToString());

                string adminname = null;
                t.AdminID = CheckAdministrative(dbTarget, out adminname, t.GXDWDM);

                string typename = null;
                t.TypeID = CheckParam(dbTarget, t.BJFSDM, out typename);
                t.TypeName = typename;

                string cmdStr = t.InsertCmd();
                dbTarget.ExecuteNonQuery(cmdStr);
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }

        private string GetMax(Dao.DataHandler db)
        {
            return string.Format("{0}", db.ExecuteScalar(GetMaxCmd()));
        }

        public static string GetMaxCmd()
        {
            return string.Format("select max(AlarmTime) from {0}", "Pgis_JCJ_JJDB");
        }

        private int CheckParam(Dao.DataHandler db, string code, out string name)
        {
            switch (code)
            {
                case "01":
                    name = "110 报警";
                    break;
                case "02":
                    name = "122 报警";
                    break;
                case "03":
                    name = "119 报警";
                    break;
                default:
                    name = "其它";
                    break;
            }

            return CheckParam(db, name, GetCode(), 5);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.DWCYRY
{
    public class Job:BaseSyncHandler<Model>
    {
        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            base.ExecuteJob(context);
        }

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Model.tick = 0;
            Console.WriteLine("单位从业人员数据同步开始 ...");
            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            try
            {
                Console.WriteLine(t.ToString());

                //获取性别编码
                string sexdesc = "";
                t.GenderID = GetSex(t.XB, dbTarget, out sexdesc);
                t.GenderDesc = sexdesc;

                //获取单位编码
                int typeid = 0;
                string typename = null;
                t.OrganID = GetOrganID(t.DWID, dbTarget, out typeid, out typename);
                t.OrganTypeID = typeid;
                t.OrganTypeName = typename;

                t.CardTypeName = "身份证";
                t.CardTypeID = CheckParam(dbTarget, t.CardTypeName, GetCode(), 16);

                var obj = dbTarget.ExecuteScalar(t.ExistCmd());
                if (obj == null)
                {
                    dbTarget.ExecuteNonQuery(t.InsertCmd());
                    return;
                }
                dbTarget.ExecuteNonQuery(t.UpdateCmd(obj));
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }

        protected override int GetSex(string xb, Dao.DataHandler db, out string desc)
        {
            switch (xb)
            {
                case "1":
                    desc = "男";
                    break;
                case "2":
                    desc = "女";
                    break;
                default:
                    desc = "其它";
                    break;
            }

            return CheckParam(db, desc, GetCode(), 13);
        }

        private int GetOrganID(string token, Dao.DataHandler db, out int typeid, out string typename)
        {
            var obj = GetCompanyID(token, db);
            if (obj > 0)
            {
                typename = "企事业单位";
                typeid = CheckParam(db, typename, GetCode(), 15);
                return obj;
            }

            obj = GetHotelID(token, db);
            if (obj > 0)
            {
                typename = "酒店，宾馆，旅店";
                typeid = CheckParam(db, typename, GetCode(), 15);
                return obj;
            }

            typename = "";
            typeid = 0;
            return 0;
        }
    }
}

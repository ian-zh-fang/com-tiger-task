using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.DWJBXX
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
            Console.WriteLine("单位基本信息数据同步开始 ...");
            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            Console.WriteLine(t.ToString());
            try
            {
                string typename = null;
                t.TypeID = GetType(t.DWGXLB, dbTarget, out typename);
                t.TypeName = typename;

                t.Corporation = string.Format("{0}", dbTarget.ExecuteScalar(t.GetCorporationCmd()));

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

        private int GetType(string code, Dao.DataHandler db, out string name)
        {
            switch (code)
            {
                case "91":
                    name = "企事业单位";
                    break;
                case "92":
                    name = "特种行业";
                    break;
                case "93":
                    name = "公共娱乐场所";
                    break;
                case "94":
                    name = "公共场所";
                    break;
                case "95":
                    name = "其他经营服务门店";
                    break;
                default:
                    name = "其他单位";
                    break;
            }

            return CheckParam(db, name, GetCode(), 9);
        }
    }
}

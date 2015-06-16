using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.JWRK
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
            Console.WriteLine("入境人员数据同步开始 ...");
            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        private int CheckPopId(Model t, Dao.DataHandler db)
        {
            var obj = db.ExecuteScalar(t.GetPopIDCmd());
            if (obj == null)
            {
                db.ExecuteNonQuery(t.InsertPopulationBasicInfoCmd());
                obj = db.ExecuteScalar(t.GetPopIDCmd());
            }

            return obj == null ? 0 : int.Parse(string.Format("{0}", obj));
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            Console.WriteLine(t.ToString());
            try
            {
                //校验国籍
                t.CountryID = CheckParam(dbTarget, t.GJDQ, null, 22);

                //校验人员ID
                t.PoID = CheckPopId(t, dbTarget);

                //校验签证类型
                t.CardTypeID = CheckParam(dbTarget, t.ZJZL, null, 21);

                var obj = dbTarget.ExecuteScalar(t.ExistCmd());
                if (obj == null)
                {
                    dbTarget.ExecuteNonQuery(t.InsertCmd());
                    return;
                }

                dbTarget.ExecuteNonQuery(t.updateCmd(obj));
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }   
    }
}

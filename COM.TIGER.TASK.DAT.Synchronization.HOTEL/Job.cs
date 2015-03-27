using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.HOTEL
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
            Console.WriteLine("酒店，宾馆，旅店数据同步开始 ...");

            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            try
            {
                t.AddressID = CheckAddress(t.XXDZ, dbTarget);

                var obj = dbTarget.ExecuteScalar(t.ExistsCmd());
                if (obj != null)
                {
                    dbTarget.ExecuteNonQuery(t.UpdateCmd());
                    return;
                }

                dbTarget.ExecuteNonQuery(t.InsertCmd());
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.GPSTask
{
    /// <summary>
    /// GPS 单兵作战设备任务调度程序
    /// </summary>
    public class Job : BaseSyncHandler<Model>
    {
        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            base.ExecuteJob(context);
        }

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Model.tick = 0;
            Console.WriteLine("GPS 单兵作战数据同步开始 ...");

            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.YJBJ
{
    public class Job:BaseSyncHandler<YJBJ>
    {
        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            base.ExecuteJob(context);
        }

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            try
            {
                YJBJ.tick = 0;
                Console.WriteLine("学校，金融一件报警数据同步开始 ...");
                var maxunm = Getmax(dbTarget);

                if (!string.IsNullOrWhiteSpace(maxunm))
                {
                    if (dataFromCmdString.Contains(" where "))
                        dataFromCmdString = string.Format("{0} and AlarmID > {1}", dataFromCmdString, maxunm);
                    else
                        dataFromCmdString = string.Format("{0} where AlarmID > {1}", dataFromCmdString, maxunm);
                }

                base.Executed(dbFrom, dbTarget, dataFromCmdString);
            }
            catch (Exception e)
            {
                _eventContext = new Task.SyncEventArg(this, Task.SyncEventLevel.EXCEPTION, e);
                EventTigger();
            }
        }

        protected override void Executed(YJBJ t, Dao.DataHandler dbTarget)
        {
            try
            {
                Console.WriteLine(t.ToString());
                //首先校验
                CheckParm(t, dbTarget);

                var ret = dbTarget.ExecuteNonQuery(t.InsertCmd());
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }

        private void CheckParm(YJBJ t, Dao.DataHandler db)
        {
            var id = db.ExecuteScalar(t.GetParam());
            if (id == null)
            {
                db.ExecuteScalar(t.InsertParam());
            }
            id = db.ExecuteScalar(t.GetParam());

            if (id != null)
                t.TypeID = int.Parse(string.Format("{0}", id));
        }

        public string Getmax(Dao.DataHandler db)
        {
            var cmdtext = "select max(Num) from Pgis_YJBJ";
            return string.Format("{0}", db.ExecuteScalar(cmdtext));
        }
    }
}

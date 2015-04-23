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
        /// <summary>
        /// 缓存接收到的 GPS 实体信息
        /// </summary>
        private readonly static List<Model> ENTITIESCACHE = new List<Model>();

        /// <summary>
        /// 缓存已经绑定的警员信息
        /// </summary>
        private readonly static List<Officer> OFFICERCACHE = new List<Officer>();

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Model.tick = 0;
            Console.WriteLine("GPS 单兵作战数据同步开始 ...");

            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            try
            {
                //尚未发生移动
                if (!CheckEntity(t)) return;
                //获取缓存中设备绑定的警员（或者警车）信息
                string officernum = CacheOfficer(t.GpsID, dbTarget);
                //警员信息不存在
                if (string.IsNullOrWhiteSpace(officernum)) return;
                //此处插入数据
                dbTarget.ExecuteNonQuery(t.InsertCmd(officernum));
            }
            catch (Exception e) 
            {
                _eventContext = new Task.SyncEventArg(null, Task.SyncEventLevel.EXCEPTION, e, "获取数据信息错误");
                EventTigger();
                Console.WriteLine("错误：{0}", e.Message); 
            }
            finally { }
        }

        /// <summary>
        /// 检查实体集合中是否已经存在当前实体在缓存中的索引值
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool CheckEntity(Model m)
        {
            int index = ENTITIESCACHE.FindIndex(t => t.GpsID == m.GpsID);
            
            //尚未接收到当前设备的任何信息
            if(index < 0)
            {
                ENTITIESCACHE.Add(m);
                return true;
            }

            //尚未发生移动
            Model x = ENTITIESCACHE[index];
            if (x.X == m.X && x.Y == m.Y) return false;

            //已经发生移动
            ENTITIESCACHE[index] = m;
            return true;
        }

        /// <summary>
        /// 缓存设备绑定的警员信息
        /// </summary>
        /// <param name="gpsid"></param>
        /// <param name="db"></param>
        /// <param name="flag">是否需要从数据</param>
        private string CacheOfficer(string gpsid, Dao.DataHandler db, bool flag = true)
        {
            Officer o = OFFICERCACHE.FirstOrDefault(t => t.DeviceID == gpsid);
            if (o != null)
                return string.IsNullOrWhiteSpace(o.OfficerID) ? o.CarNum : o.OfficerID;

            if(flag)
            {
                string query = "select DeviceID,OfficerID,CarNum from Pgis_GpsDevice";
                List<Officer> officers = db.ExecuteEntities<Officer>(query);
                OFFICERCACHE.AddRange(officers);
                return CacheOfficer(gpsid, db, false);
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.GPSTask
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_GpsDeviceTrack"; }
        }

        /// <summary>
        /// 序号；（样式：3254235）
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// GPS设备号；（样式：a029）
        /// </summary>
        public string GpsID { get; set; }

        /// <summary>
        /// 方向；(样式：305)
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 速度（样式：23）
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        /// 经度 （样式：106.821363）
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 纬度（样式：28.135913）
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 时间（样式：2015-04-21添加到日历 16:32:22）
        /// </summary>
        public DateTime RealTime { get; set; }


        public string InsertCmd(string officernum)
        {
            string[] fields = { "DeviceID", "OfficerNum", "X", "Y", "CurrentTime" };
            string[] vals = { 
                                    ParseString(GpsID),
                                    ParseString(officernum),
                                    string.Format("'{0}'", X),
                                    string.Format("'{0}'", Y),
                                    string.Format("'{0}'", RealTime.ToString("yyyy-MM-dd HH:mm:ss"))
                                };

            return Insert(fields, vals);
        }
    }
}

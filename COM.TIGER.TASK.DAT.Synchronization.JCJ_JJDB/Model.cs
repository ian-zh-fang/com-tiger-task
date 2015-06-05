using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.JCJ_JJDB
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        //var __transformdata = {
        //'Lat':{Intercept:28.1330657075451,Variable1:1.33787091963793E-06,Variable2:-0.0000020215162449493},
        //'Lng':{Intercept:106.789681858985,Variable1:1.65852591313745E-06,Variable2:2.58596774313365E-06},
        //'X':{Intercept:-42366937.9472284,Variable1:296730.555100016,Variable2:379595.202327182},
        //'Y':{Intercept:-14122049.5868561,Variable1:196378.300462638,Variable2:-243454.671333262}
        //};

        private readonly static double Lat_Intercept = 28.1330657075451d;
        private readonly static double Lat_Variable1 = 1.33787091963793E-06d;
        private readonly static double Lat_Variable2 = -0.0000020215162449493d;

        private readonly static double Lng_Intercept = 106.789681858985d;
        private readonly static double Lng_Variable1 = 1.65852591313745E-06d;
        private readonly static double Lng_Variable2 = 2.58596774313365E-06d;

        private readonly static double X_Intercept = -42366937.9472284d;
        private readonly static double X_Variable1 = 296730.555100016d;
        private readonly static double X_Variable2 = 379595.202327182d;

        private readonly static double Y_Intercept = -14122049.5868561d;
        private readonly static double Y_Variable1 = 196378.300462638d;
        private readonly static double Y_Variable2 = -243454.671333262d;

        public static int tick = 0;

        //报警编号 ——必填
        public string JJDBH { get; set; }

        //报警类别（01 表示 110, 02 表示122, 03 表示 119, 04 其它）——必填
        public string BJFSDM { get; set; }

        //报警电话 ——必填
        public string BJDH { get; set; }

        //报警人
        public string BJRXM { get; set; }

        //报警地址 ——必填
        public string SFDD { get; set; }

        //报警坐标 ——必填 
        public string GIS_X { get; set; }

        //报警坐标 ——必填 
        public string GIS_Y { get; set; }

        private DateTime _dTime = DateTime.Now;
        //报警时间（年月日） ——必填
        public DateTime BJSJ { get { return _dTime; } set { _dTime = value; } }

        //行政区划（派出所）
        public string GXDWDM { get; set; }

        public int AdminID { get; set; }

        public string AdminName { get { return GXDWDM; } }

        public int TypeID { get; set; }

        public string TypeName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, JJDBH);
        }

        public string InsertCmd()
        {
            //double x = 0.0d, y = 0.0d;
            //double.TryParse(GIS_X, out x);
            //double.TryParse(GIS_Y, out y);
            //Point p = EPoint2ELatLng(x, y);

            var fields = new string[] 
            {
                "Num,TypeID", "TypeName", "Tel", "AlarmMan", "Location", "X", "Y", "AlarmTime", "Token"
            };
            string str = BJSJ.ToString("yyyy-MM-dd HH:mm:ss");
            var values = new string[] 
            {
                ParseString(JJDBH), TypeID.ToString(), 
                ParseString(TypeName), ParseString(BJDH), 
                ParseString(BJRXM), ParseString(SFDD),
                GIS_X,GIS_Y,
                ParseString(str), ParseString(JJDBH)
            };

            return Insert(fields, values);
        }

        protected override string TABLENAME
        {
            get { return "Pgis_JCJ_JJDB"; }
        }



        /// <summary>
        /// 经纬度坐标转 Alagis 坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override Point ELatLng2EPoint(double x, double y)
        {
            Point point = new Point()
            {
                X = (X_Variable1 * x) + (X_Variable2 * y) + X_Intercept,
                Y = (Y_Variable1 * x) + (Y_Variable2 * y) + Y_Intercept
            };

            return point;
        }

        /// <summary>
        /// Alagis 坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override Point EPoint2ELatLng(double x, double y)
        {
            Point point = new Point()
            {
                X = (Lat_Variable1 * x) + (Lat_Variable2 * y) + Lat_Intercept,
                Y = (Lng_Variable1 * x) + (Lng_Variable2 * y) + Lng_Intercept
            };

            return point;
        }
    }
}

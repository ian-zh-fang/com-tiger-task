using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.JCJ_JJDB
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
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
            double x = 0.0d, y = 0.0d;
            double.TryParse(GIS_X, out x);
            double.TryParse(GIS_Y, out y);
            Point p = ELatLng2EPoint(x, y);

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
                string.Format("'{0}'", p.X),string.Format("'{0}'", p.Y),
                ParseString(str), ParseString(JJDBH)
            };

            return Insert(fields, values);
        }

        protected override string TABLENAME
        {
            get { return "Pgis_JCJ_JJDB"; }
        }
    }
}

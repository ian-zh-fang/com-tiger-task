using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization
{
    public abstract class DatBaseModel
    {
        private readonly static double Lat_Intercept = 28.1295295988415d;
        private readonly static double Lat_Variable1 = 0.0000013749824444732d;
        private readonly static double Lat_Variable2 = -1.96814028627638E-06d;

        private readonly static double Lng_Intercept = 106.779551091342d;
        private readonly static double Lng_Variable1 = 1.65108712853934E-06d;
        private readonly static double Lng_Variable2 = 2.57289803037774E-06d;

        private readonly static double X_Intercept = -41625228.1212446d;
        private readonly static double X_Variable1 = 379077.48425634d;
        private readonly static double X_Variable2 = 289961.487137384d;

        private readonly static double Y_Intercept = -14787167.9659441d;
        private readonly static double Y_Variable1 = -243261.514199763d;
        private readonly static double Y_Variable2 = 202566.881067585d;

        protected abstract string TABLENAME { get; }

        protected virtual string ParseDate(string datestr)
        {
            if (string.IsNullOrWhiteSpace(datestr))
                return "NULL";

            var dt = DateTime.ParseExact(datestr, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            return string.Format("'{0}'", dt.ToString("yyyy-MM-dd"));
        }

        protected virtual string ParseString(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "NULL" : string.Format("'{0}'", str);
        }

        protected virtual string ParseInt(string str)
        {
            int val = 0;
            int.TryParse(str, out val);

            return string.Format("'{0}'", val);
        }

        protected virtual string ParseDouble(string str)
        {
            double val = 0.00d;
            double.TryParse(str, out val);

            return string.Format("'{0}'", val);
        }

        protected virtual string ParseFloat(string str)
        {
            float val = 0.00f;
            float.TryParse(str, out val);

            return string.Format("'{0}'", val);
        }

        protected virtual string ParseDecimal(string str)
        {
            decimal val = 0.00m;
            decimal.TryParse(str, out val);

            return string.Format("'{0}'", val);
        }

        protected virtual string Insert(string[] fields, string[] values)
        {
            return string.Format("insert into {0}({1}) values({2})", TABLENAME, string.Join(",", fields), string.Join(",", values));
        }

        protected virtual string Exist(string tokenfield, params string[] whereExpression)
        {
            return string.Format("select {0} from {1} where {2}", tokenfield, TABLENAME, string.Join(" and ", whereExpression));
        }

        protected virtual string Update(string[] expressions, params string[] whereExpression)
        {
            return string.Format("update {0} set {1} where {2}", TABLENAME, string.Join(",", expressions), string.Join(" and ", whereExpression));
        }

        /// <summary>
        /// 获取指定名称的首字母大写组合
        /// eg. 
        ///     '首字母'->'SZM'
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetFirstLetter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "NULL";

            return string.Format("dbo.fun_getPYFirst('{0}')", str);
        }
        
        /// <summary>
        /// 经纬度坐标转 Alagis 坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Point ELatLng2EPoint(double x, double y)
        {
            Point point = new Point()
            {
                X = (X_Variable1 * x) + (X_Variable2 * y) + X_Intercept,
                Y = (Y_Variable1 * y) + (Y_Variable2 * y) + Y_Intercept
            };

            return point;
        }

        /// <summary>
        /// Alagis 坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Point EPoint2ELatLng(double x, double y)
        {
            Point point = new Point()
            {
                X = (Lat_Variable1 * x) + (Lat_Variable2 * y) + Lat_Intercept,
                Y = (Lng_Variable1 * y) + (Lng_Variable1 * y) + Lng_Intercept
            };

            return point;
        }
    }

    public class Point
    {
        public double X { get; set; }

        public double Y { get; set; }
    }
}

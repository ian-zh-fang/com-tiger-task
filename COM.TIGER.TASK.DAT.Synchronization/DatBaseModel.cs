using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization
{
    public abstract class DatBaseModel
    {
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
    }
}

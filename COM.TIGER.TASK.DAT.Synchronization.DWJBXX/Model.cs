using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.DWJBXX
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_Company"; }
        }
        //单位ID-token
        public string ID { get; set; }
        //单位名称
        public string DWMC { get; set; }
        //单位大类：91-企事业单位；92-特种行业；93-公共娱乐场所；94-公共场所；95-其他经营服务门店；96-其他单位
        public string DWGXLB { get; set; }
        //主营范围
        public string ZYJYFW { get; set; }
        //兼营范围
        public string JYJYFW { get; set; }
        //经营面积
        public string JYMJ { get; set; }
        //营业执照编号
        public string YYZZBH { get; set; }
        //营业执照开始日期
        public string YYZZQSRQ { get; set; }
        //营业执照截至日期
        public string YYZZJZRQ { get; set; }
        //开业日期
        public string KYRQ { get; set; }
        //注册资金
        public string ZCZJ { get; set; }
        //联系电话
        public string LXDH { get; set; }
        //外来务工人数
        public string WLWGRS { get; set; }
        //消防等级
        public string XFDJ { get; set; }
        //法人ID
        public string FRID { get; set; }

        public int TypeID { get; set; }

        public string TypeName { get; set; }

        public string Corporation { get; set; }

        public int FireRating { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, ID);
        }

        public string InsertCmd()
        {
            var fields = new string[] 
            { 
                "TypeID", "TypeName", "Name", "Capital", "Corporation", "Square", "StartTime", "Tel", 
                "LicenceNum", "LicenceStartTime", "LicenceEndTime", "MainFrame", "Concurrently", "MigrantWorks", "FireRating", "token"
            };
            var values = new string[] 
            {
                TypeID.ToString(), ParseString(TypeName), ParseString(DWMC), ParseDecimal(ZCZJ), ParseString(Corporation), ParseFloat(JYMJ), ParseDate(KYRQ), ParseString(LXDH),
                ParseString(YYZZBH), ParseDate(YYZZQSRQ), ParseDate(YYZZJZRQ), ParseString(ZYJYFW), ParseString(JYJYFW), ParseInt(WLWGRS), ParseInt(XFDJ), ParseString(ID)
            };

            return Insert(fields, values);
        }

        public string ExistCmd()
        {
            return Exist("id", string.Format("token = '{0}'", ID));
        }

        public string GetCorporationCmd()
        {
            return string.Format("select name from Pgis_PopulationBasicInfo where token = '{0}'", FRID);
        }

        public string UpdateCmd(object id)
        {
            var expressions = new string[] 
            { 
                string.Format("TypeID = {0}", TypeID),
                string.Format("TypeName = {0}", ParseString(TypeName)),
                string.Format("Name = {0}", ParseString(DWMC)),
                string.Format("Capital = {0}", ParseDecimal(ZCZJ)),
                string.Format("Corporation = {0}", ParseString(Corporation)),
                string.Format("Square = {0}", ParseFloat(JYMJ)),
                string.Format("StartTime = {0}", ParseDate(KYRQ)),
                string.Format("Tel = {0}", ParseString(LXDH)),
                string.Format("LicenceNum = {0}", ParseString(YYZZBH)),
                string.Format("LicenceStartTime = {0}", ParseDate(YYZZQSRQ)),
                string.Format("LicenceEndTime = {0}", ParseDate(YYZZJZRQ)),
                string.Format("MainFrame = {0}", ParseString(ZYJYFW)),
                string.Format("Concurrently = {0}", ParseString(JYJYFW)),
                string.Format("MigrantWorks = {0}", ParseInt(WLWGRS)),
                string.Format("FireRating = {0}", ParseInt(XFDJ)),
            };
            var whereExpression = string.Format("id = {0}", id);

            return Update(expressions, whereExpression);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.HJRK
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_PopulationBasicInfo"; }
        }
        //人员编号
        public string RYBH { get; set; }
        //姓名
        public string XM { get; set; }
        //曾用名
        public string CYM { get; set; }
        //民族CODE
        public string MZ { get; set; }
        //性别，1-男性，2-女性
        public string XB { get; set; }
        //公民身份证号码
        public string GMSFHM { get; set; }
        //身高
        public string SG { get; set; }
        //联系电话
        public string LXDH { get; set; }
        //家庭住址
        public string ZZ { get; set; }
        //户籍编号
        public string HH { get; set; }
        //与户主关系
        public string YHZGX { get; set; }
        //TOKEN
        public string ZJID { get; set; }

        public int SexID { get; set; }

        public string Sex { get; set; }

        public int HRelationID { get; set; }

        public string HRelation { get { return YHZGX; } }

        public int AddrID { get; set; }

        public int LiveTypeID { get { return 1; } }

        public string LiveType { get { return "常住人员"; } }

        public string Nation { get { return "汉族"; } }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, ZJID);
        }

        public string InsertCmd()
        {
            var fields = new string[] 
            {
                "Name", "OtherName", "SexID", "Sex", "LiveTypeID", "LiveType", 
                "Nation", "Stature", "CardNo", "Telephone1", "HouseholdNo",  "HRelationID", "HRelation", 
                "HomeAddrID", "CurrentAddrID", "token"
            };
            var values = new string[] 
            {
                ParseString(XM), ParseString(CYM), SexID.ToString(), ParseString(Sex), LiveTypeID.ToString(), ParseString(LiveType),
                ParseString(Nation), ParseString(SG), ParseString(GMSFHM), ParseString(LXDH), ParseString(HH), HRelationID.ToString(), ParseString(HRelation), 
                AddrID.ToString(), AddrID.ToString(), ParseString(ZJID)
            };

            return Insert(fields, values);
        }

        public string ExistCmd()
        {
            return Exist("id", string.Format("token = '{0}'", ZJID));
        }

        public string UpdateCmd(object id)
        {
            var expressions = new string[] 
            {
                string.Format("Name={0}", ParseString(XM)),
                string.Format("OtherName={0}", ParseString(CYM)),
                string.Format("SexID={0}", SexID),
                string.Format("Sex={0}", ParseString(Sex)),
                string.Format("LiveTypeID={0}", LiveTypeID),
                string.Format("LiveType={0}", ParseString(LiveType)),
                string.Format("Nation={0}", ParseString(Nation)),
                string.Format("Stature={0}", ParseString(SG)),
                string.Format("CardNo={0}", ParseString(GMSFHM)),
                string.Format("Telephone1={0}", ParseString(LXDH)),
                string.Format("HouseholdNo={0}", ParseString(HH)),
                string.Format("HRelationID={0}", HRelationID),
                string.Format("HRelation={0}", ParseString(HRelation)),
                string.Format("HomeAddrID={0}", AddrID),
                string.Format("CurrentAddrID={0}", AddrID)
            };
            var whereExpression = string.Format("id = {0}", id);

            return Update(expressions, whereExpression);
        }
    }
}

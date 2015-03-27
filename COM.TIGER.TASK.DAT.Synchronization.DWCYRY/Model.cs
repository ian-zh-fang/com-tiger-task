using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.DWCYRY
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_Employee"; }
        }

        public string ID { get; set; }

        public string DWID { get; set; }

        public string ZWXM { get; set; }

        public string GMSFHM { get; set; }

        public string XB { get; set; }

        public string HJDXZ { get; set; }

        public string GZZZ1 { get; set; }

        public string GZXZ { get; set; }

        public string CYRQ { get; set; }

        public string ZXRQ { get; set; }

        public string LXDH { get; set; }
        //单位ID
        public int OrganID { get; set; }
        //单位类型，普通单位，或者酒店
        public int OrganTypeID { get; set; }
        //单位类型名称
        public string OrganTypeName { get; set; }

        public int CardTypeID { get; set; }

        public string CardTypeName { get; set; }

        public int GenderID { get; set; }

        public string GenderDesc { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, ID);
        }

        public string InsertCmd()
        {
            var fields = new string[] 
            { 
                "OrganID", 
                "OrganTypeID", 
                "OrganTypeName", 
                "Name", 
                "CardTypeID", 
                "CardTypeName", 
                "IdentityCardNum", 
                "GenderID", 
                "GenderDesc", 
                "Address", 
                "Tel", 
                "EntryTime", 
                "QuitTime", 
                "IsInService", 
                "token" 
            };
            var values = new string[] 
            { 
                OrganID.ToString(), 
                OrganTypeID.ToString(), 
                string.Format("'{0}'", OrganTypeName), 
                ParseString(ZWXM),
                CardTypeID.ToString(),
                string.Format("'{0}'", CardTypeName),
                ParseString(GMSFHM),
                GenderID.ToString(),
                string.Format("'{0}'", GenderDesc),
                ParseString(HJDXZ),
                ParseString(LXDH),
                ParseDate(CYRQ),
                ParseDate(ZXRQ),
                string.Format("{0}", string.IsNullOrWhiteSpace(ZXRQ) ? 0 : 1),
                ParseString(ID)
            };

            return Insert(fields, values);
        }

        public string UpdateCmd(object id)
        {
            var expressions = new string[] 
            {
                string.Format("OrganID={0}", OrganID),
                string.Format("OrganTypeID={0}", OrganTypeID),
                string.Format("OrganTypeName='{0}'", OrganTypeName),
                string.Format("Name={0}", ParseString(ZWXM)),
                string.Format("CardTypeID={0}", CardTypeID),
                string.Format("CardTypeName='{0}'", CardTypeName),
                string.Format("IdentityCardNum={0}", ParseString(GMSFHM)),
                string.Format("GenderID={0}", GenderID),
                string.Format("GenderDesc='{0}'", GenderDesc),
                string.Format("Address={0}", ParseString(HJDXZ)),
                string.Format("Tel={0}", ParseString(LXDH)),
                string.Format("EntryTime={0}", ParseDate(CYRQ)),
                string.Format("QuitTime={0}", ParseDate(ZXRQ)),
                string.Format("IsInService={0}", string.IsNullOrWhiteSpace(ZXRQ) ? 1 : 0)
            };
            var whereExpression = string.Format("id = '{0}'", id);

            return Update(expressions, whereExpression);
        }

        public string ExistCmd()
        {
            return Exist("id", string.Format("token = '{0}'", ID));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.YJBJ
{
    public class YJBJ : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        protected override string TABLENAME
        {
            get { return "Pgis_YJBJ"; }
        }

        public static int tick = 0;

        public int AlarmID { get; set; }

        public DateTime AlarmTime { get; set; }

        public string AlarmType { get; set; }

        public string AlarmDetail { get; set; }

        public string ACode { get; set; }

        public string ClientName { get; set; }

        public string FBoxTel { get; set; }

        public string FAddr { get; set; }

        public int TypeID { get; set; }

        public string GetParam()
        {
            return string.Format("select id from PGis_Param where name = '{0}'", AlarmType);
        }

        public string InsertParam()
        {
            return string.Format("insert into PGis_Param(pid, code, name, disabled) values(5, '{0}', '{1}', 1)", ACode, AlarmType);
        }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, AlarmID);
        }

        public string InsertCmd()
        {
            return string.Format("insert into {0}(Num,TypeID, TypeName, Tel, AlarmMan, Location, AlarmTime, Token) values('{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{1}')",
                TABLENAME, AlarmID, TypeID, AlarmType, FBoxTel, ClientName, FAddr, AlarmTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.AJJBXX
{
    public class AJJBXX:DatBaseModel
    {
        protected override string TABLENAME
        {
            get { return "Pgis_AJJBXX"; }
        }

        public static int tick = 0;

        public string AJBH { get; set; }

        public string XM { get; set; }

        public string SFZH { get; set; }

        public string SFSWZT { get; set; }

        public string CH { get; set; }

        public string XZDXXDZ { get; set; }

        public string SFPZXJ { get; set; }

        public string BZ { get; set; }

        public string LRSJ { get; set; }

        public string RYBH { get; set; }

        public override string ToString()
        {
            return string.Format("{2} -- {0}{1}", RYBH, LRSJ, ++tick);
        }

        public string InsertCmd()
        {
            string cardno, xm, ajbh, ch, addr, bz, token;
            int isdrup, ispursuit, sfpzxj;
            Setvalues(out cardno, out xm, out ajbh, out isdrup, out ispursuit, out ch, out addr, out sfpzxj, out bz, out token);

            return string.Format("insert into {0}(cardno, ajbh, xm, isdrup, ISPURSUIT, ALIAS, CURRENTADDr, ISARREST, PROOF, TOKEN) values({1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                TABLENAME, cardno, ajbh, xm, isdrup, ispursuit, ch, addr, sfpzxj, bz, token);
        }

        public string ExistCmd()
        {
            return string.Format("select id from {0} where token = '{1}{2}'", TABLENAME, RYBH, LRSJ);
        }

        public string UpdateCmd(object id)
        {
            string cardno, xm, ajbh, ch, addr, bz, token;
            int isdrup, ispursuit, sfpzxj;
            Setvalues(out cardno, out xm, out ajbh, out isdrup, out ispursuit, out ch, out addr, out sfpzxj, out bz, out token);

            return string.Format("update {0} set cardno={1}, ajbh={2}, xm={3}, isdrup={4}, ISPURSUIT={5}, ALIAS={6}, CURRENTADDr={7}, ISARREST={8}, PROOF={9}, TOKEN={10} where id = {11}",
                TABLENAME, cardno, ajbh, xm, isdrup, ispursuit, ch, addr, sfpzxj, bz, token, id);
        }

        private void Setvalues(out string cardno, out string xm, out string ajbh, out int isdrup, out int ispursuit, out string ch, out string addr, out int sfpzxj, out string bz, out string token)
        {
            cardno = string.IsNullOrWhiteSpace(SFZH) ? string.Format("NULL") : string.Format("'{0}'", SFZH);
            xm = string.IsNullOrWhiteSpace(XM) ? string.Format("NULL") : string.Format("'{0}'", XM);
            ajbh = string.IsNullOrWhiteSpace(AJBH) ? string.Format("NULL") : string.Format("'{0}'", AJBH);
            isdrup = 0;

            ispursuit = 0;
            int.TryParse(SFSWZT, out ispursuit);

            ch = string.IsNullOrWhiteSpace(CH) ? string.Format("NULL") : string.Format("'{0}'", CH);
            addr = string.IsNullOrWhiteSpace(XZDXXDZ) ? string.Format("NULL") : string.Format("'{0}'", XZDXXDZ);

            sfpzxj = 0;
            int.TryParse(SFPZXJ, out sfpzxj);

            bz = string.IsNullOrWhiteSpace(BZ) ? string.Format("NULL") : string.Format("'{0}'", BZ);
            token = string.Format("'{0}{1}'", RYBH, LRSJ);
        }
    }
}

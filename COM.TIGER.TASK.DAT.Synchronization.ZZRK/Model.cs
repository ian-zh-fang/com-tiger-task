using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.ZZRK
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        protected override string TABLENAME
        {
            get { return "Pgis_TemporaryPopulation"; }
        }

        public static int tick = 0;

        //公民身份证号码
        public string GMSFHM { get; set; }
        //房主姓名
        public string FZXM { get; set; }
        //来本地日期
        public string LBDRQ { get; set; }
        //暂住事由
        public string ZZSY { get; set; }
        //暂住证编号
        public string ZZZBH { get; set; }
        //地址
        public string ZZDXZ { get; set; }
        //标识
        public string ID { get; set; }
        //人员编号
        public int PopID { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, ID);
        }

        public string InsertCmd()
        {
            string sfhm, fzxm, lbdrq, zzsy, zzzbh, addr, token;
            SetValue(out sfhm, out fzxm, out lbdrq, out zzsy, out zzzbh, out addr, out token);

            return string.Format("insert into {0}(LandlordName,TP_Date,TP_Reason,ResidenceNo,PoID,Addr, token) values({1}, {2}, {3}, {4}, {5}, {6}, {7})",
                TABLENAME, fzxm, lbdrq, zzsy, zzzbh, PopID, addr, token);
        }

        public string UpdateCmd(object id)
        {
            string sfhm, fzxm, lbdrq, zzsy, zzzbh, addr, token;
            SetValue(out sfhm, out fzxm, out lbdrq, out zzsy, out zzzbh, out addr, out token);

            return string.Format("update {0} set LandlordName = {1},  TP_Date = {2},  TP_Reason = {3},  ResidenceNo = {4},  PoID = {5},  Addr = {6} where tp_id = '{7}'",
                TABLENAME, fzxm, lbdrq, zzsy, zzzbh, PopID, addr, id);
        }

        public string ExistCmd()
        {
            return string.Format("select tp_id from {0} where token = '{1}'", TABLENAME, ID);
        }

        public string GetPopIDCmd()
        {
            return string.Format("select id from Pgis_PopulationBasicInfo where cardno = '{0}'", GMSFHM);
        }

        public string UpdateZZCmd(int id)
        {
            return string.Format("update Pgis_PopulationBasicInfo set LiveTypeID = 2, LiveType = '暂住人员' where id = {0}", id);
        }

        private void SetValue(out string sfhm, out string fzxm, out string lbdrq, out string zzsy, out string zzzbh, out string addr, out string token)
        {

            sfhm = string.IsNullOrWhiteSpace(GMSFHM) ? "NULL" : string.Format("'{0}'", GMSFHM);
            fzxm = string.IsNullOrWhiteSpace(FZXM) ? "NULL" : string.Format("'{0}'", FZXM);
            zzsy = string.IsNullOrWhiteSpace(ZZSY) ? "NULL" : string.Format("'{0}'", ZZSY);
            zzzbh = string.IsNullOrWhiteSpace(ZZZBH) ? "NULL" : string.Format("'{0}'", ZZZBH);
            addr = string.IsNullOrWhiteSpace(ZZDXZ) ? "无" : string.Format("'{0}'", ZZDXZ);
            token = string.IsNullOrWhiteSpace(ID) ? "NULL" : string.Format("'{0}'", ID);

            lbdrq = null;
            if (!string.IsNullOrWhiteSpace(LBDRQ))
            {
                var dt = DateTime.ParseExact(LBDRQ, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                lbdrq = dt.ToString("yyyy-MM-dd");
            }
        }
    }
}

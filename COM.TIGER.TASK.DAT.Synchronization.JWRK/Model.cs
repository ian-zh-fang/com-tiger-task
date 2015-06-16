using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.JWRK
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        public string YWBH { get; set; }

        public string YWX { get; set; }

        public string YWM { get; set; }

        public string GJDQ { get; set; }

        public string ZJZL { get; set; }

        public string ZJHM { get; set; }

        public string ZJYXQ { get; set; }

        public string QZHM { get; set; }

        public string QZYXQ { get; set; }

        public string RJRQ { get; set; }

        public string DWMC { get; set; }

        public string SJHM { get; set; }

        public int PoID { get; set; }

        public int CountryID { get; set; }

        public int CardTypeID { get; set; }

        public string InsertCmd()
        {
            string fname, lname, country, cardtype, cardno, validate, visanum, visadate, entrydate, receivepersion, tel;
            SetValues(out fname, out lname, out country, out cardtype, out cardno, out validate, out visanum, out visadate, out entrydate, out receivepersion, out tel);

            var fields = new string[] { "PoID", "FirstName", "LastName", "CountryID", "Country", "CardTypeID", "CardTypeName", "CardNo", "ValidityDate", "VisaNoAndValidity", "StayValidityDate", "EntryDate", "ReceivePerson", "Phone", "Token" };
            var values = new string[] { PoID.ToString(), fname, lname, CountryID.ToString(), country, CardTypeID.ToString(), cardtype, cardno, validate, visanum, visadate, entrydate, receivepersion, tel, ParseString(YWBH) };

            return Insert(fields, values);
        }

        public string InsertPopulationBasicInfoCmd()
        {
            string fname, lname, country, cardtype, cardno, validate, visanum, visadate, entrydate, receivepersion, tel;
            SetValues(out fname, out lname, out country, out cardtype, out cardno, out validate, out visanum, out visadate, out entrydate, out receivepersion, out tel);

            var fields = new string[] 
            {
                "Name", "LiveTypeID", "LiveType", "CardNo", "Telephone1", "token"
            };
            var values = new string[] 
            {
                ParseString(string.Format("{0}_{1}",fname, lname)), "3", "境外人口", cardno, tel, ParseString(YWBH)
            };

            return Insert(fields, values);
        }

        public string GetPopulationIdCmd()
        {
            string fname, lname, country, cardtype, cardno, validate, visanum, visadate, entrydate, receivepersion, tel;
            SetValues(out fname, out lname, out country, out cardtype, out cardno, out validate, out visanum, out visadate, out entrydate, out receivepersion, out tel);

            return string.Format("select id from Pgis_PopulationBasicInfo where CardNo = '{0}'", cardno);
        }

        public string ExistCmd()
        {
            var tokenfield = "AP_ID";
            var whereExpression = string.Format("token = '{0}'", YWBH);
            return Exist(tokenfield, whereExpression);
        }

        public string updateCmd(object id)
        {
            string fname, lname, country, cardtype, cardno, validate, visanum, visadate, entrydate, receivepersion, tel;
            SetValues(out fname, out lname, out country, out cardtype, out cardno, out validate, out visanum, out visadate, out entrydate, out receivepersion, out tel);

            var expressions = new string[] 
            {
                string.Format("PoID = {0}", PoID),
                string.Format("FirstName = {0}", fname),
                string.Format("LastName = {0}", lname),
                string.Format("CountryID = {0}", CountryID),
                string.Format("Country = {0}", country),
                string.Format("CardTypeID = {0}", CardTypeID),
                string.Format("CardTypeName = {0}", cardtype),
                string.Format("CardNo = {0}", cardno),
                string.Format("ValidityDate = {0}", validate),
                string.Format("VisaNoAndValidity = {0}", visanum),
                string.Format("StayValidityDate = {0}", visadate),
                string.Format("EntryDate = {0}", entrydate),
                string.Format("ReceivePerson = {0}", receivepersion),
                string.Format("Phone = {0}", tel),
                string.Format("Token = {0}", ParseString(YWBH)),
            };
            var whereExpression = string.Format("AP_ID = '{0}'", id);
            return Update(expressions, whereExpression);
        }

        public string GetPopIDCmd()
        {
            return string.Format("select id from Pgis_PopulationBasicInfo where cardno = '{0}'", ZJHM);
        }

        public string UpdateRJCmd(int id)
        {
            return string.Format("update Pgis_PopulationBasicInfo set LiveTypeID = 3, LiveType = '入境人员' where id = {0}", id);
        }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, YWBH);
        }

        private void SetValues(out string fname, out string lname, out string country, out string cardtype, out string cardno,
            out string validate, out string visanum, out string visadate, out string entrydate, out string receivepersion, out string tel)
        {
            fname = string.IsNullOrWhiteSpace(YWM) ? "NULL" : string.Format("'{0}'", YWM);
            lname = string.IsNullOrWhiteSpace(YWX) ? "NULL" : string.Format("'{0}'", YWX);
            country = string.IsNullOrWhiteSpace(GJDQ) ? "NULL" : string.Format("'{0}'", GJDQ);
            cardtype = string.IsNullOrWhiteSpace(ZJZL) ? "NULL" : string.Format("'{0}'", ZJZL);
            cardno = string.IsNullOrWhiteSpace(ZJHM) ? "NULL" : string.Format("'{0}'", ZJHM);
            validate = ParseDate(ZJYXQ);
            visanum = string.IsNullOrWhiteSpace(QZHM) ? "NULL" : string.Format("'{0}'", QZHM);
            visadate = ParseDate(QZYXQ);
            entrydate = ParseDate(RJRQ);
            receivepersion = string.IsNullOrWhiteSpace(DWMC) ? "NULL" : string.Format("'{0}'", DWMC);
            tel = string.IsNullOrWhiteSpace(SJHM) ? "NULL" : string.Format("'{0}'", YWM);
        }

        protected override string TABLENAME
        {
            get { return "Pgis_AbroadPerson"; }
        }
    }
}

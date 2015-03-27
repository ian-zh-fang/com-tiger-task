using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.HOTELSTAY
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_HotelStay"; }
        }

        /// <summary>
        /// 标识符
        /// </summary>
        public string ZJID { get; set; }

        /// <summary>
        /// 旅馆编号
        /// </summary>
        public string LGBM { get; set; }

        /// <summary>
        /// 中文姓名
        /// </summary>
        public string XM { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string XB { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }

        /// <summary>
        /// 入住时间
        /// </summary>
        public string RZSJ { get; set; }

        /// <summary>
        /// 离开时间
        /// </summary>
        public string TFSJ { get; set; }

        /// <summary>
        /// 入住房号
        /// </summary>
        public string RZFH { get; set; }

        /// <summary>
        /// 中文姓名
        /// </summary>
        public string Name { get { return XM; } }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 性别描述
        /// </summary>
        public string GenderDesc { get; set; }

        /// <summary>
        /// 证件类型ID
        /// </summary>
        public int CredentialsID { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string Credentials { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string CredentialsNum { get { return ZJHM; } }

        /// <summary>
        /// 旅店ID
        /// </summary>
        public int HotelID { get; set; }

        /// <summary>
        /// 旅店名称
        /// </summary>
        public string HotelName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, ZJID);
        }

        public string InsertCmd()
        {
            var fields = new string[] 
            {
               @"[Name]
               ,[Gender]
               ,[GenderDesc]
               ,[CredentialsID]
               ,[Credentials]
               ,[CredentialsNum]
               ,[PutinTime]
               ,[GetoutTime]
               ,[PutinRoomNum]
               ,[HotelID]
               ,[HotelName]
               ,[TOKEN]"
            };
            var values = new string[] 
            {
                ParseString(Name),
                Gender.ToString(),
                ParseString(GenderDesc),
                CredentialsID.ToString(),
                ParseString(Credentials),
                ParseString(CredentialsNum),
                ParseDate(RZSJ),
                ParseDate(TFSJ),
                ParseString(RZFH),
                HotelID.ToString(),
                ParseString(HotelName),
                ParseString(ZJID)
            };

            return Insert(fields, values);
        }

        public string UpdateCmd()
        {
            var expressions = new string[] 
            {
               string.Format("[Name] = {0} ", ParseString(Name))
              ,string.Format("[Gender] = {0} ", Gender)
              ,string.Format("[GenderDesc] = {0} ", ParseString(GenderDesc))
              ,string.Format("[CredentialsID] = {0} ", CredentialsID)
              ,string.Format("[Credentials] = {0} ", ParseString(Credentials))
              ,string.Format("[CredentialsNum] = {0} ", ParseString(CredentialsNum))
              ,string.Format("[PutinTime] = {0} ", ParseDate(RZSJ))
              ,string.Format("[GetoutTime] = {0} ", ParseDate(TFSJ))
              ,string.Format("[PutinRoomNum] = {0} ", ParseString(RZFH))
              ,string.Format("[HotelID] = {0} ", HotelID)
              ,string.Format("[HotelName] = {0} ", ParseString(HotelName))
            };
            var whereExpressions = new string[] 
            {
                string.Format(" token = {0} ", ParseString(ZJID))
            };

            return Update(expressions, whereExpressions);
        }

        public string ExistsCmd()
        {
            return Exist("ID", string.Format(" token = {0} ", ParseString(ZJID)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.HOTEL
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return "Pgis_Hotel"; }
        }

        /// <summary>
        /// 旅馆编码
        /// </summary>
        public string QYBM { get; set; }

        /// <summary>
        /// 旅馆名称
        /// </summary>
        public string QYMC { get; set; }

        /// <summary>
        /// 旅馆地址
        /// </summary>
        public string XXDZ { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LXDH { get; set; }

        /// <summary>
        /// 保安部电话
        /// </summary>
        public string BABDH { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        public string FDDBR { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string FZR { get; set; }

        /// <summary>
        /// 治安负责人
        /// </summary>
        public string ZAZRR { get; set; }

        /// <summary>
        /// 房间数
        /// </summary>
        public string FJS { get; set; }

        /// <summary>
        /// 床位数
        /// </summary>
        public string CWS { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        public string XJ { get; set; }

        /// <summary>
        /// 地址ID
        /// </summary>
        public int AddressID { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", ++tick, QYBM);
        }

        public string InsertCmd()
        {
            var fields = new string[] 
            {
               "[Name]"
               ,"[AddressID]"
               ,"[Tel]"
               ,"[SafetyTel]"
               ,"[Corporation]"
               ,"[Official]"
               ,"[PoliceOffcial]"
               ,"[RoomCount]"
               ,"[BedCount]"
               ,"[StarLevel]"
               ,"[Disable]"
               ,"[TOKEN]"
            };
            var values = new string[] 
            {
                ParseString(QYMC),
                AddressID.ToString(),
                ParseString(LXDH),
                ParseString(BABDH),
                ParseString(FDDBR),
                ParseString(FZR),
                ParseString(ZAZRR),
                ParseInt(FJS),
                ParseInt(CWS),
                ParseInt(XJ),
                "'1'",
                ParseString(QYBM)
            };

            return Insert(fields, values);
        }

        public string UpdateCmd()
        {
            var expressions = new string[] 
            {
               string.Format("[Name] = {0}", ParseString(QYMC))
              ,string.Format("[AddressID] = '{0}'", AddressID)
              ,string.Format("[Tel] = {0}", ParseString(LXDH))
              ,string.Format("[SafetyTel] = {0}", ParseString(BABDH))
              ,string.Format("[Corporation] = {0}", ParseString(FDDBR))
              ,string.Format("[Official] = {0}", ParseString(FZR))
              ,string.Format("[PoliceOffcial] = {0}", ParseString(ZAZRR))
              ,string.Format("[RoomCount] = {0}", ParseInt(FJS))
              ,string.Format("[BedCount] = {0}", ParseInt(CWS))
              ,string.Format("[StarLevel] = {0}", ParseInt(XJ))
              ,string.Format("[TOKEN] = {0}", ParseString(QYBM))
            };
            var whereExpressions = new string[] 
            {
                string.Format(" TOKEN = {0} ", ParseString(QYBM))
            };

            return Update(expressions, whereExpressions);
        }

        public string ExistsCmd()
        {
            return Exist("ID", string.Format(" TOKEN = {0} ", ParseString(QYBM)));
        }
    }
}

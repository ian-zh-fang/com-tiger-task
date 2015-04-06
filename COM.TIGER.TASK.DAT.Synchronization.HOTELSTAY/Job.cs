using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.HOTELSTAY
{
    public class Job:BaseSyncHandler<Model>
    {
        protected override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            base.ExecuteJob(context);
        }

        protected override void Executed(Dao.DataHandler dbFrom, Dao.DataHandler dbTarget, string dataFromCmdString)
        {
            Model.tick = 0;
            Console.WriteLine("酒店住宿人员数据同步开始 ...");

            base.Executed(dbFrom, dbTarget, dataFromCmdString);
        }

        protected override void Executed(Model t, Dao.DataHandler dbTarget)
        {
            try
            {
                var sexname = "";
                t.Gender = GetSex(t.XB, dbTarget, out sexname);
                t.GenderDesc = sexname;

                var typename = "";
                t.CredentialsID = GetCredentials(dbTarget, t.ZJLX, out typename);
                t.Credentials = typename;

                var hotelname = "";
                t.HotelID = GetHotel(t.LGBM, dbTarget, out hotelname);
                t.HotelName = hotelname;

                var obj = dbTarget.ExecuteScalar(t.ExistsCmd());
                if (obj == null)
                {
                    dbTarget.ExecuteNonQuery(t.InsertCmd());
                    return;
                }

                dbTarget.ExecuteNonQuery(t.UpdateCmd());
            }
            catch (Exception e) { Console.WriteLine("错误：{0}", e.Message); }
        }

        private int GetHotel(string bh, Dao.DataHandler db, out string name)
        {
            name = null;
            if (string.IsNullOrWhiteSpace(bh))
                return 0;

            var str = string.Format("select id as HotelID, name as HotelName from Pgis_Hotel where token = '{0}'", bh);
            var t = GetEntity(db, str);

            name = (t == null) ? null : t.HotelName;
            return (t == null) ? 0 : t.HotelID;
        }

        private int GetCredentials(Dao.DataHandler db, string code, out string name)
        {
            switch (code)
            {
                default:
                    name = "身份证";
                    break;
            }

            return CheckParam(db, name, GetCode(), 16);
        }
    }
}

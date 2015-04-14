using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEntLib = Microsoft.Practices.EnterpriseLibrary.Data;

namespace COM.TIGER.TASK.DAT.Synchronization.Dao
{
    public class OracleHandler : DataHandler
    {
        private static readonly string CONNECTIONSTRINGFORMAT = null;

        static OracleHandler()
        {
            CONNECTIONSTRINGFORMAT = "Data Source={0};User ID={1};Password={2}";
        }

        public OracleHandler(string connectionString)
        {
            dbConnectionString = connectionString;
            
        }

        public OracleHandler(string serviceName, string userid, string pwd)
        {
            dbConnectionString = string.Format(CONNECTIONSTRINGFORMAT, serviceName, userid, pwd);
        }

        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database CreateDB()
        {
            DEntLib.Database db = new DEntLib.Oracle.OracleDatabase(dbConnectionString);
            return db;
        }
    }
}

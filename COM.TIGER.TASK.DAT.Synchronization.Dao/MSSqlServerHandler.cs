using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEntLib = Microsoft.Practices.EnterpriseLibrary.Data;

namespace COM.TIGER.TASK.DAT.Synchronization.Dao
{
    public class MSSqlServerHandler : DataHandler
    {
        private static readonly string CONNECTIONSTRINGFORMAT = null;

        static MSSqlServerHandler()
        {
            CONNECTIONSTRINGFORMAT = "Server={0};database={1};user id={2};password={3};";
        }

        public MSSqlServerHandler(string connectionString)
        {
            dbConnectionString = connectionString;
        }

        public MSSqlServerHandler(string dbServer, string dbName, string userId, string pwd)
        {
            dbConnectionString = string.Format(CONNECTIONSTRINGFORMAT, dbServer, dbName, userId, pwd);
        }

        protected override Microsoft.Practices.EnterpriseLibrary.Data.Database CreateDB()
        {
            DEntLib.Database db = new DEntLib.Sql.SqlDatabase(dbConnectionString);
            return db;
        }
    }
}

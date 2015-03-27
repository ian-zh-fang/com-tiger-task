using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEntLib = Microsoft.Practices.EnterpriseLibrary.Data;

namespace COM.TIGER.TASK.DAT.Synchronization.Dao
{
    public abstract class DataHandler
    {
        protected string dbConnectionString = null;
        private DEntLib.Database dbConnection = null;

        protected abstract DEntLib.Database CreateDB();

        protected virtual System.Data.Common.DbCommand CreateCommand(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException();

            dbConnection = CreateDB();
            System.Data.Common.DbCommand cmd = dbConnection.GetSqlStringCommand(commandText);
            return cmd;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public System.Data.IDataReader ExecuteReader(string commandText)
        {
            var cmd = CreateCommand(commandText);
            return dbConnection.ExecuteReader(cmd);
        }

        /// <summary>
        /// 执行命令，返回受影响的行数
        /// </summary>
        /// <param name="commandText">T-SQL命令</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            var cmd = CreateCommand(commandText);
            return dbConnection.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 查询语句，并返回首行首列数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            var cmd = CreateCommand(commandText);
            return dbConnection.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 查询受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int Count(string commandText)
        {
            var cmd = CreateCommand(commandText);
            var obj = dbConnection.ExecuteScalar(cmd);

            int count = 0;
            int.TryParse(string.Format("{0}", obj), out count);

            return count;
        }
    }
}

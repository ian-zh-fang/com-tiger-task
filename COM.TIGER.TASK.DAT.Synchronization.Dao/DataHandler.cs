using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEntLib = Microsoft.Practices.EnterpriseLibrary.Data;
using System.Reflection;

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

        public T ExecuteEntity<T>(string cmdText) where T : class, new()
        {
            T t = null;
            using(System.Data.IDataReader reader = ExecuteReader(cmdText))
            {
                if (reader.Read())
                {
                    t = new T();
                    Type type = typeof(T);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string colname = reader.GetName(i);
                        PropertyInfo p = type.GetProperty(colname);
                        if (p == null) continue;

                        object val = reader.GetValue(i);
                        if (val == DBNull.Value) continue;

                        p.SetValue(t, val, null);
                    }
                }
                reader.Close();
            }

            return t;
        }

        public List<T> ExecuteEntities<T>(string cmdText) where T:class, new()
        {
            List<T> list = new List<T>();
            using (System.Data.IDataReader reader = ExecuteReader(cmdText))
            {
                Type type = typeof(T);
                while (reader.Read())
                {
                    T t = new T();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string colname = reader.GetName(i);
                        PropertyInfo p = type.GetProperty(colname);
                        if (p == null) continue;

                        object val = reader.GetValue(i);
                        if (val == DBNull.Value) continue;

                        p.SetValue(t, val, null);
                    }
                    list.Add(t);
                }
            }

            return list;
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

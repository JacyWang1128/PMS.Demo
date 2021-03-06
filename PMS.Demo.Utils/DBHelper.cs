using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PMS.Demo.Utils
{
    //用于Sql Server的帮助类
    public class SqlClientHelper : DBHelper
    {
        public SqlClientHelper(string connectionString) : base(connectionString)
        {
            this.DbProviderFactory = SqlClientFactory.Instance;
        }
        public override DbProviderFactory DbProviderFactory { get; }
    }

    //用于SQLite的帮助类
    public class SQLiteHelper : DBHelper
    {
        public SQLiteHelper(string connectionString) : base(connectionString)
        {
            DbProviderFactory = SQLiteFactory.Instance;
        }
        public override DbProviderFactory DbProviderFactory { get; }
    }

    public abstract class DBHelper
    {
        public DBHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract DbProviderFactory DbProviderFactory { get; }

        public string ConnectionString { get; }

        public void CreateDatabase(string path)
        {
            SQLiteConnection.CreateFile(path);
        }
        public bool TableExist(string tablename)
        {
            try
            {
                ExecuteNonQuery($"SELECT 1 FROM {tablename}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TestConnection()
        {
            using (DbConnection connection = DbProviderFactory.CreateConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

        private void ThrowExceptionIfLengthNotEqual(string[] sqls, params DbParameter[][] parameters)
        {
            if (parameters.GetLength(0) != 0 && sqls.Length != parameters.GetLength(0)) throw new ArgumentException($"一维数组{nameof(sqls)}的长度与二维数组{nameof(parameters)}长度的第一维长度不一致");
        }

        private T[] Execute<T>(string[] sqls, CommandType commandType = CommandType.Text, ExecuteMode executeMode = ExecuteMode.NonQuery, params DbParameter[][] parameters)
        {
            ThrowExceptionIfLengthNotEqual(sqls, parameters);
            if (executeMode == ExecuteMode.NonQuery && typeof(T) != typeof(int)) throw new InvalidCastException("使用NonQuery模式时，必须将类型T指定为int");
            using (DbConnection connection = DbProviderFactory.CreateConnection())
            {
                using (DbCommand command = DbProviderFactory.CreateCommand())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = commandType;
                    DbTransaction transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        List<T> resultList = new List<T>();
                        for (int i = 0; i < sqls.Length; i++)
                        {
                            command.CommandText = sqls[i];
                            if (parameters.GetLength(0) != 0)
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddRange(parameters[i]);
                            }
                            object result = null;
                            switch (executeMode)
                            {
                                case ExecuteMode.NonQuery:
                                    result = command.ExecuteNonQuery(); break;
                                case ExecuteMode.Scalar:
                                    result = command.ExecuteScalar(); break;
                                default: throw new NotImplementedException();
                            }
                            resultList.Add((T)Convert.ChangeType(result, typeof(T)));
                        }
                        transaction.Commit();
                        return resultList.ToArray();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] parameter) => ExecuteNonQuery(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public int[] ExecuteNonQuery(string[] sqls, params DbParameter[][] parameters) => Execute<int>(sqls, CommandType.Text, ExecuteMode.NonQuery, parameters);

        public int ExecuteNonQueryWithProc(string sql, params DbParameter[] parameter) => ExecuteNonQueryWithProc(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public int[] ExecuteNonQueryWithProc(string[] sqls, params DbParameter[][] parameters) => Execute<int>(sqls, CommandType.StoredProcedure, ExecuteMode.NonQuery, parameters);

        public T ExecuteScalar<T>(string sql, params DbParameter[] parameter) => ExecuteNonQuery<T>(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public T[] ExecuteNonQuery<T>(string[] sqls, params DbParameter[][] parameters) => Execute<T>(sqls, CommandType.Text, ExecuteMode.Scalar, parameters);

        public T ExecuteScalarWithProc<T>(string sql, params DbParameter[] parameter) => ExecuteNonQuery<T>(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public T[] ExecuteNonQueryWithProc<T>(string[] sqls, params DbParameter[][] parameters) => Execute<T>(sqls, CommandType.StoredProcedure, ExecuteMode.Scalar, parameters);

        enum ExecuteMode
        {
            NonQuery, Scalar
        }

        private DataTable[] Fill(string[] selectSqls, CommandType commandType = CommandType.Text, params DbParameter[][] parameters)
        {
            ThrowExceptionIfLengthNotEqual(selectSqls, parameters);
            using (DbConnection connection = DbProviderFactory.CreateConnection())
            using (DbDataAdapter adapter = DbProviderFactory.CreateDataAdapter())
            using (DbCommand command = DbProviderFactory.CreateCommand())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command.Connection = connection;
                command.CommandType = commandType;
                adapter.SelectCommand = command;
                List<DataTable> resultList = new List<DataTable>();
                for (int i = 0; i < selectSqls.Length; i++)
                {
                    command.CommandText = selectSqls[i];
                    if (parameters.GetLength(0) != 0)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddRange(parameters[i]);
                    }
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    resultList.Add(table);
                }
                return resultList.ToArray();
            }
        }

        public DataTable Fill(string selectSql, params DbParameter[] parameter) => Fill(new string[] { selectSql }, new DbParameter[][] { parameter })[0];

        public DataTable[] Fill(string[] selectSqls, params DbParameter[][] parameters) => Fill(selectSqls, CommandType.Text, parameters);

        public DataTable FillWithProc(string selectSql, params DbParameter[] parameter) => FillWithProc(new string[] { selectSql }, new DbParameter[][] { parameter })[0];

        public DataTable[] FillWithProc(string[] selectSqls, params DbParameter[][] parameters) => Fill(selectSqls, CommandType.StoredProcedure, parameters);
    }
}

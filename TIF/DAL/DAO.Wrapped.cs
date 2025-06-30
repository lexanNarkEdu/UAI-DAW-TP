using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DAL
{
    public partial class DAO
    {
        private SqlCommand CreateCommand(string spNameOrText, List<SqlParameter> sqlParameters = null, CommandType? commandType = null)
        {
            SqlCommand sqlCommand = new SqlCommand(spNameOrText, miConnection);

            if (commandType.HasValue)
                sqlCommand.CommandType = commandType.Value;

            if (sqlParameters?.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

            return sqlCommand;
        }

        public SqlCommand CreateCommandForSp(string spName, List<SqlParameter> sqlParameters = null)
            => CreateCommand(spName, sqlParameters, CommandType.StoredProcedure);

        public SqlCommand CreateCommandForPlainText(string text, List<SqlParameter> sqlParameters = null)
            => CreateCommand(text, sqlParameters);

        public SqlParameter CreateParameter<TValue>(string name, TValue value, DbType dbType)
        {
            var sqlParameter = new SqlParameter(name, value);
            sqlParameter.DbType = dbType;
            return sqlParameter;
        }

        public SqlParameter CreateParameter(string name, int value)
            => CreateParameter(name, value, DbType.Int32);

        public SqlParameter CreateParameter(string name, int? value)
            => value.HasValue ? CreateParameter(name, value.Value) : CreateNullParameter(name);

        public SqlParameter CreateParameter(string name, string value)
            => CreateParameter(name, value, DbType.String);

        public SqlParameter CreateParameter(string name, DateTime value)
            => CreateParameter(name, value, DbType.DateTime);

        public SqlParameter CreateParameter(string name, bool value)
            => CreateParameter(name, value, DbType.Boolean);

        public SqlParameter CreateParameter(string name, decimal value)
            => CreateParameter(name, value, DbType.Decimal);

        public SqlParameter CreateParameter(string name, long value)
            => CreateParameter(name, value, DbType.Int64);

        public SqlParameter CreateNullParameter(string name)
            => new SqlParameter(name, DBNull.Value);

        public void Try(Action func, bool safe = false)
        {
            if (safe)
                TrySafe(func);
            else
                func();
        }

        public void TrySafe(Action func)
        {
            try
            {
                func();
            }
            catch (Exception)
            {
            }
        }

        private int _WriteInternal(
            string spNameOrText, List<SqlParameter> sqlParameters = null, CommandType commandType = CommandType.StoredProcedure, bool throwException = false)
        {
            var command = commandType == CommandType.StoredProcedure
                ? CreateCommandForSp(spNameOrText, sqlParameters)
                : CreateCommandForPlainText(spNameOrText, sqlParameters);

            int result;
            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                result = -1;

                if (throwException)
                    throw ex;
            }

            return result;
        }

        public int Write(
            string spNameOrText,
            List<SqlParameter> sqlParameters = null,
            CommandType commandType = CommandType.StoredProcedure,
            bool throwException = false,
            bool openAndClose = true)
        {
            if (openAndClose)
                Try(miConnection.Open, safe: throwException is false);

            int result = -1;

            try
            {
                result = _WriteInternal(spNameOrText, sqlParameters, commandType, throwException: throwException);
            }
            finally
            {
                if (openAndClose)
                    Try(miConnection.Close, safe: throwException is false);
            }

            return result;
        }

        public DataTable Read(
            string spNameOrText,
            List<SqlParameter>
            sqlParameters = null,
            CommandType commandType = CommandType.StoredProcedure,
            bool throwException = false,
            bool openAndClose = true)
        {
            if (openAndClose)
                Try(miConnection.Open, safe: throwException is false);

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
                {
                    var command = commandType == CommandType.StoredProcedure
                            ? CreateCommandForSp(spNameOrText, sqlParameters)
                            : CreateCommandForPlainText(spNameOrText, sqlParameters);

                    sqlDataAdapter.SelectCommand = command;
                    sqlDataAdapter.Fill(dataTable);
                    sqlDataAdapter.Dispose();
                }
            }
            finally
            {
                if (openAndClose)
                    Try(miConnection.Close, safe: throwException is false);
            }

            return dataTable;
        }
    }
}

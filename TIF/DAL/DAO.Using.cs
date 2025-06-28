using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
	public class DAOu : IDisposable
	{
		private readonly string connectionString = "Data Source=daw-db.cihg2e8ouwd8.us-east-1.rds.amazonaws.com;Initial Catalog=DAW_DB;User ID=admin;Password=663GsFAVvbT0h4dkUtK5";

		public void Dispose()
		{
		}

		public DataSet ExecuteDataSet(string commandText, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text)
		{
			DataSet ds = new DataSet();

			try
			{
				using (var connection = new SqlConnection(connectionString))
				using (var command = new SqlCommand(commandText, connection))
				using (var adapter = new SqlDataAdapter(command))
				{
					command.CommandType = commandType;
					AddParameters(command, parameters);

					connection.Open();
					adapter.Fill(ds);
				}
			}
			catch (SqlException ex)
			{
				throw new Exception($"Error ejecutando ExecuteDataSet: {ex.Message}", ex);
			}

			return ds;
		}

		public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text)
		{
			try
			{
				using (var connection = new SqlConnection(connectionString))
				using (var command = new SqlCommand(commandText, connection))
				{
					command.CommandType = commandType;
					AddParameters(command, parameters);

					connection.Open();
					return command.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				throw new Exception($"Error ejecutando ExecuteNonQuery: {ex.Message}", ex);
			}
		}

		public object ExecuteNonQuery(string commandText, out Dictionary<string, object> outputParameters, Dictionary<string, object> parameters = null)
		{
			outputParameters = new Dictionary<string, object>();

			try
			{
				using (var connection = new SqlConnection(connectionString))
				using (var command = new SqlCommand(commandText, connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					AddParameters(command, parameters);

					connection.Open();
					var result = command.ExecuteNonQuery();

					foreach (SqlParameter param in command.Parameters)
					{
						if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
						{
							outputParameters[param.ParameterName] = param.Value;
						}
					}

					return result;
				}
			}
			catch (SqlException ex)
			{
				throw new Exception($"Error ejecutando ExecuteNonQuery (con output): {ex.Message}", ex);
			}
		}

		public object ExecuteScalar(string commandText, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text)
		{
			try
			{
				using (var connection = new SqlConnection(connectionString))
				using (var command = new SqlCommand(commandText, connection))
				{
					command.CommandType = commandType;
					AddParameters(command, parameters);

					connection.Open();
					return command.ExecuteScalar();
				}
			}
			catch (SqlException ex)
			{
				throw new Exception($"Error ejecutando ExecuteScalar: {ex.Message}", ex);
			}
		}

		private void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
		{
			if (parameters == null)
				return;

			foreach (var param in parameters)
			{
				if (param.Value is SqlParameter sqlParam)
				{
					command.Parameters.Add(sqlParam);
				}
				else
				{
					command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
				}
			}
		}
	}
}

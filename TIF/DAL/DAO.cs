﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class DAO
    {

        /////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////    ATRIBUTOS     ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        private static DAO singleton;
        private SqlConnection miConnection;

        /////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////    CONSTRUCTOR     //////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        public static DAO GetDAO()
        {
            if (singleton == null)
            {
                singleton = new DAO();
            }
            return singleton;
        }

        private DAO()
        {
            string stringConexion = "Data Source=daw-db.cihg2e8ouwd8.us-east-1.rds.amazonaws.com;Initial Catalog=DAW_DB;User ID=admin;Password=663GsFAVvbT0h4dkUtK5";
            this.miConnection = new SqlConnection(stringConexion);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////    METODOS DE BD     //////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        //Prueba la conexion y devuelve resultado acorde
        private bool probarConexion()
        {
            try
            {
                this.miConnection.Open();
                this.miConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public DataSet ExecuteDataSet(string pCommandText)
        {
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(pCommandText, miConnection);
                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (miConnection.State != ConnectionState.Closed)
                    miConnection.Close();
            }

        }

        public int ExecuteNonQuery(string pCommandText)
        {
            try
            {
                SqlCommand command = new SqlCommand(pCommandText, miConnection);
                miConnection.Open();
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (miConnection.State != ConnectionState.Closed)
                    miConnection.Close();
            }
        }

        public int ExecuteNonQuery(string pCommandText, List<SqlParameter> parametros, CommandType tipo)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(pCommandText, miConnection))
                {
                    command.CommandType = tipo;
                    if (parametros != null)
                        command.Parameters.AddRange(parametros.ToArray());

                    miConnection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw; 
            }
            finally
            {
                if (miConnection.State != ConnectionState.Closed)
                    miConnection.Close();
            }
        }

    }
}
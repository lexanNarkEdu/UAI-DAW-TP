﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AccesoBDDAL
    {
        private SqlConnection conexion;

        public void AbrirConexion()
        {
            //String de conexion para la bd compartida
            conexion = new SqlConnection("Data Source=daw-db.cihg2e8ouwd8.us-east-1.rds.amazonaws.com;Initial Catalog=DAW_DB;User ID=admin;Password=663GsFAVvbT0h4dkUtK5");

            //String de conexion para la bd de la facu
            //conexion = new SqlConnection("Data Source=.;Initial Catalog=[NOMBRE BASE];Integrated Security=SSPI");

            //String de conexion para el instalador (usando el App.config)
            //conexion = new SqlConnection(ConfigurationManager.AppSettings["ConnectionDatabase"]);
            conexion.Open();
        }

        public void CerrarConexion()
        {
            conexion.Close();
            conexion = null;
            GC.Collect();
        }

        private SqlCommand CrearComando(string sql, List<SqlParameter> args = null, bool type = true)
        {
            SqlCommand cmd = new SqlCommand(sql, conexion);
            if (args != null)
            {
                cmd.Parameters.AddRange(args.ToArray());
            }

            if (type)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            return cmd;
        }

        public int Escribir(string sql, List<SqlParameter> args = null, bool type = true)
        {
            SqlCommand cmd = CrearComando(sql, args, type);
            int filAfec = 0;
            try
            {
                filAfec = cmd.ExecuteNonQuery();
            }
            catch
            {
                filAfec = -1;
            }
            return filAfec;
        }

        public DataTable Leer(string sql, List<SqlParameter> args = null, bool type = true)
        {
            DataTable tabla = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(CrearComando(sql, args, type)))
            {
                da.Fill(tabla);
                da.Dispose();
            }
            return tabla;
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            param.DbType = DbType.Int32;
            return param;
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            param.DbType = DbType.String;
            return param;
        }

        public SqlParameter CrearParametro(string nombre, float valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            //No especifico el DbType para que el motor lo asigne de manera automática
            return param;
        }

        public SqlParameter CrearParametro(string nombre, double valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            //No especifico el DbType para que el motor lo asigne de manera automática
            return param;
        }

        public SqlParameter CrearParametro(string nombre, byte[] valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            //No especifico el DbType para que el motor lo asigne de manera automática
            return param;
        }

        public SqlParameter CrearParametro(string nombre, DBNull valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            //No especifico el DbType para que el motor lo asigne de manera automática
            return param;
        }

        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            SqlParameter param = new SqlParameter(nombre, valor);
            param.DbType = DbType.DateTime;
            return param;
        }
    }
}

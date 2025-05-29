using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BE;
using System.Threading.Tasks;

namespace DAL
{
    public class UsuarioDAL
    {


        public static Usuario obtenerUsuario(string username)
        {
            string commandText = "" +
            "SELECT " +
                "usuario_username, " +
                "usuario_nombre, " +
                "usuario_apellido, " +
                "usuario_dni, " +
                "usuario_email, " +
                "usuario_fallos_autenticacion_consecutivos, " +
                "usuario_bloqueado, " +
                "usuario_domicilio, " +
                "usuario_password " +
            "FROM Usuario " +
            "WHERE usuario_username = '" + username + "'";

            DAO miDAO = DAO.GetDAO();

            DataSet dataSet = miDAO.ExecuteDataSet(commandText);

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return ValorizarEntidad(dataSet.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }

        }

        public static void loginInvalido(Usuario usuario)
        {
            string commandText = "" +
            "UPDATE Usuario " +
            "SET usuario_fallos_autenticacion_consecutivos = usuario_fallos_autenticacion_consecutivos + 1, " +
                "usuario_bloqueado = '" + usuario.Bloqueado.ToString() + "' " +
            "WHERE usuario_username = '" + usuario.Username + "'";

            DAO miDAO = DAO.GetDAO();
            miDAO.ExecuteNonQuery(commandText);
        }

        public static void loginValido(Usuario usuario)
        {
            string commandText = "" +
            "UPDATE Usuario " +
            "SET usuario_fallos_autenticacion_consecutivos = 0 " +
            "WHERE usuario_username = '" + usuario.Username + "'";

            DAO miDAO = DAO.GetDAO();
            miDAO.ExecuteNonQuery(commandText);
        }

        internal static Usuario ValorizarEntidad(DataRow pDataRow)
        {
            string usuario_username = pDataRow["usuario_username"].ToString();
            string usuario_nombre = pDataRow["usuario_nombre"].ToString();
            string usuario_apellido = pDataRow["usuario_apellido"].ToString();
            int usuario_dni = int.Parse(pDataRow["usuario_dni"].ToString());
            string usuario_email = pDataRow["usuario_email"].ToString();
            int usuario_fallos_autenticacion_consecutivos = int.Parse(pDataRow["usuario_fallos_autenticacion_consecutivos"].ToString());
            bool usuario_bloqueado = bool.Parse(pDataRow["usuario_bloqueado"].ToString());
            string usuario_domicilio = pDataRow["usuario_domicilio"].ToString();
            string usuario_password = pDataRow["usuario_password"].ToString();

            return new Usuario(usuario_username, usuario_password, usuario_nombre, usuario_apellido, usuario_dni, usuario_email, usuario_domicilio, usuario_fallos_autenticacion_consecutivos, usuario_bloqueado);

        }

    }

}
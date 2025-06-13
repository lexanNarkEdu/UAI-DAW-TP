using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UsuarioDAL
    {
        internal AccesoBDDAL acceso = new AccesoBDDAL();

        public List<UsuarioBE> buscar(UsuarioBE entidad)
        {
            List<UsuarioBE> usuarios = new List<UsuarioBE>();

            acceso.AbrirConexion();
            string commandText = $"SELECT * FROM Usuario WHERE usuario_username = '{entidad.Username}' AND usuario_password = '{entidad.Password}'";
            DataTable tabla = acceso.Leer(commandText, null, false);
            acceso.CerrarConexion();

            foreach (DataRow dr in tabla.Rows)
            {
                usuarios.Add(ValorizarEntidad(dr));
            }
            return usuarios;
        }

        public UsuarioBE obtenerUsuarioConUsername(string username)
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

            List<UsuarioBE> usuarios = new List<UsuarioBE>();

            acceso.AbrirConexion();
            DataTable tabla = acceso.Leer(commandText, null, false);
            acceso.CerrarConexion();

            foreach (DataRow dr in tabla.Rows)
            {
                usuarios.Add(ValorizarEntidad(dr));
            }
            return usuarios[0];
        }

        public void loginInvalido(UsuarioBE usuario)
        {
            string commandText = "" +
            "UPDATE Usuario " +
            "SET usuario_fallos_autenticacion_consecutivos = usuario_fallos_autenticacion_consecutivos + 1, " +
                "usuario_bloqueado = '" + usuario.Bloqueado.ToString() + "' " +
            "WHERE usuario_username = '" + usuario.Username + "'";

            acceso.AbrirConexion();
            acceso.Escribir(commandText, null, false);
            acceso.CerrarConexion();
        }

        public void loginValido(UsuarioBE usuario)
        {
            string commandText = "" +
            "UPDATE Usuario " +
            "SET usuario_fallos_autenticacion_consecutivos = 0 " +
            "WHERE usuario_username = '" + usuario.Username + "'";

            acceso.AbrirConexion();
            acceso.Escribir(commandText, null, false);
            acceso.CerrarConexion();
        }

        internal UsuarioBE ValorizarEntidad(DataRow pDataRow)
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

            return new UsuarioBE(usuario_username, usuario_password, usuario_nombre, usuario_apellido, usuario_dni, usuario_email, usuario_domicilio, usuario_fallos_autenticacion_consecutivos, usuario_bloqueado);
        }
    }
}

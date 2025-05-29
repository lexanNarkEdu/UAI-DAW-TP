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
        private static readonly PermisoDAL _permisoDAL = new PermisoDAL();

        /// <summary>
        /// Obtiene un usuario por su username incluyendo sus permisos
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Usuario con permisos cargados o null si no existe</returns>
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
                "usuario_password, " +
                "usuario_fecha_creacion " +
            "FROM Usuario " +
            "WHERE usuario_username = '" + username + "'";

            DAO miDAO = DAO.GetDAO();

            DataSet dataSet = miDAO.ExecuteDataSet(commandText);

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var usuario = ValorizarEntidad(dataSet.Tables[0].Rows[0]);

                // Cargar permisos del usuario
                CargarPermisosUsuario(usuario);

                return usuario;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su username sin cargar permisos (para operaciones simples)
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Usuario sin permisos cargados o null si no existe</returns>
        public static Usuario obtenerUsuarioSimple(string username)
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
                "usuario_password, " +
                "usuario_fecha_creacion " +
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

        /// <summary>
        /// Carga los permisos de un usuario
        /// </summary>
        /// <param name="usuario">Usuario al que cargar los permisos</param>
        private static void CargarPermisosUsuario(Usuario usuario)
        {
            try
            {
                var permisos = _permisoDAL.ObtenerPermisosPorUsuario(usuario.Username);

                // Limpiar permisos existentes
                usuario.LimpiarPermisos();

                // Agregar los permisos cargados
                foreach (var permiso in permisos)
                {
                    usuario.AgregarPermiso(permiso);
                }
            }
            catch (Exception ex)
            {
                // Log del error pero no interrumpir la carga del usuario
                System.Diagnostics.Debug.WriteLine($"Error al cargar permisos del usuario {usuario.Username}: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public static bool UsuarioTienePermiso(string username, string nombrePermiso)
        {
            try
            {
                return _permisoDAL.UsuarioTienePermiso(username, nombrePermiso);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar permiso {nombrePermiso} para usuario {username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene todos los permisos efectivos de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos efectivos</returns>
        public static List<IComponentePermiso> ObtenerPermisosEfectivos(string username)
        {
            try
            {
                return _permisoDAL.ObtenerTodosLosPermisosEfectivos(username);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener permisos efectivos para usuario {username}: {ex.Message}");
                return new List<IComponentePermiso>();
            }
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

            DateTime usuario_fecha_creacion = DateTime.Now;
            if (pDataRow["usuario_fecha_creacion"] != DBNull.Value)
            {
                usuario_fecha_creacion = Convert.ToDateTime(pDataRow["usuario_fecha_creacion"]);
            }

            var usuario = new Usuario(usuario_username, usuario_password, usuario_nombre, usuario_apellido, usuario_dni, usuario_email, usuario_domicilio, usuario_fallos_autenticacion_consecutivos, usuario_bloqueado);

            // Usar reflection para setear la fecha de creación (ya que es readonly)
            var fieldInfo = typeof(Usuario).GetField("<FechaCreacion>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(usuario, usuario_fecha_creacion);
            }

            return usuario;
        }
    }
}
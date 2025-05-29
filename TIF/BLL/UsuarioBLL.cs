using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class UsuarioBLL
    {
        private readonly PermisoBLL _permisoBLL;

        public UsuarioBLL()
        {
            _permisoBLL = new PermisoBLL();
        }

        /// <summary>
        /// Obtiene un usuario con sus permisos cargados
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Usuario con permisos o null si no existe</returns>
        public Usuario obtenerUsuario(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            return UsuarioDAL.obtenerUsuario(username);
        }

        /// <summary>
        /// Obtiene un usuario sin cargar permisos (para operaciones simples)
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Usuario sin permisos o null si no existe</returns>
        public Usuario obtenerUsuarioSimple(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            return UsuarioDAL.obtenerUsuarioSimple(username);
        }

        public void loginInvalido(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            usuario.FallosAutenticacionConsecutivos++;
            if (usuario.FallosAutenticacionConsecutivos >= 3)
            {
                usuario.Bloqueado = true;
            }

            UsuarioDAL.loginInvalido(usuario);
        }

        public void loginValido(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            usuario.FallosAutenticacionConsecutivos = 0;

            UsuarioDAL.loginValido(usuario);
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public bool UsuarioTienePermiso(string username, string nombrePermiso)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(nombrePermiso))
                return false;

            try
            {
                return _permisoBLL.UsuarioTienePermiso(username, nombrePermiso);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar permiso {nombrePermiso} para usuario {username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene un rol específico
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="nombreRol">Nombre del rol a verificar</param>
        /// <returns>True si el usuario tiene el rol</returns>
        public bool UsuarioTieneRol(string username, string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(nombreRol))
                return false;

            try
            {
                return _permisoBLL.UsuarioTieneRol(username, nombreRol);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar rol {nombreRol} para usuario {username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Valida si un usuario puede acceder a una funcionalidad específica
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="accionRequerida">Acción requerida para acceder</param>
        /// <returns>True si puede acceder</returns>
        public bool ValidarAcceso(string username, string accionRequerida)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(accionRequerida))
                return false;

            try
            {
                return _permisoBLL.ValidarAcceso(username, accionRequerida);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al validar acceso a {accionRequerida} para usuario {username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene todos los permisos efectivos de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos efectivos</returns>
        public List<IComponentePermiso> ObtenerPermisosEfectivos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new List<IComponentePermiso>();

            try
            {
                return _permisoBLL.ResolverPermisosEfectivos(username);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener permisos efectivos para usuario {username}: {ex.Message}");
                return new List<IComponentePermiso>();
            }
        }

        /// <summary>
        /// Obtiene los roles de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de roles</returns>
        public List<PermisoCompuesto> ObtenerRoles(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new List<PermisoCompuesto>();

            try
            {
                return _permisoBLL.ObtenerRoles(username);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener roles para usuario {username}: {ex.Message}");
                return new List<PermisoCompuesto>();
            }
        }

        /// <summary>
        /// Obtiene solo los permisos simples (acciones) de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos simples</returns>
        public List<PermisoSimple> ObtenerPermisosSimples(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new List<PermisoSimple>();

            try
            {
                return _permisoBLL.ObtenerPermisosSimples(username);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener permisos simples para usuario {username}: {ex.Message}");
                return new List<PermisoSimple>();
            }
        }

        /// <summary>
        /// Obtiene un resumen de permisos de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>String con resumen de permisos</returns>
        public string ObtenerResumenPermisos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Usuario no válido";

            try
            {
                return _permisoBLL.ObtenerResumenPermisos(username);
            }
            catch (Exception ex)
            {
                return $"Error al obtener resumen: {ex.Message}";
            }
        }

        /// <summary>
        /// Autenticación completa del usuario con validación de permisos
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="password">Password del usuario</param>
        /// <returns>Usuario autenticado con permisos cargados</returns>
        public Usuario AutenticarUsuario(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("El password es requerido", nameof(password));

            var usuario = obtenerUsuario(username);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuario no encontrado");

            if (usuario.Bloqueado)
                throw new UnauthorizedAccessException("Usuario bloqueado");

            if (!usuario.Password.Equals(password))
                throw new UnauthorizedAccessException("Credenciales incorrectas");

            return usuario;
        }
    }
}
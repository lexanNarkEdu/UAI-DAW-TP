using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using BE;
using BLL;

namespace TIF.UI
{
    /// <summary>
    /// Helper estático para manejar autorización y permisos en la interfaz web
    /// </summary>
    public static class AutorizacionHelper
    {
        private static readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();
        private static readonly PermisoBLL _permisoBLL = new PermisoBLL();

        #region Constantes de Permisos
        public const string GESTIONAR_BITACORA = "GestionarBitacoraEventos";
        public const string GESTIONAR_BITACORA_CAMBIOS = "GestionarBitacoraCambios";
        public const string GESTIONAR_BACKUP = "GestionarBackup";
        public const string ABM_USUARIOS = "ABMUsuarios";
        public const string ABM_PRODUCTO = "ABMProducto";
        public const string VER_PRODUCTOS = "VerProductos";

        public const string ROL_ADMINISTRADOR = "Administrador";
        public const string ROL_CLIENTE = "Cliente";
        public const string ROL_OPERADOR = "Operador";
        #endregion

        #region Validación de Sesión
        /// <summary>
        /// Verifica si hay un usuario logueado
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si hay usuario logueado</returns>
        public static bool EstaLogueado(HttpSessionState session)
        {
            return session != null &&
                   session["Usuario"] != null &&
                   session["Username"] != null;
        }

        /// <summary>
        /// Obtiene el username del usuario logueado
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>Username o null si no está logueado</returns>
        public static string ObtenerUsername(HttpSessionState session)
        {
            if (!EstaLogueado(session))
                return null;

            return session["Username"]?.ToString();
        }

        /// <summary>
        /// Obtiene el nombre completo del usuario logueado
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>Nombre completo o string vacío</returns>
        public static string ObtenerNombreCompleto(HttpSessionState session)
        {
            if (!EstaLogueado(session))
                return string.Empty;

            string nombre = session["Usuario"]?.ToString() ?? "";
            string apellido = session["Apellido"]?.ToString() ?? "";

            return $"{nombre} {apellido}".Trim();
        }
        #endregion

        #region Validación de Permisos
        /// <summary>
        /// Verifica si el usuario actual tiene un permiso específico
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <param name="permiso">Nombre del permiso</param>
        /// <returns>True si tiene el permiso</returns>
        public static bool TienePermiso(HttpSessionState session, string permiso)
        {
            string username = ObtenerUsername(session);
            if (string.IsNullOrEmpty(username))
                return false;

            return _usuarioBLL.UsuarioTienePermiso(username, permiso);
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <param name="rol">Nombre del rol</param>
        /// <returns>True si tiene el rol</returns>
        public static bool TieneRol(HttpSessionState session, string rol)
        {
            string username = ObtenerUsername(session);
            if (string.IsNullOrEmpty(username))
                return false;

            return _usuarioBLL.UsuarioTieneRol(username, rol);
        }

        /// <summary>
        /// Verifica si el usuario puede acceder a una funcionalidad
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <param name="accion">Acción requerida</param>
        /// <returns>True si puede acceder</returns>
        public static bool PuedeAcceder(HttpSessionState session, string accion)
        {
            string username = ObtenerUsername(session);
            if (string.IsNullOrEmpty(username))
                return false;

            return _usuarioBLL.ValidarAcceso(username, accion);
        }
        #endregion

        #region Validaciones Específicas de Funcionalidades
        /// <summary>
        /// Verifica si el usuario puede acceder a la bitácora
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si puede acceder</returns>
        public static bool PuedeAccederBitacora(HttpSessionState session)
        {
            return TienePermiso(session, GESTIONAR_BITACORA) ||
                   TieneRol(session, ROL_ADMINISTRADOR);
        }

        /// <summary>
        /// Verifica si el usuario puede gestionar usuarios
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si puede gestionar usuarios</returns>
        public static bool PuedeGestionarUsuarios(HttpSessionState session)
        {
            return TienePermiso(session, ABM_USUARIOS) ||
                   TieneRol(session, ROL_ADMINISTRADOR);
        }

        /// <summary>
        /// Verifica si el usuario puede gestionar productos
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si puede gestionar productos</returns>
        public static bool PuedeGestionarProductos(HttpSessionState session)
        {
            return TienePermiso(session, ABM_PRODUCTO) ||
                   TieneRol(session, ROL_ADMINISTRADOR) ||
                   TieneRol(session, ROL_OPERADOR);
        }

        /// <summary>
        /// Verifica si el usuario puede ver productos
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si puede ver productos</returns>
        public static bool PuedeVerProductos(HttpSessionState session)
        {
            return TienePermiso(session, VER_PRODUCTOS) ||
                   TienePermiso(session, ABM_PRODUCTO) ||
                   TieneRol(session, ROL_ADMINISTRADOR) ||
                   TieneRol(session, ROL_OPERADOR) ||
                   TieneRol(session, ROL_CLIENTE);
        }

        /// <summary>
        /// Verifica si el usuario puede gestionar backups
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>True si puede gestionar backups</returns>
        public static bool PuedeGestionarBackup(HttpSessionState session)
        {
            return TienePermiso(session, GESTIONAR_BACKUP) ||
                   TieneRol(session, ROL_ADMINISTRADOR);
        }
        #endregion

        #region Redirecciones y Control de Acceso
        /// <summary>
        /// Verifica autenticación y redirige al login si no está logueado
        /// </summary>
        /// <param name="page">Página actual</param>
        /// <returns>True si está autenticado</returns>
        public static bool VerificarAutenticacion(Page page)
        {
            if (!EstaLogueado(page.Session))
            {
                page.Response.Redirect("Login.aspx", false);
                //page.Context.ApplicationInstance.CompleteRequest();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifica autorización específica para la bitácora
        /// </summary>
        /// <param name="page">Página actual</param>
        /// <returns>True si puede acceder</returns>
        public static bool VerificarAccesoBitacora(Page page)
        {
            return PageAuthorizationExtensions.VerificarAutorizacion(page, "BITACORA");
        }
        #endregion

        #region Información de Usuario
        /// <summary>
        /// Obtiene un resumen de los permisos del usuario actual
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>Resumen de permisos</returns>
        public static string ObtenerResumenPermisos(HttpSessionState session)
        {
            string username = ObtenerUsername(session);
            if (string.IsNullOrEmpty(username))
                return "Usuario no autenticado";

            return _usuarioBLL.ObtenerResumenPermisos(username);
        }

        /// <summary>
        /// Obtiene la lista de permisos efectivos del usuario actual
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <returns>Lista de permisos</returns>
        public static List<IComponentePermiso> ObtenerPermisosEfectivos(HttpSessionState session)
        {
            string username = ObtenerUsername(session);
            if (string.IsNullOrEmpty(username))
                return new List<IComponentePermiso>();

            return _usuarioBLL.ObtenerPermisosEfectivos(username);
        }

        /// <summary>
        /// Almacena información de permisos en la sesión para acceso rápido
        /// </summary>
        /// <param name="session">Sesión HTTP</param>
        /// <param name="username">Username del usuario</param>
        public static void CargarPermisosEnSesion(HttpSessionState session, string username)
        {
            try
            {
                var permisos = _usuarioBLL.ObtenerPermisosEfectivos(username);
                var roles = _usuarioBLL.ObtenerRoles(username);

                session["PermisosUsuario"] = permisos;
                session["RolesUsuario"] = roles;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar permisos en sesión para {username}: {ex.Message}");
            }
        }
        #endregion
    }
}
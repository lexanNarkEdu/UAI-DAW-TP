using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BLL
{
    public class PermisoBLL
    {
        private readonly PermisoDAL _permisoDAL;

        public PermisoBLL()
        {
            _permisoDAL = new PermisoDAL();
        }

        /// <summary>
        /// Obtiene un permiso por su ID
        /// </summary>
        /// <param name="permisoId">ID del permiso</param>
        /// <returns>Permiso encontrado o null</returns>
        public IComponentePermiso ObtenerPorId(int permisoId)
        {
            if (permisoId <= 0)
                throw new ArgumentException("El ID del permiso debe ser mayor a 0", nameof(permisoId));

            return _permisoDAL.ObtenerPorId(permisoId);
        }

        /// <summary>
        /// Obtiene todos los permisos disponibles
        /// </summary>
        /// <returns>Lista de todos los permisos</returns>
        public List<IComponentePermiso> ObtenerTodos()
        {
            return _permisoDAL.ObtenerTodos();
        }

        /// <summary>
        /// Construye el árbol completo de permisos para un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de permisos con estructura jerárquica</returns>
        public List<IComponentePermiso> ConstruirArbolPermisos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            try
            {
                return _permisoDAL.ObtenerPermisosPorUsuario(username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al construir árbol de permisos para usuario {username}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Resuelve todos los permisos efectivos de un usuario (incluyendo herencia)
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de todos los permisos efectivos</returns>
        public List<IComponentePermiso> ResolverPermisosEfectivos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            try
            {
                return _permisoDAL.ObtenerTodosLosPermisosEfectivos(username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al resolver permisos efectivos para usuario {username}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <param name="nombrePermiso">Nombre del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public bool UsuarioTienePermiso(string username, string nombrePermiso)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            if (string.IsNullOrWhiteSpace(nombrePermiso))
                return false;

            try
            {
                return _permisoDAL.UsuarioTienePermiso(username, nombrePermiso);
            }
            catch (Exception ex)
            {
                // Log del error pero devolver false por seguridad
                System.Diagnostics.Debug.WriteLine($"Error al verificar permiso {nombrePermiso} para usuario {username}: {ex.Message}");
                return false;
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
                throw new ArgumentException("El username es requerido", nameof(username));

            try
            {
                return _permisoDAL.ObtenerPermisosSimplesPorUsuario(username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener permisos simples para usuario {username}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene los roles (permisos compuestos) de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de roles</returns>
        public List<PermisoCompuesto> ObtenerRoles(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El username es requerido", nameof(username));

            try
            {
                var permisosDirectos = _permisoDAL.ObtenerPermisosPorUsuario(username);
                return permisosDirectos.OfType<PermisoCompuesto>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener roles para usuario {username}: {ex.Message}", ex);
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
                var roles = ObtenerRoles(username);
                return roles.Any(rol => string.Equals(rol.Nombre, nombreRol, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar rol {nombreRol} para usuario {username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene los nombres de todos los permisos efectivos de un usuario
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>Lista de nombres de permisos</returns>
        public List<string> ObtenerNombresPermisosEfectivos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new List<string>();

            try
            {
                var permisos = ResolverPermisosEfectivos(username);
                return permisos.Select(p => p.Nombre).Distinct().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener nombres de permisos para usuario {username}: {ex.Message}");
                return new List<string>();
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

            // Verificar permisos específicos para funcionalidades críticas
            switch (accionRequerida.ToUpper())
            {
                case "BITACORA":
                    return UsuarioTienePermiso(username, "GestionarBitacoraEventos") ||
                           UsuarioTieneRol(username, "Administrador");

                case "BACKUP":
                    return UsuarioTienePermiso(username, "GestionarBackup") ||
                           UsuarioTieneRol(username, "Administrador");

                case "USUARIOS":
                    return UsuarioTienePermiso(username, "ABMUsuarios") ||
                           UsuarioTieneRol(username, "Administrador");

                case "PRODUCTOS":
                    return UsuarioTienePermiso(username, "ABMProducto") ||
                           UsuarioTienePermiso(username, "VerProductos") ||
                           UsuarioTieneRol(username, "Administrador") ||
                           UsuarioTieneRol(username, "Operador") ||
                           UsuarioTieneRol(username, "Cliente");

                default:
                    return UsuarioTienePermiso(username, accionRequerida);
            }
        }

        /// <summary>
        /// Obtiene un resumen de permisos de un usuario para debugging
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns>String con resumen de permisos</returns>
        public string ObtenerResumenPermisos(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Usuario no válido";

            try
            {
                var permisosDirectos = _permisoDAL.ObtenerPermisosPorUsuario(username);
                var permisosEfectivos = ResolverPermisosEfectivos(username);

                var resumen = $"Usuario: {username}\n";
                resumen += $"Permisos directos: {permisosDirectos.Count}\n";
                resumen += $"Permisos efectivos: {permisosEfectivos.Count}\n";

                resumen += "\nRoles:\n";
                foreach (var permiso in permisosDirectos.OfType<PermisoCompuesto>())
                {
                    resumen += $"  - {permiso.Nombre}\n";
                }

                resumen += "\nPermisos simples efectivos:\n";
                foreach (var permiso in permisosEfectivos.OfType<PermisoSimple>())
                {
                    resumen += $"  - {permiso.Nombre}\n";
                }

                return resumen;
            }
            catch (Exception ex)
            {
                return $"Error al obtener resumen: {ex.Message}";
            }
        }
    }
}
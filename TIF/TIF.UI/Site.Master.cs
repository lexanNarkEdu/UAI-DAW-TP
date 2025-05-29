using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace TIF.UI
{
    public partial class SiteMaster : MasterPage
    {
        private readonly BitacoraBLL _bitacoraBLL;

        public SiteMaster()
        {
            _bitacoraBLL = new BitacoraBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Controlar visibilidad del navbar basado en la sesión
            bool usuarioLogueado = AutorizacionHelper.EstaLogueado(Session);
            NavbarPanel.Visible = usuarioLogueado;

            // Si hay usuario logueado, configurar la interfaz
            if (usuarioLogueado)
            {
                ConfigurarInterfazUsuario();
                ConfigurarMenuSegunPermisos();
            }
        }

        /// <summary>
        /// Configura la información del usuario en la interfaz
        /// </summary>
        private void ConfigurarInterfazUsuario()
        {
            try
            {
                string nombreCompleto = AutorizacionHelper.ObtenerNombreCompleto(Session);
                UsuarioLogueado.Text = !string.IsNullOrEmpty(nombreCompleto) ? nombreCompleto : "Usuario";

                // Agregar información adicional si es administrador
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    UsuarioLogueado.Text += " (Admin)";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar interfaz de usuario: {ex.Message}");
                UsuarioLogueado.Text = "Usuario";
            }
        }

        /// <summary>
        /// Configura la visibilidad del menú según los permisos del usuario
        /// </summary>
        private void ConfigurarMenuSegunPermisos()
        {
            try
            {
                // Encontrar los controles del menú en el navbar
                var navbarContainer = NavbarPanel.FindControl("navbar-collapse") as Panel;

                // Como estamos trabajando con HTML estático, vamos a usar JavaScript para ocultar/mostrar elementos
                ConfigurarMenuConJavaScript();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar menú: {ex.Message}");
            }
        }

        /// <summary>
        /// Configura el menú usando JavaScript basado en los permisos del usuario
        /// </summary>
        private void ConfigurarMenuConJavaScript()
        {
            try
            {
                string script = GenerarScriptPermisos();

                if (!string.IsNullOrEmpty(script))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfigurarMenu", script, true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al generar script de permisos: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera el script JavaScript para controlar la visibilidad del menú
        /// </summary>
        /// <returns>Script JavaScript</returns>
        private string GenerarScriptPermisos()
        {
            var script = @"
                document.addEventListener('DOMContentLoaded', function() {
                    try {
                        // Función para ocultar un elemento del menú
                        function ocultarMenuItem(href) {
                            var enlaces = document.querySelectorAll('a.nav-link[href*=""' + href + '""]');
                            enlaces.forEach(function(enlace) {
                                var li = enlace.closest('.nav-item');
                                if (li) {
                                    li.style.display = 'none';
                                }
                            });
                        }

                        // Función para mostrar un elemento del menú
                        function mostrarMenuItem(href) {
                            var enlaces = document.querySelectorAll('a.nav-link[href*=""' + href + '""]');
                            enlaces.forEach(function(enlace) {
                                var li = enlace.closest('.nav-item');
                                if (li) {
                                    li.style.display = 'block';
                                }
                            });
                        }

                        // Controlar visibilidad según permisos
            ";

            // Agregar lógica específica según permisos
            if (!AutorizacionHelper.PuedeAccederBitacora(Session))
            {
                script += @"
                        ocultarMenuItem('Bitacora');
                ";
            }

            // Si el usuario no tiene permisos especiales, ocultar elementos administrativos
            if (!AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
            {
                script += @"
                        // Ocultar elementos administrativos si no es admin
                        // (Aquí se pueden agregar más elementos cuando se implementen)
                ";
            }

            script += @"
                        // Agregar indicadores visuales para usuarios con permisos especiales
                        " + GenerarIndicadoresPermisos() + @"

                    } catch (e) {
                        console.log('Error al configurar menú:', e);
                    }
                });
            ";

            return script;
        }

        /// <summary>
        /// Genera indicadores visuales para mostrar permisos especiales
        /// </summary>
        /// <returns>Código JavaScript para indicadores</returns>
        private string GenerarIndicadoresPermisos()
        {
            string indicadores = "";

            try
            {
                // Agregar badge si puede acceder a bitácora
                if (AutorizacionHelper.PuedeAccederBitacora(Session))
                {
                    indicadores += @"
                        var bitacoraLink = document.querySelector('a.nav-link[href*=""Bitacora""]');
                        if (bitacoraLink && !bitacoraLink.querySelector('.badge')) {
                            var badge = document.createElement('span');
                            badge.className = 'badge bg-info ms-1';
                            badge.textContent = 'Audit';
                            badge.title = 'Acceso a auditoría';
                            bitacoraLink.appendChild(badge);
                        }
                    ";
                }

                // Agregar badge si es administrador
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    indicadores += @"
                        var homeLink = document.querySelector('a.nav-link[href*=""Home""]');
                        if (homeLink && !homeLink.querySelector('.badge')) {
                            var badge = document.createElement('span');
                            badge.className = 'badge bg-danger ms-1';
                            badge.textContent = 'Admin';
                            badge.title = 'Administrador del sistema';
                            homeLink.appendChild(badge);
                        }
                    ";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al generar indicadores: {ex.Message}");
            }

            return indicadores;
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Registrar logout en bitácora si hay usuario en sesión
                if (AutorizacionHelper.EstaLogueado(Session))
                {
                    string username = AutorizacionHelper.ObtenerUsername(Session);
                    if (!string.IsNullOrEmpty(username))
                    {
                        _bitacoraBLL.RegistrarLogout(username);
                        System.Diagnostics.Debug.WriteLine($"Logout registrado para usuario: {username}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error pero no interrumpir el logout
                System.Diagnostics.Debug.WriteLine($"Error al registrar logout: {ex.Message}");
            }
            finally
            {
                // Limpiar sesión y redirigir al login
                try
                {
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al limpiar sesión: {ex.Message}");
                    // Forzar redirección en caso de error
                    Response.Redirect("Login.aspx", true);
                }
            }
        }

        /// <summary>
        /// Método público para que las páginas puedan verificar permisos desde el master
        /// </summary>
        /// <param name="permiso">Permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso</returns>
        public bool UsuarioTienePermiso(string permiso)
        {
            return AutorizacionHelper.TienePermiso(Session, permiso);
        }

        /// <summary>
        /// Método público para que las páginas puedan verificar roles desde el master
        /// </summary>
        /// <param name="rol">Rol a verificar</param>
        /// <returns>True si el usuario tiene el rol</returns>
        public bool UsuarioTieneRol(string rol)
        {
            return AutorizacionHelper.TieneRol(Session, rol);
        }

        /// <summary>
        /// Obtiene información del usuario actual para mostrar en las páginas
        /// </summary>
        /// <returns>Información del usuario</returns>
        public string ObtenerInformacionUsuario()
        {
            try
            {
                if (!AutorizacionHelper.EstaLogueado(Session))
                    return "Usuario no autenticado";

                string nombreCompleto = AutorizacionHelper.ObtenerNombreCompleto(Session);
                string username = AutorizacionHelper.ObtenerUsername(Session);

                var info = $"Usuario: {nombreCompleto} ({username})";

                // Agregar roles
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                    info += " - Administrador";
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_OPERADOR))
                    info += " - Operador";
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_CLIENTE))
                    info += " - Cliente";

                return info;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener información de usuario: {ex.Message}");
                return "Error al obtener información del usuario";
            }
        }
    }
}
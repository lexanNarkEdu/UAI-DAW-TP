using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using BE;
using BLL;

namespace TIF.UI
{
    public partial class Home : Page
    {
        private readonly UsuarioBLL _usuarioBLL;

        public Home()
        {
            _usuarioBLL = new UsuarioBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario esté logueado
            if (!AutorizacionHelper.VerificarAutenticacion(this))
                return;

            if (!IsPostBack)
            {
                CargarDatosUsuario();
                MostrarInformacionContextual();
                ConfigurarInterfazSegunPermisos();
            }
        }

        private void CargarDatosUsuario()
        {
            try
            {
                // Obtener datos de la sesión
                string nombreCompleto = AutorizacionHelper.ObtenerNombreCompleto(Session);

                if (string.IsNullOrEmpty(nombreCompleto))
                {
                    nombreCompleto = "Usuario";
                }

                NombreUsuario.Text = nombreCompleto;

                // Mostrar última conexión
                DateTime fechaLogin = DateTime.Now;
                if (Session["FechaLogin"] != null && DateTime.TryParse(Session["FechaLogin"].ToString(), out DateTime fecha))
                {
                    fechaLogin = fecha;
                }

                FechaHoraIngreso.Text = fechaLogin.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar información básica
                NombreUsuario.Text = "Usuario";
                FechaHoraIngreso.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                System.Diagnostics.Debug.WriteLine($"Error al cargar datos del usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Muestra información contextual según los permisos del usuario
        /// </summary>
        private void MostrarInformacionContextual()
        {
            try
            {
                StringBuilder mensajeContextual = new StringBuilder();
                string username = AutorizacionHelper.ObtenerUsername(Session);

                // Mensaje según el rol principal
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    mensajeContextual.AppendLine("🔧 <strong>Administrador del Sistema</strong>");
                    mensajeContextual.AppendLine("<br/>Tienes acceso completo a todas las funcionalidades del sistema.");

                    // Mostrar resumen de permisos para administradores
                    var resumenPermisos = ObtenerResumenPermisosAdmin(username);
                    if (!string.IsNullOrEmpty(resumenPermisos))
                    {
                        mensajeContextual.AppendLine($"<br/><small>{resumenPermisos}</small>");
                    }
                }
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_OPERADOR))
                {
                    mensajeContextual.AppendLine("⚙️ <strong>Operador del Sistema</strong>");
                    mensajeContextual.AppendLine("<br/>Puedes gestionar productos y realizar operaciones del sistema.");
                }
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_CLIENTE))
                {
                    mensajeContextual.AppendLine("👤 <strong>Cliente</strong>");
                    mensajeContextual.AppendLine("<br/>Puedes consultar productos y realizar pedidos.");
                }
                else
                {
                    mensajeContextual.AppendLine("👋 <strong>Bienvenido</strong>");
                    mensajeContextual.AppendLine("<br/>Accede a las funcionalidades disponibles desde el menú.");
                }

                // Agregar funcionalidades disponibles
                var funcionalidades = ObtenerFuncionalidadesDisponibles();
                if (funcionalidades.Count > 0)
                {
                    mensajeContextual.AppendLine("<br/><br/><strong>Funcionalidades disponibles:</strong>");
                    mensajeContextual.AppendLine("<ul>");
                    foreach (var funcionalidad in funcionalidades)
                    {
                        mensajeContextual.AppendLine($"<li>{funcionalidad}</li>");
                    }
                    mensajeContextual.AppendLine("</ul>");
                }

                // Mostrar mensaje en la interfaz (necesitaremos agregar un control en el ASPX)
                if (mensajeContextual.Length > 0)
                {
                    Session["MensajeContextual"] = mensajeContextual.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al mostrar información contextual: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un resumen de permisos para administradores
        /// </summary>
        /// <param name="username">Username del administrador</param>
        /// <returns>Resumen de permisos</returns>
        private string ObtenerResumenPermisosAdmin(string username)
        {
            try
            {
                var permisos = _usuarioBLL.ObtenerPermisosEfectivos(username);
                var permisosSimples = permisos.OfType<PermisoSimple>().Count();
                var roles = _usuarioBLL.ObtenerRoles(username).Count;

                return $"Permisos activos: {permisosSimples} acciones, {roles} roles";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener resumen de permisos: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades disponibles para el usuario actual
        /// </summary>
        /// <returns>Lista de funcionalidades</returns>
        private List<string> ObtenerFuncionalidadesDisponibles()
        {
            var funcionalidades = new List<string>();

            try
            {
                // Verificar acceso a bitácora
                if (AutorizacionHelper.PuedeAccederBitacora(Session))
                {
                    funcionalidades.Add("📊 <strong>Bitácora del Sistema</strong> - Consulta eventos y auditoría");
                }

                // Verificar gestión de usuarios
                if (AutorizacionHelper.PuedeGestionarUsuarios(Session))
                {
                    funcionalidades.Add("👥 <strong>Gestión de Usuarios</strong> - Crear, modificar y eliminar usuarios");
                }

                // Verificar gestión de productos
                if (AutorizacionHelper.PuedeGestionarProductos(Session))
                {
                    funcionalidades.Add("📦 <strong>Gestión de Productos</strong> - Administrar catálogo de productos");
                }
                else if (AutorizacionHelper.PuedeVerProductos(Session))
                {
                    funcionalidades.Add("👀 <strong>Ver Productos</strong> - Consultar catálogo de productos");
                }

                // Verificar gestión de backups
                if (AutorizacionHelper.PuedeGestionarBackup(Session))
                {
                    funcionalidades.Add("💾 <strong>Gestión de Backups</strong> - Crear y restaurar copias de seguridad");
                }

                // Si no tiene funcionalidades específicas, mostrar las básicas
                if (funcionalidades.Count == 0)
                {
                    funcionalidades.Add("🏠 <strong>Panel Principal</strong> - Información general del sistema");
                    funcionalidades.Add("ℹ️ <strong>Información</strong> - Detalles sobre el sistema");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener funcionalidades: {ex.Message}");
                funcionalidades.Add("Error al cargar funcionalidades disponibles");
            }

            return funcionalidades;
        }

        /// <summary>
        /// Configura la interfaz según los permisos del usuario
        /// </summary>
        private void ConfigurarInterfazSegunPermisos()
        {
            try
            {
                // Generar script para personalizar la interfaz
                string script = GenerarScriptPersonalizacion();

                if (!string.IsNullOrEmpty(script))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "PersonalizarHome", script, true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar interfaz: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera script JavaScript para personalizar la interfaz
        /// </summary>
        /// <returns>Script de personalización</returns>
        private string GenerarScriptPersonalizacion()
        {
            var script = @"
                document.addEventListener('DOMContentLoaded', function() {
                    try {
                        // Agregar información contextual
                        var mensajeContextual = '" + (Session["MensajeContextual"]?.ToString() ?? "").Replace("'", "\\'").Replace("\r\n", "\\n").Replace("\n", "\\n") + @"';
                        
                        if (mensajeContextual) {
                            // Buscar el contenedor del mensaje de bienvenida
                            var jumbotron = document.querySelector('.jumbotron');
                            if (jumbotron) {
                                var mensajeDiv = document.createElement('div');
                                mensajeDiv.className = 'mt-3 p-3 bg-light rounded';
                                mensajeDiv.innerHTML = mensajeContextual;
                                jumbotron.appendChild(mensajeDiv);
                            }
                        }

                        // Agregar enlaces rápidos según permisos
                        " + GenerarEnlacesRapidos() + @"

                        // Personalizar colores según el rol
                        " + GenerarPersonalizacionColores() + @"

                    } catch (e) {
                        console.log('Error al personalizar home:', e);
                    }
                });
            ";

            return script;
        }

        /// <summary>
        /// Genera enlaces rápidos según los permisos del usuario
        /// </summary>
        /// <returns>Código JavaScript para enlaces rápidos</returns>
        private string GenerarEnlacesRapidos()
        {
            var enlaces = new List<string>();

            try
            {
                if (AutorizacionHelper.PuedeAccederBitacora(Session))
                {
                    enlaces.Add("'<a href=\"Bitacora.aspx\" class=\"btn btn-outline-primary me-2 mb-2\"><i class=\"bi bi-journal-text\"></i> Bitácora</a>'");
                }

                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    enlaces.Add("'<a href=\"#\" class=\"btn btn-outline-danger me-2 mb-2\"><i class=\"bi bi-gear\"></i> Administración</a>'");
                }

                if (enlaces.Count > 0)
                {
                    return $@"
                        var enlacesRapidos = [{string.Join(", ", enlaces)}];
                        var container = document.querySelector('.row:last-child');
                        if (container) {{
                            var enlacesDiv = document.createElement('div');
                            enlacesDiv.className = 'col-md-12 mt-4';
                            enlacesDiv.innerHTML = '<h5>Accesos Rápidos</h5><div>' + enlacesRapidos.join('') + '</div>';
                            container.appendChild(enlacesDiv);
                        }}
                    ";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al generar enlaces rápidos: {ex.Message}");
            }

            return "";
        }

        /// <summary>
        /// Genera personalización de colores según el rol del usuario
        /// </summary>
        /// <returns>Código JavaScript para personalización de colores</returns>
        private string GenerarPersonalizacionColores()
        {
            try
            {
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    return @"
                        // Personalización para administradores
                        var jumbotron = document.querySelector('.jumbotron');
                        if (jumbotron) {
                            jumbotron.style.background = 'linear-gradient(135deg, #dc3545, #6f42c1)';
                            jumbotron.style.borderLeft = '5px solid #dc3545';
                        }
                    ";
                }
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_OPERADOR))
                {
                    return @"
                        // Personalización para operadores
                        var jumbotron = document.querySelector('.jumbotron');
                        if (jumbotron) {
                            jumbotron.style.background = 'linear-gradient(135deg, #28a745, #20c997)';
                            jumbotron.style.borderLeft = '5px solid #28a745';
                        }
                    ";
                }
                else if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_CLIENTE))
                {
                    return @"
                        // Personalización para clientes
                        var jumbotron = document.querySelector('.jumbotron');
                        if (jumbotron) {
                            jumbotron.style.background = 'linear-gradient(135deg, #007bff, #6610f2)';
                            jumbotron.style.borderLeft = '5px solid #007bff';
                        }
                    ";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al generar personalización de colores: {ex.Message}");
            }

            return "";
        }
    }
}
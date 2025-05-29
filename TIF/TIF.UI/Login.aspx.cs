using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;
using Services;

namespace TIF.UI
{
    public partial class _Default : Page
    {
        private readonly BitacoraBLL _bitacoraBLL;
        private readonly UsuarioBLL _usuarioBLL;

        public _Default()
        {
            _bitacoraBLL = new BitacoraBLL();
            _usuarioBLL = new UsuarioBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Si ya está logueado, redirigir al home
            if (AutorizacionHelper.EstaLogueado(Session))
            {
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        protected void ingresarButton_Click(object sender, EventArgs e)
        {
            EncriptadorService encriptador = EncriptadorService.GetEncriptadorService();
            string username = encriptador.EncriptarAES(usuarioTextbox.Text.Trim());
            string password = encriptador.EncriptarMD5(passwordTextbox.Text.Trim());

            try
            {
                Usuario usuario = _usuarioBLL.obtenerUsuario(username);

                if (usuario == null)
                {
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username,
                        "Intento de acceso con usuario no existente", EventoCriticidadEnum.Media);
                    throw new UsuarioInvalidoException();
                }

                if (usuario.Bloqueado)
                {
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username,
                        "Intento de acceso con usuario bloqueado", EventoCriticidadEnum.Media);
                    throw new UsuarioBloqueadoException();
                }

                if (!usuario.Password.Equals(password))
                {
                    _usuarioBLL.loginInvalido(usuario);
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username,
                        "Intento de acceso con password incorrecta", EventoCriticidadEnum.Media);
                    throw new UsuarioInvalidoException();
                }

                // Login exitoso
                _usuarioBLL.loginValido(usuario);
                _bitacoraBLL.RegistrarEvento(EventoTipoEnum.Login, username,
                    "Login exitoso", EventoCriticidadEnum.Baja);

                // Establecer datos en sesión
                EstablecerSesionUsuario(usuario);

                // Redirigir según permisos del usuario
                RedirigirSegunPermisos();
            }
            catch (Exception ex)
            {
                // Solo registrar error de sistema si no es una excepción de usuario conocida
                if (!(ex is UsuarioInvalidoException) && !(ex is UsuarioBloqueadoException))
                {
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.ErrorSistema, username,
                        $"Error durante login: {ex.Message}", EventoCriticidadEnum.Alta);
                }

                // Mostrar mensaje de error al usuario
                MostrarMensajeError(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Establece los datos del usuario en la sesión
        /// </summary>
        /// <param name="usuario">Usuario autenticado</param>
        private void EstablecerSesionUsuario(Usuario usuario)
        {
            Session["Usuario"] = usuario.Nombre;
            Session["Username"] = usuario.Username;
            Session["Apellido"] = usuario.Apellido;
            Session["Email"] = usuario.Email;
            Session["FechaLogin"] = DateTime.Now;

            // Cargar permisos en sesión para acceso rápido
            AutorizacionHelper.CargarPermisosEnSesion(Session, usuario.Username);

            // Obtener resumen de permisos para debugging
            var resumenPermisos = _usuarioBLL.ObtenerResumenPermisos(usuario.Username);
            System.Diagnostics.Debug.WriteLine($"Permisos cargados para {usuario.Username}:\n{resumenPermisos}");
        }

        /// <summary>
        /// Redirige al usuario según sus permisos
        /// </summary>
        private void RedirigirSegunPermisos()
        {
            try
            {
                // Verificar si es administrador
                if (AutorizacionHelper.TieneRol(Session, AutorizacionHelper.ROL_ADMINISTRADOR))
                {
                    Response.Redirect("Home.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                // Verificar si puede acceder a la bitácora
                if (AutorizacionHelper.PuedeAccederBitacora(Session))
                {
                    // Mostrar mensaje de bienvenida especial para usuarios con acceso a bitácora
                    Session["MensajeBienvenida"] = "Tienes acceso a funciones de auditoría del sistema.";
                }

                // Verificar si puede gestionar productos
                if (AutorizacionHelper.PuedeGestionarProductos(Session))
                {
                    Session["MensajeBienvenida"] = Session["MensajeBienvenida"] + " Puedes gestionar productos.";
                }

                // Redirigir a la página principal
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al redirigir usuario: {ex.Message}");
                // En caso de error, redirigir al home por defecto
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Muestra un mensaje de error al usuario
        /// </summary>
        /// <param name="mensaje">Mensaje de error</param>
        private void MostrarMensajeError(string mensaje)
        {
            string mensajeSeguro = HttpUtility.JavaScriptStringEncode(mensaje);
            string script = $"alert('Error: {mensajeSeguro}');";
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorLogin", script, true);
        }

        protected void newUser_Click(object sender, EventArgs e)
        {
            // Funcionalidad para crear nuevo usuario (a implementar)
            string script = "alert('Funcionalidad de registro no implementada. Contacte al administrador.');";
            ClientScript.RegisterStartupScript(this.GetType(), "RegistroNoDisponible", script, true);
        }

        protected void forgotPassword_Click(object sender, EventArgs e)
        {
            // Funcionalidad para recuperar contraseña (a implementar)
            string script = "alert('Funcionalidad de recuperación no implementada. Contacte al administrador.');";
            ClientScript.RegisterStartupScript(this.GetType(), "RecuperacionNoDisponible", script, true);
        }
    }
}
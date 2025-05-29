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

        public _Default() 
            => _bitacoraBLL = new BitacoraBLL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ingresarButton_Click(object sender, EventArgs e)
        {
            
            EncriptadorService encriptador = EncriptadorService.GetEncriptadorService();
            string username = encriptador.EncriptarAES(usuarioTextbox.Text.Trim());
            string password = encriptador.EncriptarMD5(passwordTextbox.Text.Trim());
            UsuarioBLL usuarioBLL = new UsuarioBLL();

            try {

                Usuario usuario = usuarioBLL.obtenerUsuario(username);

                if (usuario == null ) {
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username, "Intento de acceso con usuario no existente", EventoCriticidadEnum.Media);
                    throw new UsuarioInvalidoException();
                }
                if (usuario.Bloqueado) {
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username, "Intento de acceso con usuario bloqueado", EventoCriticidadEnum.Media);
                    throw new UsuarioBloqueadoException();
                }
                if (!usuario.Password.Equals(password))
                {
                    usuarioBLL.loginInvalido(usuario);
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.AccesoNoAutorizado, username, "Intento de acceso con passord incorrecta", EventoCriticidadEnum.Media);
                    throw new UsuarioInvalidoException();
                }
                else 
                {
                    usuarioBLL.loginValido(usuario);
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.Login, username, "Login exitoso", EventoCriticidadEnum.Baja);
                }

                Session["Usuario"] = usuario.Nombre;
                Session["Username"] = usuario.Username;
                Session["Apellido"] = usuario.Apellido;
                Response.Redirect("Home.aspx", false);
            }
            catch (Exception ex)
            {
                if (!(ex is UsuarioInvalidoException) && !(ex is UsuarioBloqueadoException))
                    _bitacoraBLL.RegistrarEvento(EventoTipoEnum.ErrorSistema, username, ex.Message, EventoCriticidadEnum.Alta);

                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return;
            }
        }

        protected void newUser_Click(object sender, EventArgs e)
        {

        }

        protected void forgotPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
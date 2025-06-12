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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ExitoRegistro"] != null) {
                Response.Write("<script>alert('"+ Session["ExitoRegistro"].ToString() + "');</script>");
            }
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
                    throw new UsuarioInvalidoException();
                }
                if (usuario.Bloqueado) { 
                    throw new UsuarioBloqueadoException();
                }
                if (!usuario.Password.Equals(password))
                {
                    usuarioBLL.loginInvalido(usuario);
                    throw new UsuarioInvalidoException();
                }
                else { 
                    usuarioBLL.loginValido(usuario);
                }

                Session["Usuario"] = usuario.Nombre;
                Session["Apellido"] = usuario.Apellido;
                Response.Redirect("Home.aspx");

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return;
            }

        }

        protected void newUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx");
        }

        protected void forgotPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace TIF.UI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ingresarButton_Click(object sender, EventArgs e)
        {
            string username = usuarioTextbox.Text.Trim();
            string password = passwordTextbox.Text.Trim();

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            Usuario usuario = usuarioBLL.validarUsuario(username, password); 

            if (usuario == null)
            {
                Response.Write("<script>alert('Usuario no existe o password es incorrecto');</script>");
                return;
            }
            else
            {
                string alert = "<script>alert('Usuario es " + usuario.Nombre + " y apellido es " + usuario.Apellido + "');</script>";
                Response.Write(alert);
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
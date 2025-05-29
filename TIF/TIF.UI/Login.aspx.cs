using BE;
using BE.Permisos;
using BE.Sesion;
using BLL;
using Microsoft.Ajax.Utilities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class Login : Page
    {
        SesionBLL sesionbll = new SesionBLL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ingresarButton_Click(object sender, EventArgs e)
        {
            EncriptadorService encriptador = EncriptadorService.GetEncriptadorService();
            //Encripta el usuario y la contraseña inresados en el formulario
            string username = encriptador.EncriptarAES(usuarioTextbox.Text.Trim());
            string password = encriptador.EncriptarMD5(passwordTextbox.Text.Trim());
            
            try
            {
                var res = sesionbll.LogIn(username, password);
                
                Session["UsuarioNombre"] = SesionBE.ObtenerInstancia.Usuario.Nombre;
                Session["UsuarioApellido"] = SesionBE.ObtenerInstancia.Usuario.Apellido;
                Session["UsuarioCorreo"] = SesionBE.ObtenerInstancia.Usuario.Email;
                Session["UsuarioRol"] = SesionBE.ObtenerInstancia.Usuario.ListaDePermisos[0].Nombre;
                Session["UsuarioPermisos"] = SesionBE.ObtenerInstancia.Usuario.ListaDePermisos; // Guarda los permisos del usuario en la sesión
                
                string mensaje = $"Bienvenido {SesionBE.ObtenerInstancia.Usuario.Nombre} {SesionBE.ObtenerInstancia.Usuario.Apellido}\\nCorreo: {SesionBE.ObtenerInstancia.Usuario.Email}\\nRol: {SesionBE.ObtenerInstancia.Usuario.ListaDePermisos[0].Nombre}";
                string script = $"alert('{mensaje}');";
                ClientScript.RegisterStartupScript(this.GetType(), "loginExitoso", script, true);

                Response.Redirect("Home.aspx");
            }
            catch (LoginExcepcionBE error)
            {
                string mensaje = error.Message.Replace("'", "\\'"); // Escapa comillas simples
                string script = "alert('Nombre de usuario y/o contraseña incorrecta.');";
                ClientScript.RegisterStartupScript(this.GetType(), "alertaLogin", script, true);
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
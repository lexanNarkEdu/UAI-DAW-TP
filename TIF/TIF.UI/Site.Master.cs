using System;
using System.Web.UI;
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
            bool usuarioLogueado = Session["Usuario"] != null;
            NavbarPanel.Visible = usuarioLogueado;

            // Si hay usuario logueado, mostrar su nombre en el navbar
            if (usuarioLogueado)
            {
                string nombre = Session["Usuario"].ToString();
                string apellido = Session["Apellido"]?.ToString() ?? "";
                UsuarioLogueado.Text = $"{nombre} {apellido}".Trim();
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Registrar logout en bitácora si hay usuario en sesión
                if (Session["Username"] != null)
                {
                    string username = Session["Username"].ToString();
                    _bitacoraBLL.RegistrarLogout(username);
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
                Session.Clear();
                Session.Abandon();
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
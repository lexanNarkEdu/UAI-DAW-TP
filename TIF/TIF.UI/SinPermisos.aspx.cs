using System;
using System.Web.UI;

namespace TIF.UI
{
    public partial class SinPermisos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarInformacionUsuario();
            }
        }

        private void CargarInformacionUsuario()
        {
            // Mostrar usuario actual
            string usuarioActual = "Usuario no identificado";
            if (Session["Username"] != null)
            {
                usuarioActual = Session["Username"].ToString();
            }
            ltUsuarioActual.Text = usuarioActual;

            // Mostrar fecha y hora actual
            ltFechaHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }

        protected void btnContactar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contact.aspx");
        }
    }
}
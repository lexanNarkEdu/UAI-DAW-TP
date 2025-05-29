using System;
using System.Web.UI;

namespace TIF.UI
{
    public partial class Home : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario esté logueado
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosUsuario();
            }
        }

        private void CargarDatosUsuario()
        {
            try
            {
                // Obtener datos de la sesión
                string nombre = Session["Usuario"]?.ToString() ?? "";
                string apellido = Session["Apellido"]?.ToString() ?? "";

                // Mostrar nombre completo del usuario
                string nombreCompleto = $"{nombre} {apellido}".Trim();
                if (string.IsNullOrEmpty(nombreCompleto))
                {
                    nombreCompleto = "Usuario";
                }

                NombreUsuario.Text = nombreCompleto;

                // Mostrar fecha y hora actual como "última conexión"
                FechaHoraIngreso.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar información básica
                NombreUsuario.Text = "Usuario";
                FechaHoraIngreso.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // Log del error para debugging
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos del usuario: {ex.Message}");
            }
        }
    }
}
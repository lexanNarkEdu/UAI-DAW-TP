using System.Web.UI;

namespace TIF.UI
{
    internal static class PageAuthorizationExtensions
    {

        /// <summary>
        /// Verifica autorización para una acción específica y redirige si no tiene permisos
        /// </summary>
        /// <param name="page">Página actual</param>
        /// <param name="accionRequerida">Acción requerida</param>
        /// <param name="paginaError">Página de error (opcional)</param>
        /// <returns>True si está autorizado</returns>
        public static bool VerificarAutorizacion(this Page page, string accionRequerida, string paginaError = null)
        {
            if (!AutorizacionHelper.VerificarAutenticacion(page))
                return false;

            if (!AutorizacionHelper.PuedeAcceder(page.Session, accionRequerida))
            {
                if (!string.IsNullOrEmpty(paginaError))
                {
                    page.Response.Redirect(paginaError, false);
                }
                else
                {
                    // Mostrar mensaje de error y redirigir al home
                    page.ClientScript.RegisterStartupScript(
                        page.GetType(),
                        "AccesoDenegado",
                        "alert('No tienes permisos para acceder a esta funcionalidad.'); window.location='Home.aspx';",
                        true);
                }
                //page.Context.ApplicationInstance.CompleteRequest();
                return false;
            }
            return true;
        }
    }
}
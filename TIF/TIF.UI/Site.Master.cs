using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBitacora_Click(object sender, EventArgs e)
        {
            var permisos = Session["UsuarioPermisos"] as IList<BE.Permisos.PermisoBE>;
            bool tienePermiso = false;

            if (permisos != null)
            {
                // Función recursiva para buscar el permiso en la jerarquía
                bool BuscarPermiso(IList<BE.Permisos.PermisoBE> lista, string nombre)
                {
                    foreach (var permiso in lista)
                    {
                        if (permiso.Nombre == nombre)
                            return true;
                        if (permiso.ListaDeHijos != null && BuscarPermiso(permiso.ListaDeHijos, nombre))
                            return true;
                    }
                    return false;
                }

                tienePermiso = BuscarPermiso(permisos, "GestionarBitacoraEventos");
            }

            if (tienePermiso)
            {
                Response.Redirect("~/Bitacora.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                string script = "alert('No tiene permisos para acceder a Bitacora. Será redirigido a la página principal.'); window.location='Home.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "sinPermisoBitacora", script, true);
            }
        }

        protected void btnABMUsuarios_Click(object sender, EventArgs e)
        {
            var permisos = Session["UsuarioPermisos"] as IList<BE.Permisos.PermisoBE>;
            bool tienePermiso = false;

            if (permisos != null)
            {
                // Función recursiva para buscar el permiso en la jerarquía
                bool BuscarPermiso(IList<BE.Permisos.PermisoBE> lista, string nombre)
                {
                    foreach (var permiso in lista)
                    {
                        if (permiso.Nombre == nombre)
                            return true;
                        if (permiso.ListaDeHijos != null && BuscarPermiso(permiso.ListaDeHijos, nombre))
                            return true;
                    }
                    return false;
                }

                tienePermiso = BuscarPermiso(permisos, "ABMUsuarios");
            }

            if (tienePermiso)
            {
                Response.Redirect("~/Bitacora.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                string script = "alert('No tiene permisos para acceder a ABM Usuarios. Será redirigido a la página principal.'); window.location='Home.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "sinPermisoBitacora", script, true);
            }
        }

        protected void btnProductos_Click(object sender, EventArgs e)
        {
            var permisos = Session["UsuarioPermisos"] as IList<BE.Permisos.PermisoBE>;
            bool tienePermiso = false;

            if (permisos != null)
            {
                // Función recursiva para buscar el permiso en la jerarquía
                bool BuscarPermiso(IList<BE.Permisos.PermisoBE> lista, string nombre)
                {
                    foreach (var permiso in lista)
                    {
                        if (permiso.Nombre == nombre)
                            return true;
                        if (permiso.ListaDeHijos != null && BuscarPermiso(permiso.ListaDeHijos, nombre))
                            return true;
                    }
                    return false;
                }

                tienePermiso = BuscarPermiso(permisos, "VerProductos");
            }

            if (tienePermiso)
            {
                Response.Redirect("~/Bitacora.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                string script = "alert('No tiene permisos para acceder a Productos. Será redirigido a la página principal.'); window.location='Home.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "sinPermisoBitacora", script, true);
            }
        }
    }
}
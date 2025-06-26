using System;
using System.Collections.Generic;
using System.Web.UI;
using BE.Permisos;
using BLL;
using TIF.UI.Helpers;

namespace TIF.UI
{
    public partial class SiteMaster : MasterPage
    {
        private readonly BitacoraBLL _bitacoraBLL;
        private readonly RoLBLL _roLBLL;

        public SiteMaster()
        {
            _bitacoraBLL = new BitacoraBLL();
            _roLBLL = new RoLBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Controlar visibilidad del navbar basado en la sesión
            bool usuarioLogueado = Session["Username"] != null;
            if (Request.Path.EndsWith("/Login"))
                NavbarPanel.Visible = usuarioLogueado;

            // Si hay usuario logueado, mostrar su nombre en el navbar
            if (usuarioLogueado)
            {
                Login.Visible = false;
                string nombre = Session["UsuarioNombre"].ToString();
                string apellido = Session["UsuarioApellido"]?.ToString() ?? "";
                UsuarioLogueado.Text = $"{nombre} {apellido}".Trim();
                
                if (Session["UsuarioRol"].ToString() == "Cliente")
                    liHome.Visible = false;
                var permisos = Session["UsuarioPermisos"] as List<PermisoBE>;
                liBitacora.Visible = _roLBLL.EstaPermisoEnRol(permisos, PermisoToRouteHelper.ToPermiso(aBitacora.HRef));
                liABMUsuarios.Visible = _roLBLL.EstaPermisoEnRol(permisos, PermisoToRouteHelper.ToPermiso(aABMUsuarios.HRef));
                liProductos.Visible = _roLBLL.EstaPermisoEnRol(permisos, PermisoToRouteHelper.ToPermiso(aProductos.HRef));
                liABMProductos.Visible = _roLBLL.EstaPermisoEnRol(permisos, PermisoToRouteHelper.ToPermiso(aABMProductos.HRef));
            }
            else
            {
                liHome.Visible = false;
                LogoutButton.Visible = false;
                liBitacora.Visible = false;
                liABMUsuarios.Visible = false;
                liProductos.Visible = false;
                liABMProductos.Visible = false;
            }
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
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
                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
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
                Response.Redirect("~/ABMUsuarios.aspx", false);
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
                Response.Redirect("~/Tienda.aspx", false);
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
using BE.Permisos;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using TIF.UI.Helpers;

namespace TIF.UI
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // Excluir páginas que no requieren validación
            string[] paginasExcluidas = { "/Default", "/Tienda", "/About", "/Contact", "/Login", "/Registro", "/SinPermisos", "/Error" };
            if (paginasExcluidas.Any(p => Request.Path.EndsWith(p)))
                return;

            if (Request.Path.EndsWith("/"))
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            //Revisar si hay sesion disponible
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null){
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Verificar si usuario está autenticado
            if (Session["Username"] == null) 
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Obtener lista de permisos del usuario desde Session
            var permisosUsuario = Session["UsuarioPermisos"] as List<PermisoBE>;
            if (permisosUsuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            //No tiene permisos, entiendo
            if (Request.Path.EndsWith("/Home") || Request.Path.EndsWith("/Contact") || Request.Path.EndsWith("/About"))
                return;

            var autorizado = new RoLBLL().EstaPermisoEnRol(permisosUsuario, PermisoToRouteHelper.ToPermiso(Request.Path));

            if (!autorizado)
            {
                Response.Redirect("~/SinPermisos.aspx");
            }
        }
    }
}
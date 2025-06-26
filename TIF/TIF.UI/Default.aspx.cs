using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace TIF.UI
{
    public partial class Default : Page
    {
        private readonly ProductoBLL _productoBLL = new ProductoBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
            }
        }

        public enum CondicionEnum
        {
            Nuevo = 1,
            Usado = 2
        }


        private void CargarProductos()
        {
            var listaBanners = _productoBLL.ObtenerBanner();
            lvProductoBanner.DataSource = listaBanners;
            lvProductoBanner.DataBind();

            var listaDestacados = _productoBLL.ObtenerDestacados();
            lvProductos.DataSource = listaDestacados;
            lvProductos.DataBind();

            var listaUltimosIngresos = _productoBLL.ObtenerUltimosIngresos();
            lvUltimosIngresos.DataSource = listaUltimosIngresos;
            lvUltimosIngresos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Construir URL con parámetros de filtro
            string url = "Tienda.aspx?";
            List<string> parametros = new List<string>();

            if (!string.IsNullOrEmpty(idDataBuscarProducto.Text.Trim()))
            {
                parametros.Add($"query={HttpUtility.UrlEncode(idDataBuscarProducto.Text.Trim())}");
            }

            if (parametros.Count > 0)
            {
                url += string.Join("&", parametros);
            }

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Convierte el ID de condición en texto legible.
        /// </summary>
        protected string GetCondicionTexto(object condicionIdObj)
        {
            if (!int.TryParse(condicionIdObj?.ToString(), out int condicionId))
                return "Desconocido";

            switch ((CondicionEnum)condicionId)
            {
                case CondicionEnum.Nuevo:
                    return "Nuevo";
                case CondicionEnum.Usado:
                    return "Usado";
                default:
                    return "Desconocido";
            }
        }

    }
}
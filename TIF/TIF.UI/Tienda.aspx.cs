using BE;
using BLL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class Tienda : Page
    {
        private readonly ProductoBLL _productoBLL = new ProductoBLL();
        private readonly CategoriaBLL _categoriaBLL = new CategoriaBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario esté logueado
            //if (Session["Username"] == null && Session["UsuarioPermisos"] == null)
            //{
            //    Response.Redirect("Login.aspx", false);
            //    Context.ApplicationInstance.CompleteRequest();
            //    return;
            //}

            if (!IsPostBack)
            {
                CargarDropdowns();
                string query = Request.QueryString["query"] ?? null;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var lista = _productoBLL.ObtenerPorNombre(query.Trim());
                    lvProductos.DataSource = lista;
                    lvProductos.DataBind();
                    //lblcantidadProductosResultado.Text = $"{lista.Count} productos(s) encontrado(s)";
                }
                else
                {
                    CargarProductos();
                }
            }
        }

        public enum CondicionEnum
        {
            Nuevo = 1,
            Usado = 2
        }

        private void CargarDropdowns()
        {
            // Categorías para filtro y para agregar
            var categorias = _categoriaBLL.ObtenerTodas();
            ddlFiltroCategorias.DataSource = categorias;
            ddlFiltroCategorias.DataTextField = "Nombre";
            ddlFiltroCategorias.DataValueField = "CategoriaId";
            ddlFiltroCategorias.DataBind();
            ddlFiltroCategorias.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas", ""));

            // Condiciones (tabla condicion)
            ddlFiltroCondiciones.Items.Clear();
            foreach (CondicionEnum cond in Enum.GetValues(typeof(CondicionEnum)))
            {
                // Texto = "Nuevo"/"Usado", Value = "1"/"2"
                ddlFiltroCondiciones.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        cond.ToString(),
                        ((int)cond).ToString()
                    )
                );
            }
            // Opcional: insertar item vacío o “Seleccione…”
            ddlFiltroCondiciones.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione condición", ""));
        }

        private void CargarProductos()
        {
            int? categoriaId = null;
            int? condicionId = null;

            if (int.TryParse(ddlFiltroCategorias.SelectedValue, out int cat))
                categoriaId = cat;
            if (int.TryParse(ddlFiltroCondiciones.SelectedValue, out int cond))
                condicionId = cond;

            // Llama a un nuevo overload que acepte ambos filtros
            var lista = _productoBLL.ObtenerPorCategoriaYCondicion(categoriaId, condicionId, true);
            lvProductos.DataSource = lista;
            lvProductos.DataBind();
            //lblcantidadProductosResultado.Text = $"{lista.Count} productos(s) encontrado(s)";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string query = Request.QueryString["query"] ?? null;

            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(idDataBuscarProducto.Text))
            {
                CargarProductos();
            }
            else
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
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tienda.aspx", false);
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
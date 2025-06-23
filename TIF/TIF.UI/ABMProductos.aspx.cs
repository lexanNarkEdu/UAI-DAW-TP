using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace TIF.UI
{
    public partial class ABMProductos : System.Web.UI.Page
    {
        private readonly ProductoBLL _productoBLL = new ProductoBLL();
        private readonly CategoriaBLL _categoriaBLL = new CategoriaBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario esté logueado
            if (Session["Username"] == null && Session["UsuarioPermisos"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarDropdowns();
                CargarProductos();
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


            var lista = _productoBLL.ObtenerPorCategoriaYCondicion(categoriaId, condicionId);
            gvProductos.DataSource = lista;
            gvProductos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlFiltroCategorias.SelectedIndex = 0;
            ddlFiltroCondiciones.SelectedIndex = 0;
            CargarProductos();
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

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var prod = _productoBLL.ObtenerPorId(id);

                hfProductoId.Value = prod.ProductoId.ToString();
                txtNombre.Text = prod.Nombre;
                txtPrecio.Text = prod.Precio.ToString("F2");
                txtStock.Text = prod.Stock.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#productoModal').modal('show');", true);
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                _productoBLL.Eliminar(id);
                CargarProductos();
            }
        }

        private void LimpiarModal()
        {
            hfProductoId.Value = "";
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#productoModal').modal('show');", true);
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var producto = new Producto()
            {
                ProductoId = string.IsNullOrEmpty(hfProductoId.Value) ? 0 : Convert.ToInt32(hfProductoId.Value),
                Nombre = txtNombre.Text,
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Stock = Convert.ToInt32(txtStock.Text),
                UsuarioCreacion = Session["username"].ToString()
            };

            if (producto.ProductoId == 0)
                _productoBLL.Agregar(producto);
            else
                _productoBLL.Modificar(producto);

            CargarProductos();
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var productoId = DataBinder.Eval(e.Row.DataItem, "ProductoId").ToString();
                string url = $"DetalleProducto.aspx?id={productoId}";

                // Hace toda la fila clickeable
                e.Row.Attributes["onclick"] = $"window.location='{url}'";
                e.Row.Style["cursor"] = "pointer";
            }
        }
    }
}
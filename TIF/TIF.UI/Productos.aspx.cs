using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class Productos : Page
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

            // Llama a un nuevo overload que acepte ambos filtros
            var lista = _productoBLL.ObtenerPorCategoriaYCondicion(categoriaId, condicionId);
            lvProductos.DataSource = lista;
            lvProductos.DataBind();
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
    }
}
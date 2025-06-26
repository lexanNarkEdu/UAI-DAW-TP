using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            // Llama a un nuevo overload que acepte ambos filtros
            var lista = _productoBLL.ObtenerTodosActivos();
            lvProductos.DataSource = lista;
            lvProductos.DataBind();
            //lblcantidadProductosResultado.Text = $"{lista.Count} evento(s) encontrado(s)";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
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
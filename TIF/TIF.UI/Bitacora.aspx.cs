using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace TIF.UI
{
    public partial class Bitacora : Page
    {
        private readonly BitacoraBLL _bitacoraBLL;

        public Bitacora()
        {
            _bitacoraBLL = new BitacoraBLL();
        }

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
                CargarFiltrosDesdeQueryString();
                CargarDatos();
            }
        }

        private void CargarFiltrosDesdeQueryString()
        {
            // Cargar valores de los filtros desde la QueryString
            txtUsuario.Text = Request.QueryString["usuario"] ?? "";

            string tipoEvento = Request.QueryString["tipo"] ?? "";
            if (!string.IsNullOrEmpty(tipoEvento))
            {
                ddlTipoEvento.SelectedValue = tipoEvento;
            }

            string fechaDesde = Request.QueryString["fechaDesde"] ?? "";
            if (!string.IsNullOrEmpty(fechaDesde) && DateTime.TryParse(fechaDesde, out DateTime fDesde))
            {
                txtFechaDesde.Text = fDesde.ToString("yyyy-MM-dd");
            }

            string fechaHasta = Request.QueryString["fechaHasta"] ?? "";
            if (!string.IsNullOrEmpty(fechaHasta) && DateTime.TryParse(fechaHasta, out DateTime fHasta))
            {
                txtFechaHasta.Text = fHasta.ToString("yyyy-MM-dd");
            }
        }

        private void CargarDatos()
        {
            try
            {
                List<Evento> eventos = ObtenerEventosFiltrados();

                if (eventos != null && eventos.Count > 0)
                {
                    // Ordenar por fecha descendente (más recientes primero)
                    eventos = eventos.OrderByDescending(e => e.FechaHora).ToList();

                    gvBitacora.DataSource = eventos;
                    gvBitacora.DataBind();

                    lblResultados.Text = $"{eventos.Count} evento(s) encontrado(s)";
                    pnlResultados.Visible = true;
                    pnlSinResultados.Visible = false;
                }
                else
                {
                    gvBitacora.DataSource = null;
                    gvBitacora.DataBind();

                    lblResultados.Text = "0 eventos encontrados";
                    pnlResultados.Visible = false;
                    pnlSinResultados.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Mostrar error en la interfaz
                lblResultados.Text = "Error al cargar datos";
                pnlResultados.Visible = false;
                pnlSinResultados.Visible = true;

                // Log del error para debugging
                System.Diagnostics.Debug.WriteLine($"Error al cargar bitácora: {ex.Message}");
            }
        }

        private List<Evento> ObtenerEventosFiltrados()
        {
            try
            {
                List<Evento> eventos;

                // Obtener valores de los filtros
                string usuario = txtUsuario.Text.Trim();
                string tipoEvento = ddlTipoEvento.SelectedValue;
                string fechaDesdeStr = txtFechaDesde.Text;
                string fechaHastaStr = txtFechaHasta.Text;

                // Si no hay filtros, obtener todos los eventos
                if (string.IsNullOrEmpty(usuario) &&
                    string.IsNullOrEmpty(tipoEvento) &&
                    string.IsNullOrEmpty(fechaDesdeStr) &&
                    string.IsNullOrEmpty(fechaHastaStr))
                {
                    eventos = _bitacoraBLL.ObtenerTodos();
                }
                else
                {
                    // Aplicar filtros específicos
                    eventos = _bitacoraBLL.ObtenerTodos();

                    // Filtrar por usuario
                    if (!string.IsNullOrEmpty(usuario))
                    {
                        eventos = eventos.Where(e => e.UsuarioUsername != null &&
                                                   e.UsuarioUsername.ToLower().Contains(usuario.ToLower())).ToList();
                    }

                    // Filtrar por tipo de evento
                    if (!string.IsNullOrEmpty(tipoEvento))
                    {
                        if (Enum.TryParse<EventoTipoEnum>(tipoEvento, out EventoTipoEnum tipo))
                        {
                            eventos = eventos.Where(e => e.Nombre == tipo).ToList();
                        }
                    }

                    // Filtrar por rango de fechas
                    if (!string.IsNullOrEmpty(fechaDesdeStr) && DateTime.TryParse(fechaDesdeStr, out DateTime fechaDesde))
                    {
                        eventos = eventos.Where(e => e.FechaHora.Date >= fechaDesde.Date).ToList();
                    }

                    if (!string.IsNullOrEmpty(fechaHastaStr) && DateTime.TryParse(fechaHastaStr, out DateTime fechaHasta))
                    {
                        eventos = eventos.Where(e => e.FechaHora.Date <= fechaHasta.Date).ToList();
                    }
                }

                return eventos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener eventos filtrados: {ex.Message}");
                return new List<Evento>();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Construir URL con parámetros de filtro
            string url = "Bitacora.aspx?";
            List<string> parametros = new List<string>();

            if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()))
            {
                parametros.Add($"usuario={HttpUtility.UrlEncode(txtUsuario.Text.Trim())}");
            }

            if (!string.IsNullOrEmpty(ddlTipoEvento.SelectedValue))
            {
                parametros.Add($"tipo={HttpUtility.UrlEncode(ddlTipoEvento.SelectedValue)}");
            }

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                parametros.Add($"fechaDesde={HttpUtility.UrlEncode(txtFechaDesde.Text)}");
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                parametros.Add($"fechaHasta={HttpUtility.UrlEncode(txtFechaHasta.Text)}");
            }

            if (parametros.Count > 0)
            {
                url += string.Join("&", parametros);
            }
            else
            {
                url = "Bitacora.aspx"; // Sin parámetros
            }

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Bitacora.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Métodos auxiliares para el GridView
        protected string GetTipoEventoTexto(string tipoEvento)
        {
            switch (tipoEvento)
            {
                case "Login": return "Inicio de Sesión";
                case "Logout": return "Cierre de Sesión";
                case "CrearUsuario": return "Crear Usuario";
                case "ModificarUsuario": return "Modificar Usuario";
                case "EliminarUsuario": return "Eliminar Usuario";
                case "CambiarPassword": return "Cambiar Contraseña";
                case "AccesoNoAutorizado": return "Acceso No Autorizado";
                case "ErrorSistema": return "Error del Sistema";
                default: return tipoEvento;
            }
        }

        protected string GetTipoEventoClass(string tipoEvento)
        {
            switch (tipoEvento)
            {
                case "Login": return "success";
                case "Logout": return "info";
                case "CrearUsuario":
                case "ModificarUsuario": return "primary";
                case "EliminarUsuario": return "warning";
                case "CambiarPassword": return "secondary";
                case "AccesoNoAutorizado":
                case "ErrorSistema": return "danger";
                default: return "light";
            }
        }

        protected string GetCriticidadTexto(int criticidadId)
        {
            switch (criticidadId)
            {
                case 1: return "Baja";
                case 2: return "Media";
                case 3: return "Alta";
                default: return "Desconocida";
            }
        }

        protected string GetCriticidadClass(int criticidadId)
        {
            switch (criticidadId)
            {
                case 1: return "success";  // Baja - Verde
                case 2: return "warning";  // Media - Amarillo
                case 3: return "danger";   // Alta - Rojo
                default: return "secondary";
            }
        }
    }
}
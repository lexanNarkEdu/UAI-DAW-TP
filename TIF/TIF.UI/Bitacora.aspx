<%@ Page Title="Bitácora del Sistema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="TIF.UI.Bitacora" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="mb-4">
                <i class="bi bi-journal-text"></i> Bitácora del Sistema
            </h2>
            <p class="text-muted mb-4">
                Consulta y filtra los eventos registrados en el sistema. 
                Puedes aplicar filtros individuales o combinarlos para obtener resultados más específicos.
            </p>
        </div>
    </div>

    <!-- Panel de Filtros -->
    <div class="card mb-4">
        <div class="card-header">
            <h5 class="mb-0">
                <i class="bi bi-funnel"></i> Filtros de Búsqueda
            </h5>
        </div>
        <div class="card-body">
            <form method="get" action="Bitacora.aspx">
                <div class="row">
                    <div class="col-md-3">
                        <div class="mb-3">
                            <label for="txtUsuario" class="form-label">Usuario</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" 
                                         placeholder="Filtrar por usuario..." />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="mb-3">
                            <label for="ddlTipoEvento" class="form-label">Tipo de Evento</label>
                            <asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="form-select">
                                <asp:ListItem Value="" Text="-- Todos los tipos --"></asp:ListItem>
                                <asp:ListItem Value="Login" Text="Inicio de Sesión"></asp:ListItem>
                                <asp:ListItem Value="Logout" Text="Cierre de Sesión"></asp:ListItem>
                                <asp:ListItem Value="CrearUsuario" Text="Crear Usuario"></asp:ListItem>
                                <asp:ListItem Value="ModificarUsuario" Text="Modificar Usuario"></asp:ListItem>
                                <asp:ListItem Value="EliminarUsuario" Text="Eliminar Usuario"></asp:ListItem>
                                <asp:ListItem Value="CambiarPassword" Text="Cambiar Contraseña"></asp:ListItem>
                                <asp:ListItem Value="AccesoNoAutorizado" Text="Acceso No Autorizado"></asp:ListItem>
                                <asp:ListItem Value="ErrorSistema" Text="Error del Sistema"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="mb-3">
                            <label for="txtFechaDesde" class="form-label">Fecha Desde</label>
                            <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" 
                                         TextMode="Date" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="mb-3">
                            <label for="txtFechaHasta" class="form-label">Fecha Hasta</label>
                            <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" 
                                         TextMode="Date" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="mb-3">
                            <label class="form-label">&nbsp;</label>
                            <div class="d-grid">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                            CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Filtros" 
                                    CssClass="btn btn-outline-secondary btn-sm" OnClick="btnLimpiar_Click" />
                    </div>
                </div>
            </form>
        </div>
    </div>

    <!-- Resultados -->
    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="bi bi-list-ul"></i> Resultados
            </h5>
            <asp:Label ID="lblResultados" runat="server" CssClass="badge bg-secondary" />
        </div>
        <div class="card-body">
            <asp:Panel ID="pnlSinResultados" runat="server" Visible="false">
                <div class="alert alert-info text-center" role="alert">
                    <i class="bi bi-info-circle"></i>
                    No se encontraron eventos que coincidan con los filtros aplicados.
                    <br />
                    <small>Intenta modificar los criterios de búsqueda o limpiar los filtros.</small>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlResultados" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvBitacora" runat="server" 
                                  CssClass="table table-striped table-hover table-sm"
                                  AutoGenerateColumns="false"
                                  EmptyDataText="No hay eventos para mostrar."
                                  GridLines="None">
                        <Columns>
<%--                            <asp:BoundField DataField="Id" HeaderText="ID" 
                                            ItemStyle-CssClass="text-center" 
                                            HeaderStyle-CssClass="text-center" />--%>
                            
                            <asp:BoundField DataField="FechaHora" HeaderText="Fecha y Hora" 
                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                            ItemStyle-CssClass="text-nowrap" />
                            
                            <asp:BoundField DataField="UsuarioUsername" HeaderText="Usuario" />
                            
                            <asp:TemplateField HeaderText="Tipo de Evento">
                                <ItemTemplate>
                                    <span class="badge bg-<%# GetTipoEventoClass(Eval("Nombre").ToString()) %>">
                                        <%# GetTipoEventoTexto(Eval("Nombre").ToString()) %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" 
                                            ItemStyle-CssClass="text-wrap" />
                            
                            <asp:TemplateField HeaderText="Criticidad">
                                <ItemTemplate>
                                    <span class="badge bg-<%# GetCriticidadClass((int)Eval("CriticidadId")) %>">
                                        <%# GetCriticidadTexto((int)Eval("CriticidadId")) %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="table-dark" />
                        <EmptyDataRowStyle CssClass="text-center text-muted" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
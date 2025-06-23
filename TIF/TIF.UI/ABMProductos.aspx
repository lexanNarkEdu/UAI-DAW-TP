<%@ Page Title="Gestión de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMProductos.aspx.cs" Inherits="TIF.UI.ABMProductos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="row">
            <div class="col-md-12">
                <h2 class="mb-4">
                    <i class="bi bi-journal-text"></i> <%:Title%>
                </h2>
                <p class="text-muted mb-4">
                    Gestión de productos. 
                Puedes aplicar filtros individuales o combinarlos para obtener resultados más específicos.
                </p>
            </div>
        </div>

        <!-- 2) SECCIÓN FILTRO POR CATEGORÍA -->
        <asp:Panel ID="pnlFiltro" runat="server" CssClass="card align-items-center" Style="display: block;">
            <div class="card-header">
                <h5 class="mb-0">
                    <i class="bi bi-funnel"></i>Filtros de Búsqueda
                </h5>
            </div>
            <div class="card-body">
                <section class="card-body">
                    <div class="row g-3 mb-4">
                        <!-- Filtro por Categoría -->
                        <div class="col-auto d-flex align-items-center">
                            <asp:Label ID="lblFiltroCategoria" runat="server" Text="Categoría:"
                                AssociatedControlID="ddlFiltroCategorias" CssClass="me-2" />
                            <asp:DropDownList ID="ddlFiltroCategorias" runat="server"
                                CssClass="form-select" />
                        </div>
                        <!-- Filtro por Condición -->
                        <div class="col-auto d-flex align-items-center">
                            <asp:Label ID="lblFiltroCondicion" runat="server" Text="Condición:"
                                AssociatedControlID="ddlFiltroCondiciones" CssClass="me-2" />
                            <asp:DropDownList ID="ddlFiltroCondiciones" runat="server"
                                CssClass="form-select" />
                            <!--  AutoPostBack="true" OnSelectedIndexChanged="Filtros_SelectedIndexChanged" -->
                        </div>
                        <div class="col-auto">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" CausesValidation="false" />
                        </div>
                        <!-- Botón para limpiar ambos filtros -->
                        <div class="col-auto">
                            <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnLimpiarFiltros_Click" CausesValidation="false" />
                        </div>
                    </div>
                </section>

                <!-- 3) LISTADO DE PRODUCTOS -->
                <asp:GridView ID="gvProductos" runat="server" CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="False" OnRowCommand="gvProductos_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ProductoId" HeaderText="ID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Stock" HeaderText="Stock" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("ProductoId") %>'
                                    Text="Editar" CssClass="btn btn-sm btn-primary" />
                                <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("ProductoId") %>'
                                    Text="Eliminar" CssClass="btn btn-sm btn-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>





                </section>
            </div>

        </asp:Panel>

    </main>




    <!-- Modal Bootstrap para Alta/Modificación -->
    <div class="modal fade" id="productoModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="tituloModal">Producto</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfProductoId" runat="server" />
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control mb-2" placeholder="Nombre" />
                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control mb-2" placeholder="Precio" />
                    <asp:TextBox ID="txtStock" runat="server" CssClass="form-control mb-2" placeholder="Stock" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

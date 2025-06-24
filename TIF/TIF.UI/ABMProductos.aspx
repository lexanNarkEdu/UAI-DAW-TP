<%@ Page Title="Gestión de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMProductos.aspx.cs" Inherits="TIF.UI.ABMProductos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="row">
            <div class="col-md-12">
                <h2 class="mb-4">
                    <i class="bi bi-journal-text pe-3"></i><%:Title%>
                </h2>
                <p class="text-muted mb-4">
                    Gestión de productos. 
                Puedes aplicar filtros individuales o combinarlos para obtener resultados más específicos.
                </p>
            </div>
        </div>

        <!-- 1) SECCIÓN NUEVO PRODUCTO    -->
        <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-lg btn-success mb-4" Text="Nuevo Producto" OnClick="btnNuevo_Click" Visible="true" />


        <!-- 2) SECCIÓN FILTRO POR CATEGORÍA -->
        <asp:Panel ID="pnlFiltro" runat="server" CssClass="card mb-4 align-items-center" Style="display: block;">
            <section class="card-header">
                <h5 class="mb-0">
                    <i class="bi bi-funnel pe-2"></i>Filtros de Búsqueda
                </h5>
            </section>
            <section class="card-body">
                <div class="row g-3 mb-4">
                    <!-- Filtro por Nombre y Categorias 
                    <div class="col-auto d-flex align-items-center">
                        <asp:Label ID="lblFiltroNombre" runat="server" Text="Busca Producto:" AssociatedControlID="ddlFiltroNombre" CssClass="me-2" />
                        <asp:TextBox ID="ddlFiltroNombre" runat="server" CssClass="form-control" />
                    </div>
                     -->
                    <!-- Filtro por Categoría -->
                    <div class="col-auto d-flex align-items-center">
                        <asp:Label ID="lblFiltroCategoria" runat="server" Text="Categoría:" AssociatedControlID="ddlFiltroCategorias" CssClass="me-2" />
                        <asp:DropDownList ID="ddlFiltroCategorias" runat="server" CssClass="form-select" />
                    </div>
                    <!-- Filtro por Condición -->
                    <div class="col-auto d-flex align-items-center">
                        <asp:Label ID="lblFiltroCondicion" runat="server" Text="Condición:" AssociatedControlID="ddlFiltroCondiciones" CssClass="me-2" />
                        <asp:DropDownList ID="ddlFiltroCondiciones" runat="server" CssClass="form-select" />
                    </div>
                    <!-- Botón para buscar con los filtros seleccionados -->
                    <div class="col-auto">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" CausesValidation="false" />
                    </div>
                    <!-- Botón para limpiar ambos filtros -->
                    <br />
                    <div class="col-auto">
                        <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnLimpiarFiltros_Click" CausesValidation="false" />
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlResultado" runat="server" CssClass="card align-items-center" Style="display: block;">
            <!-- 3) LISTADO DE PRODUCTOS -->
            <section class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-list-ul pe-2"></i>Resultados
                </h5>
                <asp:Label ID="lblcantidadProductosResultado" runat="server" CssClass="badge bg-secondary" />
            </section>
            <section class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvProductos" runat="server" CssClass="table table-bordered table-striped table-hover table-vertical-align"
                        AutoGenerateColumns="False" OnRowCommand="gvProductos_RowCommand" OnRowDataBound="gvProductos_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Etiqueta">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-center">
                                        <i class='<%# Convert.ToInt32(Eval("Stock")) == 0 ? "bi bi-x-circle-fill  text-warning fw-bold p-1" : "" %>' title="Producto sin Stock"></i>
                                        <i class='<%# Convert.ToBoolean(Eval("Activo")) ? "" : "bi bi-stop-circle-fill text-danger p-1" %>' title="Producto Inactivo o Eliminado"></i>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProductoId" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" ItemStyle-CssClass="text-nowrap" />
                            <asp:BoundField DataField="Stock" HeaderText="Stock" />
                            <asp:BoundField DataField="CategoriaId" HeaderText="Categoría" />
                            <asp:TemplateField HeaderText="Condición">
                                <ItemTemplate>
                                    <%# GetCondicionTexto(Eval("CondicionId")) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Activo" HeaderText="Activo" />
                            <asp:TemplateField HeaderText="Foto">
                                <ItemTemplate>
                                    <div class="thumbnail-container">
                                        <img src='<%# Eval("Foto") %>' alt='<%# Eval("Nombre") %>' class="thumbnail-img" />
                                        <div class="tooltip-img">
                                            <img src='<%# Eval("Foto") %>' alt="Vista Ampliada" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- 
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-center">
                                        <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("ProductoId") %>'
                                            CssClass="btn btn-sm btn-primary m-1" ToolTip="Editar">
                                                <i class="bi bi-pencil-square"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                            CssClass="btn btn-sm btn-danger m-1" ToolTip="Eliminar">
                                                 <i class="bi bi-trash-fill"></i>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            --%>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
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

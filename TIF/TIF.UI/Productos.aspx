<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TIF.UI.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="row">
            <div class="col-md-12">
                <h2 class="mb-4">
                    <i class="bi bi-journal-text pe-3"></i><%:Title%>
                </h2>
                <p class="text-muted mb-4">
                    Consulta y filtra todos los productos. 
                Puedes aplicar filtros individuales o combinarlos para obtener resultados más específicos.
                </p>
            </div>
        </div>

        <!-- 2) SECCIÓN FILTRO POR CATEGORÍA -->
        <asp:Panel ID="pnlFiltro" runat="server" CssClass="card align-items-center" Style="display: block;">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-funnel pe-2"></i>Filtros de Búsqueda
                </h5>
                <asp:Label ID="lblcantidadProductosResultado" runat="server" CssClass="badge bg-secondary" />
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
                <section class="card-body mt-2">
                    <asp:ListView ID="lvProductos" runat="server" ItemPlaceholderID="itemPlaceholder">
                        <LayoutTemplate>
                            <div class="row">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="col-sm-3 col-md-3 mb-3">
                                <a href="DetalleProducto.aspx?id=<%# Eval("ProductoId") %>"
                                    class="text-decoration-none text-dark d-block h-100">
                                    <div class="card h-100 shadow product-card">
                                        <div class="card-img-top p-4 d-flex" style="align-items: center; height: inherit;">
                                            <img src='<%# Eval("Foto") %>' class="img-fluid d-block mx-auto" alt='<%# Eval("Nombre") %>' style="height: fit-content;" />
                                        </div>
                                        <div class="card-body d-flex flex-column">
                                            <h5 class="card-title text-secondary mb-2"><%# Eval("Nombre") %></h5>
                                            <p class="h4 mb-1">$<%# Eval("Precio", "{0:N0}") %></p>
                                            <small class="text-muted mb-2">en 6 cuotas de $$<%# (Convert.ToDecimal(Eval("Precio")) * 1.15m / 6).ToString("N2") %></small>
                                            <span class="bg-secondary text-white rounded-pill d-inline-block px-2 py-1 ms-auto" style="width: fit-content;">
                                                <%# GetCondicionTexto(Eval("CondicionId")) %>
                                            </span>
                                        </div>
                                    </div>
                                </a>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No hay productos para mostrar.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </section>
            </div>

        </asp:Panel>

    </main>
</asp:Content>

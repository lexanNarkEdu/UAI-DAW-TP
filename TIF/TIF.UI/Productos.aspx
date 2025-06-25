<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TIF.UI.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="carouselProductosDestacados" class="carousel slide mt-1" data-bs-ride="carousel">
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="https://www.gezatek.com.ar/uploads/16-06-2025-12-06-29-27-05-2025-03-05-11-Banner-kingston.png" class="d-block w-100" alt="Notebook Gamer">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-10 rounded">
                    <h5>Notebook Gamer</h5>
                    <p>$ 650.000</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="https://www.gezatek.com.ar/uploads/17-06-2025-03-06-47-banner-principal.png" class="d-block w-100" alt="Mouse Gamer">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-10 rounded">
                    <h5>Mouse Logitech G203</h5>
                    <p>$ 12.999</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="https://www.gezatek.com.ar/uploads/17-06-2025-03-06-33-banner-principal-2.png" class="d-block w-100" alt="Monitor Samsung">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-10 rounded">
                    <article style="width:50%;">
                        <h5>Monitor Samsung 24"</h5>
                        <p>$ 98.000</p>
                    </article>
                </div>
            </div>
        </div>
        <button class="carousel-control-prev" type="button" data-bs-target="#carouselProductosDestacados" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Anterior</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#carouselProductosDestacados" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Siguiente</span>
        </button>
    </div>
    <div class="container" style="margin-top: -1.5rem;">
        <div class="row justify-content-center">
            <div class="col-md-4">
                <div class="input-group justify-content-center bg-color-tif p-2 rounded">
                    <span class="input-group-text border-end-0">
                        <i class="bi bi-search text-secondary"></i>
                    </span>
                    <input
                        type="text"
                        class="form-control border-start-0"
                        placeholder="Buscá tu producto"
                        aria-label="Buscar producto">
                    <button class="btn btn-primary" type="submit">Buscar!</button>
                </div>
            </div>
        </div>
    </div>

    <main aria-labelledby="title" class="container body-content">
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

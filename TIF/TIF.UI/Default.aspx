<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TIF.UI.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="carouselProductosDestacados" class="carousel slide mt-1" data-bs-ride="carousel">

        <asp:ListView ID="lvProductoBanner" runat="server" ItemPlaceholderID="itemPlaceholder">
            <LayoutTemplate>
                <div class="carousel-inner">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div class='carousel-item <%# Container.DataItemIndex == 0 ? "active" : "" %>'>
                    <img src="<%# Eval("FotoBanner") %>" alt="<%# Eval("Nombre") %>">
                    <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-10 rounded">
                        <h5><%# Eval("Nombre") %></h5>
                        <p>$<%# Eval("Precio", "{0:N0}") %></p>
                    </div>
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="alert alert-info">No hay productos para mostrar.</div>
            </EmptyDataTemplate>
        </asp:ListView>

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
                    <asp:TextBox
                        ID="idDataBuscarProducto"
                        runat="server"
                        type="text"
                        class="form-control border-start-0"
                        placeholder="Buscá tu producto"
                        aria-label="Buscar producto"></asp:TextBox>
                    <asp:Button id="btnBuscar" runat="server" class="btn btn-primary" OnClick="btnBuscar_Click" Text="Buscar!"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <main aria-labelledby="title" class="container body-content">
        <!-- 2) SECCIÓN FILTRO POR CATEGORÍA -->
        <section class="d-flex justify-content-center mb-4 mt-5">
            <h5 class="mb-0">
                <i class="bi bi-stars pe-2"></i>PRODUCTOS DESTACADOS
            </h5>
        </section>
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
                                    <%# Eval("Nombre") %>
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

        <section class="d-flex justify-content-center mb-4 mt-5">
            <h5 class="mb-0">
                <i class="bi bi-stars pe-2"></i>ÚLTIMOS INGRESOS
            </h5>
        </section>
        <section class="card-body mt-2">
            <asp:ListView ID="lvUltimosIngresos" runat="server" ItemPlaceholderID="itemPlaceholder">
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
                                    <%# Eval("Nombre") %>
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

    </main>

</asp:Content>

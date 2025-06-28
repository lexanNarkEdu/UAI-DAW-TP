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
            <div class="col-md-4 col-lg-4">
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
                    <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary" OnClick="btnBuscar_Click" Text="Buscar!"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <main aria-labelledby="title" class="container body-content">

        <asp:ListView ID="lvProductos" runat="server">
            <LayoutTemplate>
                <section class="container my-4 p-3 bg-white rounded shadow-xs">
                    <h4 class="fw-medium mb-3"><i class="bi bi-stars pe-1"></i>Productos destacados</h4>
                    <div class="d-flex">
                        <div class="row" id="itemPlaceholder" runat="server"></div>
                    </div>
                </section>
            </LayoutTemplate>
            <ItemTemplate>
                <article class="col-6 col-md-4 col-lg-2 mb-4">
                    <a href="DetalleProducto.aspx?id=<%# Eval("ProductoId") %>" class="text-decoration-none">
                        <div class="card product-card h-100 border-0 ">
                            <img src='<%# Eval("Foto") %>' class="card-img-top p-3" style="height: inherit; object-fit: contain;" alt='<%# Eval("Nombre") %>' />
                            <div class="card-body d-flex flex-column p-2">
                                <h6 class="text-secondary mb-2"><%# Eval("Nombre") %></h6>
                                <h5 class="text-black fw-bold mb-1">$ <%# Eval("Precio", "{0:N0}") %></h5>
                                <p class="text-muted small mb-1">en 6 cuotas de $<%# (Convert.ToDecimal(Eval("Precio")) * 1.15m / 6).ToString("N2") %></p>
                            </div>
                            <span class="badge bg-secondary text-white mb-2 p-1" style="width: min-content;"><%# GetCondicionTexto(Eval("CondicionId")) %></span>
                        </div>
                    </a>
                </article>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="alert alert-info">No hay productos para mostrar.</div>
            </EmptyDataTemplate>
        </asp:ListView>

        <asp:ListView ID="lvUltimosIngresos" runat="server">
            <LayoutTemplate>
                <section class="container my-4 p-3 bg-white rounded shadow-xs">
                    <h4 class="fw-medium mb-3"><i class="bi bi-bell pe-1"></i>Últimos productos ingresados</h4>
                    <div class="d-flex">
                        <div class="row" id="itemPlaceholder" runat="server"></div>
                    </div>
                </section>
            </LayoutTemplate>
            <ItemTemplate>
                <article class="col-6 col-md-4 col-lg-2 mb-4">
                    <a href="DetalleProducto.aspx?id=<%# Eval("ProductoId") %>" class="text-decoration-none">
                        <div class="card product-card h-100 border-0 ">
                            <img src='<%# Eval("Foto") %>' class="card-img-top p-3" style="height: inherit; object-fit: contain;" alt='<%# Eval("Nombre") %>' />
                            <div class="card-body d-flex flex-column p-2">
                                <h6 class="text-secondary mb-2"><%# Eval("Nombre") %></h6>
                                <h5 class="text-black fw-bold mb-1">$ <%# Eval("Precio", "{0:N0}") %></h5>
                                <p class="text-muted small mb-1">en 6 cuotas de $<%# (Convert.ToDecimal(Eval("Precio")) * 1.15m / 6).ToString("N2") %></p>
                            </div>
                            <span class="badge bg-secondary text-white mb-2 p-1" style="width: min-content;"><%# GetCondicionTexto(Eval("CondicionId")) %></span>
                        </div>
                    </a>
                </article>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="alert alert-info">No hay productos para mostrar.</div>
            </EmptyDataTemplate>
        </asp:ListView>

    </main>

</asp:Content>

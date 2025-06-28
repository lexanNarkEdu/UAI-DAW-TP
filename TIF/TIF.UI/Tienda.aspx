<%@ Page Title="Tienda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tienda.aspx.cs" Inherits="TIF.UI.Tienda" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <main aria-labelledby="title" class="container body-content">
        <div class="row mt-4">
            <asp:Panel ID="pnlFiltro" runat="server" CssClass="col col-lg-3 d-flex flex-column">
                <aside class="d-flex flex-column my-4">
                    <div class="input-group align-items-start rounded mb-4">
                        <%-- 
                        <span class="input-group-text border-end-0">
                            <i class="bi bi-search text-secondary"></i>
                        </span>
                        --%>
                        <asp:TextBox runat="server" name="idDataBuscarProducto" id="idDataBuscarProducto" type="text" 
                            class="form-control border-start-0" placeholder="Buscá tu producto" aria-label="Buscar producto" />
                    </div>
                    <!-- Filtro por Categoría -->
                    <div class="d-flex align-items-start flex-column mb-4">
                        <asp:Label ID="lblFiltroCategoria" runat="server" Text="Categoría:"
                            AssociatedControlID="ddlFiltroCategorias" CssClass="mb-2 ps-2" />
                        <asp:DropDownList ID="ddlFiltroCategorias" runat="server"
                            CssClass="form-select" />
                    </div>
                    <!-- Filtro por Condicion -->
                    <div class="d-flex align-items-start flex-column mb-4">
                        <asp:Label ID="lblFiltroCondicion" runat="server" Text="Condición:"
                            AssociatedControlID="ddlFiltroCondiciones" CssClass="mb-2 ps-2" />
                        <asp:DropDownList ID="ddlFiltroCondiciones" runat="server"
                            CssClass="form-select" />
                        <!--  AutoPostBack="true" OnSelectedIndexChanged="Filtros_SelectedIndexChanged" -->
                    </div>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary mb-3" OnClick="btnBuscar_Click" CausesValidation="false" />
                    <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnLimpiarFiltros_Click" CausesValidation="false" />
                </aside>
            </asp:Panel>

            <asp:Panel ID="pnProductos" runat="server" CssClass="col">
                <asp:ListView ID="lvProductos" runat="server">
                    <LayoutTemplate>
                        <section class="container my-4 p-3 bg-white rounded shadow-xs">
                            <asp:Label ID="lblcantidadProductosResultado" runat="server" CssClass="badge bg-secondary" />
                            <ol class="d-flex flex-column">
                                <div class="row" id="itemPlaceholder" runat="server"></div>
                            </ol>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="li-none">
                            <a href="DetalleProducto.aspx?id=<%# Eval("ProductoId") %>" class="text-decoration-none">
                                <div class="card product-card h-100 border-0 flex-row">
                                    <img src='<%# Eval("Foto") %>' class="card-img-top p-3" style="object-fit: contain; width: 18rem; max-height: 12rem; height: 10rem;" alt='<%# Eval("Nombre") %>' />
                                    <div class="card-body d-flex flex-column p-2" style="width: min-content;">
                                        <h6 class="text-secondary mb-2"><%# $"{ Eval("Nombre")}" %></h6>
                                        <p class="text-muted small mb-1 descripcion"><%# Eval("Descripcion") %></p>
                                        <div class="d-flex flex-row justify-content-start mt-3">
                                            <h4 class="text-black pe-4">$ <%# Eval("Precio", "{0:N0}") %></h4>
                                            <p class="text-muted small">en 6 cuotas de $<%# (Convert.ToDecimal(Eval("Precio")) * 1.15m / 6).ToString("N2") %></p>
                                            <span class="badge bg-secondary text-white p-1" style="width: min-content; height: min-content; margin-left: auto; right: 0;"><%# GetCondicionTexto(Eval("CondicionId")) %></span>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div class="alert alert-info">No hay productos para mostrar.</div>
                    </EmptyDataTemplate>
                </asp:ListView>
            </asp:Panel>
        </div>

    </main>

</asp:Content>

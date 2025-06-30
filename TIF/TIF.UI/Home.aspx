<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TIF.UI.Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title" class="container body-content">
        <div class="row">
            <div class="col-md-12">
                <div class="jumbotron bg-primary text-white p-5 rounded mb-4">
                    <div class="container-fluid py-5">
                        <h1 class="display-5 fw-bold">¡Bienvenido al Sistemas!</h1>
                        <p class="col-md-8 fs-4">
                            Hola <strong>
                                <asp:Literal ID="NombreUsuario" runat="server"></asp:Literal></strong>, 
                        has iniciado sesión exitosamente en la tienda de insumos informaticos.
                        </p>
                        <p class="fs-6 text-light">
                            Última conexión:
                            <asp:Literal ID="FechaHoraIngreso" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">
                            <i class="bi bi-house-door pe-3"></i>Panel Principal
                        </h5>
                        <p class="card-text">
                            Desde aquí puedes acceder a todas las funcionalidades del sistema.
                        Utiliza el menú de navegación superior para moverte entre las diferentes secciones.
                        </p>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">
                            <i class="bi bi-info-circle pe-3"></i>Información
                        </h5>
                        <p class="card-text">
                            Sistema desarrollado para la gestión y auditoría de eventos.
                        Para más información consulta la sección "Acerca de".
                        </p>
                        <a href="About.aspx" class="btn btn-outline-secondary">Más Info</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-4">
            <div class="col-md-12">
                <div class="alert alert-info" role="alert">
                    <h6 class="alert-heading">
                        <i class="bi bi-lightbulb pe-3"></i>Tip del Sistema
                    </h6>
                    <p class="mb-0">
                        Todas las acciones que realizas en el sistema quedan registradas en la bitácora 
                    para mantener un historial completo de actividades y garantizar la trazabilidad.
                    </p>
                </div>
            </div>
        </div>
    </main>
</asp:Content>

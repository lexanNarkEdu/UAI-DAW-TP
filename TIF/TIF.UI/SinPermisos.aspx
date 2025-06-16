<%@ Page Title="Acceso Denegado" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SinPermisos.aspx.cs" Inherits="TIF.UI.SinPermisos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6">
            <div class="text-center mb-4">
                <i class="bi bi-shield-exclamation text-danger" style="font-size: 4rem;"></i>
                <h2 class="mb-3 text-danger">
                    Acceso Denegado
                </h2>
                <p class="text-muted mb-4">
                    No tienes permisos suficientes para acceder a esta página del sistema.
                </p>
            </div>

            <!-- Card con información del error -->
            <div class="card border-danger mb-4">
                <div class="card-header bg-danger text-white">
                    <h5 class="mb-0">
                        <i class="bi bi-exclamation-triangle"></i> Información del Error
                    </h5>
                </div>
                <div class="card-body">
                    <div class="alert alert-danger" role="alert">
                        <strong>Motivo:</strong> Tu cuenta de usuario no tiene los permisos necesarios para acceder a esta funcionalidad.
                    </div>
                    
                    <div class="row text-center">
                        <div class="col-md-6">
                            <i class="bi bi-person-check text-primary" style="font-size: 2rem;"></i>
                            <h6 class="mt-2">Usuario Actual</h6>
                            <p class="text-muted">
                                <asp:Literal ID="ltUsuarioActual" runat="server"></asp:Literal>
                            </p>
                        </div>
                        <div class="col-md-6">
                            <i class="bi bi-clock text-warning" style="font-size: 2rem;"></i>
                            <h6 class="mt-2">Fecha y Hora</h6>
                            <p class="text-muted">
                                <asp:Literal ID="ltFechaHora" runat="server"></asp:Literal>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Card con opciones de acción -->
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">
                        <i class="bi bi-gear"></i> ¿Qué puedes hacer?
                    </h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <div class="d-grid">
                                <asp:Button ID="btnVolver" runat="server" 
                                           Text="Volver al Inicio" 
                                           CssClass="btn btn-primary"
                                           OnClick="btnVolver_Click" />
                            </div>
                        </div>
                        <div class="col-md-6 mb-3">
                            <div class="d-grid">
                                <asp:Button ID="btnContactar" runat="server" 
                                           Text="Contactar Administrador" 
                                           CssClass="btn btn-outline-secondary"
                                           OnClick="btnContactar_Click" />
                            </div>
                        </div>
                    </div>
                    
                    <hr />
                    
                    <div class="alert alert-info" role="alert">
                        <i class="bi bi-info-circle"></i>
                        <strong>Sugerencia:</strong> Si necesitas acceso a esta funcionalidad, contacta con el administrador del sistema para solicitar los permisos correspondientes.
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<%@ Page Title="ABM Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMUsuarios.aspx.cs" Inherits="TIF.UI.ABMUsuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title" class="container body-content">
        <style>
            .login-form .button {
                width: 100%;
                padding: 10px;
                background-color: #007bff;
                color: white;
                border: none;
                border-radius: 4px;
                cursor: pointer;
                font-size: 16px;
                margin-top: 20px;
                transition: background-color 0.3s ease;
            }

                .login-form .button:hover {
                    background-color: #0056b3;
                }
        </style>

        <h2 id="title"><%:Title%>.</h2>
        <asp:Button ID="manageUsers" runat="server" Text="Gestionar usuarios existentes" CssClass="button" OnClick="manageUsers_Click" />
        <br />
        <br />
        <asp:Button ID="addUser" runat="server" Text="Agregar nuevo usuario" CssClass="button" OnClick="addUser_Click" />

    </main>
</asp:Content>

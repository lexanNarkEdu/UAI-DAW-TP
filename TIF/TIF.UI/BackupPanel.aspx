<%@ Page Title="Backup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackupPanel.aspx.cs" Inherits="TIF.UI.BackupPanel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title" class="container body-content">
        <h3>Backup de Base de Datos</h3>
        <div>
            <asp:Label ID="lblDatabaseName" runat="server" Text="Nombre de la Base de Datos:" AssociatedControlID="txtDatabaseName" />
            <asp:TextBox ID="txtDatabaseName" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDatabaseName" ErrorMessage="Debe completar el nombre" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
        <div style="margin-top:10px;">
            <asp:Label ID="lblBackupPath" runat="server" Text="Ruta de Backup:" AssociatedControlID="txtBackupPath" />
            <asp:TextBox ID="txtBackupPath" runat="server" Width="300px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBackupPath" ErrorMessage="Debe completar la ruta" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
        <div style="margin-top:20px;">
            <asp:Button ID="btnBackup" runat="server" Text="Realizar Backup" OnClick="btnBackup_Click" />
        </div>
        <div style="margin-top:20px;">
            <asp:Label ID="lblResult" runat="server" ForeColor="Red" />
        </div>
    </main>
</asp:Content>
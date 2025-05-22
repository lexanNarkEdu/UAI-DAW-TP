<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TIF.UI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: calc(100vh - 100px);
            padding: 20px;
            box-sizing: border-box; 
        }

        .login-form {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            width: 100%; 
            max-width: 400px; 
            text-align: center;
            box-sizing: border-box;
        }

        .form-group {
            margin-bottom: 15px;
            text-align: left; 
        }

        .form-group label {
            display: block; 
            margin-bottom: 5px;
            font-weight: bold;
            color: #333;
        }

        .form-group input[type="text"],
        .form-group input[type="password"] {
            width: calc(100% - 22px); 
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 16px;
            box-sizing: border-box; 
        }

        .login-form h2 {
            margin-bottom: 25px;
            color: #0056b3;
            font-size: 24px;
        }

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

        .login-form .link-button {
            display: block; 
            margin-top: 10px;
            color: #007bff;
            text-decoration: none;
            font-size: 0.95em;
            transition: text-decoration 0.3s ease;
        }

        .login-form .link-button:hover {
            text-decoration: underline;
        }

        @media (max-width: 600px) {
            .login-form {
                padding: 20px;
            }
            .login-form h2 {
                font-size: 20px;
            }
            .form-group input[type="text"],
            .form-group input[type="password"] {
                font-size: 14px;
            }
        }
    </style>

    <div class="login-container">
        <div class="login-form">
            <asp:Label ID="titulo" runat="server" Font-Bold="True" Font-Underline="False" Text="Iniciar Sesión"></asp:Label>
            
            <div class="form-group">
                <asp:Label ID="usuarioLabel" runat="server" Text="Usuario"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="usuarioTextbox" runat="server"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <asp:Label ID="passwordLabel" runat="server" Text="Password"></asp:Label>
                &nbsp;<asp:TextBox ID="passwordTextbox" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <asp:Button ID="ingresarButton" runat="server" OnClick="ingresarButton_Click" Text="Ingresar" CssClass="button" />
            
            <asp:LinkButton ID="newUser" runat="server" OnClick="newUser_Click" CssClass="link-button">Quiero mi usuario</asp:LinkButton>
            
            <asp:LinkButton ID="forgotPassword" runat="server" OnClick="forgotPassword_Click" CssClass="link-button">Olvidé mi usuario/password</asp:LinkButton>
        </div>
    </div>
</asp:Content>
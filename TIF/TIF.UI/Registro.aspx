<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="TIF.UI.Registro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Registro de Usuario</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f4f4f4;
        }
        .container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            max-width: 500px;
            margin: auto;
        }
        h2 {
            text-align: center;
            color: #333;
            margin-bottom: 25px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
            color: #555;
        }
        .form-group input[type="text"],
        .form-group input[type="email"],
        .form-group input[type="password"],
        .form-group input[type="number"] {
            width: calc(100% - 22px);
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box; 
        }
        .form-group input:focus {
            border-color: #007bff;
            outline: none;
            box-shadow: 0 0 5px rgba(0, 123, 255, 0.2);
        }
        .btn-register {
            background-color: #28a745;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            width: 100%;
            margin-top: 20px;
        }
        .btn-register:hover {
            background-color: #218838;
        }
        .validation-message {
            color: red;
            font-size: 0.9em;
            margin-top: 5px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Registro de Nuevo Usuario</h2>
            <div class="form-group">
                <label for="txtNombre">Nombre:</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese su nombre"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                    ErrorMessage="El nombre es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtApellido">Apellido:</label>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ingrese su apellido"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                    ErrorMessage="El apellido es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtDni">DNI:</label>
                <asp:TextBox ID="txtDni" runat="server" TextMode="Number" CssClass="form-control" placeholder="Ingrese su DNI"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvDni" runat="server" ControlToValidate="txtDni"
                    ErrorMessage="El DNI es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revDni" runat="server" ControlToValidate="txtDni"
                    ValidationExpression="^\d+$" ErrorMessage="El DNI debe contener solo números." CssClass="validation-message" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:RangeValidator ID="rvDni" runat="server" ControlToValidate="txtDni"
                    ErrorMessage="El DNI debe ser un número entre 1 y 99999999." CssClass="validation-message" Display="Dynamic"></asp:RangeValidator>
            </div>

            <div class="form-group">
                <label for="txtEmail">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="Ingrese su email"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="El email es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ErrorMessage="Ingrese un formato de email válido." CssClass="validation-message" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="txtDomicilio">Domicilio:</label>
                <asp:TextBox ID="txtDomicilio" runat="server" CssClass="form-control" placeholder="Ingrese su domicilio"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvDomicilio" runat="server" ControlToValidate="txtDomicilio"
                    ErrorMessage="El domicilio es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtNombreUsuario">Nombre de Usuario:</label>
                <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" placeholder="Elija un nombre de usuario"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvNombreUsuario" runat="server" ControlToValidate="txtNombreUsuario"
                    ErrorMessage="El nombre de usuario es obligatorio." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group" ID="divRol" runat="server">
                <label for="txtRol">Rol:</label>
                <asp:DropDownList ID="dropDownRol" runat="server" CssClass="form-control" placeholder="Seleccione un rol para el usuario">
                </asp:DropDownList>
                <br/>
            </div>

            <div class="form-group">
                <label for="txtContrasena">Contraseña:</label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese su contraseña"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" ControlToValidate="txtContrasena"
                    ErrorMessage="La contraseña es obligatoria." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revContrasena" runat="server" ControlToValidate="txtContrasena"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&]).{8,}$"
                    ErrorMessage="La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un caracter especial (!@#$%&)."
                    CssClass="validation-message" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="txtRepetirContrasena">Repetir Contraseña:</label>
                <asp:TextBox ID="txtRepetirContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="Repita su contraseña"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvRepetirContrasena" runat="server" ControlToValidate="txtRepetirContrasena"
                    ErrorMessage="Debe repetir la contraseña." CssClass="validation-message" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cmpContrasenas" runat="server" ControlToCompare="txtContrasena"
                    ControlToValidate="txtRepetirContrasena" ErrorMessage="Las contraseñas no coinciden." CssClass="validation-message" Display="Dynamic"></asp:CompareValidator>
            </div>

            <asp:Button ID="btnRegistrar" runat="server" Text="Registrarse" CssClass="btn-register" OnClick="btnRegistrar_Click"/>
        </div>
    </form>
</body>
</html>

using BE;
using BE.Permisos;
using BLL;
using BLL;
using Services;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TIF.UI.Helpers;

namespace TIF.UI
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rvDni.MinimumValue = "00000001";
            rvDni.MaximumValue = "99999999";
            
            RoLBLL roLBLL = new RoLBLL();
            bool permisoABM = roLBLL.EstaPermisoEnRol(Session["UsuarioPermisos"] as List<PermisoBE>, PermisoToRouteHelper.ToPermiso("~/ABMUsuarios"));
            dropDownRol.Visible = permisoABM;

            if (permisoABM) {
                PermisoBLL permisoBLL = new PermisoBLL();
                foreach (var permiso in permisoBLL.BuscarTodosLosRoles())
                {
                    dropDownRol.Items.Add(new ListItem(permiso.Nombre, permiso.Id.ToString()));
                }
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            ValidatorCollection validators =  Page.Validators;
            foreach (BaseValidator validator in validators)
            {
                if (!validator.IsValid)
                {
                    Response.Write("<script>alert('Por favor, corregí los errores en el formulario.');</script>");
                    return;
                }
            }

            EncriptadorService encriptador = EncriptadorService.GetEncriptadorService();
            string username = encriptador.EncriptarAES(txtNombreUsuario.Text.Trim());
            UsuarioBLL usuarioBLL = new UsuarioBLL();
            try
            {
                if (usuarioBLL.obtenerUsuario(username) != null)
                {
                    throw new Exception("El nombre de usuario no se puede utilizar, por favor intentá con otro.");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return;
            }

            string password = encriptador.EncriptarMD5(txtContrasena.Text.Trim());
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            int dni = int.Parse(txtDni.Text.Trim());
            string email = txtEmail.Text.Trim();
            string domicilio = txtDomicilio.Text.Trim();
            ListItem a = dropDownRol.SelectedItem;
            string rol = dropDownRol.Visible ? dropDownRol.SelectedItem.Value : "7";

            bool exito = usuarioBLL.GuardarUsuario(
                new UsuarioBE(username, password, nombre, apellido, dni, email, domicilio, 0, false),
                rol
            );
            
            if (exito)
            {  
                Session["ExitoRegistro"] = "Usuario registrado exitosamente.";
                Response.Redirect("Login.aspx");
            }
            else
            {
                Response.Write("<script>alert('Hubo un error al registrar el usuario, por favor volvé a intentarlo en unos momentos.');</script>");
            }
        }
    }
}
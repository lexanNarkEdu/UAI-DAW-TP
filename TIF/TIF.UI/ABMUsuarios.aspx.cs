using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class ABMUsuarios : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null && Session["UsuarioPermisos"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void manageUsers_Click(object sender, EventArgs e)
        {
            Response.Write("<script>alert('" + "No está implementado aún!!" + "');</script>");
        }

        protected void addUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx");
        }
    }
}
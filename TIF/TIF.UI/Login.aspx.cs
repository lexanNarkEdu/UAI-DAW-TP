using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ingresarButton_Click(object sender, EventArgs e)
        {
            string usuario = usuarioTextbox.Text.Trim();
            string password = passwordTextbox.Text.Trim();

            string alert = "<script>alert('Usuario es " + usuario + " y password es " + password + "');</script>";
            Response.Write(alert);
        }

        protected void newUser_Click(object sender, EventArgs e)
        {

        }

        protected void forgotPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
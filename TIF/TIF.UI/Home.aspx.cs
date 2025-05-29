using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string nombre = (string)Session["Usuario"];
            string apellido = (string)Session["Apellido"];

            string alert = "<script>alert('Usuario es " + nombre + " y apellido es " + apellido + "');</script>";
            Response.Write(alert);
        }
    }
}
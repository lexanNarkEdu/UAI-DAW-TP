using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class About : Page
    {
        public string NombreEmpresa { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            NombreEmpresa = "TecnoComponentes Globales";
        }
    }
}
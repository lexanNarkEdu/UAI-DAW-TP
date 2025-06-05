using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIF.UI
{
    public partial class BackupPanel : Page
    {
        private AdminBLL adminBll = new AdminBLL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            lblResult.ForeColor = System.Drawing.Color.Red;
            bool isWebMaster = true;
            if (!isWebMaster)
            {
                lblResult.Text = "No tiene permisos para realizar esta acción.";
                return;
            }

            try
            {
                adminBll.backup(txtDatabaseName.Text.Trim(), txtBackupPath.Text.Trim());
                lblResult.ForeColor = System.Drawing.Color.Green;
                lblResult.Text = "Backup accionado con éxito, es unos instantes estará accesible.";
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error al realizar el backup: " + ex.Message;
            }
        }
    }
}
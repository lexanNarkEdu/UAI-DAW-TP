using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL
{
    public class AdminBLL
    {
        private readonly AdminDAL _adminDAL = new AdminDAL();

        public void backup(string dbName, string backupPath)
        {
            _adminDAL.backup(dbName, backupPath);
        }
    }
}

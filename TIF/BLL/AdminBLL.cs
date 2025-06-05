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

        public void backup(string dbName, string backupPath)
        {
            DAO.GetDAO().backup(dbName, backupPath);
        }
    }
}

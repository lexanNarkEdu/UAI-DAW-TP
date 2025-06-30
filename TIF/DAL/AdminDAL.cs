using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AdminDAL
    {
        private readonly DAO _dao = DAO.GetDAO();

        public void backup(string dbName, string backupPath)
        {
            string backupCommand = $@"
                exec msdb.dbo.rds_backup_database 
                    @source_db_name = '{dbName}',
                    @s3_arn_to_backup_to = 'arn:aws:s3:::dawbackup/{backupPath}',
                    @type = 'FULL';";
            _dao.ExecuteNonQuery(backupCommand);
        }
    }
}

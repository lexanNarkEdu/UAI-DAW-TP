using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BE;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {


        public static Cliente Obtener(string cUsername)
        {
            string mCommandText = "SELECT usuario_id, usuario_username, usuario_nombre, usuario_apellido, usuario_dni, usuario_email, usuario_fallos_autenticacion_consecutivos, usuario_bloqueado, usuario_telefono, usuario_domicilio FROM Usuario WHERE usuario_username = " + cUsername;

            DAO mDAO = new DAO();

            DataSet mDs = mDAO.ExecuteDataSet(mCommandText);

            if (mDs.Tables.Count > 0 && mDs.Tables[0].Rows.Count > 0)
            {
                Cliente cliente = new Cliente(cUsername);
                ValorizarEntidad(cliente, mDs.Tables[0].Rows[0]);
                return cliente;
            }
            else
            {
                return null;
            }

        }

        internal static void ValorizarEntidad(Cliente cliente, DataRow pDataRow)
        {
            //TODO
        }

    }

}

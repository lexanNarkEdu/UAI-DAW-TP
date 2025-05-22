using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    internal class DAO
    {

        SqlConnection mCon = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);

        public DataSet ExecuteDataSet(string pCommandText)
        {
            try
            {
                SqlDataAdapter mDa = new SqlDataAdapter(pCommandText, mCon);
                DataSet mDs = new DataSet();

                mDa.Fill(mDs);

                return mDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed)
                    mCon.Close();
            }

        }

        public int ExecuteNonQuery(string pCommandText)
        {
            try
            {
                SqlCommand mCom = new SqlCommand(pCommandText, mCon);
                mCon.Open();
                return mCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed)
                    mCon.Close();
            }
        }

        public int ObtenerUltimoId(string pTabla)
        {
            try
            {
                SqlCommand mCom = new SqlCommand("SELECT ISNULL(MAX(" + pTabla + "_Id),0) FROM " + pTabla, mCon);
                mCon.Open();
                return int.Parse(mCom.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (mCon.State != ConnectionState.Closed)
                    mCon.Close();
            }
        }

    }
}

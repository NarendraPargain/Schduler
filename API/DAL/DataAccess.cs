using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.DAL
{
    public class DataAccess
    {
        static readonly string conStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        public static void updateData(string eamilId)
        {

            using (SqlConnection con = new SqlConnection(conStr))
            {

                DataSet ds = new DataSet();

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("sp_LinkAccessed", con);
                da.SelectCommand.Parameters.AddWithValue("@Email", eamilId);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                
            }
        }
    }
}
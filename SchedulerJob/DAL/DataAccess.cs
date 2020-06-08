using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using SchedulerJob.Model;

namespace SchedulerJob.DataAccess
{
    class DataAccess
    {
        static readonly string conStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

        // public static Dictionary<int, Employee> FetchData()
        public static List<Employee> FetchData()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("sp_getEmployee", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                da.Fill(ds);

                List<Employee> list = new List<Employee>();

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        Employee emp = new Employee();
                        emp.Name = dr["Name"].ToString();
                        emp.Email = dr["EmailId"].ToString();
                        emp.IsLinkAccessed = (bool)dr["IsLinkAccessed"];
                        emp.RemainderCount = (int)dr["RemainderCount"];

                        list.Add(emp);
                    }
                }

                return list;
            }

        }

        public static void updateData(List<Employee> list)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                DataTable dt = ToDataTable(list);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("sp_updateEmployees", con);
                da.SelectCommand.Parameters.AddWithValue("@dataTable", dt);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                da.Fill(ds);
            }
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;


        }
    }
}

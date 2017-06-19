using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Calculator
{
    public class sqlRetrieve
    {
        public List<double> retrievePrice(string stock, string server, string db, string pwd, string uid)
        {
            Dictionary<string, List<double>> values = new Dictionary<string, List<double>>();
            List<double> prices = new List<double>();

            MySqlConnection conn = new MySqlConnection();
            string myConnectionString;

            myConnectionString = String.Format("server={0};uid={1};" +
        "pwd={2};database={3};", server, uid, pwd, db);
            string query = "SELECT price FROM nasdaq WHERE Symbol = '" + stock + "' and time > '09:00:00' and time < '16:30:00';";

            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();


                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);

                DataSet ds = new DataSet();

                da.Fill(ds, "nasdaq");
                DataTable dt = ds.Tables["nasdaq"];

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        prices.Add((double)(row[col]));
                    }
                }
                

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }

            }
            return prices;
        }}
}



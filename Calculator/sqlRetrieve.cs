using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Calculator
{
    public class sqlRetrieve
    {
        public List<double> retrievePrice(string stock)
        {
            Dictionary<string, List<double>> values = new Dictionary<string, List<double>>();
            List<double> prices = new List<double>();

            MySqlConnection conn = new MySqlConnection();
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=pmacarthur;" +
        "pwd=theblob;database=StockInfo;";
            string query = "SELECT price FROM nasdaq WHERE Symbol = '" + stock + "';";

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
        }
    }




}

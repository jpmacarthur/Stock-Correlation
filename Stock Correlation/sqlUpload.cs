using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace Stock_Correlation
{
    public class sqlUpload
    {
        public void upload(Dictionary<string, string> values)
        {
            List < string > commands = new List<String>();
            bool ending = false;
            MySql.Data.MySqlClient.MySqlConnection conn = new MySqlConnection();
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=pmacarthur;" +
        "pwd=theblob;database=StockInfo;";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MySqlCommand comm = conn.CreateCommand();
            StringBuilder sb = new StringBuilder("INSERT INTO nasdaq(Symbol,Price, Date, Time) values ");
            foreach (KeyValuePair<string, string> id in values) {
                if (sb.Length < 950)
                {
                    sb.Append("(\"" + id.Key + "\"");
                    if (id.Value == null) sb.Append(", null");
                    else sb.Append(", " + id.Value);
                    sb.Append(", CURDATE(), CURTIME()), ");
                    ending = false;
                }
                else
                {
                    sb.Length = sb.Length - 2;
                    sb.Append(";");
                    commands.Add(sb.ToString());
                    sb = new StringBuilder("INSERT INTO nasdaq(Symbol,Price, Date, Time) values ");
                    sb.Append("(\"" + id.Key + "\"");
                    if (id.Value == null) sb.Append(", null");
                    else sb.Append(", " + id.Value);
                    sb.Append(", CURDATE(), CURTIME()), ");
                    ending = true;
                }
             }
            if (ending == true)
            {
                sb.Length = sb.Length - 2;
                sb.Append(";");
            }
            foreach (string com in commands)
            {
                comm.CommandText = com;
                comm.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}

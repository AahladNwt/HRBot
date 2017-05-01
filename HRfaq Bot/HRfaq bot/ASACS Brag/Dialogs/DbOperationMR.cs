using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace NewWave_Bot_Sample.Dialogs
{
    public class DbOperationMR
    {
        string conn;
        string val;
        MySqlConnection connect;

        public void db_connection()
        {
            try
            {
                conn = "Server=localhost;port=3306;uid=team013;pwd=team013;database=cs6400_sp17_team013;";
                connect = new MySqlConnection(conn);
                string Query = "SELECT min(num1) AS meals FROM (SELECT * FROM (SELECT SUM(Number_of_Units) As num1 FROM cs6400_sp17_team013.item " +
                "WHERE Food_Supplies_type = 'meat/seafood' OR food_supplies_type = 'dairy/eggs' "+
                "UNION SELECT SUM(Number_of_Units) FROM cs6400_sp17_team013.item WHERE Food_Supplies_type = 'vegetables' "+
                "UNION SELECT SUM(Number_of_Units) FROM cs6400_sp17_team013.item WHERE Food_Supplies_type = 'nuts/grains/beans') AS value_list) AS Valio; ";
                MySqlCommand scmd = new MySqlCommand(Query, connect);
                MySqlDataReader rdr;
                connect.Open();
                rdr = scmd.ExecuteReader();
                if (rdr.Read())
                {
                    val = rdr["meals"].ToString();
                    System.Diagnostics.Debug.WriteLine(rdr["meals"].ToString());
                }
                System.Diagnostics.Debug.WriteLine("The connection has been established: {0}", connect);
                System.Diagnostics.Debug.WriteLine("\r\n Your query has been executed check the database \r\n");

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Exception value: {0}", ex);
            }
            finally
            {
                connect.Close();
            }
        }

        public string value
        {
            set
            {
                this.value = val;
            }
            get
            {
                return this.val;
            }

        }

    }
}
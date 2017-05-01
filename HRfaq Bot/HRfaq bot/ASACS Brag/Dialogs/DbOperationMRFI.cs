using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace NewWave_Bot_Sample.Dialogs
{
    public class DbOperationMRFI
    {
        string conn;
        string val;
        MySqlConnection connect;

        public void db_connectionfi(string srr)
        {
            string fbid = null;
            if (srr == "food_bank1")
            {
                fbid = "21672";                
            }
            else if(srr == "food_bank2")
            {
                fbid = "21673";                
            }
            else if(srr == "food_bank3")
            {
                fbid = "21674";                
            }
            
            try
            {
                conn = "Server=localhost;port=3306;uid=team013;pwd=team013;database=cs6400_sp17_team013;";
                connect = new MySqlConnection(conn);
                string[] Query = new string[4];
                string[] ConQ = new string[2];
                Query[0] = "SELECT min(num1) AS meals FROM (SELECT * FROM (SELECT SUM(Number_of_Units) As num1 FROM cs6400_sp17_team013.item " +
                "WHERE (Food_Supplies_type = 'meat/seafood' OR food_supplies_type = 'dairy/eggs') AND service_id = ";
                Query[1] = " UNION SELECT SUM(Number_of_Units) FROM cs6400_sp17_team013.item WHERE Food_Supplies_type = 'vegetables' AND service_id = ";
                Query[2] = " UNION SELECT SUM(Number_of_Units) FROM cs6400_sp17_team013.item WHERE Food_Supplies_type = 'nuts/grains/beans' AND service_id = ";
                Query[3] = ") AS value_list) AS Valio;";
                ConQ[0] = string.Concat(Query[0], fbid, Query[1], fbid);
                ConQ[1] = string.Concat(ConQ[0], Query[2], fbid, Query[3]);                       
                MySqlCommand scmd = new MySqlCommand(ConQ[1], connect);
                MySqlDataReader rdr;
                connect.Open();
                rdr = scmd.ExecuteReader();
                if (rdr.Read())
                {
                    val = rdr["meals"].ToString();
                    System.Diagnostics.Debug.WriteLine(rdr["meals"].ToString());
                }
                System.Diagnostics.Debug.WriteLine("In Entity DB Class");                
                System.Diagnostics.Debug.WriteLine("\r\n\r\n\r\n");
                System.Diagnostics.Debug.WriteLine(ConQ[1]);

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
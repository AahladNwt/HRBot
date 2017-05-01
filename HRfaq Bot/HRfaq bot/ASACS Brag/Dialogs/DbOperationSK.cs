using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace NewWave_Bot_Sample.Dialogs
{
    public class DbOperationSK
    {
        string conn;
        string valrc, valbc;
        MySqlConnection connect;

        public void db_connection()
        {
            try
            {
                conn = "Server=localhost;port=3306;uid=team013;pwd=team013;database=cs6400_sp17_team013;";
                connect = new MySqlConnection(conn);
                string Query = "SELECT sum(seat_count) AS Seat_Count FROM cs6400_sp17_team013.service"+
                " WHERE type_of_service = 'soup_kitchen'"; 
                               
                MySqlCommand scmd = new MySqlCommand(Query, connect);
                MySqlDataReader rdr;
                connect.Open();
                rdr = scmd.ExecuteReader();
                if (rdr.Read())
                {
                    valrc = rdr["Seat_Count"].ToString();                       
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

        public string Valrc { get => valrc; set => valrc = value; }        
    }
}
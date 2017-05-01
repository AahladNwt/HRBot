using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace NewWave_Bot_Sample.Dialogs
{
    public class DbOperationDonat
    {
        string conn, donat;
        //string[] val = new string[3];
        int vls;
        int[] vl = new int[3];
        MySqlConnection connect;

        public void db_connectiondonat()
        {
            try
            {
                string[] Query = new string[3];
                Query[0] = "SELECT SUM(Number_of_Units) AS meat_dairy FROM cs6400_sp17_team013.item " +
                            "WHERE Food_Supplies_type = 'meat/seafood' OR food_supplies_type = 'dairy/eggs';";
                Query[1] = "SELECT SUM(Number_of_Units) AS veg   FROM cs6400_sp17_team013.item "+
                            "WHERE Food_Supplies_type = 'vegetables'; ";
                Query[2] = "SELECT SUM(Number_of_Units) AS nut_grain FROM cs6400_sp17_team013.item "+
                            "WHERE Food_Supplies_type = 'nuts/grains/beans'; ";
                vl[0] = ExecQuery(Query[0]);
                vl[1] = ExecQuery(Query[1]);
                vl[2] = ExecQuery(Query[2]);
                System.Diagnostics.Debug.WriteLine(vl[0]);
                System.Diagnostics.Debug.WriteLine(vl[1]);
                System.Diagnostics.Debug.WriteLine(vl[2]);                                 
                System.Diagnostics.Debug.WriteLine("The connection has been established: {0}", connect);
                System.Diagnostics.Debug.WriteLine("\r\n Your query has been executed check the database \r\n");

                if (vl[1] >= vl[0] && vl[1] >= vl[2])
                {
                    vls = vl[1];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Nuts/Grains/Beans of " + (vls - vl[2]) + " units and Donations of Meat/Dairy of "+ (vls - vl[0]) +" units";
                }
                else if(vl[0] >= vl[1] && vl[0] >= vl[2])
                {
                    vls = vl[0];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Nuts/Grains/Beans of " + (vls - vl[2]) + " units and Donations of Vegetables of " + (vls - vl[1]) + " units";
                }
                else
                {
                    vls = vl[2];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Vegetables of " + (vls - vl[1]) + " units and Donations of Meat/Dairy of " + (vls - vl[0]) + " units";
                }

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

        public int ExecQuery(string query)
        {
            int vl = 0;
            conn = "Server=localhost;port=3306;uid=team013;pwd=team013;database=cs6400_sp17_team013;";
            connect = new MySqlConnection(conn);
            MySqlDataReader rdr;
            MySqlCommand scmd = new MySqlCommand(query, connect);
            connect.Open();
            rdr = scmd.ExecuteReader();
            if (rdr.Read())
            {
                //val[0] = rdr0["meat_dairy"].ToString();
                vl = Int32.Parse(rdr[0].ToString());
                System.Diagnostics.Debug.WriteLine(rdr[0].ToString());
            }
            return vl;
        }

        public string value
        {
            set
            {
                this.value = donat;
            }
            get
            {
                return this.donat;
            }

        }

    }
}
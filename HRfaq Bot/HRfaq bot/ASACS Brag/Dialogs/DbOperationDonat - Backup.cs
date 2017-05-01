using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace NewWave_Bot_Sample.Dialogs
{
    public class DbOperationDonatBU
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
                conn = "Server=localhost;port=3306;uid=team013;pwd=team013;database=cs6400_sp17_team013;";
                connect = new MySqlConnection(conn);
                string[] Query = new string[3];
                Query[0] = "SELECT SUM(Number_of_Units) AS meat_dairy FROM cs6400_sp17_team013.item " +
                            "WHERE Food_Supplies_type = 'meat/seafood' OR food_supplies_type = 'dairy/eggs';";
                Query[1] = "SELECT SUM(Number_of_Units) AS veg   FROM cs6400_sp17_team013.item "+
                            "WHERE Food_Supplies_type = 'vegetables'; ";
                Query[2] = "SELECT SUM(Number_of_Units) AS nut_grain FROM cs6400_sp17_team013.item "+
                            "WHERE Food_Supplies_type = 'nuts/grains/beans'; ";
                System.Diagnostics.Debug.WriteLine(Query[0]);
                System.Diagnostics.Debug.WriteLine(Query[1]);
                System.Diagnostics.Debug.WriteLine(Query[2]);
                MySqlCommand scmd0 = new MySqlCommand(Query[0], connect);
                MySqlCommand scmd1 = new MySqlCommand(Query[1], connect);
                MySqlCommand scmd2 = new MySqlCommand(Query[2], connect);
                MySqlDataReader rdr0, rdr1, rdr2;
                connect.Open();
                rdr0 = scmd0.ExecuteReader();
                rdr1 = scmd1.ExecuteReader();
                rdr2 = scmd2.ExecuteReader();
                if (rdr0.Read())
                {
                    //val[0] = rdr0["meat_dairy"].ToString();
                    vl[0] = Int32.Parse(rdr0["meat_dairy"].ToString());
                    System.Diagnostics.Debug.WriteLine(rdr0["meat_dairy"].ToString());
                }
                if (rdr1.Read())
                {
                    //val[1] = rdr0["veg"].ToString();
                    vl[1] = Int32.Parse(rdr1["veg"].ToString());
                    System.Diagnostics.Debug.WriteLine(rdr1["veg"].ToString());
                }
                if (rdr2.Read())
                {
                    //val[2] = rdr0["nut_grain"].ToString();
                    vl[2] = Int32.Parse(rdr2["nut_grain"].ToString());
                    System.Diagnostics.Debug.WriteLine(rdr2["nut_grain"].ToString());
                } 
                System.Diagnostics.Debug.WriteLine("The connection has been established: {0}", connect);
                System.Diagnostics.Debug.WriteLine("\r\n Your query has been executed check the database \r\n");

                if (vl[1] >= vl[0] && vl[1] >= vl[2])
                {
                    vls = vl[1];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Nuts/Grains/Beans of " + (vls - vl[2]) + "units and Doantions of Meat/Dairy of "+ (vls - vl[0]) +" units";
                }
                else if(vl[0] >= vl[1] && vl[0] >= vl[2])
                {
                    vls = vl[0];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Nuts/Grains/Beans of " + (vls - vl[2]) + "units and Doantions of Vegetables of " + (vls - vl[1]) + " units";
                }
                else
                {
                    vls = vl[2];
                    donat = "To Provide the maximum possible meals with current Inventory, Donations of Vegetables of " + (vls - vl[1]) + "units and Doantions of Meat/Dairy of " + (vls - vl[0]) + " units";
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
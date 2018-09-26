using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
namespace controlFallos
{
    class conexion
    {
      public  MySqlConnection dbcon;
        public MySqlConnection dbconection()
        {
            try
            {
                dbcon = new MySqlConnection("Server = 192.168.1.76; user=UPT; password = UPT2018; database = sistrefaccmant;");
                if (dbcon.State != System.Data.ConnectionState.Open) {
        
                    dbcon.Open();
                }
            
            }
            catch 
            {
             
                Application.Exit();
            }
            return dbcon;

        }
        public bool insertar(String sql)
        {
            try
            {
                dbconection();
                MySqlCommand cmd = new MySqlCommand(sql, dbcon);
                int i = cmd.ExecuteNonQuery();

                dbcon.Close();
                if (i > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                dbcon.Close();
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
          
                return false;
            }
        }
       
    
    }
   
}
    

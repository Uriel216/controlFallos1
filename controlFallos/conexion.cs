using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace controlFallos
{
    class conexion
    {
        public MySqlConnection dbcon;

        public string conex()
        {

            string path = Application.StartupPath + @"\conexion.txt";
            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path,true,Encoding.ASCII);
                sw.Write("localhost;usu;pass;sistrefaccmant");
                sw.Close();
            }
            StreamReader lector = new StreamReader(path);
            string var = lector.ReadLine();
            lector.Close();
            return var;
        }
        private static string conec()
        {
            StreamReader lector = new StreamReader(Application.StartupPath + @"\conexion.txt");
            string var = lector.ReadLine();
            lector.Close();
            return var;
        }
        public MySqlConnection dbconection()
        {
            try
            {
                validaciones v = new validaciones();
                string[] arreglo = conex().Split(';');
                string server = v.Desencriptar(arreglo[0]);
                string user = v.Desencriptar(arreglo[1]);
                string password = v.Desencriptar(arreglo[2]);
                string database = v.Desencriptar(arreglo[3]);
                dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                if (dbcon.State != System.Data.ConnectionState.Open)
                {

                    dbcon.Open();
                }

            }
            catch
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.GetType() == typeof(menuPrincipal))
                    {
                        int idUsuarioTemp;
                        menuPrincipal f = (menuPrincipal)frm;
                        idUsuarioTemp = f.idUsuario;
                        referencia(idUsuarioTemp);
                        Application.Exit();
                        break;
                    }
                }

                Application.Exit();
            }
            return dbcon;

        }

        public static MySqlConnection Conexion()
        {
            validaciones v = new validaciones();

            string[] arreglo = conec().Split(';');
            string server = v.Desencriptar(arreglo[0]);
            string user = v.Desencriptar(arreglo[1]);
            string password = v.Desencriptar(arreglo[2]);
            string database = v.Desencriptar(arreglo[3]);

            MySqlConnection conectar = new MySqlConnection("server=" + server + "; database=" + database + "; Uid=" + user + "; pwd=" + password + ";");
            conectar.Open();
            return conectar;

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

                MessageBox.Show(ex.HResult + ": " + ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);


                return false;
            }
        }

      public  void referencia(int idUsuario)
        {
            string path = Application.StartupPath + @"\contains.txt";
               StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
                sw.Write(idUsuario + ";");
                sw.Close();
        }
    }
}
    

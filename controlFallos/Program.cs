using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ress = false;
            string path = Application.StartupPath + @"\conexion.txt";
            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
                sw.Write("localhost;usu;pass;sistrefaccmant");
                sw.Close();
            }
            StreamReader lector = new StreamReader(path);

            var res = lector.ReadLine();
            lector.Close();
            try
            {
                validaciones v = new validaciones();
                string[] arreglo = res.Split(';');
                string server = v.Desencriptar(arreglo[0]);
                string user = v.Desencriptar(arreglo[1]);
                string password = v.Desencriptar(arreglo[2]);
                string database = v.Desencriptar(arreglo[3]);
                MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                if (dbcon.State != System.Data.ConnectionState.Open)
                {
                    dbcon.Open();
                    dbcon.Close();
                    ress = true;
                }

            }
            catch (Exception ex)
            {

               
                if (ex.HResult == -2147467259) {
                    obtenerHost();
                }else
                {
                    MessageBox.Show("Error No. " + ex.HResult + "\n " + ex.Source + "\n" + ex.StackTrace + "\n" + ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (ress)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new login());
                }
                catch (Exception ex)
                {

                    if (ex.HResult == -2147467259 || ex.HResult == -241633079)
                    {
                        Application.Restart();
                    }
                    else{
                        conexion c = new conexion();
                        c.dbconection();
                        Application.Exit();
                    }
                }
            }
        
            }
        static void obtenerHost()
        {
            foreach (Form frm in Application.OpenForms)
            {
                    frm.Close();
            }

            Application.EnableVisualStyles();
                Application.Run(new leerHost());
            
            }
    }
}

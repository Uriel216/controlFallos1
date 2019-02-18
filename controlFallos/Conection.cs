using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace controlFallos
{
    class Conection
    {
        private static string conec()
        {
            StreamReader lector = new StreamReader(Application.StartupPath + @"\conexion.txt");
            string var = lector.ReadLine();
            lector.Close();
            return var;
        }
        public static MySqlConnection Connection()
        {
            validaciones v = new validaciones();

            string[] arreglo = conec().Split(';');
            string server = v.Desencriptar(arreglo[0]);
            string user = v.Desencriptar(arreglo[1]);
            string password = v.Desencriptar(arreglo[2]);
            string database = v.Desencriptar(arreglo[3]);
            MySqlConnection con = new MySqlConnection(@"server=" + server + "; database=" + database + "; Uid=" + user + "; pwd=" + password + ";");
            con.Open();
            return con;
            //using(MySqlConnection con = new MySqlConnection("server = localhost; database = sistrefaccmant; Uid = root; pwd = ;"));
        }
    }
}

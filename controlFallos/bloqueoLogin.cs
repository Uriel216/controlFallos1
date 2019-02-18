using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    class bloqueoLogin
    {
        conexion c = new conexion();
        bool existeUsuario;
    
        public bool tipoBloqueo
        {
            set;
            get;
        }
        public int intentos
        {
            set;
            get;
        }
        public bloqueoLogin()
        {
            intentos = 0; 
        }
        public string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;

        }
        public void yaexisteUsuario(string usuario)
        {
            String existe = "SELECT count(iddato) as cuenta FROM datosistema WHERE usuario = '" + usuario + "'";
            MySqlCommand cm = new MySqlCommand(existe, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();
           
            if (dr.GetInt32("cuenta")>0)
            {
                existeUsuario = true;
            }
            else
            {
                existeUsuario = false;
            }
            dr.Close();
            c.dbcon.Close();
        }

        public void bloquear(string usuario)
        {
            try
            {
                if (existeUsuario)
                {
                    c.insertar("INSERT INTO bloqueologin(usuario, fechaHora, ipclient, statusbloqueo, tipobloqueo) VALUES('" + usuario + "',NOW(),'" + GetIPAddress() + "','1','0')");
                    tipoBloqueo = false;
                }
                else
                {
                    c.insertar("INSERT INTO bloqueologin(usuario, fechaHora, ipclient, statusbloqueo, tipobloqueo) VALUES('" + usuario + "',NOW(),'" + GetIPAddress() + "','1','1')");
                    tipoBloqueo = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            }

        public bool usuarionobloqueado(string usuario)
        {
            try
            {
                string bloqueado = "SELECT COUNT(idloginstatus) as cuenta FROM bloqueologin WHERE tipobloqueo = 0 and statusbloqueo = 1 and usuario='"+usuario+"'";
                MySqlCommand cm = new MySqlCommand(bloqueado, c.dbconection());
                int cuenta = Convert.ToInt32(cm.ExecuteScalar());
                c.dbcon.Close();
                if ( cuenta> 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }
              
            }
            catch (Exception ex)
            {
                c.dbcon.Close();
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }
        public bool noHainiciadoSesion(string usuario)
        {
            string sql = "SELECT statusiniciosesion FROM datosistema WHERE usuario= '" + usuario + "'";
            MySqlCommand m = new MySqlCommand(sql,c.dbconection());
            bool res = Convert.ToInt32(m.ExecuteScalar()) > 0;
            c.dbconection().Close();
            return res;   
        }
    }
}

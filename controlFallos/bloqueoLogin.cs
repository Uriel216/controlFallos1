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
                    c.dbcon.Close();
                }
                else
                {
                    c.insertar("INSERT INTO bloqueologin(usuario, fechaHora, ipclient, statusbloqueo, tipobloqueo) VALUES('" + usuario + "',NOW(),'" + GetIPAddress() + "','1','1')");
                    tipoBloqueo = true;
                    c.dbcon.Close();
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
                string bloqueado = "SELECT COUNT(idloginstatus) as cuenta FROM bloqueologin WHERE tipobloqueo = 0 and statusbloqueo = 1";
                MySqlCommand cm = new MySqlCommand(bloqueado, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                dr.Read();
           
                if (dr.GetInt32("cuenta") > 0)
                {
                    c.dbcon.Close();
                    return true;
                }
                else
                {
                    c.dbcon.Close();
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
    }
}

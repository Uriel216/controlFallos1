using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class login : Form
    {
        validaciones v = new validaciones();
        bloqueoLogin bl = new bloqueoLogin();
        conexion c = new conexion();
        TimeSpan diferencia;
        public login()
        {
            InitializeComponent();
            timer2.Start();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
            if (e.KeyChar==13) 
            {
                button1_Click(null,e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {                       
           
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int Wmsg, int Param, int IParam);
    

        private void status_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text))
            {

                string usu = txtgetusu.Text;

                if (!v.usuarioDesactivado(usu)) {
                    if (!bl.usuarionobloqueado(usu))
                    {
                        String pass = v.Encriptar(txtgetpass.Text);
                        string sql = "SELECT t2.idpersona as id,t2.empresa, t2.area as puesto FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE  t1.usuario COLLATE utf8_bin ='" + usu + "' and t1.password COLLATE utf8_bin ='" + pass + "' and status = '1'";
                        MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                        MySqlDataReader dr = cm.ExecuteReader();

                         if (dr.HasRows)
                        {

                            dr.Read();
                            bl.intentos = 0;
                            timer1.Stop();
                            timer2.Stop();
                                                  
                            menuPrincipal m = new menuPrincipal(dr.GetInt32("id"),dr.GetInt32("empresa"), dr.GetInt32("puesto"));
                            c.dbcon.Close();
                            m.Show();
                            this.Hide();
                        }
                        else
                        {

                            if (bl.intentos == 2)
                            {
                                bl.bloquear(txtgetusu.Text);
                                if (bl.tipoBloqueo)
                                {
                                    MessageBox.Show("El Sistema se ha bloqueado");
                                    plogin.Visible = false;
                                    lblsistemaBloqueado.Visible = false;
                                    btnlogin.Visible = false;
                                    lbllogin.Visible = false;
                                    bl.intentos = 0;
                                    lblintentos.Text = "";
                                    diferencia = new TimeSpan(0, 5, 0);
                                    timer1.Start();
                                }
                                else
                                {
                                    MessageBox.Show("El usuario se ha bloqueado por exceso de intentos Fallidos");
                                    bl.intentos = 0;
                                    lblintentos.Text = "";
                                    lblintentos.Visible = false;
                                }


                            }
                            else
                            {
                                bl.intentos = bl.intentos + 1;
                                lblintentos.Text = "Intentos Fallidos: " + bl.intentos;
                                bl.yaexisteUsuario(txtgetusu.Text);
                                lblintentos.Visible = false;
                                MessageBox.Show("Acceso Incorrecto");
                            }
                        }
                    } else
                    {
                        MessageBox.Show("El usuario ha sido Bloqueado");
                    }
                }
                else
                {
                    MessageBox.Show("El usuario ingresado ha sido desactivado por el Administrador");
                }
            }
                else
                {
                    MessageBox.Show("El usuario o la contraseña no pueden estar vacíos");
                }
            
                txtgetusu.Clear();
                txtgetpass.Clear();
            
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            diferencia = diferencia.Subtract(TimeSpan.FromSeconds(1));
            lblintentos.Text = "El Sistema se Desbloqueara en : " + diferencia.Minutes + ":" + diferencia.Seconds;
            if (diferencia.Minutes == 0 && diferencia.Seconds == 0)
            {
                plogin.Visible = true;
                lblsistemaBloqueado.Visible = false;
                btnlogin.Visible = true;
                lbllogin.Visible = true;
                bl.intentos = 0;
                lblintentos.Text = "";
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            c.insertar("UPDATE bloqueologin SET statusbloqueo = 0 WHERE TIME_TO_SEC(TIMEDIFF(TIME(NOW()),TIME(fechaHora))) > 300");
            c.dbcon.Close();
        }

        private void login_Load(object sender, EventArgs e)
        {try
            {
                string sql = "SELECT TIMEDIFF(TIME(NOW()),TIME(fechaHora)) as tiempo FROM bloqueologin WHERE ipclient = '" + bl.GetIPAddress() + "' and statusbloqueo = 1 and tipobloqueo =1";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    plogin.Visible = false;
                    lblsistemaBloqueado.Visible = true;
                    btnlogin.Visible = false;
                    lbllogin.Visible = false;
                    lblintentos.Visible = true;
                    bl.intentos = 0;
                    diferencia = new TimeSpan(0, 5, 0);
                    diferencia = diferencia.Subtract(TimeSpan.Parse(dr.GetString("tiempo")));
                    timer1.Start();
                    c.dbcon.Close();
                }
            }
            catch
            {
                Application.Exit();
            }
        }

        private void txtgetusu_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }
    }
}

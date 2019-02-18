using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using System.IO;

namespace controlFallos
{
    public partial class login : Form
    {
       
        validaciones v = new validaciones();
        bloqueoLogin bl = new bloqueoLogin();
        conexion c = new conexion();
        TimeSpan diferencia;
        //Creamos el delegado 
       static ThreadStart delegado = new ThreadStart(desbloquearUsuarios);
        //Creamos la instancia del hilo 
        Thread hilo = new Thread(delegado);

        static bool res = true;
        public login()
        {
            InitializeComponent();
            lbltitle.Left = (status.Width-lbltitle.Width)/2;
            lbltitle.Top = (status.Height - status.Height) / 2;
            //Iniciamos el hilo 
            if (hilo.ThreadState == ThreadState.Stopped)
            {
                    hilo.Start();
            }

            string path = Application.StartupPath + @"\contains.txt";
          
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                string line = sr.ReadLine();
                string[] idUsuarios =  line.Trim(';').Split(';');
                for(int i=0; i<idUsuarios.Length;i++)
                c.insertar("UPDATE datosistema SET statusiniciosesion = 0 WHERE usuariofkcpersonal ='" + idUsuarios[i] + "'");
                sr.Close();
                File.Delete(path);
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
            if (e.KeyChar==13) 
            {
                button1_Click(null,e);
            }
        }
      
       

        private void status_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text))
            {

                string usu = txtgetusu.Text;

                if (!v.usuarioDesactivado(usu)) {
                    if (!bl.usuarionobloqueado(usu))
                    {
                        if (!bl.noHainiciadoSesion(usu))
                        {


                            String pass = v.Encriptar(txtgetpass.Text);
                            string sql = "SELECT t2.idpersona as id,t2.empresa, t2.area FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE  t1.usuario COLLATE utf8_bin ='" + usu + "' and t1.password COLLATE utf8_bin ='" + pass + "' and status = '1'";
                            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                            MySqlDataReader dr = cm.ExecuteReader();

                            if (dr.HasRows)
                            {

                                dr.Read();
                                bl.intentos = 0;
                                menuPrincipal m = new menuPrincipal(dr.GetInt32("id"), dr.GetInt32("empresa"), dr.GetInt32("area"));
                                dr.Close();
                                c.dbcon.Close();
                                m.Show();
                                this.Hide();

                            }
                            else
                            {
                                dr.Close();
                                c.dbcon.Close();
                                if (bl.intentos == 2)
                                {
                                    bl.bloquear(txtgetusu.Text);
                                    if (bl.tipoBloqueo)
                                    {
                                        MessageBox.Show("El Sistema se ha bloqueado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        plogin.Visible = false;
                                        lblsistemaBloqueado.Visible = true;
                                        btnlogin.Visible = false;
                                        lbllogin.Visible = false;
                                        bl.intentos = 0;
                                        lblintentos.Text = "";
                                        diferencia = new TimeSpan(0, 5, 0);
                                        timer1.Start();
                                    }
                                    else
                                    {
                                        MessageBox.Show("El usuario se ha bloqueado por exceso de intentos Fallidos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    MessageBox.Show("Acceso Incorrecto",validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                        }else
                        {
                            MessageBox.Show("El usuario Tiene una Sesión Activa", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        } else
                        {
                            MessageBox.Show("El usuario ha sido Bloqueado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    
                }
                else
                {
                    MessageBox.Show("El usuario ingresado ha sido desactivado por el Administrador", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
                else
                {
                    MessageBox.Show("El usuario o la contraseña no pueden estar vacíos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
                txtgetusu.Clear();
                txtgetpass.Clear();
            
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
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
            string sql = "SELECT TIMEDIFF(TIME(NOW()),TIME(fechaHora)) as tiempo FROM bloqueologin WHERE ipclient = '" + bl.GetIPAddress() + "' and statusbloqueo = 1 and tipobloqueo =1";
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
                    //lblintentos.Visible = true;
                    bl.intentos = 0;
                    diferencia = new TimeSpan(0, 5, 0);
                    diferencia = diferencia.Subtract(TimeSpan.Parse(dr.GetString("tiempo")));
                    timer1.Start();
                    c.dbcon.Close();
                }
                dr.Close();
            }
            catch
            {
                this.Hide();
                
            }
        }

        private void txtgetusu_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }
     static void desbloquearUsuarios()
        {
       
            while (res) {
                MySqlConnection dbcon1 = new MySqlConnection("Server = 192.168.1.76; user=UPT; password = UPT2018; database = sistrefaccmant;");
                if (dbcon1.State != System.Data.ConnectionState.Open)
                {

                    dbcon1.Open();
                }
           
                MySqlCommand cmd = new MySqlCommand("UPDATE bloqueologin SET statusbloqueo = 0 WHERE TIME_TO_SEC(TIMEDIFF(TIME(NOW()),TIME(fechaHora))) > 300", dbcon1);
                cmd.ExecuteNonQuery();
                dbcon1.Close();
                Thread.Sleep(500);
            }
            }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            res = false;
            hilo.Abort();
            Application.ExitThread();
            Application.Exit();

        }

        private void lblsistemaBloqueado_Click(object sender, EventArgs e)
        {

        }
    }
}

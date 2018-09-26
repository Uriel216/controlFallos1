using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class menuPrincipal : Form
    {
        conexion c = new conexion();
        string idFolio = "";
        string consultaReportes = "";
        int tipoArea;
        int idUsuario;
        int puesto;
        public String nombre = "Sistema de Reporte de Fallos - ";
        Form form;
        Point defaultLocation = new Point(665, 11);
        Image newimg = Properties.Resources.Dbkel_CXkAE43aG;
        int empresa;
        public menuPrincipal(int idUsuario, int puesto,int empresa)
        {   InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.puesto = puesto;
            
            menuStrip1.Renderer = new MyRenderer();
        }
        public void TraerVariable(string f)
        {
      
            idFolio = f;
                        cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            Form hijo = null;
            if (this.tipoArea == 1) {
                lbltitle.Text = nombre + "Nuevo Reporte Supervision";
                lbltitle.Location = new Point(600, 11);
                var form = Application.OpenForms.OfType<nuevoReporteSupervision>().FirstOrDefault();
                 hijo = form ?? new nuevoReporteSupervision(f);
            }else if (this.tipoArea ==2)
            {
                lbltitle.Text = nombre + "Nuevo Reporte Mantenimiento";
                lbltitle.Location = new Point(600, 11);
                var form = Application.OpenForms.OfType<nuevoReporteMantenimiento>().FirstOrDefault();
                hijo = form ?? new nuevoReporteMantenimiento();
            }else if (tipoArea ==3)
            {
                lbltitle.Text = nombre + "Nuevo Reporte Almacén";
                lbltitle.Location = new Point(600, 11);
                var form = Application.OpenForms.OfType<TransInsumos>().FirstOrDefault();
                hijo = form ?? new TransInsumos(f);
            }
                AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
    
            NotificacionTri n = new NotificacionTri(tipoArea);
            n.Owner = this;
            DialogResult res= n.ShowDialog();
            
             }
        public void AddFormInPanel(Form fh)
        {
            this.form = fh;
            if (this.lblnumnotificaciones.Controls.Count != 0)
                this.lblnumnotificaciones.Controls.RemoveAt(0);
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.lblnumnotificaciones.Controls.Add(fh);
            this.lblnumnotificaciones.Tag = fh;
            fh.Show(); 
        }
        public void cerrar()
        {
            if (this.lblnumnotificaciones.Controls.Count != 0)
            {
                this.lblnumnotificaciones.Controls.RemoveAt(0);
                lblnumnotificaciones.BackgroundImage = controlFallos.Properties.Resources.Dbkel_CXkAE43aG;
                form.Close();
            }
        }
        private class MyRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (!e.Item.Selected) base.OnRenderMenuItemBackground(e);
                else
                {
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(Brushes.Crimson, rc);
                    e.Graphics.DrawRectangle(Pens.White, 1, 0, rc.Width - 2, rc.Height - 1);
                }
            }
        }
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            pictureBox3_Click(null, e);
            notifyIcon1.Visible = false;
        }
        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int Wmsg, int Param, int IParam);
        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {

            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void catálogoDeFallosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Catálogo de Fallos";
            lbltitle.Location = defaultLocation;
            var form = Application.OpenForms.OfType<catfallosGrales>().FirstOrDefault();
            catfallosGrales hijo = form ?? new catfallosGrales(idUsuario);
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void catálogoDePersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Catálogo de Personal";
            lbltitle.Location = defaultLocation;
            var form = Application.OpenForms.OfType<catPersonal>().FirstOrDefault();
            catPersonal hijo = form ?? new catPersonal(this.idUsuario,this.puesto,this.empresa,newimg);
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void catálogoDeUnidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Catálogo de Unidades";
            lbltitle.Location = defaultLocation;
            if (this.puesto == 1) {
                var form = Application.OpenForms.OfType<catUnidades>().FirstOrDefault();
                catUnidades hijo = form ?? new catUnidades(this.idUsuario);
                AddFormInPanel(hijo);

            } else if (this.puesto==2) {
                var form = Application.OpenForms.OfType<catUnidaesTRI>().FirstOrDefault();
                catUnidaesTRI hijo = form ?? new catUnidaesTRI(newimg);
                AddFormInPanel(hijo);
            } button1.Visible = true;
        }
        private void reporteNivelSupervisiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirReporte();
        }
        public void abrirReporte(){
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Nuevo Reporte Nivel Supervision";
            lbltitle.Location = new Point(600, 11);
            var form = Application.OpenForms.OfType<nuevoReporteSupervision>().FirstOrDefault();
            nuevoReporteSupervision hijo = form ?? new nuevoReporteSupervision();
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void reporteNivelTransisumosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Nuevo Reporte Nivel Transinsumos";
            lbltitle.Location = new Point(600, 11);
            var form = Application.OpenForms.OfType<TransInsumos>().FirstOrDefault();
            TransInsumos hijo = form ?? new TransInsumos();
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void reporteNivelMantenimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            lbltitle.Text = nombre + "Nuevo Reporte Nivel Mantenimiento";
            lbltitle.Location = new Point(600, 11);
            var form = Application.OpenForms.OfType<nuevoReporteMantenimiento>().FirstOrDefault();
            nuevoReporteMantenimiento hijo = form ?? new nuevoReporteMantenimiento();
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = newimg;
            button1.Visible = false;
            lbltitle.Text = nombre + "Menú Principal";
            lbltitle.Location = defaultLocation;
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
            timer2.Stop();
            login l = new login();
            l.Show();
            this.Close();

        }
        private void menuPrincipal_Load(object sender, EventArgs e)
        {
            if (this.puesto == 1)
            {
                newimg = Properties.Resources.Dbkel_CXkAE43aG;
            }else if(this.puesto==2)
            {
                newimg = Properties.Resources.Imagen2;
            }
            lblnumnotificaciones.BackgroundImage = newimg;
            var consultaPrivilegioscount = "SELECT count(idprivilegio) FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver > 0";
            MySqlCommand count = new MySqlCommand(consultaPrivilegioscount, c.dbconection());
            
            if (Convert.ToInt32(count.ExecuteScalar().ToString())==0)
            {
                MessageBox.Show("No tiene privilegios para navegar por el sistema. Contacte a su administrador de area");
                login l = new login();
                l.Show();
                this.Close();
            }
            c.dbcon.Close();
            var consultaPrivilegios = "SELECT namForm FROM privilegios WHERE usuariofkcpersonal= '"+this.idUsuario+"' and ver > 0";
           MySqlCommand cm = new MySqlCommand(consultaPrivilegios, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
           while (dr.Read())
            {
                PrivilegiosVisibles(dr.GetString("namForm"));
           }
            c.dbcon.Close();
            obtenerconsulta();
            

        }
        void obtenerconsulta()
        {
            string consulta = "SELECT ver as tipo FROM privilegios WHERE namform='bnotif' and usuariofkcpersonal = " + this.idUsuario;
            MySqlCommand cm = new MySqlCommand(consulta, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt32("tipo")>0) {
                    switch (dr.GetInt32("tipo"))
                    {
                        case 1:
                            consultaReportes = "SELECT COUNT(t1.idReporte) as cuenta FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad WHERE t1.seen = 0 and (t1.Estatus='Liberada' or t1.Estatus='Reprogramada' )";
                            tipoArea = 1;
                            break;
                        case 2:
                            consultaReportes = "SELECT count(idReporteSupervicion) as cuenta FROM reportesupervicion WHERE seen = 0";
                            tipoArea = 2;
                            break;
                        case 3:
                            consultaReportes = "SELECT count(IdReporte) as cuenta FROM reportemantenimiento  WHERE StatusRefacciones = 'Se Requieren Refacciones' and seen=0";
                            tipoArea = 3;
                            break;
                    }
                    timer2.Start();
                }
            }
            c.dbcon.Close();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
          
            MySqlCommand cm = new MySqlCommand(consultaReportes, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();
         
            lblnotif.Text = dr.GetString("cuenta");
            c.dbcon.Close();
            if (lblnotif.Text.Length==1)
            {
                lblnotif.Location = new Point(77, 13);
            }else if (lblnotif.Text.Length == 2)
            {
                lblnotif.Location = new Point(70, 13);
            }
       
            
        }
        void PrivilegiosVisibles(string nombreForm)
        {
            nombreForm = nombreForm.ToLower();   
            if (nombreForm == "catfallosgrales")
            {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDeFallosToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "catpersonal" || nombreForm == "catpuestos")
            {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDePersonalToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "catunidades" || nombreForm == "catservicios" || nombreForm == "catempresas")
            {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDeUnidadesToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "form1")
            {
                if (!reportesToolStripMenuItem.Visible)
                {
                    reportesToolStripMenuItem.Visible = true;
                    creaciónDeReportesToolStripMenuItem.Visible = true;

                }
                reporteNivelSupervisiónToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "mantenimiento")
            {
                if (!reportesToolStripMenuItem.Visible)
                {
                    reportesToolStripMenuItem.Visible = true;
                    creaciónDeReportesToolStripMenuItem.Visible = true;

                }
                reporteNivelMantenimientoToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "almacen")
            {
                if (!reportesToolStripMenuItem.Visible)
                {
                    reportesToolStripMenuItem.Visible = true;
                    creaciónDeReportesToolStripMenuItem.Visible = true;

                }
                reporteNivelTransisumosToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "bnotif")
            {
                pbnotif.Visible = true;
                lblnotif.Visible = true;
            }
            if (nombreForm == "catrefacciones")
            {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDeRefaccionesToolStripMenuItem.Visible = true;
            }
            if (nombreForm =="ordencompra")
            {
                requisicionesToolStripMenuItem.Visible = true;
                generarOrdenDeCompraToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "catproveedores") {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDeProveedoresToolStripMenuItem.Visible = true;
            }
        }
        private void catálogoDeRefaccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lbltitle.Text = nombre + "Catálogo de Refacciones";
            lbltitle.Location = defaultLocation;
            var form = Application.OpenForms.OfType<catRefacciones>().FirstOrDefault();
            catRefacciones hijo = form ?? new catRefacciones(newimg,this.idUsuario);
            AddFormInPanel(hijo);
            button1.Visible = true;
        }

        private void catálogoDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrar();
            lblnumnotificaciones.BackgroundImage = null;
            lbltitle.Text = nombre + "Catálogo de Refacciones";
            lbltitle.Location = defaultLocation;
            var form = Application.OpenForms.OfType<catProveedores>().FirstOrDefault();
            catProveedores hijo = form ?? new catProveedores(this.idUsuario, newimg);
            AddFormInPanel(hijo);
            button1.Visible = true;
        }
    }
    }

using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class menuPrincipal : Form
    {
        conexion c = new conexion();
        string idFolio = "";
        string consultaReportes = "";
        int tipoArea;
        public int idUsuario;
        public String nombre = "";
        Form form;
        int tipo;
        Point defaultLocation = new Point(1560, 13);
        Image newimg = Properties.Resources.Dbkel_CXkAE43aG;
        validaciones v = new validaciones();
        int empresa;
        int area;
        bool res=true;
      public  int resAnterior=0;
        Thread BuscarValidaciones;
        Thread session;
        delegate void obtenerNotificacionesD();
        delegate void sesioncaducada();
        Thread hilo;
        Thread hiloMuestraNotificacion;
        int totalAnteriorPedidos;
        public menuPrincipal(int idUsuario, int empresa, int area)
        {   InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            if (idUsuario==0 || empresa==0 || area==0)
            {
                login l = new login();
                l.Show();
                this.Close();
            }
            paraTipo();
            menuStrip1.Renderer = new MyRenderer();
        }
        void paraTipo() {
            if (empresa==1)
            {
                tipo = 1;
            }
            else if (empresa == 2)
            {
                if (area==1)
                {
                    tipo = 2;
                }else
                {
                    tipo = 3;
                }
            }
        }
       public void irArefacciones(string idRefaccion)
        {
            iraRefacciones(idRefaccion);
        }
        public void TraerVariable(string f)
        {
            
            idFolio = f;
          
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
           
            if (this.tipoArea == 1)
            {

               
                abrirReporte();
                
            }
            else if (this.tipoArea == 2)
            {
             
                abrirMantenimiento();   
            }
            else if (tipoArea == 3)
            {
              
                  
                abrirAlmacen();
            }
              

            
            }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblnotif.Text)>0) {
                Opacity = 0.9;
                DialogResult res;
                if (empresa == 1) {

                    NotificacionTri n = new NotificacionTri(tipoArea);
                    n.Owner = this;
                    res = n.ShowDialog();
                }
                else
                {
                    if (area == 1)
                    {
                        NotificacionTri n = new NotificacionTri(tipoArea);
                        n.Owner = this;
                        res = n.ShowDialog();
                    } else
                    {
                        NotificacionAlmacen n = new NotificacionAlmacen();
                        n.Owner = this;
                        res = n.ShowDialog();
                    }
                }
                if (res == DialogResult.Cancel)
                {
                    Opacity = 1;
                }
            }
        }
        public void AddFormInPanel(Form fh)
        {
            this.form = fh;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.lblnumnotificaciones.Controls.Add(fh);
            this.lblnumnotificaciones.Tag = fh;
            fh.Show();
     }
        public bool cerrar()
        {
            if (this.lblnumnotificaciones.Controls.Count != 0)
            {
                if (MessageBox.Show("¿Está Seguro Que Desea Salir del Formulario?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes) {
                    this.lblnumnotificaciones.Controls.RemoveAt(0);
               
                    form.Close();
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return true;
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
            foreach (Form frm in Application.OpenForms)

            {
                if (frm.Owner != null)
                {
                    frm.Close();
                    frm.DialogResult = DialogResult.Cancel;
                }
                }

                this.Show();
            this.WindowState = FormWindowState.Normal;
            pictureBox3_Click(null, e);
        }
        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
       
        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {

            v.mover(sender, e, this);
        }
        private void catálogoDeFallosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Catálogo de Fallos";
                this.Text = "Catálogo de Fallos";
                lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = true;
                catálogoDeFallosToolStripMenuItem.Enabled = false;
                catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = true;
                historialDeModificacionesToolStripMenuItem.Enabled = true;
                var form = Application.OpenForms.OfType<catfallosGrales>().FirstOrDefault();
                catfallosGrales hijo = form ?? new catfallosGrales(idUsuario,empresa,area,this);
                AddFormInPanel(hijo);
            }
         
        }
        private void catálogoDePersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
              if (cerrar())
                {
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Registro de Personal";
                    this.Text = "Sistema de Reporte de Fallos - Registro de Personal";
                    lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = false;
                catálogoDeFallosToolStripMenuItem.Enabled = true;
                catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = true;
                historialDeModificacionesToolStripMenuItem.Enabled = true;
                var form = Application.OpenForms.OfType<catPersonal>().FirstOrDefault();
                    catPersonal hijo = form ?? new catPersonal(this.idUsuario, this.empresa, this.area, newimg);
                    AddFormInPanel(hijo);
  
                }
            
           
        }
        private void catálogoDeUnidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Catálogo de Unidades";
                this.Text = "Catálogo de Unidades";
                lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = true;
                catálogoDeFallosToolStripMenuItem.Enabled = true;
                catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                catálogoDeUnidadesToolStripMenuItem.Enabled = false;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = true;
                historialDeModificacionesToolStripMenuItem.Enabled = true;
                if (this.empresa == 1)
                {
                    var form = Application.OpenForms.OfType<catUnidades>().FirstOrDefault();
                    catUnidades hijo = form ?? new catUnidades(this.idUsuario,empresa,area);
                    AddFormInPanel(hijo);

                }
                else if (empresa == 2)
                {
                    var form = Application.OpenForms.OfType<catUnidaesTRI>().FirstOrDefault();
                    catUnidaesTRI hijo = form ?? new catUnidaesTRI(this.idUsuario,newimg,empresa,area);
                    AddFormInPanel(hijo);
                }
            }
        }
        private void reporteNivelSupervisiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirReporte();
        }
        public void abrirReporte(){
            string name ="";
            if (form!=null) name = form.Name;
                if (name!= "Supervisión") {
                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Supervisión";
                    this.Text = lbltitle.Text;
                    lbltitle.Location = new Point(1591, 13);
                    catálogoDePersonalToolStripMenuItem.Enabled = true;
                    catálogoDeFallosToolStripMenuItem.Enabled = true;
                    catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                    catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                    catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                    reporteSupervicionToolStripMenuItem.Enabled = false;
                    reporteMantenimientoToolStripMenuItem.Enabled = true;
                    reporteAlmacenToolStripMenuItem1.Enabled = true;
                    requisicionesToolStripMenuItem.Enabled = true;
                    historialDeModificacionesToolStripMenuItem.Enabled = true;
                    var form = Application.OpenForms.OfType<Supervisión>().FirstOrDefault();
                    Supervisión hijo = form ?? new Supervisión(this.idUsuario, empresa, area);

                    AddFormInPanel(hijo);
                }
            }else
            {
                Supervisión p =  (Supervisión)form;
                p.cargarDAtos();
            }
        }
        private void reporteNivelTransisumosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirAlmacen();
        }
        void abrirAlmacen()
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "TRI")
            {
                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Almacén";
                    this.Text = lbltitle.Text;
                    lbltitle.Location = new Point(1575, 13);
                    catálogoDePersonalToolStripMenuItem.Enabled = true;
                    catálogoDeFallosToolStripMenuItem.Enabled = true;
                    catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                    catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                    catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                    reporteSupervicionToolStripMenuItem.Enabled = true;
                    reporteMantenimientoToolStripMenuItem.Enabled = true;
                    reporteAlmacenToolStripMenuItem1.Enabled = false;
                    requisicionesToolStripMenuItem.Enabled = true;
                    historialDeModificacionesToolStripMenuItem.Enabled = true;
                    var form = Application.OpenForms.OfType<TRI>().FirstOrDefault();
                    TRI hijo = form ?? new TRI(this.idUsuario, empresa, area);
                    AddFormInPanel(hijo);
                }
            }else
            {
                TRI t = (TRI)form;
                t.CargarDatos();
            }
        }
        private void reporteNivelMantenimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirMantenimiento();
        }
      void abrirMantenimiento()
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "FormFallasMantenimiento")
            {

                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Mantenimiento";
                    this.Text = lbltitle.Text;
                    lbltitle.Location = new Point(1575, 13);
                    catálogoDePersonalToolStripMenuItem.Enabled = true;
                    catálogoDeFallosToolStripMenuItem.Enabled = true;
                    catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                    catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                    catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                    reporteSupervicionToolStripMenuItem.Enabled = true;
                    reporteMantenimientoToolStripMenuItem.Enabled = false;
                    reporteAlmacenToolStripMenuItem1.Enabled = true;
                    requisicionesToolStripMenuItem.Enabled = true;
                    historialDeModificacionesToolStripMenuItem.Enabled = true;
                    var form = Application.OpenForms.OfType<FormFallasMantenimiento>().FirstOrDefault();
                    FormFallasMantenimiento hijo = form ?? new FormFallasMantenimiento(idUsuario,empresa,area);
                    AddFormInPanel(hijo);
                }
            }else
            {
                FormFallasMantenimiento m = (FormFallasMantenimiento)form;
                m.metodoCarga();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta Seguro Que Quiere Cerrar Sesión?",validaciones.MessageBoxTitle.Confirmar.ToString(),MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes) {
                this.Close();
            }
        }
        private void menuPrincipal_Load(object sender, EventArgs e)
        {
           
            if (this.empresa == 1)
            {
                newimg = Properties.Resources.Dbkel_CXkAE43aG;
            }else if(this.empresa==2)
            {
                newimg = Properties.Resources.Imagen2;
            }
            lblnumnotificaciones.BackgroundImage = newimg;
            var consultaPrivilegioscount = "SELECT count(idprivilegio) FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver > 0";
            MySqlCommand count = new MySqlCommand(consultaPrivilegioscount, c.dbconection());
            if (Convert.ToInt32(count.ExecuteScalar().ToString()) == 0)
            {
                MessageBox.Show("No tiene privilegios para navegar por el sistema. Contacte a su administrador de area", v.sistema(),MessageBoxButtons.OK,MessageBoxIcon.Error);
               
                this.Close();
                   
            }
            else
            {
                c.referencia(idUsuario);
                c.dbcon.Close();
                var consultaPrivilegios = "SELECT namForm FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver > 0";
                MySqlCommand cm = new MySqlCommand(consultaPrivilegios, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    PrivilegiosVisibles(dr.GetString("namForm"));
                }
                dr.Close();
                c.dbcon.Close();
                obtenerconsulta();
                cambiarstatus(1);
              //Creamos el delegado 
              ThreadStart delegado = new ThreadStart(obtenerNotificaciones);
                //Creamos la instancia del hilo 
                hilo = new Thread(delegado);
                //Iniciamos el hilo 
                hilo.Start();
                timer1.Start();
                // predeterminado(e);
                if (empresa == 2 && area == 1)
                {
                    BuscarValidaciones = new Thread(new ThreadStart(buscaValidar));
                    BuscarValidaciones.Start();
                }else
                {
                    notifyIcon1.Dispose();
                }
            }
            ThreadStart delegatse = new ThreadStart(sesion);
            //Creamos la instancia del hilo 
            session = new Thread(delegatse);
            //Iniciamos el hilo 
           session.Start();

        }
        void sesion()
        {
            while (res)
            {
                if (this.InvokeRequired)
                {
                    string[] arreglo = c.conex().Split(';');
                    string server = v.Desencriptar(arreglo[0]);
                    string user = v.Desencriptar(arreglo[1]);
                    string password = v.Desencriptar(arreglo[2]);
                    string database = v.Desencriptar(arreglo[3]);
                    MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                    dbcon.Open();
                    string sql = "SELECT statusiniciosesion FROM datosistema WHERE usuariofkcpersonal='" + idUsuario + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, dbcon);
                    int res = Convert.ToInt32(cmd.ExecuteScalar());
                    if (res == 0)
                   {
                       sesioncaducada sesioncaducada = new sesioncaducada(sesion);
                       this.Invoke(sesioncaducada);
                   }
                    dbcon.Close();
                   Thread.Sleep(2000);
                  
                }else
                {
                 MessageBox.Show("La Sesión Ha Caducado", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    c.insertar("UPDATE datosistema SET statusiniciosesion=1  WHERE usuariofkcpersonal='" + idUsuario+"'");
                    // Close();
                    return;
                }


            }
        }
        void salir()
        {
            
        }
        void buscaValidar()
        {
            while (res)
            {try
                {
                    string[] arreglo = c.conex().Split(';');
                    string server = v.Desencriptar(arreglo[0]);
                    string user = v.Desencriptar(arreglo[1]);
                    string password = v.Desencriptar(arreglo[2]);
                    string database = v.Desencriptar(arreglo[3]);
                    MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                    dbcon.Open();
                    string sql = "SELECT COUNT(idestatusValidado) FROM estatusvalidado WHERE seen = 0";
                    MySqlCommand cmd = new MySqlCommand(sql, dbcon);
                    int res = Convert.ToInt32(cmd.ExecuteScalar());
                    dbcon.Close();
                    if (res != totalAnteriorPedidos)
                    {
                        mostrarNotificacion = new Thread(new ThreadStart(MostrarNotificacion));
                        mostrarNotificacion.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                Thread.Sleep(5000);
            }
        }
        Thread mostrarNotificacion;
        void MostrarNotificacion()
        {
            string[] arreglo = c.conex().Split(';');
            string server = v.Desencriptar(arreglo[0]);
            string user = v.Desencriptar(arreglo[1]);
            string password = v.Desencriptar(arreglo[2]);
            string database = v.Desencriptar(arreglo[3]);
            MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
            try
            {
                dbcon.Open();
            }
            catch
            {
                Close();
            }
            string cadena = "";
            string sql = "SELECT t2.folio FROM estatusValidado as t1 INNER JOIN reportesupervicion as t2 On t1.idreportefkreportesupervicion = idReporteSupervicion WHERE t1.seen = 0";
            MySqlCommand cm = new MySqlCommand(sql, dbcon);
            MySqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                cadena = cadena + "\n" + dr.GetString("folio");
            }
            dr.Close();
            dbcon.Close();
            notifyIcon1.BalloonTipText = "Se Han Validado Las Refacciones de Los Reportes: " + cadena;
            notifyIcon1.ShowBalloonTip(5000);
           
        }
        public void predeterminado(EventArgs e)
        {
            if (empresa==1)
            {
                reporteNivelSupervisiónToolStripMenuItem_Click(null,e);
            }
            else
            {
                if (area==1)
                {
                    reporteNivelMantenimientoToolStripMenuItem_Click(null,e);
                }else
                {
                    reporteNivelTransisumosToolStripMenuItem_Click(null,e);
                }
            }
        }
        void obtenerconsulta()
        {
      switch (tipo)
                {
                    case 1:
                        consultaReportes = "SELECT COUNT(t1.idReporte) as cuenta FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad WHERE t1.seen = 0 and (t1.Estatus='Liberada' or t1.Estatus='Reprogramada') AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate(); ";
                        tipoArea = 1;
                        break;
                    case 2:
                        consultaReportes = "SELECT count(idReporteSupervicion) as cuenta FROM reportesupervicion WHERE seen = 0 and FechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate()";
                        tipoArea = 2;
                        break;

                case 3:
                    consultaReportes = "SELECT (count(IdReporte)+(SELECT count(idrefaccion) as cuenta FROM crefacciones  WHERE existencias <=media OR existencias<= abastecimiento OR datediff(proximoAbastecimiento,curdate()) <=20 and status=1)) as cuenta FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.foliofkSupervicion= t2.idReporteSupervicion  WHERE StatusRefacciones = 'Se Requieren Refacciones' and seenAlmacen=0 AND t2.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                    tipoArea = 3;
                    break;
                }
            
        }

        private void obtenerNotificaciones()
        {
            int res = 0;
            string[] arreglo = c.conex().Split(';');
            string server = v.Desencriptar(arreglo[0]);
            string user = v.Desencriptar(arreglo[1]);
            string password = v.Desencriptar(arreglo[2]);
            string database = v.Desencriptar(arreglo[3]);
            MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
            try
            {
                dbcon.Open();
                MySqlCommand cm = new MySqlCommand(consultaReportes, dbcon);
                res = Convert.ToInt32(cm.ExecuteScalar());
                dbcon.Close();
            }
            catch
            {
                Close();
            }
              
                if (this.InvokeRequired)
                {
                    obtenerNotificacionesD delegado = new obtenerNotificacionesD(obtenerNotificaciones);
                    this.Invoke(delegado);
                }
                else
                {
                    pbnotif.BackgroundImage = null;
                    if (res > 0)
                    {
                        if (resAnterior != res)
                        {
                            resAnterior = res;
                            ThreadStart delegado = new ThreadStart(MostrarNotifiacacion);
                            //Creamos la instancia del hilo 
                            hiloMuestraNotificacion = new Thread(delegado);
                            //Iniciamos el hilo 
                            hiloMuestraNotificacion.Start();
                        }

                        pbnotif.BackgroundImage = controlFallos.Properties.Resources.notification__3_1;

                    }
                    else
                    {

                        pbnotif.BackgroundImage = controlFallos.Properties.Resources.notification__4_;

                    }
                    lblnotif.Text = "" + res;
                    if (lblnotif.Text.Length == 1)
                    {
                        lblnotif.Location = new Point(95, 10);
                    }
                    else if (lblnotif.Text.Length == 2)
                    {
                        lblnotif.Location = new Point(90, 10);
                    }
                }

                if(hilo!=null)
                hilo.Abort();
         
            }
        private void MostrarNotifiacacion()
        {
       
            notif.BalloonTipText= "Tienes Nuevas Notificaciones ("+resAnterior+")";
            notif.ShowBalloonTip(6000); 
       


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
                if (area==1) {
                    if (!catálogosToolStripMenuItem.Visible)
                    {
                        catálogosToolStripMenuItem.Visible = true;
                    }
                    catálogoDeUnidadesToolStripMenuItem.Visible = true;
                }
                }
            if (nombreForm == "form1")
            {
                reporteSupervicionToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "mantenimiento")
            {
                reporteMantenimientoToolStripMenuItem.Visible = true;
            }
            if (nombreForm == "almacen")
            { 
                reporteAlmacenToolStripMenuItem1.Visible = true;
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
            }
            if (nombreForm == "catproveedores" || nombreForm == "catGiros") {
                if (!catálogosToolStripMenuItem.Visible)
                {
                    catálogosToolStripMenuItem.Visible = true;
                }
                catálogoDeProveedoresToolStripMenuItem.Visible = true;
            }
            if (nombreForm=="historial")
            {
                historialDeModificacionesToolStripMenuItem.Visible = true;
            }
            if (nombreForm=="changeiva")
            {
                actualizaciónDeIVAToolStripMenuItem.Visible = true;
            }

        }
        private void catálogoDeRefaccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iraRefacciones(null);
           }
        void iraRefacciones(string idref)
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "catRefacciones")
            {
                if (cerrar())
                {

                    lblnumnotificaciones.BackgroundImage = null;
                    lbltitle.Text = nombre + "Catálogo de Refacciones";
                    this.Text = "Catálogo de Refacciones";
                    lbltitle.Location = defaultLocation;
                    catálogoDePersonalToolStripMenuItem.Enabled = true;
                    catálogoDeFallosToolStripMenuItem.Enabled = true;
                    catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                    catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                    catálogoDeRefaccionesToolStripMenuItem.Enabled = false;
                    reporteSupervicionToolStripMenuItem.Enabled = true;
                    reporteMantenimientoToolStripMenuItem.Enabled = true;
                    reporteAlmacenToolStripMenuItem1.Enabled = true;
                    requisicionesToolStripMenuItem.Enabled = true;
                    historialDeModificacionesToolStripMenuItem.Enabled = true;
                    var form = Application.OpenForms.OfType<catRefacciones>().FirstOrDefault();
                    catRefacciones hijo;
                    if (!string.IsNullOrWhiteSpace(idref))
                    {
                        hijo = form ?? new catRefacciones(newimg, this.idUsuario, idref.ToString());
                    }
                    else
                    {
                        hijo = form ?? new catRefacciones(newimg, this.idUsuario, empresa, area);
                    }
                    AddFormInPanel(hijo);
                }
            }else
            {
                catRefacciones c = (catRefacciones) form;
                c.actualizarTabla(idref);
            }
        }

        private void catálogoDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lbltitle.Text = nombre + "Catálogo de Proveedores";
                this.Text = "Catálogo de Proveedores";
                lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = true;
                catálogoDeFallosToolStripMenuItem.Enabled = true;
                catálogoDeProveedoresToolStripMenuItem.Enabled = false;
                catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = true;
                historialDeModificacionesToolStripMenuItem.Enabled = true;
                var form = Application.OpenForms.OfType<catProveedores>().FirstOrDefault();
                catProveedores hijo = form ?? new catProveedores(this.idUsuario, newimg,empresa,area);
                AddFormInPanel(hijo);
            }
            }

        private void catálogoDeFallosToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
         
        }

        private void requisicionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Generar Orden de Compra";
                lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = true;
                catálogoDeFallosToolStripMenuItem.Enabled = true;
                catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = false;
                historialDeModificacionesToolStripMenuItem.Enabled = true;
                var form = Application.OpenForms.OfType<OrdenDeCompra>().FirstOrDefault();
                OrdenDeCompra hijo = form ?? new OrdenDeCompra(idUsuario,empresa,area);
                AddFormInPanel(hijo);
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void menuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            cambiarstatus(0);
            res = false;
            if (session!=null) {
                session.Abort();

            } if (empresa==2 && area==1)
            {
                if (BuscarValidaciones!=null) {
                    BuscarValidaciones.Abort();
                }
                    if (mostrarNotificacion!=null)
                {

                    mostrarNotificacion.Abort();
                }
            }
            if (hilo!=null)
            {
                hilo.Abort();
            }
       
          
            timer1.Stop();
            
            if (this.lblnumnotificaciones.Controls.Count != 0)
            {
                this.lblnumnotificaciones.Controls.RemoveAt(0);
               
            }
            notifyIcon1.Dispose();
            notif.Dispose();
            login login = new login();
            login.Show();
        }

        private void catálogosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)

        {
            obtenerNotificaciones();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            c.insertar("UPDATE estatusValidado SET seen = 1 WHERE seen = 0");
            totalAnteriorPedidos = 0;
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void notifyIcon1_BalloonTipClicked_1(object sender, EventArgs e)
        {
            c.insertar("UPDATE estatusValidado SET seen = 1 WHERE seen = 0");
            totalAnteriorPedidos = 0;
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }
        public void cambiarstatus(object i)
        {
            c.insertar("UPDATE datosistema SET statusiniciosesion = "+i+ " WHERE usuariofkcpersonal ='"+idUsuario+"'");
        }

        private void catálogoDePersonalToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
     
           ((ToolStripMenuItem) sender).ForeColor = Color.White;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void historialDeModificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = lbltitle.Text = nombre + "Historial de Modificaciones";
                lbltitle.Location = defaultLocation;
                catálogoDePersonalToolStripMenuItem.Enabled = true;
                catálogoDeFallosToolStripMenuItem.Enabled = true;
                catálogoDeProveedoresToolStripMenuItem.Enabled = true;
                catálogoDeUnidadesToolStripMenuItem.Enabled = true;
                catálogoDeRefaccionesToolStripMenuItem.Enabled = true;
                reporteSupervicionToolStripMenuItem.Enabled = true;
                reporteMantenimientoToolStripMenuItem.Enabled = true;
                reporteAlmacenToolStripMenuItem1.Enabled = true;
                requisicionesToolStripMenuItem.Enabled = true;
                historialDeModificacionesToolStripMenuItem.Enabled = false;
                var form = Application.OpenForms.OfType<modificaciones>().FirstOrDefault();
                modificaciones hijo = form ?? new modificaciones(empresa,area);
                AddFormInPanel(hijo);
            }
        }

        private void lbltitle_DoubleClick(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void catálogosToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).BackColor = Color.Crimson;
        }

        private void catálogosToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void actualizaciónDeIVAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            catIVA cat = new catIVA(empresa,area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void notifyIcon1_Click(object sender, MouseEventArgs e)
        {

        }
    }
    }

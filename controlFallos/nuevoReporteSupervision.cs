using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace controlFallos
{

    public partial class nuevoReporteSupervision : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        public nuevoReporteSupervision()
        {
            InitializeComponent();
        }
        
        public nuevoReporteSupervision(string idFolio)
        {
            InitializeComponent();
           
        }



        public void CargarDatos(TextBox cajaTexto)
        {

            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
            string consulta = @"Select concat(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres)as supervisor,t1.idPersona from cpersonal as t1 inner join reportesupervicion as t2 on t1.idPersona=t2.SupervisorfkCPersonal  ";
            MySqlCommand cmd = new MySqlCommand(consulta, c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                    namesCollection.Add(dr["supervisor"].ToString());

            }

            dr.Close();


            txtBuscSupervisor.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBuscSupervisor.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBuscSupervisor.AutoCompleteCustomSource = namesCollection;
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            cargarDAtos();
            CargarDatos(txtBuscSupervisor);
         

            MySqlCommand cmd4 = new MySqlCommand("SELECT(iddescfallo),(descfallo ) from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idFalloGral=t1.falloGralfkcfallosgrales;", c.dbconection());
          
            MySqlDataAdapter da4 = new MySqlDataAdapter(cmd4);
            DataTable dan4 = new DataTable();
            da4.Fill(dan4);

            cmbDescFallo.ValueMember = "iddescfallo";
            
            cmbDescFallo.DisplayMember = "descfallo";
            cmbDescFallo.DataSource = dan4;
            cmbBuscarDescripcion.ValueMember = "iddescfallo";
            cmbBuscarDescripcion.DisplayMember = "descfallo";
            cmbBuscarDescripcion.DataSource = dan4;
           
            cmbBuscarDescripcion.SelectedIndex = -1;


            MySqlDataAdapter da5 = new MySqlDataAdapter(cmd4);
            MySqlCommand cmd5 = new MySqlCommand("SELECT (iddescfallo),(descfallo ) from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idFalloGral=t1.falloGralfkcfallosgrales;", c.dbconection());
            DataTable dan5 = new DataTable();
            da5.Fill(dan5);

            cmbDescFallo.ValueMember = "iddescfallo";
            cmbDescFallo.DisplayMember = "descfallo";
            cmbDescFallo.DataSource = dan5;
            cmbDescFallo.SelectedIndex = -1;



            MySqlCommand cmd3 = new MySqlCommand("Select concat(clave,'-',nombre)as servicio, idservicio,status from cservicios", c.dbconection());

            MySqlDataAdapter da3 = new MySqlDataAdapter(cmd3);
            DataTable dan = new DataTable();
            da3.Fill(dan);

            cmbServicio.ValueMember = "idservicio";
            cmbServicio.DisplayMember = "servicio";
            cmbServicio.DataSource = dan;
            cmbServicio.SelectedIndex = -1;

            MySqlCommand cmd2 = new MySqlCommand("select * from cunidades;", c.dbconection());
            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
            DataTable dan2 = new DataTable();
            da2.Fill(dan2);
            cmbUnidad.ValueMember = "idunidad";
            cmbUnidad.DisplayMember = "ECO";
            cmbUnidad.DataSource = dan2;
            cmbUnidad.SelectedIndex = -1;

            MySqlCommand cmd1 = new MySqlCommand("select * from cunidades;", c.dbconection());
            MySqlDataAdapter da1 = new MySqlDataAdapter(cmd1);
            DataTable dan1 = new DataTable();
            da1.Fill(dan1);
            cmbBuscarUnidad.ValueMember = "idunidad";
            cmbBuscarUnidad.DisplayMember = "ECO";
            cmbBuscarUnidad.DataSource = dan1;
            cmbBuscarUnidad.SelectedIndex = -1;






        }
        public void cargarDAtos()
        {

         

            string sql = @"SELECT  concat('TRA0',substring(Folio,length(Folio)-6,7)+1) as folio FROM reportesupervicion WHERE idReporteSupervicion = (SELECT MAX(idReporteSupervicion) FROM reportemantenimiento);;";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());

            string Folio = Convert.ToString(cmd.ExecuteScalar());
            if (Folio=="")
            {
                Folio = "TRA0900000";
            }

            lblFolio.Text = Folio.ToString();
            MySqlDataAdapter cargar = new MySqlDataAdapter("SET lc_time_names = 'es_ES'; select t1.Folio,t2.ECO,(select Date_format(t1.FechaReporte,'%W %d %M %Y')) AS 'Fecha Del Reporte',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as'Supervisor',(SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal)as 'Credencial Conductor',concat(t3.Clave,'-',t3.Nombre)as Servicio, t1.HoraEntrada as 'Hora De Entrada', t1.KmEntrada as 'Kilometraje De Entrada', t1.TipoFallo as 'Tipo de Fallo',(select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)as 'Descripción Del Fallo',(select concat(x4.codfallo,'**',x4.falloesp) from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp)as 'Código De Fallo',t1.DescFalloNoCod as 'Descripción De Fallo No Códificado', t1.ObservacionesSupervision as 'Observaciones De Supervisión' ,coalesce((select x5.HoraInicioM from reportemantenimiento as x5 where x5.IdReporte=t1.idReporteSupervicion),'00:00:00') as 'Hora Inicio Mantenimiento',coalesce((select x6.HoraTerminoM from reportemantenimiento as x6 where x6.IdReporte=t1.idReporteSupervicion),'00:00:00')as 'Hora Termino Mantenimiento' ,(select x7.DiferenciaTiempoM  from reportemantenimiento as x7 where x7.IdReporte=t1.idReporteSupervicion)as 'Tiempo Mantenimiento', (select x8.Estatus from reportemantenimiento as x8 where x8.IdReporte=t1.idReporteSupervicion)as Estatus,(select x9.TrabajoRealizado from reportemantenimiento as x9 where x9.IdReporte=t1.idReporteSupervicion) as 'Trabajo Realizado',(select x10.ObservacionesM from reportemantenimiento as x10 where x10.IdReporte=t1.idReporteSupervicion)as 'Observaciones Mantenimiento',(select concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.IdReporte=t1.idReporteSupervicion)as 'Mecánico Que Realizo El Mantenimiento' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad INNER JOIN cservicios AS t3 on t1.Serviciofkcservicios=t3.idservicio order by t1.FechaReporte desc;", c.dbconection());

           

            DataSet ds = new DataSet();
         
            cargar.Fill(ds);
     
            dataGridView1.DataSource = ds.Tables[0];


        }


        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(cmbUnidad.Text) || string.IsNullOrWhiteSpace(lblSupervisor.Text) || string.IsNullOrWhiteSpace(txtConductor.Text) || string.IsNullOrWhiteSpace(cmbServicio.Text) || string.IsNullOrWhiteSpace(numericUpDownKilometraje.Text) || string.IsNullOrWhiteSpace(cmbTipoFallo.Text))
            {

                MessageBox.Show("Hay uno o más campos vacios, favor de llenar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cmbCodFallo.Text) && string.IsNullOrWhiteSpace(cmbDescFallo.Text) && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                {
                    MessageBox.Show("Campos vacios en sección de Fallos, favor de llenar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                else
                {
                    if (!string.IsNullOrWhiteSpace(cmbCodFallo.Text) && !string.IsNullOrWhiteSpace(cmbDescFallo.Text) && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                    {
                        validacionRegistro();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(cmbCodFallo.Text) && string.IsNullOrWhiteSpace(cmbDescFallo.Text) && !string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                        {
                            validacionRegistro();
                        }

                    }
                }
            }
        }




        public void validacionRegistro()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * from cpersonal where credencial='" + txtConductor.Text + "' and cargofkcargos=4 and status=1 or credencial='patio';", c.dbconection());
            MySqlDataReader lee = cmd.ExecuteReader();
            if (lee.Read())
            {
                try
                {
                    double km = double.Parse(numericUpDownKilometraje.Text);
                    if (km <= 0)
                    {
                        MessageBox.Show("El kilometraje debe ser mayor a 0", "Error");
                    }
                    else
                    {
                        //if(condicionando formato)

                        MySqlCommand insertar = new MySqlCommand("Insert into reportesupervicion (Folio,UnidadfkCUnidades,FechaReporte, SupervisorfkCPersonal, CredencialConductorfkCPersonal, Serviciofkcservicios,HoraEntrada,KmEntrada,TipoFallo,DescrFallofkcdescfallo,CodFallofkcfallosesp,DescFalloNoCod,ObservacionesSupervision) values ('" + lblFolio.Text+ "' , '" + cmbUnidad.SelectedValue + "' , '" + lblfecha.Text + "' , '" + lblid.Text + "' , '" + lblIdConductor.Text + "' , '" + cmbServicio.SelectedValue + "',(select curtime()) , '" + numericUpDownKilometraje.Text + "' , '" + cmbTipoFallo.Text + "' , '" + cmbDescFallo.SelectedValue + "','" + cmbCodFallo.SelectedValue + "','" + txtDescFalloNoC.Text + "' , '" + txtObserSupervicion.Text + "'  );", c.dbconection());
                        insertar.ExecuteNonQuery();

                        MessageBox.Show("Registro guardado exitosamente " + DateTime.Now.ToString(), "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cargarDAtos();
                        LimpiarReporte();
                    }
                }
                catch
                {
                    MessageBox.Show("Formato de kilometraje no valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("El numero de credencial de conductor no existe", "Verificar credencial", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void catalogoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        { }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblFechaReporte.Text = DateTime.Now.ToLongDateString();
            lblfecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

           



        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
        }

        private void txtSupervisor_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            btnGuardar.Size = new Size(69, 66);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            btnGuardar.Size = new Size(64, 61);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            btnBuscar.Size = new Size(68, 61);
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            btnBuscar.Size = new Size(73, 66);
        }





        private void txtSupervisor_TextChanged(object sender, EventArgs e)
        {

            MySqlCommand sql = new MySqlCommand("SELECT concat(t1.ApPaterno,' ',t1.ApMAterno,' ',t1.Nombres)as supervisor ,t1.idPersona,t2.puesto from cpersonal as t1  inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.password='" + v.Encriptar(txtSupervisor.Text) + "' and t2.puesto='Supervisor' and t1.status='1'", c.dbconection());
            MySqlDataReader cmd = sql.ExecuteReader();
            if (cmd.Read())
            {
                lblSupervisor.Text = Convert.ToString(cmd["supervisor"]);
                lblid.Text = Convert.ToString(cmd["idPersona"]);

            }
            else
            {
                lblid.Text = "";
                lblSupervisor.Text = "";
            }



        }

        private void comboBox2_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void txtObservacionesM_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtSupervisor_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void textBox2_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }



        private void textBox4_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void textBox3_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void txtKmEntrada_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar) || (e.KeyChar == 44) || (e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtDescFalloNoC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar) || (e.KeyChar == 47) || (e.KeyChar == 35) || (e.KeyChar == 44) || (e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

        }
        public void ValidarLetra(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar) || (e.KeyChar == 44) || (e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

        }
        private void txtObserSupervicion_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetra(e);
        }

        private void txtConductor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || char.IsLetter(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtBuscStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetra(e);
        }

        private void txtBuscSupervisor_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetra(e);

        }
        public void cargarbusquedas()
        {

            MySqlDataAdapter cargar = new MySqlDataAdapter("SET lc_time_names = 'es_ES'; select t1.Folio,t2.ECO,(select Date_format(FechaReporte,'%W %d %M %Y')) AS 'Fecha Del Reporte',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as'Supervisor',(SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal)as 'Credencial Conductor',concat(t3.Clave,'-',t3.Nombre)as Servicio, t1.HoraEntrada as 'Hora De Entrada', t1.KmEntrada as 'Kilometraje De Entrada', t1.TipoFallo as 'Tipo de Fallo',(select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)as 'Descripción Del Fallo',(select concat(x4.codfallo,'**',x4.falloesp) from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp)as 'Código De Fallo',t1.DescFalloNoCod as 'Descripción De Fallo No Códificado', t1.ObservacionesSupervision as 'Observaciones De Supervisión' ,coalesce((select x5.HoraInicioM from reportemantenimiento as x5 where x5.IdReporte=t1.idReporteSupervicion),'00:00:00') as 'Hora Inicio Mantenimiento',coalesce((select x6.HoraTerminoM from reportemantenimiento as x6 where x6.IdReporte=t1.idReporteSupervicion),'00:00:00')as 'Hora Termino Mantenimiento' ,(select x7.DiferenciaTiempoM  from reportemantenimiento as x7 where x7.IdReporte=t1.idReporteSupervicion)as 'Tiempo Mantenimiento', (select x8.Estatus from reportemantenimiento as x8 where x8.IdReporte=t1.idReporteSupervicion)as Estatus,(select x9.TrabajoRealizado from reportemantenimiento as x9 where x9.IdReporte=t1.idReporteSupervicion) as 'Trabajo Realizado',(select x10.ObservacionesM from reportemantenimiento as x10 where x10.IdReporte=t1.idReporteSupervicion)as 'Observaciones Mantenimiento',(select concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.IdReporte=t1.idReporteSupervicion)as 'Mecánico Que Realizo El Mantenimiento' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad INNER JOIN cservicios AS t3 on t1.Serviciofkcservicios=t3.idservicio WHERE(t1.UnidadfkCUnidades='"+cmbBuscarUnidad.SelectedValue+ "') OR ((select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='"+cmbBuscarDescripcion.Text+ "' and t1.DescrFallofkcdescfallo!='') OR ((select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + txtBuscSupervisor.Text+ "') OR ((select x8.Estatus from reportemantenimiento as x8 where x8.IdReporte=t1.idReporteSupervicion and x8.Estatus!='' )='" + cmbBuscStatus.Text+ "') OR (FechaReporte between '"+dtpFechaDe.Value.ToString("yyyy-MM-dd")+"' AND '"+dtpFechaA.Value.ToString("yyyy-MM-dd")+"') order by FechaReporte desc", c.dbconection());

            

            DataSet ds = new DataSet();
            
            cargar.Fill(ds);
           
            dataGridView1.DataSource = ds.Tables[0];

        }
        public void limpiarmant()
        {
            lblHIM.Text = "";
            lblHTM.Text = "";
            lblTM.Text = "";
            lblestatus.Text = "";
            lbltrabRealizado.Text = "";
            lblMecanico.Text = "";
            txtObservacionesM.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
                if (dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now)
                {
                    MessageBox.Show("Error en las fechas seleccionadas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    cargarbusquedas();

                    limpiarbusqueda();
                }
            
          
        }
        public void limpiarbusqueda()
        {
            cmbBuscarDescripcion.SelectedIndex = -1;
            txtBuscSupervisor.Clear();
            cmbBuscStatus.SelectedIndex = -1;
            cmbBuscarUnidad.SelectedIndex = -1;
            dtpFechaDe.ResetText();
            dtpFechaA.ResetText();
        }
        private void txtSupervisor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }


            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 64 || e.KeyChar == 45 || e.KeyChar == 46 || e.KeyChar == 95 || e.KeyChar == 42 || e.KeyChar == 47)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        public void LimpiarReporte()
        {


            cmbUnidad.SelectedIndex = -1;
            txtSupervisor.Clear();
            lblSupervisor.Text = "";
            lblid.Text = "";
            txtConductor.Clear();
            lblCredCond.Text = "";
            lblIdConductor.Text = "";
            cmbServicio.SelectedIndex = -1;
            numericUpDownKilometraje.ResetText();
            cmbTipoFallo.SelectedIndex = -1;
            cmbDescFallo.SelectedIndex = -1;
            cmbCodFallo.SelectedIndex = -1;
            txtDescFalloNoC.Clear();
            txtObserSupervicion.Clear();
            timer1.Start();
        }

        private void btnBuscar_MouseMove(object sender, MouseEventArgs e)
        {
            btnBuscar.Size = new Size(73, 66);
        }

        private void btnBuscar_MouseLeave(object sender, EventArgs e)
        {
            btnBuscar.Size = new Size(68, 61);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MySqlCommand cm = new MySqlCommand("select * from cpersonal ;", c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add();
            }

        }










        private void txtConductor_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbServicio_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void txtKmEntrada_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void cmbTipoFallo_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void cmbDescFallo_Click(object sender, EventArgs e)
        {
            timer1.Stop();

        }

        private void cmbCodFallo_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void cmbDescFallo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbDescFallo.SelectedIndex != -1)
            {
                txtDescFalloNoC.Enabled = false;
                txtDescFalloNoC.Clear();
            }
            else
            {
                txtDescFalloNoC.Enabled = true;
                txtDescFalloNoC.Clear();
            }


            MySqlCommand cmd = new MySqlCommand("select concat(codfallo,'**',falloesp) as Fallo, idfalloEsp,descfallofkcdescfallo from cfallosesp as t1 inner join cdescfallo as t2 on t2.iddescfallo=t1.descfallofkcdescfallo where descfallo='" + cmbDescFallo.Text + "';", c.dbconection());
            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd);
            DataTable dan2 = new DataTable();
            da2.Fill(dan2);
            cmbCodFallo.ValueMember = "idfalloEsp";
            cmbCodFallo.DisplayMember = "Fallo";
            cmbCodFallo.DataSource = dan2;
            cmbCodFallo.SelectedIndex = -1;

        }

        private void txtBuscSupervisor_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtConductor_TextChanged_1(object sender, EventArgs e)
        {
            MySqlCommand sql1 = new MySqlCommand("select concat(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres)as Conductor,t2.puesto,t1.idPersona from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + txtConductor.Text+"' and t2.puesto='Conductor'" , c.dbconection());
            MySqlDataReader cmd1 = sql1.ExecuteReader();
            if (cmd1.Read())
            {
                lblCredCond.Text = (Convert.ToString(cmd1["Conductor"]));
                lblIdConductor.Text = Convert.ToString(cmd1["idPersona"]);

            }
            else
            {
                lblIdConductor.Text = "";
                lblCredCond.Text = "";


            }
        }

        private void cmbDescFallo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbCodFallo_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        private void To_pdf()
        {
            Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf| All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            if (filename.Trim() != "")
            {
                FileStream file = new FileStream(filename,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
                PdfWriter.GetInstance(doc, file);
                doc.Open();
                string fecha = "Fecha: " + DateTime.Now.ToLongDateString();
                Chunk chunk = new Chunk("Reporte Supervisión",FontFactory.GetFont("ARIAL",18, iTextSharp.text.Font.BOLD) );
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@"C:\Users\Desarrollador 6\Documents\Visual Studio 2015\Projects\Supervicion\Supervicion\Resources\transmasivo.png");
                imagen.ScalePercent(18f);
                imagen.SetAbsolutePosition(625f, 505f);
                imagen.Alignment = Element.ALIGN_RIGHT;
                doc.Add(new Paragraph(chunk));
                doc.Add(new Paragraph(fecha));
                doc.Add(imagen);
                doc.Add(new Paragraph("                                   "));
                doc.Add(new Paragraph("                                   "));
                doc.Add(new Paragraph("                                   "));
                GenerarDocumento(doc);
                
                doc.Close();
             
            }
            
        }
        public void GenerarDocumento(Document document)
        {
            int i, j;
            PdfPTable datatable = new PdfPTable(dataGridView1.ColumnCount);
            datatable.DefaultCell.Padding = 3;
            float[] headerwidths = GetTamañoColumnas(dataGridView1);
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 100;
            datatable.DefaultCell.BorderWidth = 2;
            datatable.DefaultCell.VerticalAlignment= Element.ALIGN_CENTER;
            for (i = 0; i < dataGridView1.ColumnCount; i++)
            {
                datatable.AddCell(dataGridView1.Columns[i].HeaderText);
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BorderWidth = 1;
            for(i=0; i< dataGridView1.Rows.Count; i++)
            {
                for(j=0; j< dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1[j,i].Value != null)
                    {
                        datatable.AddCell(new Phrase(dataGridView1[j, i].Value.ToString(), FontFactory.GetFont("ARIAL",9)));
                    }
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }
        public float[] GetTamañoColumnas(DataGridView dg)
        {
            float[] values = new float[dg.ColumnCount];
            for(int i=0; i < 11; i++)
            {
                values[i] = (float)dg.Columns[i].Width;
            }
            return values;
        }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            To_pdf();
        }
    

        private void txtKmEntrada_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void cmbDescFallo_MouseMove(object sender, MouseEventArgs e)
        {
            txtDescFalloNoC.Enabled = false;
            txtDescFalloNoC.Clear();
        }

        private void cmbDescFallo_MouseLeave(object sender, EventArgs e)
        {
            txtDescFalloNoC.Enabled = true;
            txtDescFalloNoC.Clear();
        }

        private void cmbDescFallo_MouseUp(object sender, MouseEventArgs e)
        {
            txtDescFalloNoC.Enabled = true;
            txtDescFalloNoC.Clear();
        }

        private void txtKmEntrada_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtKmEntrada_KeyUp_1(object sender, KeyEventArgs e)
        {

        }

        private void txtBuscSupervisor_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBuscSupervisor_KeyUp_1(object sender, KeyEventArgs e)
        {

        }

        private void cmbTipoFallo_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbTipoFallo_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void cmbTipoFallo_DrawItem(object sender, DrawItemEventArgs e)
        {
            //switch (e.Index)
            //{
            //    case 0:
            //        e.Graphics.FillRectangle(Brushes.Green, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            //        e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush (e.ForeColor), e.Bounds);
            //        break;
            //    case 1:
            //        e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            //        e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            //        break;
            //    case 2:
            //        e.Graphics.FillRectangle(Brushes.Red, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            //        e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            //        break;
            //}
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LimpiarReporte();
            limpiarmant();
            timer1.Stop();
            lblFolio.Text= dataGridView1.CurrentRow.Cells["Folio"].Value.ToString();
            cmbUnidad.Text = dataGridView1.CurrentRow.Cells["ECO"].Value.ToString();
            //lblSupervisor.Text = dataGridView1.CurrentRow.Cells["Supervisor"].Value.ToString();
            lblFechaReporte.Text = dataGridView1.CurrentRow.Cells["Fecha Del Reporte"].Value.ToString();
            txtConductor.Text = dataGridView1.CurrentRow.Cells["Credencial Conductor"].Value.ToString();
            cmbServicio.Text = dataGridView1.CurrentRow.Cells["Servicio"].Value.ToString();
            numericUpDownKilometraje.Text = dataGridView1.CurrentRow.Cells["Kilometraje De Entrada"].Value.ToString();
            cmbTipoFallo.Text = dataGridView1.CurrentRow.Cells["Tipo De Fallo"].Value.ToString();
            cmbDescFallo.Text = dataGridView1.CurrentRow.Cells["Descripción Del Fallo"].Value.ToString();
            cmbCodFallo.Text = dataGridView1.CurrentRow.Cells["Código De Fallo"].Value.ToString();
            txtDescFalloNoC.Text = dataGridView1.CurrentRow.Cells["Descripción De Fallo No Códificado"].Value.ToString();
            txtObserSupervicion.Text = dataGridView1.CurrentRow.Cells["Observaciones De Supervisión"].Value.ToString();

            lblHIM.Text = dataGridView1.CurrentRow.Cells["Hora Inicio Mantenimiento"].Value.ToString();
            lblHTM.Text = dataGridView1.CurrentRow.Cells["Hora Termino Mantenimiento"].Value.ToString();
            lblTM.Text = dataGridView1.CurrentRow.Cells["Tiempo Mantenimiento"].Value.ToString();
            lblestatus.Text = dataGridView1.CurrentRow.Cells["Estatus"].Value.ToString();
            lbltrabRealizado.Text = dataGridView1.CurrentRow.Cells["Trabajo Realizado"].Value.ToString();
            lblMecanico.Text = dataGridView1.CurrentRow.Cells["Mecánico Que Realizo El Mantenimiento"].Value.ToString();
            txtObservacionesM.Text = dataGridView1.CurrentRow.Cells["Observaciones Mantenimiento"].Value.ToString();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
         if(this.dataGridView1.Columns[e.ColumnIndex].Name== "Tipo de Fallo")
            {
                if (Convert.ToString(e.Value)=="Preventivo")
                {
                  
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "Correctivo")
                    {
                    
                        e.CellStyle.BackColor = Color.Khaki;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "Reiterativo")
                        {
                          
                            e.CellStyle.BackColor = Color.LightCoral;

                        }
                    }
                }
            }
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if(Convert.ToString(e.Value)=="En Proceso")
                {
                 
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "Liberada")
                    {
                       
                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "Reprogramada")
                        {
                          
                            e.CellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarDAtos();
            LimpiarReporte();
            limpiarmant();
            timer1.Start();
        }

     
       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        public void exporta_a_excel()
        {

            if (dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                int ColumnIndex = 0;
                
                Color c;
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    ColumnIndex++;
                    X.Cells[1, ColumnIndex] = col.HeaderText;
                }

                for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                {
                    for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                    {
                        //foreach (DataGridViewColumn colk in dataGridView3.Columns)
                        //{
                        if (dataGridView1.Columns[j].Visible == true)
                        {
                            try
                            {
                                h.Worksheet sheet = X.ActiveSheet;
                                h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                                sheet.Cells[i + 2, j + 1] = dataGridView1[j, i].Value.ToString();
                                c = dataGridView1[j, i].Style.BackColor;
                                //if (!c.IsEmpty)
                                //{
                                //    MessageBox.Show("Esta pintado");
                                //}

                                if (dataGridView1.Rows[i].DefaultCellStyle.BackColor ==( Color.Khaki))
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                                }
                                if (dataGridView1[j, i].Style.BackColor == Color.PaleGreen)
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridView1[j, i].Style.BackColor == Color.LightCoral)
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                                }

                                if (dataGridView1[j, i].Value.ToString() == "En Proceso")
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Fuchsia);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                //if (dataGridView1[j, i].Value.ToString() == "D".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.HotPink);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "IN".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.YellowGreen);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "IM".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.DarkOrange);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "A".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Navy);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "FJ".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.DarkRed);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "B".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Maroon);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                                //}
                                //if (dataGridView1[j, i].Value.ToString() == "I".ToString())
                                //{
                                //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                                //    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                //}
                            }
                            catch (System.NullReferenceException)
                            {

                            }
                        }
                        else
                        {


                        }
                        //}
                    }
                }
                X.Columns.AutoFit();
                X.Visible = true;
            }
        }
        private void btnExcel_Click_1(object sender, EventArgs e)
        {
            //TransInsumos TRI = new TransInsumos();
            //TRI.ShowDialog();
            //this.Close();
            exporta_a_excel();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            LimpiarReporte();
        }

        private void btnCancelar_MouseLeave(object sender, EventArgs e)
        {
            btnCancelar.Size = new Size(64, 61);
        }

        private void btnCancelar_MouseMove(object sender, MouseEventArgs e)
        {
            btnCancelar.Size = new Size(69, 66);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarReporte();
        }

        private void btnExcel_MouseLeave(object sender, EventArgs e)
        {
            btnExcel.Size = new Size(66, 58);
        }

        private void btnExcel_MouseMove(object sender, MouseEventArgs e)
        {
            btnExcel.Size = new Size(71, 63);
        }

        private void btnPdf_MouseLeave(object sender, EventArgs e)
        {
            btnPdf.Size = new Size(66, 55);
        }

        private void btnPdf_MouseMove(object sender, MouseEventArgs e)
        {
            btnPdf.Size = new Size(71, 60);
        }

        private void btnActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            btnActualizar.Size= new Size (69, 66);
        }

        private void btnActualizar_MouseLeave(object sender, EventArgs e)
        {
            btnActualizar.Size = new Size(64, 61);
        }
    }
}


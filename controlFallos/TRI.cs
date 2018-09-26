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
using Microsoft.Office.Interop.Excel;

namespace controlFallos
{
    public partial class TransInsumos : Form
    {

        public TransInsumos()
        {
            InitializeComponent();
        }
        public TransInsumos(string idFolio)
        {
            InitializeComponent();
            String sql = "SELECT t2.idReporteSupervicion,t2.Folio,t3.ECO,t1.FechaReporteM AS fecha,t2.TipoFallo as Tipo,t1.Estatus, concat(t4.ApPaterno,' ',t4.ApMaterno, ' ',t4.nombres) as Mecanico FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion = t2.idReporteSupervicion  INNER JOIN cunidades as t3 ON t3.idunidad=t2.UnidadfkCUnidades INNER join cpersonal as t4 ON t4.idPersona = t1.MecanicofkPersonal  WHERE t1.StatusRefacciones = 'Se Requieren Refacciones' and t2.Folio = '" + idFolio+"'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lblId.Text = idFolio.ToString();
                lblFolio.Text = dr.GetString("Folio");
                lblUnidad.Text = dr.GetString("ECO");
                lblFechaSolicitud.Text = dr.GetString("fecha");
                lblMecanicoSolicita.Text = dr.GetString("Mecanico");
            }
          
        }
        conexion c = new conexion();
        validaciones v = new validaciones();
        public void cargardatos()
        {

            MySqlDataAdapter cargar = new MySqlDataAdapter("SET lc_time_names = 'es_ES';SELECT t1.Folio AS 'Folio De Reporte',t2.ECO as 'UNIDAD', t3.FechaReporteM AS 'Fecha De Solicitud',(SELECT CONCAT(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres) FROM cpersonal as x1 where x1.idPersona=t3.MecanicofkPersonal)AS 'Mecánico Que Solicita', t3.StatusRefacciones AS 'Estatus De Refacciones' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades=t2.idunidad INNER JOIN reportemantenimiento AS t3 ON t3.FoliofkSupervicion=t1.idreportesupervicion WHERE t3.StatusRefacciones='Se Requieren Refacciones';", c.dbconection());

            DataSet ds = new DataSet();
            cargar.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            MySqlDataAdapter cargarR = new MySqlDataAdapter("select t1.codrefaccion AS 'Código De Refaccion',t1.marcafkcmarcas as Marca,t1.refaccion AS Refacción,t1.existencias as 'Existencias' from crefacciones AS t1", c.dbconection());

            DataSet ds1 = new DataSet();

            cargarR.Fill(ds1);

            dataGridView2.DataSource = ds1.Tables[0];


        }

        private void TransInsumos_Load(object sender, EventArgs e)
        {
            cargardatos();
            timer1.Start();
            //dataGridView1.Columns[0].Visible = false;
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            lblFecha2.Text = DateTime.Now.ToLongDateString();
            lblfechainsertar.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dtpFechaA.Value.Date < dtpFechaDe.Value.Date)
            {
                MessageBox.Show("Error en las fechas seleccionadas, verifique las fechas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            btnGuardar.Size = new Size(64, 61);
        }

        private void btnGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            btnGuardar.Size = new Size(69, 66);
        }

        private void btnBuscar_MouseLeave(object sender, EventArgs e)
        {
            btnBuscar.Size = new Size(68, 61);
        }

        private void btnBuscar_MouseMove(object sender, MouseEventArgs e)
        {
            btnBuscar.Size = new Size(73, 66);
        }
        
        private void btnPdf_MouseLeave(object sender, EventArgs e)
        {
            btnPdf.Size = new Size(66, 55);
        }

        private void btnPdf_MouseMove(object sender, MouseEventArgs e)
        {
            btnPdf.Size = new Size(71, 60);
        }

        private void btnExcel_MouseLeave(object sender, EventArgs e)
        {
            btnExcel.Size = new Size(66, 58);
        }

        private void btnExcel_MouseMove(object sender, MouseEventArgs e)
        {
            btnExcel.Size = new Size(71, 63);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblFolio.Text) || string.IsNullOrWhiteSpace(lblFolio.Text) || string.IsNullOrWhiteSpace(lblFechaSolicitud.Text) || string.IsNullOrWhiteSpace(lblMecanicoSolicita.Text)  || string.IsNullOrWhiteSpace(txtFolioFactura.Text)  || string.IsNullOrWhiteSpace(lblFecha2.Text) || string.IsNullOrWhiteSpace(lblPersonaDis.Text))
            {
                MessageBox.Show("Hay uno o más campos vacios, favor de llenar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MySqlCommand insertar = new MySqlCommand("insert into reportetri (idreportemfkreportemantenimiento,FolioFactura,ExistenciaRefac,FechaEntrega,PersonaEntregafkcPersonal,ObservacionesTrans) values ('" + lblId.Text + "','" + txtFolioFactura.Text +  "','" + lblfechainsertar.Text + "','" + lblIdDispenso.Text + "','" + txtObservacionesT.Text + "')", c.dbconection());
                insertar.ExecuteNonQuery();
                MessageBox.Show("Registro guardado con exito!!!! " + DateTime.Now.ToString(), "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarregistro();
                cargardatos();
            }
        }
        public void limpiarregistro()
        {
            lblFolio.Text = "";
            lblIdDispenso.Text = "";
            lblId.Text = "";
            lblUnidad.Text = "";
            lblFechaSolicitud.Text = "";
            lblMecanicoSolicita.Text = "";
         
            txtFolioFactura.Clear();
 
            lblfechainsertar.Text = "";
            txtDispenso.Clear();
            txtObservacionesT.Clear();

        }



        private void txtFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);
        }

        private void txtObservacionesT_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetras(e);
        }


        public void ValidarNumero(KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        public void ValidarLetras(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }



        private void txtBuscFolio_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);
        }

        private void txtBuscPersonDis_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetras(e);
        }

        private void txtBuscMecSol_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarLetras(e);
        }

        private void txtDispenso_TextChanged_1(object sender, EventArgs e)
        {
            MySqlCommand sql = new MySqlCommand("SELECT concat(ApPaterno,' ',ApMAterno,' ',Nombres)as almacenista ,idPersona from cpersonal where password='" + v.Encriptar(txtDispenso.Text) + "';", c.dbconection());
            MySqlDataReader cmd = sql.ExecuteReader();
            if (cmd.Read())
            {
                lblPersonaDis.Text = Convert.ToString(cmd["almacenista"]);
                lblIdDispenso.Text = Convert.ToString(cmd["idPersona"]);

            }
            else
            {
                lblIdDispenso.Text = "";
                lblPersonaDis.Text = "";
            }
        }

        private void txtDispenso_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //lblId.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
            lblFolio.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
            lblUnidad.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
            lblFechaSolicitud.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
            lblMecanicoSolicita.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value);
            //txtFolioFactura.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[6].Value);
            //cmbExistencia.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[7].Value);
            //lblPersonaDis.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[9].Value);
            //txtObservacionesT.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[10].Value);


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtObservacionesT_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void txtFolioDe_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);
        }

        private void txtFolioA_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);
        }
        public void exportaraexcel(DataGridView tabla)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            int ColumnIndex = 0;
            foreach (DataGridViewColumn col in tabla.Columns)
            {
                ColumnIndex++;
                excel.Cells[1, ColumnIndex] = col.Name;
            }
            int rowIndex = 0;
            foreach (DataGridViewRow row in tabla.Rows)
            {
                rowIndex++;
                ColumnIndex = 0;
                foreach (DataGridViewColumn col in tabla.Columns)
                {
                    ColumnIndex++;
                    excel.Cells[rowIndex + 1, ColumnIndex] = row.Cells[col.Name].Value;
                        
                }
            }
            excel.Visible = true;
            Worksheet worksheet = (Worksheet)excel.ActiveSheet;
            worksheet.Activate();
        }
        
       

      
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //Form1 abrir = new Form1();
            //abrir.ShowDialog();
            //this.Close();
            exportaraexcel(dataGridView1);
            
        }

        private void btnActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            btnActualizar.Size = new Size(69, 66);
        }

        private void btnPdf_MouseMove_1(object sender, MouseEventArgs e)
        {
            btnPdf.Size = new Size(71, 60);
        }

        private void btnPdf_MouseLeave_1(object sender, EventArgs e)
        {
            btnPdf.Size = new Size(65, 55);
        }

        private void btnExcel_MouseLeave_1(object sender, EventArgs e)
        {
            btnExcel.Size = new Size(66, 58);
        }

        private void btnExcel_MouseMove_1(object sender, MouseEventArgs e)
        {
            btnExcel.Size = new Size(71, 63);
        }

        private void btnActualizar_MouseLeave(object sender, EventArgs e)
        {
            btnActualizar.Size = new Size(64, 61);
        }
    } 
}
    


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using h = Microsoft.Office.Interop.Excel;

namespace controlFallos
{
    public partial class FormFallasMantenimiento : Form
    {
        validaciones val = new validaciones();

        conexion co = new conexion();
        validaciones v = new validaciones();
        String folio, foliof, conta;
        String fam, reff, namr = "";
        String cdf = "", mec = "", mecapo = "", reqref = "", exiref = "", trabreak = "", folfac = "", estmant = "", supmant = "", obsmant = "", idfg = "", fgl = "", fam1 = "", reff1 = "";
        bool bedit = false;
        int cont, ctotal, nrefacc, nnrefacc, contrefin, contreini, c, inicolumn, fincolumn, crefverificadas;
        int crefacc, month, sch, cargo, fquestion, tbref, exist, tot, x = 0, dontr, falt, totfalt, cexis, cvalidacion;
        double cant;
        String cantd, facturar, exis, simb;
        string name;
        string sname;
        string ext; public Thread hilo;
        static bool res = true;
        int idUsuario, empresa, area;
        bool pinsertar { get; set; } 
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        void quitarseen()
        {
            while (res)
            {
                string[] arreglo = co.conex().Split(';');
                string server = v.Desencriptar(arreglo[0]);
                string user = v.Desencriptar(arreglo[1]);
                string password = v.Desencriptar(arreglo[2]);
                string database = v.Desencriptar(arreglo[3]);
                MySqlConnection dbcon1 = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                if (dbcon1.State != System.Data.ConnectionState.Open)
                {

                    dbcon1.Open();
                }

                MySqlCommand cmd = new MySqlCommand("UPDATE reportesupervicion SET seen = 1 WHERE seen  = 0", dbcon1);
                cmd.ExecuteNonQuery();
                dbcon1.Close();
                Thread.Sleep(180000);
            }
        }
        public FormFallasMantenimiento(int idUsuario,int empresa, int area)
        {
            InitializeComponent();
            comboBoxFalloGral.MouseWheel += new MouseEventHandler(comboBoxFalloGral_MouseWheel);
            comboBoxReqRefacc.MouseWheel += new MouseEventHandler(comboBoxReqRefacc_MouseWheel);
            comboBoxExisRefacc.MouseWheel += new MouseEventHandler(comboBoxExisRefacc_MouseWheel);
            comboBoxEstatusMant.MouseWheel += new MouseEventHandler(comboBoxEstatusMant_MouseWheel);
            comboBoxFamilia.MouseWheel += new MouseEventHandler(comboBoxFamilia_MouseWheel);
            comboBoxFRefaccion.MouseWheel += new MouseEventHandler(comboBoxFRefaccion_MouseWheel);
            comboBoxUnidadB.MouseWheel += new MouseEventHandler(comboBoxUnidadB_MouseWheel);
            comboBoxMecanicoB.MouseWheel += new MouseEventHandler(comboBoxMecanicoB_MouseWheel);
            comboBoxEstatusMB.MouseWheel += new MouseEventHandler(comboBoxEstatusMB_MouseWheel);
            comboBoxDescpFalloB.MouseWheel += new MouseEventHandler(comboBoxDescpFalloB_MouseWheel);
            comboBoxMesB.MouseWheel += new MouseEventHandler(comboBoxMesB_MouseWheel);
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }

        private void FormFallasMantenimiento_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "HH:mm:ss";
            dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker5.CustomFormat = "HH:mm:ss";
            timer1.Start();
            timer2.Start();
            metodoCarga();
           co.dbconection().Close();
            conteo();
           co.dbconection().Close();

            privilegios();
            Unidad();
            DescripFallo();
            Mecanico();
            FamiliaRef();
            ClasFallo();

            comboBoxEstatusMant.SelectedIndex = 0;
            comboBoxReqRefacc.SelectedIndex = 0;
            comboBoxExisRefacc.SelectedIndex = 0;
            comboBoxMecanicoB.SelectedIndex = 0;
            comboBoxEstatusMB.SelectedIndex = 0;
            comboBoxMesB.SelectedIndex = 0;

            AutoCompletado(textBoxFolioB);

            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;

            if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
            {
                buttonExcel.Visible = true;
                label35.Visible = true;
            }
            else
            {
                buttonExcel.Visible = false;
                label35.Visible = false;
            }

            if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
            {
                label60.Visible = true;
                label61.Visible = true;
            }
            else
            {
                label60.Visible = false;
                label61.Visible = false;
            }

            if (checkBoxFechas.Checked == false)
            {
                checkBoxFechas.ForeColor = checkBoxFechas.Checked ? Color.Crimson : Color.Crimson;
            }
            //string lol = "ZTPYQFZE8080xC2yZmo()WQ==";
            //MessageBox.Show(val.Desencriptar(lol));
            //string lol = "toto12345678";
            //textBox1.Text = val.Encriptar(lol);
            //MessageBox.Show(val.Encriptar(lol));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            dateTimePicker2.Text = DateTime.Now.ToLongTimeString();
        }

        /* Métodos Para Cargar Los ComboBox *//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Unidad()
        {
            DataTable dt = new DataTable();
            DataRow row2 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(t2.identificador, LPAD(consecutivo, 4,'0')) AS ECO, idunidad FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas= t2.idarea",co.dbconection());
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt);
            row2["idunidad"] = 0;
            row2["ECO"] = "-- UNIDAD --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxUnidadB.DataSource = dt;
            comboBoxUnidadB.ValueMember = "idunidad";
            comboBoxUnidadB.DisplayMember = "ECO";
            comboBoxUnidadB.SelectedIndex = 0;
        }

        private void ClasFallo()
        {
            DataTable dt = new DataTable();
            DataRow row2 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(nombreFalloGral) AS nombreFalloGral, idFalloGral FROM cfallosgrales WHERE status = 1 ORDER BY nombreFalloGral",co.dbconection());
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt);
            row2["idFalloGral"] = 0;
            row2["nombreFalloGral"] = "-- FALLO GENERAL --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxFalloGral.DataSource = dt;
            comboBoxFalloGral.ValueMember = "idFalloGral";
            comboBoxFalloGral.DisplayMember = "nombreFalloGral";
            comboBoxFalloGral.SelectedIndex = 0;
        }

        private void Mecanico()
        {
            DataTable dt = new DataTable();
            DataRow row2 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre, t1.idPersona FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t3.puesto LIKE  'Mecánico%' ORDER BY Nombre",co.dbconection());
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt);
            row2["idPersona"] = 0;
            row2["Nombre"] = "-- MECÁNICO --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxMecanicoB.DataSource = dt;
            comboBoxMecanicoB.ValueMember = "idPersona";
            comboBoxMecanicoB.DisplayMember = "Nombre";
            comboBoxMecanicoB.SelectedIndex = 0;
        }

        private void DescripFallo()
        {
            DataTable dt = new DataTable();
            DataRow row2 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(descfallo) AS descfallo, iddescfallo FROM cdescfallo ORDER BY descfallo",co.dbconection());
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt);
            row2["iddescfallo"] = 0;
            row2["descfallo"] = "-- DESCRIPCIÓN DEL FALLO --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxDescpFalloB.DataSource = dt;
            comboBoxDescpFalloB.ValueMember = "iddescfallo";
            comboBoxDescpFalloB.DisplayMember = "descfallo";
            comboBoxDescpFalloB.SelectedIndex = 0;
        }

        private void FamiliaRef()
        {
            DataTable dt = new DataTable();
            DataRow row2 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(familia) AS familia, idfamilia FROM cfamilias WHERE status = '1' ORDER BY familia",co.dbconection());
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt);
            row2["idfamilia"] = 0;
            row2["familia"] = "-- FAMILIA --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxFamilia.DataSource = dt;
            comboBoxFamilia.DisplayMember = "familia";
            comboBoxFamilia.ValueMember = "idfamilia";
            comboBoxFamilia.SelectedIndex = 0;
        }

        private void RefaccPed()
        {
            DataTable dt = new DataTable();
            DataRow row3 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(t2.nombreRefaccion) AS 'nombreRefaccion', t2.idrefaccion FROM cfamilias AS t1 INNER JOIN crefacciones AS t2 ON t1.idfamilia = t2.familiafkcfamilias WHERE t2.familiafkcfamilias = '" + comboBoxFamilia.SelectedValue + "' GROUP BY t2.nombreRefaccion ORDER BY t2.nombreRefaccion",co.dbconection());
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            row3["idrefaccion"] = 0;
            row3["nombreRefaccion"] = "-- REFACCIÓN --";
            dt.Rows.InsertAt(row3, 0);
            comboBoxFRefaccion.DataSource = dt;
            comboBoxFRefaccion.ValueMember = "idrefaccion";
            comboBoxFRefaccion.DisplayMember = "nombreRefaccion";
            comboBoxFRefaccion.SelectedIndex = 0;

           co.dbconection().Close();
        }

        /* Todos los métodos *//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor, Form f)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(f.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public void botonactualizar()
        {
            inicolumn = 0;
            fincolumn = 0;
            dontr = 0;
            metodocargaref();
            metodoCarga();
            conteo();

            dataGridViewMantenimiento.Refresh();
            limpiarcamposbus();
            limpiarcampos();
            limpiarstring();
            timer1.Start();
            buttonFinalizar.Visible = false;
            label37.Visible = false;
            buttonPDF.Visible = false;
            label36.Visible = false;
            radioButtonGeneral.Visible = false;
            radioButtonUnidad.Visible = false;
            if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
            {
                buttonExcel.Visible = true;
                label35.Visible = true;
            }
            else
            {
                buttonExcel.Visible = false;
                label35.Visible = false;
            }
            buttonAgregar.Visible = false;
            label39.Visible = false;
            groupBoxBusqueda.Enabled = true;
            groupBoxRefacciones.Visible = false;
            label1.Visible = false;
            groupBoxMantenimiento.Visible = true;

            label1.Visible = false;
            groupBoxRefacciones.Visible = false;
            groupBoxMantenimiento.Visible = true;
            timer2.Start();
            comboBoxFalloGral.Enabled = false;
            textBoxMecanico.Enabled = false;
            textBoxMecanicoApo.Enabled = false;
            comboBoxReqRefacc.Enabled = false;
            comboBoxExisRefacc.Enabled = false;
            textBoxFolioFactura.Enabled = false;
            textBoxTrabajoRealizado.Enabled = false;
            comboBoxEstatusMant.Enabled = false;
            textBoxSuperviso.Enabled = false;
            textBoxObsMan.Enabled = false;
            buttonAgregar.Visible = false;
            label39.Visible = false;
            buttonEditar.Visible = false;
            label58.Visible = false;
            buttonGuardar.Visible = false;
            label24.Visible = false;
          
        }

        public void textBox_TextChanged(object sender, EventArgs e)
        {
            if (bedit == true)
            {
                if (comboBoxFalloGral.SelectedIndex == 0)
                {
                    fgl = "0";
                }
                else
                {
                    fgl = Convert.ToString(comboBoxFalloGral.SelectedValue);
                }
                if ((((idfg == fgl) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabreak == textBoxTrabajoRealizado.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))) && ((folfac == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))) && ((obsmant == textBoxObsMan.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObsMan.Text)))))
                {
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label58.Visible = true;
                }
            }
        }

        private void comboBoxFalloGral_SelectedValueChanged(object sender, EventArgs e)
        {
            //if(comboBoxFalloGral.SelectedIndex == 0)
            //{
            //    comboBoxReqRefacc.Enabled = false;
            //}
            //else
            //{
            //    comboBoxReqRefacc.Enabled = true;
            //}

            if (bedit == true)
            {
                string fgl = "";
                if (comboBoxFalloGral.SelectedIndex == 0)
                {
                    fgl = "0";
                }
                else
                {
                    fgl = Convert.ToString(comboBoxFalloGral.SelectedValue);
                }
                if ((((idfg == fgl) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabreak == textBoxTrabajoRealizado.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))) && ((folfac == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))) && ((obsmant == textBoxObsMan.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObsMan.Text)))))
                {
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label58.Visible = true;
                }
            }
        }

        public void combos_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background 
                e.DrawBackground();

                // Drawing one of the items? 
                if (e.Index >= -1)
                {
                    // Set the string alignment. Choices are Center, Near and Far 
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings 
                    // Assumes Brush is solid 
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush 
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        // Draw the string 
                        DataTable f = (DataTable)cbx.DataSource;
                        if (e.Index == -1)
                        {
                            e.Graphics.DrawString("", cbx.Font, brush, e.Bounds, sf);
                        }
                        else
                        {
                            e.Graphics.DrawString(f.Rows[e.Index].ItemArray[1].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        }
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        // Draw the string 
                        DataTable f = (DataTable)cbx.DataSource;
                        if (e.Index == -1)
                        {
                            e.Graphics.DrawString("", cbx.Font, brush, e.Bounds, sf);

                        }
                        else
                        {
                            e.Graphics.DrawString(f.Rows[e.Index].ItemArray[1].ToString(), cbx.Font, brush, e.Bounds, sf);
                        }
                        e.DrawFocusRectangle();
                    }
                }
            }
        }

        public void validarstatusfallagral(string cdf)
        {
            if (string.IsNullOrWhiteSpace(cdf))
            {
                MySqlCommand cmd00 = new MySqlCommand("SELECT UPPER(t1.nombreFalloGral) AS nombreFalloGral, t1.idFalloGral FROM cfallosgrales AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idFalloGral = t2.FalloGralfkFallosGenerales WHERE t1.nombreFalloGral = '" + dataGridViewMantenimiento.CurrentRow.Cells[15].Value.ToString() + "' AND t1.status = '0'",co.dbconection());
                MySqlDataReader dr00 = cmd00.ExecuteReader();
                if (dr00.Read())
                {
                    MySqlCommand cmd01 = new MySqlCommand("SELECT UPPER(t1.nombreFalloGral) AS nombreFalloGral, t1.idFalloGral FROM cfallosgrales AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idFalloGral = t2.FalloGralfkFallosGenerales WHERE t1.nombreFalloGral = '" + dataGridViewMantenimiento.CurrentRow.Cells[15].Value.ToString() + "' AND t1.status = '1'",co.dbconection());
                    MySqlDataAdapter da01 = new MySqlDataAdapter(cmd01);
                    DataTable dt = new DataTable();
                    da01.Fill(dt);
                    DataRow row = dt.NewRow();
                    row["idFalloGral"] = dr00["idFalloGral"];
                    row["nombreFalloGral"] = dr00["nombreFalloGral"];
                    dt.Rows.InsertAt(row, 1);
                    comboBoxFalloGral.ValueMember = "idFalloGral";
                    comboBoxFalloGral.DisplayMember = "nombreFalloGral";
                    comboBoxFalloGral.DataSource = dt;
                    comboBoxFalloGral.Text = dr00["nombreFalloGral"].ToString();
                    comboBoxFalloGral.Text = dataGridViewMantenimiento.CurrentRow.Cells[15].Value.ToString();
                   co.dbconection().Close();
                }
                dr00.Close();
               co.dbconection().Close();
            }
        }

        public void exportacionexcel()
        {
            int contador = 0;
            string Folio, id;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Reporte de Mantenimiento','0','";
            foreach (DataGridViewRow row in dataGridViewMantenimiento.Rows)
            {
                contador++;
                id = row.Cells[0].Value.ToString();
                Folio = getaData("SELECT t1.idreportesupervicion FROM reportesupervicion AS t1 WHERE '" + id + "' = t1.folio").ToString();
                if (contador < dataGridViewMantenimiento.RowCount)
                {
                    Folio += ";";
                }
                sql += Folio;
            }
            sql += "','"+idUsuario+"',now(),'Exportación a Excel de reportes de mantenimiento','2','1')";
            MySqlCommand exportacion = new MySqlCommand(sql,co.dbconection());
            exportacion.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void exportacionpdf1()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', '" + labelidRepSup.Text + "', 'Exportación de reporte de la unidad en archivo pdf', '"+idUsuario+
               "', NOW(), 'Exportación a PDF de reporte en Mantenimiento', '2', '1')",co.dbconection());
            cmd.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void exportacionpdf2()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', '" + labelidRepSup.Text + "', 'Exportación de reporte de fallo en archivo pdf', '"+idUsuario+"', NOW(), 'Exportación a PDF de reporte en Mantenimiento', '2', '1')",co.dbconection());
            cmd.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void validarrefacciones()
        {
            int crefacc;
            ctotal = 0;
            nnrefacc = 0;
            nrefacc = 0;
            totfalt = 0;
            falt = 0;
            exist = 0;
            String refacc;
            MySqlCommand cmd0 = new MySqlCommand("SELECT coalesce(MAX(NumRefacc), 0) AS NumRefacc, coalesce((NumRefacc), 0) AS NumRefacc1 FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd0.ExecuteReader();
            if (dr.Read())
            {
                nrefacc = Convert.ToInt32(dr.GetString("NumRefacc"));
                nnrefacc = Convert.ToInt32(dr.GetString("NumRefacc1"));
            }
            else
            {
                nrefacc = 0;
            }
            dr.Close();
           co.dbconection().Close();
            for (crefacc = 1; crefacc <= nrefacc; crefacc++)
            {
                refacc = "";
                MySqlCommand cmd1 = new MySqlCommand("SELECT coalesce((EstatusRefaccion), '') AS EstatusRefaccion, coalesce((Cantidad - CantidadEntregada), 0) AS Faltante FROM pedidosrefaccion WHERE NumRefacc = '" + nnrefacc + "' AND FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    refacc = Convert.ToString(dr1.GetString("EstatusRefaccion"));
                    falt = Convert.ToInt32(dr1.GetString("Faltante"));
                }
                else
                {
                    refacc = "";
                }
                dr1.Close();
               co.dbconection().Close();
                if ((refacc.Equals("EXISTENCIA")) || (refacc.Equals("SIN EXISTENCIA")))
                {
                    if (refacc.Equals("EXISTENCIA"))
                    {
                        exist = exist + 1;
                    }
                    totfalt = totfalt + falt;
                    ctotal = ctotal + 1;
                }
                else
                {
                    ctotal = ctotal + 0;
                }

                nnrefacc = nnrefacc + 1;
            }
        }

        public void privilegios()
        {
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal = '"+idUsuario+"' and namform = 'Mantenimiento'";
            string[] privilegios = getaData(sql).ToString().Split(';');
            pinsertar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
            pconsultar = getBoolFromInt(Convert.ToInt32(privilegios[1]));
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
            pdesactivar = getBoolFromInt(Convert.ToInt32(privilegios[3]));
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql,co.dbconection());
            var res = cm.ExecuteScalar();
           co.dbconection();
            return res;
        }

        public void AutoCompletado(TextBox cajaTexto) //Metodo De AutoCompletado
        {
            AutoCompleteStringCollection nColl = new AutoCompleteStringCollection();
            MySqlCommand cmd = new MySqlCommand("SELECT Folio FROM reportesupervicion",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    nColl.Add(dr["Folio"].ToString());
                }
            }
            dr.Close();
           co.dbconection().Close();
            textBoxFolioB.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxFolioB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxFolioB.AutoCompleteCustomSource = nColl;
        }

        public void validar() //Metodo para validar el estatus
        {
            if (labelEstatusMan.Text.Equals("-- ESTATUS --"))
            {
                comboBoxEstatusMant.SelectedIndex = 2;
            }
            else if (labelEstatusMan.Text == "EN PROCESO")
            {
                comboBoxEstatusMant.SelectedIndex = 1;
            }
            else if (labelEstatusMan.Text == "REPROGRAMADA")
            {
                comboBoxEstatusMant.SelectedIndex = 2;
            }
            if(comboBoxReqRefacc.SelectedIndex == 1)
            {
                buttonAgregar.Visible = true;
                label39.Visible = true;
            }
            else
            {
                buttonAgregar.Visible = false;
                label39.Visible = false;
            }
            labelHoraTerminoM.Text = "";
            textBoxTerminoMan.Text = "";
        }

        public void validar2()
        {
            if (crefverificadas == 2)
            {
                if (((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    timer2.Start();
                    cexis = 1;
                    crefverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
                else if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                {
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    timer2.Start();
                    cexis = 1;
                    crefverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
                else if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 1))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    timer2.Start();
                    cexis = 0;
                    crefverificadas = 0;
                    cvalidacion = 0;
                }
                else if((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (cvalidacion == 4))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    timer2.Start();
                    cexis = 0;
                    crefverificadas = 0;
                    cvalidacion = 0;
                }
                else if ((comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) && (textBoxFolioFactura.Enabled == false) && ((cvalidacion == 1) || (cvalidacion == 7)))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    timer2.Start();
                    cexis = 1;
                    crefverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
            }
            else if (crefverificadas == 1)
            {
                if ((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 6))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    if (textBoxFolioFactura.Text.Equals(""))
                    {
                        textBoxFolioFactura.Enabled = true;
                    }
                    else
                    {
                        textBoxFolioFactura.Enabled = false;
                    }
                    timer2.Start();
                    cexis = 0;
                    crefverificadas = 0;
                    cvalidacion = 0;
                }
                else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 6))
                {
                    buttonAgregar.Visible = true;
                    label39.Visible = true;
                    timer2.Start();
                    cexis = 1;
                    crefverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 1;
                }
                else if (comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN"))
                {
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    timer2.Start();
                    cexis = 1;
                    crefverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 1;
                }
            }
        }

        public void limpiarcampos() //Metodo Para Limpiar Los Campos
        {
            comboBoxFalloGral.SelectedIndex = 0;
            textBoxMecanico.Text = "";
            textBoxMecanicoApo.Text = "";
            textBoxTrabajoRealizado.Text = "";
            textBoxFolioFactura.Text = "";
            comboBoxEstatusMant.SelectedIndex = 0;
            comboBoxExisRefacc.SelectedIndex = 0;
            comboBoxReqRefacc.SelectedIndex = 0;
            textBoxSuperviso.Text = "";
            textBoxObsMan.Text = "";
            textBoxEsperaMan.Text = "";
            textBoxTerminoMan.Text = "";
            labelNomMecanico.Text = ".";
            labelNomMecanicoApo.Text = "..";
            labelNomSuperviso.Text = "...";
            labelFolioSinLetter.Text = "";
            labelFolio.Text = "";
            labelUnidad.Text = "";
            labelFechaReporte.Text = "";
            labelKm.Text = "";
            labelSupervisor.Text = "";
            labelcodfallo.Text = "";
            labelHoraReporte.Text = "";
            textBoxDescrpFallo.Text = "";
            textBoxFallaNoCod.Text = "";
            textBoxObsSup.Text = "";
            labelHoraInicioM.Text = "";
            labelHoraTerminoM.Text = "";
            labelidRepSup.Text = "";
            mensaje = false;
        }

        public void limpiarcamposbus() //Metodo Para Limpiar Los Campos
        {
            textBoxFolioB.Text = "";
            comboBoxUnidadB.SelectedIndex = 0;
            comboBoxMecanicoB.SelectedIndex = 0;
            comboBoxEstatusMB.SelectedIndex = 0;
            comboBoxDescpFalloB.SelectedIndex = 0;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            comboBoxMesB.SelectedIndex = 0;
        }

        public void limpiarrefacc() //Metodo Para Limpiar Los Campos
        {
            comboBoxFamilia.SelectedIndex = 0;
            comboBoxFRefaccion.SelectedIndex = 0;
            textBoxCantidad.Text = "";
            textBoxUM.Text = "";
        }

        public void limpiarstring()
        {
            cdf = "";
            mec = "";
            mecapo = "";
            exiref = "";
            reqref = "";
            trabreak = "";
            folfac = "";
            estmant = "";
            obsmant = "";
            crefacc = 0;
        }

        public void notcalcul() //Hace La Validacion Si La Unidad Esta Liberada
        {
            String temp = "";
            MySqlCommand cmd = new MySqlCommand("SELECT EsperaTiempoM FROM reportemantenimiento WHERE FoliofkSupervicion = '" +labelidRepSup.Text +"'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                temp = Convert.ToString(dr.GetString("EsperaTiempoM"));
            }
            if(comboBoxEstatusMant.Text.Equals("LIBERADA") || (temp != ""))
            {
                timer1.Stop(); 
            }
            else
            {
                esperaman();

            }
            dr.Close();
           co.dbconection().Close();
        }

        public void metodobtnfinalizarsref()
        {
            FormContraFinal FCF = new FormContraFinal(empresa,area,this);
            FCF.labelidSup.Text = labelidRepSup.Text;
            FCF.labelCargo.Text = cargo.ToString();
            var res = FCF.ShowDialog();
            if (res == DialogResult.OK) {
                labelidFinal = FCF.labelidFin;
                labelNombreF = FCF.labelNomFin;
                if (string.IsNullOrWhiteSpace(labelidFinal.Text))
                {
                    labelHoraTerminoM.Text = "";
                    textBoxTerminoMan.Text = "";
                    validar();
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    buttonGuardar.Visible = true;
                    label24.Visible = true;
                }
                else if (labelidFinal.Text != "")
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET PersonaFinal = '" + labelidFinal.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                    cmd.ExecuteNonQuery();
                   co.dbconection().Close();
                    metodoActualizar();
                    metodoCarga();
                    conteo();
                    MessageBox.Show("Se libero exitosamente la unidad", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarcampos();
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                    {
                        buttonExcel.Visible = true;
                        label35.Visible = true;
                        buttonPDF.Visible = true;
                        label36.Visible = true;
                        radioButtonGeneral.Visible = true;
                        radioButtonUnidad.Visible = true;
                    }
                    else
                    {
                        buttonExcel.Visible = false;
                        label35.Visible = false;
                        buttonPDF.Visible = false;
                        label36.Visible = false;
                        radioButtonGeneral.Visible = false;
                        radioButtonUnidad.Visible = false;
                    }
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                }
                timer2.Start();
                groupBoxBusqueda.Enabled = true;
            }
        }

        public bool metodotxtchref() // checar
        {
            if (dataGridViewMRefaccion.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void metodobtnfinalizarcref()
        {
            string btn = "";     
            FormContraFinal FCF = new FormContraFinal(empresa,area,this);
            FCF.Owner = this;
            FCF.labelidSup.Text = labelidRepSup.Text;
            FCF.labelCargo.Text = cargo.ToString();
            FCF.ShowDialog();
            labelidFinal = FCF.labelidFin;
            labelNombreF = FCF.labelNomFin;
            btn = FCF.labelbtn.Text;
            if ((string.IsNullOrWhiteSpace(labelidFinal.Text)) && (btn == "1"))
            {
                labelHoraTerminoM.Text = "";
                textBoxTerminoMan.Text = "";
                validar();
                buttonFinalizar.Visible = false;
                label37.Visible = false;
                buttonGuardar.Visible = true;
                label24.Visible = true;
                fquestion = 0;
            }
            else if ((labelidFinal.Text != "") && (btn == "1"))
            {
                fquestion = 1;
                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET PersonaFinal = '" + labelidFinal.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                cmd.ExecuteNonQuery();
               co.dbconection().Close();
                metodoActualizar();
                metodoCarga();
                conteo();
                MessageBox.Show("Se liberó exitosamente la unidad", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarcampos();
                limpiarstring();
                buttonGuardar.Visible = false;
                label24.Visible = false;
                buttonFinalizar.Visible = false;
                label37.Visible = false;
                if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                {
                    buttonExcel.Visible = true;
                    label35.Visible = true;
                    buttonPDF.Visible = true;
                    label36.Visible = true;
                    radioButtonGeneral.Visible = true;
                    radioButtonUnidad.Visible = true;
                }
                else
                {
                    buttonExcel.Visible = false;
                    label35.Visible = false;
                    buttonPDF.Visible = false;
                    label36.Visible = false;
                    radioButtonGeneral.Visible = false;
                    radioButtonUnidad.Visible = false;
                }
                buttonAgregar.Visible = false;
                label39.Visible = false;
            }
            else if(btn == "2")
            {
                validar();
            }
            timer2.Start();
        }

        public void metodobtnguardar()
        {
            if((labelNomSuperviso.Text == "...") && (comboBoxEstatusMant.Text.Equals("LIBERADA")))
            {
                MessageBox.Show("No se realizó ningún cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarcampos();
                limpiarstring();
                limpiarcampos();
                limpiarcamposbus();
                buttonGuardar.Visible = false;
                label24.Visible = false;
                buttonPDF.Visible = false;
                label36.Visible = false;
                radioButtonGeneral.Visible = false;
                radioButtonUnidad.Visible = false;
                buttonExcel.Visible = true;
                label35.Visible = true; 
                metodoCarga();
            }
            else
            {
                if (!(labelEstatusMan.ToString() == comboBoxEstatusMant.Text))
                {
                    notcalcul();
                }
                MySqlCommand cmd0 = new MySqlCommand("SELECT t2.FoliofkSupervicion FROM reportesupervicion AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idReporteSupervicion = t2.FoliofkSupervicion WHERE t2.FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    metodoActualizar();
                    MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string ex;
                    if(comboBoxExisRefacc.SelectedIndex == 0)
                    {
                        ex = "";
                    }
                    else
                    {
                        ex = Convert.ToString(comboBoxExisRefacc.Text);
                    }
                    
                    if ((labelNomMecanicoApo.Text.Equals("..")) && (labelNomSuperviso.Text.Equals("...")))
                    {
                        MySqlCommand cmd1 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + labelidRepSup.Text + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + comboBoxReqRefacc.Text + "', '" + ex + "')",co.dbconection());
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       co.dbconection().Close();
                    }
                    else
                    {
                        if (labelNomMecanicoApo.Text.Equals("..") && (labelNomSuperviso.Text != "..."))
                        {
                            MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, SupervisofkPersonal, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + labelidRepSup.Text + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + labelidSuperviso.Text + "','" + comboBoxReqRefacc.Text + "', '" + ex + "')",co.dbconection());
                            cmd2.ExecuteNonQuery();
                            MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           co.dbconection().Close();
                        }
                        else
                        {
                            if (labelNomSuperviso.Text.Equals("...") && (labelNomMecanicoApo.Text != ".."))
                            {
                                labelNomSuperviso.Text = "";
                                MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, MecanicoApoyofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + labelidRepSup.Text + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', '" + labelidMecanicoApo.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + comboBoxReqRefacc.Text + "', '" + ex + "')",co.dbconection());
                                cmd2.ExecuteNonQuery();
                                MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                               co.dbconection().Close();
                            }
                            else
                            {
                                MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, MecanicoApoyofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, SupervisofkPersonal, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + labelidRepSup.Text + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', '" + labelidMecanicoApo.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + labelidSuperviso.Text + "', '" + comboBoxReqRefacc.Text + "', '" + ex + "')",co.dbconection());
                                cmd2.ExecuteNonQuery();
                                MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                               co.dbconection().Close();
                            }
                        }
                    }
                    AutoCompletado(textBoxFolioB);
                }
                metodoCarga();
                limpiarstring();
                conteo();
                limpiarcampos();
                ncontreffin();
                crefacc = 0;
                inicolumn = 0;
                fincolumn = 0;
                buttonGuardar.Visible = false;
                label24.Visible = false;
                dataGridViewMantenimiento.Refresh();
                comboBoxFalloGral.Enabled = false;
                textBoxMecanico.Enabled = false;
                textBoxMecanicoApo.Enabled = false;
                textBoxFolioFactura.Enabled = false;
                textBoxTrabajoRealizado.Enabled = false;
                comboBoxEstatusMant.Enabled = false;
                comboBoxExisRefacc.Enabled = false;
                comboBoxReqRefacc.Enabled = false;
                textBoxSuperviso.Enabled = false;
                textBoxObsMan.Enabled = false;
                buttonPDF.Visible = false;
                label36.Visible = false;
                radioButtonGeneral.Visible = false;
                radioButtonUnidad.Visible = false;
                buttonAgregar.Visible = false;
                label39.Visible = false;
                if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                {
                    buttonExcel.Visible = true;
                    label35.Visible = true;
                }
                else
                {
                    buttonExcel.Visible = false;
                    label35.Visible = false;
                }
                timer1.Start();
                buttonFinalizar.Visible = false;
                label37.Visible = false;
                buttonAgregar.Visible = false;
                label39.Visible = false;
            }
        }
  
        public void llamadadatos()
        {
            label1.Visible = false;
            groupBoxRefacciones.Visible = false;
            groupBoxMantenimiento.Visible = true;
            timer2.Start();
            limpiarcampos();
            limpiarcamposbus();
            comboBoxFalloGral.Enabled = false;
            textBoxMecanico.Enabled = false;
            textBoxMecanicoApo.Enabled = false;
            comboBoxReqRefacc.Enabled = false;
            comboBoxExisRefacc.Enabled = false;
            textBoxFolioFactura.Enabled = false;
            textBoxTrabajoRealizado.Enabled = false;
            comboBoxEstatusMant.Enabled = false;
            textBoxSuperviso.Enabled = false;
            textBoxObsMan.Enabled = false;
            buttonExcel.Visible = false;
            label35.Visible = false;
            buttonAgregar.Visible = false;
            label39.Visible = false;
            buttonActualizar.Visible = true;
            label26.Visible = true;
            buttonGuardar.Visible = true;
            label24.Visible = true;
            labelFolioSinLetter.Text = dataGridViewMantenimiento.CurrentRow.Cells["FOLIO"].Value.ToString();
            labelEstatusMan.Text = dataGridViewMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT r25.FoliofkSupervicion FROM reportemantenimiento AS r25 WHERE r25.FoliofkSupervicion = t1.idReporteSupervicion), '') AS Folio, coalesce((SELECT r25.IdReporte FROM reportemantenimiento AS r25 WHERE t1.idReporteSupervicion = r25.FoliofkSupervicion), '') AS IdMantenimiento, coalesce((SELECT r22.MecanicofkPersonal FROM reportemantenimiento AS r22 WHERE t1.idReporteSupervicion = r22.FoliofkSupervicion), '') AS IdMecanico, coalesce((SELECT r23.MecanicoApoyofkPersonal FROM reportemantenimiento AS r23 WHERE t1.idReporteSupervicion = r23.FoliofkSupervicion), '') AS IdMecanicoApoyo, coalesce((SELECT r24.SupervisofkPersonal FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS IdSuperviso, t1.idReporteSupervicion AS ID, t1.Folio, CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'Fecha Del Reporte', coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, t1.HoraEntrada AS 'Hora De Entrada', t1.KmEntrada AS 'Kilometraje', UPPER(t1.TipoFallo) AS 'Tipo De Fallo', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'Descripcion De Fallo', coalesce((SELECT UPPER(r22.codfallo) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'Codigo De Fallo', coalesce((UPPER(t1.DescFalloNoCod)), '') AS 'Descripcion De Fallo No Codificado', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'Observaciones De Supervision', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'Fallo General', coalesce((SELECT UPPER(r4.idfallogral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'IdFG', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'Mecanico', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'Mecanico De Apoyo', coalesce((SELECT UPPER(DATE_FORMAT(r10.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r10 WHERE t1.idReporteSupervicion = r10.FoliofkSupervicion), '') AS 'Fecha Del Reporte De Mantenimiento', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'Hora De Inicio De Mantenimiento', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'Hora De Termino De Mantenimiento', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'Espera De Tiempo Para Mantenimiento', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'Diferencia De Tiempo En Mantenimiento', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'Folio De Factura', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'Estatus Del Mantenimiento', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'Superviso', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'Existencia De Refacciones En Almacen', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'Estatus De Refacciones', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'Observaciones Del Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cservicios AS t3 ON t1.Serviciofkcservicios = t3.idservicio INNER JOIN careas AS t4 ON t2.areafkcareas= t4.idarea WHERE t1.Folio = '" + labelFolioSinLetter.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
           co.dbconection().Close();
            if (dr.Read())
            {
                folio = Convert.ToString(dr.GetString("Folio"));
                labelFecha.Text = Convert.ToString(dr.GetString("Fecha Del Reporte De Mantenimiento"));
                labelidMecanico.Text = Convert.ToString(dr.GetString("IdMecanico"));
                labelidMecanicoApo.Text = Convert.ToString(dr.GetString("IdMecanicoApoyo"));
                labelidSuperviso.Text = Convert.ToString(dr.GetString("IdSuperviso"));
                labelidRepMant.Text = Convert.ToString(dr.GetString("IdMantenimiento"));
                labelidRepSup.Text = Convert.ToString(dr.GetString("ID"));
                labelFolio.Text = labelFolioSinLetter.Text;
                labelUnidad.Text = Convert.ToString(dr.GetString("ECO"));
                labelFechaReporte.Text = Convert.ToString(dr.GetString("Fecha Del Reporte"));
                labelHoraReporte.Text = Convert.ToString(dr.GetString("Hora De Entrada"));
                labelKm.Text = Convert.ToString(dr.GetString("Kilometraje"));
                labelSupervisor.Text = Convert.ToString(dr.GetString("Supervisor"));
                labelcodfallo.Text = Convert.ToString(dr.GetString("Codigo De Fallo"));
                textBoxDescrpFallo.Text = Convert.ToString(dr.GetString("Descripcion De Fallo"));
                textBoxFallaNoCod.Text = Convert.ToString(dr.GetString("Descripcion De Fallo No Codificado"));
                textBoxObsSup.Text = Convert.ToString(dr.GetString("Observaciones De Supervision"));
                comboBoxReqRefacc.Text = Convert.ToString(dr.GetString("Estatus De Refacciones"));
                reqref = comboBoxReqRefacc.Text;
                if(comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                {
                    reqref = "";
                }
                labelNomMecanico.Text = Convert.ToString(dr.GetString("Mecanico"));
                mec = labelNomMecanico.Text;
                if(string.IsNullOrWhiteSpace(mec))
                {
                    labelNomMecanico.Text = ".";
                    mec = ".";
                }

                labelHoraInicioM.Text = Convert.ToString(dr.GetString("Hora De Inicio De Mantenimiento"));
                if ((string.IsNullOrWhiteSpace(labelHoraInicioM.Text)) || (labelHoraInicioM.Text == "00:00:00"))
                {
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                    dateTimePicker1.Text = Convert.ToString(dr.GetString("Hora De Inicio De Mantenimiento"));
                }

                comboBoxEstatusMant.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                estmant = comboBoxEstatusMant.Text;
                labelEstatusMan.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                if (comboBoxEstatusMant.SelectedIndex == 0)
                {
                    comboBoxEstatusMant.SelectedIndex = 1;
                    estmant = comboBoxEstatusMant.Text;
                    labelEstatusMan.Text = estmant;
                }
                comboBoxExisRefacc.Text = Convert.ToString(dr.GetString("Existencia De Refacciones En Almacen").ToString());
                exiref = Convert.ToString(dr.GetString("Existencia De Refacciones En Almacen").ToString());
                if (comboBoxExisRefacc.SelectedIndex == 0)
                {
                    exiref = "";
                }
                comboBoxFalloGral.Text = Convert.ToString(dr.GetString("Fallo General"));
                cdf = comboBoxFalloGral.Text;
                idfg = Convert.ToString(dr.GetString("IdFG"));
                if(comboBoxFalloGral.SelectedIndex == 0)
                {
                    cdf = "";
                }
                validarstatusfallagral(cdf);
                labelNomMecanicoApo.Text = Convert.ToString(dr.GetString("Mecanico De Apoyo"));
                mecapo = labelNomMecanicoApo.Text;
                textBoxEsperaMan.Text = Convert.ToString(dr.GetString("Espera De Tiempo Para Mantenimiento"));
                textBoxFolioFactura.Text = Convert.ToString(dr.GetString("Folio De Factura"));
                folfac = textBoxFolioFactura.Text;
                textBoxTrabajoRealizado.Text = Convert.ToString(dr.GetString("Trabajo Realizado"));
                trabreak = textBoxTrabajoRealizado.Text;
                labelNomSuperviso.Text = Convert.ToString(dr.GetString("Superviso").ToString());
                supmant = labelNomSuperviso.Text;
                textBoxObsMan.Text = Convert.ToString(dr.GetString("Observaciones Del Mantenimiento"));
                obsmant = textBoxObsMan.Text;

                //Validaciones

                if ((comboBoxEstatusMant.Text.Equals("LIBERADA"))&& (labelNomSuperviso.Text == ""))
                {
                    timer2.Stop();
                    dateTimePicker2.Text = Convert.ToString(dr.GetString("Hora De Termino De Mantenimiento"));
                    labelHoraTerminoM.Text = dateTimePicker2.Text;
                    textBoxTerminoMan.Text = Convert.ToString(dr.GetString("Diferencia De Tiempo En Mantenimiento"));
                    comboBoxFalloGral.Enabled = false;
                    textBoxMecanico.Enabled = false;
                    textBoxMecanicoApo.Enabled = false;
                    comboBoxReqRefacc.Enabled = false;
                    textBoxTrabajoRealizado.Enabled = false;
                    comboBoxEstatusMant.Enabled = false;
                    textBoxFolioFactura.Enabled = false;
                    textBoxSuperviso.Enabled = true;
                    textBoxObsMan.Enabled = false;
                    buttonGuardar.Visible = true;
                    label24.Visible = true;
                    if(labelNomMecanicoApo.Text.Equals(""))
                    {
                        labelNomMecanicoApo.Text = "..";
                    }
                    labelNomSuperviso.Text = "...";
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                }
                else if((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (labelNomSuperviso.Text != ""))
                {
                    timer2.Stop();
                    dateTimePicker2.Text = Convert.ToString(dr.GetString("Hora De Termino De Mantenimiento"));
                    labelHoraTerminoM.Text = dateTimePicker2.Text;
                    textBoxTerminoMan.Text = Convert.ToString(dr.GetString("Diferencia De Tiempo En Mantenimiento"));
                    comboBoxFalloGral.Enabled = false;
                    textBoxMecanico.Enabled = false;
                    textBoxMecanicoApo.Enabled = false;
                    comboBoxReqRefacc.Enabled = false;
                    textBoxTrabajoRealizado.Enabled = false;
                    comboBoxEstatusMant.Enabled = false;
                    textBoxFolioFactura.Enabled = false;
                    textBoxSuperviso.Enabled = false;
                    textBoxObsMan.Enabled = false;
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                    if (labelNomMecanicoApo.Text.Equals(""))
                    {
                        labelNomMecanicoApo.Text = "..";
                    }
                }

                dr.Close();
               co.dbconection().Close();

                if ((comboBoxFalloGral.Text.Equals("-- FALLO GENERAL --")) && (labelNomMecanico.Text.Equals(".")))
                {
                    comboBoxFalloGral.Enabled = true;
                    textBoxMecanico.Enabled = true;
                    labelNomMecanico.Text = ".";
                    textBoxMecanicoApo.Enabled = true;
                    labelNomMecanicoApo.Text = "..";
                    comboBoxEstatusMant.Enabled = true;
                    comboBoxEstatusMant.SelectedIndex = 1;
                    if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                    {
                        comboBoxReqRefacc.Enabled = true;
                    }
                    labelNomSuperviso.Text = "...";
                }
                else if ((comboBoxEstatusMant.Text.Equals("-- ESTATUS --")) || (comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA")))
                {
                    comboBoxEstatusMant.Enabled = true;
                    if(string.IsNullOrWhiteSpace(labelNomMecanico.Text))
                    {
                        textBoxMecanico.Enabled = true;
                        labelNomMecanico.Text = ".";
                        mec =labelNomMecanico.Text;
                    }

                    if (string.IsNullOrWhiteSpace(labelNomMecanicoApo.Text))
                    {
                        textBoxMecanicoApo.Enabled = true;
                        labelNomMecanicoApo.Text = "..";
                        mecapo = labelNomMecanicoApo.Text;
                    }

                    if (string.IsNullOrWhiteSpace(labelNomSuperviso.Text))
                    {
                        textBoxSuperviso.Enabled = true;
                        labelNomSuperviso.Text = "...";
                        supmant = labelNomSuperviso.Text;
                    }

                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                    {
                        textBoxFolioFactura.Enabled = true;
                    }

                    if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                    {
                        textBoxTrabajoRealizado.Enabled = true;
                    }

                    if (string.IsNullOrWhiteSpace(textBoxObsMan.Text))
                    {
                        textBoxObsMan.Enabled = true;
                    }

                    if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == false) && (comboBoxEstatusMant.Text != "LIBERADA"))
                    {
                        buttonAgregar.Visible = true;
                        label39.Visible = true;
                        comboBoxExisRefacc.Enabled = true;
                    }

                    else if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == false) && (labelEstatusMan.Text.Equals("LIBERADA")))
                    {
                        buttonAgregar.Visible = false;
                        label39.Visible = false;
                        buttonGuardar.Visible = false;
                        label24.Visible = false;
                        buttonFinalizar.Visible = false;
                        label37.Visible = false;
                        buttonEditar.Visible = false;
                        label58.Visible = false;
                    }
                    else if ((comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES")) && (comboBoxEstatusMant.Text != "LIBERADA"))
                    {
                        comboBoxReqRefacc.Enabled = true;
                        comboBoxExisRefacc.Enabled = false;
                        textBoxFolioFactura.Enabled = false;
                    }

                    if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                    {
                        if((comboBoxEstatusMant.Text != "LIBERADA") && (textBoxFolioFactura.Text != ""))
                        {
                            textBoxFolioFactura.Enabled = false;
                        }
                        else
                        {
                            textBoxFolioFactura.Enabled = true;
                        }
                    }
                    else
                    {
                        textBoxFolioFactura.Enabled = false;
                    }
                }      

                metodocargaref();
                conteoiniref();
                conteofinref();

                privilegios();

                if (((labelrefini.Text != "0") && (labelNomMecanico.Text != ".")) && (pconsultar == true))
                {
                    buttonPDF.Visible = true;
                    label36.Visible = true;
                    radioButtonGeneral.Visible = true;
                    radioButtonUnidad.Visible = true;
                }
                else if ((labelNomMecanico.Text != ".") && (pconsultar== true))
                {
                    buttonPDF.Visible = true;
                    label36.Visible = true;
                    radioButtonGeneral.Visible = true;
                    radioButtonUnidad.Visible = true;
                }
                else
                {
                    buttonPDF.Visible = false;
                    label36.Visible = false;
                    radioButtonGeneral.Visible = false;
                    radioButtonUnidad.Visible = false;         
                }
            }
        }

        public void esperaman() //Saca El Tiempo Total De En Espera
        {
            
            DateTime dt = dateTimePicker1.Value;
            DateTime dt2 = Convert.ToDateTime(labelFechaReporte.Text + " " + labelHoraReporte.Text);
            TimeSpan s2 = dt.Subtract(dt2);
            string horaminu = Convert.ToString(s2.Hours.ToString() + ":" + s2.Minutes.ToString() + ":" + s2.Seconds.ToString());
            dateTimePicker5.Text = horaminu.ToString();
            if (s2.Hours.Equals(0))
            {
                textBoxEsperaMan.Text = s2.Days.ToString() + " Dias con " + dateTimePicker5.Text + " Minutos";
            }
            else
            {
                textBoxEsperaMan.Text = s2.Days.ToString() + " Dias con " + dateTimePicker5.Text + " Horas";
            }
            labelHoraInicioM.Text = dateTimePicker1.Text;
        }

        public void sumafech() //Suma Las Fechas Para Obtener El Tiempo Final
        {
            dateTimePicker1.Value = Convert.ToDateTime(labelFechaReporte.Text + " " + labelHoraReporte.Text);
            DateTime tdt1 = dateTimePicker1.Value;
            DateTime tdt2 = dateTimePicker2.Value;
            TimeSpan ts = tdt2.Subtract(tdt1);
            string finhoraminu = Convert.ToString(ts.Hours.ToString() + ":" +ts.Minutes.ToString() + ":" +ts.Seconds.ToString());
            dateTimePicker5.Text = finhoraminu.ToString();
            if(ts.Hours.Equals(0))
            {
                textBoxTerminoMan.Text = ts.Days.ToString() + " Dias con " + dateTimePicker5.Text + " Minutos";
            }
            else
            {
                textBoxTerminoMan.Text = ts.Days.ToString() + " Dias con " + dateTimePicker5.Text + " Horas";
            }
        }

        public void metodoCarga() //Metodo Que Carga Los Reportes
        {
            DataTable dt = new DataTable();
            MySqlCommand comando = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT t1.Folio AS 'FOLIO', CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'FECHA DEL REPORTE', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'ESTATUS DEL MANTENIMIENTO',  coalesce((SELECT UPPER(CONCAT(r22.codfallo, ' - ', r22.falloesp)) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'CODIGO DE FALLO', coalesce((SELECT UPPER(DATE_FORMAT(r24.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'FECHA DEL REPORTE DE MANTENIMIENTO', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'MECANICO', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'MECANICO DE APOYO',  coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS 'SUPERVISOR', UPPER(t1.HoraEntrada) AS 'HORA DE ENTRADA', UPPER(t1.TipoFallo) AS 'TIPO DE FALLO', UPPER(t1.KmEntrada) AS 'KILOMETRAJE', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'DESCRIPCION DE FALLO', UPPER(t1.DescFalloNoCod) AS 'DESCRIPCION DE FALLO NO CODIFICADO', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'OBSERVACIONES DE SUPERVISION', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'FALLO GENERAL', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'TRABAJO REALIZADO', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'HORA DE INICIO DE MANTENIMIENTO', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'HORA DE TERMINO DE MANTENIMIENTO', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'ESPERA DE TIEMPO PARA MANTENIMIENTO', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'DIFERENCIA DE TIEMPO EN MANTENIMIENTO', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'FOLIO DE FACTURA', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'SUPERVISO', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'EXISTENCIA DE REFACCIONES EN ALMACEN', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'ESTATUS DE REFACCIONES', coalesce((SELECT UPPER(CONCAT(r23.ApPaterno, ' ', r23.ApMaterno, ' ', r23.nombres)) FROM reportemantenimiento AS r24 INNER JOIN cpersonal AS r23 ON r24.PersonaFinal = r23.idPersona WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'PERSONA QUE FINALIZO EL MANTENIMIENTO', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'OBSERVACIONES DEL MANTENIMIENTO' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cservicios AS t3 ON t1.Serviciofkcservicios = t3.idservicio INNER JOIN careas AS t4 ON t2.areafkcareas = t4.idarea WHERE (SELECT t1.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) ORDER BY t1.Folio DESC",co.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(comando);
            adp.Fill(dt);
            dataGridViewMantenimiento.DataSource = dt;
            dataGridViewMantenimiento.ClearSelection();
            dataGridViewMantenimiento.Columns[0].Frozen = true;
           co.dbconection().Close();
        }

        public void metodocargaref() //Metodo Para Cargar Los Datos De Las Refacciones
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT NumRefacc AS 'PARTIDA', coalesce((SELECT UPPER(r1.nombreRefaccion) FROM crefacciones AS r1 WHERE r1.idrefaccion = RefaccionfkCRefaccion), '') AS 'REFACCION', coalesce((SELECT CONCAT(r2.Simbolo, ' - ', UPPER(r2.Nombre)) FROM cunidadmedida AS r2 INNER JOIN crefacciones AS r3 ON r2.idunidadmedida = r3.umfkcunidadmedida WHERE r3.idRefaccion = RefaccionfkCRefaccion), '') AS 'UNIDAD DE MEDIDA', UPPER(DATE_FORMAT(FechaPedido, '%W %d %M %Y')) AS 'FECHA DE PEDIDO', Cantidad AS 'CANTIDAD SOLICITADA', coalesce((CantidadEntregada), 0) AS 'CANTIDAD ENTREGADA', coalesce((Cantidad - CantidadEntregada), 0) AS 'CANTIDAD POR ENTREGAR', coalesce((UPPER(EstatusRefaccion)), '') AS 'ESTATUS DE LA REFACCION' FROM pedidosrefaccion WHERE FolioPedfkSupervicion ='" + labelidRepSup.Text + "' ORDER BY NumRefacc ASC",co.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            dataGridViewMRefaccion.DataSource = dt;
           co.dbconection().Close();
        }

        public void metodocargarefpdf() //Metodo Para Cargar Los Datos De Las Refacciones Para El PDF
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT NumRefacc AS 'PARTIDA', coalesce((SELECT UPPER(r1.nombreRefaccion) FROM crefacciones AS r1 WHERE r1.idrefaccion = RefaccionfkCRefaccion), '') AS 'REFACCION', coalesce((SELECT CONCAT(r2.Simbolo, ' - ', UPPER(r2.Nombre)) FROM cunidadmedida AS r2 INNER JOIN crefacciones AS r3 ON r2.idunidadmedida = r3.umfkcunidadmedida WHERE r3.idRefaccion = RefaccionfkCRefaccion), '') AS 'UNIDAD DE MEDIDA', UPPER(DATE_FORMAT(FechaPedido, '%W %d %M %Y')) AS 'FECHA DE PEDIDO', Cantidad AS 'CANTIDAD SOLICITADA', coalesce((CantidadEntregada), 0) AS 'CANTIDAD ENTREGADA', coalesce((Cantidad - CantidadEntregada), 0) AS 'CANTIDAD POR ENTREGAR', coalesce((UPPER(EstatusRefaccion)), '') AS 'ESTATUS DE LA REFACCION' FROM pedidosrefaccion WHERE FolioPedfkSupervicion ='" + labelidRepSup.Text + "' ORDER BY NumRefacc ASC",co.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            dataGridViewMRefaccion.DataSource = dt;
           co.dbconection().Close();
        }

        public void metodoActualizar() //Actualiza algun registro
        {
            string ex;
            if(comboBoxExisRefacc.SelectedIndex == 0)
            {
                ex = "";
            }
            else
            {
                ex = Convert.ToString(comboBoxExisRefacc.Text);
            }

            if((labelNomMecanicoApo.Text.Equals("..")) && (labelNomSuperviso.Text.Equals("...")))
            {
                MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET FoliofkSupervicion = '" + labelidRepSup.Text + "', FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', MecanicofkPersonal = '" + labelidMecanico.Text + "', HoraInicioM = '" + labelHoraInicioM.Text + "', HoraTerminoM = '" + labelHoraTerminoM.Text + "', EsperaTiempoM = '" + textBoxEsperaMan.Text + "', DiferenciaTiempoM = '" + textBoxTerminoMan.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', Estatus = '" + comboBoxEstatusMant.Text + "', ExistenciaRefaccAlm = '" + ex + "', StatusRefacciones= '" + comboBoxReqRefacc.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                cmd.ExecuteNonQuery();
               co.dbconection().Close();
            }
            else
            {
                if(labelNomMecanicoApo.Text.Equals("..") && (labelNomSuperviso.Text != "..."))
                {
                    MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET FoliofkSupervicion = '" + labelidRepSup.Text + "', FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', MecanicofkPersonal = '" + labelidMecanico.Text + "', HoraInicioM = '" + labelHoraInicioM.Text + "', HoraTerminoM = '" + labelHoraTerminoM.Text + "', EsperaTiempoM = '" + textBoxEsperaMan.Text + "', DiferenciaTiempoM = '" + textBoxTerminoMan.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', Estatus = '" + comboBoxEstatusMant.Text + "', SupervisofkPersonal = '" + labelidSuperviso.Text + "', ExistenciaRefaccAlm = '" + ex + "', StatusRefacciones= '" + comboBoxReqRefacc.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                    cmd.ExecuteNonQuery();
                   co.dbconection().Close();
                }
                else
                {
                    if (labelNomSuperviso.Text.Equals("...") && (labelNomMecanicoApo.Text != ".."))
                    {
                        MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET FoliofkSupervicion = '" + labelidRepSup.Text + "', FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', MecanicofkPersonal = '" + labelidMecanico.Text + "', MecanicoApoyofkPersonal = '" + labelidMecanicoApo.Text + "', HoraInicioM = '" + labelHoraInicioM.Text + "', HoraTerminoM = '" + labelHoraTerminoM.Text + "', EsperaTiempoM = '" + textBoxEsperaMan.Text + "', DiferenciaTiempoM = '" + textBoxTerminoMan.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', Estatus = '" + comboBoxEstatusMant.Text + "', ExistenciaRefaccAlm = '" + ex + "', StatusRefacciones= '" + comboBoxReqRefacc.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                        cmd.ExecuteNonQuery();
                       co.dbconection().Close();
                    }
                    else
                    {
                        MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET FoliofkSupervicion = '" + labelidRepSup.Text + "', FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', MecanicofkPersonal = '" + labelidMecanico.Text + "', MecanicoApoyofkPersonal = '" + labelidMecanicoApo.Text + "', HoraInicioM = '" + labelHoraInicioM.Text + "', HoraTerminoM = '" + labelHoraTerminoM.Text + "', EsperaTiempoM = '" + textBoxEsperaMan.Text + "', DiferenciaTiempoM = '" + textBoxTerminoMan.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', Estatus = '" + comboBoxEstatusMant.Text + "', SupervisofkPersonal = '" + labelidSuperviso.Text + "', ExistenciaRefaccAlm = '" + ex + "', StatusRefacciones= '" + comboBoxReqRefacc.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                        cmd.ExecuteNonQuery();
                       co.dbconection().Close();
                    }
                } 
            }
        }

        public void conteo() //Realiza El Conteo De Los Reportes
        {
            MySqlCommand cmd = new MySqlCommand("SELECT (SELECT COUNT(b1.Estatus) AS EstatusEnProc FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion WHERE Estatus = 'EN PROCESO' && (b2.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate())) AS EstatusEnProceso, (SELECT COUNT(Estatus) AS EstatusReprog FROM reportemantenimiento WHERE Estatus = 'REPROGRAMADA' && (FechaReporteM BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate())) AS EstatusReprogramada, coalesce((SELECT COUNT(t1.idReporteSupervicion) AS EstatusEnEspera FROM reportesupervicion AS t1 WHERE t1.idReporteSupervicion NOT IN(SELECT t2.FoliofkSupervicion FROM reportemantenimiento AS t2 WHERE t1.idReporteSupervicion = t2.FoliofkSupervicion) && (FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()))) AS 'Estatus En Espera', COUNT(r1.Estatus) AS EstatusLib FROM reportemantenimiento AS r1 INNER JOIN reportesupervicion AS r2 ON r1.FoliofkSupervicion = r2.idReporteSupervicion WHERE Estatus = 'LIBERADA' && (r2.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate())",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                textBoxLiberadas.Text = Convert.ToString(dr.GetString("EstatusLib"));
                textBoxEnProceso.Text = Convert.ToString(dr.GetString("EstatusEnProceso"));
                textBoxEnEspera.Text = Convert.ToString(dr.GetString("Estatus En Espera"));
                textBoxReprogramados.Text = Convert.ToString(dr.GetString("EstatusReprogramada"));
            }
            dr.Close();
           co.dbconection().Close();
        }

        public void conteovariable()
        {
            string varproceso = "", varreprogramada = "", varespera = "", varliberada = "", wh = "";
            String Fini = "";
            String Ffin = "";

            if (!string.IsNullOrWhiteSpace(textBoxFolioB.Text))
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.Folio = '" + textBoxFolioB.Text + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    wh = " WHERE FoliofkSupervicion = (SELECT w1.FoliofkSupervicion AS EstatusEnProc FROM reportemantenimiento AS w1 INNER JOIN reportesupervicion AS w2 ON w1.FoliofkSupervicion = w2.idReporteSupervicion WHERE w2.Folio = '" + textBoxFolioB.Text +"')";
                }
                else
                {
                    varproceso += " AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    varreprogramada += " AND b2.Folio = '" + textBoxFolioB.Text  + "'";
                    varespera += " AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    varliberada += " AND b2.Folio = '" + textBoxFolioB.Text + "'";
                }
            }

            if (comboBoxUnidadB.SelectedIndex > 0)
            {
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                }
                else
                {
                    varproceso += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                    varreprogramada += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                    varespera += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                    varliberada += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "'";
                }
            }

            if (comboBoxMecanicoB.SelectedIndex > 0)
            {
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                    varespera = " WHERE (b1.Estatus = '' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                }
                else
                {
                    varproceso += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                    varreprogramada += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                    varespera += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                    varliberada += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "'";
                }
            }

            if (comboBoxEstatusMB.SelectedIndex > 0)
            {
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                    varespera = " WHERE (b1.Estatus = '' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                }
                else
                {
                    varproceso += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                    varreprogramada += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                    varespera += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                    varliberada += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "'";
                }
            }

            if(checkBoxFechas.Checked == true)
            {
                Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND ((SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "'))";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                }
                else
                {
                    varproceso += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    varreprogramada += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    varespera += " AND ((SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    varliberada += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                }
            }

            if(comboBoxDescpFalloB.SelectedIndex > 0)
            {
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                }
                else
                {
                    varproceso += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                    varreprogramada += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                    varespera += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                    varliberada += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "'";
                }
            }

            if(comboBoxMesB.SelectedIndex > 0)
            {
                if(varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' and DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')";
                }
                else
                {
                    varproceso += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                    varreprogramada += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                    varespera += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                    varliberada += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11'))";
                }
            }

            string Cconsulta = "SELECT (SELECT COUNT(b1.Estatus) AS EstatusEnProc FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varproceso + ") AS EstatusEnProceso, (SELECT COUNT(Estatus) AS EstatusReprog FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varreprogramada + ") AS EstatusReprogramada, coalesce((SELECT COUNT(b2.idReporteSupervicion) AS EstatusEnEspera FROM reportesupervicion AS b2" + varespera + "), '') AS 'Estatus En Espera', COUNT(Estatus) AS EstatusLib FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varliberada + "" + "";
            MySqlCommand cmd = new MySqlCommand(Cconsulta,co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                textBoxLiberadas.Text = Convert.ToString(dr.GetString("EstatusLib"));
                textBoxEnProceso.Text = Convert.ToString(dr.GetString("EstatusEnProceso"));
                textBoxEnEspera.Text = Convert.ToString(dr.GetString("Estatus En Espera"));
                textBoxReprogramados.Text = Convert.ToString(dr.GetString("EstatusReprogramada"));
            }
            else
            {

            }
            dr.Close();
           co.dbconection().Close();
        }

        public void conteoiniref() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                labelrefini.Text = Convert.ToString(dr.GetString("Folio"));
            }
            dr.Close();
           co.dbconection().Close();
        }

        public void conteofinref() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelreffin.Text = Convert.ToString(dr.GetString("Folio"));
            }
            dr.Close();
           co.dbconection().Close();
        }

        public void ncontrefini() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                contreini = Convert.ToInt32(dr.GetString("Folio"));
            }
            dr.Close();
           co.dbconection().Close();
        }

        public void ncontreffin() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                contrefin = Convert.ToInt32(dr.GetString("Folio"));
            }
            dr.Close();
           co.dbconection().Close();
        }

        private void To_pdf2()
        {
            string hoy = "";
            string foliopdf = "", eco = "", supervisor = "", fecha = "", hora = "", km = "", codfallo = "", desfallo = "", desfallonocod = "", observacionessup = "";
            string fallogen = "", estatus = "", mecanico = "", apomecanico = "", superviso = "", extrefacc = "", reqrefacc = "", facfoliopdf = "", inicio = "", termino = "", espera = "", diferencia = "", trabrealizado = "", observacionesmant = "";
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT UPPER(t1.Folio) AS Folio, UPPER(CONCAT(t3.identificador, LPAD(consecutivo, 4,'0'))) AS ECO, UPPER(coalesce((SELECT CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres) FROM cpersonal AS t4 WHERE t1.SupervisorfkCPersonal = t4.idPersona), '')) AS Supervisor, UPPER(coalesce((DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')), '')) AS FechaReporte, UPPER(coalesce((t1.HoraEntrada) , '')) AS HoraEntrada, UPPER(coalesce((t1.KmEntrada), '')) AS KmEntrada, UPPER(coalesce((SELECT t4.codfallo FROM cfallosesp AS t4 WHERE t1.CodFallofkcfallosesp = t4.idfalloEsp), '')) AS 'Codigo De Fallo', UPPER(coalesce((SELECT t5.descfallo FROM cdescfallo AS t5 WHERE t1.DescrFallofkcdescfallo = t5.iddescfallo), '')) AS 'Descripcion De Fallo', UPPER(coalesce((t1.DescFalloNoCod), '')) AS DescFalloNoCod, UPPER(coalesce((t1.ObservacionesSupervision), '')) AS ObservacionesSupervision, UPPER(coalesce((SELECT t5.nombreFalloGral FROM cfallosgrales AS t5 INNER JOIN reportemantenimiento AS t6 ON t6.FalloGralfkFallosGenerales = t5.idFalloGral WHERE t6.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Fallo General', UPPER(coalesce((SELECT t7.Estatus FROM reportemantenimiento AS t7 WHERE t7.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Estatus Del Mantenimiento', UPPER(coalesce((SELECT CONCAT(t8.ApPaterno, ' ', t8.ApMaterno, ' ', t8.nombres) FROM cpersonal AS t8 INNER JOIN reportemantenimiento AS t9 ON t8.idPersona = t9.MecanicofkPersonal WHERE t9.FoliofkSupervicion = t1.idReporteSupervicion))) AS Mecanico, UPPER(coalesce((SELECT CONCAT(t10.ApPaterno, ' ', t10.ApMaterno, ' ', t10.nombres) FROM cpersonal AS t10 INNER JOIN reportemantenimiento AS t11 ON t10.idPersona = t11.MecanicoApoyofkPersonal WHERE t11.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Mecanico De Apoyo', UPPER(coalesce((SELECT CONCAT(t12.ApPaterno, ' ', t12.ApMaterno, ' ', t12.nombres) FROM cpersonal AS t12 INNER JOIN reportemantenimiento AS t13 ON t12.idPersona = t13.SupervisofkPersonal WHERE t13.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS Superviso, UPPER(coalesce((SELECT t14.StatusRefacciones FROM reportemantenimiento AS t14 WHERE t14.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS Estatus, UPPER(coalesce((SELECT t15.ExistenciaRefaccAlm FROM reportemantenimiento AS t15 WHERE t15.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Existencia De Refacciones', UPPER(coalesce((SELECT t16.FolioFactura FROM reportemantenimiento AS t16 WHERE t16.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Folio De Factura', UPPER(coalesce((SELECT t17.HoraInicioM FROM reportemantenimiento AS t17 WHERE t17.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Hora De Inicio', UPPER(coalesce((SELECT t18.HoraTerminoM FROM reportemantenimiento AS t18 WHERE t18.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Hora De Termino', UPPER(coalesce((SELECT t19.EsperaTiempoM FROM reportemantenimiento AS t19 WHERE t19.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Tiempo De Espera', UPPER(coalesce((SELECT t20.DiferenciaTiempoM FROM reportemantenimiento AS t20 WHERE t20.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS 'Diferencia De Tiempo', UPPER(coalesce((SELECT t21.TrabajoRealizado FROM reportemantenimiento AS t21 WHERE t21.FoliofkSupervicion =t1.idReporteSupervicion), '')) AS 'Trabajo Realizado', UPPER(coalesce((SELECT t22.ObservacionesM FROM reportemantenimiento AS t22 WHERE t22.FoliofkSupervicion = t1.idReporteSupervicion), '')) AS Observaciones,  UPPER(coalesce((DATE_FORMAT(now(), '%W %d %M %Y %H:%i:%s')), '')) AS Hoy FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea WHERE t1.Folio = '" + labelFolio.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                hoy = Convert.ToString(dr.GetString("hoy"));
                //Supervisión
                foliopdf = Convert.ToString(dr.GetString("Folio"));
                eco = Convert.ToString(dr.GetString("ECO"));
                supervisor = Convert.ToString(dr.GetString("Supervisor"));
                fecha = Convert.ToString(dr.GetString("FechaReporte"));
                hora = Convert.ToString(dr.GetString("HoraEntrada"));
                km = Convert.ToString(dr.GetString("KmEntrada"));
                codfallo = Convert.ToString(dr.GetString("Codigo De Fallo"));
                desfallo = Convert.ToString(dr.GetString("Descripcion De Fallo"));
                desfallonocod = Convert.ToString(dr.GetString("DescFalloNoCod"));
                observacionessup = Convert.ToString(dr.GetString("ObservacionesSupervision"));
                //Mantenimiento
                fallogen = Convert.ToString(dr.GetString("Fallo General"));
                estatus = Convert.ToString(dr.GetString("Estatus Del Mantenimiento"));
                mecanico = Convert.ToString(dr.GetString("Mecanico"));
                apomecanico = Convert.ToString(dr.GetString("Mecanico De Apoyo"));
                superviso = Convert.ToString(dr.GetString("Superviso"));
                reqrefacc = Convert.ToString(dr.GetString("Estatus"));
                extrefacc = Convert.ToString(dr.GetString("Existencia De Refacciones"));
                facfoliopdf = Convert.ToString(dr.GetString("Folio De Factura"));
                inicio = Convert.ToString(dr.GetString("Hora De Inicio"));
                termino = Convert.ToString(dr.GetString("Hora De Termino"));
                espera = Convert.ToString(dr.GetString("Tiempo De Espera"));
                diferencia = Convert.ToString(dr.GetString("Diferencia De Tiempo"));
                trabrealizado = Convert.ToString(dr.GetString("Trabajo Realizado"));
                observacionesmant = Convert.ToString(dr.GetString("Observaciones"));
            }
            dr.Close();
           co.dbconection().Close();
            Document document = new Document(PageSize.LETTER);
            document.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 80;
            table.LockedWidth = true;
            float[] widths = new float[] {.8f, .8f, .8f, .8f, .8f, .8f, .8f,};
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            name = "";
            sname = "";
            ext = "";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    name = Path.GetFileName(filename);
                    sname = Path.GetFileNameWithoutExtension(name);
                    ext = Path.GetExtension(name);
                    if (ext != ".pdf")
                    {
                        sname = sname + ".pdf";
                        filename = Path.GetFileName(sname);
                        saveFileDialog1.FileName = filename;
                    }
                    while (saveFileDialog1.FileName.Contains(".pdf.pdf"))
                    {
                        saveFileDialog1.FileName = saveFileDialog1.FileName.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                        filename = saveFileDialog1.FileName;
                    }
                }
                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    PdfWriter.GetInstance(document, file);
                    document.Open();
                    Chunk chunk = new Chunk("REPORTE DE MANTENIMIENTO", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));

                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(v.tri));
                    //iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@"C:\Users\joseph\Pictures\Saved Pictures\logo.png");
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 705f /*600f, 500f*/);
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    document.Add(imagen);

                    document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("FECHA Y HORA DEL REPORTE", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
                    document.Add(new Paragraph(hoy, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL)));
                    document.Add(new Paragraph("                        "));

                    PdfPTable tb1 = new PdfPTable(6); //Inicia la primeratabla
                    tb1.DefaultCell.Border = 1;
                    tb1.WidthPercentage = 95;
                    tb1.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell cf0 = new PdfPCell(new Phrase("SUPERVISIÓN", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD))); //Celda 0, 1 y 0, 6
                    cf0.Colspan = 6;
                    cf0.Border = 0;
                    cf0.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb1.AddCell(cf0);

                    PdfPCell cf00 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda de espacio
                    cf00.Colspan = 6;
                    cf00.Border = 0;
                    cf00.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb1.AddCell(cf00);

                    PdfPCell cf01 = new PdfPCell(new Phrase("FOLIO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 1, 1 y 1, 2
                    cf01.Colspan = 2;
                    cf01.Border = 0;
                    cf01.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf01);

                    PdfPCell cf02 = new PdfPCell(new Phrase("UNIDAD", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 1, 3 y 1, 4
                    cf02.Colspan = 2;
                    cf02.Border = 0;
                    cf02.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf02);

                    PdfPCell cf03 = new PdfPCell(new Phrase("SUPERVISOR", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 1, 5 y 1, 6
                    cf03.Colspan = 2;
                    cf03.Border = 0;
                    cf03.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf03);

                    PdfPCell cf04 = new PdfPCell(new Phrase(foliopdf, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 2, 1 y 2, 2
                    cf04.Colspan = 2;
                    cf04.Border = 0;
                    cf04.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf04);

                    PdfPCell cf05 = new PdfPCell(new Phrase(eco, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 2, 3 y 2, 4
                    cf05.Colspan = 2;
                    cf05.Border = 0;
                    cf05.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf05);

                    PdfPCell cf06 = new PdfPCell(new Phrase(supervisor, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 2,5 y 2, 6
                    cf06.Colspan = 2;
                    cf06.Border = 0;
                    cf06.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf06);

                    PdfPCell cf07 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 3,1 y 3,6
                    cf07.Colspan = 6;
                    cf07.Border = 0;
                    cf07.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf07);

                    PdfPCell cf08 = new PdfPCell(new Phrase("FECHA DEL REPORTE", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 4, 1 y 4, 2
                    cf08.Colspan = 2;
                    cf08.Border = 0;
                    cf08.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf08);

                    PdfPCell cf09 = new PdfPCell(new Phrase("HORA DEL REPORTE", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 4, 3 y 4, 4
                    cf09.Colspan = 2;
                    cf09.Border = 0;
                    cf09.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf09);

                    PdfPCell cf10 = new PdfPCell(new Phrase("KILOMETRAJE DE REPORTE", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 4, 5 y 4, 6
                    cf10.Colspan = 2;
                    cf10.Border = 0;
                    cf10.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf10);

                    PdfPCell cf11 = new PdfPCell(new Phrase(fecha, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 5, 1 y 5, 2
                    cf11.Colspan = 2;
                    cf11.Border = 0;
                    cf11.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf11);

                    PdfPCell cf12 = new PdfPCell(new Phrase(hora, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 5,3 y 5, 4
                    cf12.Colspan = 2;
                    cf12.Border = 0;
                    cf12.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf12);

                    PdfPCell cf13 = new PdfPCell(new Phrase(km, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 5, 5 y 5, 6
                    cf13.Colspan = 2;
                    cf13.Border = 0;
                    cf13.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf13);

                    PdfPCell cf14 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 6,1 y 6,6
                    cf14.Colspan = 6;
                    cf14.Border = 0;
                    cf14.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf14);

                    PdfPCell cf15 = new PdfPCell(new Phrase("CÓDIGO DE FALLO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 7, 1 y 7, 2
                    cf15.Colspan = 2;
                    cf15.Border = 0;
                    cf15.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf15);

                    PdfPCell cf16 = new PdfPCell(new Phrase("DESCRIPCIÓN DE FALLO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 7, 3 y 7, 4
                    cf16.Colspan = 2;
                    cf16.Border = 0;
                    cf16.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf16);

                    PdfPCell cf17 = new PdfPCell(new Phrase("FALLA NO CODIFICADA", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 7, 5 y 7, 6
                    cf17.Colspan = 2;
                    cf17.Border = 0;
                    cf17.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf17);

                    PdfPCell cf18 = new PdfPCell(new Phrase(codfallo, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 8, 1 y 8, 2
                    cf18.Colspan = 2;
                    cf18.Border = 0;
                    cf18.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf18);

                    PdfPCell cf19 = new PdfPCell(new Phrase(desfallo, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 8, 3 y 8, 4
                    cf19.Colspan = 2;
                    cf19.Border = 0;
                    cf19.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf19);

                    PdfPCell cf20 = new PdfPCell(new Phrase(desfallonocod, FontFactory.GetFont(desfallonocod, 9, iTextSharp.text.Font.NORMAL))); //Celda 8, 5 y 8, 6
                    cf20.Colspan = 2;
                    cf20.Border = 0;
                    cf20.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf20);

                    PdfPCell cf21 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 9, 1 y 9, 6
                    cf21.Colspan = 6;
                    cf21.Border = 0;
                    cf21.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf21);

                    PdfPCell cf22 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 10, 1 y 10, 6
                    cf22.Colspan = 6;
                    cf22.Border = 0;
                    cf22.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf22);

                    PdfPCell cf23 = new PdfPCell(new Phrase(observacionessup, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 11, 1 y 11, 6
                    cf23.Colspan = 6;
                    cf23.Border = 0;
                    cf23.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf23);

                    PdfPCell cf24 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 12, 1 y 12, 6
                    cf24.Colspan = 6;
                    cf24.Border = 0;
                    cf24.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf24);

                    PdfPCell cf25 = new PdfPCell(new Phrase("MANTENIMIENTO", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD))); //Celda 13, 1 y 13, 6
                    cf25.Colspan = 6;
                    cf25.Border = 0;
                    cf25.HorizontalAlignment = Element.ALIGN_CENTER; ///////////////////////////////////////////////// AQUI COMIENZA MANTENIMIENTO
                    tb1.AddCell(cf25);

                    PdfPCell cf26 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 14, 1 y 14, 6
                    cf26.Colspan = 6;
                    cf26.Border = 0;
                    cf26.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf26);

                    PdfPCell cf27 = new PdfPCell(new Phrase("CLASIFICACIÓN DEL FALLO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 15, 1; 15, 2 y 15, 3
                    cf27.Colspan = 3;
                    cf27.Border = 0;
                    cf27.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf27);

                    PdfPCell cf28 = new PdfPCell(new Phrase("ESTATUS DEL MANTENIMIENTO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 15, 4 ; 15, 5 y 15, 6
                    cf28.Colspan = 3;
                    cf28.Border = 0;
                    cf28.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf28);

                    PdfPCell cf29 = new PdfPCell(new Phrase(fallogen, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 16, 1 ; 16, 2 y 16, 3
                    cf29.Colspan = 3;
                    cf29.Border = 0;
                    cf29.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf29);

                    PdfPCell cf30 = new PdfPCell(new Phrase(estatus, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 16, 4 ; 16, 5 y 16, 6
                    cf30.Colspan = 3;
                    cf30.Border = 0;
                    cf30.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf30);

                    PdfPCell cf31 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 17, 1 y 17, 6
                    cf31.Colspan = 6;
                    cf31.Border = 0;
                    cf31.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf31);

                    PdfPCell cf32 = new PdfPCell(new Phrase("MECÁNICO", FontFactory.GetFont("ARIAl", 9, iTextSharp.text.Font.BOLD))); //Celda 18, 1 y 18, 2
                    cf32.Colspan = 2;
                    cf32.Border = 0;
                    cf32.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf32);

                    PdfPCell cf33 = new PdfPCell(new Phrase("MECÁNICO DE APOYO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 18,3 y 18, 4
                    cf33.Colspan = 2;
                    cf33.Border = 0;
                    cf33.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf33);

                    PdfPCell cf34 = new PdfPCell(new Phrase("SUPERVISÓ", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 18, 5 y 18, 6
                    cf34.Colspan = 2;
                    cf34.Border = 0;
                    cf34.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf34);

                    PdfPCell cf35 = new PdfPCell(new Phrase(mecanico, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 19, 1 y 19, 2
                    cf35.Colspan = 2;
                    cf35.Border = 0;
                    cf35.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf35);

                    PdfPCell cf36 = new PdfPCell(new Phrase(apomecanico, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 19, 3 y 19, 4
                    cf36.Colspan = 2;
                    cf36.Border = 0;
                    cf36.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf36);

                    PdfPCell cf37 = new PdfPCell(new Phrase(superviso, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 19, 5 y 19, 6
                    cf37.Colspan = 2;
                    cf37.Border = 0;
                    cf37.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf37);

                    PdfPCell cf38 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 20, 1 y 20, 6
                    cf38.Colspan = 6;
                    cf38.Border = 0;
                    cf38.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf38);

                    PdfPCell cf41 = new PdfPCell(new Phrase("TIEMPO EN ESPERA", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 21, 3 y 21,4
                    cf41.Colspan = 2;
                    cf41.Border = 0;
                    cf41.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf41);

                    PdfPCell cf39 = new PdfPCell(new Phrase("HORA DE INICIO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 21, 1
                    cf39.Border = 0;
                    cf39.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf39);

                    PdfPCell cf40 = new PdfPCell(new Phrase("HORA DE TÉRMINO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 21, 2
                    cf40.Border = 0;
                    cf40.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf40);

                    PdfPCell cf42 = new PdfPCell(new Phrase("DIFERENCIA DE TIEMPO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 21, 5 y 21, 6
                    cf42.Colspan = 2;
                    cf42.Border = 0;
                    cf42.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf42);

                    PdfPCell cf45 = new PdfPCell(new Phrase(espera, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 22, 3 y 22, 4
                    cf45.Colspan = 2;
                    cf45.Border = 0;
                    cf45.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf45);

                    PdfPCell cf43 = new PdfPCell(new Phrase(inicio, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 22, 1
                    cf43.Border = 0;
                    cf43.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb1.AddCell(cf43);

                    PdfPCell cf44 = new PdfPCell(new Phrase(termino, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 22, 2
                    cf44.Border = 0;
                    cf44.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb1.AddCell(cf44);

                    PdfPCell cf46 = new PdfPCell(new Phrase(diferencia, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 22, 5 y 22, 6
                    cf46.Colspan = 2;
                    cf46.Border = 0;
                    cf46.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf46);

                    PdfPCell cf47 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 23, 1 y 23, 6
                    cf47.Colspan = 6;
                    cf47.Border = 0;
                    cf47.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf47);
               
                    PdfPCell cf48 = new PdfPCell(new Phrase("TRABAJO REALIZADO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 24, 1 ; 24, 2 y 24, 3
                    cf48.Colspan = 3;
                    cf48.Border = 0;
                    cf48.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf48);

                    PdfPCell cf49 = new PdfPCell(new Phrase("OBSERVACIONES DEL MANTENIMIENTO", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD))); //Celda 24, 4 ; 24, 5 y 24, 6
                    cf49.Colspan = 3;
                    cf49.Border = 0;
                    cf49.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf49);

                    PdfPCell cf50 = new PdfPCell(new Phrase(trabrealizado, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 25, 1 ; 25, 2 y 25, 3
                    cf50.Colspan = 3;
                    cf50.Border = 0;
                    cf50.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf50);

                    PdfPCell cf51 = new PdfPCell(new Phrase(observacionesmant, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))); //Celda 25, 4 ; 25, 5 y 25, 6
                    cf51.Colspan = 3;
                    cf51.Border = 0;
                    cf51.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf51);

                    PdfPCell cf52 = new PdfPCell(new Phrase("\n", FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD))); //Celda 26, 1 y 26, 6
                    cf52.Colspan = 6;
                    cf52.Border = 0;
                    cf52.HorizontalAlignment = Element.ALIGN_LEFT;
                    tb1.AddCell(cf52);

                    PdfPCell cf53 = new PdfPCell(new Phrase("REFACCIONES SOLICITADAS POR EL MECÁNICO", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD))); //Celda 27, 1 y 27, 6
                    cf53.Colspan = 6;
                    cf53.Border = 0;
                    cf53.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb1.AddCell(cf53);

                    document.Add(tb1);

                    document.Add(new Paragraph("                  "));

                    if (tbref == 1)
                    {
                        document.Add(new Paragraph("EL MECÁNICO NO SOLICITO NINGUNA REFACCIÓN", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                    }
                    else
                    {
                        GenerarDocumento(document);
                    }

                    document.AddCreationDate();
                    document.Close();
                    exportacionpdf2();
                    Process.Start(filename);
                }
            }
            catch
            {
                MessageBox.Show("Ha sucitado un error al exporta el PDF, favor de intentarlo de nuevo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void To_pdf() //Metodo Que Genera Todo El PDF
        {
            string unidad = "";
            string numtrans = "";
            string motor = "";
            string vin = "";
            string marca = "";
            string modelo = "";
            string trabajo = "";
            string fallo = "";
            string trabrealizado = "";
            string supervisor = "";
            string fechayhora = "";
            string estatus = "";
            string falloncod = "";
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT CONCAT(g2.identificador, LPAD(consecutivo, 4,'0')) FROM cunidades AS g1 INNER JOIN careas AS g2 ON g1.areafkcareas= g2.idarea WHERE g1.idunidad = t1.UnidadfkCUnidades), '') AS ECO, coalesce((SELECT r2.ntransmision FROM cunidades AS r2 WHERE r2.idunidad = t1.UnidadfkCUnidades), '') AS '# De Serie De Transmision', coalesce((SELECT r10.nmotor FROM cunidades AS r10 WHERE r10.idunidad = t1.UnidadfkCUnidades), '') AS '# De Motor', coalesce((SELECT r3.bin FROM cunidades AS r3 WHERE r3.idunidad = t1.UnidadfkCUnidades), '') AS Vin, coalesce((SELECT UPPER(r11.Marca) FROM cunidades AS r11 WHERE r11.idunidad = t1.UnidadfkCUnidades), '') AS Marca, coalesce((SELECT UPPER(r4.modelo) FROM cunidades AS r4 WHERE r4.idunidad = t1.UnidadfkCUnidades), '') AS Modelo, coalesce((SELECT UPPER(CONCAT(r5.ApPaterno, ' ', r5.ApMaterno, ' ', r5.nombres)) FROM reportemantenimiento AS t2 INNER JOIN cpersonal AS r5 ON t2.MecanicofkPersonal = r5.idPersona WHERE t1.idReporteSupervicion = t2.FoliofkSupervicion), '') AS 'Quien Realizo El Trabajo', coalesce((SELECT UPPER(r6.descfallo) FROM cdescfallo AS r6 WHERE t1.DescrFallofkcdescfallo = r6.iddescfallo), '') AS 'Descripcion De Fallo', coalesce((t1.DescFalloNoCod), '') AS 'Descripcion De Falla No Codificada', coalesce((SELECT UPPER(r8.TrabajoRealizado) FROM reportemantenimiento AS r8 WHERE r8.FoliofkSupervicion = t1.idReporteSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM cpersonal AS r7 WHERE t1.SupervisorfkCPersonal = r7.idPersona), '') AS Supervisor, coalesce((SELECT UPPER(CONCAT(DATE_FORMAT(r9.FechaReporteM, '%W %d %M %Y'), ' a las ', r9.HoraInicioM)) FROM reportemantenimiento AS r9 WHERE t1.idReporteSupervicion = r9.FoliofkSupervicion), '') AS 'Fecha Y Hora', coalesce((SELECT UPPER(r12.Estatus) FROM reportemantenimiento AS r12 WHERE r12.FoliofkSupervicion = t1.idReporteSupervicion), '') AS Estatus FROM reportesupervicion AS t1 WHERE t1.Folio = '" + labelFolio.Text + "'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                unidad = Convert.ToString(dr.GetString("ECO"));
                numtrans = Convert.ToString(dr.GetString("# De Serie De Transmision"));
                vin = Convert.ToString(dr.GetString("Vin"));
                motor = Convert.ToString(dr.GetString("# De Motor"));
                marca = Convert.ToString(dr.GetString("Marca"));
                modelo = Convert.ToString(dr.GetString("Modelo"));
                trabajo = Convert.ToString(dr.GetString("Quien Realizo El Trabajo"));
                trabrealizado = Convert.ToString(dr.GetString("Trabajo Realizado"));
                fallo = Convert.ToString(dr.GetString("Descripcion De Fallo"));
                falloncod = Convert.ToString(dr.GetString("Descripcion De Falla No Codificada"));
                supervisor = Convert.ToString(dr.GetString("Supervisor"));
                fechayhora = Convert.ToString(dr.GetString("Fecha Y Hora"));
                estatus = Convert.ToString(dr.GetString("Estatus"));
            }
            dr.Close();
           co.dbconection().Close();
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 80;
            table.LockedWidth = true;
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            name = "";
            sname = "";
            ext = "";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    name = Path.GetFileName(filename);
                    sname = Path.GetFileNameWithoutExtension(name);
                    ext = Path.GetExtension(name);
                    if(ext != ".pdf")
                    {
                        sname = sname + ".pdf";
                        filename =Path.GetFileName(sname);
                        saveFileDialog1.FileName = filename;
                    }
                    while (saveFileDialog1.FileName.Contains(".pdf.pdf"))
                    {
                        saveFileDialog1.FileName = saveFileDialog1.FileName.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                        filename = saveFileDialog1.FileName;
                    }
                }
                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    PdfWriter.GetInstance(doc, file);
                    doc.Open();
                    Chunk chunk = new Chunk("REPORTE DE MANTENIMIENTO", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));

                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(v.tri));
                    //iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@"C:\Users\joseph\Pictures\Saved Pictures\logo.png");
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 705f /*600f, 500f*/);
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    doc.Add(imagen);

                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("FECHA Y HORA DEL REPORTE: " , FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph(""+fechayhora.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL)));
                    doc.Add(new Paragraph("                       "));

                    PdfPTable tb1 = new PdfPTable(4); // INICIO DE LA PRIMERA TABLA
                    tb1.DefaultCell.Border = 1;
                    tb1.WidthPercentage = 95;
                    tb1.HorizontalAlignment = Left;

                    PdfPCell c11 = new PdfPCell(); //CELDA 1
                    c11.Border = 0;
                    Phrase Folio = new Phrase("UNIDAD", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lFolio = new Phrase( labelUnidad.Text, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c11.AddElement(Folio);
                    c11.AddElement(lFolio);

                    PdfPCell c21 = new PdfPCell(); //CELDA 2
                    c21.Border = 0;
                    Phrase Marca = new Phrase("MARCA", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lMarca = new Phrase( marca.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c21.AddElement(Marca);
                    c21.AddElement(lMarca);

                    PdfPCell c31 = new PdfPCell(); //CELDA 3
                    c31.Border = 0;
                    Phrase Modelo = new Phrase("MODELO", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lModelo = new Phrase( modelo.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c31.AddElement(Modelo);
                    c31.AddElement(lModelo);

                    PdfPCell c41 = new PdfPCell(); //CELDA 4
                    c41.Border = 0;
                    Phrase Estatus = new Phrase("ESTATUS", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lEstatus = new Phrase( estatus.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c41.AddElement(Estatus);
                    c41.AddElement(lEstatus);

                    tb1.AddCell(c11);
                    tb1.AddCell(c21);
                    tb1.AddCell(c31);
                    tb1.AddCell(c41);
                    doc.Add(tb1);

                    doc.Add(new Paragraph("                       "));
  
                    PdfPTable tb2 = new PdfPTable(3); // INICIO DE LA SEGUNDA TABLA
                    tb2.DefaultCell.Border = 1;
                    tb2.WidthPercentage = 95;
                    tb2.HorizontalAlignment = Left;

                    PdfPCell c12 = new PdfPCell(); //CELDA 1
                    c12.Border = 0;
                    Phrase NumTrans = new Phrase("NÚMERO DE TRANSMISIÓN", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lNumTrans = new Phrase( numtrans.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c12.AddElement(NumTrans);
                    c12.AddElement(lNumTrans);

                    PdfPCell c22 = new PdfPCell(); // CELDA 2
                    c22.Border = 0;
                    Phrase NumMotor = new Phrase("NÚMERO DE MOTOR", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lNumMotor = new Phrase(motor.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c22.AddElement(NumMotor);
                    c22.AddElement(lNumMotor);

                    PdfPCell c32 = new PdfPCell(); //CELDA 3
                    c32.Border = 0;
                    Phrase VIN = new Phrase("VIN", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lVIN = new Phrase(vin.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c32.AddElement(VIN);
                    c32.AddElement(lVIN);

                    tb2.AddCell(c12);
                    tb2.AddCell(c22);
                    tb2.AddCell(c32);
                    doc.Add(tb2);

                    doc.Add(new Paragraph("                       "));

                    PdfPTable tb3 = new PdfPTable(2); // INICIO DE LA TERCERA TABLA
                    tb3.DefaultCell.Border = 1;
                    tb3.WidthPercentage = 95;
                    tb3.HorizontalAlignment = Left;

                    PdfPCell c13 = new PdfPCell(); //CELDA 1
                    c13.Border = 0;
                    Phrase Mecanico = new Phrase("MECÁNICO", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lMecanico = new Phrase( trabajo.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c13.AddElement(Mecanico);
                    c13.AddElement(lMecanico);

                    PdfPCell c23 = new PdfPCell(); //CELDA 2
                    c23.Border = 0;
                    Phrase Supervisor = new Phrase("SUPERVISOR", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lSupervisor = new Phrase(supervisor.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c23.AddElement(Supervisor);
                    c23.AddElement(lSupervisor);

                    tb3.AddCell(c13);
                    tb3.AddCell(c23);
                    doc.Add(tb3);

                    doc.Add(new Paragraph("                       "));

                    PdfPTable tb4 = new PdfPTable(2); // INICIO DE LA CUARTA TABLA
                    tb4.DefaultCell.Border = 1;
                    tb4.WidthPercentage = 95;
                    tb4.HorizontalAlignment = Left;

                    PdfPCell c14 = new PdfPCell(); //CELDA 1
                    c14.Border = 0;
                    if(fallo.ToString().Equals(""))
                    {
                        Phrase Fallo = new Phrase("DESCRIPCIÓN DEL FALLO NO CODIFICADO", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        Phrase lFallo = new Phrase(falloncod.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                        c14.AddElement(Fallo);
                        c14.AddElement(lFallo);
                    }
                    else
                    {
                        Phrase Fallo = new Phrase("DESCRIPCIÓN DEL FALLO", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        Phrase lFallo = new Phrase(fallo.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                        c14.AddElement(Fallo);
                        c14.AddElement(lFallo);
                    }

                    PdfPCell c24 = new PdfPCell(); //CELDA 2
                    c24.Border = 0;
                    Phrase TRealizado = new Phrase("TRABAJO REALIZADO", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    Phrase lTRealizado = new Phrase(trabrealizado.ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL));
                    c24.AddElement(TRealizado);
                    c24.AddElement(lTRealizado);

                    tb4.AddCell(c14);
                    tb4.AddCell(c24);
                    doc.Add(tb4);

                    doc.Add(new Paragraph("                       "));
                    doc.Add(new Paragraph("REFACCIONES SOLICITADAS          ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("                       "));
                    if (tbref == 1)
                    {
                        doc.Add(new Paragraph("EL MECÁNICO NO SOLICITO NINGUNA REFACCIÓN", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                    }
                    else
                    {
                        GenerarDocumento(doc);
                    }

                    doc.AddCreationDate();
                    doc.Close();
                    exportacionpdf1();
                    Process.Start(filename);
                }
            }
            catch
            {
                MessageBox.Show("Ha sucitado un error al exporta el PDF, favor de intentarlo de nuevo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GenerarDocumento(Document document) //Genera El Documento
        {
            int i, j;
            PdfPTable datatable = new PdfPTable(dataGridViewMRefaccion.ColumnCount);
            datatable.DefaultCell.Padding = 3;
            float[] headerwidths = GetTamañoColumnas(dataGridViewMRefaccion);
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 100;
            datatable.DefaultCell.BorderWidth = 2;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(/*245, 222, 213*/234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dataGridViewMRefaccion.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(dataGridViewMRefaccion.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dataGridViewMRefaccion.Rows.Count; i++)
            {
                for (j = 0; j < dataGridViewMRefaccion.Columns.Count; j++)
                {
                    if (dataGridViewMRefaccion[j, i].Value != null)
                    {
                        datatable.AddCell(new Phrase(dataGridViewMRefaccion[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 7)));
                    }
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }

        public float[] GetTamañoColumnas(DataGridView dg) //Metodo Del Tamaño De La Tabla PDF
        {
            float[] values = new float[dg.ColumnCount];
            for (int i = 0; i < 8; i++)
            {
                values[i] = (float)dg.Columns[i].Width;
            }
            return values;
        }

        Thread hiloEx;

        delegate void Loading();

        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            buttonActualizar.Visible = false;
            label26.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Location = new Point(1527, 599); 
            groupBoxBusqueda.Enabled = false;
            dataGridViewMantenimiento.Enabled = false;
        }

        delegate void Loading1();

        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            buttonExcel.Visible = true;
            buttonActualizar.Visible = true;
            label26.Visible = true;
            label35.Text = "EXPORTAR";
            label35.Location = new Point(1551, 599); 
            groupBoxBusqueda.Enabled = true;
            dataGridViewMantenimiento.Enabled = true;
        }

        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            if (dataGridViewMantenimiento.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    Loading load = new Loading(cargando1);
                    this.Invoke(load);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                int ColumnIndex = 0;
                foreach (DataGridViewColumn col in dataGridViewMantenimiento.Columns)
                {
                    ColumnIndex++;
                    X.Cells[1, ColumnIndex] = col.HeaderText;
                    X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    X.Cells[1, ColumnIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Crimson);
                    X.Cells[1, ColumnIndex].Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                    X.Cells[1, ColumnIndex].Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                }

                for (int i = 0; i <= dataGridViewMantenimiento.RowCount - 1; i++)
                {
                    for (int j = 0; j <= dataGridViewMantenimiento.ColumnCount - 1; j++)
                    {
                        if (dataGridViewMantenimiento.Columns[j].Visible == true)
                        {
                            try
                            {
                                h.Worksheet sheet = X.ActiveSheet;
                                h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                                sheet.Cells[i + 2, j + 1] = dataGridViewMantenimiento[j, i].Value.ToString();
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(200, 200, 200));
                                rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);

                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "EN PROCESO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "LIBERADA".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "REPROGRAMADA".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }      
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "CORRECTIVO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "PREVENTIVO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "REITERATIVO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "REPROGRAMADO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                                if (dataGridViewMantenimiento[j, i].Value.ToString() == "SEGUIMIENTO".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(246, 106, 77));
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                            }
                            catch (System.NullReferenceException)
                            {

                            }   
                        }
                    }
                }
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;

                Thread.Sleep(500);

                exportacionexcel();

                if (this.InvokeRequired)
                {
                    Loading1 load1 = new Loading1(cargando2);
                    this.Invoke(load1);
                }           
            }
            else
            {
                MessageBox.Show("Es necesario que existan datos en la tabla para poder generar un archivo de excel \nFavor de actualizar la tabla para que se visualizen los reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        /* Acciones con los botones y gridview *////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAgregarMasPed_Click(object sender, EventArgs e) //Regresa Para Meter Mas Refacciones
        {
            buttonAgregaPed.Visible = true;
            label33.Visible = true;
            buttonActualizar.Visible = true;
            label26.Visible = true;
            buttonActualizarPed.Visible = false;
            label3.Visible = false;
            buttonAgregarMasPed.Visible = false;
            label29.Visible = false;
            metodocargaref();
            limpiarrefacc();
        }

        private void buttonAgregar_Click(object sender, EventArgs e) //Manda A La Ventana De Refacciones
        {
            groupBoxRefacciones.Visible = true;
            privilegios();
            sch = 0; 
            if(dontr == 0)
            {
                inicolumn = 0; 
                inicolumn = dataGridViewMRefaccion.Rows.Count; 
                x = 1; 
            }
            label1.Visible = true;
            groupBoxMantenimiento.Visible = false;
            buttonGuardar.Visible = false;
            label24.Visible = false;
            buttonAgregaPed.Visible = true;
            label33.Visible = true;
            buttonActualizarPed.Visible = false;
            label3.Visible = false;
            buttonAgregarMasPed.Visible = false;
            label29.Visible = false;
            dataGridViewMRefaccion.Visible = true;
            if (sch == 0)
            {
                sch = 1;
                label62.Visible = true;
                label63.Visible = true;
            }
            else
            {
                sch = 0;
                label62.Visible = false;
                label63.Visible = false;
            } 
            metodocargaref();
            conteoiniref();
            conteofinref();
        }

        private void buttonExcel_Click(object sender, EventArgs e) //Genera Un Documento De Excel
        {
            if(labelFolio.Text.Equals(""))
            {
                ThreadStart excel = new ThreadStart(exporta_a_excel);
                hiloEx = new Thread(excel);
                hiloEx.Start();
            }
        }

        private void buttonFinalizar_Click(object sender, EventArgs e) //Finalizar Matenimiento
        {
            int cfin = 0;
            if(cfin == 0)
            {
                cfin = cfin + 1;
                cargo = 2;
                if (comboBoxEstatusMant.Text.Equals("LIBERADA"))
                {
                    if (labelEstatusMan.Text.Equals("REPROGRAMADA"))
                    {
                        MessageBox.Show("La unidad no puede ser liberada porque primero debe pasar por un proceso antes de terminar el reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelHoraTerminoM.Text = "";
                        textBoxTerminoMan.Text = "";
                        validar();
                    }
                    else if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                    {
                        if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- ESTATUS --")))
                        {
                            MessageBox.Show("El reporte no puede ser finalizado porque el campo de 'Existencia De Refacciones' contiene una opcion no valida, cambie la opción para continuar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                        }
                        else if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                        {
                            if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                            {
                                MessageBox.Show("El Folio de Factura no puede quedar vacio si está validada la Existencia de Refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                            }
                            else if ((textBoxFolioFactura.Text.Equals("0")) || (textBoxFolioFactura.Text.Equals("00")) || (textBoxFolioFactura.Text.Equals("000")) || (textBoxFolioFactura.Text.Equals("0000")) || (textBoxFolioFactura.Text.Equals("00000")) || (textBoxFolioFactura.Text.Equals("000000")) || (textBoxFolioFactura.Text.Equals("0000000")) || (textBoxFolioFactura.Text.Equals("00000000")) || (textBoxFolioFactura.Text.Equals("000000000")))
                            {
                                MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                            }
                            else
                            {
                                MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'",co.dbconection());
                                MySqlDataReader dr01 = cmd01.ExecuteReader();
                                if (dr01.Read())
                                {
                                    facturar = Convert.ToString(dr01.GetString("FolioFactura"));
                                }
                                if ((facturar == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == true))
                                {
                                    MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    validar();
                                }
                                else
                                {
                                    fquestion = 0;
                                    metodobtnfinalizarcref();
                                    if (fquestion == 1)
                                    {
                                        comboBoxFalloGral.Enabled = false;
                                        textBoxMecanico.Enabled = false;
                                        textBoxMecanicoApo.Enabled = false;
                                        textBoxFolioFactura.Enabled = false;
                                        textBoxTrabajoRealizado.Enabled = false;
                                        comboBoxEstatusMant.Enabled = false;
                                        comboBoxExisRefacc.Enabled = false;
                                        comboBoxReqRefacc.Enabled = false;
                                        textBoxSuperviso.Enabled = false;
                                        textBoxObsMan.Enabled = false;
                                        buttonPDF.Visible = false;
                                        label36.Visible = false;
                                        radioButtonGeneral.Visible = false;
                                        radioButtonUnidad.Visible = false;
                                        if (pconsultar == true)
                                        {
                                            buttonExcel.Visible = true;
                                            label35.Visible = true;
                                        }
                                        else
                                        {
                                            buttonExcel.Visible = false;
                                            label35.Visible = false;
                                        }
                                        buttonGuardar.Visible = false;
                                        label24.Visible = false;
                                        buttonFinalizar.Visible = false;
                                        label37.Visible = false;
                                        buttonAgregar.Visible = false;
                                        label39.Visible = false;
                                    }
                                }
                            }
                        }
                    }

                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                    {
                        fquestion = 0;
                        metodobtnfinalizarcref();
                        if (fquestion == 1)
                        {
                            comboBoxFalloGral.Enabled = false;
                            textBoxMecanico.Enabled = false;
                            textBoxMecanicoApo.Enabled = false;
                            textBoxFolioFactura.Enabled = false;
                            textBoxTrabajoRealizado.Enabled = false;
                            comboBoxEstatusMant.Enabled = false;
                            comboBoxExisRefacc.Enabled = false;
                            comboBoxReqRefacc.Enabled = false;
                            textBoxSuperviso.Enabled = false;
                            textBoxObsMan.Enabled = false;
                            buttonPDF.Visible = false;
                            label36.Visible = false;
                            radioButtonGeneral.Visible = false;
                            radioButtonUnidad.Visible = false;
                            if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                            {
                                buttonExcel.Visible = true;
                                label35.Visible = true;
                            }
                            else
                            {
                                buttonExcel.Visible = false;
                                label35.Visible = false;
                            }
                            buttonGuardar.Visible = false;
                            label24.Visible = false;
                            buttonFinalizar.Visible = false;
                            label37.Visible = false;
                            buttonAgregar.Visible = false;
                            label39.Visible = false;
                            cfin = 0;
                        }
                    }
                }
            }
            else
            {}
        }

        private void buttonGuardar_Click(object sender, EventArgs e) // Guardar
        {
            privilegios();

            if (comboBoxFalloGral.Text.Equals("-- FALLO GENERAL --"))
            {
                MessageBox.Show("Seleccione una Clasificación del Fallo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((textBoxMecanico.Text != "") && (labelNomMecanico.Text.Equals(".")))
            {
                MessageBox.Show("Contraseña del Mecánico Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanico.Text = "";
            }
            else if ((textBoxMecanicoApo.Text != "") && (labelNomMecanicoApo.Text.Equals("..")))
            {
                MessageBox.Show("Contraseña del Mecánico de Apoyo Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanicoApo.Text = "";
            }
            else if ((textBoxSuperviso.Text != "") && (labelNomSuperviso.Text.Equals("...")))
            {
                MessageBox.Show("Contraseña de la Persona que Supervisó el Mantenimiento Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxSuperviso.Text = "";
            }
            else if (labelNomMecanico.Text == ".")
            {
                MessageBox.Show("Ingrese la Contraseña del Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((labelNomMecanico.Text == ".") && (labelNomMecanicoApo.Text != ".."))
            {
                MessageBox.Show("El Mecánico de Apoyo no puede ser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((labelNomMecanico.Text == ".") && (labelNomSuperviso.Text != "..."))
            {
                MessageBox.Show("El Supevisor no puedeser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (x == 1)
                {
                    fincolumn = dataGridViewMRefaccion.Rows.Count;
                    x = 0;
                }
                if ((cdf.Equals(comboBoxFalloGral.Text)) && (mec.Equals(labelNomMecanico.Text)) && (mecapo.Equals(labelNomMecanicoApo.Text)) && (exiref.Equals(comboBoxExisRefacc.Text)) && (reqref.Equals(comboBoxReqRefacc.Text)) && (trabreak.Equals(textBoxTrabajoRealizado.Text)) && (folfac.Equals(textBoxFolioFactura.Text)) && (estmant.Equals(comboBoxEstatusMant.Text)) && (supmant.Equals(labelNomSuperviso.Text)) && (obsmant.Equals(textBoxObsMan.Text)) && (inicolumn == fincolumn))
                {
                    conteo();
                    metodoCarga();
                    limpiarcampos();
                    limpiarstring();
                    ncontreffin();
                    inicolumn = 0;
                    fincolumn = 0;
                    dontr = 0;

                    dataGridViewMantenimiento.Refresh();
                    comboBoxFalloGral.Enabled = false;
                    textBoxMecanico.Enabled = false;
                    textBoxMecanicoApo.Enabled = false;
                    textBoxFolioFactura.Enabled = false;
                    textBoxTrabajoRealizado.Enabled = false;
                    comboBoxEstatusMant.Enabled = false;
                    comboBoxExisRefacc.Enabled = false;
                    comboBoxReqRefacc.Enabled = false;
                    textBoxSuperviso.Enabled = false;
                    textBoxObsMan.Enabled = false;
                    buttonPDF.Visible = false;
                    label36.Visible = false;
                    radioButtonGeneral.Visible = false;
                    radioButtonUnidad.Visible = false;
                    if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                    {
                        buttonExcel.Visible = true;
                        label35.Visible = true;
                    }
                    else
                    {
                        buttonExcel.Visible = false;
                        label35.Visible = false;
                    }
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    timer1.Start();

                    MessageBox.Show("No se realizó ningun cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxEstatusMant.SelectedIndex == 0)
                {
                    MessageBox.Show("Seleccione un estatus del mantenimiento", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if ((comboBoxEstatusMant.Text == "EN PROCESO") || (comboBoxEstatusMant.Text == "REPROGRAMADA"))
                {
                    if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (labelEstatusMan.Text != "EN PROCESO"))
                    {
                        labelHoraInicioM.Text = "";
                        textBoxEsperaMan.Text = "";
                    }
                    if (comboBoxReqRefacc.SelectedIndex == 1 && !metodotxtchref())
                    {
                        if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                        {
                            if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))))
                            {
                                metodobtnguardar();
                            }
                            else if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0)))
                            {
                                MessageBox.Show("El campo de 'Existencia De Refacciones' no debe quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if ((textBoxFolioFactura.Text != "") && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))) && (textBoxFolioFactura.Enabled == true))
                            {
                                MessageBox.Show("El Folio de Factura debe quedar en blanco si el apartado 'Existencia De Refacciones' está en espera de las refacciones o no hay existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textBoxFolioFactura.Text = "";
                            }
                            else if ((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (textBoxFolioFactura.Enabled == false)))
                            {
                                if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                                {
                                    MessageBox.Show("El Folio de Factura no puede quedar vacío si hay existencias en las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if ((textBoxFolioFactura.Text.Equals("0")) || (textBoxFolioFactura.Text.Equals("00")) || (textBoxFolioFactura.Text.Equals("000")) || (textBoxFolioFactura.Text.Equals("0000")) || (textBoxFolioFactura.Text.Equals("00000")) || (textBoxFolioFactura.Text.Equals("000000")) || (textBoxFolioFactura.Text.Equals("0000000")) || (textBoxFolioFactura.Text.Equals("00000000")) || (textBoxFolioFactura.Text.Equals("000000000")))
                                {
                                    MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    textBoxFolioFactura.Focus();
                                }
                                else
                                {
                                    MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'",co.dbconection());
                                    MySqlDataReader dr01 = cmd01.ExecuteReader();
                                    if (dr01.Read())
                                    {
                                        facturar = Convert.ToString(dr01.GetString("FolioFactura"));
                                    }

                                    if ((facturar == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == true))
                                    {
                                        MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        textBoxFolioFactura.Text = "";
                                    }
                                    else if ((facturar == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == false))
                                    {
                                        metodobtnguardar();

                                    }
                                    else
                                    {
                                        metodobtnguardar();
                                    }
                                }
                            }
                        }
                    }
                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                    {
                        metodobtnguardar();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione una opcion en " + "'SE REQUIEREN REFACCIONES'", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        comboBoxReqRefacc.Enabled = true;
                    }
                }
                else if (comboBoxEstatusMant.Enabled == false)
                {
                    metodobtnguardar();
                }
            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxTrabajoRealizado.Enabled &&   string.IsNullOrWhiteSpace(trabreak))
                {
                    MessageBox.Show("No puede dejar en blanco el trabajo realizado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if ((cdf != "") && (comboBoxFalloGral.SelectedIndex == 0))
                {
                    MessageBox.Show("No puede dejar en blanco la falla general", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if ((folfac != "") && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                {
                    MessageBox.Show("No puede dejar en blanco el folio de factura en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if( !string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) && Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                {
                    MessageBox.Show("El folio de factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!((cdf.Equals(comboBoxFalloGral.Text)) && (trabreak.Equals(textBoxTrabajoRealizado.Text)) && (folfac.Equals(textBoxFolioFactura.Text)) && (obsmant.Equals(textBoxObsMan.Text))))
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'",co.dbconection());
                    cmd.ExecuteNonQuery();
                   co.dbconection().Close();

                    MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', (SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'), CONCAT('" + comboBoxFalloGral.SelectedValue + ";', '" + textBoxFolioFactura.Text + ";', '" + textBoxTrabajoRealizado.Text + ";', '" + textBoxObsMan.Text + "'), '" + idUsuario + "', now(), 'Actualización de Reporte de Mantenimiento', '2', '1')",co.dbconection());
                    cmd0.ExecuteNonQuery();
                   co.dbconection().Close();

                    metodoCarga();
                    limpiarcampos();
                    limpiarstring();
                    crefacc = 0;
                    inicolumn = 0;
                    fincolumn = 0;
                    ncontreffin();
                    conteo();
                    MessageBox.Show("Reporte Editado Correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridViewMantenimiento.Refresh();
                    if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                    {
                        buttonExcel.Visible = true;
                        label35.Visible = true;
                    }
                    else
                    {
                        buttonExcel.Visible = false;
                        label35.Visible = false;
                    }
                    buttonPDF.Visible = false;
                    label36.Visible = false;
                    radioButtonGeneral.Visible = false;
                    radioButtonUnidad.Visible = false;
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                    comboBoxFalloGral.Enabled = false;
                    textBoxMecanico.Enabled = false;
                    textBoxMecanicoApo.Enabled = false;
                    textBoxFolioFactura.Enabled = false;
                    textBoxTrabajoRealizado.Enabled = false;
                    comboBoxEstatusMant.Enabled = false;
                    comboBoxExisRefacc.Enabled = false;
                    comboBoxReqRefacc.Enabled = false;
                    textBoxSuperviso.Enabled = false;
                    textBoxObsMan.Enabled = false;
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    timer1.Start();
                }
                else
                {
                    metodoCarga();
                    limpiarcampos();
                    limpiarstring();
                    crefacc = 0;
                    inicolumn = 0;
                    fincolumn = 0;
                    ncontreffin();
                    conteo();
                    MessageBox.Show("Sin Modificaciones", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridViewMantenimiento.Refresh();
                    if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                    {
                        buttonExcel.Visible = true;
                        label35.Visible = true;
                    }
                    else
                    {
                        buttonExcel.Visible = false;
                        label35.Visible = false;
                    }
                    buttonPDF.Visible = false;
                    label36.Visible = false;
                    radioButtonGeneral.Visible = false;
                    radioButtonUnidad.Visible = false;
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                    comboBoxFalloGral.Enabled = false;
                    textBoxMecanico.Enabled = false;
                    textBoxMecanicoApo.Enabled = false;
                    textBoxFolioFactura.Enabled = false;
                    textBoxTrabajoRealizado.Enabled = false;
                    comboBoxEstatusMant.Enabled = false;
                    comboBoxExisRefacc.Enabled = false;
                    comboBoxReqRefacc.Enabled = false;
                    textBoxSuperviso.Enabled = false;
                    textBoxObsMan.Enabled = false;
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                    timer1.Start();
                }
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.ToString(),validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void dataGridViewMantenimiento_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Mantenimiento
        {
            mensaje = false;
            if (e.RowIndex >= 0) 
            {
                if (string.IsNullOrWhiteSpace(mec))
                {
                    mec = ".";
                }

                if (string.IsNullOrWhiteSpace(mecapo))
                {
                    mecapo = "..";
                }

                if (string.IsNullOrWhiteSpace(supmant))
                {
                    supmant = "...";
                }

                if (x == 1)
                {
                    fincolumn = dataGridViewMRefaccion.Rows.Count;
                    x = 0;
                }

                string fg = "", rq = "", et = "", ex = "", rr = "";
                if (comboBoxFalloGral.Text.Equals("-- FALLO GENERAL --"))
                {
                    fg = "";
                }
                else
                {
                    fg = comboBoxFalloGral.Text;
                }

                if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                {
                    rq = "";
                }
                else
                {
                    rq = comboBoxReqRefacc.Text;
                }

                if (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --"))
                {
                    ex = "";
                }
                else
                {
                    ex = comboBoxExisRefacc.Text;
                }

                if (comboBoxEstatusMant.Text.Equals("-- ESTATUS --"))
                {
                    et = "";
                }
                else
                {
                    et = comboBoxEstatusMant.Text;
                }

                if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                {
                    rr = "";
                }
                else
                {
                    rr = comboBoxReqRefacc.Text;
                }

                if ((cdf.Equals(fg)) && (mec.Equals(labelNomMecanico.Text)) && (mecapo.Equals(labelNomMecanicoApo.Text)) && (exiref.Equals(ex)) && (reqref.Equals(rr)) && (trabreak.Equals(textBoxTrabajoRealizado.Text)) && (folfac.Equals(textBoxFolioFactura.Text)) && ((estmant.Equals(et)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supmant.Equals(labelNomSuperviso.Text)) && (obsmant.Equals(textBoxObsMan.Text)) && ((inicolumn == 0) || (inicolumn == fincolumn)))
                {
                    limpiarstring();
                    inicolumn = 0;
                    fincolumn = 0;
                    dontr = 0;
                    llamadadatos();

                    ncontrefini();
                }
                else
                {
                    int total, cant;

                    total = fincolumn - inicolumn;
                    if (MessageBox.Show("Si usted cambia de reporte y/o actualiza la tabla se perderan los datos ingresados\n\n ¿Esta seguro de querer continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "",co.dbconection());
                        cmd1.ExecuteNonQuery();
                       co.dbconection().Close();
                        inicolumn = 0;
                        fincolumn = 0;
                        dontr = 0;
                        limpiarstring();
                        metodocargaref();
                        llamadadatos();
                    }
                }
            }    
        }

        private void dataGridViewMRefaccion_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Refacciones
        {
            if(sch == 1)
            {
                exis = dataGridViewMRefaccion.CurrentRow.Cells["ESTATUS DE LA REFACCION"].Value.ToString();
                if ((exis == "SIN EXISTENCIA") || (exis == "EXISTENCIA"))
                {
                    MessageBox.Show("La Refacción ya fue validada por almacen, esta refacción ya no se puede editar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    buttonActualizar.Visible = false;
                    label26.Visible = false;
                    buttonAgregaPed.Visible = false;
                    label33.Visible = false;
                    buttonAgregarMasPed.Visible = true;
                    label29.Visible = true;
                    buttonActualizarPed.Visible = true;
                    label3.Visible = true;

                    fam = "";
                    reff = "";

                    conta = dataGridViewMRefaccion.CurrentRow.Cells["PARTIDA"].Value.ToString();

                    MySqlCommand familia = new MySqlCommand("select upper(t1.familia) as familia, t1.idfamilia from cfamilias as t1 inner join crefacciones as t2 on t2.familiafkcfamilias=t1.idfamilia where upper(t2.nombreRefaccion)='" + dataGridViewMRefaccion.CurrentRow.Cells[1].Value.ToString() + "' and t1.status='0'",co.dbconection());
                    MySqlDataReader dtr = familia.ExecuteReader();
                    if (dtr.Read())
                    {
                        comboBoxFamilia.DataSource = null;
                        DataTable dt = new DataTable();
                        MySqlCommand cmd2 = new MySqlCommand("SELECT UPPER(familia) AS familia, idfamilia FROM cfamilias WHERE status = '1' ORDER BY familia",co.dbconection());
                        MySqlDataAdapter adap = new MySqlDataAdapter(cmd2);
                        adap.Fill(dt);
                        DataRow row2 = dt.NewRow();
                        DataRow row3 = dt.NewRow();
                        row2["idfamilia"] = 0;
                        row2["familia"] = " -- FAMILIA --";
                        row3["idfamilia"] = dtr["idfamilia"];
                        row3["familia"] = dtr["familia"].ToString();
                        dt.Rows.InsertAt(row2, 0);
                        dt.Rows.InsertAt(row3, 1);
                        comboBoxFamilia.ValueMember = "idfamilia";
                        comboBoxFamilia.DisplayMember = "familia";
                        comboBoxFamilia.DataSource = dt;
                        comboBoxFamilia.SelectedIndex = 0;
                        comboBoxFamilia.Text = dtr["familia"].ToString();
                    }

                    MySqlCommand cmd = new MySqlCommand("SELECT t1.idPedRef, t3.idfamilia, UPPER(t3.familia) AS Familia, t1.Cantidad, UPPER(t2.nombreRefaccion) AS Refaccion, t1.RefaccionfkCRefaccion AS idRefaccion FROM pedidosrefaccion AS t1 INNER JOIN crefacciones AS t2 ON t1.RefaccionfkCRefaccion = t2.idrefaccion INNER JOIN cfamilias AS t3 ON t2.familiafkcfamilias = t3.idfamilia WHERE t1.NumRefacc = '" + conta + "' AND t1.FolioPedfkSupervicion ='" + labelidRepSup.Text + "'",co.dbconection());
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        foliof = Convert.ToString(dr.GetString("idPedRef"));
                        comboBoxFamilia.Text = Convert.ToString(dr.GetString("Familia"));
                        fam = Convert.ToString(dr.GetString("Familia"));
                        fam1 = Convert.ToString(dr.GetString("idFamilia"));

                        comboBoxFRefaccion.Text = Convert.ToString(dr.GetString("Refaccion"));
                        reff = comboBoxFRefaccion.Text;
                        reff1 = Convert.ToString(dr.GetString("idRefaccion"));
                        textBoxCantidad.Text = Convert.ToString(dr.GetString("Cantidad"));
                        cantd = Convert.ToString(dr.GetString("Cantidad"));
                    }

                    comboBoxFRefaccion.Text = dataGridViewMRefaccion.CurrentRow.Cells[1].Value.ToString();
                    dr.Close();
                   co.dbconection().Close();
                }
            }
        }

        private void buttonActualizarPed_Click(object sender, EventArgs e) //Actualizar Pedido
        {
            if ((comboBoxFamilia.SelectedIndex == 0) || (comboBoxFRefaccion.SelectedIndex == 0) || (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
            {
                MessageBox.Show("Alguno de los campos esta vacío", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
            {
                MessageBox.Show("El campo debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((textBoxCantidad.Text == "0") || (textBoxCantidad.Text == "00") || (textBoxCantidad.Text == "000") || (textBoxCantidad.Text == ".00") || (textBoxCantidad.Text == "0.0") || (textBoxCantidad.Text == "00.") || (textBoxCantidad.Text == "0/0") || (textBoxCantidad.Text == "/00") || (textBoxCantidad.Text == "00/") || (textBoxCantidad.Text == ".") || (textBoxCantidad.Text == "/"))
            {
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cant = Convert.ToDouble(textBoxCantidad.Text);
                if ((comboBoxFamilia.Text.Equals(fam)) && (comboBoxFRefaccion.Text.Equals(reff)) && (textBoxCantidad.Text.Equals(cantd)))
                {
                    MessageBox.Show("No se realizó ningún cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE pedidosrefaccion SET RefaccionfkCRefaccion = '" + comboBoxFRefaccion.SelectedValue + "', Cantidad = '" + cant + "' WHERE NumRefacc = '" + conta + "'",co.dbconection());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Refacción actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   co.dbconection().Close();
                    MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', (SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "'), CONCAT('" + comboBoxFRefaccion.SelectedValue + ";', '" + textBoxCantidad.Text + "'), '"+idUsuario+"', now(), 'Actualización de Refacción en Reporte de Mantenimiento', '2', '1')",co.dbconection());
                    cmd0.ExecuteNonQuery();
                   co.dbconection().Close();
                }
                metodocargaref();
                limpiarrefacc();
                buttonActualizarPed.Visible = false;
                label3.Visible = false;
                buttonAgregarMasPed.Visible = false;
                label29.Visible = false;
                buttonAgregaPed.Visible = true;
                label33.Visible = true;
                buttonActualizar.Visible = true;
                label26.Visible = true;
            } 
        }

        private void buttonActualizar_Click(object sender, EventArgs e)  //Actualizar
        {
            privilegios();

            if (string.IsNullOrWhiteSpace(mec))
            {
                mec = ".";
            }

            if (string.IsNullOrWhiteSpace(mecapo))
            {
                mecapo = "..";
            }

            if (string.IsNullOrWhiteSpace(supmant))
            {
                supmant = "...";
            }

            if (x == 1)
            {
                fincolumn = dataGridViewMRefaccion.Rows.Count;
                x = 0;
            }

            string fg = "", rq = "", et = "", ex = "", rr = "";
            if (comboBoxFalloGral.Text.Equals("-- FALLO GENERAL --"))
            {
                fg = "";
            }
            else
            {
                fg = comboBoxFalloGral.Text;
                if(bedit == true)
                {
                    fg = Convert.ToString(comboBoxFalloGral.SelectedValue);
                }
            }

            if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
            {
                rq = "";
            }
            else
            {
                rq = comboBoxReqRefacc.Text;
            }

            if (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --"))
            {
                ex = "";
            }
            else
            {
                ex = comboBoxExisRefacc.Text;
            }

            if (comboBoxEstatusMant.Text.Equals("-- ESTATUS --"))
            {
                et = "";
            }
            else
            {
                et = comboBoxEstatusMant.Text;
            }

            if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
            {
                rr = "";
            }
            else
            {
                rr = comboBoxReqRefacc.Text;
            }

            if(bedit == true)
            {
                if ((((idfg == fg) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabreak == textBoxTrabajoRealizado.Text) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))) && ((folfac == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))) && ((obsmant == textBoxObsMan.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObsMan.Text)))))
                {
                    botonactualizar();
                }
                else
                {
                    if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        botonactualizar();
                    }
                }
            }
            else
            {
                if ((cdf.Equals(fg)) && (mec.Equals(labelNomMecanico.Text)) && (mecapo.Equals(labelNomMecanicoApo.Text)) && (exiref.Equals(ex)) && (reqref.Equals(rr)) && (trabreak.Equals(textBoxTrabajoRealizado.Text)) && (folfac.Equals(textBoxFolioFactura.Text)) && (estmant.Equals(et)) && (supmant.Equals(labelNomSuperviso.Text)) && (obsmant.Equals(textBoxObsMan.Text)) && ((inicolumn == fincolumn) || (inicolumn == 0)))
                {
                    botonactualizar();
                }
                else
                {
                    int total;

                    total = fincolumn - inicolumn;
                    if (MessageBox.Show("Si usted cambia de reporte y/o actualiza la tabla se perderan los datos ingresados al formulario\n\n ¿Esta seguro de querer continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "",co.dbconection());
                        cmd1.ExecuteNonQuery();
                       co.dbconection().Close();
                        botonactualizar();
                    }
                }
            }
        }

        private void buttonPDF_Click(object sender, EventArgs e) //Generar PDF
        {
            string comporacion = "";
            if((radioButtonGeneral.Checked == true) && (radioButtonUnidad.Checked == false))
            {
                comporacion = "el reporte de fallo del mantenimiento en PDF?";
            }
            else
            {
                comporacion = "los datos de la unidad del mantenimiento en PDF?";
            }
            if((radioButtonGeneral.Checked == false) && (radioButtonUnidad.Checked == false))
            {
                MessageBox.Show("Favor de seleccionar entre Unidad y General", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
            else
            {
                if ((MessageBox.Show("¿Desea generar " +comporacion, "INFORMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
                {
                    if(dataGridViewMRefaccion.Rows.Count == 0)
                    {
                        tbref = 1;
                    }
                    else
                    {
                        tbref = 0;
                    }

                    if (radioButtonUnidad.Checked == true)
                    {
                        dataGridViewMRefaccion.Visible = true;
                        metodocargarefpdf();
                        co.dbconection().Close();
                        To_pdf();
                        metodocargaref();
                        dataGridViewMRefaccion.Visible = false;

                    }
                    else if (radioButtonGeneral.Checked == true)
                    {
                        dataGridViewMRefaccion.Visible = true;
                        metodocargarefpdf();
                        co.dbconection().Close();
                        To_pdf2();
                        metodocargaref();
                        dataGridViewMRefaccion.Visible = false;
                    }
                }
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e) //Buscar
        {
            if ((string.IsNullOrWhiteSpace(textBoxFolioB.Text)) && (comboBoxUnidadB.Text.Equals("-- UNIDAD --")) && (comboBoxMecanicoB.Text.Equals("-- MECÁNICO --")) && (comboBoxEstatusMB.Text.Equals("-- ESTATUS --")) && (checkBoxFechas.Checked == false) && (comboBoxMesB.Text.Equals("-- MES --")) && (comboBoxDescpFalloB.Text.Equals("-- DESCRIPCIÓN DEL FALLO --")))
            {
                MessageBox.Show("Los campos de busqueda están vacios", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimePickerIni.Value > dateTimePickerFin.Value)
            {
                MessageBox.Show("La fecha inicial no debe superar a la fecha final", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiarcamposbus();
            }
            else if ((dateTimePickerIni.Value >= DateTime.Now) || (dateTimePickerFin.Value >= DateTime.Now))
            {
                MessageBox.Show("Las fechas no deben superar el día de hoy", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiarcamposbus();
                checkBoxFechas.Checked = false;
            }
            else
            {
                String Fini = "";
                String Ffin = "";
                string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.Folio AS 'FOLIO', CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'FECHA DEL REPORTE', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'ESTATUS DEL MANTENIMIENTO',  coalesce((SELECT UPPER(CONCAT(r22.codfallo, ' - ', r22.falloesp)) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'CODIGO DE FALLO', coalesce((SELECT UPPER(DATE_FORMAT(r24.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'FECHA DEL REPORTE DE MANTENIMIENTO', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'MECANICO', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'MECANICO DE APOYO',  coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, UPPER(t1.HoraEntrada) AS 'HORA DE ENTRADA', UPPER(t1.TipoFallo) AS 'TIPO DE FALLO', UPPER(t1.KmEntrada) AS 'KILOMETRAJE', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'DESCRIPCION DE FALLO', UPPER(t1.DescFalloNoCod) AS 'DESCRIPCION DE FALLO NO CODIFICADO', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'OBSERVACIONES DE SUPERVISION', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'FALLO GENERAL', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'TRABAJO REALIZADO', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'HORA DE INICIO DE MANTENIMIENTO', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'HORA DE TERMINO DE MANTENIMIENTO', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'ESPERA DE TIEMPO PARA MANTENIMIENTO', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'DIFERENCIA DE TIEMPO EN MANTENIMIENTO', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'FOLIO DE FACTURA', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'SUPERVISO', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'EXISTENCIA DE REFACCIONES EN ALMACEN', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'ESTATUS DE REFACCIONES', coalesce((SELECT UPPER(CONCAT(r23.ApPaterno, ' ', r23.ApMaterno, ' ', r23.nombres)) FROM reportemantenimiento AS r24 INNER JOIN cpersonal AS r23 ON r24.PersonaFinal = r23.idPersona WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'PERSONA QUE FINALIZO EL MANTENIMIENTO', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'OBSERVACIONES DEL MANTENIMIENTO' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cservicios AS t3 ON t1.Serviciofkcservicios = t3.idservicio INNER JOIN careas AS t4 ON t2.areafkcareas = t4.idarea ";
                string WHERE = "";
                if (!string.IsNullOrWhiteSpace(textBoxFolioB.Text))
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE t1.Folio = '" + textBoxFolioB.Text + "'";
                    }
                    else
                    {
                        WHERE += " AND t1.Folio = '" + textBoxFolioB.Text + "'";
                    }
                }
                if (comboBoxUnidadB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                    }
                    else
                    {
                        WHERE += " AND CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                    }
                }
                if (comboBoxMecanicoB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                    }
                    else
                    {
                        WHERE += " AND (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                    }
                }
                if (comboBoxEstatusMB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                    }
                    else
                    {
                        WHERE += " AND (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                    }
                }
                if (checkBoxFechas.Checked == true)
                {
                    Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                    Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                }
                if (comboBoxMesB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                    }
                }
                if (comboBoxDescpFalloB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                    }
                    else
                    {
                        WHERE += " AND (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                    }
                }
                if (WHERE != "")
                {
                    WHERE += " ORDER BY t1.Folio DESC";
                }
                MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE,co.dbconection());
                DataSet ds = new DataSet();
                adp.Fill(ds);
                dataGridViewMantenimiento.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron reportes", "ADVERTEMCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarcamposbus();
                    metodoCarga();
                    conteo();
                }
                else
                {
                 
                    conteovariable();
                    limpiarcamposbus();
                }
               co.dbconection().Close();

                //buttonGuardar.Visible = false;
                //label24.Visible = false;
                checkBoxFechas.Checked = false;
            }
        }

        private void labelCerrarHerr_Click(object sender, EventArgs e) //Acciones Del Label Cerrar De Refacciones
        {
            groupBoxRefacciones.Visible = false;
            label1.Visible = false;
            groupBoxMantenimiento.Visible = true;
            buttonGuardar.Visible = true;
            label24.Visible = true;
            buttonExcel.Visible = false;
            label35.Visible = false;
            buttonActualizar.Visible = true;
            label26.Visible = true;
            limpiarrefacc();
            dontr = 1;
            if(dataGridViewMRefaccion.Rows.Count == 0)
            {
                comboBoxReqRefacc.SelectedIndex = 2;
            }
            else
            {
                comboBoxReqRefacc.SelectedIndex = 1;
            }
        }

        private void buttonAgregaPed_Click(object sender, EventArgs e) //Agrega Una Nueva Refaccion
        {
            if ((comboBoxFamilia.SelectedIndex == 0) || (comboBoxFRefaccion.SelectedIndex == 0) || (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
            {
                MessageBox.Show("Alguno de los campos esta vacío", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
            {
                MessageBox.Show("El campo debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((textBoxCantidad.Text == "0") || (textBoxCantidad.Text == "00") || (textBoxCantidad.Text == "000") || (textBoxCantidad.Text == ".00") || (textBoxCantidad.Text == "0.0") || (textBoxCantidad.Text == "00.") || (textBoxCantidad.Text == "0/0") || (textBoxCantidad.Text == "/00") || (textBoxCantidad.Text == "00/") || (textBoxCantidad.Text == ".") || (textBoxCantidad.Text == "/"))
            {
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                double pedcantidad = Convert.ToDouble(textBoxCantidad.Text);
                if (pedcantidad == 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                MySqlCommand cmd0 = new MySqlCommand("SELECT NumRefacc FROM pedidosrefaccion WHERE FolioPedfkSupervicion= '" + labelidRepSup.Text + "' ORDER BY idPedRef DESC ",co.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    cont = Convert.ToInt32(dr0.GetString("NumRefacc"));
                    cont = cont + 1;
                }
                else
                {
                    cont = 0;
                    cont = cont + 1;
                }
                dr0.Close();
               co.dbconection().Close();

                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO pedidosrefaccion(NumRefacc, FolioPedfkSupervicion, RefaccionfkCRefaccion, FechaPedido, HoraPedido, Cantidad) VALUES ('" + cont + "', '" + labelidRepSup.Text + "', '" + comboBoxFRefaccion.SelectedValue + "', curdate(), Time(Now()),'" + textBoxCantidad.Text + "')",co.dbconection());
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(dt);
                dataGridViewMRefaccion.DataSource = dt;
               co.dbconection().Close();
                ncontreffin();
                conteofinref();
                metodocargaref();
                MySqlCommand sql = new MySqlCommand("UPDATE reportemantenimiento SET seenalmacen = 0 WHERE FoliofkSupervicion = '" + labelidRepSup.Text + "' ",co.dbconection());
                sql.ExecuteNonQuery();
                MessageBox.Show("Refacción agregada correctamente", "COMPLETADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                crefacc = crefacc + 1;
                limpiarrefacc();
                comboBoxExisRefacc.SelectedIndex = 2;
            }
        }

        private void textBoxFolioB_Click(object sender, EventArgs e)
        {
            textBoxFolioB.SelectAll();
        }

        /* Validaciones de los campos de contraseña*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxMecanico_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsWhiteSpace(e.KeyChar);
            if (e.KeyChar == 44 || e.KeyChar == 46 || e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 45 || e.KeyChar == 46 || e.KeyChar == 47 || e.KeyChar == 42 || e.KeyChar == 64 || e.KeyChar == 95)
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9) y las letras (a - z / A - Z)", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxMecanicoApo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsWhiteSpace(e.KeyChar);
            if (e.KeyChar == 127 || e.KeyChar == 08)
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9) y las letras (a - z / A - Z)", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxSuperviso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127 || e.KeyChar == 08)
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9) y las letras (a - z / A - Z)", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127 || e.KeyChar == 08)
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9)", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxObsMan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 44 || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9), las letras (a - z / A - Z) el punto (.), la coma (,) y el espacio", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxFolioB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127 || e.KeyChar == 08)
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9) y las letras (a - z / A - Z)", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxTrabajoRealizado_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            //optimizar
            if (e.KeyChar == 44 || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan los números (0 - 9), las letras (a - z / A - Z) el punto (.), la coma (,) y el espacio", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números y un solo punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un punto decimal en la caja de texto", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /* Validaciones de las contraseñas *////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxSuperviso.Text) + "' AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidSuperviso.Text = dr["idPersona"].ToString();
                labelNomSuperviso.Text = dr["Nombre"] as string;
                if ((labelNomSuperviso.Text == labelNomMecanico.Text) || (labelNomSuperviso.Text == labelNomMecanicoApo.Text))
                {
                    MessageBox.Show("El Supervisor no debe ser igual al Mecánico y/o Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomSuperviso.Text = "...";
                    textBoxSuperviso.Text = "";
                }
            }
            else if (textBoxSuperviso.Text != "")
            {
                labelidSuperviso.Text = "";
                labelNomSuperviso.Text = "...";
            }
            dr.Close();
           co.dbconection().Close();
        }

        private void textBoxMecanicoApo_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos=t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxMecanicoApo.Text) + "' AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanicoApo.Text = dr["idPersona"].ToString();
                labelNomMecanicoApo.Text = dr["Nombre"] as string;
                if ((labelNomMecanicoApo.Text == labelNomMecanico.Text) || (labelNomMecanicoApo.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico de Apoyo no debe ser igual al Mecánico y/o al Supervisor", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanicoApo.Text = "..";
                    textBoxMecanicoApo.Text = "";
                }
            }
            else if (textBoxMecanicoApo.Text != "")
            {
                labelidMecanicoApo.Text = "";
                labelNomMecanicoApo.Text = "..";
            }
            dr.Close();
           co.dbconection().Close();
        }

        private void textBoxMecanico_Leave(object sender, EventArgs e)
        {
        MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxMecanico.Text) + "' AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'",co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanico.Text = dr["idPersona"].ToString();
                labelNomMecanico.Text = dr["Nombre"] as string;
                if ((labelNomMecanico.Text == labelNomMecanicoApo.Text) || (labelNomMecanico.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico no debe ser igual al Supervisor y/o al Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanico.Text = ".";
                    textBoxMecanico.Text = "";
                }
            }
            else if (textBoxMecanico.Text != "")
            {
                labelidMecanico.Text = "";
                labelNomMecanico.Text = ".";
            }
            dr.Close();
           co.dbconection().Close();
        }

        /*Validaciones extras */
        private void comboBoxFamilia_SelectedValueChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow row3 = dt.NewRow();
            MySqlCommand cmd = new MySqlCommand("SELECT UPPER(t2.nombreRefaccion) AS 'nombreRefaccion', t2.idrefaccion FROM cfamilias AS t1 INNER JOIN crefacciones AS t2 ON t1.idfamilia = t2.familiafkcfamilias WHERE t2.familiafkcfamilias = '" + comboBoxFamilia.SelectedValue + "' GROUP BY t2.nombreRefaccion ORDER BY t2.nombreRefaccion",co.dbconection());
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            row3["idrefaccion"] = 0;
            row3["nombreRefaccion"] = "-- REFACCIÓN --";
            dt.Rows.InsertAt(row3, 0);
            comboBoxFRefaccion.DataSource = dt;
            comboBoxFRefaccion.ValueMember = "idrefaccion";
            comboBoxFRefaccion.DisplayMember = "nombreRefaccion";
            comboBoxFRefaccion.SelectedIndex = 0;

           co.dbconection().Close();

            if(comboBoxFamilia.SelectedIndex == 0)
            {
                comboBoxFRefaccion.Enabled = false;
                comboBoxFRefaccion.SelectedIndex = 0;
            }
            else
            {
                comboBoxFRefaccion.Enabled = true;
            }
        }

        private void comboBoxFRefaccion_SelectedValueChanged(object sender, EventArgs e)
        {
            simb = "";
            MySqlCommand cmd0 = new MySqlCommand("SELECT t2.idunidadmedida, t1.nombreRefaccion, CONCAT(UPPER(t2.Nombre), ' - ', t2.Simbolo) AS 'Unidad De Medida' FROM crefacciones AS t1 INNER JOIN cunidadmedida AS t2 ON t2.idunidadmedida = t1.umfkcunidadmedida WHERE familiafkcfamilias = '" + comboBoxFamilia.SelectedValue + "' AND idrefaccion = '" + comboBoxFRefaccion.SelectedValue + "'", co.dbconection());
            MySqlDataReader dr = cmd0.ExecuteReader();
            if (dr.Read())
            {
                namr = Convert.ToString(dr.GetString("nombreRefaccion"));
                simb = Convert.ToString(dr.GetString("Unidad De Medida"));
                tot = Convert.ToInt32(dr.GetString("idunidadmedida"));
            }
            dr.Close();
            co.dbconection().Close();

            textBoxUM.Text = simb;
        }

        private void comboBoxReqRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxRefacciones.Visible = false;
            if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == true))
            {
                metodocargaref();
                sch = 0;
                if (dontr == 0)
                {
                    inicolumn = 0;
                    inicolumn = dataGridViewMRefaccion.Rows.Count;
                    x = 1;
                }
                buttonGuardar.Visible = false;
                label24.Visible = false;
                buttonAgregaPed.Visible = true;
                label33.Visible = true;
                buttonActualizarPed.Visible = false;
                label3.Visible = false;
                buttonAgregarMasPed.Visible = false;
                label29.Visible = false;
                groupBoxRefacciones.Visible = true;
                label1.Visible = true;
                if (sch == 0)
                {
                    sch = 1;
                    label62.Visible = true;
                    label63.Visible = true;
                }
                else
                {
                    sch = 0;
                    label62.Visible = false;
                    label63.Visible = false;
                }
                dataGridViewMRefaccion.Visible = true;
                groupBoxMantenimiento.Visible = false;
                conteoiniref();
                conteofinref();
                if (comboBoxReqRefacc.Text == "SE REQUIEREN REFACCIONES")
                {
                    comboBoxExisRefacc.Enabled = true;
                }
            }

            else if (((comboBoxReqRefacc.Text == "NO SE REQUIEREN REFACCIONES")) && (buttonEditar.Visible == false))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = false;
                    groupBoxMantenimiento.Visible = true;
                    buttonGuardar.Visible = true;
                    label24.Visible = true;
                }
                else
                {
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    comboBoxExisRefacc.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;



                    textBoxFolioFactura.Enabled = false;
                    textBoxFolioFactura.Text = "";
                }
            }
            else if ((comboBoxReqRefacc.SelectedIndex == 0) && (buttonAgregar.Enabled == true) && !(comboBoxFalloGral.SelectedIndex == 0))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = false;
                    groupBoxMantenimiento.Visible = true;
                    buttonGuardar.Visible = true;
                    label24.Visible = true;
                }
                else
                {
                    buttonAgregar.Visible = false;
                    label39.Visible = false;
                    comboBoxExisRefacc.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;
                    textBoxFolioFactura.Enabled = false;
                    textBoxFolioFactura.Text = "";
                }
            }
        }

        private void comboBoxExisRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cexis == 0) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (!buttonGuardar.Visible))
            {
                
            }
            else if((cexis == 0) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (buttonGuardar.Visible))
            {
                if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) || ((textBoxFolioFactura.Enabled == false) && (textBoxFolioFactura.Text != "")))
                {
                    if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")))
                    {
                        if (!textBoxFolioFactura.Enabled == false)
                        {
                            textBoxFolioFactura.Enabled = false;
                            textBoxFolioFactura.Text = "";
                        }
                    }

                    if (!(comboBoxFalloGral.SelectedIndex == 0))
                    {
                        validarrefacciones();

                        if (nrefacc == ctotal)
                        {
                            cvalidacion = cvalidacion + 3;
                            if (nrefacc == exist)
                            {
                                cvalidacion = cvalidacion + 2;
                                if (totfalt == 0)
                                {
                                    cvalidacion = cvalidacion + 1;
                                }
                                else
                                {
                                    cvalidacion = cvalidacion + 2;
                                }
                            }
                            else
                            {
                                cvalidacion = cvalidacion + 1;
                            }
                        }
                        else
                        {
                            cvalidacion = cvalidacion + 1;
                        }

                        if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 1))
                        {
                            if (!groupBoxRefacciones.Visible == true)
                            {
                                if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES") && ! mensaje)
                                {
                                    MessageBox.Show("No todas las refacciones solicitadas estan validadas\n Espere hasta que las validen", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))
                                {
                                    
                                }
                                else if (comboBoxExisRefacc.SelectedIndex == 0)
                                {
                                    MessageBox.Show("Seleccione otra opción valida", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            crefverificadas = 2;
                            validar2();
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 4))
                        {
                            if (!(comboBoxFalloGral.Text == "-- FALLO GENERAL --"))
                            {
                                MessageBox.Show("No todas las refacciones solicitadas están en existencia\n Espere hasta que almacén vuelva a tener existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            crefverificadas = 2;
                            validar2();
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 7))
                        {

                            MessageBox.Show("No todas las refacciones han sido entregadas\nespere hasta que almacén le entregue todas las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            crefverificadas = 2;
                            validar2();
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (textBoxFolioFactura.Enabled == false) && (cvalidacion == 6) && !mensaje)
                        {
                            if(!mensaje)MessageBox.Show("Todas las refacciones han sido entregadas", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            crefverificadas = 1;
                            validar2();
                        }
                    }
                    else if ((textBoxFolioFactura.Text != "") && ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA"))))
                    {
                        buttonAgregar.Visible = true;
                        label34.Visible = true;
                    }
                    //else
                    //{
                    //    comboBoxExisRefacc.SelectedIndex = 0;
                    //}
                }
            }
            else if (cexis == 1)
            {
                cexis = 0;
                cvalidacion = 0;
            }
        }

        private void comboBoxEstatusMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (comboBoxEstatusMant.Enabled == false))
            {
                buttonGuardar.Visible = false;
                label24.Visible = false;
                buttonFinalizar.Visible = false;
                label37.Visible = false;
            }
            else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (comboBoxEstatusMant.Enabled == true))
            {
                if ((string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)) && (string.IsNullOrWhiteSpace(textBoxMecanico.Text)) && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && (comboBoxEstatusMant.Text != "LIBERADA") && (string.IsNullOrWhiteSpace(comboBoxReqRefacc.Text)) && (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                {
                    MessageBox.Show("Algunos campos les faltan información", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validar();
                }
                else if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                {
                    MessageBox.Show("El trabajo realizado no puede quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validar();
                }
                else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (labelEstatusMan.Text.Equals("LIBERADA")))
                {
                    buttonGuardar.Visible = false;
                    label24.Visible = false;
                    buttonFinalizar.Visible = false;
                    label37.Visible = false;
                }
                else
                {
                    if ((comboBoxFalloGral.Text != "") && (labelNomMecanico.Text != ".") && (textBoxTrabajoRealizado.Text != "") && (comboBoxEstatusMant.Text != ""))
                    {
                        timer2.Stop();
                        sumafech();
                        labelHoraTerminoM.Text = dateTimePicker2.Text;
                        buttonGuardar.Visible = false;
                        label24.Visible = false;
                        buttonFinalizar.Visible = true;
                        label37.Visible = true;
                        buttonAgregar.Visible = false;
                        label39.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Verifique los datos ingresados, puede que falten algunos por llenar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        validar();
                    }
                }
            }
            else if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (string.IsNullOrWhiteSpace(labelEstatusMan.Text)) || (labelEstatusMan.Text.Equals("REPROGRAMADA")))
            {
                labelHoraTerminoM.Text = "";
                textBoxTerminoMan.Text = "";
                buttonFinalizar.Visible = false;
                label37.Visible = false;
                buttonGuardar.Visible = true;
                label24.Visible = true;
            }
            else if ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) && (string.IsNullOrWhiteSpace(labelEstatusMan.Text)) || (labelEstatusMan.Text.Equals("EN PROCESO")))
            {
                buttonFinalizar.Visible = false;
                label37.Visible = false;
                buttonGuardar.Visible = true;
                label24.Visible = true;
            }
        }

        private void checkBoxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechas.Checked == true)
            {
                dateTimePickerIni.Enabled = true;
                dateTimePickerFin.Enabled = true;
                comboBoxMesB.Enabled = false;
                comboBoxMesB.SelectedIndex = 0;
            }
            else
            {
                dateTimePickerIni.Enabled = false;
                dateTimePickerFin.Enabled = false;
                comboBoxMesB.Enabled = true;
            }
        }

        private void dateTimePickerIni_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dateTimePickerFin_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        /* Acciones al presionar una tecla o dar click*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_TextChanged(object sender, EventArgs e)
        {
            if(textBoxSuperviso.Text == "")
            {
                labelNomSuperviso.Text = "...";
            }
        }

        private void textBoxMecanicoApo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMecanicoApo.Text))
            {
                labelNomMecanicoApo.Text = "..";
            }

            if (bedit == true)
            {
                string fgl = "";
                if (comboBoxFalloGral.SelectedIndex == 0)
                {
                    fgl = "";
                }
                else
                {
                    fgl = Convert.ToString(comboBoxFalloGral.Text);
                }
                if ((((cdf == fgl) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabreak == textBoxTrabajoRealizado.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))) && ((folfac == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((obsmant == textBoxObsMan.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObsMan.Text))))))
                {
                    buttonEditar.Visible = false;
                    label58.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label58.Visible = true;
                }
            }
        }

        private void comboBoxMesB_TextChanged(object sender, EventArgs e)
        {
            month = 00;
            if (comboBoxMesB.Text.Equals("ENERO"))
            {
                month = 01;
            }
            else if (comboBoxMesB.Text.Equals("FEBRERO"))
            {
                month = 02;
            }
            else if (comboBoxMesB.Text.Equals("MARZO"))
            {
                month = 03;
            }
            else if (comboBoxMesB.Text.Equals("ABRIL"))
            {
                month = 04;
            }
            else if (comboBoxMesB.Text.Equals("MAYO"))
            {
                month = 05;
            }
            else if (comboBoxMesB.Text.Equals("JUNIO"))
            {
                month = 06;
            }
            else if (comboBoxMesB.Text.Equals("JULIO"))
            {
                month = 07;
            }
            else if (comboBoxMesB.Text.Equals("AGOSTO"))
            {
                month = 08;
            }
            else if (comboBoxMesB.Text.Equals("SEPTIEMBRE"))
            {
                month = 09;
            }
            else if (comboBoxMesB.Text.Equals("OCTUBRE"))
            {
                month = 10;
            }
            else if (comboBoxMesB.Text.Equals("NOVIEMBRE"))
            {
                month = 11;
            }
            else if (comboBoxMesB.Text.Equals("DICIEMBRE"))
            {
                month = 12;
            }
        }

        private void textBoxMecanico_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxMecanico.Text))
            {
                labelNomMecanico.Text = ".";
            }
        }

        /* Movimiento de los botónes y bloqueo de la rueda de mouse *//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonPDF_MouseMove(object sender, MouseEventArgs e)
        {
            buttonPDF.Size = new Size(63, 63);
        }

        private void buttonPDF_MouseLeave(object sender, EventArgs e)
        {
            buttonPDF.Size = new Size(58, 58);
        }

        private void buttonExcel_MouseMove(object sender, MouseEventArgs e)
        {
            buttonExcel.Size = new Size(63, 63);
        }

        private void buttonExcel_MouseLeave(object sender, EventArgs e)
        {
            buttonExcel.Size = new Size(58, 58);
        }

        private void buttonBuscar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonBuscar.Size = new Size(44, 41);
        }

        private void buttonBuscar_MouseLeave(object sender, EventArgs e)
        {
            buttonBuscar.Size = new Size(39, 36);
        }

        private void buttonGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonGuardar.Size = new Size(68, 62);
        }

        private void buttonGuardar_MouseLeave(object sender, EventArgs e)
        {
            buttonGuardar.Size = new Size(63, 57);
        }

        private void FormFallasMantenimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            hilo.Abort();
        }

        private void buttonActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonActualizar.Size = new Size(68, 62);
        }

        private void buttonActualizar_MouseLeave(object sender, EventArgs e)
        {
            buttonActualizar.Size = new Size(63, 57);
        }

        private void buttonFinalizar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonFinalizar.Size = new Size(68, 62);
        }

        private void buttonFinalizar_MouseLeave(object sender, EventArgs e)
        {
            buttonFinalizar.Size = new Size(63, 57);
        }

        private void buttonAgregaPed_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAgregaPed.Size = new Size(54, 50);
        }

        private void buttonAgregaPed_MouseLeave(object sender, EventArgs e)
        {
            buttonAgregaPed.Size = new Size(49, 45);
        }

        private void buttonEliminarPed_MouseMove(object sender, MouseEventArgs e)
        {
            buttonActualizarPed.Size = new Size(54, 50);
        }

        private void buttonEliminarPed_MouseLeave(object sender, EventArgs e)
        {
            buttonActualizarPed.Size = new Size(49, 45);
        }

        private void buttonAgregar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAgregar.Size = new Size(30, 30);
        }

        private void buttonAgregar_MouseLeave(object sender, EventArgs e)
        {
            buttonAgregar.Size = new Size(27, 27);
        }

        private void buttonAgregarMasPed_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAgregarMasPed.Size = new Size(54, 50);
        }

        private void buttonAgregarMasPed_MouseLeave(object sender, EventArgs e)
        {
            buttonAgregarMasPed.Size = new Size(49, 45);
        }

        private void buttonEditar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonEditar.Size = new Size(68, 62);
        }

        private void buttonEditar_MouseLeave(object sender, EventArgs e)
        {
            buttonEditar.Size = new Size(63, 57);
        }

        void comboBoxFalloGral_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxReqRefacc_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxExisRefacc_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxEstatusMant_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxFamilia_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxFRefaccion_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxUnidadB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxMecanicoB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxEstatusMB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxDescpFalloB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxMesB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        /* Color a Celdas de GridView y Label *////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void labelCerrarHerr_MouseClick(object sender, MouseEventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.LightCoral;
        }

        private void labelCerrarHerr_MouseLeave(object sender, EventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void labelCerrarHerr_MouseDown(object sender, MouseEventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void dataGridViewMantenimiento_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(this.dataGridViewMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DEL MANTENIMIENTO")
            {
                if(Convert.ToString(e.Value) == "EN PROCESO")
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else if(Convert.ToString(e.Value) == "LIBERADA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else if(Convert.ToString(e.Value) == "REPROGRAMADA")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }

            if(this.dataGridViewMantenimiento.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
            {
                if (Convert.ToString(e.Value) == "CORRECTIVO")
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else if (Convert.ToString(e.Value) == "PREVENTIVO")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else if (Convert.ToString(e.Value) == "REITERATIVO")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
                else if (Convert.ToString(e.Value) == "REPROGRAMADO")
                {
                    e.CellStyle.BackColor = Color.LightBlue;
                }
                else if (Convert.ToString(e.Value) == "SEGUIMIENTO")
                {
                    e.CellStyle.BackColor = Color.FromArgb(246, 106, 77);
                }
            }

            if(this.dataGridViewMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DE REFACCIONES")
            {
                if(Convert.ToString(e.Value) == "SE REQUIEREN REFACCIONES")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else if(Convert.ToString(e.Value) == "NO SE REQUIEREN REFACCIONES")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }

            if(this.dataGridViewMantenimiento.Columns[e.ColumnIndex].Name == "EXISTENCIA DE REFACCIONES EN ALMACEN")
            {
                if (Convert.ToString(e.Value) == "EXISTENCIA DE REFACCIONES")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else if(Convert.ToString(e.Value) == "EN ESPERA DE LA REFACCIÓN")
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else if(Convert.ToString(e.Value) == "SIN REFACCIONES")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void dataGridViewMRefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(this.dataGridViewMRefaccion.Columns[e.ColumnIndex].Name == "ESTATUS DE LA REFACCION")
            {
                if(Convert.ToString(e.Value) == "EXISTENCIA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen; 
                }
                else if(Convert.ToString(e.Value) == "SIN EXISTENCIA")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
            if(this.dataGridViewMRefaccion.Columns[e.ColumnIndex].Name == "CANTIDAD POR ENTREGAR")
            {
                if(Convert.ToString(e.Value) != "0")
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
            }
        }

        private void comboBoxEstatusMant_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                }
            }
            else
            {
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;
                        case 1:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;
                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 3:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;
                    }
                }
            }
        }

        private void comboBoxReqRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                }
            }
            else
            {
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;
                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                        break;
                    }
                }
            }
        }

        private void comboBoxExisRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);


            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                }
            }
            else
            {
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 2:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 3:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        private void comboBoxEstatusMB_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);


            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                }
            }
            else
            {
                if (e.Index == -1)
                {
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                }
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 1:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                        case 3:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        private void dataGridViewMantenimiento_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridViewMRefaccion_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();

                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        public void combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        private void textBoxTrabajoRealizado_Validated(object sender, EventArgs e)
        {
            while (textBoxTrabajoRealizado.Text.Contains("  "))
            {
                textBoxTrabajoRealizado.Text = textBoxTrabajoRealizado.Text.Replace("  ", " ");
            }
        }

        private void textBoxObsMan_Validated(object sender, EventArgs e)
        {
            while (textBoxObsMan.Text.Contains("  "))
            {
                textBoxObsMan.Text = textBoxObsMan.Text.Replace("  ", " ");
            }
        }

        public int rowIndex { get; set; }
        bool mensaje = false;

        private void dataGridViewMantenimiento_MouseClick(object sender, MouseEventArgs e)
        {
            bedit = false;
            mensaje = false;
            if(peditar == true)
            {
                if(e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                    int xy = dataGridViewMantenimiento.HitTest(e.X, e.Y).RowIndex;

                    if(xy >= 0)
                    {
                        mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                    }
                    mn.Show(dataGridViewMantenimiento, new Point(e.X, e.Y));
                    mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);   
                }
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            mensaje = false;
            string fg = "";
            labelEstatusMan.Text = dataGridViewMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            if (!(labelEstatusMan.Text.Equals("")))
            {
                if (comboBoxFalloGral.Text.Equals("-- FALLO GENERAL --"))
                {
                    fg = "";
                }
                else
                {
                    fg = Convert.ToString(comboBoxFalloGral.SelectedValue);
                }
                if ((((idfg == fg) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabreak == textBoxTrabajoRealizado.Text) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))) && ((folfac == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))) && ((obsmant == textBoxObsMan.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObsMan.Text)))))
                {
                    switch (e.ClickedItem.Name.ToString())
                    {
                        case "EDITAR":
                       
                            privilegios();
                            label1.Visible = false;
                            groupBoxRefacciones.Visible = false;
                            groupBoxMantenimiento.Visible = true;
                            timer2.Start();
                            limpiarcampos();
                            limpiarcamposbus();
                            comboBoxFalloGral.Enabled = false;
                            textBoxMecanico.Enabled = false;
                            textBoxMecanicoApo.Enabled = false;
                            comboBoxReqRefacc.Enabled = false;
                            comboBoxExisRefacc.Enabled = false;
                            textBoxFolioFactura.Enabled = false;
                            textBoxTrabajoRealizado.Enabled = false;
                            comboBoxEstatusMant.Enabled = false;
                            textBoxSuperviso.Enabled = false;
                            textBoxObsMan.Enabled = false;
                            buttonAgregar.Visible = false;
                            label39.Visible = false;
                            buttonGuardar.Visible = false;
                            label24.Visible = false;
                            buttonEditar.Visible = false;
                            label58.Visible = false;
                            buttonExcel.Visible = false;
                            label35.Visible = false;
                            buttonActualizar.Visible = true;
                            label26.Visible = true;
                            mensaje = true;
                            if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                            {
                                buttonPDF.Visible = true;
                                label36.Visible = true;
                                radioButtonGeneral.Visible = true;
                                radioButtonUnidad.Visible = true;
                            }
                            else
                            {
                                buttonPDF.Visible = false;
                                label36.Visible = false;
                                radioButtonGeneral.Visible = false;
                                radioButtonUnidad.Visible = false;
                            }
                            labelFolioSinLetter.Text = dataGridViewMantenimiento.CurrentRow.Cells["FOLIO"].Value.ToString();
                            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT r25.FoliofkSupervicion FROM reportemantenimiento AS r25 WHERE r25.FoliofkSupervicion = t1.idReporteSupervicion), '') AS Folio, coalesce((SELECT r25.IdReporte FROM reportemantenimiento AS r25 WHERE t1.idReporteSupervicion = r25.FoliofkSupervicion), '') AS IdMantenimiento, coalesce((SELECT r22.MecanicofkPersonal FROM reportemantenimiento AS r22 WHERE t1.idReporteSupervicion = r22.FoliofkSupervicion), '') AS IdMecanico, coalesce((SELECT r23.MecanicoApoyofkPersonal FROM reportemantenimiento AS r23 WHERE t1.idReporteSupervicion = r23.FoliofkSupervicion), '') AS IdMecanicoApoyo, coalesce((SELECT r24.SupervisofkPersonal FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS IdSuperviso, t1.idReporteSupervicion AS ID, t1.Folio, CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'Fecha Del Reporte', coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, t1.HoraEntrada AS 'Hora De Entrada', t1.KmEntrada AS 'Kilometraje', UPPER(t1.TipoFallo) AS 'Tipo De Fallo', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'Descripcion De Fallo', coalesce((SELECT UPPER(r22.codfallo) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'Codigo De Fallo', coalesce((UPPER(t1.DescFalloNoCod)), '') AS 'Descripcion De Fallo No Codificado', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'Observaciones De Supervision', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'Fallo General', coalesce((SELECT UPPER(r4.idfallogral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'IdFG',coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'Mecanico', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'Mecanico De Apoyo', coalesce((SELECT UPPER(DATE_FORMAT(r10.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r10 WHERE t1.idReporteSupervicion = r10.FoliofkSupervicion), '') AS 'Fecha Del Reporte De Mantenimiento', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'Hora De Inicio De Mantenimiento', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'Hora De Termino De Mantenimiento', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'Espera De Tiempo Para Mantenimiento', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'Diferencia De Tiempo En Mantenimiento', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'Folio De Factura', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'Estatus Del Mantenimiento', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'Superviso', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'Existencia De Refacciones En Almacen', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'Estatus De Refacciones', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'Observaciones Del Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cservicios AS t3 ON t1.Serviciofkcservicios = t3.idservicio INNER JOIN careas AS t4 ON t2.areafkcareas= t4.idarea WHERE t1.Folio = '" + labelFolioSinLetter.Text + "'",co.dbconection());
                            MySqlDataReader dr = cmd.ExecuteReader();
                            co.dbconection().Close();
                            if (dr.Read())
                            {
                                folio = Convert.ToString(dr.GetString("Folio"));
                                labelFecha.Text = Convert.ToString(dr.GetString("Fecha Del Reporte De Mantenimiento"));
                                labelidMecanico.Text = Convert.ToString(dr.GetString("IdMecanico"));
                                labelidMecanicoApo.Text = Convert.ToString(dr.GetString("IdMecanicoApoyo"));
                                labelidSuperviso.Text = Convert.ToString(dr.GetString("IdSuperviso"));
                                labelidRepMant.Text = Convert.ToString(dr.GetString("IdMantenimiento"));
                                labelidRepSup.Text = Convert.ToString(dr.GetString("ID"));
                                labelFolio.Text = labelFolioSinLetter.Text;
                                labelUnidad.Text = Convert.ToString(dr.GetString("ECO"));
                                labelFechaReporte.Text = Convert.ToString(dr.GetString("Fecha Del Reporte"));
                                labelHoraReporte.Text = Convert.ToString(dr.GetString("Hora De Entrada"));
                                labelKm.Text = Convert.ToString(dr.GetString("Kilometraje"));
                                labelSupervisor.Text = Convert.ToString(dr.GetString("Supervisor"));
                                labelcodfallo.Text = Convert.ToString(dr.GetString("Codigo De Fallo"));
                                textBoxDescrpFallo.Text = Convert.ToString(dr.GetString("Descripcion De Fallo"));
                                textBoxFallaNoCod.Text = Convert.ToString(dr.GetString("Descripcion De Fallo No Codificado"));
                                textBoxObsSup.Text = Convert.ToString(dr.GetString("Observaciones De Supervision"));
                                comboBoxEstatusMant.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                                labelNomMecanico.Text = Convert.ToString(dr.GetString("Mecanico"));
                                mec = labelNomMecanico.Text;
                                labelHoraInicioM.Text = Convert.ToString(dr.GetString("Hora De Inicio De Mantenimiento"));
                                estmant = comboBoxEstatusMant.Text;                         
                                labelEstatusMan.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                                comboBoxFalloGral.Text = Convert.ToString(dr.GetString("Fallo General"));
                                cdf = comboBoxFalloGral.Text;
                                idfg = Convert.ToString(dr.GetString("IdFG"));
                                labelNomMecanicoApo.Text = Convert.ToString(dr.GetString("Mecanico De Apoyo"));
                                mecapo = labelNomMecanicoApo.Text;
                                if (string.IsNullOrWhiteSpace(mecapo))
                                {
                                    labelNomMecanicoApo.Text = "..";
                                }
                                textBoxEsperaMan.Text = Convert.ToString(dr.GetString("Espera De Tiempo Para Mantenimiento"));
                                textBoxFolioFactura.Text = Convert.ToString(dr.GetString("Folio De Factura"));
                                folfac = textBoxFolioFactura.Text;
                                textBoxTrabajoRealizado.Text = Convert.ToString(dr.GetString("Trabajo Realizado"));
                                trabreak = textBoxTrabajoRealizado.Text;
                                labelNomSuperviso.Text = Convert.ToString(dr.GetString("Superviso").ToString());
                                supmant = labelNomSuperviso.Text;
                                if (string.IsNullOrWhiteSpace(supmant))
                                {
                                    labelNomSuperviso.Text = "...";
                                }
                                textBoxObsMan.Text = Convert.ToString(dr.GetString("Observaciones Del Mantenimiento"));
                                obsmant = textBoxObsMan.Text;
                                conteoiniref();
                                comboBoxReqRefacc.Text = Convert.ToString(dr.GetString("Estatus De Refacciones"));
                                reqref = comboBoxReqRefacc.Text;
                                comboBoxExisRefacc.Text = Convert.ToString(dr.GetString("Existencia De Refacciones En Almacen"));
                                exiref = comboBoxExisRefacc.Text;
                                if (comboBoxExisRefacc.Text == "-- EXISTENCIA --")
                                {
                                    exiref = "";
                                }
                                metodocargaref();
                                conteofinref();
                                ncontrefini();
                            }
                            buttonGuardar.Visible = false;
                            label24.Visible = false;
                            if (labelEstatusMan.Text.Equals("LIBERADA"))
                            {
                                dateTimePicker2.Text = Convert.ToString(dr.GetString("Hora De Termino De Mantenimiento"));
                                labelHoraTerminoM.Text = dateTimePicker2.Text;
                                textBoxTerminoMan.Text = Convert.ToString(dr.GetString("Diferencia De Tiempo En Mantenimiento"));

                                if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                                {
                                    comboBoxFalloGral.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                                {
                                    textBoxTrabajoRealizado.Enabled = true;
                                }
                                else
                                {
                                    textBoxTrabajoRealizado.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                                {
                                    textBoxFolioFactura.Enabled = true;
                                }
                                if ((string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                                {
                                    textBoxObsMan.Enabled = true;
                                }
                                else
                                {
                                    textBoxObsMan.Enabled = true;
                                }
                            }
                            else
                            {
                                if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                                {
                                    comboBoxFalloGral.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                                {
                                    textBoxTrabajoRealizado.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                                {
                                    textBoxFolioFactura.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                                {
                                    textBoxObsMan.Enabled = true;
                                }
                                buttonExcel.Visible = false;
                                label35.Visible = false;
                                buttonPDF.Visible = true;
                                label36.Visible = true;
                                radioButtonGeneral.Visible = true;
                                radioButtonUnidad.Visible = true;
                                buttonAgregar.Visible = false;
                                label39.Visible = false;
                            }
                            dr.Close();
                            co.dbconection().Close();
                            bedit = true;
                        break;

                    default:
                        MessageBox.Show("DEFAULT");
                    break;
                    }
                }
                else
                {
                    if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        switch (e.ClickedItem.Name.ToString())
                        {
                            case "EDITAR":

                                privilegios();
                                label1.Visible = false;
                                groupBoxRefacciones.Visible = false;
                                groupBoxMantenimiento.Visible = true;
                                timer2.Start();
                                limpiarcampos();
                                limpiarcamposbus();
                                comboBoxFalloGral.Enabled = false;
                                textBoxMecanico.Enabled = false;
                                textBoxMecanicoApo.Enabled = false;
                                comboBoxReqRefacc.Enabled = false;
                                comboBoxExisRefacc.Enabled = false;
                                textBoxFolioFactura.Enabled = false;
                                textBoxTrabajoRealizado.Enabled = false;
                                comboBoxEstatusMant.Enabled = false;
                                textBoxSuperviso.Enabled = false;
                                textBoxObsMan.Enabled = false;
                                buttonAgregar.Visible = false;
                                label39.Visible = false;
                                buttonGuardar.Visible = false;
                                label24.Visible = false;
                                buttonEditar.Visible = false;
                                label58.Visible = false;
                                limpiarcampos();
                                buttonExcel.Visible = false;
                                label35.Visible = false;
                                if (pinsertar == true && pconsultar == true && peditar == true && pdesactivar == true)
                                {
                                    buttonPDF.Visible = true;
                                    label36.Visible = true;
                                    radioButtonGeneral.Visible = true;
                                    radioButtonUnidad.Visible = true;
                                }
                                else
                                {
                                    buttonPDF.Visible = false;
                                    label36.Visible = false;
                                    radioButtonGeneral.Visible = false;
                                    radioButtonUnidad.Visible = false;
                                }
                                labelFolioSinLetter.Text = dataGridViewMantenimiento.CurrentRow.Cells["FOLIO"].Value.ToString();
                                labelEstatusMan.Text = dataGridViewMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
                                if (!(labelEstatusMan.Text.Equals("")))
                                {
                                    MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT r25.FoliofkSupervicion FROM reportemantenimiento AS r25 WHERE r25.FoliofkSupervicion = t1.idReporteSupervicion), '') AS Folio, coalesce((SELECT r25.IdReporte FROM reportemantenimiento AS r25 WHERE t1.idReporteSupervicion = r25.FoliofkSupervicion), '') AS IdMantenimiento, coalesce((SELECT r22.MecanicofkPersonal FROM reportemantenimiento AS r22 WHERE t1.idReporteSupervicion = r22.FoliofkSupervicion), '') AS IdMecanico, coalesce((SELECT r23.MecanicoApoyofkPersonal FROM reportemantenimiento AS r23 WHERE t1.idReporteSupervicion = r23.FoliofkSupervicion), '') AS IdMecanicoApoyo, coalesce((SELECT r24.SupervisofkPersonal FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS IdSuperviso, t1.idReporteSupervicion AS ID, t1.Folio, CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'Fecha Del Reporte', coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, t1.HoraEntrada AS 'Hora De Entrada', t1.KmEntrada AS 'Kilometraje', UPPER(t1.TipoFallo) AS 'Tipo De Fallo', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'Descripcion De Fallo', coalesce((SELECT UPPER(r22.codfallo) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'Codigo De Fallo', coalesce((UPPER(t1.DescFalloNoCod)), '') AS 'Descripcion De Fallo No Codificado', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'Observaciones De Supervision', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'Fallo General', coalesce((SELECT UPPER(r4.idfallogral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'IdFG',coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'Mecanico', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'Mecanico De Apoyo', coalesce((SELECT UPPER(DATE_FORMAT(r10.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r10 WHERE t1.idReporteSupervicion = r10.FoliofkSupervicion), '') AS 'Fecha Del Reporte De Mantenimiento', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'Hora De Inicio De Mantenimiento', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'Hora De Termino De Mantenimiento', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'Espera De Tiempo Para Mantenimiento', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'Diferencia De Tiempo En Mantenimiento', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'Folio De Factura', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'Estatus Del Mantenimiento', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'Superviso', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'Existencia De Refacciones En Almacen', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'Estatus De Refacciones', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'Observaciones Del Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cservicios AS t3 ON t1.Serviciofkcservicios = t3.idservicio INNER JOIN careas AS t4 ON t2.areafkcareas= t4.idarea WHERE t1.Folio = '" + labelFolioSinLetter.Text + "'",co.dbconection());
                                    MySqlDataReader dr = cmd.ExecuteReader();
                                    co.dbconection().Close();
                                    if (dr.Read())
                                    {
                                        folio = Convert.ToString(dr.GetString("Folio"));
                                        labelFecha.Text = Convert.ToString(dr.GetString("Fecha Del Reporte De Mantenimiento"));
                                        labelidMecanico.Text = Convert.ToString(dr.GetString("IdMecanico"));
                                        labelidMecanicoApo.Text = Convert.ToString(dr.GetString("IdMecanicoApoyo"));
                                        labelidSuperviso.Text = Convert.ToString(dr.GetString("IdSuperviso"));
                                        labelidRepMant.Text = Convert.ToString(dr.GetString("IdMantenimiento"));
                                        labelidRepSup.Text = Convert.ToString(dr.GetString("ID"));
                                        labelFolio.Text = labelFolioSinLetter.Text;
                                        labelUnidad.Text = Convert.ToString(dr.GetString("ECO"));
                                        labelFechaReporte.Text = Convert.ToString(dr.GetString("Fecha Del Reporte"));
                                        labelHoraReporte.Text = Convert.ToString(dr.GetString("Hora De Entrada"));
                                        labelKm.Text = Convert.ToString(dr.GetString("Kilometraje"));
                                        labelSupervisor.Text = Convert.ToString(dr.GetString("Supervisor"));
                                        labelcodfallo.Text = Convert.ToString(dr.GetString("Codigo De Fallo"));
                                        textBoxDescrpFallo.Text = Convert.ToString(dr.GetString("Descripcion De Fallo"));
                                        textBoxFallaNoCod.Text = Convert.ToString(dr.GetString("Descripcion De Fallo No Codificado"));
                                        textBoxObsSup.Text = Convert.ToString(dr.GetString("Observaciones De Supervision"));
                                        comboBoxEstatusMant.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                                        labelNomMecanico.Text = Convert.ToString(dr.GetString("Mecanico"));
                                        mec = labelNomMecanico.Text;
                                        labelHoraInicioM.Text = Convert.ToString(dr.GetString("Hora De Inicio De Mantenimiento"));
                                        estmant = comboBoxEstatusMant.Text;
                                        labelEstatusMan.Text = Convert.ToString(dr.GetString("Estatus Del Mantenimiento").ToString());
                                        comboBoxFalloGral.Text = Convert.ToString(dr.GetString("Fallo General"));
                                        cdf = comboBoxFalloGral.Text;
                                        idfg = Convert.ToString(dr.GetString("IdFG"));
                                        labelNomMecanicoApo.Text = Convert.ToString(dr.GetString("Mecanico De Apoyo"));
                                        mecapo = labelNomMecanicoApo.Text;
                                        if (string.IsNullOrWhiteSpace(mecapo))
                                        {
                                            labelNomMecanicoApo.Text = "..";
                                        }
                                        textBoxEsperaMan.Text = Convert.ToString(dr.GetString("Espera De Tiempo Para Mantenimiento"));
                                        textBoxFolioFactura.Text = Convert.ToString(dr.GetString("Folio De Factura"));
                                        folfac = textBoxFolioFactura.Text;
                                        textBoxTrabajoRealizado.Text = Convert.ToString(dr.GetString("Trabajo Realizado"));
                                        trabreak = textBoxTrabajoRealizado.Text;
                                        labelNomSuperviso.Text = Convert.ToString(dr.GetString("Superviso").ToString());
                                        supmant = labelNomSuperviso.Text;
                                        if (string.IsNullOrWhiteSpace(supmant))
                                        {
                                            labelNomSuperviso.Text = "...";
                                        }
                                        textBoxObsMan.Text = Convert.ToString(dr.GetString("Observaciones Del Mantenimiento"));
                                        obsmant = textBoxObsMan.Text;
                                        conteoiniref();
                                        comboBoxReqRefacc.Text = Convert.ToString(dr.GetString("Estatus De Refacciones"));
                                        reqref = comboBoxReqRefacc.Text;
                                        comboBoxExisRefacc.Text = Convert.ToString(dr.GetString("Existencia De Refacciones En Almacen"));
                                        exiref = comboBoxExisRefacc.Text;
                                        metodocargaref();
                                        conteofinref();
                                        ncontrefini();
                                    }
                                    buttonGuardar.Visible = false;
                                    label24.Visible = false;
                                    if (labelEstatusMan.Text.Equals("LIBERADA"))
                                    {
                                        dateTimePicker2.Text = Convert.ToString(dr.GetString("Hora De Termino De Mantenimiento"));
                                        labelHoraTerminoM.Text = dateTimePicker2.Text;
                                        textBoxTerminoMan.Text = Convert.ToString(dr.GetString("Diferencia De Tiempo En Mantenimiento"));
    
                                        if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                                        {
                                            comboBoxFalloGral.Enabled = true;
                                        }
                                        if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                                        {
                                            textBoxTrabajoRealizado.Enabled = true;
                                        }
                                        else
                                        {
                                            textBoxTrabajoRealizado.Enabled = true;
                                        }
                                        if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                                        {
                                            textBoxFolioFactura.Enabled = true;
                                        }
                                        if ((string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                                        {
                                            textBoxObsMan.Enabled = true;
                                        }
                                        else
                                        {
                                            textBoxObsMan.Enabled = true;
                                        }
                                    }   
                                    else
                                    {
                                        if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                                        {
                                            comboBoxFalloGral.Enabled = true;
                                        }
                                        if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                                        {
                                            textBoxTrabajoRealizado.Enabled = true;
                                        }
                                        if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                                        {   
                                            textBoxFolioFactura.Enabled = true;
                                        }
                                        if (!(string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                                        {
                                            textBoxObsMan.Enabled = true;
                                        }
                                        buttonExcel.Visible = false;
                                        label35.Visible = false;
                                        buttonPDF.Visible = true;
                                        label36.Visible = true;
                                        radioButtonGeneral.Visible = true;
                                        radioButtonUnidad.Visible = true;
                                        radioButtonUnidad.Visible = true;            
                                    }
                                    dr.Close();
                                    co.dbconection().Close();
                                    bedit = true;
                                }
                                else
                                {
                                    MessageBox.Show("No puede editar un reporte si no se ha guardado por lo menos una vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            break;

                            default:
                                MessageBox.Show("DEFAULT");
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No puede editar un reporte si no se ha guardado por lo menos una vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        private void groupBoxSupervision_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxTrabR_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxObsM_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxRefacciones_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxMantenimiento_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxBusqueda_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxFechas_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxDescrF_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxFNCod_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxObsSup_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxRefacc_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
    }
}
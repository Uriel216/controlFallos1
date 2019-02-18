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
using h = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace controlFallos
{
    public partial class catUnidaesTRI : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario,empresa,area;
        string binanterior,nmotoranterior,ntransmisionAnterior,modeloAnterior,marcaAnterior;
        
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }

        public void privilegios()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catUnidades'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
                pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
                peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
                pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
                mostrar();
                mdr.Close();
                c.dbconection().Close();
            }
            else
            {
                this.Close();
            }
        }

        private void catUnidaesTRI_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                iniecos();
                bunidades();
                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) as nombreEmpresa FROM cempresas WHERE status=1 ")) > 0)
                {
                    iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status=1 ORDER BY nombreEmpresa ASC", csetEmpresa, "idempresa", "nombreEmpresa", "--Seleccione una Empresa--");
                    if (Convert.ToInt32(v.getaData("SELECT COUNT(idarea)FROM careas")) > 0)
                    {
                        iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.nombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--Seleccione un Área--");
                    }
                    else
                    {
                        MessageBox.Show("No Hay Areas Activas", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("No Hay Empresas Activas", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        // Todos los métodos ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void bunidades()
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SELECT t1.consecutivo AS 'ECO TRI', CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) as ECO, UPPER(COALESCE(t1.bin,'')) AS VIN, UPPER(coalesce(t1.nmotor, '')) AS 'NÚMERO DE MOTOR', UPPER(coalesce(t1.ntransmision, '')) AS 'NÚMERO DE TRANSMISION', UPPER(coalesce(t1.modelo, '')) AS MODELO, UPPER(coalesce(t1.Marca, '')) AS MARCA FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas=t2.idarea WHERE t1.status = 1 ORDER BY ECO DESC", c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            dataGridViewUnidadesTRI.DataSource = dt;
            dataGridViewUnidadesTRI.ClearSelection();
            dataGridViewUnidadesTRI.Columns[0].Frozen = true;
            c.dbconection().Close();
            //try
            //{
            //    //dataGridView1.Rows.Clear();
            //    String sql = @"SELECT t1.idunidad, concat(t2.identificador,LPAD(t1.consecutivo,4,'0')) as ECO, UPPER(COALESCE(t1.bin,'')) as bin,UPPER(COALESCE(t1.nmotor,'')) as nmotor, UPPER(COALESCE(t1.ntransmision,'')) as ntransmision, UPPER(COALESCE(t1.modelo,'')) as modelo, UPPER(COALESCE(t1.Marca,'')) as marca  FROM cunidades AS t1 INNER JOIN careas as t2 ON t1.areafkcareas=t2.idarea WHERE t1.status =1 ORDER BY ECO DESC";
            //    MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            //    MySqlDataReader dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        //dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("bin"), dr.GetString("nmotor"), dr.GetString("ntransmision"), dr.GetString("modelo"), dr.GetString("marca"));
            //    }
            //    dr.Close();
            //    c.dbconection().Close();
            //    //dataGridView1.ClearSelection();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        void iniecos()
        {
            DataTable dt = (DataTable)v.getData("SELECT idunidad, CONCAT(t2.identificador, LPAD(consecutivo, 4, '0')) AS eco FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t1.status = 1");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "-- ECO --".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbeco.DisplayMember = "eco";
            cbeco.ValueMember = "idunidad";
            cbeco.DataSource = dt;
        }

        void restablecer()
        {
            txtgetbin.Clear();
            txtgetnmotor.Clear();
            txtgettransmision.Clear();
            txtgetmodelo.Clear();
            txtgetmarca.Clear();
            binanterior = null;
            nmotoranterior = null;
            ntransmisionAnterior = null;
            modeloAnterior = null;
            idUnidadTemp = null;
            if (pconsultar)
            {
                bunidades();
            }
            gbECO.Enabled = false;
        }

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

        void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
        {
            cbx.DataSource = null;
            DataTable dt = (DataTable)v.getData(sql);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila[ValueMember] = 0;
            nuevaFila[DisplayMember] = TextoInicial.ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbx.DisplayMember = DisplayMember;
            cbx.ValueMember = ValueMember;
            cbx.DataSource = dt;
        }

        void getCambios(object sender,EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(txtgetbin.Text) && binanterior != txtgetbin.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgetnmotor.Text.Trim()) && nmotoranterior!=txtgetnmotor.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgettransmision.Text) && ntransmisionAnterior!=txtgettransmision.Text.Trim()) ||(!string.IsNullOrWhiteSpace(txtgetmodelo.Text) && modeloAnterior !=v.mayusculas(txtgetmodelo.Text.ToLower().Trim())) || (!string.IsNullOrWhiteSpace(txtgetmarca.Text) && marcaAnterior!= v.mayusculas(txtgetmarca.Text.ToLower().Trim())))
            { 

                buttonGuardar.Visible = label9.Visible = true;
            }
            else
            {
                buttonGuardar.Visible = label9.Visible = false;
            }
        }

        void mostrar()
        {
            if (pconsultar)
            {
                //gbUnidades.Visible = true;
                //gbbuscar.Visible = true;
            }
            if (peditar)
            {
                label12.Visible = true;
                label11.Visible = true;
            }
        }

        private string idUnidadTemp
        {
            get;
            set;
        }

        public catUnidaesTRI(int idUsuario, Image logo,int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            //cbeco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            //dataGridView1.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
        }

        Thread hiloEx;

        delegate void Loading();

        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            buttonActualizar.Visible = false;
            label28.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Location = new Point(356, 324);
            groupBoxBusqueda.Enabled = false;
            dataGridViewUnidadesTRI.Enabled = false;
        }

        delegate void Loading1();

        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            buttonExcel.Visible = true;
            buttonActualizar.Visible = true;
            label28.Visible = true;
            label35.Text = "EXPORTAR";
            label35.Location = new Point(380, 324);
            groupBoxBusqueda.Enabled = true;
            dataGridViewUnidadesTRI.Enabled = true;
        }

        public void exporta_a_excel()
        {
            if(dataGridViewUnidadesTRI.Rows.Count > 0)
            {
                if(this.InvokeRequired)
                {
                    Loading load = new Loading(cargando1);
                    this.Invoke(load);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                int ColumnIndex = 0;
                foreach(DataGridViewColumn col in dataGridViewUnidadesTRI.Columns)
                {
                    ColumnIndex++;
                    X.Cells[1, ColumnIndex] = col.HeaderText;
                    X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;   
                    X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    X.Cells[1, ColumnIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Crimson);
                    X.Cells[1, ColumnIndex].Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                    X.Cells[1, ColumnIndex].Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                }

                for (int i = 0; i <= dataGridViewUnidadesTRI.RowCount - 1; i++)
                {
                    for (int j = 0; j <= dataGridViewUnidadesTRI.ColumnCount - 1; j++)
                    {
                        if (dataGridViewUnidadesTRI.Columns[j].Visible == true)
                        {
                            try
                            {
                                h.Worksheet sheet = X.ActiveSheet;
                                h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                                if(j == 3)
                                {
                                    rng.NumberFormat = "0";
                                    sheet.Cells[i + 2, j + 1] = dataGridViewUnidadesTRI[j, i].Value;
                                }
                                else
                                {
                                    sheet.Cells[i + 2, j + 1] = dataGridViewUnidadesTRI[j, i].Value;
                                }
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(200, 200, 200));
                                rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
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

                //exportacionexcel();

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
     
        // Acciones con datagridview's ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                if (peditar)
                {
                    idUnidadTemp = dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[0].Value.ToString();
                    MySqlCommand cmd = new MySqlCommand("SELECT coalesce(bin, '') AS bin, coalesce(nmotor, '') AS nmotor, coalesce(ntransmision, '') AS ntransmision, coalesce(modelo, '') AS modelo, coalesce(Marca, '') AS Marca FROM cunidades WHERE consecutivo = RIGHT('" + idUnidadTemp +"', 2)", c.dbconection());
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if(dr.Read())
                    {
                        txtgetbin.Text = this.binanterior = Convert.ToString(dr.GetString("bin"));
                        txtgetnmotor.Text = this.nmotoranterior = Convert.ToString(dr.GetString("nmotor"));
                        txtgettransmision.Text = this.ntransmisionAnterior = Convert.ToString(dr.GetString("ntransmision"));
                        txtgetmodelo.Text = this.modeloAnterior = Convert.ToString(dr.GetString("modelo"));
                        txtgetmarca.Text = this.marcaAnterior = Convert.ToString(dr.GetString("Marca"));
                    }
                    gbECO.Enabled = true;
                    buttonGuardar.Visible = true;
                    label9.Visible = true;
                    btncancelu.Visible = true;
                    label13.Visible = true;
                    //gbECO.Text = "Especificaciones de ECO: " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    //idUnidadTemp = dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //txtgetbin.Text = this.binanterior = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    //txtgetnmotor.Text = this.nmotoranterior = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    //txtgettransmision.Text = this.ntransmisionAnterior = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    //txtgetmodelo.Text = this.modeloAnterior = v.mayusculas(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString().ToLower());
                    //txtgetmarca.Text = this.marcaAnterior = v.mayusculas(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString().ToLower());
                    //button1.Visible = label9.Visible = false;
                    //gbECO.Enabled = true;
                    gbECO.Focus();
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //string vin = v.mayusculas(txtgetbin.Text.ToLower());
            //string motor = v.mayusculas(txtgetnmotor.Text.ToLower());
            //string trans = v.mayusculas(txtgettransmision.Text.ToLower());
            //string modelo = v.mayusculas(txtgetmodelo.Text.ToLower());
            //string marca = v.mayusculas(txtgetmarca.Text.ToLower());
            //if (!string.IsNullOrWhiteSpace(idUnidadTemp) && (!vin.Equals(binanterior) || !motor.Equals(nmotoranterior) || !trans.Equals(ntransmisionAnterior) || !modelo.Equals(modeloAnterior) || !marca.Equals(marcaAnterior)))
            //{
            //    if (MessageBox.Show("Se Detectaron Modificaciones en los Datos del Económico. ¿Desea Guardar Los Cambios?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        buttonGuardar_Click(null, e);
            //    }
            //    else
            //    {
            //        guardarReporte(e);
            //    }
            //}
            //else
            //{
            //    guardarReporte(e);
            //}
        }

        private void dataGridViewUnidadesTRI_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string vin = txtgetbin.Text;
                string motor = txtgetnmotor.Text;
                string trans = txtgettransmision.Text;
                string modelo = txtgetmodelo.Text;
                string marca = txtgetmarca.Text;
                if (!string.IsNullOrWhiteSpace(idUnidadTemp) && (!vin.Equals(binanterior) || !motor.Equals(nmotoranterior) || !trans.Equals(ntransmisionAnterior) || !modelo.Equals(modeloAnterior) || !marca.Equals(marcaAnterior)))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Datos del Económico. ¿Desea Guardar Los Cambios?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        buttonGuardar_Click(null, e);
                    }
                    else
                    {
                        guardarReporte(e);
                    }
                }
                else
                {
                    guardarReporte(e);
                }
            }         
        }

        // Acciones con botónes ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                object eco = cbeco.SelectedValue;
                string vin = txtgetbinBusq.Text;
                string motor = txtgetnmotorbusq.Text;
                string trans = txtgettransmisionbusq.Text;
                string modelo = txtgetmodelobusq.Text;
                string marca = txtgetmarcaBusq.Text;

                restablecer();
                //dataGridView1.Rows.Clear();
                string wheres = "";
                String sql = @"SELECT t1.idunidad AS 'ECO TRI', concat(t2.identificador,LPAD(t1.consecutivo,4,'0')) as ECO, UPPER(COALESCE(t1.bin,'')) as VIN, UPPER(COALESCE(t1.nmotor,'')) as 'NÚMERO DE MOTOR', UPPER(COALESCE(t1.ntransmision,'')) as 'NÚMERO DE TRANSMISIÓN', UPPER(COALESCE(t1.modelo,'')) as MODELO, UPPER(COALESCE(t1.Marca,'')) as MARCA  FROM cunidades AS t1 INNER JOIN careas as t2 ON t1.areafkcareas=t2.idarea WHERE t1.status = 1 ";
                if (csetEmpresa.DataSource!=null &&  csetEmpresa.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres = "AND ((SELECT idempresa FROM careas as t11 INNER JOIN cempresas as t22 ON t11.empresafkcempresas = t22.idempresa WHERE t11.idarea=t2.idarea) = '" + csetEmpresa.SelectedValue + "' ";
                    }
                    else
                    {
                        wheres += "AND (SELECT idempresa FROM careas as t11 INNER JOIN cempresas as t22 ON t11.empresafkcempresas = t22.idempresa WHERE t11.idarea=t2.idarea) = '" + csetEmpresa.SelectedValue + "' ";
                    }
                }
                if (csetarea.DataSource!=null && csetarea.SelectedIndex>0)
                {
                    if (wheres=="")
                    {
                        wheres = "AND (t2.idarea= '"+csetarea.SelectedValue+"'";
                    }else
                    {
                        wheres += "AND t2.idarea = '"+csetarea.SelectedValue+"'";
                    }
                }
                if (Convert.ToInt32(eco) > 0)
                {
                    if (wheres == "")
                    {
                        wheres = "AND (t1.idUnidad = '" + eco + "%'";
                    }
                    else
                    {
                        wheres += " AND t1.idUnidad = '" + eco + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(vin))
                {
                    if (wheres=="")
                    {
                        wheres = "AND (bin LIKE '"+vin+"%'";
                    }else
                    {
                        wheres += " AND bin LIKE '" + vin + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(motor))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (nmotor LIKE '" + motor + "%'";
                    }
                    else
                    {
                        wheres += " AND nmotor LIKE '" + motor + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(trans))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (ntransmision LIKE '" + trans + "%'";
                    }
                    else
                    {
                        wheres += " AND ntransmision LIKE '" + trans + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(modelo))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (modelo LIKE '" + modelo + "%'";
                    }
                    else
                    {
                        wheres += " AND modelo LIKE '" + modelo + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(marca))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (marca LIKE '" + marca + "%'";
                    }
                    else
                    { 
                        wheres += " AND  marca LIKE '" + marca + "%'";
                    }
                }
                if (wheres!="")
                {
                    wheres += ")";
                }
                sql += wheres + " ORDER BY ECO DESC";

                MySqlDataAdapter cm = new MySqlDataAdapter(sql, c.dbconection());
                DataSet ds = new DataSet();
                cm.Fill(ds);
                dataGridViewUnidadesTRI.DataSource = ds.Tables[0];
                if(ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bunidades();
                }
                //MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE, co.dbconection());
                //DataSet ds = new DataSet();
                //adp.Fill(ds);
                //dataGridViewMantenimiento.DataSource = ds.Tables[0];
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    MessageBox.Show("No se encontraron reportes", "ADVERTEMCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    limpiarcamposbus();
                //    metodoCarga();
                //    conteo();
                //}
                //else
                //{

                //    conteovariable();
                //    limpiarcamposbus();
                //}


                //var res = cm.ExecuteScalar();
                //if (Convert.ToInt32(res) > 0)
                //{
                //    MySqlDataReader dr = cm.ExecuteReader();
                //    while (dr.Read())
                //    {
                //        //dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("bin"), dr.GetString("nmotor"), dr.GetString("ntransmision"), dr.GetString("modelo"), dr.GetString("marca"));
                //    }
                //    dr.Close();
                //    c.dbconection().Close();
                //    //dataGridView1.ClearSelection();
                //    ////pActualizar.Visible = true;
                //}
                //else
                //{
                //    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    bunidades();
                //}
                cbeco.SelectedValue = 0;
                txtgetbinBusq.Clear();
                txtgetnmotorbusq.Clear();
                csetarea.SelectedValue = 0;
                txtgettransmisionbusq.Clear();
                txtgetmodelobusq.Clear();
                csetEmpresa.SelectedIndex = 0;
                txtgetmarcaBusq.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            bunidades();
            pActualizar.Visible = false;
        }

        private void btncancelu_Click(object sender, EventArgs e)
        {
            string vin = txtgetbin.Text;
            string motor = txtgetnmotor.Text;
            string trans = txtgettransmision.Text;
            string modelo = txtgetmodelo.Text;
            string marca = txtgetmarca.Text;
            //string vin = v.mayusculas(txtgetbin.Text.ToLower());
            //string motor = v.mayusculas(txtgetnmotor.Text.ToLower());
            //string trans = v.mayusculas(txtgettransmision.Text.ToLower());
            //string modelo = v.mayusculas(txtgetmodelo.Text.ToLower());
            //string marca = v.mayusculas(txtgetmarca.Text.ToLower());
            if (!vin.Equals(binanterior) || !motor.Equals(nmotoranterior) || !trans.Equals(ntransmisionAnterior) || !modelo.Equals(modeloAnterior) || !marca.Equals(marcaAnterior))
            {
                if (MessageBox.Show("Se Detectaron Modificaciones en los Datos del Económico. ¿Desea Guardar Los Cambios?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    buttonGuardar_Click(null, e);
                }
                else
                {
                    restablecer();
                }
            }
            else
            {
                restablecer();
            }
            buttonGuardar.Visible = false;
            label9.Visible = false;
            btncancelu.Visible = false;
            label13.Visible = false;
        }

        private void buttonExcel_Click(object sender, EventArgs e)
        {
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string vin = txtgetbin.Text;
                string motor = txtgetnmotor.Text;
                string trans = txtgettransmision.Text;
                string modelo = txtgetmodelo.Text;
                string marca = txtgetmarca.Text;
                //string motor = v.mayusculas(txtgetnmotor.Text.ToLower());
                //string trans = v.mayusculas(txtgettransmision.Text.ToLower());
                //string modelo = v.mayusculas(txtgetmodelo.Text.ToLower());
                //string marca = v.mayusculas(txtgetmarca.Text.ToLower());
                if (!v.formularioUnidadesTRI(vin, motor, trans, modelo, marca) && !v.existeUnidadTRI(vin, this.binanterior, motor, this.nmotoranterior, trans, this.ntransmisionAnterior, modelo, this.modeloAnterior, marca, this.marcaAnterior))
                {
                    if (v.mayusculas(vin).Equals(this.binanterior) && v.mayusculas(motor).Equals(this.nmotoranterior) && v.mayusculas(trans).Equals(this.ntransmisionAnterior) && v.mayusculas(modeloAnterior).Equals(modelo) && v.mayusculas(marcaAnterior).Equals(marca))
                    {
                        MessageBox.Show("No se hicieron Modificaciones", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Todos los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                    else
                    {
                        c.insertar("UPDATE cunidades SET bin=LTRIM(RTRIM('" + v.mayusculas(vin) + "')), nmotor=LTRIM(RTRIM('" + v.mayusculas(motor) + "')) ,ntransmision=LTRIM(RTRIM('" + v.mayusculas(trans) + "')), modelo=LTRIM(RTRIM('" + v.mayusculas(modelo) + "')), Marca=LTRIM(RTRIM('" + v.mayusculas(marca) + "')) WHERE consecutivo = RIGHT('" + this.idUnidadTemp + "', 2)");
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades', RIGHT('" + idUnidadTemp + "', 2),'" + binanterior + ";" + nmotoranterior + ";" + ntransmisionAnterior + ";" + modeloAnterior + ";" + marcaAnterior + "','" + idUsuario + "',NOW(),'Actualización de Unidad','" + empresa + "','" + area + "')");
                        restablecer();
                        MessageBox.Show("Especificaciones Guardadas", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    buttonGuardar.Visible = false;
                    label9.Visible = false;
                    btncancelu.Visible = false;
                    label13.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validaciones de combobox ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void csetEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (csetEmpresa.SelectedIndex>0)
            {
                if (Convert.ToInt32(v.getaData("SELECT COUNT(t1.idarea) FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t2.idempresa='" + csetEmpresa.SelectedValue + "' AND t1.status=1 ")) > 0)
                {
                    iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.NombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t2.idempresa='" + csetEmpresa.SelectedValue + "' AND t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--Seleccione un Área--");
                    if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea INNER JOIN cempresas as t3 ON t2.empresafkcempresas=t3.idempresa WHERE t1.status =1 AND t3.idempresa='" + csetEmpresa.SelectedValue + "'")) > 0)
                    {
                        iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea INNER JOIN cempresas as t3 ON t2.empresafkcempresas=t3.idempresa  WHERE t1.status =1 AND t3.idempresa='" + csetEmpresa.SelectedValue + "'", cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                    }
                    else
                    {
                        MessageBox.Show("No Hay Económicos Registrados Con El Área Seleccionada", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        csetarea.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("No Hay Areas Con la Empresa Seleccionada", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    csetEmpresa.SelectedIndex = 0;
                }
            }
            else
            {
                iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.nombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--Seleccione un Área--");
            }
        }

        private void csetarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (csetarea.SelectedIndex>0)
            {
                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea='" + csetarea.SelectedValue + "'"))>0) {
                    iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea='" + csetarea.SelectedValue + "'", cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                }else
                {
                    MessageBox.Show("No Hay Económicos Registrados Con El Área Seleccionada",validaciones.MessageBoxTitle.Advertencia.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    csetarea.SelectedIndex = 0;
                }
                } else
            {
                iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1", cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
            }
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        // Validaciones de las cajas de texto ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void txtgetbinBusq_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetnmotor_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgetbin_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtgetecoBusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void txtgetmarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bunidades();
        }

        // Diseño de los botones y group box del formulario //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void cbeco_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void gbbuscar_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void gbECO_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        
        private void buttonBuscar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonBuscar.Size = new Size(57, 47);
        }

        private void buttonBuscar_MouseLeave(object sender, EventArgs e)
        {
            buttonBuscar.Size = new Size(52, 42);
        }

        private void buttonActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonActualizar.Size = new Size(68, 62);
        }

        private void buttonActualizar_MouseLeave(object sender, EventArgs e)
        {
            buttonActualizar.Size = new Size(63, 57);
        }

        private void buttonExcel_MouseMove(object sender, MouseEventArgs e)
        {
            buttonExcel.Size = new Size(63, 63);
        }

        private void buttonExcel_MouseLeave(object sender, EventArgs e)
        {
           buttonExcel.Size = new Size(58, 58);
        }

        private void buttonGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonGuardar.Size = new Size(68, 62);
        }

        private void buttonGuardar_MouseLeave(object sender, EventArgs e)
        {
            buttonGuardar.Size = new Size(63, 57);
        }

        private void btncancelu_MouseMove(object sender, MouseEventArgs e)
        {
            btncancelu.Size = new Size(63, 63);
        }

        private void btncancelu_MouseLeave(object sender, EventArgs e)
        {
            btncancelu.Size = new Size(58, 58);
        }

        private void dataGridViewUnidadesTRI_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}

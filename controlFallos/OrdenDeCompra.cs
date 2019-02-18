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
using System.Globalization;

namespace controlFallos
{
    public partial class OrdenDeCompra : Form
    {
        conexion co = new conexion();
        int cont, idfolio, numrefacc, folioOC, nref;
        String fentrega = "", codref, pf, estatus, obsref = "", comref = "", observOC = "", obseref, fact, factid, fc, proveeid, provee, pr; 
        double cantref, valorlbl, CS, PC, CSol, PC2, CSSuma, stotal, resultado, iva, resultadototal;
        int c, c2, cont4, a, aa, iddetOC, totalref, edt,idUsuario,empresa,area;
        double ivas;
        bool bedit = false;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        public OrdenDeCompra(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            comboBoxProveedor.MouseWheel += new MouseEventHandler(comboBoxProveedor_MouseWheel);
            comboBoxClave.MouseWheel += new MouseEventHandler(comboBoxClave_MouseWheel);
            comboBoxFacturar.MouseWheel += new MouseEventHandler(comboBoxFacturar_MouseWheel);
            comboBoxProveedorB.MouseWheel += new MouseEventHandler(comboBoxProveedorB_MouseWheel);
            comboBoxEmpresaB.MouseWheel += new MouseEventHandler(comboBoxEmpresaB_MouseWheel);
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }

        private void OrdenDeCompra_Load(object sender, EventArgs e)
        {
            textBoxIVA.Enabled = false;
            timer1.Start();
            CargarProveedores();
            CargarProveedoresBusqueda();
            CargarEmpresas();
            CargarEmpresasBusqueda();
            CargarClave();
            cargafolio();
            metodocargaorden();
            metodocargadetordenPDF();
            conteoiniref();
            dateTimePickerFechaEntrega.Value = DateTime.Now;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
            privilegios();
            metodocargaiva();

            AutoCompletado(textBoxOCompraB);
            if(pconsultar == true)
            {
                buttonExcel.Visible = true;
                label38.Visible = true;
            }
            else
            {
                buttonExcel.Visible = false;
                label38.Visible = false;
            }

            if((pinsertar == true) && (peditar == true) && (pconsultar == true) && (pdesactivar == true))
            {
                label62.Visible = true;
                label63.Visible = true;
                label60.Visible = true;
                label61.Visible = true;
            }
            else
            {
                label62.Visible = false;
                label63.Visible = false;
                label60.Visible = false;
                label61.Visible = false;
            }

            if(checkBoxFechas.Checked == false)
            {
                checkBoxFechas.ForeColor = checkBoxFechas.Checked ? Color.Crimson : Color.Crimson;
            }

            if (checkBoxFechasE.Checked == false)
            {
                checkBoxFechasE.ForeColor = checkBoxFechasE.Checked ? Color.Crimson : Color.Crimson;
            }
        }

        public void limpiarRefacc()
        {
            comboBoxClave.SelectedIndex = 0;
            labelDescripcion.Text = "";
            labelExistencia.Text = "";
            textBoxCSolicitada.Text = "0";
            textBoxPCotizado.Text = "0";
            labelSubTotal.Text = "0";
            textBoxObservacionesRefacc.Text = "";
        }

        public void limpiarRefaccN()
        {
            comboBoxProveedor.SelectedIndex = 0;
            labelCorreo.Text = "";
            labelRepresentante.Text = "";
            comboBoxClave.SelectedIndex = 0;
            labelDescripcion.Text = "";
            labelExistencia.Text = "0";
            textBoxCSolicitada.Clear();
            textBoxPCotizado.Clear();
            labelSubTotal.Text = "0";
            comboBoxFacturar.SelectedIndex = 0;
            textBoxObservaciones.Text = "";
            textBoxObservacionesRefacc.Text = "";
            dateTimePickerFechaEntrega.Value = DateTime.Now;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
            textBoxIVA.Enabled = false;
        }

        public void limpiarRefaccB()
        {
            textBoxOCompraB.Text = "";
            comboBoxProveedorB.SelectedIndex = 0;
            comboBoxEmpresaB.SelectedIndex = 0;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
        }
        bool pverEmpresa;

        /* Todos los métodos */////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                g.Clear(f.BackColor);
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public void textBox_TextChanged(object sender, EventArgs e)
        {
            if (bedit == true)
            {
                string pr = "";
                if (comboBoxProveedor.SelectedIndex == 0)
                {
                    pr = "0";
                }
                else
                {
                    pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                }
                if (comboBoxFacturar.SelectedIndex == 0)
                {
                    fc = "0";
                }
                else
                {
                    fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                }
                if ((((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((observOC == textBoxObservaciones.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))) && (dateTimePickerFechaEntrega.Value.Date == dateTimePicker1.Value.Date)))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        public void exportacionexcel()
        {
            int contador = 0;
            string Folio, id;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra','0','";
            foreach (DataGridViewRow row in dataGridViewOCompra.Rows)
            {
                contador++;
                id = row.Cells[0].Value.ToString();
                Folio = getaData("SELECT t1.idOrdCompra FROM ordencompra AS t1 WHERE '" + id + "' = t1.FolioOrdCompra").ToString();
                if (contador < dataGridViewOCompra.RowCount)
                {
                    Folio += ";";
                }
                sql += Folio;
            }
            sql += "','"+idUsuario+"',now(),'Exportación a Excel de ordenes de compra','2','2')";
            MySqlCommand exportacion = new MySqlCommand(sql, co.dbconection());
            exportacion.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void exportacionpdf()
        {
            metodorecid();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra', '" + idfolio + "', 'Exportación de orden de compra en archivo pdf', '"+idUsuario+"', NOW(), 'Exportación a PDF de orden de compra de almacen', '2', '2')", co.dbconection());
            cmd.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void privilegios()
        {
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal = '"+idUsuario+"' and namform = 'ordencompra'";
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
            MySqlCommand cm = new MySqlCommand(sql, co.dbconection());
            var res = cm.ExecuteScalar();
            co.dbconection();
            return res;
        }

        public void AutoCompletado(TextBox cajaTexto) //Metodo De AutoCompletado
        {
            AutoCompleteStringCollection nColl = new AutoCompleteStringCollection();
            MySqlCommand cmd = new MySqlCommand("SELECT FolioOrdCompra AS Folio FROM ordencompra", co.dbconection());
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
            textBoxOCompraB.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxOCompraB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxOCompraB.AutoCompleteCustomSource = nColl;
        }

        public void cargafolio()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(SUBSTRING(FolioOrdCompra, LENGTH(FolioOrdCompra) -6, 7)+1) AS FolioOC FROM ordencompra WHERE idOrdCompra = (SELECT MAX(idOrdCompra) FROM ordencompra)", co.dbconection());
            string Folio = (string)cmd.ExecuteScalar();
            if(Folio == null)
            {
                Folio = "0000001";
            }
            else
            {
                while(Folio.Length < 7)
                {
                    Folio = "0"+Folio;
                }
            }
            labelFolioOC.Text = "OC-"+Folio.ToString();
            co.dbconection().Close();
        }
        validaciones v = new validaciones();
        public void metodorecid()
        {
            try
            {
                string res = v.getaData("SELECT idOrdCompra FROM ordencompra WHERE FolioOrdCompra = '" + labelFolioOC.Text + "'").ToString() ?? "";         
                idfolio = Convert.ToInt32(res);
                labelidFolioOC.Text = res;
            }
            catch
            {
                idfolio = 0;
                labelidFolioOC.Text = Convert.ToString(idfolio);
            }
        }

        public void metodocargaiva()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT iva FROM civa ", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                textBoxIVA.Text = Convert.ToString(dr.GetString("iva"));   
            }
            dr.Close();
            co.dbconection().Close();
        }

        public void metodocargadetorden() 
        {
            DataTable dt = (DataTable) v.getData("SELECT t1.NumRefacc AS '# De Refaccion', t2.codrefaccion AS 'Codigo De Refaccion', t2.nombreRefaccion AS 'Nombre De La Refaccion', t2.existencias AS Existencia, t1.Cantidad, t1.Precio, t1.Total FROM detallesordencompra AS t1 INNER JOIN crefacciones AS t2 ON t1.ClavefkCRefacciones = t2.idrefaccion WHERE OrdfkOrdenCompra = '" + idfolio + "'");
            dataGridViewPedOCompra.DataSource = dt;
        }

        public void metodocargadetordenPDF() //Metodo Para Cargar Los Datos De Las Refacciones
        {
            DataTable dt = (DataTable) v.getData("SELECT t1.NumRefacc AS PARTIDA, UPPER(t2.codrefaccion) AS CLAVE, UPPER(t2.nombreRefaccion) AS DESCRIPCION, t2.existencias AS 'CANTIDAD EN EXISTENCIA', t1.Cantidad AS 'CANTIDAD SOLICITADA', t1.Precio AS 'PRECIO COTIZADO', t1.TOTAL, coalesce((UPPER(t1.ObservacionesRefacc)), '') AS OBSERVACIONES FROM detallesordencompra AS t1 INNER JOIN crefacciones AS t2 ON t1.ClavefkCRefacciones = t2.idrefaccion INNER JOIN ordencompra AS t3 ON t1.OrdfkOrdenCompra = t3.idOrdCompra WHERE t3.FolioOrdCompra = '" + labelFolioOC.Text + "' AND t1.OrdfkOrdenCompra = '" + labelidFolioOC.Text + "'");
            dataGridViewPedOCompra.DataSource = dt;
            co.dbconection().Close();
        }
       
        public void metodocargaorden()
        {
            DataTable dt =(DataTable) v.getData("SET lc_time_names = 'es_ES'; SELECT t1.FolioOrdCompra AS 'ORDEN DE COMPRA', UPPER(CONCAT(t2.aPaterno, ' ', t2.aMaterno, ' ', t2.nombres)) AS PROVEEDOR, UPPER(t3.nombreEmpresa) AS 'NOMBRE DE LA EMPRESA', UPPER(DATE_FORMAT(t1.FechaOCompra, '%W %d %M %Y')) AS 'FECHA', UPPER(DATE_FORMAT(t1.FechaEntregaOCompra, '%W %d %M %Y')) AS 'FECHA DE ENTREGA', t1.SUBTOTAL,cast(((t1.IVA/100)*t1.Subtotal) as decimal (5,2)) as IVA, t1.TOTAL, coalesce((UPPER(t1.ESTATUS)), '') AS ESTATUS, coalesce((SELECT UPPER(CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres)) FROM cpersonal AS t4 WHERE t1.PersonaFinal = t4.idPersona), '') AS 'PERSONA FINAL', UPPER(t1.ObservacionesOC) AS 'OBSERVACIONES' FROM ordencompra AS t1 LEFT JOIN cproveedores AS t2 ON t1.ProveedorfkCProveedores = t2.idproveedor INNER JOIN cempresas AS t3 ON t1.FacturadafkCEmpresas = t3.idempresa ORDER BY t1.FolioOrdCompra DESC");
            dataGridViewOCompra.DataSource = dt;
            co.dbconection().Close();
        }

        public void conteoiniref() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            string res = v.getaData("SELECT COUNT(NumRefacc) AS Numero FROM detallesordencompra").ToString();
            if (res!=null)
            {
                labelrefini.Text = Convert.ToString(res);
            }
        }

        public void conteofinref() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
           string res = v.getaData("SELECT COUNT(NumRefacc) AS Numero FROM detallesordencompra").ToString();
         
            if (res!=null)
                labelreffin.Text = res;
        }

        public void CargarClave()
        {   
            DataTable dt =(DataTable) v.getData("SELECT codrefaccion, idrefaccion FROM crefacciones WHERE status = 1 ORDER BY codrefaccion");
            DataRow row2 = dt.NewRow();
            row2["idrefaccion"] = 0;
            row2["codrefaccion"] = " -- SELECCIONE UNA OPCIÓN --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxClave.ValueMember = "idrefaccion";
            comboBoxClave.DisplayMember = "codrefaccion";
            comboBoxClave.DataSource = dt;
            comboBoxClave.SelectedIndex = 0;
        }
        int res;
        public void CargarEmpresas()
        {
            DataTable dt = (DataTable) v.getData("SELECT UPPER(nombreEmpresa) AS nombreEmpresa, idempresa FROM cempresas WHERE status = 1 order by nombreEmpresa");
            DataRow row2 = dt.NewRow();
            row2["idempresa"] = 0;
            row2["nombreEmpresa"] = " - SELECCIONE UNA EMPRESA--";
            dt.Rows.InsertAt(row2, 0);
            pverEmpresa = v.getBoolFromInt(Convert.ToInt32( v.getaData("SELECT Coalesce(ver,'0') FROM privilegios WHERE namform='catEmpresas' and usuariofkcpersonal='" + idUsuario + "'")));
            if (v.getIntFrombool(pverEmpresa)==1) 
            {
                row2 = dt.NewRow();
                row2["idempresa"] = res = Convert.ToInt32(v.getaData("SELECT coalesce(MAX(idempresa), '0') AS idempresa FROM cempresas WHERE status = 1")) + 1;
                row2["nombreEmpresa"] = "OTRA EMPRESA";
                dt.Rows.InsertAt(row2, dt.Rows.Count);
            }
            comboBoxFacturar.ValueMember = "idempresa";
            comboBoxFacturar.DisplayMember = "nombreEmpresa";       
            comboBoxFacturar.DataSource = dt;
            comboBoxFacturar.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public void CargarEmpresasBusqueda()
        {        
            DataTable dt =(DataTable) v.getData("SELECT UPPER(nombreEmpresa) AS nombreEmpresa, idempresa FROM cempresas order by nombreEmpresa");
            DataRow row2 = dt.NewRow();
            row2["idempresa"] = 0;
            row2["nombreEmpresa"] = " -- SELECCIONE UNA OPCIÓN --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxEmpresaB.ValueMember = "idempresa";
            comboBoxEmpresaB.DisplayMember = "nombreEmpresa";
            comboBoxEmpresaB.DataSource = dt;
            comboBoxEmpresaB.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public void CargarProveedores()
        {
            DataTable dt = (DataTable) v.getData("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 1 order by empresa");      
            DataRow row2 = dt.NewRow();
            row2["idproveedor"] = 0;
            row2["empresa"] = " -- SELECCIONE UNA OPCIÓN --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxProveedor.ValueMember = "idproveedor";
            comboBoxProveedor.DisplayMember = "empresa";
            comboBoxProveedor.DataSource = dt;
            comboBoxProveedor.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public void CargarProveedoresBusqueda()
        {
            DataTable dt = (DataTable) v.getData("SELECT  UPPER(empresa) AS empresa, idproveedor FROM cproveedores order by empresa");
            DataRow row2 = dt.NewRow();
            row2["idproveedor"] = 0;
            row2["empresa"] = " -- SELECCIONE UNA OPCIÓN --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxProveedorB.ValueMember = "idproveedor";
            comboBoxProveedorB.DisplayMember = "empresa";
            comboBoxProveedorB.DataSource = dt;
            comboBoxProveedorB.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public bool metodotxtpedcompra() // checar
        {
            if (dataGridViewPedOCompra.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void llamarorden()
        {
            privilegios();
            //limpiarRefaccN();
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonPDF.Visible = true;
            label3.Visible = true;
            buttonEditar.Visible = false;
            label37.Visible = false;
            buttonNuevoOC.Visible = true;
            label17.Visible = true;
            buttonFinalizar.Visible = false;
            label34.Visible = false;
            folioOC = 0;
            estatus = "";
            edt = 0;
            labelFolioOC.Text = dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value.ToString();
            metodorecid();
            pf = dataGridViewOCompra.CurrentRow.Cells["PERSONA FINAL"].Value.ToString();
            estatus = dataGridViewOCompra.CurrentRow.Cells["ESTATUS"].Value.ToString();
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idOrdCompra, t1.FechaEntregaOCompra, coalesce((t1.Subtotal), '0') AS Subtotal, coalesce((t1.IVA), '0') AS IVA, coalesce((t1.Total), '0') AS Total, coalesce(upper((t1.ObservacionesOC)), '') AS Observaciones, UPPER(t2.nombreEmpresa) AS NEmpresa, t1.FacturadafkCEmpresas AS EFacturar, UPPER(t3.empresa) AS Proveedor, t1.ProveedorfkCProveedores AS NProveedores, SUM(t4.Total) AS TotalCS FROM ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN cproveedores AS t3 ON t1.ProveedorfkCProveedores = t3.idproveedor INNER JOIN detallesordencompra AS t4 ON t1.idOrdCompra = t4.OrdfkOrdenCompra WHERE t1.FolioOrdCompra =  '" + labelFolioOC.Text + "'", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                folioOC = Convert.ToInt32(dr.GetString("idOrdCompra"));
                dateTimePickerFechaEntrega.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                dateTimePicker1.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                labelSubTotalOC.Text = Convert.ToString(dr.GetString("Subtotal"));
                if(pf.Equals(""))
                {
                    metodocargaiva();
                }
                else
                {
                    textBoxIVA.Text = Convert.ToString(dr["IVA"]).ToString();
                }
                labelTotalOC.Text = Convert.ToString(dr.GetString("Total"));
                textBoxObservaciones.Text = Convert.ToString(dr.GetString("Observaciones"));
                observOC = Convert.ToString(dr.GetString("Observaciones"));
                comboBoxProveedor.Text = Convert.ToString(dr.GetString("Proveedor"));
                provee = Convert.ToString(dr.GetString("Proveedor"));
                proveeid = Convert.ToString(dr.GetString("NProveedores"));
                comboBoxFacturar.Text = Convert.ToString(dr.GetString("NEmpresa"));
                fact = Convert.ToString(dr.GetString("NEmpresa"));
                factid = Convert.ToString(dr.GetString("EFacturar"));
                CSSuma = Convert.ToDouble(dr.GetString("TotalCS"));
                //comboBoxFacturar.Text = dataGridViewOCompra.CurrentRow.Cells[2].Value.ToString();
                //fact = dataGridViewOCompra.CurrentRow.Cells[2].Value.ToString();
                textBoxIVA.Enabled = false;
            }
            dr.Close();
            co.dbconection().Close();
            MySqlCommand cmd1 = new MySqlCommand("select UPPER (t1.nombreEmpresa) as nombreEmpresa, t1.idempresa from cempresas as t1 inner join ordencompra as t2 on t1.idempresa=t2.FacturadafkCEmpresas where t2.FolioOrdCompra='" + dataGridViewOCompra.CurrentRow.Cells[0].Value.ToString() + "' and t1.status='0'",co.dbconection());
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                comboBoxFacturar.DataSource = null;
                MySqlCommand comando = new MySqlCommand("SELECT UPPER(nombreEmpresa) AS nombreEmpresa, idempresa FROM cempresas where status='1' order by nombreEmpresa", co.dbconection());
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow row2 = dt.NewRow();
                DataRow row = dt.NewRow();
                row["idempresa"] = 0;
                row["nombreEmpresa"] = "-- EMPRESA --";
                row2["idempresa"] = dr1["idempresa"];
                row2["nombreEmpresa"] = dr1["nombreEmpresa"].ToString();
                dt.Rows.InsertAt(row2, 1);
                dt.Rows.InsertAt(row, 0);
                comboBoxFacturar.ValueMember = "idempresa";
                comboBoxFacturar.DisplayMember = "nombreEmpresa";
                comboBoxFacturar.DataSource = dt;
                comboBoxFacturar.SelectedIndex = 1;
                comboBoxFacturar.Text = dr1["nombreEmpresa"].ToString();
            }
            dr1.Close();
            co.dbconection().Close();
            MySqlCommand proveedor = new MySqlCommand("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 0 and upper(concat(aPaterno,' ',aMaterno,' ',nombres))='"+dataGridViewOCompra.CurrentRow.Cells[1].Value.ToString()+"'",co.dbconection());
            MySqlDataReader dr2 = proveedor.ExecuteReader();
            if (dr2.Read())
            {   
                DataTable dt = (DataTable) v.getData("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 1 order by empresa");
                DataRow row2 = dt.NewRow();
                DataRow row3 = dt.NewRow();
                row2["idproveedor"] = 0;
                row2["empresa"] = " -- PROVEEDOR --";
                row3["idproveedor"] = dr2["idproveedor"];
                row3["empresa"] = dr2["empresa"].ToString();
                dt.Rows.InsertAt(row3, 1);
                dt.Rows.InsertAt(row2, 0);
                comboBoxProveedor.ValueMember = "idproveedor";
                comboBoxProveedor.DisplayMember = "empresa";
                comboBoxProveedor.DataSource = dt;
                comboBoxProveedor.SelectedIndex = 1;
                comboBoxProveedor.Text = dr2["empresa"].ToString();
                co.dbconection().Close();
            }
            dr2.Close();
            co.dbconection().Close();
            if (pf != "")
            {
                buttonFinalizar.Visible = false;
                label34.Visible = false;
                buttonAgregar.Visible = false;
                label9.Visible = false;
                textBoxCSolicitada.Enabled = false;
                textBoxPCotizado.Enabled = false;
                comboBoxClave.Enabled = false;
                dateTimePickerFechaEntrega.Enabled = false;
                buttonExcel.Visible = false;
                label38.Visible = false;
                textBoxObservaciones.Enabled = false;
                textBoxObservacionesRefacc.Enabled = false;
                if (pconsultar == true)
                {
                    buttonPDF.Visible = true;
                    label3.Visible = true;
                }
                else
                {
                    buttonPDF.Visible = false;
                    label3.Visible = false;
                }
            }
            else
            {
                buttonFinalizar.Visible = true;
                label34.Visible = true;
                buttonAgregar.Visible = true;
                label9.Visible = true;
                textBoxCSolicitada.Enabled = true;
                textBoxPCotizado.Enabled = true;
                comboBoxClave.Enabled = true;
                buttonExcel.Visible = false;
                label38.Visible = false;
                if (pconsultar == true)
                {
                    buttonPDF.Visible = true;
                    label3.Visible = true;
                }
                else
                {
                    buttonPDF.Visible = false;
                    label3.Visible = false;
                }
                if (peditar == true)
                {
                    edt = 1;
                }
                else
                {
                    edt = 0;
                }
                dateTimePickerFechaEntrega.Enabled = true;
            }
            metodorecid();
            metodocargadetordenPDF();
            resultado = CS * PC;
            labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
            labelSubTotalOC.Text = CSSuma.ToString("00.00", CultureInfo.InvariantCulture);
            double i = Convert.ToDouble(textBoxIVA.Text);
            iva = CSSuma * (i/100);
            labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
            resultadototal = CSSuma + iva;
            labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
       }

        public void llamarref()
        {
            buttonEditar.Visible = true;
            label8.Visible = true;
            buttonAgregarMas.Visible = true;
            label29.Visible = true;
            buttonPDF.Visible = false;
            label3.Visible = false;
            buttonNuevoOC.Visible = false;
            label17.Visible = false;
            buttonAgregar.Visible = false;
            label9.Visible = false;
        }
        Thread hiloEx2;

        delegate void Loading();
        public void carga1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            buttonAgregar.Visible = false;
            label9.Visible = false;
            label38.Text = "EXPORTANDO";
            label38.Location = new Point(1081, 546);
            groupBoxBusqueda.Enabled = false;
            dataGridViewOCompra.Enabled = false;
            comboBoxProveedor.Enabled = false;
            comboBoxClave.Enabled = false;
            comboBoxFacturar.Enabled = false;
            dateTimePickerFechaEntrega.Enabled = false;
            textBoxIVA.Enabled = false;
            comboBoxEmpresaB.Enabled = false;
        }

        delegate void Loading1();
        public void carga2()
        {
            pictureBoxExcelLoad.Image = null;
            buttonExcel.Visible = true;
            buttonAgregar.Visible = true;
            label9.Visible = true;
            label38.Text = "EXPORTAR";
            label38.Location = new Point(1099, 546);
            groupBoxBusqueda.Enabled = true;
            dataGridViewOCompra.Enabled = true;
            comboBoxProveedor.Enabled = true;
            comboBoxClave.Enabled = true;
            comboBoxFacturar.Enabled = true;
            dateTimePickerFechaEntrega.Enabled = true;
            comboBoxEmpresaB.Enabled = true;
        }

        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            if (dataGridViewOCompra.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    Loading load = new Loading(carga1);
                    this.Invoke(load);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                int ColumnIndex = 0;
                foreach (DataGridViewColumn col in dataGridViewOCompra.Columns)
                {
                    ColumnIndex++;
                    X.Cells[1, ColumnIndex] = col.HeaderText;
                    X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    X.Cells[1, ColumnIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Crimson);
                    X.Cells[1, ColumnIndex].Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                    X.Cells[1, ColumnIndex].Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                }

                for (int i = 0; i <= dataGridViewOCompra.RowCount - 1; i++)
                {
                    for (int j = 0; j <= dataGridViewOCompra.ColumnCount - 1; j++)
                    {
                        if (dataGridViewOCompra.Columns[j].Visible == true)
                        {
                            try
                            {
                                h.Worksheet sheet = X.ActiveSheet;
                                h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                                sheet.Cells[i + 2, j + 1] = dataGridViewOCompra[j, i].Value.ToString();
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(200, 200, 200));
                                rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                if(dataGridViewOCompra[j, i].Value.ToString() == "FINALIZADA".ToString())
                                {
                                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                }
                            }
                            catch (Exception)
                            {
                                hiloEx2.Abort();
                            }
                        }
                    }
                }
                Thread.Sleep(10);
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                exportacionexcel();
                if(this.InvokeRequired)
                {
                    Loading1 load1 = new Loading1(carga2);
                    this.Invoke(load1);
                }
            }
            else
            {
                MessageBox.Show("Es necesario que existan datos en la tabla para poder generar un archivo de excel \n Favor de actualizar la tabla para que existan reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            hiloEx2.Abort();
        }

        public void To_pdf()
        {
            fentrega = dateTimePickerFechaEntrega.Value.ToString("dd/MM/yyyy");
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 95;
            table.LockedWidth = true;
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Desktop";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "Reporte";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    string p = Path.GetExtension(filename);
                    if (p.ToLower() != ".pdf")
                    {
                        filename = filename + ".pdf";
                    }
                }
                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    PdfWriter writer = PdfWriter.GetInstance(doc, file);
                    doc.Open();
                    Chunk chunk = new Chunk("REPORTE DE MANTENIMIENTO", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    var res = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa='"+factid+"'").ToString();
                    byte[] img;
                    if (res == "") {

                        img = Convert.FromBase64String(v.tri);
                    }
                    else
                    {
                        System.Drawing.Image temp = v.StringToImage2(res);
                        temp = v.CambiarTamanoImagen(temp,622,261);
                        img = Convert.FromBase64String( v.SerializarImg(temp));
                    }
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);

                    byte[] img1 = Convert.FromBase64String(v.autobuses);
                    iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(img1);
                    //byte[] imagen3 = Convert.FromBase64String(img2);

                    imagen.ScalePercent(10f);
                   imagen2.ScalePercent(10f);
                    imagen.SetAbsolutePosition(65f, 707f/*600f, 500f*/);
                   imagen2.SetAbsolutePosition(360f, 704f);                   
                   imagen.Alignment = Element.ALIGN_CENTER;
                   imagen2.Alignment = Element.ALIGN_CENTER;
                    float percentage = 0.0f;
                   float percentage1 = 0.0f;
                    percentage = 150 / imagen.Width;

                    imagen.ScalePercent(percentage * 80);
                   imagen2.ScalePercent(percentage1 * 50);
                   imagen2.ScaleAbsolute(190f, 53.5f);
                    doc.Add(imagen);
                   doc.Add(imagen2);
                    PdfPTable tb0 = new PdfPTable(1);
                    tb0.DefaultCell.Border = 1;
                    tb0.WidthPercentage = 95;
                    tb0.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell c01 = new PdfPCell();
                    c01.HorizontalAlignment = Element.ALIGN_CENTER;
                    c01.Border = 5;
                    c01.BorderColorLeft = BaseColor.BLACK;
                    c01.BorderColorTop = BaseColor.BLACK;
                    c01.BorderColorBottom = BaseColor.BLACK;
                    c01.BorderColorRight = BaseColor.BLACK;
                    c01.BorderWidthLeft = 2f;
                    c01.BorderWidthRight = 2f;
                    c01.BorderWidthTop = 2f;
                    c01.BorderWidthBottom = 2f;
                    Phrase Espacio = new Phrase("\n\n\n");
                    c01.AddElement(Espacio);
                    tb0.AddCell(c01);
                    doc.Add(tb0);
                    doc.Add(new Paragraph("                                                       ORDEN DE COMPRA                                                        ", FontFactory.GetFont("ARIAL", 13, iTextSharp.text.Font.BOLD)));
                    PdfPTable tb2 = new PdfPTable(6);
                    tb2.WidthPercentage = 95;
                    tb2.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell c00 = new PdfPCell(new Phrase(""));
                    c00.UseAscender = true;
                    c00.Border = 2;
                    c00.BorderColorTop = BaseColor.WHITE;
                    c00.BorderColorLeft = BaseColor.WHITE;
                    c00.BorderColorRight = BaseColor.WHITE;
                    c00.BorderColorBottom = BaseColor.WHITE;
                    c00.HorizontalAlignment = Element.ALIGN_CENTER;
                    c00.Colspan = 6;
                    tb2.AddCell(c00);
                    PdfPCell c11 = new PdfPCell(new Phrase("Carretera Federal México-Pachuca Km. 26.5 Lte. 2 Col. Venta de Carpio, Ecatepec, Estado De México CP: 55060", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c11.UseAscender = false;
                    c11.UseDescender = true;
                    c11.HorizontalAlignment = Element.ALIGN_CENTER;
                    c11.VerticalAlignment = Element.ALIGN_CENTER;
                    c11.Rowspan = 2;
                    c11.Colspan = 3;
                    tb2.AddCell(c11);
                    PdfPCell c12 = new PdfPCell(new Phrase(labelFolioOC.Text, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD)));
                    c12.UseAscender = true;
                    c12.UseDescender = false;
                    c12.HorizontalAlignment = Element.ALIGN_CENTER;
                    c12.Colspan = 3;
                    tb2.AddCell(c12);
                    PdfPCell c13 = new PdfPCell(new Phrase("Fecha Elaborada:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c13.UseAscender = true;
                    c13.UseDescender = false;
                    c13.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c13);

                    PdfPCell c14 = new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    c14.Colspan = 2;
                    c14.UseAscender = true;
                    c14.UseDescender = false;
                    c14.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c14);

                    PdfPCell c15 = new PdfPCell(new Phrase("Proveedor:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c15.UseAscender = false;
                    c15.UseDescender = true;
                    c15.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c15);

                    PdfPCell c16 = new PdfPCell(new Phrase(comboBoxProveedor.Text, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    c16.Colspan = 2;
                    c16.UseAscender = false;
                    c16.UseDescender = true;
                    c16.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c16);

                    PdfPCell c17 = new PdfPCell(new Phrase("Fecha De Entrega:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c17.UseAscender = false;
                    c17.UseDescender = true;
                    c17.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c17);

                    PdfPCell c18 = new PdfPCell(new Phrase(fentrega.ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    c18.Colspan = 2;
                    c18.UseAscender = false;
                    c18.UseDescender = true;
                    c18.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c18);

                    PdfPCell c19 = new PdfPCell(new Phrase("Correo:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c19.UseAscender = false;
                    c19.UseDescender = true;
                    c19.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c19);

                    PdfPCell c20 = new PdfPCell(new Phrase("almacen@tribuses.com", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    c20.Colspan = 2;
                    c20.UseAscender = false;
                    c00.UseDescender = true;
                    c20.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c20);

                    PdfPCell c21 = new PdfPCell(new Phrase("Facturar A:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                    c21.UseAscender = false;
                    c21.UseDescender = true;
                    c21.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c21);

                    PdfPCell c22 = new PdfPCell(new Phrase(comboBoxFacturar.Text, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    c22.Colspan = 2;
                    c22.UseAscender = false;
                    c22.UseDescender = true;
                    c22.HorizontalAlignment = Element.ALIGN_CENTER;
                    tb2.AddCell(c22);

                    doc.Add(tb2);

                    PdfPTable tb3 = new PdfPTable(16);
                    tb3.WidthPercentage = 95;
                    tb3.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell c31 = new PdfPCell(new Phrase("PARTIDA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c31.UseDescender = true;
                    c31.HorizontalAlignment = Element.ALIGN_CENTER;
                    c31.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c31);

                    PdfPCell c32 = new PdfPCell(new Phrase("CLAVE", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c32.Colspan = 2;
                    c32.UseDescender = true;
                    c32.HorizontalAlignment = Element.ALIGN_CENTER;
                    c32.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c32);

                    PdfPCell c33 = new PdfPCell(new Phrase("DESCRIPCIÓN", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c33.UseDescender = true;
                    c33.Colspan = 4;
                    c33.HorizontalAlignment = Element.ALIGN_CENTER;
                    c33.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c33);

                    PdfPCell c34 = new PdfPCell(new Phrase("CANTIDAD EN EXISTENCIA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c34.UseAscender = true;
                    c34.UseDescender = false;
                    c34.HorizontalAlignment = Element.ALIGN_CENTER;
                    c34.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c34);

                    PdfPCell c35 = new PdfPCell(new Phrase("CANTIDAD SOLICITADA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c35.UseAscender = true;
                    c35.UseDescender = false;
                    c35.HorizontalAlignment = Element.ALIGN_CENTER;
                    c35.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c35);

                    PdfPCell c36 = new PdfPCell(new Phrase("PRECIO COTIZADO", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c36.UseAscender = true;
                    c36.UseDescender = false;
                    c36.HorizontalAlignment = Element.ALIGN_CENTER;
                    c36.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c36);

                    PdfPCell c37 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c37.UseDescender = true;
                    c37.Colspan = 3;
                    c37.HorizontalAlignment = Element.ALIGN_CENTER;
                    c37.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c37);

                    PdfPCell c38 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                    c38.UseDescender = true;
                    c38.Colspan = 3;
                    c38.HorizontalAlignment = Element.ALIGN_CENTER;
                    c38.BackgroundColor = new BaseColor(234, 231, 231);
                    tb3.AddCell(c38);

                    string cantidad, cod, DescRef, existenciasA, cSOlicitada, pre_cotizado, total_Ref;

                    MySqlCommand maximo = new MySqlCommand("select max(t1.NumRefacc) as tam from detallesordencompra as t1 inner join ordencompra as t2 on t1.OrdfkOrdenCompra=t2.idOrdCompra  where t2.FolioOrdCompra='" + labelFolioOC.Text + "' ", co.dbconection());
                    MySqlDataReader DR1 = maximo.ExecuteReader();

                    MySqlCommand almacenar_datos = new MySqlCommand("select  t1.NumRefacc as partida, t3.codrefaccion as codigo ,t3.nombreRefaccion as nombre,t3.existencias as existencias_almacen, t1.Cantidad as canti_soli, t1.Precio as precio_cot, t1.Total as Tot_ref from detallesordencompra as t1 inner join ordencompra as t2 on t1.OrdfkOrdenCompra=t2.idOrdCompra inner join crefacciones as t3 on t1.ClavefkCRefacciones=t3.idrefaccion where  t2.FolioOrdCompra='" + labelFolioOC.Text + "'", co.dbconection());
                    MySqlDataReader DR = almacenar_datos.ExecuteReader();
                    int i = 0;
                    while (DR.Read())
                    {
                        cantidad = Convert.ToString(DR["Partida"]);
                        cod = Convert.ToString(DR["codigo"]);
                        DescRef = Convert.ToString(DR["nombre"]);
                        existenciasA = Convert.ToString(DR["existencias_almacen"]);
                        cSOlicitada = Convert.ToString(DR["canti_soli"]);
                        pre_cotizado = Convert.ToString(DR["precio_cot"]);
                        total_Ref = Convert.ToString(DR["Tot_ref"]);
                        totalref = Convert.ToInt32(DR.GetString("Partida"));

                        PdfPCell c1 = new PdfPCell(new Phrase(cantidad, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c1.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c1);

                        PdfPCell c2 = new PdfPCell(new Phrase(cod, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c2.Colspan = 2;
                        c2.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c2);

                        PdfPCell c3 = new PdfPCell(new Phrase(DescRef, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c3.Colspan = 4;
                        c3.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c3);

                        PdfPCell c4 = new PdfPCell(new Phrase(existenciasA, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c4.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c4);

                        PdfPCell c5 = new PdfPCell(new Phrase(cSOlicitada, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c5.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c5);

                        PdfPCell c6 = new PdfPCell(new Phrase("$  " + pre_cotizado, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c6.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c6);

                        PdfPCell c7 = new PdfPCell(new Phrase("$  " + total_Ref, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c7.Colspan = 3;
                        c7.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c7);

                        if (DR1.Read())
                        {

                            totalref = Convert.ToInt32(DR1["tam"]);
                            string obs = "";
                            for(int inn = 1; inn <= totalref; inn ++)
                            {
                                obsref = v.getaData("SELECT ObservacionesRefacc FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "' AND NumRefacc = '" + inn + "'").ToString();                                                             
                                if(obsref != "")
                                {
                                    obs += "PARTIDA " + inn + "- " + obsref +"\n \n";
                                }
                                if (inn == totalref)
                                {
                                    obs += textBoxObservaciones.Text + "\n \n";
                                }
                            }
                            PdfPCell c39 = new PdfPCell(new Phrase(obs, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c39.Colspan = 3;
                            c39.Rowspan = 54;
                            c39.HorizontalAlignment = Element.ALIGN_CENTER;
                            c39.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tb3.AddCell(c39);
                        }
                        i++;
                    }
                    DR.Close();
                    co.dbconection().Close();

                    while (i < 54)
                    {
                        PdfPCell c1 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c1.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c1);

                        PdfPCell c2 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c2.Colspan = 2;
                        c2.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c2);

                        PdfPCell c3 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c3.Colspan = 4;
                        c3.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c3);

                        PdfPCell c4 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c4.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c4);

                        PdfPCell c5 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c5.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c5);

                        PdfPCell c6 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c6.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c6);

                        PdfPCell c7 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                        c7.Colspan = 3;
                        c7.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb3.AddCell(c7);
                        i++;
                    }
                    DR1.Close();

                    doc.Add(tb3);
                }

                PdfPTable tablefot = new PdfPTable(6);
                tablefot.WidthPercentage = 95;
                tablefot.TotalWidth = 541f;
                tablefot.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cf01 = new PdfPCell(new Phrase("SUBTOTAL:", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf01.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf01);

                PdfPCell cf02 = new PdfPCell(new Phrase("$ " + labelSubTotalOC.Text, FontFactory.GetFont("ARIAl", 8, iTextSharp.text.Font.NORMAL)));
                cf02.UseDescender = true;
                cf02.HorizontalAlignment = Element.ALIGN_LEFT;
                tablefot.AddCell(cf02);

                PdfPCell cf03 = new PdfPCell(new Phrase("IVA ("+textBoxIVA.Text+"%):", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf03.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf03);

                PdfPCell cf04 = new PdfPCell(new Phrase("$ " + labelIVAOC.Text, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.NORMAL)));
                cf04.UseDescender = true;
                cf04.HorizontalAlignment = Element.ALIGN_LEFT;
                tablefot.AddCell(cf04);

                PdfPCell cf05 = new PdfPCell(new Phrase("TOTAL:", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf05.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf05);

                PdfPCell cf06 = new PdfPCell(new Phrase("$ " + labelTotalOC.Text, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.NORMAL)));
                cf06.UseDescender = true;
                cf06.HorizontalAlignment = Element.ALIGN_LEFT;
                tablefot.AddCell(cf06);

                PdfPCell cf11 = new PdfPCell(new Phrase("\n\n RAFAEL JUAREZ", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf11.Colspan = 3;
                cf11.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf11);

                PdfPCell cf12 = new PdfPCell(new Phrase("\n\n ADAIR SALAZAR", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf12.Colspan = 3;
                cf12.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf12);

                PdfPCell cf13 = new PdfPCell(new Phrase("ALMACÉN TRI", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf13.Colspan = 3;
                cf13.UseAscender = true;
                cf13.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf13);

                PdfPCell cf14 = new PdfPCell(new Phrase("AUTORIZA", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                cf14.Colspan = 3;
                cf14.UseAscender = true;
                cf14.HorizontalAlignment = Element.ALIGN_CENTER;
                tablefot.AddCell(cf14);

                doc.Add(tablefot);

                doc.AddCreationDate();
                doc.Close();
                exportacionpdf();           
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* Acciones con los botones y gridview *////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonPDF_Click(object sender, EventArgs e)
        {
            To_pdf();
        }

        string observaciones;
        private void dataGridViewOCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (comboBoxProveedor.SelectedIndex == 0)
                    {
                        pr = "0";
                    }
                    else
                    {
                        pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                    }
                    if (comboBoxFacturar.SelectedIndex == 0)
                    {
                        fc = "0";
                    }
                    else
                    {
                        fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                    }
                    if (((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((textBoxObservaciones.Text.Trim() == observOC) || (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))))
                    {
                        limpiarRefaccN();
                        llamarorden();
                        metodocargadetordenPDF();
                        dataGridViewPedOCompra.Enabled = true;
                        if (comboBoxProveedor.Text != "")
                        {
                            comboBoxProveedor.Enabled = false;
                        }
                        if (comboBoxFacturar.Text != "")
                        {
                            comboBoxFacturar.Enabled = false;
                        }
                        if (textBoxObservaciones.Text != "")
                        {
                            textBoxObservaciones.Enabled = false;
                        }
                        buttonActualizar.Visible = false;
                        label37.Visible = false;
                        dateTimePickerFechaEntrega.Enabled = false;
                        observaciones = Convert.ToString(dataGridViewOCompra.CurrentRow.Cells[9].Value);
                    }
                    else
                    {
                        if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            CS = 0;
                            PC = 0;
                            llamarorden();
                            metodocargadetordenPDF();
                            limpiarRefaccN();
                            labelSubTotal.Text = "00.00";
                            dataGridViewPedOCompra.Enabled = true;
                            if (comboBoxProveedor.Text != "")
                            {
                                comboBoxProveedor.Enabled = false;
                            }
                            if (comboBoxFacturar.Text != "")
                            {
                                comboBoxFacturar.Enabled = false;
                            }
                            if (textBoxObservaciones.Text != "")
                            {
                                textBoxObservaciones.Enabled = false;
                            }
                            buttonActualizar.Visible = false;
                            label8.Visible = true;
                            buttonEditar.Visible = true;
                            label37.Visible = false;
                            dateTimePickerFechaEntrega.Enabled = false;
                            observaciones = Convert.ToString(dataGridViewOCompra.CurrentRow.Cells[9].Value);
                            dataGridViewPedOCompra.ClearSelection();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewPedOCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            estatus = getaData("SELECT Estatus FROM ordencompra WHERE FolioOrdCompra='" + labelFolioOC.Text + "'").ToString();
            folioOC = Convert.ToInt32(getaData("SELECT idOrdCompra FROM ordencompra WHERE FolioOrdCompra='" + labelFolioOC.Text + "'").ToString());
            if (estatus.ToString().Equals("FINALIZADA"))
            {
                MessageBox.Show("Ya no puede editar refacciones cuando la orden ya esta finalizada", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (/*(edt == 1) ||*/((pinsertar == true) && (peditar == true) && (pconsultar == true) && (pdesactivar == true)))
                {
                cont4 = 0;
                limpiarRefacc();
                buttonNuevoOC.Visible = false;
                label17.Visible = false;
                buttonAgregar.Visible = false;
                label9.Visible = false;
                buttonAgregarMas.Visible = true;
                label29.Visible = true;
                buttonEditar.Visible = true;
                label8.Visible = true;
                buttonPDF.Visible = false;
                label3.Visible = false;
                buttonFinalizar.Visible = false;
                label34.Visible = false;
                labelIVAOC.Visible = false;
                labelTotalOC.Visible = false;
                labelSubTotalOC.Visible = false;
                Limpia_variables();
                double sub = 0;
                foreach (DataGridViewRow row in dataGridViewPedOCompra.Rows)
                {
                    sub += Convert.ToDouble(row.Cells[6].Value.ToString());
                }
                numrefacc = Convert.ToInt32(dataGridViewPedOCompra.CurrentRow.Cells["PARTIDA"].Value.ToString());
                MySqlCommand cmd = new MySqlCommand("SELECT t1.idDetOrdCompra, t2.codrefaccion, t1.Cantidad, t1.Precio, t1.Total, t1.ObservacionesRefacc FROM detallesordencompra AS t1 INNER JOIN crefacciones AS t2 ON t1.ClavefkCRefacciones = t2.idrefaccion WHERE OrdfkOrdenCompra = '" + folioOC + "' AND NumRefacc = '" + numrefacc + "'", co.dbconection());
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    iddetOC = Convert.ToInt32(dr.GetString("idDetOrdCompra"));
                    comboBoxClave.Text = Convert.ToString(dr.GetString("codrefaccion"));
                    codref = Convert.ToString(dr.GetString("codrefaccion"));
                    CS = Convert.ToDouble(dr.GetString("Cantidad"));
                    CSol = Convert.ToDouble(dr.GetString("Cantidad"));
                    textBoxCSolicitada.Text = Convert.ToString(dr.GetString("Cantidad"));
                    PC = Convert.ToDouble(dr.GetString("Precio"));
                    PC2 = Convert.ToDouble(dr.GetString("Precio"));
                    textBoxPCotizado.Text = Convert.ToString(dr.GetString("Precio"));
                    labelSubTotal.Text = Convert.ToString(dr.GetString("Total"));
                    stotal = Convert.ToDouble(dr.GetString("Total"));
                    labelSubTotalOC.Text = sub.ToString();
                    textBoxObservacionesRefacc.Text = Convert.ToString(dr.GetString("ObservacionesRefacc"));
                    obseref = Convert.ToString(dr.GetString("ObservacionesRefacc"));
                }
                dr.Close();
                co.dbconection().Close();
                MySqlCommand cmd1 = new MySqlCommand("select UPPER (t1.codrefaccion) as codrefaccion, t1.idrefaccion from crefacciones as t1 inner join detallesordencompra as t2 on t1.idrefaccion=t2.ClavefkCRefacciones where t1.codrefaccion='" + dataGridViewPedOCompra.CurrentRow.Cells[1].Value.ToString() + "' and t1.status='0'", co.dbconection());
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    comboBoxClave.DataSource = null;
                    MySqlCommand comando = new MySqlCommand("SELECT codrefaccion, idrefaccion FROM crefacciones WHERE status = 1 ORDER BY codrefaccion", co.dbconection());
                    MySqlDataAdapter da = new MySqlDataAdapter(comando);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataRow row2 = dt.NewRow();
                    DataRow row = dt.NewRow();
                    row["idrefaccion"] = 0;
                    row["codrefaccion"] = "--SELECCIONE UNA OPCIÓN--";
                    row2["idrefaccion"] = dr1["idrefaccion"];
                    row2["codrefaccion"] = dr1["codrefaccion"].ToString();
                    dt.Rows.InsertAt(row2, 1);
                    dt.Rows.InsertAt(row, 0);
                    comboBoxClave.ValueMember = "idrefaccion";
                    comboBoxClave.DisplayMember = "codrefaccion";
                    comboBoxClave.DataSource = dt;
                    comboBoxClave.SelectedIndex = 1;
                    comboBoxClave.Text = dr1["codrefaccion"].ToString();
                }
                dr1.Close();
            }
            textBoxCSolicitada.Text = CS.ToString();
            textBoxPCotizado.Text = PC.ToString();
            metodocargaiva();
            co.dbconection().Close();
          }
        }

        void Limpia_variables()
        {
            CSSuma = 0;
            CS = 0;
            ivas = 0;
            PC = 0;
            PC2 = 0;
            iva = 0;
            resultado = 0;
            resultadototal = 0;
            valorlbl = 0;
            cantref = 0;
            valorlbl = 0;
            CS = 0;
            PC = 0;
            CSol = 0;
            PC2 = 0;
            CSSuma = 0; stotal = 0;
            resultado = 0; iva = 0;
            resultadototal = 0;
            c = 0;
            c2 = 0;
            cont4 = 0; a = 0;
            aa = 0; iddetOC = 0; totalref = 0; edt = 0;
        }

        private void buttonNuevoOC_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxProveedor.SelectedIndex == 0)
                {
                    pr = "0";
                }
                else
                {
                    pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                }
                if (comboBoxFacturar.SelectedIndex == 0)
                {
                    fc = "0";
                }
                else
                {
                    fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                }
                bedit = false;
                if (((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((textBoxObservaciones.Text.Trim() == observOC) || (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))))
                {
              
                    limpiarRefaccN();
                    cargafolio();
                    conteoiniref();
                    metodocargadetordenPDF();
                    comboBoxProveedor.Enabled = true;
                    comboBoxFacturar.Enabled = true;
                    textBoxObservaciones.Enabled = true;
                    buttonNuevoOC.Visible = false;
                    label17.Visible = false;
                    buttonPDF.Visible = false;
                    label3.Visible = false;
                    if (peditar == true)
                    {
                        buttonExcel.Visible = true;
                        label38.Visible = true;
                    }
                    else
                    {
                        buttonExcel.Visible = false;
                        label38.Visible = false;
                    }
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                    buttonAgregarMas.Visible = false;
                    label29.Visible = false;
                    buttonAgregar.Visible = true;
                    label9.Visible = true;
                    textBoxObservaciones.Visible = true;
                    buttonFinalizar.Visible = false;
                    label34.Visible = false;
                    dataGridViewPedOCompra.Enabled = false;
                    a = 0;
                    aa = 0;
                    observOC = "";
                    obsref = "";
                    Limpiar_labels();
                    textBoxCSolicitada.Enabled = true;
                    textBoxPCotizado.Enabled = true;
                    comboBoxFacturar.SelectedIndex = 0;
                    dataGridViewOCompra.ClearSelection();
                    Limpia_variables();
                    comboBoxClave.Enabled = true;
                    CargarClave();
                    CargarEmpresas();
                    CargarProveedores();
                    dateTimePickerFechaEntrega.Enabled = true;
                    textBoxObservacionesRefacc.Enabled = true;
                    metodocargaiva();
                }
                else
                {
                    if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        limpiarRefaccN();
                        cargafolio();
                        conteoiniref();
                        metodocargaorden();
                        metodocargadetordenPDF();
                        comboBoxProveedor.Enabled = true;
                        comboBoxFacturar.Enabled = true;
                        textBoxObservaciones.Enabled = true;
                        buttonNuevoOC.Visible = false;
                        label17.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonExcel.Visible = true;
                        label38.Visible = true;
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                        buttonAgregarMas.Visible = false;
                        label29.Visible = false;
                        buttonAgregar.Visible = true;
                        label9.Visible = true;
                        textBoxObservaciones.Visible = true;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        dataGridViewPedOCompra.Enabled = false;
                        a = 0;
                        aa = 0;
                        observOC = "";
                        obsref = "";
                        Limpiar_labels();
                        textBoxCSolicitada.Enabled = true;
                        textBoxPCotizado.Enabled = true;
                        comboBoxFacturar.SelectedIndex = 0;
                        dataGridViewOCompra.ClearSelection();
                        Limpia_variables();
                        comboBoxClave.Enabled = true;
                        CargarClave();
                        CargarEmpresas();
                        CargarProveedores();
                        dateTimePickerFechaEntrega.Enabled = true;
                        textBoxObservacionesRefacc.Enabled = true;
                        metodocargaiva();      
                    }
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }
        }

        string Iv;
        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(textBoxIVA.Text))
                {
                    Iv = textBoxIVA.Text;
                }
                if ((comboBoxProveedor.SelectedIndex == 0) && (comboBoxClave.SelectedIndex == 0) && (comboBoxFacturar.SelectedIndex == 0) && (CS == 0) && (PC == 0))
                {
                    MessageBox.Show("Tiene que llenar todos los campos para añadir una refacción", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DateTime actual = DateTime.Now.Date.AddMonths(4);
                    if (comboBoxProveedor.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione un proveedor", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePickerFechaEntrega.Value.Date < DateTime.Now.Date)
                    {
                        MessageBox.Show("La fecha de entrega no debe de ser menor a la fecha actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if(dateTimePickerFechaEntrega.Value.Date > actual)
                    {
                        MessageBox.Show("La fecha de entrega no debe superar más de 4 meses", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (comboBoxClave.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione una clave", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (comboBoxFacturar.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione una empresa para facturar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (PC <= 0.20)
                    {
                        MessageBox.Show("El precio cotizada debe de ser mayor a 0.20", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (CS <= 0.20)
                    {
                        MessageBox.Show("La cantidad solicitada debe de ser mayor a 0.20", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    else
                    {
                        metodorecid();

                        MySqlCommand cmd0 = new MySqlCommand("SELECT NumRefacc FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "' ORDER BY idDetOrdCompra DESC ", co.dbconection());
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
                        if (idfolio == 0)
                        {
                            String FEnt = "";
                            FEnt = dateTimePickerFechaEntrega.Value.ToString("yyyy-MM-dd");
                            DataTable dt1 = new DataTable();
                            MySqlCommand cmd1 = new MySqlCommand("INSERT INTO ordencompra(FolioOrdCompra, ProveedorfkCProveedores, FacturadafkCEmpresas, FechaOCompra, FechaEntregaOCompra,IVA) VALUES('" + labelFolioOC.Text + "', '" + comboBoxProveedor.SelectedValue + "', '" + comboBoxFacturar.SelectedValue + "', curdate(), '" + FEnt.ToString() + "','" + Iv + "')", co.dbconection());
                            MySqlDataAdapter adp1 = new MySqlDataAdapter(cmd1);
                            adp1.Fill(dt1);
                            dataGridViewOCompra.DataSource = dt1;
                            co.dbconection().Close();
                            proveeid = Convert.ToString(comboBoxProveedor.SelectedValue);
                            factid = Convert.ToString(comboBoxFacturar.SelectedValue);
                            if(string.IsNullOrWhiteSpace(textBoxObservaciones.Text))
                            {
                                observOC = "";
                            }
                            else
                            {
                                observOC = textBoxObservaciones.Text;
                            }
                            AutoCompletado(textBoxOCompraB);
                        }
                        metodorecid();

                        DataTable dt = new DataTable();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO detallesordencompra(OrdfkOrdenCompra, NumRefacc, ClavefkCRefacciones, Cantidad, Precio, Total, ObservacionesRefacc) VALUES('" + idfolio + "', '" + cont + "', '" + comboBoxClave.SelectedValue + "', '" + CS + "', '" + PC + "', '" + resultado + "', '" + textBoxObservacionesRefacc.Text + "')", co.dbconection());
                        cmd.ExecuteNonQuery();
                        co.dbconection().Close();

                        MySqlCommand cmd3 = new MySqlCommand("SELECT SUM(Total) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "'", co.dbconection());
                        MySqlDataReader dr3 = cmd3.ExecuteReader();
                        if (dr3.Read())
                        {
                            CSSuma = Convert.ToDouble(dr3.GetString("Total"));
                        }
                        dr3.Close();
                        co.dbconection().Close();
                        if (comboBoxProveedor.SelectedIndex != 0)
                        {
                            comboBoxProveedor.Enabled = false;
                        }
                        if (comboBoxFacturar.SelectedIndex != 0)
                        {
                            comboBoxFacturar.Enabled = false;
                        }
                        if (dateTimePickerFechaEntrega.Value != DateTime.Now)
                        {
                            dateTimePickerFechaEntrega.Enabled = false;
                        }
                        buttonNuevoOC.Visible = true;
                        label17.Visible = true;
                        buttonFinalizar.Visible = true;
                        label34.Visible = true;
                        buttonExcel.Visible = false;
                        label38.Visible = false;
                        buttonPDF.Visible = true;
                        label3.Visible = true;
                        dataGridViewPedOCompra.Enabled = true;

                        if (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))
                        {}
                        else
                        {
                            if ((string.IsNullOrWhiteSpace(textBoxObservaciones.Text)) && (textBoxObservacionesRefacc.Text != ""))
                            {
                                if (obsref == "")
                                {
                                    obsref = "Partida " + cont + " " + textBoxObservacionesRefacc.Text;
                                }
                                else
                                {
                                    obsref += ", Partida " + cont + " " + textBoxObservacionesRefacc.Text;
                                }
                                MySqlCommand cmd00 = new MySqlCommand("UPDATE pedidosrefaccion SET ObservacionesRefacc = '" + textBoxObservacionesRefacc.Text + "' WHERE OrdfkOrdenCompra = '" + idfolio + "' AND NumRefacc = '" + cont + "'", co.dbconection());
                                cmd00.ExecuteNonQuery();
                                co.dbconection().Close();
                                MySqlCommand cmd04 = new MySqlCommand("UPDATE ordencompra SET ObservacionesOC ='" + obsref + "' WHERE idOrdCompra = '" + idfolio + "'", co.dbconection());
                                cmd04.ExecuteNonQuery();
                                co.dbconection().Close();
                            }
                            else if ((textBoxObservaciones.Text != "") && (string.IsNullOrWhiteSpace(textBoxObservacionesRefacc.Text)))
                            {
                                observOC = textBoxObservaciones.Text + "; ";
                                MySqlCommand cmd03 = new MySqlCommand("UPDATE ordencompra SET ObservacionesOC = '" + textBoxObservaciones.Text + "' WHERE idOrdCompra = '" + idfolio + "'", co.dbconection());
                                cmd03.ExecuteNonQuery();
                                co.dbconection().Close();
                            }
                        }
                        a = 0;
                        c2 = 0;
                        aa = aa + 1;
                        limpiarRefacc();
                        metodocargaorden();
                        metodocargadetordenPDF();
                        MessageBox.Show("Refacción agregada correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(textBoxOCompraB.Text)) && (comboBoxProveedorB.Text == " -- SELECCIONE UNA OPCIÓN --") && (comboBoxEmpresaB.Text == " -- SELECCIONE UNA OPCIÓN --") && (checkBoxFechas.Checked == false) && (checkBoxFechasE.Checked == false))
            {
                MessageBox.Show("Los campos de busqueda están vacios", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(dateTimePickerIni.Value > dateTimePickerFin.Value)
            {
                MessageBox.Show("La fecha inicial no debe superar a la fecha final \n Favor de cambiar el rango de fechas", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiarRefaccB();
            }   
            else if(dateTimePickerIniBFE.Value > dateTimePickerFinBFE.Value)
            {
                MessageBox.Show("La fecha de entrega inicial no debe superar a la fecha final \n Favor de cambiar el rango de fechas", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiarRefaccB();
            }
            else
            {
                String Fini = "";
                String Ffin = "";
                String FiniE = "";
                String FfinE = "";
                string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.FolioOrdCompra AS 'Orden De Compra', UPPER(CONCAT(t2.aPaterno, ' ', t2.aMaterno, ' ', t2.nombres)) AS Proveedor, UPPER(t3.nombreEmpresa) AS 'Nombre De La Empresa', UPPER(DATE_FORMAT(t1.FechaOCompra, '%W %d %M %Y')) AS 'Fecha', UPPER(DATE_FORMAT(t1.FechaEntregaOCompra, '%W %d %M %Y')) AS 'Fecha De Entrega', coalesce((t1.Subtotal), '') AS Subtotal, coalesce(IF(((t1.IVA != '') && (t1.Estatus = 'FINALIZADA')), cast(((t1.IVA/100)*t1.Subtotal) as decimal (5,2)), ''), '') AS IVA, coalesce((t1.Total), '') AS Total, coalesce((UPPER(t1.Estatus)), '') AS ESTATUS, coalesce((SELECT UPPER(CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres)) FROM cpersonal AS t4 WHERE t1.PersonaFinal = t4.idPersona), '') AS 'Persona Final', UPPER(t1.ObservacionesOC) AS 'Observaciones' FROM ordencompra AS t1 LEFT JOIN cproveedores AS t2 ON t1.ProveedorfkCProveedores = t2.idproveedor INNER JOIN cempresas AS t3 ON t1.FacturadafkCEmpresas = t3.idempresa";
                string WHERE = "";
                if(!string.IsNullOrWhiteSpace(textBoxOCompraB.Text))
                {
                    if(WHERE == "")
                    {
                        WHERE = " WHERE (t1.FolioOrdCompra = '" + textBoxOCompraB.Text + "')";
                    }
                    else
                    {
                        WHERE += " AND (t1.FolioOrdCompra = '" + textBoxOCompraB.Text + "')";
                    }
                }
                if(comboBoxProveedorB.SelectedIndex > 0)
                {
                    if(WHERE == "")
                    {
                        WHERE = " WHERE (t2.idproveedor = '" + comboBoxProveedorB.SelectedValue + "')";
                    }
                    else
                    {
                        WHERE += " AND (t2.idproveedor = '" + comboBoxProveedorB.SelectedValue + "')";
                    }
                }
                if(comboBoxEmpresaB.SelectedIndex > 0)
                {
                    if(WHERE == "")
                    {
                        WHERE = " WHERE (t3.nombreEmpresa = '" + comboBoxEmpresaB.Text + "')";
                    }
                    else
                    {
                        WHERE += " AND (t3.nombreEmpresa = '" + comboBoxEmpresaB.Text + "')";
                    }
                }
                if(checkBoxFechas.Checked == true)
                {
                    Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                    Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                    if(WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaOCompra BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaOCompra BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                }
                if(checkBoxFechasE.Checked == true)
                {
                    FiniE = dateTimePickerIniBFE.Value.ToString("yyyy-MM-dd");
                    FfinE = dateTimePickerFinBFE.Value.ToString("yyyy-MM-dd");
                    if(WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaEntregaOCompra BETWEEN '" + FiniE.ToString() + "' AND '" + FfinE.ToString() + "')";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaEntregaOCompra BETWEEN '" + FiniE.ToString() + "' AND '" + FfinE.ToString() + "')";
                    }
                }
                if(WHERE != "")
                {
                    WHERE += " ORDER BY t1.FolioOrdCompra DESC";
                }
                MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE, co.dbconection());
                DataSet ds = new DataSet();
                adp.Fill(ds);
                dataGridViewOCompra.DataSource = ds.Tables[0];
                co.dbconection().Close();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    metodocargaorden();
                    limpiarRefaccB();
                }
                else
                {
                    limpiarRefaccB();
                    metodo();
                }

            
                co.dbconection().Close();
            }
        }
        void metodo()
        {
            buttonActualizar.Visible = true;
            label37.Visible = true;
            buttonExcel.Visible = false;
            label38.Visible = false;
            buttonAgregar.Visible = false;
            label9.Visible = false;
            buttonNuevoOC.Visible = false;
            label17.Visible = false;
            checkBoxFechas.Checked = false;
            checkBoxFechasE.Checked = false;

        }
        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            if (comboBoxProveedor.Text == " -- SELECCIONE UNA OPCIÓN --")
            {
                cargafolio();
                buttonActualizar.Visible = false;
                label37.Visible = false;
                buttonNuevoOC.Visible = false;
                label17.Visible = false;
                buttonAgregar.Visible = true;
                label9.Visible = true;
                buttonAgregarMas.Visible = false;
                label29.Visible = false;
                buttonEditar.Visible = false;
                label8.Visible = false;
                buttonFinalizar.Visible = false;
                label34.Visible = false;
                buttonPDF.Visible = false;
                label3.Visible = false;
                buttonExcel.Visible = true;
                label38.Visible = true;
            }
            else
            {
                buttonActualizar.Visible = false;
                label37.Visible = false;
                buttonPDF.Visible = true;
                label3.Visible = true;
                buttonNuevoOC.Visible = true;
                label17.Visible = true;
                buttonAgregar.Visible = true;
                label9.Visible = true;
            }
            metodocargaorden();
            if (comboBoxProveedor.Text == " -- SELECCIONE UNA OPCIÓN --")
            {
                metodocargadetordenPDF();
            }
            aa = 0;
        }

        private void buttonAgregarMas_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd1 = new MySqlCommand("SELECT SUM(Total) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + folioOC + "'", co.dbconection());
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                CSSuma = Convert.ToDouble(dr1.GetString("Total"));
            }
            dr1.Close();
            co.dbconection().Close();
            metodocargaiva();
            buttonActualizar.Visible = false;
            label37.Visible = false;
            buttonNuevoOC.Visible = true;
            label17.Visible = true;
            buttonAgregar.Visible = true;
            label9.Visible = true;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonFinalizar.Visible = true;
            label34.Visible = true;
            buttonPDF.Visible = true;
            label3.Visible = true;
            labelSubTotalOC.Visible = true;
            labelIVAOC.Visible = true;
            labelTotalOC.Visible = true;
            limpiarRefacc();
            CargarClave();
            a = 0;
            c2 = 0;
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if (textBoxCSolicitada.Enabled == false)
            {
                if (dateTimePickerFechaEntrega.Value.Date >= DateTime.Now.Date)
                {
                    string fech = "";
                    string fech2 = "";
                    fech = dateTimePickerFechaEntrega.Value.ToString("yyyy-MM-dd");
                    fech2 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    if (!((provee.Equals(comboBoxProveedor.Text)) && (fact.Equals(comboBoxFacturar.Text)) && (observOC.Equals(textBoxObservaciones.Text)) && (fech2.Equals(fech))))
                    {
                        MySqlCommand cmd0 = new MySqlCommand("UPDATE ordencompra SET ProveedorfkCProveedores = '" + comboBoxProveedor.SelectedValue + "', FacturadafkCEmpresas = '" + comboBoxFacturar.SelectedValue + "', FechaEntregaOCompra = '" + fech + "', ObservacionesOC = '" + textBoxObservaciones.Text + "' WHERE idOrdCompra = '" + folioOC + "'", co.dbconection());
                        cmd0.ExecuteNonQuery();
                        co.dbconection().Close();

                        MySqlCommand cmd00 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra', '" + folioOC + "', CONCAT('" + comboBoxProveedor.SelectedValue + ";', '" + comboBoxFacturar.SelectedValue + ";', '" + dateTimePickerFechaEntrega.Value.ToString("yyyy/MM/dd") + ";', '" + textBoxObservaciones.Text + "'), '" + idUsuario + "', now(), 'Actualización de Orden de Compra', '2', '2')", co.dbconection());
                        cmd00.ExecuteNonQuery();
                        co.dbconection().Close();

                        MessageBox.Show("Orden de Compra editada correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccN();
                        cargafolio();
                        conteoiniref();
                        metodocargaorden();
                        metodocargadetordenPDF();
                        comboBoxProveedor.Enabled = true;
                        comboBoxFacturar.Enabled = true;
                        textBoxObservaciones.Enabled = true;
                        buttonNuevoOC.Visible = false;
                        label17.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonExcel.Visible = true;
                        label38.Visible = true;
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                        buttonAgregarMas.Visible = false;
                        label29.Visible = false;
                        buttonAgregar.Visible = true;
                        label9.Visible = true;
                        buttonActualizar.Visible = false;
                        label37.Visible = false;
                        textBoxObservaciones.Visible = true;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        dataGridViewPedOCompra.Enabled = false;
                        a = 0;
                        aa = 0;
                        observOC = "";
                        obsref = "";
                        Limpiar_labels();
                        textBoxCSolicitada.Enabled = true;
                        textBoxPCotizado.Enabled = true;
                        comboBoxFacturar.SelectedIndex = 0;
                        dataGridViewOCompra.ClearSelection();
                        Limpia_variables();
                        comboBoxClave.Enabled = true;
                        CargarClave();
                        CargarEmpresas();
                        CargarProveedores();
                        dateTimePickerFechaEntrega.Enabled = true;
                        textBoxObservacionesRefacc.Enabled = true;
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Sin cambios", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccN();
                        cargafolio();
                        conteoiniref();
                        metodocargaorden();
                        metodocargadetordenPDF();
                        comboBoxProveedor.Enabled = true;
                        comboBoxFacturar.Enabled = true;
                        textBoxObservaciones.Enabled = true;
                        buttonNuevoOC.Visible = false;
                        label17.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonExcel.Visible = true;
                        label38.Visible = true;
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                        buttonAgregarMas.Visible = false;
                        label29.Visible = false;
                        buttonAgregar.Visible = true;
                        label9.Visible = true;
                        buttonActualizar.Visible = false;
                        label37.Visible = false;
                        textBoxObservaciones.Visible = true;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        dataGridViewPedOCompra.Enabled = false;
                        a = 0;
                        aa = 0;
                        observOC = "";
                        obsref = "";
                        Limpiar_labels();
                        textBoxCSolicitada.Enabled = true;
                        textBoxPCotizado.Enabled = true;
                        comboBoxFacturar.SelectedIndex = 0;
                        dataGridViewOCompra.ClearSelection();
                        Limpia_variables();
                        comboBoxClave.Enabled = true;
                        CargarClave();
                        CargarEmpresas();
                        CargarProveedores();
                        dateTimePickerFechaEntrega.Enabled = true;
                        textBoxObservacionesRefacc.Enabled = true;
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("La fecha no debe de ser menor a la actual, favor de modificar la fecha de entrega", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (textBoxCSolicitada.Enabled == true)
            {
                if (PC <= 0)
                {
                    MessageBox.Show("El precio cotizado debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (CS <=0)
                {
                    MessageBox.Show("La cantidad solicitada debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (comboBoxClave.SelectedIndex == 0)
                {
                    MessageBox.Show("No puede dejar en blanco la clave de la refacción", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if ((comboBoxClave.Text.Equals(codref)) && (CSol.Equals(CS)) && (PC2.Equals(PC)) && (textBoxObservacionesRefacc.Text.Equals(obseref)))
                    {
                        MessageBox.Show("No se realizó ningún cambio en los datos", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        numrefacc = Convert.ToInt32(dataGridViewPedOCompra.CurrentRow.Cells["PARTIDA"].Value.ToString());
                        MySqlCommand cmd = new MySqlCommand("UPDATE detallesordencompra SET ClavefkCRefacciones = '" + comboBoxClave.SelectedValue + "', Cantidad = '" + CS + "', Precio = '" + PC + "', Total = '" + resultado + "', ObservacionesRefacc = '" + textBoxObservacionesRefacc.Text + "' WHERE NumRefacc = '" + numrefacc + "' AND OrdfkOrdenCompra = '" + folioOC + "'", co.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Refacción actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        co.dbconection().Close();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT SUM(Total) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + folioOC + "'", co.dbconection());
                        MySqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.Read())
                        {
                            CSSuma = Convert.ToDouble(dr1.GetString("Total"));
                        }
                        dr1.Close();
                        co.dbconection().Close();
                        MySqlCommand cmd2 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra', '" + folioOC + "', CONCAT('" + comboBoxClave.SelectedValue + ";', '" + textBoxPCotizado.Text + ";', '" + textBoxCSolicitada.Text + ";', '" + textBoxObservacionesRefacc.Text + "'), '" + idUsuario + "', now(), 'Actualización de Refacción de Orden de Compra', '2', '2')", co.dbconection());
                        cmd2.ExecuteNonQuery();
                        co.dbconection().Close();
                    }
                    metodorecid();
                    metodocargadetordenPDF();
                    limpiarRefacc();
                    buttonActualizar.Visible = false;
                    label37.Visible = false;
                    buttonNuevoOC.Visible = true;
                    label17.Visible = true;
                    buttonAgregar.Visible = true;
                    label9.Visible = true;
                    buttonAgregarMas.Visible = false;
                    label29.Visible = false;
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                    buttonFinalizar.Visible = false;
                    label34.Visible = false;
                    buttonPDF.Visible = true;
                    label3.Visible = true;
                    labelSubTotalOC.Visible = true;
                    labelIVAOC.Visible = true;
                    labelTotalOC.Visible = true;
                    comboBoxProveedor.Enabled = false;
                    comboBoxFacturar.Enabled = false;
                    textBoxCSolicitada.Enabled = true;
                    textBoxPCotizado.Enabled = true;
                    a = 0;
                    c2 = 0;
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
            }
        }

        void Limpiar_labels()
        {
            labelSubTotal.Text = "0";
            labelSubTotalOC.Text = "0";
            labelIVAOC.Text = "0";
            labelTotalOC.Text = "0";
            Iv = "";
        }
        private void buttonFinalizar_Click(object sender, EventArgs e)
        {
            FormContraFinal FCF = new FormContraFinal(empresa,area,this);
            if (metodotxtpedcompra() == true)
            {
                MessageBox.Show("Tiene que agregar al menos una refaccion en la Orden De Compra", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FCF.LabelTitulo.Text = "¿Desea Finalizar La Orden De Compra?";
                FCF.LabelTitulo.Left = (FCF.Width - FCF.LabelTitulo.Width) / 2;
                FCF.LabelEncabezado.Left = (FCF.Width - FCF.LabelEncabezado.Width) / 2;

                DialogResult res = FCF.ShowDialog();
                if (res== DialogResult.OK) {
                    if (string.IsNullOrWhiteSpace(labelidFinal.Text))
                    {}
                    else if (!string.IsNullOrWhiteSpace(labelidFinal.Text))
                    {
                        nref = 0;
                        folioOC = Convert.ToInt32(getaData("select idOrdCompra from ordencompra where FolioOrdCompra='" + labelFolioOC.Text + "'").ToString());
                        string sql = "SELECT MAX(NumRefacc) AS NumRefacc, SUM(Total) AS Cantidad FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + folioOC + "'";
                        MySqlCommand cmd1 = new MySqlCommand(sql, co.dbconection());
                        MySqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.Read())
                        {
                            nref = Convert.ToInt32(dr1.GetString("NumRefacc"));
                            cantref = Convert.ToDouble(dr1.GetString("Cantidad"));
                        }
                        dr1.Close();
                        co.dbconection().Close();
                        double resociva = Convert.ToDouble(labelIVAOC.Text);
                        double totaloc = Convert.ToDouble(labelTotalOC.Text);
                        double subt = Convert.ToDouble(labelSubTotalOC.Text);
                        double i_v_a = Convert.ToDouble(getaData("SELECT IVA FROM ordencompra where FolioOrdCompra = '" + labelFolioOC.Text + "'").ToString());
                        resociva = subt * (i_v_a / 100);
                        totaloc = resociva + cantref;
                        labelSubTotalOC.Text = cantref.ToString();
                        labelIVAOC.Text = resociva.ToString();
                        labelTotalOC.Text = totaloc.ToString();
                        metodorecid();
                        MySqlCommand cmd = new MySqlCommand("UPDATE ordencompra SET SubTotal = '" + labelSubTotalOC.Text + "',Total = '" + labelTotalOC.Text + "', Estatus = 'FINALIZADA',  PersonaFinal = '" + FCF.labelidFin.Text + "', ObservacionesOC='" + textBoxObservaciones.Text + "' WHERE FolioOrdCompra = '" + labelFolioOC.Text + "'", co.dbconection());
                        cmd.ExecuteNonQuery();
                        co.dbconection().Close();
                        cargafolio();
                        metodocargaorden();
                        idfolio = 0;
                        metodocargadetorden();
                        MessageBox.Show("La orden de compra ha sido finalizada", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccN();
                        buttonAgregar.Visible = true;
                        label9.Visible = true;
                        if(peditar == true)
                        {
                            buttonExcel.Visible = true;
                            label38.Visible = true;
                        }
                        else
                        {
                            buttonExcel.Visible = false;
                            label38.Visible = false;
                        }
                        buttonNuevoOC.Visible = false;
                        label17.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        comboBoxProveedor.Enabled = true;
                        comboBoxFacturar.Enabled = true;
                        labelExistencia.Text = "";
                        Limpiar_labels();
                    }
                }
            }
        }
        void mostrar_Botones()
        {
            buttonAgregar.Visible = true;
            buttonExcel.Enabled = true;
        }
        private void buttonExcel_Click(object sender, EventArgs e)
        {
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx2 = new Thread(excel);
            hiloEx2.Start();
        }
        /*Validaciones extras */////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void comboBoxProveedor_SelectedValueChanged(object sender, EventArgs e)
        {
            MySqlCommand validar = new MySqlCommand("SELECT correo, UPPER(CONCAT(aPaterno, ' ', aMaterno, ' ', nombres)) AS representante, coalesce((paginaweb), '') AS paginaweb FROM cproveedores WHERE idproveedor = '" + comboBoxProveedor.SelectedValue + "'", co.dbconection());
            MySqlDataReader leer = validar.ExecuteReader();
            if (leer.Read())
            {
                labelCorreo.Text = leer["correo"].ToString();
                labelRepresentante.Text = leer["representante"].ToString();
                labelpaginaweb.Text = leer["paginaweb"].ToString();
            }
            else
            {
                if (!leer.Read())
                {
                    labelCorreo.Text = "";
                    labelRepresentante.Text = "";
                    labelpaginaweb.Text = "";
                }
            }
            leer.Close();
            co.dbconection().Close();

            if (bedit == true)
            {
                if (comboBoxProveedor.SelectedIndex == 0)
                {
                    pr = "0";
                }
                else
                {
                    pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                }
                if(comboBoxFacturar.SelectedIndex == 0)
                {
                    fc = "0";
                }
                else
                {
                    fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                }
                if ((((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && observOC == textBoxObservaciones.Text.Trim() && (dateTimePickerFechaEntrega.Value.Date == dateTimePicker1.Value.Date)))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void comboBoxFacturar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (bedit == true)
            {
                string pr = "";
                if (comboBoxProveedor.SelectedIndex == 0)
                {
                    pr = "0";
                }
                else
                {
                    pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                }
                if (comboBoxFacturar.SelectedIndex == 0)
                {
                    fc = "0";
                }
                else
                {
                    fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                }
                if ((((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((observOC == textBoxObservaciones.Text.Trim()) ) && (dateTimePickerFechaEntrega.Value.Date == dateTimePicker1.Value.Date)))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void textBoxIVA_TextChanged(object sender, EventArgs e)
        {
            if(textBoxIVA.Text=="")
            {
                textBoxIVA.Text = "0";
                textBoxIVA.SelectionStart = textBoxIVA.TextLength; 
            }
            ivas = Convert.ToDouble(textBoxIVA.Text);
            if(!string.IsNullOrWhiteSpace(textBoxCSolicitada.Text) || !String.IsNullOrWhiteSpace(textBoxPCotizado.Text))
            {
                textBoxCSolicitada.Text = textBoxCSolicitada.Text.Replace("..", ".");
                CS = Convert.ToDouble(textBoxCSolicitada.Text);
                ivas = Convert.ToDouble(textBoxIVA.Text);
                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                valorlbl = CSSuma + resultado;
                labelSubTotalOC.Text = valorlbl.ToString("00.00", CultureInfo.InvariantCulture);
                iva = valorlbl * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = valorlbl + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
            else
            {
                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                labelSubTotalOC.Text = CSSuma.ToString("00.00", CultureInfo.InvariantCulture);
                iva = CSSuma * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = CSSuma + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
        }

        private void textBoxIVA_Click(object sender, EventArgs e)
        {
            textBoxIVA.SelectAll();
        }

        private void textBoxCSolicitada_TextChanged(object sender, EventArgs e)
        {
            if(textBoxCSolicitada.Text==".")
            {
                MessageBox.Show("No puede iniciar con un punto en este campo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCSolicitada.Text = "0";
            }
            if(!string.IsNullOrWhiteSpace(textBoxCSolicitada.Text))
            {
                textBoxCSolicitada.Text = textBoxCSolicitada.Text.Replace("..", ".");

                CS = Convert.ToDouble(textBoxCSolicitada.Text);
                ivas = Convert.ToDouble(textBoxIVA.Text);
                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                valorlbl = CSSuma + resultado;
                labelSubTotalOC.Text = valorlbl.ToString("00.00", CultureInfo.InvariantCulture);
                iva = valorlbl * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = valorlbl + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
            else
            {
                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                labelSubTotalOC.Text = CSSuma.ToString("00.00", CultureInfo.InvariantCulture);
                iva = CSSuma * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = CSSuma + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
            textBoxCSolicitada.SelectionStart = textBoxCSolicitada.TextLength;
        }

        private void textBoxCSolicitada_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCSolicitada.Text))
            {
                textBoxCSolicitada.Text = "";
            }
        }

        private void textBoxCSolicitada_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxPCotizado_TextChanged(object sender, EventArgs e)
        {
            if(textBoxPCotizado.Text.Equals("."))
            {
                MessageBox.Show("No puede iniciar con un punto en este campo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPCotizado.Text = "";
            }
            else if(!(string.IsNullOrWhiteSpace(textBoxPCotizado.Text)))
            {
                textBoxPCotizado.Text = textBoxPCotizado.Text.Replace("..", ".");
                PC = Convert.ToDouble(textBoxPCotizado.Text);
                ivas = Convert.ToDouble(textBoxIVA.Text);

                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                valorlbl = CSSuma + resultado;
                labelSubTotalOC.Text = valorlbl.ToString("00.00", CultureInfo.InvariantCulture);
                iva = valorlbl * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = valorlbl + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
            else
            {
                resultado = CS * PC;
                labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
                labelSubTotalOC.Text = CSSuma.ToString("00.00", CultureInfo.InvariantCulture);
                iva = CSSuma * (ivas / 100);
                labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
                resultadototal = CSSuma + iva;
                labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
            }
            textBoxPCotizado.SelectionStart = textBoxPCotizado.TextLength;
        }

        private void textBoxPCotizado_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxPCotizado.Text))
            {
                textBoxPCotizado.Text = "0";
            }
        }

        private void textBoxPCotizado_KeyPress(object sender, KeyPressEventArgs e)
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

        private void comboBoxClave_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand validar = new MySqlCommand("SELECT existencias, UPPER(nombreRefaccion) AS nombreRefaccion FROM crefacciones WHERE idrefaccion='" + comboBoxClave.SelectedValue + "'", co.dbconection());
            MySqlDataReader leer = validar.ExecuteReader();
            if (leer.Read())
            {
                labelExistencia.Text = leer["existencias"].ToString();
                labelDescripcion.Text = leer["nombreRefaccion"].ToString();
            }
            else
            {
                if (!leer.Read())
                {
                    labelExistencia.Text = "";
                    labelDescripcion.Text = "";
                }
            }
            leer.Close();
            co.dbconection().Close();
        }

        private void checkBoxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechas.Checked == true)
            {
                dateTimePickerIni.Enabled = true;
                dateTimePickerFin.Enabled = true;
            }
            else
            {
                dateTimePickerIni.Enabled = false;
                dateTimePickerFin.Enabled = false;
            }
        }

        private void checkBoxFechasE_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxFechasE.Checked == true)
            {
                dateTimePickerIniBFE.Enabled = true;
                dateTimePickerFinBFE.Enabled = true;
            }
            else
            {
                dateTimePickerIniBFE.Enabled = false;
                dateTimePickerFinBFE.Enabled = false;
            }
        }

        private void dateTimePickerFechaEntrega_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void textBoxPCotizado_Click(object sender, EventArgs e)
        {
            textBoxPCotizado.SelectAll();
        }

        private void textBoxCSolicitada_Click(object sender, EventArgs e)
        {
            textBoxCSolicitada.SelectAll();
        }

        /* Validaciones de los campos*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxObservaciones_KeyPress(object sender, KeyPressEventArgs e)
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
                MessageBox.Show("Solo se pueden ocupar los números (0 - 9), las letras (a - z / A - Z) el punto (.), la coma (,) y el espacio", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxIVA_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxObservacionesRefacc_KeyPress(object sender, KeyPressEventArgs e)
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
                MessageBox.Show("Solo se pueden ocupar los números (0 - 9), las letras (a - z / A - Z) el punto (.), la coma (,) y el espacio", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxOCompraB_KeyPress(object sender, KeyPressEventArgs e)
      {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != 79 && e.KeyChar != 67 && e.KeyChar != 111 && e.KeyChar != 99)
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números, un solo guion y las letras O y C", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un guion en la caja de texto", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /* Movimiento de los botónes *////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAgregar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAgregar.Size = new Size(59, 56);
        }

        private void OrdenDeCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hiloEx2!=null) 
            {
                hiloEx2.Abort();
            }
        }

        private void comboBoxFacturar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pverEmpresa)
                {
                    ComboBox cb = sender as ComboBox;
                    int? res1 =(int?) cb.SelectedValue??0;
                    if (res1== this.res)
                    {
                        aEmpresas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void aEmpresas()
        {
            catEmpresas c = new catEmpresas(idUsuario, empresa, area);
            c.Owner = this;
            c.lbllogo.Visible = true;
            c.pblogo.Visible = true;
            DialogResult res = c.ShowDialog();
            if (res==DialogResult.Cancel)
            {
                comboBoxFacturar.SelectedIndex = 0;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            aEmpresas();
        }

        private void buttonAgregar_MouseLeave(object sender, EventArgs e)
        {
            buttonAgregar.Size = new Size(54, 51);
        }

        private void buttonNuevoOC_MouseMove(object sender, MouseEventArgs e)
        {
            buttonNuevoOC.Size = new Size(59, 56);
        }

        private void buttonNuevoOC_MouseLeave(object sender, EventArgs e)
        {
            buttonNuevoOC.Size = new Size(54, 51);
        }
        private void buttonPDF_MouseMove(object sender, MouseEventArgs e)
        {
            buttonPDF.Size = new Size(59, 56);
        }

        private void buttonPDF_MouseLeave(object sender, EventArgs e)
        {
            buttonPDF.Size = new Size(54, 51);
        }

        private void buttonEditar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonEditar.Size = new Size(59, 56);
        }

        private void buttonEditar_MouseLeave(object sender, EventArgs e)
        {
            buttonEditar.Size = new Size(54, 51);
        }

        private void buttonAgregarMas_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAgregarMas.Size = new Size(59, 56);
        }

        private void buttonAgregarMas_MouseLeave(object sender, EventArgs e)
        {
            buttonAgregarMas.Size = new Size(54, 51);
        }

        private void buttonBuscar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonBuscar.Size = new Size(44, 41);
        }

        private void buttonBuscar_MouseLeave(object sender, EventArgs e)
        {
            buttonBuscar.Size = new Size(39, 36);
        }

        private void buttonActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonActualizar.Size = new Size(59, 56);
        }

        private void buttonActualizar_MouseLeave(object sender, EventArgs e)
        {
            buttonActualizar.Size = new Size(54, 51);
        }

        private void buttonExcel_MouseMove(object sender, MouseEventArgs e)
        {
            buttonExcel.Size = new Size(59, 56);
        }

        private void buttonExcel_MouseLeave(object sender, EventArgs e)
        {
            buttonExcel.Size = new Size(54, 51);
        }

        private void buttonFinalizar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonFinalizar.Size = new Size(59, 56);
        }

        private void buttonFinalizar_MouseLeave(object sender, EventArgs e)
        {
            buttonFinalizar.Size = new Size(54, 51);
        }

        void comboBoxProveedor_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxClave_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxFacturar_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxProveedorB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void comboBoxEmpresaB_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void dataGridViewOCompra_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridViewOCompra.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "FINALIZADA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
            }
        }

        private void dataGridViewOCompra_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridViewPedOCompra_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void textBoxObservacionesRefacc_Validated(object sender, EventArgs e)
        {
            while(textBoxObservacionesRefacc.Text.Contains("  "))
            {
                textBoxObservacionesRefacc.Text = textBoxObservacionesRefacc.Text.Replace("  ", " ");
            }
        }

        private void textBoxObservaciones_Validated(object sender, EventArgs e)
        {
            while(textBoxObservaciones.Text.Contains("  "))
            {
                textBoxObservaciones.Text = textBoxObservaciones.Text.Replace("  ", " ");
            }
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
        public void combos_DrawItem(object sender, DrawItemEventArgs e)
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

        private void textBoxIVA_Leave(object sender, EventArgs e)
        {
            if(textBoxIVA.TextLength==1)
            {
                MessageBox.Show("Es necesario que sean 2 dijitos en el campo de iva", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxIVA.Text = "0";
            }
        }

        private void dateTimePickerFechaEntrega_ValueChanged(object sender, EventArgs e)
        {
            if(bedit == true)
            {
                if (comboBoxProveedor.SelectedIndex == 0)
                {
                    pr = "0";
                }
                else
                {
                    pr = Convert.ToString(comboBoxProveedor.SelectedValue);
                }
                if (comboBoxFacturar.SelectedIndex == 0)
                {
                    fc = "0";
                }
                else
                {
                    fc = Convert.ToString(comboBoxFacturar.SelectedValue);
                }
                if ((((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((observOC == textBoxObservaciones.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))) && (dateTimePickerFechaEntrega.Value.Date == dateTimePicker1.Value.Date)))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void dataGridViewOCompra_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                bedit = false;
                if ((pinsertar == true) && (peditar == true) && (pconsultar == true) && (pdesactivar == true))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                        int xy = dataGridViewOCompra.HitTest(e.X, e.Y).RowIndex;
                        if (xy >= 0)
                        {
                            mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                        }
                        mn.Show(dataGridViewOCompra, new Point(e.X, e.Y));

                        mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (comboBoxProveedor.SelectedIndex == 0)
            {
                pr = "0";
            }
            else
            {
                pr = Convert.ToString(comboBoxProveedor.SelectedValue);
            }
            if (comboBoxFacturar.SelectedIndex == 0)
            {
                fc = "0";
            }
            else
            {
                fc = Convert.ToString(comboBoxFacturar.SelectedValue);
            }
            if (((proveeid == pr) || (comboBoxProveedor.SelectedIndex == 0)) && ((factid == fc) || (comboBoxFacturar.SelectedIndex == 0)) && ((textBoxObservaciones.Text.Trim() == observOC)))
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "EDITAR":

                        metodocargadetordenPDF();
                        buttonEditar.Visible = false;
                        label8.Visible = false;
                        buttonAgregarMas.Visible = false;
                        label29.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonNuevoOC.Visible = true;
                        label17.Visible = true;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        buttonExcel.Visible = false;
                        label38.Visible = false;
                        buttonAgregar.Visible = false;
                        label9.Visible = false;
                        comboBoxProveedor.Enabled = false;
                        comboBoxFacturar.Enabled = false;
                        comboBoxClave.Enabled = false;
                        dateTimePickerFechaEntrega.Enabled = false;
                        textBoxIVA.Enabled = false;
                        textBoxObservaciones.Enabled = false;
                        textBoxObservacionesRefacc.Enabled = false;
                        dataGridViewPedOCompra.Enabled = false;

                            folioOC = 0;
                            estatus = "";
                            edt = 0;
                            labelFolioOC.Text = dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value.ToString();
                            metodorecid();
                            pf = dataGridViewOCompra.CurrentRow.Cells["PERSONA FINAL"].Value.ToString();
                            estatus = dataGridViewOCompra.CurrentRow.Cells["ESTATUS"].Value.ToString();

                            MySqlCommand cmd = new MySqlCommand("SELECT coalesce((t1.idOrdCompra), '0') AS 'idOrdCompra', t1.FechaEntregaOCompra, coalesce((t1.Subtotal), '0') AS Subtotal, coalesce((t1.IVA), '0') AS IVA, coalesce((t1.Total), '0') AS Total, coalesce((t1.ObservacionesOC), '') AS Observaciones, UPPER(t2.nombreEmpresa) AS NEmpresa, t1.FacturadafkCEmpresas AS EFacturar, coalesce((SELECT UPPER(t3.empresa) FROM cproveedores AS t3 WHERE t1.ProveedorfkCProveedores = t3.idproveedor), '0') AS Proveedor, t1.ProveedorfkCProveedores AS NProveedores, SUM(t4.Total) AS TotalCS FROM ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN detallesordencompra AS t4 ON t1.idOrdCompra = t4.OrdfkOrdenCompra WHERE t1.FolioOrdCompra = '" + labelFolioOC.Text + "'", co.dbconection());
                            MySqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                folioOC = Convert.ToInt32(dr.GetString("idOrdCompra"));
                                dateTimePickerFechaEntrega.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                                dateTimePicker1.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                                labelSubTotalOC.Text = Convert.ToString(dr.GetString("Subtotal"));
                                if (pf.Equals(""))
                                {
                                    metodocargaiva();
                                }
                                else
                                {
                                    textBoxIVA.Text = Convert.ToString(dr["IVA"]).ToString();
                                }
                                labelTotalOC.Text = Convert.ToString(dr.GetString("Total"));
                                textBoxObservaciones.Text = Convert.ToString(dr.GetString("Observaciones"));
                                observOC = Convert.ToString(dr.GetString("Observaciones"));
                                comboBoxProveedor.Text = Convert.ToString(dr.GetString("Proveedor"));
                                provee = Convert.ToString(dr.GetString("Proveedor"));
                                proveeid = Convert.ToString(dr.GetString("NProveedores"));
                                comboBoxFacturar.Text = Convert.ToString(dr.GetString("NEmpresa"));
                                fact = Convert.ToString(dr.GetString("NEmpresa"));
                                factid = Convert.ToString(dr.GetString("EFacturar"));
                                CSSuma = Convert.ToDouble(dr.GetString("TotalCS"));
                                textBoxIVA.Enabled = false;
                            }
                            dr.Close();
                            co.dbconection().Close();
                            textBoxCSolicitada.Enabled = false;
                            textBoxPCotizado.Enabled = false;

                            if (estatus == "FINALIZADA")
                            {
                                if (!(string.IsNullOrWhiteSpace(comboBoxProveedor.Text)))
                                {
                                    comboBoxProveedor.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(comboBoxFacturar.Text)))
                                {
                                    comboBoxFacturar.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                                {
                                    dateTimePickerFechaEntrega.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                                {
                                    textBoxObservaciones.Enabled = true;
                                }
                                else
                                {
                                    textBoxObservaciones.Enabled = true;
                                }
                            }
                            else
                            {
                                if (!(string.IsNullOrWhiteSpace(comboBoxProveedor.Text)))
                                {
                                    comboBoxProveedor.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(comboBoxFacturar.Text)))
                                {
                                    comboBoxFacturar.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                                {
                                    dateTimePickerFechaEntrega.Enabled = true;
                                }
                                if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                                {
                                    textBoxObservaciones.Enabled = true;
                                }

                                buttonAgregar.Visible = false;
                                label9.Visible = false;
                                buttonActualizar.Visible = false;
                                label37.Visible = false;
                                buttonExcel.Visible = false;
                                label38.Visible = false;
                                buttonPDF.Visible = false;
                                label3.Visible = false;
                            }
                            dr.Close();
                            co.dbconection().Close();
                            bedit = true;                
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

                            metodocargadetordenPDF();
                            buttonEditar.Visible = false;
                            label8.Visible = false;
                            buttonAgregarMas.Visible = false;
                            label29.Visible = false;
                            buttonPDF.Visible = false;
                            label3.Visible = false;
                            buttonNuevoOC.Visible = true;
                            label17.Visible = true;
                            buttonFinalizar.Visible = false;
                            label34.Visible = false;
                            buttonExcel.Visible = false;
                            label38.Visible = false;
                            buttonAgregar.Visible = false;
                            label9.Visible = false;
                            comboBoxProveedor.Enabled = false;
                            comboBoxFacturar.Enabled = false;
                            comboBoxClave.Enabled = false;
                            dateTimePickerFechaEntrega.Enabled = false;
                            textBoxIVA.Enabled = false;
                            textBoxObservaciones.Enabled = false;
                            textBoxObservacionesRefacc.Enabled = false;
                                folioOC = 0;
                                estatus = "";
                                edt = 0;
                                labelFolioOC.Text = dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value.ToString();
                                metodorecid();
                                pf = dataGridViewOCompra.CurrentRow.Cells["PERSONA FINAL"].Value.ToString();
                                estatus = dataGridViewOCompra.CurrentRow.Cells["ESTATUS"].Value.ToString();

                                MySqlCommand cmd = new MySqlCommand("SELECT coalesce((t1.idOrdCompra), '0') AS 'idOrdCompra', t1.FechaEntregaOCompra, coalesce((t1.Subtotal), '0') AS Subtotal, coalesce((t1.IVA), '0') AS IVA, coalesce((t1.Total), '0') AS Total, coalesce((t1.ObservacionesOC), '') AS Observaciones, UPPER(t2.nombreEmpresa) AS NEmpresa, t1.FacturadafkCEmpresas AS EFacturar, coalesce((SELECT UPPER(t3.empresa) FROM cproveedores AS t3 WHERE t1.ProveedorfkCProveedores = t3.idproveedor), '0') AS Proveedor, t1.ProveedorfkCProveedores AS NProveedores, SUM(t4.Total) AS TotalCS FROM ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN detallesordencompra AS t4 ON t1.idOrdCompra = t4.OrdfkOrdenCompra WHERE t1.FolioOrdCompra = '" + labelFolioOC.Text + "'", co.dbconection());
                                MySqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    folioOC = Convert.ToInt32(dr.GetString("idOrdCompra"));
                                    dateTimePickerFechaEntrega.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                                    dateTimePicker1.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                                    labelSubTotalOC.Text = Convert.ToString(dr.GetString("Subtotal"));
                                    if (pf.Equals(""))
                                    {
                                        metodocargaiva();
                                    }
                                    else
                                    {
                                        textBoxIVA.Text = Convert.ToString(dr["IVA"]).ToString();
                                    }
                                    labelTotalOC.Text = Convert.ToString(dr.GetString("Total"));
                                    textBoxObservaciones.Text = Convert.ToString(dr.GetString("Observaciones"));
                                    observOC = Convert.ToString(dr.GetString("Observaciones"));
                                    comboBoxProveedor.Text = Convert.ToString(dr.GetString("Proveedor"));
                                    provee = Convert.ToString(dr.GetString("Proveedor"));
                                    proveeid = Convert.ToString(dr.GetString("NProveedores"));
                                    comboBoxFacturar.Text = Convert.ToString(dr.GetString("NEmpresa"));
                                    fact = Convert.ToString(dr.GetString("NEmpresa"));
                                    factid = Convert.ToString(dr.GetString("EFacturar"));
                                    CSSuma = Convert.ToDouble(dr.GetString("TotalCS"));
                                    textBoxIVA.Enabled = false;
                                }

                                textBoxCSolicitada.Enabled = false;
                                textBoxPCotizado.Enabled = false;

                                if (estatus == "FINALIZADA")
                                {
                                    if (!(string.IsNullOrWhiteSpace(comboBoxProveedor.Text)))
                                    {
                                        comboBoxProveedor.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(comboBoxFacturar.Text)))
                                    {
                                        comboBoxFacturar.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                                    {
                                        dateTimePickerFechaEntrega.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                                    {
                                        textBoxObservaciones.Enabled = true;
                                    }
                                    else
                                    {
                                        textBoxObservaciones.Enabled = true;
                                    }
                                }
                                else
                                {
                                    if (!(string.IsNullOrWhiteSpace(comboBoxProveedor.Text)))
                                    {
                                        comboBoxProveedor.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(comboBoxFacturar.Text)))
                                    {
                                        comboBoxFacturar.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                                    {
                                        dateTimePickerFechaEntrega.Enabled = true;
                                    }
                                    if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                                    {
                                        textBoxObservaciones.Enabled = true;
                                    }

                                    buttonAgregar.Visible = false;
                                    label9.Visible = false;
                                    buttonActualizar.Visible = false;
                                    label37.Visible = false;
                                    buttonExcel.Visible = false;
                                    label38.Visible = false;
                                    buttonPDF.Visible = false;
                                    label3.Visible = false;
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
            }
        }

        private void groupBoxProveedor_Paint(object sender, PaintEventArgs e)
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

        private void groupBoxFechasE_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxRefaccion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxEdicion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxObsRef_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxObs_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void textBoxOCompraB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
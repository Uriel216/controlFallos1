using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace controlFallos
{
    public partial class nuevaRefaccion : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario, status,empresa,area;
        string idRefaccionTemp,codrefAnterior, nomrefanterior, modrefanterior, familiaanterior, umAnterior, marcaAnterior, nivelAnterior, charolaAnterior,ultimoabastecimiento, mediaAnterior, abastecimientoAnterior,descripcionAnterior;
         public bool editar { private set; get; }
        
        decimal ultimacantidad;
        public string idRefaccionMediaAbast;
        bool yaAparecioMensaje = false;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        void getCambios(object sender,EventArgs e)
        {
            try
            {
                int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
                int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
                int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
                decimal media = 0; if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text);
                decimal abastecimiento = 0; if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text);
                int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
                decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
                if (editar)
                {
                    if (status == 1 && (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) && !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) && proxabastecimiento.Value > DateTime.Now && cbfamilia.SelectedIndex > 0 && cbum.SelectedIndex > 0 && cbmarcas.SelectedIndex > 0 && pasillo > 0 && nivel > 0 && anaquel > 0 && charolafkccharolas > 0 && media> 0 && abastecimiento > 0) && (codrefAnterior != txtcodrefaccion.Text.Trim() || nomrefanterior != v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower()) || (modrefanterior != txtmodeloRefaccion.Text.Trim()) || (proxabastecimiento.Value.ToString("dd / MMMM / yyyy").ToUpper() != ultimoabastecimiento) || (familiaanterior != cbfamilia.SelectedValue.ToString()) || (umAnterior != cbum.SelectedValue.ToString()) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen> 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower()))))
                    {
                        btnsave.Visible = lblsave.Visible = true;
                    }
                    else
                    {
                        btnsave.Visible = lblsave.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public nuevaRefaccion(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbfamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbum.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbmarcas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbnivel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbpasillo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbanaquel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbcharola.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbfamiliabusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbmarcasbusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area; DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefaccion.ColumnHeadersDefaultCellStyle = d;
        }
        public nuevaRefaccion(int idUsuario,string idrefaccion)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.idRefaccionMediaAbast=idrefaccion;
        }
        public void establecerPrivilegios()
        {
            string sql = "SELECT insertar,consultar,editar, desactivar  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
                Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
                Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
                Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            }
            mdr.Close();
                c.dbcon.Close();
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddrefaccion.Visible = true;
            }
            if (Pconsultar)
            {
                gbconsultar.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
        }
        void iniubicaciones()
        {
            string sq = "SELECT COUNT(idpasillo) as cuenta FROM cpasillos WHERE status='1'";
            MySqlCommand cmd = new MySqlCommand(sq, c.dbconection());
            int numFilas = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            c.dbcon.Close();

            if (numFilas > 0)
            {
                string sql = "SELECT idpasillo,UPPER(pasillo) AS pasillo FROM cpasillos WHERE status='1' ORDER BY pasillo ASC";
                DataTable dt = new DataTable();
                DataRow nuevaFila = dt.NewRow();
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
                AdaptadorDatos.Fill(dt);
                cbpasillo.ValueMember = "idpasillo";
                cbpasillo.DisplayMember = "pasillo";
                nuevaFila["idpasillo"] = 0;
                nuevaFila["pasillo"] = "--Seleccione pasillo--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cbpasillo.DataSource = dt;
          
            }
        }
        void iniFamilias()
        {
            string sql = "SELECT idfamilia,UPPER(CONCAT(familia,' - ',descripcionFamilia)) as familia FROM cfamilias WHERE status='1' ORDER BY CONCAT(familia,' - ',descripcionFamilia) ASC";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila1 = dt1.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            cbfamilia.ValueMember = "idfamilia";
            cbfamilia.DisplayMember = "familia";
            cbfamiliabusq.ValueMember = "idfamilia";
            cbfamiliabusq.DisplayMember = "familia";
            nuevaFila["idfamilia"] = 0;
            nuevaFila["familia"] = "--Seleccione Familia--".ToUpper();
            nuevaFila1["idfamilia"] = 0;
            nuevaFila1["familia"] = "--Seleccione Familia--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt1.Rows.InsertAt(nuevaFila1, 0);
            cbfamilia.DataSource = dt;
            cbfamiliabusq.DataSource = dt1;
        }
        void iniUnidadMedida()
        {
            string sql = "SELECT idunidadmedida,upper(CONCAT(Simbolo, ' - ',Nombre)) as um FROM cunidadmedida WHERE status='1' order by CONCAT(Simbolo, ' - ',Nombre) ASC";
            DataTable dt = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbum.ValueMember = "idunidadmedida";
            cbum.DisplayMember = "um";
            nuevaFila["idunidadmedida"] = 0;
            nuevaFila["um"] = "--Seleccione Unidad de Medida--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbum.DataSource = dt;
        }
        void inimarcas()
        {
            string sql = "SELECT idmarca,UPPER(marca) AS marca FROM cmarcas WHERE status='1' order by marca ASC";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila1 = dt1.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            cbmarcas.ValueMember = "idmarca";
            cbmarcas.DisplayMember = "marca";
            cbmarcasbusq.ValueMember = "idmarca";
            cbmarcasbusq.DisplayMember = "marca";
            nuevaFila["idmarca"] = 0;
            nuevaFila["marca"] = "--Seleccione marca--".ToUpper();
            nuevaFila1["idmarca"] = 0;
            nuevaFila1["marca"] = "--Seleccione marca--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt1.Rows.InsertAt(nuevaFila1, 0);
            cbmarcas.DataSource = dt;
            cbmarcasbusq.DataSource = dt1;
        }
      
        private void nuevaRefaccion_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar) {
                iniFamilias();
                iniubicaciones();
                iniUnidadMedida();
                inimarcas();
                proxabastecimiento.Value = DateTime.Now;
            }
            if (Pconsultar) {
                insertarRefacciones();
            }
            if (!string.IsNullOrWhiteSpace(idRefaccionMediaAbast))
            {
                BuscarRefaccion();
            }
        }
       public void BuscarRefaccion()
        {
            for (int i=0;i<tbrefaccion.Rows.Count;i++)
            {
                if (tbrefaccion.Rows[i].Cells[0].Value.ToString().Equals(idRefaccionMediaAbast))
                {
                    tbrefaccion.Rows[i].Selected = true;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            limpiar();
            tbrefaccion.Rows.Clear();
     
            string sql = @"SELECT t1.idrefaccion,UPPER(t1.codrefaccion) as codrefaccion,UPPER(t1.nombreRefaccion) as nombreRefaccion,UPPER(t1.modeloRefaccion) as modeloRefaccion,UPPER(CONCAT(t2.familia,' - ',t2.descripcionFamilia)) as familia,UPPER(CONCAT(t3.Simbolo, ' - ',t3.Nombre)) as um, UPPER(t8.marca) as marca,UPPER(t1.nivel) AS nivel,UPPER(concat(t7.nombres,' ',t7.ApPaterno,' ',t7.ApMaterno)) as nombre, UPPER((SELECT CONCAT('Pasillo: ',tabla4.pasillo, '; Anaquel: ', tabla3.anaquel,'; Charola:',tabla2.charola) FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion)) as Ubicacion, UPPER(CONCAT(t1.existencias, ' ',t3.Simbolo)) AS existencias,UPPER(CONCAT(t1.media,' ',t3.Simbolo)) AS media,UPPER(t1.media) as idmedia,UPPER(CONCAT(t1.abastecimiento,' ',t3.Simbolo)) AS abastecimiento,t1.abastecimiento as idabastecimiento,UPPER(t1.fechaHoraalta) as fecha,t1.status,t2.idfamilia,t3.idunidadmedida, t8.idmarca,(SELECT tabla4.idpasillo FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idpasillo,(SELECT tabla3.idanaquel FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idanaquel, t1.charolafkcharolas as idcharola, DATE(t1.proximoAbastecimiento) as proxabast, t1.existencias as ultim,upper(COALESCE(t1.descripcionRefaccion,'')) as descr FROM crefacciones as t1 INNER JOIN cfamilias as t2 ON t1.familiafkcfamilias=t2.idfamilia INNER JOIN cunidadmedida as t3 ON t1.umfkcunidadmedida=t3.idunidadmedida INNER JOIN cpersonal as t7 ON t1.usuarioaltafkcpersonal= t7.idPersona INNER JOIN cmarcas as t8 ON t1. marcafkcmarcas = t8.idmarca ";
            string wheres = "";
            if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text))
            {
                if (wheres == "")
                {
                    wheres = "WHERE nombreRefaccion LIKE '" + txtnombrereFaccionbusq.Text + "%' ";
                }
                else
                {
                    wheres += " AND nombreRefaccion LIKE'" + txtnombrereFaccionbusq.Text + "%' ";
                }
            }
            if (cbfamiliabusq.SelectedIndex > 0)
            {
                if (wheres == "")
                {
                    wheres = "WHERE familiafkcfamilias ='" + cbfamiliabusq.SelectedValue + "' ";
                }
                else
                {
                    wheres += " AND familiafkcfamilias ='" + cbfamiliabusq.SelectedValue + "' ";
                }
            }
            if (cbmarcasbusq.SelectedIndex > 0)
            {
                if (wheres == "")
                {
                    wheres = "WHERE marcafkcmarcas ='" + cbmarcasbusq.SelectedValue + "' ";
                }
                else
                {
                    wheres += " AND marcafkcmarcas ='" + cbmarcasbusq.SelectedValue + "' ";
                }
            }
            sql += wheres+" ORDER BY t1.nombreRefaccion ASC";
            txtnombrereFaccionbusq.Clear();
            cbfamiliabusq.SelectedIndex = 0;
            cbmarcasbusq.SelectedIndex = 0;

            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            if (Convert.ToInt32(cm.ExecuteScalar()) > 0)
            {
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbrefaccion.Rows.Add(dr.GetString("idrefaccion"), dr.GetString("codrefaccion"), dr.GetString("nombreRefaccion"), dr.GetString("modeloRefaccion"), dr.GetString("familia"), dr.GetString("um"), dr.GetString("marca"), v.getNivelFromID(dr.GetInt32("nivel")), dr.GetString("Ubicacion"), dr.GetString("existencias"), dr.GetString("media"), dr.GetString("abastecimiento"), dr.GetDateTime("proxabast").ToString("dd/MM/yyy"), dr.GetString("descr"), dr.GetString("nombre"), dr.GetString("fecha"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idfamilia"), dr.GetString("idunidadmedida"), dr.GetString("idmarca"), dr.GetString("nivel"), dr.GetString("idpasillo"), dr.GetString("idanaquel"), dr.GetString("idcharola"), dr.GetString("nivel"), dr.GetString("idmedia"), dr.GetString("idabastecimiento"), dr.GetString("ultim"));
            }
            dr.Close();
            }
            else
            {
                MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                insertarRefacciones();
            }

            c.dbconection().Close();
            tbrefaccion.ClearSelection();
            pActualizar.Visible = true;
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarRefacciones();
        }

        private void cbanaquel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM ccharolas where anaquelfkcanaqueles='" + cbanaquel.SelectedValue + "'")) > 0)
            {
                string sql = "SELECT idcharola,UPPER(charola) AS charola FROM ccharolas WHERE status='1' and anaquelfkcanaqueles= '" + cbanaquel.SelectedValue + "' ORDER BY charola ASC";
                v.iniCombos(sql, cbcharola, "idcharola", "charola", "--SELECCIONE UN ANAQUEL");
                cbcharola.Enabled = true;
            }
            else
            {

                cbcharola.DataSource = null;
                cbcharola.Enabled = false;
            }
        }

        private void txtcodrefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.codrefaccionValido(txtcodrefaccion.Text);
        }

        private void txtnombrereFaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void txtmodeloRefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.modrefaccionValido(txtmodeloRefaccion.Text);
        }

        private void cbfamilia_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbrefaccion_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUM(e);
        }

        private void cbmarcas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbnivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM canaqueles where nivelfkcniveles ='" + cbnivel.SelectedValue + "'")) > 0)
            {
                string sql = "SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE status='1' and nivelfkcniveles= '" + cbnivel.SelectedValue + "' ORDER BY anaquel ASC";
                v.iniCombos(sql, cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                cbanaquel.Enabled = true;
            }
            else
            {

                cbanaquel.DataSource = null;
                cbanaquel.Enabled = false;
            }
        }

        private void cbum_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbcharola_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gbaddrefaccion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            insertarRefacciones();
            pActualizar.Visible = false;
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox txtKilometraje = sender as TextBox; 
            try
            {
                if (!string.IsNullOrWhiteSpace(txtKilometraje.Text))
                {
                    double km = double.Parse(txtKilometraje.Text);
                    if (txtKilometraje.TextLength <= 3)
                    {
                        txtKilometraje.Text = string.Format("{0:F2}", km);
                    }
                    else
                    {
                        txtKilometraje.Text = Convert.ToString((Math.Floor(km * 100) / 100));
                        km = double.Parse(txtKilometraje.Text);
                        txtKilometraje.Text = string.Format("{0:N2}", km);
                        if (km > 5000)
                        {
                            txtKilometraje.Text = "5,000.00";
                        }
                    }
                    Regex r4 = new Regex(@"^\d{1,3}\,\d{3}\.\d{2,2}$");
                    Regex r5 = new Regex(@"^\d{1,3}\,\d{3}\,\d{3}\.\d{2,2}");
                    Regex r1 = new Regex(@"^\d{0,3}\.\d{1,2}$");
                    if (!r1.IsMatch(txtKilometraje.Text) && !r4.IsMatch(txtKilometraje.Text) && !r5.IsMatch(txtKilometraje.Text))
                    {
                        MessageBox.Show("El formato de Notificación de Media ingresado es incorrecto".ToUpper(),validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKilometraje.Focus();
                        txtKilometraje.Clear();
                    }

                }
            }
            catch
            {
                MessageBox.Show("El formato de Notificación de Media es incorrecto".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKilometraje.Focus();
                txtKilometraje.Clear();
            }
        }

        private void validcacionNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtKilometraje = sender as TextBox;
            char signo_decimal = (char)46;
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan: numéros y ( . ) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.KeyChar == 46)
            {
                if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                {
                    e.Handled = true; // Interceptamos la pulsación para que no permitirla.
                }
            }
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtcodrefaccion_Validating_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            v.espaciosenblanco(sender,e);
        }

        private void pExistencias_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cniveles where pasillofkcpasillos ='" + cbpasillo.SelectedValue + "'")) > 0)
            {
                string sql = "SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "' ORDER BY nivel ASC";
                v.iniCombos(sql, cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                cbnivel.Enabled = true;
            }
            else
            {

                cbnivel.DataSource = null;
                cbnivel.Enabled = false;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            decimal media = 0; if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text);
            decimal abastecimiento = 0; if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text);
            int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
            if (!string.IsNullOrWhiteSpace(idRefaccionTemp) && status == 1 && (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) && !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) && proxabastecimiento.Value > DateTime.Now && cbfamilia.SelectedIndex > 0 && cbum.SelectedIndex > 0 && cbmarcas.SelectedIndex > 0 && pasillo > 0 && nivel > 0 && anaquel > 0 && charolafkccharolas > 0 && media > 0 && abastecimiento > 0) && (codrefAnterior != txtcodrefaccion.Text.Trim() || nomrefanterior != v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower()) || (modrefanterior != txtmodeloRefaccion.Text.Trim()) || (proxabastecimiento.Value.ToString("dd / MMMM / yyyy").ToUpper() != ultimoabastecimiento) || (familiaanterior != cbfamilia.SelectedValue.ToString()) || (umAnterior != cbum.SelectedValue.ToString()) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen > 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower())))) { 
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button11_Click(null,e);
                }
                else
                {
                    limpiar();
                }
            }
            else
            {

                limpiar();
            }
        }

        private void btndelcla_Click(object sender, EventArgs e)
        {
            try
            {
                int status;
                string msg;
                if (this.status == 0)
                {
                    status = 1;
                    msg = "Re";
                } else
                {
                    status = 0;
                    msg = "Des";
                }

                if (MessageBox.Show("¿Está Seguro que desea " + msg + "activar la Refacción?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (c.insertar("UPDATE crefacciones SET status ='" + status + "' WHERE  idrefaccion='" + this.idRefaccionTemp + "'"))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones','" + idRefaccionTemp + "','" + msg + "activación de Refacción','" + idUsuario + "',NOW(),'" + msg + "activación de Refacción','" + empresa + "','" + area + "')");
                        MessageBox.Show("Se ha " + msg + "activado la Refacción Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtcodrefaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paracodrefaccion(e);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasnumerosdiagonalypunto(e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }else
                 {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        } 
        void _editar()
        {
            string codrefaccion =txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            string proxabastParaValidacion = proxabastecimiento.Value.ToShortDateString();
            int familia = Convert.ToInt32(cbfamilia.SelectedValue.ToString());
            int um = Convert.ToInt32(cbum.SelectedValue.ToString());
            int marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            int charolafkccharolas = 0; if(cbcharola.DataSource !=null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
            decimal media = Convert.ToDecimal(notifmedia.Text);
           decimal abastecimiento = Convert.ToDecimal(notifabastecimiento.Text);
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, familia, um, marca,pasillo,nivel,anaquel, charolafkccharolas, proxabastecimiento.Value) && !v.NumericsUpDownRefaccionEdicion(media,abastecimiento) )
            {
                if (codrefaccion.Equals(codrefAnterior) && nombreRefaccion.Equals(nomrefanterior) && modeloRefaccion.Equals(modrefanterior) && proxabastParaValidacion.Equals(ultimoabastecimiento) && familia.ToString().Equals(familiaanterior) && um.ToString().Equals(umAnterior) && marca.ToString().Equals(marcaAnterior) && nivel.ToString().Equals(nivelAnterior) && charolafkccharolas.ToString().Equals(charolaAnterior) && media.ToString().Equals(mediaAnterior) && abastecimiento.ToString().Equals(abastecimientoAnterior) && cantidadAlmacen==0 && descripcionAnterior== v.mayusculas(txtdesc.Text.ToLower()))
                {
                    MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desea Limpiar los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        limpiar();
                    }
                }else
                {
                    if (!v.existeRefaccionActualizar(codrefaccion,codrefAnterior,nombreRefaccion,nomrefanterior,modeloRefaccion,modrefanterior))
                    {
                        if (status==1) {
                            decimal exist = Convert.ToDecimal(cantidad) + Convert.ToDecimal(ultimacantidad);
                            if (c.insertar(@"UPDATE crefacciones SET codrefaccion =LTRIM(RTRIM('" + codrefaccion + "')), nombreRefaccion = LTRIM(RTRIM('" + nombreRefaccion + "')), modeloRefaccion =LTRIM(RTRIM('" + modeloRefaccion + "')), proximoAbastecimiento = '" + proxabast + "', familiafkcfamilias = '" + familia + "', umfkcunidadmedida = '" + um + "', charolafkcharolas = '" + charolafkccharolas + "', existencias = '" + exist + "', marcafkcmarcas = '" + marca + "', media = '" + media + "', abastecimiento = '" + abastecimiento + "',descripcionRefaccion='"+v.mayusculas(txtdesc.Text.ToLower())+"' WHERE idrefaccion = '" + this.idRefaccionTemp + "'"))
                            {
                                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones','"+idRefaccionTemp+"','" + codrefAnterior + ";" + nomrefanterior + ";" + modrefanterior + ";" + ultimoabastecimiento + ";" + familiaanterior + ";" + umAnterior + ";" + marcaAnterior + ";" + charolaAnterior + ";" +  mediaAnterior + ";" + abastecimientoAnterior + ";"+descripcionAnterior+"','" + idUsuario + "',NOW(),'Actualización de Refacción','" + empresa + "','" + area + "')");
                               if(!yaAparecioMensaje) MessageBox.Show("Refacción Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                            }
                        }else
                        {
                            MessageBox.Show("No se Puede Actualizar una Refacción Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }

                }
                

                
            }
        }
        void insertar()
        {
            string codrefaccion = txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            int familia = Convert.ToInt32(cbfamilia.SelectedValue.ToString());
            int um = Convert.ToInt32(cbum.SelectedValue.ToString());
            int marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
            decimal media = Convert.ToDecimal(notifmedia.Text);
            decimal abastecimiento = Convert.ToDecimal(notifabastecimiento.Text.ToString());
            DateTime paraValidacion = proxabastecimiento.Value;
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, familia, um, marca,pasillo,nivel,anaquel,charolafkccharolas,paraValidacion) && !v.existeRefaccion(codrefaccion, nombreRefaccion, modeloRefaccion) && !v.NumericsUpDownRefaccion(cantidadAlmacen, media,abastecimiento))
            {
                if (c.insertar(@"INSERT INTO crefacciones(codrefaccion, nombreRefaccion, modeloRefaccion, proximoAbastecimiento, familiafkcfamilias, umfkcunidadmedida, charolafkcharolas, existencias, marcafkcmarcas,fechaHoraalta,usuarioaltafkcpersonal, media, abastecimiento,descripcionRefaccion)  VALUES (LTRIM(RTRIM('" + codrefaccion + "')),LTRIM(RTRIM('" + nombreRefaccion + "')),LTRIM(RTRIM('" + modeloRefaccion + "')),'" + proxabast + "','" + familia + "','" + um + "','" + charolafkccharolas + "','" + cantidad + "','" + marca + "',NOW(),'" + idUsuario + "','" + media + "','" + abastecimiento + "','"+v.mayusculas(txtdesc.Text.ToLower())+"')"))
                {

                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones',(SELECT idrefaccion FROM crefacciones WHERE codRefaccion='"+codrefaccion+"' AND nombreRefaccion='"+nombreRefaccion+"'),'"+codrefaccion+";"+nombreRefaccion+";"+modeloRefaccion+";"+proxabast+";"+familia+";"+um+";"+marca+";"+charolafkccharolas+";"+cantidad+";"+media+";"+abastecimiento+";"+ v.mayusculas(txtdesc.Text.ToLower())+ "','" + idUsuario + "',NOW(),'Inserción de Refacción','" + empresa + "','" + area + "')");
                    MessageBox.Show("Refacción Agregada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
               gbaddrefaccion.Text = "Agregar Refacción";
                lblsave.Text = "Agregar";
            }
            txtcodrefaccion.Clear();
            txtnombrereFaccion.Clear();
            txtmodeloRefaccion.Clear();
            proxabastecimiento.Value = DateTime.Now.Date;
            cbfamilia.SelectedIndex = 0;
            cbum.SelectedIndex = 0;
            cbmarcas.SelectedIndex = 0;
            cbnivel.SelectedIndex = 0;
            cbpasillo.SelectedIndex = 0;
            cantidada.Clear();
            notifmedia.Clear(); 
            txtdesc.Clear();
            notifabastecimiento.Clear();
            pCancelar.Visible = false;
            pdelref.Visible = false;
            yaAparecioMensaje = false;
            if (Pconsultar)
            {
                insertarRefacciones();
            }
            idRefaccionTemp = null;
            codrefAnterior = null;
            nomrefanterior = null;
            modrefanterior = null;
            familiaanterior = null;
            umAnterior = null;
            marcaAnterior = null;
            nivelAnterior = null;
            charolaAnterior = null;
            ultimoabastecimiento = null;
            btnsave.Visible = lblsave.Visible = true;
            mediaAnterior = null;
            abastecimientoAnterior = null;
            lblexistencias.Text = null;
            pExistencias.Visible = false;
        }

        public void insertarRefacciones()
        {
            tbrefaccion.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SET lc_time_names='es_ES';SELECT t1.idrefaccion,UPPER(t1.codrefaccion) as codrefaccion,UPPER(t1.nombreRefaccion) as nombreRefaccion,UPPER(t1.modeloRefaccion) as modeloRefaccion,UPPER(CONCAT(t2.familia,' - ',t2.descripcionFamilia)) as familia,UPPER(CONCAT(t3.Simbolo, ' - ',t3.Nombre)) as um, UPPER(t8.marca) as marca,UPPER((SELECT CONCAT('Pasillo: ',tabla5.pasillo, ';Nivel: ',tabla4.nivel ,' ;Anaquel: ', tabla3.anaquel,'; Charola:',tabla2.charola) FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cniveles as tabla4 ON tabla3.nivelfkcniveles=tabla4.idnivel INNER JOIN  cpasillos as tabla5 ON tabla4.pasillofkcpasillos=tabla5.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion)) as Ubicacion, UPPER(CONCAT(t1.existencias, ' ',t3.Simbolo)) AS existencias,UPPER(CONCAT(t1.media,' ',t3.Simbolo)) AS media,UPPER(CONCAT(t1.abastecimiento,' ',t3.Simbolo)) AS abastecimiento, UPPER(DATE_FORMAT(t1.proximoAbastecimiento,'%d / %M / %Y')) as proxabast,upper(COALESCE(t1.descripcionRefaccion,'')) as descr,UPPER(concat(t7.nombres,' ',t7.ApPaterno,' ',t7.ApMaterno)) as nombre, UPPER(DATE_FORMAT(t1.fechaHoraalta,'%W, %d de %M del %Y  a las %H:%i:%s')) as fecha,if(t1.status=1,'ACTIVO','NO ACTIVO'),t2.idfamilia,t3.idunidadmedida,(SELECT tabla5.idpasillo FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cniveles as tabla4 ON tabla3.nivelfkcniveles=tabla4.idnivel  INNER JOIN cpasillos as tabla5 ON tabla4.pasillofkcpasillos=tabla5.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idpasillo,(SELECT tabla4.idnivel FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cniveles as tabla4 ON tabla3.nivelfkcniveles=tabla4.idnivel WHERE tabla1.idrefaccion =t1.idrefaccion) as idnivel,(SELECT tabla3.idanaquel FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel  WHERE tabla1.idrefaccion = t1.idrefaccion ) as idanaquel, t1.charolafkcharolas as idcharola, t8.idmarca,t1.media as idmedia,t1.abastecimiento as idabastecimiento,t1.existencias as ultim FROM crefacciones as t1 INNER JOIN cfamilias as t2 ON t1.familiafkcfamilias=t2.idfamilia INNER JOIN cunidadmedida as t3 ON t1.umfkcunidadmedida=t3.idunidadmedida INNER JOIN cpersonal as t7 ON t1.usuarioaltafkcpersonal= t7.idPersona INNER JOIN cmarcas as t8 ON t1. marcafkcmarcas = t8.idmarca  ORDER BY t1.nombreRefaccion ASC;");
            for (int i = 0; i < dt.Rows.Count; i++) tbrefaccion.Rows.Add(dt.Rows[i].ItemArray);
            tbrefaccion.ClearSelection();
        }

        private void tbrefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbrefaccion.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void tbrefaccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
                    int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
                    int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
                    decimal media = 0; if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text);
                    decimal abastecimiento = 0; if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text);
                    int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
                    decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
                    if (!string.IsNullOrWhiteSpace(idRefaccionTemp) && status == 1 && (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) && !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) && proxabastecimiento.Value > DateTime.Now && cbfamilia.SelectedIndex > 0 && cbum.SelectedIndex > 0 && cbmarcas.SelectedIndex > 0 && pasillo > 0 && nivel > 0 && anaquel > 0 && charolafkccharolas > 0 && media > 0 && abastecimiento > 0) && (codrefAnterior != txtcodrefaccion.Text.Trim() || nomrefanterior != v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower()) || (modrefanterior != txtmodeloRefaccion.Text.Trim()) || (proxabastecimiento.Value.ToString("dd / MMMM / yyyy").ToUpper() != ultimoabastecimiento) || (familiaanterior != cbfamilia.SelectedValue.ToString()) || (umAnterior != cbum.SelectedValue.ToString()) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen > 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower()))))
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            button11_Click(null, e);
                        }
                    }
                    guardarReporte(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                cantidada.Clear();
                idRefaccionTemp = tbrefaccion.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbrefaccion.Rows[e.RowIndex].Cells[15].Value.ToString());
                pExistencias.Visible = true;
                if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelref.Text = "Reactivar";
                    }
                    else
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelref.Text = "Desactivar";
                    }

                    pCancelar.Visible = true;
                    pdelref.Visible = true;
                }
                if (Peditar)
                {
                    lblexistencias.Text = v.getExistenciasFromIDRefaccion(idRefaccionTemp);
                    txtcodrefaccion.Text = codrefAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[1].Value;
                    txtnombrereFaccion.Text = nomrefanterior = v.mayusculas(tbrefaccion.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    txtmodeloRefaccion.Text = modrefanterior = tbrefaccion.Rows[e.RowIndex].Cells[3].Value.ToString();
                    proxabastecimiento.Value = Convert.ToDateTime(ultimoabastecimiento = (string)tbrefaccion.Rows[e.RowIndex].Cells[11].Value);
                    cbfamilia.SelectedValue = familiaanterior = tbrefaccion.Rows[e.RowIndex].Cells[16].Value.ToString();
                    if (cbfamilia.SelectedIndex==-1)
                    {
                        if (status==1)
                        {
                            MessageBox.Show("La Familia Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        cbfamilia.SelectedIndex = 0;
                        cbfamilia.Focus();
                    }
                    cbum.SelectedValue = umAnterior = tbrefaccion.Rows[e.RowIndex].Cells[17].Value.ToString();
                    if (cbum.SelectedIndex == -1)
                    {
                        cbum.SelectedIndex = 0;
                        cbum.Focus();
                        MessageBox.Show("La Unidad de Medida Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbmarcas.SelectedValue = marcaAnterior = tbrefaccion.Rows[e.RowIndex].Cells[22].Value.ToString();
                    if (cbmarcas.SelectedIndex == -1)
                    {
                        cbmarcas.SelectedIndex = 0;
                        cbmarcas.Focus();
                        MessageBox.Show("La Marca Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbpasillo.SelectedValue = tbrefaccion.Rows[e.RowIndex].Cells[18].Value;
                    if (cbpasillo.SelectedIndex == -1)
                    {
                        cbpasillo.SelectedIndex = 0;
                        cbpasillo.Focus();
                        MessageBox.Show("El Pasillo Asociado a la Refacción ha sido desactivado.\nSeleccione uno de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbnivel.SelectedValue = nivelAnterior = tbrefaccion.Rows[e.RowIndex].Cells[19].Value.ToString();
                    cbanaquel.SelectedValue = tbrefaccion.Rows[e.RowIndex].Cells[20].Value;
                    if (cbanaquel.SelectedIndex == -1)
                    {
                        cbanaquel.SelectedIndex = 0;
                        cbanaquel.Focus();
                        MessageBox.Show("El Anaquel Asociado a la Refacción ha sido desactivado.\nSeleccione uno de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        cbcharola.SelectedValue = charolaAnterior = tbrefaccion.Rows[e.RowIndex].Cells[21].Value.ToString();
                        if (cbcharola.SelectedIndex == -1)
                        {
                            cbcharola.SelectedIndex = 0;
                            cbcharola.Focus();
                            MessageBox.Show("La Charola Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    txtdesc.Text = descripcionAnterior = v.mayusculas(tbrefaccion.Rows[e.RowIndex].Cells[12].Value.ToString().ToLower());
                    notifmedia.Text = mediaAnterior = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[23].Value).ToString();
                    notifabastecimiento.Text = abastecimientoAnterior = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[24].Value).ToString();
                    ultimacantidad = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[25].Value);
                    editar = true;
                    gbaddrefaccion.Text = "Actualizar Refacción";
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Guardar"; btnsave.Visible = lblsave.Visible = false;
                    notifmedia.Focus();
                    notifabastecimiento.Focus();
                    txtcodrefaccion.Focus();
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

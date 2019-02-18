using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catfallosGrales : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        string codfalloAnterior;
        string nombrefalloAnterior;
        public string idclasfallo;
        public string iddescfallo;
        int status,empresa,area;
        public bool editar, yaAparecioMensaje;
        string idnombrefalloTemp;
        int idUsuario;
        public Form MenuPrincipal;
        public catfallosGrales(int idUsuario,int empresa,int area,Form MenuPrincipal)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbclasificacion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbdescripcion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            this.MenuPrincipal = MenuPrincipal;
        }
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        void getCambios(object sender, EventArgs e)
        {
            if (editar) {
                if (status == 1 && ((cbclasificacion.SelectedIndex>0 && idclasfallo != cbclasificacion.SelectedValue.ToString()) || (cbdescripcion.SelectedIndex > 0 && iddescfallo != cbdescripcion.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtgetdescfallo.Text) && nombrefalloAnterior != v.mayusculas(txtgetdescfallo.Text.ToLower()).Trim()) || (!string.IsNullOrWhiteSpace(lblcodfallo.Text) && codfalloAnterior!=lblcodfallo.Text)))
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                }else
                {

                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }
        public void privilegios()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = '" + Name + "'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
            pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
            peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
            pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            mostrar();
            mdr.Close();
            c.dbcon.Close();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddnomfallo.Visible = true;
            }
            if (pconsultar)
            {
                gbconsulta.Visible = true;
            }
            if (peditar)
            {
                label9.Visible = true;
                label23.Visible = true;
            }
            if(peditar && !pinsertar)
            {
                editar = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Nombre";
            }
        }
        public void iniNombres()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.idfalloEsp,upper(t1.codfallo) as codfallo,UPPER(t1.falloesp) AS falloesp,UPPER(concat(t4.nombres,' ',t4.ApPaterno,' ',t4.ApMaterno)) as nombre,t1.status,UPPER(t2.descfallo) AS descfallo,UPPER(t3.nombreFalloGral) as nombreFalloGral,t2.iddescfallo as iddesc,t3.idFalloGral as idclasif FROM cfallosesp as t1 inner JOIN cdescfallo as t2 ON t1.descfallofkcdescfallo= t2.iddescfallo INNER JOIN cfallosgrales as t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal as t4 ON t1.usuariofkcpersonal = t4.idpersona ORDER BY SUBSTRING(codfallo,LENGTh(codFALLO)-3,4) DESC";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfallos.Rows.Add(dr.GetInt32("idfalloesp"), dr.GetString("NombreFalloGral"), dr.GetString("descfallo"), dr.GetString("codfallo"),dr.GetString("falloesp") ,dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idclasif"), dr.GetString("iddesc"));
                }
                tbfallos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void iniClasificacionesFallos()
        {
            String sql = "SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE status = 1 ORDER BY NombreFalloGral ASC";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            if (Convert.ToInt32(cm.ExecuteScalar())==0)
            {
                MessageBox.Show("No Hay Clasificaciones de Fallos Activas. Por Tanto No Podra Insertar o Editar Nombres de Fallos", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione una Clasificación de Fallo--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbclasificacion.ValueMember = "id";
            cbclasificacion.DisplayMember = "nombre";
            cbclasificacion.DataSource = dt;
            cbclasificacion.ValueMember = "id";
            cbclasificacion.DisplayMember = "nombre";

        } 
        public void busqueda_clasificacion()
        {
            String sql = "SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales ORDER BY NombreFalloGral ASC";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione una Clasificación de Fallo--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbClasificacionb.ValueMember = "id";
            cbClasificacionb.DisplayMember = "nombre";
            cbClasificacionb.DataSource = dt;
            cbClasificacionb.ValueMember = "id";
            cbClasificacionb.DisplayMember = "nombre";
        }

        public void iniDescripcionesFallos()
        {
            String sql = "SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo WHERE status = 1 and falloGralfkcfallosgrales='" + cbclasificacion.SelectedValue + "' ORDER BY descfallo ASC";
            DataTable dt = new DataTable();                    
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            if (cbclasificacion.SelectedIndex > 0)
            {
                if (Convert.ToInt32(cm.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("No Hay Descrpiciones de Fallos Activas.", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbclasificacion.SelectedIndex = 0;
                    cbdescripcion.DataSource = null;
                    cbdescripcion.Enabled = false;
                }else
                {
                    cbdescripcion.DataSource = null;
                    MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
                    AdaptadorDatos.Fill(dt);
                    cbdescripcion.DataSource = dt;
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["id"] = 0;
                    nuevaFila["nombre"] = "--Seleccione una Descripción de Fallo--".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    cbdescripcion.ValueMember = "id";
                    cbdescripcion.DisplayMember = "nombre";
                    cbdescripcion.SelectedIndex = 0;
                    cbdescripcion.Enabled = true;
                }
            }
            else
            {
                cbdescripcion.DataSource = null;
                cbdescripcion.Enabled = false;
            }
            

        }
        private void txtgetdescfallo_TextChanged(object sender, EventArgs e)
        {
            if (cbclasificacion.SelectedIndex>0) {
                if (cbdescripcion.SelectedIndex>0) {
                    if (v.folio == "")
                    {
                        v.setFolio();
                    }
                    if (string.IsNullOrWhiteSpace(txtgetdescfallo.Text))
                    {
                        lblcodfallo.Text = null;
                        v.folio = "";
                    }
                    else {
                        lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + txtgetdescfallo.Text);
                    }
                }else
                {
                    MessageBox.Show("Seleccione una Descripción de Fallo Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgetdescfallo.Clear();
                    cbdescripcion.Focus();
                }
            }
            else
            {
            
                MessageBox.Show("Seleccione una Clasificación de Fallo Para Continuar",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtgetdescfallo.Clear();
                cbclasificacion.Focus();
            }
            getCambios(sender,e);
        }

        private void catNombresFallos_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                iniNombres();
            }
            if (pinsertar || peditar)
            {
                iniClasificacionesFallos();
                busqueda_clasificacion();
            }
        }


        private AutoCompleteStringCollection cargarCodigos()
        {
            AutoCompleteStringCollection data = new AutoCompleteStringCollection();

            MySqlCommand cm = new MySqlCommand("SELECT UPPER(codfallo) as m FROM cfallosesp WHERE codfallo LIKE '" + txtgetcodbusq.Text.Trim() + "%'", c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                data.Add(dr.GetString("m"));
            }
            tbfallos.ClearSelection();
            dr.Close();
            c.dbconection().Close();
            return data;
        }



        private void cbclasificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editar && v.folio !="")
            {
                lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + txtgetdescfallo.Text);
            }
            if (txtgetdescfallo.Text == "")
            {
                lblcodfallo.Text = null;
                v.folio = "";
            }
            if (cbclasificacion.SelectedIndex>0) {
                iniDescripcionesFallos();
            }else
            {
                cbdescripcion.DataSource = null;
                cbdescripcion.Enabled = false;
            }
            }

        private void cbdescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editar && v.folio != "")
            {
                lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + txtgetdescfallo.Text);
            }
            if (txtgetdescfallo.Text == "")
            {
                lblcodfallo.Text = null;
                v.folio = "";
            }
        }

        private void txtgetdescfallo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraNombreFallo(e);
            if (txtgetdescfallo.Text.Equals(null))
            {
                lblcodfallo.Text = null;
                v.folio = "";
            }
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {

                if (!editar)
                {
                    insertar();
                }else
                {
                    editarN();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message+"\n"+ex.HelpLink, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
           
         
                if (!v.formularioNombreFallos(cbclasificacion.SelectedIndex,cbdescripcion.SelectedIndex, txtgetdescfallo.Text.ToLower()) && !v.yaExisteFalloEsp(iddescfallo, txtgetdescfallo.Text.ToLower()))
                {
                string iddescfallo = cbdescripcion.SelectedValue.ToString();
                string nomfallo = txtgetdescfallo.Text.ToLower();
                string codfallo = lblcodfallo.Text;

                var res =c.insertar("INSERT INTO cfallosesp (descfallofkcdescfallo, codfallo,falloesp,usuariofkcpersonal) VALUES('" + iddescfallo + "','" + codfallo + "',LTRIM(RTRIM('" + v.mayusculas(nomfallo) + "')),'"+this.idUsuario+"')");
                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos',(SELECT idfalloesp From cfallosesp Where codfallo='"+codfallo+"'),'Inserción de Fallo','" + idUsuario + "',NOW(),'Inserción de Nombre de Fallo','" + empresa + "','" + area + "')");
                MessageBox.Show("Se ha Insertado el Nombre del Fallo Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);
                    restablecer();
                }else
                {
                    txtgetdescfallo.Clear();
                }
                }
        void editarN()
        {
            if (!string.IsNullOrWhiteSpace(idnombrefalloTemp))
            {


                string iddescfallo = cbdescripcion.SelectedValue.ToString();
                string nomfallo = txtgetdescfallo.Text.ToLower();
                string codfallo = lblcodfallo.Text;
                if (!v.formularioNombreFallos(cbclasificacion.SelectedIndex,cbdescripcion.SelectedIndex,nomfallo))
                {
                    if (this.iddescfallo.Equals(iddescfallo) && this.nombrefalloAnterior.Equals(v.mayusculas(nomfallo)) && codfalloAnterior.Equals(codfallo))
                    {
                        MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                    else
                    {
                        if (status == 1)
                        {
                            if (!v.existenomfalloActualizar(iddescfallo, this.iddescfallo, nomfallo, nombrefalloAnterior))
                            {
                                c.insertar("UPDATE cfallosesp SET descfallofkcdescfallo = '" + iddescfallo + "', codfallo = LTRIM(RTRIM('" + codfallo + "')), falloesp = LTRIM(RTRIM('" + v.mayusculas(nomfallo) + "')) WHERE idfalloEsp='" + this.idnombrefalloTemp + "'");
                                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos','"+idnombrefalloTemp+"','"+iddescfallo+";"+codfalloAnterior+";"+nombrefalloAnterior+"','" + idUsuario + "',NOW(),'Actualización de Nombre de Fallo','" + empresa + "','" + area + "')");
                                if (!yaAparecioMensaje) {
                                    MessageBox.Show("Se ha Actualizado el Nombre del Fallo Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                    restablecer();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se Puede Modificar un Fallo Inactivo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
               
            }else
            {
                MessageBox.Show("Seleccione un Nombre de Fallo Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public void restablecer()
        {
            if (pinsertar)
            {
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                gbaddnomfallo.Text = "Agregar";

            }
            if (pconsultar) {
                iniNombres();
            }
            btnsavemp.Visible = lblsavemp.Visible = true;
            idnombrefalloTemp = null;
            nombrefalloAnterior = null;
            txtgetdescfallo.Clear();
            pCancelar.Visible = false;
            status = 0;
            yaAparecioMensaje = false;
            btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar";
            pEliminarClasificacion.Visible = false;
            cbclasificacion.SelectedIndex = 0;
            txtgetdescfallo.Clear();
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string nomfallo = txtgetdescfallo.Text.Trim();
            if ((!this.iddescfallo.Equals(iddescfallo) || !this.nombrefalloAnterior.Equals(v.mayusculas(nomfallo))) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarN();
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
        }

        private void tbfallos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfallos.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                string nomfallo = txtgetdescfallo.Text.Trim();
                if (!string.IsNullOrWhiteSpace(this.idnombrefalloTemp)&& peditar && this.iddescfallo.Equals(iddescfallo) && this.nombrefalloAnterior.Equals(v.mayusculas(nomfallo)) && status==1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        editarN();
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
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                this.idnombrefalloTemp = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[6].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletedesc.Text = "Reactivar";
                    }
                    else
                    {
                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletedesc.Text = "Desactivar";
                    }
                    pEliminarClasificacion.Visible = true;
                }
                if (peditar)
                {
                    this.codfalloAnterior = tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString();
                    this.nombrefalloAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString().ToLower());
                    cbclasificacion.SelectedValue = tbfallos.Rows[e.RowIndex].Cells[7].Value.ToString();
                    cbdescripcion.SelectedValue = iddescfallo = tbfallos.Rows[e.RowIndex].Cells[8].Value.ToString();
                    if (cbclasificacion.SelectedIndex == -1)
                    {
                        cbclasificacion.SelectedIndex = 0;
                        if (status == 1)
                        {
                            cbclasificacion.Focus();
                            MessageBox.Show("La Clasificación Asociada al Nombre de Fallo ha Sido Desactivado. Seleccione Otro de la lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (cbdescripcion.SelectedIndex == -1)
                    {
                        cbdescripcion.SelectedIndex = 0;
                        if (status == 1)
                        {
                            cbdescripcion.Focus();
                            MessageBox.Show("La Descripción Asociada al Nombre de Fallo ha Sido Desactivada. Seleccione Otra de la Lista Desplegable ", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    txtgetdescfallo.Text = nombrefalloAnterior;
                    idclasfallo = cbclasificacion.SelectedValue.ToString();
                    v.setFolio(codfalloAnterior);
                    lblcodfallo.Text = codfalloAnterior;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    pCancelar.Visible = true;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbaddnomfallo.Text = "Actualizar Nombre de Fallo";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            catClasificaciones catC = new catClasificaciones(this.idUsuario, empresa, area);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            catDescFallos catC = new catDescFallos(this.idUsuario,empresa,area);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void btndeletedesc_Click(object sender, EventArgs e)
        {
            try {
                string msg;
                int status;

                if (this.status == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                   
                    status = 0;
                }
                if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cfallosgrales WHERE idfallogral=(SELECT falloGralfkcfallosgrales FROM cdescfallo WHERE iddescfallo=(SELECT descfallofkcdescfallo FROM cfallosesp WHERE idfalloEsp='" + idnombrefalloTemp + "'))")) == 0 )
                {
                    MessageBox.Show("El Nombre de Fallo No Puede Ser Reactivado Debido a que La Clasificación de Fallo Se Encuentra Desactivada",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                }else if (this.status==0 &&  Convert.ToInt32(v.getaData("SELECT status FROM cdescfallo WHERE iddescfallo=(SELECT descfallofkcdescfallo FROM cfallosEsp WHERE idFalloEsp='" + idnombrefalloTemp + "')")) == 0)
                {

                    MessageBox.Show("El Nombre de Fallo No Puede Ser Reactivado Debido a que La Descripción de Fallo Se Encuentra Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Nombre de Fallo?\n", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE idfalloEsp=" + this.idnombrefalloTemp);
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos','" + idnombrefalloTemp + "','" + msg + "activación de Fallo','" + idUsuario + "',NOW(),'" + msg + "activación de Nombre de Fallo','" + empresa + "','" + area + "')");
                        restablecer();
                        MessageBox.Show("El Nombre de Fallo se ha " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtgetecoBusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string codfallo = txtgetcodbusq.Text;
                restablecer();
                tbfallos.Rows.Clear();
                string sql = "SELECT t1.idfalloEsp,upper(t1.codfallo) as codfallo,UPPER(t1.falloesp) AS falloesp,UPPER(concat(t4.nombres,' ',t4.ApPaterno,' ',t4.ApMaterno)) as nombre,t1.status,UPPER(t2.descfallo) AS descfallo,UPPER(t3.nombreFalloGral) as nombreFalloGral,t2.iddescfallo as iddesc,t3.idFalloGral as idclasif FROM cfallosesp as t1 inner JOIN cdescfallo as t2 ON t1.descfallofkcdescfallo= t2.iddescfallo INNER JOIN cfallosgrales as t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal as t4 ON t1.usuariofkcpersonal = t4.idpersona ";
                string wheres = "";
                if (cbClasificacionb.SelectedIndex > 0)
                {
                    if(wheres == "")
                    {
                        wheres = " WHERE T3.idFalloGral='" + cbClasificacionb.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " AND t3.idFalloGral='" + cbClasificacionb.SelectedValue + "'";
                    }
                }
                if (cbDescFallob.SelectedIndex > 0)
                {
                    if(wheres == "")
                    {
                        wheres = " WHERE t2.iddescfallo='" + cbDescFallob.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " AND t2.iddescfallo='"+cbDescFallob.SelectedValue+"'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(codfallo))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE t1.codfallo LIKE '" + codfallo + "%' ";
                    }
                    else
                    {
                        wheres += "AND t1.codfallo LIKE '" + codfallo + "%' ";
                    }
                }
                if (cbnombrefb.SelectedIndex > 0)
                {
                    if(wheres == "")
                    {
                        wheres = " Where t1.idfalloEsp='" + cbnombrefb.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " ANd t1.idfalloEsp='" + cbnombrefb.SelectedValue + "' ";
                    }
                }
                sql += wheres;
                txtgetcodbusq.Clear();
                cbClasificacionb.SelectedIndex = 0;
                cbDescFallob.SelectedIndex = 0;
                cbnombrefb.SelectedIndex = 0;
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cm.ExecuteScalar())==0) {
                    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    iniNombres();
                } else { MySqlDataReader dr = cm.ExecuteReader();
                 
                    while (dr.Read())
                    {
                        tbfallos.Rows.Add(dr.GetInt32("idfalloesp"), dr.GetString("NombreFalloGral"), dr.GetString("descfallo"), dr.GetString("codfallo"), dr.GetString("falloesp"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idclasif"), dr.GetString("iddesc"));
                    }
                    tbfallos.ClearSelection();
                    dr.Close();
                    pActualizar.Visible = true;
                }
                c.dbconection().Close();
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbclasificacion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetclasifBusq_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtgetcodbusq_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtgetnomBusq_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbaddnomfallo_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "Exportar";
        }
        void ExportarExcel()
        {
            if (tbfallos.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);

                }
                v.exportaExcel(tbfallos);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        Thread exportar;
        private void button5_Click(object sender, EventArgs e)
        {
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        public void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
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

        private void cbClasificacionb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClasificacionb.SelectedIndex > 0)
            {
                iniCombos("SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo WHERE  falloGralfkcfallosgrales='" + cbClasificacionb.SelectedValue + "' ORDER BY descfallo ASC", cbDescFallob, "id", "nombre", "-SELECIONE UNA DESCRIPCIÓN-");
            }
            else
            {
                iniCombos("SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo  ORDER BY descfallo ASC", cbDescFallob, "id", "nombre", "-SELECIONE UNA DESCRIPCIÓN-");
            }
        }

        private void cbDescFallob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDescFallob.SelectedIndex == 0)
            {
                iniCombos("SELECT idfalloEsp id,UPPER(falloesp) as fallo FROM cfallosesp  ORDER BY falloesp ASC", cbnombrefb, "id", "fallo", "-SELECIONE UN NOMBRE-");
            }
            else
            {
                iniCombos("SELECT idfalloEsp id,UPPER(falloesp) as fallo FROM cfallosesp where descfallofkcdescfallo='"+cbDescFallob.SelectedValue+"'  ORDER BY falloesp ASC", cbnombrefb, "id", "fallo", "-SELECIONE UN NOMBRE-");
            }

        }

        private void cbnombrefb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            iniNombres();

            pActualizar.Visible = false;
        }

        private void txtgetdescfallo_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

  

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {

        }
    }
    }


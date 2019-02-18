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

namespace controlFallos
{
    public partial class catDescFallos : Form
    {
        bool editar,yaAparecioMensaje=false;
        string descripcionAnterior;
        string idDescripcion;
        string clasifAnterior;
        conexion c = new conexion();
        int idUsuario,empresa,area;
        int status;
        validaciones v = new validaciones();
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        
        
        public catDescFallos(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbclasificacion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        void getCambios(object sender,EventArgs e)
        {
            if (editar) {
                if (((cbclasificacion.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text)) && (!cbclasificacion.SelectedValue.ToString().Equals(clasifAnterior) || !v.mayusculas(txtgetdescfallo.Text.ToString()).Equals(this.descripcionAnterior))) && status==1)
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                } else
                {
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }

        public void privilegios()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catfallosGrales'";
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

                gbaddclasif.Visible = true;
            }
            if (pconsultar)
            {
                gbclasif.Visible = true;
            }
            if (peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        void iniClasificacionesFallos()
        {
            String sql = "SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE status = 1 ORDER BY NombreFalloGral ASC";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione Una Clasificación--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbclasificacion.DataSource = dt;
            cbclasificacion.ValueMember = "id";
            cbclasificacion.DisplayMember = "nombre";

        }
        void iniDescripciones()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.iddescfallo,upper(t1.descfallo) as descfallo, t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno))as nombre ,UPPER(t3.NombreFalloGral) AS NombreFalloGral,t1.falloGralfkcfallosgrales as idclasif FROM cdescfallo as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona INNER JOIN cfallosgrales as t3 ON  t1.falloGralfkcfallosgrales= t3.idFalloGral ORDER BY t3.NombreFalloGral,t1.descfallo ASC ";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfallos.Rows.Add(dr.GetInt32("iddescfallo"), dr.GetString("NombreFalloGral"), dr.GetString("descfallo") ,dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")),dr.GetString("idclasif"));
                }
                dr.Close();
                c.dbconection().Close();
                tbfallos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void catDescFallos_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pinsertar || peditar)
            {
                iniClasificacionesFallos();

            }
            if (pconsultar)
            {
                iniDescripciones();
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

        private void txtgetclasificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
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
                    editarDe();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string descfallo = txtgetdescfallo.Text.ToLower();
            string clasif = cbclasificacion.SelectedValue.ToString();
            if (cbclasificacion.SelectedIndex>0)
            {
                if (!string.IsNullOrWhiteSpace(descfallo))
                {
                    if (!v.yaExisteDescFallo(descfallo))
                    {
                        if (c.insertar("INSERT INTO cdescfallo (falloGralfkcfallosgrales, descfallo, usuariofkcpersonal) VALUES ('" + clasif + "',LTRIM(RTRIM('" + v.mayusculas(descfallo) + "')),'" + this.idUsuario + "')"))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Descripciones',(SELECT iddescfallo From cdescfallo WHERE falloGralfkcfallosgrales='"+clasif+"' AND descfallo='"+descfallo+"'),'Inserción de Descripción','" + idUsuario + "',NOW(),'Inserción de Descripción','" + empresa + "','" + area + "')");
                            MessageBox.Show("La Descripción del Fallo se ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            restablecer();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("La Descripción del Fallo No puede estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgetdescfallo.Focus();
                }
            }else
            {
                MessageBox.Show("Seleccione una Calsificación de Fallo de la Lista Desplegable",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                cbclasificacion.Focus();
            }
            
        }
        void editarDe()
        {
            if (!string.IsNullOrWhiteSpace(idDescripcion)) {
                if (status == 1) {
                    string desc = txtgetdescfallo.Text.ToLower();
                    string clasif = cbclasificacion.SelectedValue.ToString();
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        if (v.mayusculas(desc).Equals(this.descripcionAnterior) && clasif.Equals(clasifAnterior))
                        {
                            MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(),MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(),MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            { 
                                restablecer();
                            }
                        }
                        else {
                            if (!v.existeDescFalloActualizar(v.mayusculas(desc), descripcionAnterior))
                            {
                                if (c.insertar("UPDATE cdescfallo SET falloGralfkcfallosgrales='" + clasif + "', descfallo = LTRIM(RTRIM('" + v.mayusculas(desc) + "')) WHERE iddescfallo=" + this.idDescripcion))
                                {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Descripciones','" + idDescripcion + "','"+clasifAnterior+";"+descripcionAnterior+"','" + idUsuario + "',NOW(),'Actualización de Descripción','" + empresa + "','" + area + "')");
                                  if(!yaAparecioMensaje) MessageBox.Show("Descripcion Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    restablecer();
                                   
                                }
                            }
                        }
                    } else
                    {
                        MessageBox.Show("El campo Descripción de Fallo no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else
                {
                    MessageBox.Show("No se Puede Editar una Descripción Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Seleccione una Descripcion de Fallos de la Tabla Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                string desc = txtgetdescfallo.Text.ToLower().Trim();
                string clasif = cbclasificacion.SelectedValue.ToString();
                if ((cbclasificacion.SelectedIndex > 0 && !clasif.Equals(clasifAnterior) || (!string.IsNullOrWhiteSpace(desc) && !v.mayusculas(desc).Equals(this.descripcionAnterior))) && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(sender, e);
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
                idDescripcion = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString());
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
                }
                if (peditar)
                {
                    txtgetdescfallo.Text = descripcionAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    cbclasificacion.SelectedValue = clasifAnterior = tbfallos.Rows[e.RowIndex].Cells[5].Value.ToString();
                    pEliminarClasificacion.Visible = true;
                    if (cbclasificacion.SelectedIndex == -1)
                    {
                        cbclasificacion.SelectedIndex = 0;
                    }
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbaddclasif.Text = "Actualizar Descripción de Fallo";
                    pCancelar.Visible = true; btnsavemp.Visible = lblsavemp.Visible = false;
                }
                else
                {
                    MessageBox.Show("Usted no Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            if (pinsertar)
            {
                editar = false;
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                gbaddclasif.Text = "Agregar Descripción de Fallo";
            }
            idDescripcion = null;
            descripcionAnterior = null;
            txtgetdescfallo.Clear();
            cbclasificacion.SelectedIndex = 0;
            pCancelar.Visible = false;
            pEliminarClasificacion.Visible = false;
            yaAparecioMensaje = false;
            if (pconsultar)
            {
                iniDescripciones();

            }
            catfallosGrales catF = (catfallosGrales)this.Owner;
            catF.iniDescripcionesFallos();
            catF.iniNombres();
            catF.restablecer();
            btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar";
            btnsavemp.Visible = true;
            lblsavemp.Visible = true;
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string desc = txtgetdescfallo.Text.ToLower().Trim();
            string clasif = cbclasificacion.SelectedValue.ToString();
            if ((cbclasificacion.SelectedIndex > 0 && !clasif.Equals(clasifAnterior) || (!string.IsNullOrWhiteSpace(desc) && !v.mayusculas(desc).Equals(this.descripcionAnterior))) && status==1 && peditar)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender,e);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try {
                string msg, msg2 = "";
                int status;

                if (this.status == 0)
                {
                    msg = "Re";
                    status = 1;
                } else
                {
                    msg = "Des";
                    msg2 = "Se Desactivarán los Nombres de Fallos Asociados a él";
                    status = 0;
                }
                if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cfallosgrales WHERE idFalloGral=(SELECT falloGralfkcfallosgrales FROM cdescfallo WHERE iddescfallo='" + this.idDescripcion + "')")) == 0)
                {
                    MessageBox.Show("No Se Puede Reactivar La Descripción Puesto Que La Clasificación de Fallo Ha Sido Desactivada",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {

                    if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar la Descripción del Fallo?\n" + msg2, validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        c.insertar("UPDATE cdescfallo SET status = '" + status + "' WHERE iddescfallo=" + this.idDescripcion);
                        c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE descfallofkcdescfallo=" + this.idDescripcion);
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Descripciones','" + idDescripcion + "','" + msg + "activación de Descripción','" + idUsuario + "',NOW(),'" + msg + "activación de Descripción','" + empresa + "','" + area + "')");
                        restablecer();
                        MessageBox.Show("La Descripción y Todos sus Componentes se han " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void gbaddclasif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void txtgetdescfallo_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void cbclasificacion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }
    }
    }
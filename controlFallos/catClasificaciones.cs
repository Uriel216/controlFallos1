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
    public partial class catClasificaciones : Form
    {
        int idUsuario,empresa,area;
        bool editar,yaAparecioMensaje;
        int idFalloTemp;
        string clasificacionAnterior;
        int status;
        conexion c = new conexion();
        validaciones v = new validaciones();
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
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

                gbaddClalif.Visible = true;
            }
            if (pconsultar)
            {
                gbclasif.Visible = true;
            }
            if (peditar)
            {
                label2.Visible = true;
                label23.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        public catClasificaciones(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbfallos.ColumnHeadersDefaultCellStyle = d;
            d.Font = new Font("Garamond", 12, FontStyle.Regular);
            tbfallos.DefaultCellStyle = d;
        }
        void iniClasificaciones()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.idFalloGral,UPPER(t1.nombreFalloGral) AS nombreFalloGral,t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as nombre FROM cfallosgrales as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfallos.Rows.Add(dr.GetInt32("idFalloGral"), dr.GetString("nombreFalloGral"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
                }
                dr.Close();
                c.dbconection().Close();
                tbfallos.ClearSelection();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void catClasificaciones_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                iniClasificaciones();
            }
        }

        private void txtgetcempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
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
                    editarC();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string clasificacion = txtgetclasificacion.Text.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(clasificacion))
            {
                if (!v.yaExisteFalloGral(clasificacion))
                {
                    if (c.insertar("INSERT INTO cfallosgrales(NombreFalloGral,usuariofkcpersonal) VALUES (LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')),'" + idUsuario + "')"))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones',(SELECT idfalloGral From cfallosgrales WHERE nombrefalloGral ='"+clasificacion+"'),'Inserción de Clasificación','" + idUsuario + "',NOW(),'Inserción de Clasificación','" + empresa + "','" + area + "')");
                        MessageBox.Show("Se Ha Insertado La Clasificación del Fallo Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtgetclasificacion.Clear();
                        iniClasificaciones();
                        catfallosGrales catC = (catfallosGrales)this.Owner;
                        catC.iniClasificacionesFallos();
                        catC.iniNombres();
                        if (catC.editar)
                        {
                            catC.cbclasificacion.SelectedValue = catC.idclasfallo;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("El Campo 'Clasificacion de Fallo' No Puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtgetclasificacion.Focus();
            }
        }
        void editarC()
        {
            if (idFalloTemp>0)
            { 
            if (status == 0)
            {
                MessageBox.Show("No se Puede Editar Una Clasificación Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    restablecer();
                }
            }
            else
            {
                string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower());
                if (!string.IsNullOrWhiteSpace(clasificacion))
                {
                    if (!v.mayusculas(clasificacion).Equals(clasificacionAnterior))
                    {
                        if (!v.existeFalloGralActualizar(clasificacion, clasificacionAnterior))
                        {

                            if (c.insertar("UPDATE cfallosgrales SET NombreFalloGral = LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')) WHERE idFalloGral=" + this.idFalloTemp + ""))
                            {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones','" + idFalloTemp + "','"+clasificacionAnterior+"','" + idUsuario + "',NOW(),'Actualización de Clasificación','" + empresa + "','" + area + "')");
                                    if (!yaAparecioMensaje) {
                                        MessageBox.Show("Se ha Actualizado la Clasificación del Fallo Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                        catfallosGrales catC = (catfallosGrales)this.Owner;
                                catC.iniClasificacionesFallos();
                                catC.iniNombres();
                                if (catC.editar)
                                {
                                    catC.cbclasificacion.SelectedValue = catC.idclasfallo;
                                }
                                restablecer();
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("No se Han Realizado Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se Puede Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }else
            {
                MessageBox.Show("Seleccione una Clasificacion de la Tabla para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower().Trim());
                if (idFalloTemp > 0 && peditar && !v.mayusculas(clasificacion).Equals(clasificacionAnterior) && status==1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
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

                idFalloTemp = Convert.ToInt32(tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (pdesactivar)
                {
                    if (v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
                    {

                        btndelcla.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelcla.Text = "Reactivar";
                    }
                    else
                    {

                        btndelcla.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelcla.Text = "Desactivar";
                    }
                    pEliminarClasificacion.Visible = true;
                }
                if (peditar)
                {
                    txtgetclasificacion.Text = clasificacionAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbaddClalif.Text = "Actualizar Clasificación de Fallo";
                    pCancelar.Visible = true;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            idFalloTemp = 0;
            clasificacionAnterior = null;
            txtgetclasificacion.Clear();
            if (pinsertar)
            {

                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                gbaddClalif.Text = "Agregar Clasificación de Fallo";

            }
            pCancelar.Visible = false;
            if (pconsultar)
            {
                iniClasificaciones();
            }
            yaAparecioMensaje = false;
            btnsavemp.Visible = lblsavemp.Visible = true;
            pEliminarClasificacion.Visible = false;
            catfallosGrales catC = (catfallosGrales)this.Owner;
            catC.iniClasificacionesFallos();
            catC.iniNombres();
            catC.restablecer();
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower().Trim());
            if (!v.mayusculas(clasificacion).Equals(clasificacionAnterior) && status==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarC();
                }
                else
                {
                   restablecer();
                }
            }else
            {
                restablecer();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this.status==0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán las Descripciones y Los Nombres de Fallos Asociados a él";
                }

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar la Clasificación de Fallo? \n " + msg2 + "", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res =c.insertar("UPDATE cfallosgrales SET status = '" + status + "' WHERE idFalloGral= " + this.idFalloTemp);
                    var res1 = c.insertar("UPDATE cdescfallo SET status ='" + status + "' WHERE falloGralfkcfallosgrales =" + this.idFalloTemp);
                    String sql = "SELECT iddescfallo AS id FROM cdescfallo WHERE falloGralfkcfallosgrales="+this.idFalloTemp;
                    MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        var resc= c.insertar("UPDATE cfallosesp SET status ='"+status+ "' WHERE descfallofkcdescfallo='"+dr.GetString("id")+"'");

                    }

                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones','" + idFalloTemp + "','" + msg + "activación de Clasificación','" + idUsuario + "',NOW(),'" + msg + "activación de Clasificación','"+empresa+"','"+area+"')");
                    MessageBox.Show("La Clasificación de Fallo se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            msg = null;
                            status = 0;
                            msg2 = null;
               
                    restablecer();
                        
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {
        
        }

        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetclasificacion_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void pEliminarClasificacion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gbaddClalif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void txtgetclasificacion_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (status==1 && (!string.IsNullOrWhiteSpace(txtgetclasificacion.Text) && clasificacionAnterior!= v.mayusculas(txtgetclasificacion.Text.ToLower()).Trim()))
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                }else
                {
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
    }
}

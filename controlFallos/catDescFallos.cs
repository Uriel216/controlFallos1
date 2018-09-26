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
        bool editar;
        string descripcionAnterior;
        string idDescripcion;
        string clasifAnterior;
        conexion c = new conexion();
        int idUsuario;
        int status;
        validaciones v = new validaciones();
        public catDescFallos(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        void iniClasificacionesFallos()
        {
            String sql = "SELECT idFalloGral id,NombreFalloGral as nombre FROM cfallosgrales WHERE status = 1";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);

            cbclasificacion.DataSource = dt;
            cbclasificacion.ValueMember = "id";
            cbclasificacion.DisplayMember = "nombre";

        }
        void iniDescripciones()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.iddescfallo,t1.descfallo, t1.status, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombre ,t3.NombreFalloGral,t1.falloGralfkcfallosgrales as idclasif FROM cdescfallo as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona INNER JOIN cfallosgrales as t3 ON  t1.falloGralfkcfallosgrales= t3.idFalloGral ORDER BY t3.NombreFalloGral,t1.descfallo ASC ";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfallos.Rows.Add(dr.GetInt32("iddescfallo"), dr.GetString("NombreFalloGral"), dr.GetString("descfallo") ,dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")),dr.GetString("idclasif"));
                }
                tbfallos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void catDescFallos_Load(object sender, EventArgs e)
        {
            iniClasificacionesFallos();
            iniDescripciones();
        }

        private void tbfallos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfallos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo")
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
                    editarDe();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string descfallo = txtgetdescfallo.Text.ToLower();
            string clasif = cbclasificacion.SelectedValue.ToString();
            if (!string.IsNullOrWhiteSpace(descfallo))
            {
                if (!v.yaExisteDescFallo(descfallo))
                {
                    if (c.insertar("INSERT INTO cdescfallo (falloGralfkcfallosgrales, descfallo, usuariofkcpersonal) VALUES ('"+clasif+ "',LTRIM(RTRIM('" + v.mayusculas(descfallo)+"')),'"+this.idUsuario+"')"))
                    {
                        MessageBox.Show("La Descripción del Fallo se ha Agregado Correctamente");
                        txtgetdescfallo.Clear();
                        catfallosGrales catF = (catfallosGrales)this.Owner;
                        catF.iniDescripcionesFallos();
                        iniDescripciones();
                        catF.iniNombres();
                    }
                }
            }
            else
            {
                MessageBox.Show("La Descripción del Fallo No puede estar Vacío","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void editarDe()
        {
            if (status==1) {
                string desc = txtgetdescfallo.Text.ToLower();
                string clasif = cbclasificacion.SelectedValue.ToString();
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    if (v.mayusculas(desc).Equals(this.descripcionAnterior) && clasif.Equals(clasifAnterior))
                    {
                        MessageBox.Show("No se Realizaron Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                    else {
                        if (!v.existeDescFalloActualizar(v.mayusculas(desc), descripcionAnterior))
                        {
                            if (c.insertar("UPDATE cdescfallo SET falloGralfkcfallosgrales='" + clasif + "', descfallo = LTRIM(RTRIM('" + v.mayusculas(desc) + "')) WHERE iddescfallo=" + this.idDescripcion))
                            {
                                MessageBox.Show("Descripcion Actualizada Exitosamente");
                                restablecer();
                                catfallosGrales catF = (catfallosGrales)this.Owner;
                                catF.iniDescripcionesFallos();
                                catF.iniNombres();
                                if (catF.editar)
                                {
                                    catF.cbdescripcion.SelectedValue = catF.iddescfallo;
                                }
                            }
                        }
                    }
                } else
                {
                    MessageBox.Show("El campo Descripción de Fallo no puede Estar Vacío");
                }
            }else
            {
                MessageBox.Show("No se Puede Editar una Descripción Desactivada");
            }
        }
        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString()) == 0)
                {
               
                    btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldeletedesc.Text = "Reactivar Descripción";
                }
                else
                {
                   
                    btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldeletedesc.Text = "Desactivar Descripción";
                }
               
                idDescripcion = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                descripcionAnterior = tbfallos.Rows[e.RowIndex].Cells[2].Value.ToString();
                clasifAnterior = tbfallos.Rows[e.RowIndex].Cells[5].Value.ToString();
                cbclasificacion.SelectedValue = clasifAnterior;
                txtgetdescfallo.Text = descripcionAnterior;
                pEliminarClasificacion.Visible = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Descripcion";
                editar = true;
                gbClasificacion.Text = "Editar Descripción de Fallo";
                pCancelar.Visible = true;
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            idDescripcion = null;
            descripcionAnterior = null;
            txtgetdescfallo.Clear();
            btnsavemp.BackgroundImage = Properties.Resources.save;
            lblsavemp.Text = "Agregar Descripción";
            editar = false;
            gbClasificacion.Text = "Agregar Descripción de Fallo";
            pCancelar.Visible = false;
            pEliminarClasificacion.Visible = false;
            iniDescripciones();
            btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar Descripción";
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            restablecer();
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
                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar la Descripción del Fallo?\n" + msg2,"Control de  Fallos",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    c.insertar("UPDATE cdescfallo SET status = '"+status+"' WHERE iddescfallo="+this.idDescripcion);
                    c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE descfallofkcdescfallo=" + this.idDescripcion);
                    restablecer();
                    MessageBox.Show("La Descripción y Todos sus Componentes se han "+msg+"activado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    catfallosGrales catF = (catfallosGrales)this.Owner;
                    catF.iniDescripcionesFallos();
                    catF.iniNombres();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }
    }
    }


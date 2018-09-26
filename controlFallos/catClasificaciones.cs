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
        int idUsuario;
        bool editar;
        int idFalloTemp;
        string clasificacionAnterior;
        bool reactivar;
        int status;
        conexion c = new conexion();
        validaciones v = new validaciones();
        public catClasificaciones(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        void iniClasificaciones()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.idFalloGral,t1.nombreFalloGral,t1.status, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombre FROM cfallosgrales as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfallos.Rows.Add(dr.GetInt32("idFalloGral"), dr.GetString("nombreFalloGral"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
                }
                tbfallos.ClearSelection();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void catClasificaciones_Load(object sender, EventArgs e)
        {
            iniClasificaciones();
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
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string clasificacion = txtgetclasificacion.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(clasificacion))
            {
                if (!v.yaExisteFalloGral(clasificacion))
                {
                    if (c.insertar("INSERT INTO cfallosgrales(NombreFalloGral,usuariofkcpersonal) VALUES (LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')),'" + idUsuario + "')"))
                    {
                        MessageBox.Show("Se Ha Insertado La Clasificación del Fallo Correctamente");
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
                MessageBox.Show("La Clasificacion del Fallo es requerida para Inserción","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void editarC()
        {
            if (status == 0) {
                MessageBox.Show("No se Puede Editar Una Clasificación Desactivada","Control Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    restablecer();
                }
            } else {
                string clasificacion = txtgetclasificacion.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(clasificacion))
                {
                    if (!v.mayusculas(clasificacion).Equals(clasificacionAnterior))
                    {
                        if (!v.existeFalloGralActualizar(clasificacion, clasificacionAnterior))
                        {
                           
                                if (c.insertar("UPDATE cfallosgrales SET NombreFalloGral = LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')) WHERE idFalloGral=" + this.idFalloTemp + ""))
                                {
                                    MessageBox.Show("Se ha Actualizado la Clasificación del Fallo Exitosamente", "Control de fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("No se Han Realizado Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                } else
                {
                    MessageBox.Show("No se Puede Actualizar", "control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
                {

                    btndelcla.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldelcla.Text = "Reactivar Clasificación";
                }
                else
                {

                    btndelcla.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldelcla.Text = "Desactivar Clasificación";
                }
                idFalloTemp = Convert.ToInt32(tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString());
                clasificacionAnterior = tbfallos.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtgetclasificacion.Text = clasificacionAnterior;
                pEliminarClasificacion.Visible = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Clasificación";
                editar = true;
                gbClasificacion.Text = "Editar Clasificación de Fallo";
                pCancelar.Visible = true;
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            idFalloTemp = 0;
            clasificacionAnterior = null;
            txtgetclasificacion.Clear();
            btnsavemp.BackgroundImage = Properties.Resources.save;
            lblsavemp.Text = "Agregar Clasificación";
            editar = false;
            gbClasificacion.Text = "Agregar Clasificación de Fallo";
            pCancelar.Visible = false;
            iniClasificaciones();
            reactivar = false;
            pEliminarClasificacion.Visible = false;
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            restablecer();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (reactivar)
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

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar la Clasificación de Fallo? \n " + msg2 + "", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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


                    MessageBox.Show("La Clasificación de Fallo se " + msg + "activó Correctamente");
                            msg = null;
                            status = 0;
                            msg2 = null;
                    catfallosGrales catC = (catfallosGrales)this.Owner;
                    catC.iniClasificacionesFallos();
                    catC.iniNombres();
                    restablecer();
                        
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

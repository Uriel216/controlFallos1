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
    public partial class catfallosGrales : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        string codfalloAnterior;
        string nombrefalloAnterior;
        public string idclasfallo;
        public string iddescfallo;
        int status;
        public bool editar;
        string idnombrefalloTemp;
        int idUsuario;
        public catfallosGrales(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        public void iniNombres()
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.idfalloEsp,t1.codfallo,t1.falloesp,concat(t4.nombres,' ',t4.ApPaterno,' ',t4.ApMaterno) as nombre,t1.status,t2.descfallo,t3.nombreFalloGral,t2.iddescfallo as iddesc,t3.idFalloGral as idclasif FROM cfallosesp as t1 inner JOIN cdescfallo as t2 ON t1.descfallofkcdescfallo= t2.iddescfallo INNER JOIN cfallosgrales as t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal as t4 ON t1.usuariofkcpersonal = t4.idpersona";
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
            String sql = "SELECT idFalloGral id,NombreFalloGral as nombre FROM cfallosgrales WHERE status = 1";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbclasificacion.DataSource = dt;
            cbclasificacion.ValueMember = "id";
            cbclasificacion.DisplayMember = "nombre";

        }
        public void iniDescripcionesFallos()
        {
            String sql = "SELECT iddescfallo id,descfallo as nombre FROM cdescfallo WHERE status = 1 and falloGralfkcfallosgrales='"+cbclasificacion.SelectedValue+"'";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbdescripcion.DataSource = dt;
            cbdescripcion.ValueMember = "id";
            cbdescripcion.DisplayMember = "nombre";

        }
        private void txtgetdescfallo_TextChanged(object sender, EventArgs e)
        {
           
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
          
        }

        private void catNombresFallos_Load(object sender, EventArgs e)
        {
            iniClasificacionesFallos();
            iniNombres();
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
            iniDescripcionesFallos();
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
            v.letrasynumeros(e);
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
                MessageBox.Show(ex.Message+"\n"+ex.HelpLink,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string iddescfallo = cbdescripcion.SelectedValue.ToString();
            string nomfallo = txtgetdescfallo.Text.ToLower();
            string codfallo = lblcodfallo.Text;
            if (!string.IsNullOrWhiteSpace(nomfallo))
            {
                if (!v.yaExisteFalloEsp(iddescfallo, v.mayusculas(nomfallo)))
                {
                    var res =c.insertar("INSERT INTO cfallosesp (descfallofkcdescfallo, codfallo,falloesp,usuariofkcpersonal) VALUES('" + iddescfallo + "','" + codfallo + "',LTRIM(RTRIM('" + v.mayusculas(nomfallo) + "')),'"+this.idUsuario+"')");
                    MessageBox.Show("Se ha Insertado el Nombre del Fallo Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    restablecer();
                }else
                {
                    txtgetdescfallo.Clear();
                }
            }
            else
            {
                MessageBox.Show("No se Puede Insertar el Nombre de Fallo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void editarN()
        {
            string iddescfallo = cbdescripcion.SelectedValue.ToString();
            string nomfallo = txtgetdescfallo.Text.ToLower();
            string codfallo = lblcodfallo.Text;
            if (!string.IsNullOrWhiteSpace(nomfallo)) {
                if (this.iddescfallo.Equals(iddescfallo) && this.nombrefalloAnterior.Equals(v.mayusculas(nomfallo)))
                {
                    MessageBox.Show("No se Realizaron Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        restablecer();
                    }
                } else {
                    if (status == 1)
                    {
                        if (!v.existenomfalloActualizar(iddescfallo, this.iddescfallo, nomfallo, nombrefalloAnterior))
                        {
                            c.insertar("UPDATE cfallosesp SET descfallofkcdescfallo = '" + iddescfallo + "', codfallo = LTRIM(RTRIM('" + codfallo + "')), falloesp = LTRIM(RTRIM('" + v.mayusculas(nomfallo) + "')) WHERE idfalloEsp='" + this.idnombrefalloTemp + "'");
                            MessageBox.Show("Se ha Actualizado el Nombre del Fallo Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            restablecer();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se Puede Modificar un Fallo Inactivo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    }
            }else {
                MessageBox.Show("El Campo Nombre de Fallo no Puede Estar Vacío","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            idnombrefalloTemp = null;
            nombrefalloAnterior = null;
            txtgetdescfallo.Clear();
            btnsavemp.BackgroundImage = Properties.Resources.save;
            lblsavemp.Text = "Agregar Descripción";
            editar = false;
            gbClasificacion.Text = "Agregar Nombre de Fallo";
            pCancelar.Visible = false;
            iniNombres();
            status = 0;
            btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar Nombre";
            pEliminarClasificacion.Visible = false;
            cbclasificacion.SelectedIndex = 0;
            cbdescripcion.SelectedIndex = 0;
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            restablecer();
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
                var res = tbfallos.Rows[e.RowIndex].Cells[6].Value.ToString();
                if (v.getStatusInt(res) == 0)
                {
                    btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldeletedesc.Text = "Reactivar Nombre";
                }else
                {
                    btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldeletedesc.Text = "Desactivar Nombre";
                }
                this.idnombrefalloTemp = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.codfalloAnterior = tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString();
                this.nombrefalloAnterior = tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString();
                this.idclasfallo = tbfallos.Rows[e.RowIndex].Cells[7].Value.ToString();
                this.iddescfallo = tbfallos.Rows[e.RowIndex].Cells[8].Value.ToString();
                cbclasificacion.SelectedValue = idclasfallo;
                cbdescripcion.SelectedValue = iddescfallo;
                txtgetdescfallo.Text = nombrefalloAnterior;
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[6].Value.ToString());
                v.setFolio(codfalloAnterior);
                lblcodfallo.Text = codfalloAnterior;
                pEliminarClasificacion.Visible = true;
                pCancelar.Visible = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Nombre";
                editar = true;
                gbClasificacion.Text = "Editar Nombre de Fallo";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            catClasificaciones catC = new catClasificaciones(this.idUsuario);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            catDescFallos catC = new catDescFallos(this.idUsuario);
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
                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Nombre de Fallo?\n", "Control de  Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE idfalloEsp=" + this.idnombrefalloTemp);
                    restablecer();
                    MessageBox.Show("El Nombre de Fallo se ha " + msg + "activado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string clasiffallo=v.mayusculas(txtgetclasifBusq.Text.ToLower());
                string descfallo = v.mayusculas(txtgetdescBusq.Text.ToLower());
                string codfallo = txtgetcodbusq.Text;
                string nomfallo = v.mayusculas(txtgetnomBusq.Text.ToLower());
                tbfallos.Rows.Clear();
                string sql = "SELECT t1.idfalloEsp,t1.codfallo,t1.falloesp,concat(t4.nombres,' ',t4.ApPaterno,' ',t4.ApMaterno) as nombre,t1.status,t2.descfallo,t3.nombreFalloGral,t2.iddescfallo as iddesc,t3.idFalloGral as idclasif FROM cfallosesp as t1 inner JOIN cdescfallo as t2 ON t1.descfallofkcdescfallo= t2.iddescfallo INNER JOIN cfallosgrales as t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal as t4 ON t1.usuariofkcpersonal = t4.idpersona ";
                string wheres = "";
                if (!string.IsNullOrWhiteSpace(clasiffallo))
                {
                    if (wheres=="")
                    {
                        wheres = "WHERE t3.nombreFalloGral LIKE '" + clasiffallo + "%' ";
                    }
                    else
                    {
                        wheres += "OR t3.nombreFalloGral LIKE '" + clasiffallo + "%' ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(descfallo))
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE t2.descfallo LIKE '" + descfallo + "%' ";
                    }
                    else
                    {
                        wheres += "OR t2.descfallo LIKE '" + descfallo + "%' ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(codfallo))
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE t1.codfallo LIKE '" + codfallo + "%' ";
                    }
                    else
                    {
                        wheres += "OR t1.codfallo LIKE '" + codfallo + "%' ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(nomfallo))
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE t1.falloesp LIKE '" + nomfallo + "%' ";
                    }
                    else
                    {
                        wheres += "OR t1.falloesp LIKE '" + nomfallo + "%' ";
                    }
                }
                sql += wheres;
                txtgetclasifBusq.Clear();
                txtgetdescBusq.Clear();
                txtgetcodbusq.Clear();
                txtgetnomBusq.Clear();
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cm.ExecuteScalar())==0) {
                    MessageBox.Show("No se Encontraron Resultados","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                } else { MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        tbfallos.Rows.Add(dr.GetInt32("idfalloesp"), dr.GetString("NombreFalloGral"), dr.GetString("descfallo"), dr.GetString("codfallo"), dr.GetString("falloesp"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idclasif"), dr.GetString("iddesc"));
                    }
                    tbfallos.ClearSelection();
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            iniNombres();
        }

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {

        }
    }
    }


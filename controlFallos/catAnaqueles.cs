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
    public partial class catAnaqueles : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        string idpasilloTemp;
        string pasilloAnterior;
        string pasilloValueAnterior;
        bool editar;
        int _state;
        int idUsuario;
        public catAnaqueles(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        void _insertarPasillo()
        {

            string pasillo = txtpasillo.Text;
            if (!string.IsNullOrWhiteSpace(pasillo))
            {
                if (!v.existeAnaquel(cbpasillo.SelectedValue.ToString(), pasillo))
                {

                    string sql = "INSERT INTO canaqueles (pasillofkcpasillos,anaquel,usuariofkcpersonal) VALUES('"+cbpasillo.SelectedValue+"','" + pasillo + "','"+idUsuario+"')";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("Anaquel Insertado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        ubicaciones ub = (ubicaciones)this.Owner;
                        ub.busqUbic();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Se puede Insertar el Anaquel", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void busqUbic()
        {
            String sql = "SELECT idpasillo id, pasillo as nombre FROM cpasillos WHERE status = 1";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);

            cbpasillo.DataSource = dt;
            cbpasillo.ValueMember = "id";
            cbpasillo.DisplayMember = "nombre";

            c.dbcon.Close();
        }
        void limpiar()
        {
            txtpasillo.Clear();
            insertarpasillos();
            btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsavemp.Text = "Agregar Pasillo";
            editar = false;
            idpasilloTemp = null;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            gbClasificacion.Text = "Agregar Pasillo";
            idpasilloTemp = null;
            pasilloAnterior = null;
            _state = 0;
            cbpasillo.SelectedIndex = 0;
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        void _editarPasillo()
        {
            string pasillo = txtpasillo.Text;
            if (!string.IsNullOrWhiteSpace(pasillo))
            {
                if (!v.existeAnaquelActualizar(pasilloValueAnterior, pasillo, pasilloAnterior))
                {
                    string sql = "UPDATE canaqueles SET pasillofkcpasillos='"+cbpasillo.SelectedValue+"',  anaquel= '" + pasillo + "' WHERE idanaquel= '" + idpasilloTemp + "'";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("Anaquel Acualizado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        ubicaciones u = (ubicaciones) Owner;
                        u.insertarUbicaciones();
                        u.busqUbic();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Se puede Editar el Pasillo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void insertarpasillos()
        {
            try
            {
                tbubicaciones.Rows.Clear();
                string sql = "SELECT t1.idanaquel as id,t1.anaquel as p,t1.status as s, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombres,t3.idpasillo,t3.pasillo FROM canaqueles as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona INNER JOIN cpasillos as t3 ON t1.pasillofkcpasillos=t3.idpasillo ORDER BY t3.pasillo,t1.anaquel ASC";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbubicaciones.Rows.Add(dr.GetString("id"), dr.GetString("pasillo"), dr.GetString("p"), dr.GetString("nombres"), v.getStatusString(dr.GetInt32("s")),dr.GetString("idpasillo"));
                }
                tbubicaciones.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {
            txtpasillo.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void catAnaqueles_Load(object sender, EventArgs e)
        {
            insertarpasillos();
            busqUbic();
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertarPasillo();
                }else {
                    _editarPasillo();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[4].Value.ToString()) == 0)
                {

                    btndelpa.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldelpa.Text = "Reactivar Anaquel";
                }
                else
                {

                    btndelpa.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldelpa.Text = "Desactivar Anaquel";
                }
                idpasilloTemp = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                pasilloAnterior = txtpasillo.Text = (string)tbubicaciones.Rows[e.RowIndex].Cells[2].Value;
               cbpasillo.SelectedValue = tbubicaciones.Rows[e.RowIndex].Cells[5].Value;
                pasilloValueAnterior = cbpasillo.SelectedValue.ToString();
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
                pdelete.Visible = true;
                pCancelar.Visible = true;
                _state = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[4].Value.ToString());
                gbClasificacion.Text = "Editar Anauqel";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

        private void btndelpa_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this._state == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán Las Charolas Asociadas al Anaquel";
                }

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Anaquel? \n " + msg2 + "", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = c.insertar("UPDATE canaqueles SET status = '" + status + "' WHERE idanaquel= " + this.idpasilloTemp);
                    var res1 = c.insertar("UPDATE ccharolas SET status ='" + status + "' WHERE anaquelfkcanaqueles =" + this.idpasilloTemp);
                   


                    MessageBox.Show("El Pasillo se " + msg + "activó Correctamente");
                    msg = null;
                    status = 0;
                    msg2 = null;
                    ubicaciones u = (ubicaciones)Owner;
                    u.insertarUbicaciones();
                    u.busqUbic();
                    limpiar();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Conttrol de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbubicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbubicaciones.Columns[e.ColumnIndex].Name == "Estatus")
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
    }
}

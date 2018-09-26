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
    public partial class catPasillos : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        string idpasilloTemp;
        string pasilloAnterior;
        bool editar;
        int _state;
        int idUsuario;
        public catPasillos(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        void _insertarPasillo()
        {

            string pasillo = txtpasillo.Text;
            if (!string.IsNullOrWhiteSpace(pasillo)) {
                if (!v.existePasillo(pasillo))
                {

                    string sql = "INSERT INTO cpasillos (pasillo,usuariofkcpersonal) VALUES('" + pasillo + "','"+this.idUsuario+"')";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("Pasillo Insertado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        ubicaciones ub = (ubicaciones)this.Owner;
                        ub.busqUbic();
                    }
                }
            }else
            {
                MessageBox.Show("No Se puede Insertar el Pasillo","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _editarPasillo()
        {
            string pasillo = txtpasillo.Text;
            if (!string.IsNullOrWhiteSpace(pasillo)) {
                if (!v.existePasilloActualizar(pasillo, pasilloAnterior))
                {
                    string sql = "UPDATE cpasillos SET pasillo= '"+pasillo+"' WHERE idpasillo= '"+this.idpasilloTemp+"'";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("Pasillo Acualizado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        ubicaciones u = (ubicaciones)Owner;
                        u.insertarUbicaciones();
                        u.busqUbic();
                    }
                }
            }else
            {
                MessageBox.Show("No Se puede Editar el Pasillo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            gbPasillo.Text = "Editar Pasillo";
            idpasilloTemp = null;
            pasilloAnterior = null;
            _state = 0;
        }
        public void insertarpasillos()
        {try
            {
                tbubicaciones.Rows.Clear();
                string sql = "SELECT t1.idpasillo as id,t1.pasillo as p,t1.status as s, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombres FROM cpasillos as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbubicaciones.Rows.Add(dr.GetString("id"), dr.GetString("p"),  dr.GetString("nombres"), v.getStatusString(dr.GetInt32("s")));
                }
                tbubicaciones.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }

        private void txtpasillo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar) {
                    _insertarPasillo();
                }else
                {
                    _editarPasillo();
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void catPasillos_Load(object sender, EventArgs e)
        {
            insertarpasillos();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this._state ==0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán los Anaqueles y Las Charolas Asociados al Pasillo";
                }

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Pasillo? \n " + msg2 + "", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = c.insertar("UPDATE cpasillos SET status = '" + status + "' WHERE idpasillo= " + this.idpasilloTemp);
                    var res1 = c.insertar("UPDATE canaqueles SET status ='" + status + "' WHERE pasillofkcpasillos =" + this.idpasilloTemp);
                    String sql = "SELECT idanaquel AS id FROM canaqueles WHERE pasillofkcpasillos=" + this.idpasilloTemp;
                    MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        var resc = c.insertar("UPDATE ccharolas SET status ='" + status + "' WHERE anaquelfkcanaqueles='" + dr.GetString("id") + "'");
                    }


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

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
                {

                    btndelpa.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldelpa.Text = "Reactivar Pasillo";
                }
                else
                {

                    btndelpa.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldelpa.Text = "Desactivar Pasillo";
                }
                idpasilloTemp = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                pasilloAnterior = txtpasillo.Text = (string)tbubicaciones.Rows[e.RowIndex].Cells[1].Value;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Pasillo";
                editar = true;
                pdelete.Visible = true;
               pCancelar.Visible = true;
                  _state = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[3].Value.ToString());
                gbPasillo.Text = "Editar Pasillo";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void tbubicaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

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
using System.Globalization;

namespace controlFallos
{
    public partial class catEmpresas : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        bool editbussines = false;
        int bussinestemp,idUsuario,status;
        string claveAnterior, nombreAnterior;
        public catEmpresas(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
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
            c.dbcon.Close();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbadd.Visible = true;
            }
            if (pconsultar)
            {
                gbcempresa.Visible = true;
            }
            if (peditar)
            {
                label1.Visible = true;
                label23.Visible = true;
            }
        }

        private void txtgetnempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void txtgetcempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editbussines)
                {

                    insertarEmpresa();

                }
                else
                {

                    editarEmpresa();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void limpiar()
        {
            if (pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Agregar Empresa";
                editbussines = false;
            }
            if (pconsultar)
            {
                busqempr();
            }
            txtgetcempresa.Clear();
            txtgetnempresa.Clear();
            pEliminarEmpresa.Visible = false;
            pCancel.Visible = false;
            busqempresa.ClearSelection();
           catUnidades cat = (catUnidades)this.Owner;
            cat.busqempresas();
            cat.bunidades();
        }
        public void insertarEmpresa()
        {
            string clave = txtgetcempresa.Text;
            string nombre = v.mayusculas(txtgetnempresa.Text.ToLower()); 
            if (!v.formularioEmpresa(clave,nombre) && !v.yaExisteEmpresa(txtgetcempresa.Text, txtgetnempresa.Text))
            {
                    String sql = "INSERT INTO cempresas (claveEmpresa,nombreEmpresa,usuariofkcpersonal) VALUES ('" + clave + "','" + nombre + "','"+this.idUsuario+"')";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("La Empresa se Ha Insertado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        limpiar();
                    }

             
            }
        }
        public void editarEmpresa()
        {
            if (bussinestemp>0) {
                string clave = txtgetcempresa.Text;
                string nombre = v.mayusculas(txtgetnempresa.Text.ToLower());
                if (!v.formularioEmpresa(clave, nombre)) {
                    if (clave.Equals(claveAnterior) && nombre.Equals(nombreAnterior))
                    {
                        MessageBox.Show("No se Realizaron Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else
                    {
                        if (!v.existeEmpresaActualizar(clave, claveAnterior, nombre, nombreAnterior)) {
                            String sql = "UPDATE cempresas SET claveEMpresa = '" + txtgetcempresa.Text + "', nombreEmpresa ='" + txtgetnempresa.Text + "' WHERE idEmpresa = " + this.bussinestemp;
                            if (c.insertar(sql))
                            {
                                MessageBox.Show("Se Ha Actualizado La Empresa Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                            }
                        }
                    }
                }
            }else
            {
                MessageBox.Show("Seleccione una Empresa Para Actualizar","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public void busqempr()
        {
            busqempresa.Rows.Clear();
            String sql = "SELECT t1.idempresa,t1.claveEmpresa,t1.nombreEmpresa, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)as persona, t1.status FROM cempresas as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona  ORDER BY claveEmpresa ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqempresa.Rows.Add(dr.GetInt32("idempresa"), dr.GetString("claveEmpresa"), dr.GetString("nombreEmpresa"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")));
            }
            busqempresa.ClearSelection();
        }

        private void busqempresa_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.bussinestemp = Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(busqempresa.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = Properties.Resources.up;
                        lbldelete.Text = "Reactivar Empresa";
                    }
                    else
                    {
                        btndelete.BackgroundImage = Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar Empresa";
                    }
                    pEliminarEmpresa.Visible = true;

                   }
                if (peditar)
                {
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Editar Empresa";
                    txtgetcempresa.Text = claveAnterior = busqempresa.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtgetnempresa.Text = nombreAnterior = busqempresa.Rows[e.RowIndex].Cells[2].Value.ToString();
                    editbussines = true;
                    pCancel.Visible = true;
                    busqempresa.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (status==0)
            {
                state = 1;
                msg = "Re";
            }else
            {
                state = 0;
                msg = "Des";
            }

            if (MessageBox.Show("¿Desea "+msg+"activar la Empresa?", "Control de Fallos",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
          == DialogResult.Yes)
            {
                String sql = "UPDATE cempresas SET status = '"+state+"' WHERE idEmpresa = '" + this.bussinestemp + "'";
                if (c.insertar(sql))
                {
                    MessageBox.Show("La Empresa se ha "+msg+"activado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    limpiar();

                }
                else
                {
                    MessageBox.Show("Unidad no Desactivada");
                }
          ;
            }
        }

        private void gbEmp_Enter(object sender, EventArgs e)
        {

        }

        private void catEmpresas_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                busqempr();
            }
        }

        private void busqempresa_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (busqempresa.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

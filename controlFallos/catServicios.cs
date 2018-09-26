using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catServicios : Form
    {
        int idservicetemp = 0; bool editarservice = false;
        conexion c = new conexion();
        int idUsuario;
        validaciones v = new validaciones();
        int status;
        string nombreAnterior, descripcionAnterior;
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public catServicios(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
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
                gbservicios.Visible = true;
            }
            if (peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void txtgetclave_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void txtgetnombre_s_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void btnsaves_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editarservice)
                {
                    insertar();
                }
                else
                {
                    editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void editar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string des = v.mayusculas(txtgetnombre_s.Text.ToLower());
            if (nombre.Equals(nombreAnterior) && des.Equals(descripcionAnterior))
            {
                MessageBox.Show("No se Han Realizado Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    limpiar();
                }
            } else {
                if (!v.formularioServicio(nombre, des) && !v.existeServicioActualizar(nombre,nombreAnterior)) {

                    String sql = "UPDATE cservicios SET Nombre = '" + nombre + "', Descripcion = '" + des + " ' WHERE idservicio =" + this.idservicetemp;
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("El Servicio Se Ha Actualizado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        limpiar();

                    }
                  


                }
            }

        }
        void limpiar()
        {
            if (pinsertar)
            {
                editarservice = false;
                btnsaves.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsaves.Text = "Guardar Servicio";
            }
            if (pconsultar)
            {
                busqservices();

            }
            pCancelar.Visible = false;
            idservicetemp = 0;
            pEliminarService.Visible = false;          
            txtgetclave.Clear();
            txtgetnombre_s.Clear();
            catUnidades cat = (catUnidades)Owner;
            cat.busqserviciosaComboBox();
            cat.bunidades();
        }
        void insertar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string descripcion = v.mayusculas(txtgetnombre_s.Text.ToLower());
            if (!v.formularioServicio(nombre, descripcion) && !v.yaExisteServicio(nombre))
            {

                String sql = "INSERT INTO cservicios (Nombre,Descripcion,usuariofkcpersonal) VALUES ('" + nombre + "','" + descripcion + "','" + this.idUsuario + "')";
                if (c.insertar(sql))
                {
                    MessageBox.Show("El Servicio se Ha Agregado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    limpiar();

                }
              
            }


            
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int state;
                string msg;
                if (status==0)
                {
                    state = 1;
                    msg = "Re";
                }else
                {
                    state = 0;
                    msg = "Des";
                }
                if (MessageBox.Show("¿Desea "+msg+"activar El Servicio?", "Control de Fallos",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
                {
                    String sql = "UPDATE cservicios SET status = '"+state+"' WHERE idservicio = '" + this.idservicetemp + "'";
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("El Servicio se Ha "+msg+"activado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("Servicio no Desactivado");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public void busqservices()
        {
            dataGridView2.Rows.Clear();
            String sql = "SELECT t1.idservicio,t1.Nombre,t1.Descripcion,t1.status, CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) AS persona FROM cservicios as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona ";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView2.Rows.Add(dr.GetInt32("idservicio"), dr.GetString("Nombre"), dr.GetString("Descripcion"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")));
            }
            dataGridView2.ClearSelection();
            c.dbcon.Close();
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idservicetemp = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt((string)dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (pdesactivar) {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar Servicio";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar Servicio";
                    }
                    pEliminarService.Visible = true;
                }
                if (peditar)
                {
                    editarservice = true;
                    txtgetclave.Text = nombreAnterior = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtgetnombre_s.Text = descripcionAnterior = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    dataGridView2.ClearSelection();
                    btnsaves.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsaves.Text = "Editar Servicio";
                    pCancelar.Visible = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Seleccione una fila para Editar "); Console.WriteLine(ex);
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            pCancelar.Visible = false;
            idservicetemp = 0;
            busqservices();
            editarservice = false;
            pEliminarService.Visible = false;

            btnsaves.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsaves.Text = "Guardar Servicio";
            txtgetclave.Clear();
            txtgetnombre_s.Clear();
        }

        private void catServicios_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                busqservices();
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Estatus")
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

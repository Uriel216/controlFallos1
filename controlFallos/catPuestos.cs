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
    public partial class catPuestos : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario, empresa,area,idpuesto,status;
        string puestoAnterior;
        bool editar;
        public catPuestos(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }
        void limpiar()
        {
            txtgetpuesto.Clear();
            idpuesto = 0;
            editar = false;
            btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsave.Text = "Agregar Puesto";
            gbpuesto.Text = "Nuevo Puesto";
            busquedapuestos();
            pcancel.Visible = true;
            catPersonal cat = (catPersonal)Owner;
            cat.busemp();
            cat.busqPuestos();
            pdelete.Visible = false;
            pcancel.Visible = false;
        }

        private void btnguardarpuesto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetpuesto.Text))
            {

                if (!editar)
                {

                    insertar();
                }
                else
                {
                    _editar();
                }
            }
        }

        private void gbpuestos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
               idpuesto =Convert.ToInt32(gbpuestos.Rows[e.RowIndex].Cells[0].Value.ToString());
               txtgetpuesto.Text = puestoAnterior = gbpuestos.Rows[e.RowIndex].Cells[1].Value.ToString();
               gbpuestos.ClearSelection();
                gbpuesto.Visible = true;
                pcancel.Visible = true;
                editar = true;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsave.Text = "Editar Puesto";
                gbpuesto.Text = "Editar Puesto";
                pdelete.Visible = true;
                status = v.getStatusInt(gbpuestos.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (status == 0)
                {
                    btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldelete.Text = "Reactivar Puesto";
                }
                else
                {
                    btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldelete.Text = "Desactivar Puesto";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            int state;
            string msg;
            if (status==0)
            {
                state = 1;
                msg = "Re";
            }
            else
            {
                state = 0;
                msg = "Des";
            }
            if (MessageBox.Show("¿Desea " + msg + "activar el Puesto?", "Control de Fallos",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
            {
                try
                {

                    String sql = "UPDATE puestos SET status = " + state + " WHERE idpuesto  = " + this.idpuesto;
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("El Puesto ha sido " + msg + "activado");
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("El empleado no ha sido desactivado");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void txtgetpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void gbpuestos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void catPuestos_Load(object sender, EventArgs e)
        {
            busquedapuestos();
        }

        private void gbpuestos_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void btndelete_Click_1(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (this.status==0)
            {

                msg = "Re";
                state = 1;
                    }
            else
            {
                state = 0;
                msg = "Des";

            }
            if (MessageBox.Show("¿Esta Seguro de "+msg+"activar el Puesto?", "Control de Fallos",
      MessageBoxButtons.YesNo, MessageBoxIcon.Question)
      == DialogResult.Yes)
            {
                try
                {
                    String sql = "UPDATE puestos SET status = " + state + " WHERE idpuesto  = " + idpuesto;
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("El Puesto ha sido " + msg + "activado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }else
                {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        void _editar()
        {
            if (idpuesto > 0) {
                string puesto = v.mayusculas(txtgetpuesto.Text.ToLower());
                if (!string.IsNullOrWhiteSpace(puesto)) {
                    if (!puesto.Equals(puestoAnterior)) {
                        String sql = "UPDATE puestos SET puesto ='" + puesto + "' WHERE idpuesto = " + this.idpuesto;
                        if (c.insertar(sql))
                        {
                            MessageBox.Show("El Puesto Se Ha Actualizado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                        else
                        {
                            MessageBox.Show("Ha ocurrido un error");
                        }
                    }else
                    {
                        MessageBox.Show("No Se Realizó Ningún Cambio Al Puesto", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes ) {
                            limpiar();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El Nombre del Puesto no puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                    MessageBox.Show("Seleccione un Puesto Para Actualizar", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        void insertar()
        {
            string puesto = v.mayusculas(txtgetpuesto.Text.ToLower());
            if (!v.yaExistePuesto(puesto))
            {

                String sql = "INSERT INTO puestos (puesto,empresa,area,usuariofkcpersonal) VALUES('" + puesto + "','" + empresa + "','" + area + "','"+idUsuario+"')";
                if (c.insertar(sql))
                {
                    MessageBox.Show("El Puesto Se Ha Insertado Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    limpiar();

                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error");
                }
            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public void busquedapuestos()
        {
            gbpuestos.Rows.Clear();
            String sql = "SELECT t1.idpuesto as id, t1.puesto, t1.status,CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as persona  FROM puestos as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.empresa = '" + empresa + "' and t1.area ='"+area+"' ORDER BY puesto ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                gbpuestos.Rows.Add(dr.GetString("id"), dr.GetString("puesto"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")));
            }
        
            gbpuestos.ClearSelection();
        }
    }
}

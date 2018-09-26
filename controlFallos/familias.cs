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
    public partial class familias : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idfamTemp;
        bool reactivar;
        string familiaAnterior,descAnterior;
        int _status;
        bool editar;
        int idUsuario;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public familias(int idUsuario)
        {
            InitializeComponent();
            txtnombre.Focus();
            this.idUsuario = idUsuario;
        }
        public void establecerPrivilegios()
        {
            string sql = "SELECT insertar,consultar,editar, desactivar  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
                Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
                Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
                Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            }
            c.dbcon.Close();
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddfamilia.Visible = true;
            }
            if (Pconsultar)
            {
                gbfamilias.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            try{
                if (!editar)
                {
                    _insertar();;
                }else
                {
                    _editar();
                }
            }catch(Exception ex)

            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            string nombre = v.mayusculas(txtnombre.Text.ToLower());
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower());
            if (!v.formulariofamilias(nombre, desc) && !v.existeFamilia(nombre, desc))
            {

                if (c.insertar("INSERT INTO cfamilias (familia,descripcionFamilia,usuariofkcpersonal) VALUES('" + v.mayusculas(nombre.ToLower()) + "','" + v.mayusculas(desc.ToLower()) + "','" + idUsuario + "')"))
                {
                    MessageBox.Show("Familia Insertada Exitosamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void _editar()
        {
            string nombre = v.mayusculas(txtnombre.Text.ToLower());
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower());
            if (!v.formulariofamilias(nombre, desc) && !v.existefamiliaActualizar(nombre, familiaAnterior, desc, descAnterior))
            {
                if (nombre.Equals(familiaAnterior) && desc.Equals(descAnterior))
            {
                MessageBox.Show("No se Realizaron Modificaciones","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desea Limpiar los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Information)==DialogResult.Yes)
                    {
                        limpiar();
                    }
            }else { 
              
                    if (c.insertar("UPDATE cfamilias SET familia ='" + v.mayusculas(nombre.ToLower()) + "', descripcionFamilia='" + v.mayusculas(desc.ToLower()) + "' WHERE idfamilia=" + this.idfamTemp))
                    {
                        MessageBox.Show("Familia Actualizada Exitosamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                }
            }
        }
        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }
        public void insertarfamilias()
        {
            try
            {
                tbfamilias.Rows.Clear();
                string sql = "SELECT t1.idfamilia,t1.familia,t1.descripcionfamilia,t1.status,CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombre FROM cfamilias as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfamilias.Rows.Add(dr.GetString("idfamilia"), dr.GetString("familia"), dr.GetString("descripcionfamilia"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
                }
                tbfamilias.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }

        private void tbfamilias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfamilias.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void familias_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
            {
                insertarfamilias();
            }
        }

        private void tbfamilias_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.idfamTemp = Convert.ToInt32(tbfamilias.Rows[e.RowIndex].Cells[0].Value);
                _status = v.getStatusInt(tbfamilias.Rows[e.RowIndex].Cells[4].Value.ToString());

                if (Pdesactivar){
                    pdeletefam.Visible = true;
                    if (_status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletefam.Text = "Reactivar Familia";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar Familia";
                    }

                }
                if (Peditar)
                {
                    txtnombre.Text = familiaAnterior = (string)tbfamilias.Rows[e.RowIndex].Cells[1].Value;
                    txtdescfamilia.Text = descAnterior = (string)tbfamilias.Rows[e.RowIndex].Cells[2].Value;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Editar Familia";
                    pcancel.Visible = true;
                    editar = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            if (idfamTemp>0)
            {
                string msg;
                string texto;
                int status;
                if (reactivar)
                {
                    texto = "¿Desea Reactivar la Familia de Refacciones";
                    status = 1;
                    msg = "Reactivado";
                }
                else
                {
                    texto = "¿Desea Desactivar la Familia de Refacciones?";
                    status = 0;
                    msg = "Desactivado";

                }
                if (MessageBox.Show(texto, "Control de Fallos",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
                {
                    try
                    {
                        String sql = "UPDATE cfamilias SET status = " + status + " WHERE idfamilia  = " + this.idfamTemp;
                        if (c.insertar(sql))
                        {
                            MessageBox.Show("La Familia de Refacciones ha sido " + msg, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            insertarfamilias();
                        } else
                        {
                            MessageBox.Show("La Familia de Refacciones no ha sido " + msg, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsave.Text = "Agregar Familia";

            }
            if (Pconsultar)
            {
                insertarfamilias();

            }
            txtnombre.Clear();
            txtdescfamilia.Clear();
            reactivar = false;
            idfamTemp = 0;
            pcancel.Visible = false;
            pdeletefam.Visible = false;
    }
    }
}

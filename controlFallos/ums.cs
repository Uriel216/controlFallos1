using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace controlFallos
{
    public partial class ums : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idumTemp;
        bool reactivar;
        string nombreAnterior, simboloAnterior;
        int idUsuario;
        int status;
        bool editar;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public ums(int idUsuario)
        {
            InitializeComponent();
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

                gbaddum.Visible = true;
            }
            if (Pconsultar)
            {
                gbum.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void button5_Click(object sender, EventArgs e)
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
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _editar()
        {
            string um = v.mayusculas(txtumedida.Text.ToLower());
            string _simbolo = txtsimbolo.Text;
            if (um.Equals(nombreAnterior) && _simbolo.Equals(simboloAnterior))
            {
                MessageBox.Show("No se Realizaron Cambios", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    limpiar();
                }
            } else {
                if (!v.formularioums(um, _simbolo) && !v.existeUMActualizar(um, this.nombreAnterior, _simbolo, this.simboloAnterior))
                {
                    var res = c.insertar("UPDATE cunidadmedida SET Nombre ='" + um + "', Simbolo ='" + _simbolo + "' WHERE idunidadmedida=" + this.idumTemp);
                    MessageBox.Show("Se ha Actualizado la Unidad de Medida Exitosamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void insertar()
        {

            string um =v.mayusculas(txtumedida.Text.ToLower());
            string _simbolo = txtsimbolo.Text;
            if (!v.formularioums(um, _simbolo) && !v.existeUM(um, _simbolo))
            {
                if (c.insertar("INSERT INTO cunidadmedida(Nombre,Simbolo,usuariofkcpersonal) VALUES('" + um + "','" + _simbolo + "','" + this.idUsuario + "')"))
                {
                    MessageBox.Show("Unidad de Medida Insertada", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        private void txtumedida_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }
        public void insertarums()
        {
            tbum.Rows.Clear();
            string sql = "SELECT t1.idunidadmedida,t1.Nombre,t1.Simbolo,t1.status,CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombre FROM cunidadmedida as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbum.Rows.Add(dr.GetString("idunidadmedida"), dr.GetString("Nombre"), dr.GetString("Simbolo"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
            }
            tbum.ClearSelection();
        }

        private void tbum_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbum.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void ums_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
            {
                insertarums();
            }
        
        }

        private void tbum_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try{
                idumTemp = Convert.ToInt32(tbum.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbum.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar)
                {
                    pdeleteum.Visible = true;

                    if (status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeleteum.Text = "Reactivar Ud. Medida";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeleteum.Text = "Deactivar Ud. Medida";
                    }
                }
                if (Peditar)
                {
                    editar = true;
                    txtumedida.Text = nombreAnterior = (string)tbum.Rows[e.RowIndex].Cells[1].Value;
                    txtsimbolo.Text = simboloAnterior = (string)tbum.Rows[e.RowIndex].Cells[2].Value;
                    padd.Visible = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Editar Ud. Medida";
                    gbaddum.Text = "Editar Unidad de Medida";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {

            if (idumTemp> 0)
            {
                string msg;
                string texto;
                int status;
                if (reactivar)
                {
                    texto = "¿Desea Reactivar la Unidad de Medida";
                    status = 1;
                    msg = "Reactivada";
                }
                else
                {
                    texto = "¿Desea Desactivar la Unidad de Medida?";
                    status = 0;
                    msg = "Desactivada";

                }
                if (MessageBox.Show(texto, "Control de Fallos",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
                {
                    try
                    {
                        String sql = "UPDATE cunidadmedida SET status = " + status + " WHERE idunidadmedida  = " + this.idumTemp;
                        if (c.insertar(sql))
                        {
                            MessageBox.Show("La Unidad de Medida ha sido " + msg);
                            limpiar();
                            insertarums();
                        }
                        else
                        {
                            MessageBox.Show("La Unidad de Medida no ha sido " + msg);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                lblsave.Text = "Agregar Ud. Medida";
              
            }
            if (Pconsultar)
            {

                insertarums();
            }
            padd.Visible = false;
            pdeleteum.Visible = false;
            idumTemp = 0;
            reactivar = false;
            txtsimbolo.Clear();
            txtumedida.Clear();
            gbaddum.Text = "Agregar Unidad de Medida";
          
        }
    }
}

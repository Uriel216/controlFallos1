using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace controlFallos
{
    public partial class marcas : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        int idmarcaTemp,idUsuario,status;
        bool reactivar,editar;
        string marcaAnterior;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }

        public marcas(int idUsuario)
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

                gbadd.Visible = true;
            }
            if (Pconsultar)
            {
                gbconsulta.Visible = true;
            }
            if (Peditar)
            {
                label2.Visible = true;
                label3.Visible = true;
            }
        }
        private void marcas_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
            {
                insertarums();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertar();
                }
                else
                {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _editar()
        {
            string marca = v.mayusculas(txtmarca.Text.ToLower());
            if (!string.IsNullOrWhiteSpace(marca))
            {
                if (marca.Equals(marcaAnterior)) {
                    MessageBox.Show("No se Realizaron Cambios","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desa Limpiar los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        limpiar();
                    }
                }else { 
                    if (!v.existeMarcaActualizar(marca, this.marcaAnterior))
                    {
                        if (c.insertar("UPDATE cmarcas SET marca='" + marca + "' WHERE idmarca='" + this.idmarcaTemp + "'"))
                        {
                            MessageBox.Show("Marca Actualizada Existosamante", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                }
            }else
            {
                MessageBox.Show("No se Puede Actualizar la Marca","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            string marca =v.mayusculas(txtmarca.Text.ToLower());
            if (!string.IsNullOrWhiteSpace(marca))
            {
                if (!v.existeMarca(marca))
                {
                    if (c.insertar("INSERT INTO cmarcas(marca,personafkcpersonal) VALUES('" + marca + "','" + this.idUsuario + "')"))
                    {
                        MessageBox.Show("Marca Agregada Existosamante", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("La Marca no se Agrego", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("El Campo Marca no Puede Estar Vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void insertarums()
        {
            tbmarcas.Rows.Clear();
            string sql = "SELECT t1.idmarca,t1.marca,t1.status,CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as nombre FROM cmarcas as t1 INNER JOIN cpersonal as t2 On t1.personafkcpersonal= t2.idpersona";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbmarcas.Rows.Add(dr.GetString("idmarca"), dr.GetString("marca"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
            }
            tbmarcas.ClearSelection();
        }

        private void tbmarcas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbmarcas.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void btncancel_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void txtmarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void tbmarcas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.idmarcaTemp = Convert.ToInt32(tbmarcas.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbmarcas.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletefam.Text = "Reactivar Marca";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar Marca";

                    }
                    pdeletefam.Visible = true;
                }
                if (Peditar)
                {
                    txtmarca.Text = marcaAnterior = (string)tbmarcas.Rows[e.RowIndex].Cells[1].Value;
                    pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Editar Marca";
                    tbmarcas.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            if (idmarcaTemp > 0)
            {
                string msg;
                string texto;
                int status;
                if (reactivar)
                {
                    texto = "¿Desea Reactivar la Marca";
                    status = 1;
                    msg = "Reactivada";
                }
                else
                {
                    texto = "¿Desea Desactivar la Marca?";
                    status = 0;
                    msg = "Desactivada";

                }
                if (MessageBox.Show(texto, "Control de Fallos",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
                {
                    try
                    {
                        String sql = "UPDATE cmarcas SET status = " + status + " WHERE idmarca  = " + this.idmarcaTemp;
                        if (c.insertar(sql))
                        {
                            MessageBox.Show("La Marca ha sido " + msg);
                            limpiar();
                            insertarums();
                        }
                        else
                        {
                            MessageBox.Show("La Marca no ha sido " + msg);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsave.Text = "Agregar Marca";
                editar = false;
            }
            if (Pconsultar)
            {

                insertarums();
            }
            txtmarca.Clear();
            this.idmarcaTemp = 0;
            reactivar = false;
            pdeletefam.Visible = false;
            pcancel.Visible = false;
           
            marcaAnterior = null;
        
        }
    }
}

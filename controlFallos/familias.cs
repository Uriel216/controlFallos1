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
        int _status,empresa,area;
        bool editar,yaAparecioMensaje;
        int idUsuario;

        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        new catRefacciones Owner;
        public familias(int idUsuario,int empresa, int area,Form fh)
        {
            InitializeComponent();
            txtnombre.Focus();
            this.idUsuario = idUsuario;
            tbfamilias.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            Owner = (catRefacciones)fh;
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbfamilias.ColumnHeadersDefaultCellStyle = d;
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (_status == 1 && ((!string.IsNullOrWhiteSpace(txtnombre.Text) && familiaAnterior != v.mayusculas(txtnombre.Text.ToLower().Trim())) || (!string.IsNullOrWhiteSpace(txtdescfamilia.Text) && descAnterior != v.mayusculas(txtdescfamilia.Text.ToLower().Trim()))))
                {
                    btnsave.Visible = lblsave.Visible = true;
                } else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
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
            mdr.Close();
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
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            string nombre = v.mayusculas(txtnombre.Text.ToLower());
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower());
            if (!v.formulariofamilias(nombre, desc) && !v.existeFamilia(nombre, desc))
            {

                if (c.insertar("INSERT INTO cfamilias (familia,descripcionFamilia,usuariofkcpersonal) VALUES(LTRIM(RTRIM('" + v.mayusculas(nombre.ToLower()) + "')),LTRIM(RTRIM('" + v.mayusculas(desc.ToLower()) + "')),'" + idUsuario + "')"))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Familias',(SELECT idfamilia FROM cfamilias WHERE familia='"+nombre+ "' AND descripcionFamilia='"+desc+"'),'"+nombre+";"+desc+"','" + idUsuario + "',NOW(),'Inserción de Familia','" + empresa + "','" + area + "')");
                    Owner.familia = v.getaData("SELECT idfamilia FROM cfamilias WHERE familia='" + nombre + "' AND descripcionFamilia='" + desc + "'").ToString();
                    MessageBox.Show("Familia Insertada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void _editar()
        {
            string nombre = v.mayusculas(txtnombre.Text.ToLower());
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower());
            if (this._status==1) {
                if (!v.formulariofamilias(nombre, desc) && !v.existefamiliaActualizar(nombre, familiaAnterior, desc, descAnterior))
                {
                    if (nombre.Equals(familiaAnterior) && desc.Equals(descAnterior))
                    {
                        MessageBox.Show("No se Realizaron Modificaciones", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else {

                        if (c.insertar("UPDATE cfamilias SET familia =LTRIM(RTRIM('" + v.mayusculas(nombre.ToLower()) + "')), descripcionFamilia=LTRIM(RTRIM('" + v.mayusculas(desc.ToLower()) + "')) WHERE idfamilia=" + this.idfamTemp))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Familias','"+idfamTemp+"','" + familiaAnterior + ";" + descAnterior + "','" + idUsuario + "',NOW(),'Actualización de Familia','" + empresa + "','" + area + "')");
                           if(!yaAparecioMensaje) MessageBox.Show("Familia Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                }
            }else
            {
                MessageBox.Show("No se Puede Modificar una Familia Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                string sql = "SELECT t1.idfamilia,upper(t1.familia) as familia,UPPER(t1.descripcionfamilia) as descripcionfamilia,t1.status,UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as nombre FROM cfamilias as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfamilias.Rows.Add(dr.GetString("idfamilia"), dr.GetString("familia"), dr.GetString("descripcionfamilia"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
                }
                dr.Close();
                c.dbconection().Close();
                tbfamilias.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }

        private void tbfamilias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfamilias.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
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
            if (e.RowIndex>=0)
            {
                if (idfamTemp>0 && Peditar &&(!(v.mayusculas(txtnombre.Text.ToLower())).Trim().Equals(familiaAnterior) || !v.mayusculas(txtdescfamilia.Text.ToLower()).Trim().Equals(descAnterior)) && _status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button10_Click(null, e);
                    }
                    else
                    {
                        guardarReporte(e);
                    }
                }
                else
                {
                    guardarReporte(e);
                }
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                this.idfamTemp = Convert.ToInt32(tbfamilias.Rows[e.RowIndex].Cells[0].Value);
                _status = v.getStatusInt(tbfamilias.Rows[e.RowIndex].Cells[4].Value.ToString());

                if (Pdesactivar)
                {
                    pdeletefam.Visible = true;
                    if (_status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletefam.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar";
                    }

                }
                if (Peditar)
                {
                    txtnombre.Text = familiaAnterior = v.mayusculas(tbfamilias.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    txtdescfamilia.Text = descAnterior = v.mayusculas(tbfamilias.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbaddfamilia.Text = "Actualizar Familia de Refacciones";
                    pcancel.Visible = true;
                    editar = true; btnsave.Visible = lblsave.Visible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!(v.mayusculas(txtnombre.Text.ToLower())).Trim().Equals(familiaAnterior) || !v.mayusculas(txtdescfamilia.Text.ToLower()).Trim().Equals(descAnterior) && _status==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button10_Click(null, e);
                }
                else
                {
                    limpiar();
                }
            }
            else
            {
                limpiar();
            }
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
                    msg = "Re";
                }
                else
                {
                    texto = "¿Desea Desactivar la Familia de Refacciones?";
                    status = 0;
                    msg = "Des";

                }
                if (MessageBox.Show(texto, validaciones.MessageBoxTitle.Confirmar.ToString(),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
                {
                    try
                    {
                        String sql = "UPDATE cfamilias SET status = " + status + " WHERE idfamilia  = " + this.idfamTemp;
                        if (c.insertar(sql))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Familias','" + this.idfamTemp+ "','" + msg + "activación de Familia','" + idUsuario + "',NOW(),'" + msg + "activación de Unidad de Medida','" + empresa + "','" + area + "')");
                            MessageBox.Show("La Familia de Refacciones ha sido " + msg+"activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            insertarfamilias();
                        } else
                        {
                            MessageBox.Show("La Familia de Refacciones no ha sido " + msg+ "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void gbaddfamilia_Enter(object sender, EventArgs e)
        {

        }

        private void tbfamilias_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void gbaddfamilia_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void tbfamilias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pcancel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtnombre_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsave.Text = "Agregar";

                gbaddfamilia.Text = "Agregar Familia de Refacciones";

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
            yaAparecioMensaje = false;
    }
    }
}

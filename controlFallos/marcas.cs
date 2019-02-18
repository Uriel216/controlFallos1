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
        int idmarcaTemp,idUsuario,status,empresa,area;
        bool reactivar,editar;
        string marcaAnterior;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        new catRefacciones Owner;
        public marcas(int idUsuario,int empresa,int area,Form fh)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            tbmarcas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            Owner = (catRefacciones)fh;
        }
        void getCambios(object sender,EventArgs e)
        {
            if (editar) {
                if (status==1 && !string.IsNullOrWhiteSpace(txtmarca.Text) && marcaAnterior != v.mayusculas(txtmarca.Text.ToLower().Trim()))
                {
                    btnsave.Visible =lblsave.Visible = true;
                }else
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
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _editar()
        {
            string marca = v.mayusculas(txtmarca.Text.ToLower());
            if (this.status==1) {
                if (!string.IsNullOrWhiteSpace(marca))
                {
                    if (marca.Equals(marcaAnterior)) {
                        MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desa Limpiar los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else {
                        if (!v.existeMarcaActualizar(marca, this.marcaAnterior))
                        {
                            if (c.insertar("UPDATE cmarcas SET marca=LTRIM(RTRIM('" + marca + "')) WHERE idmarca='" + this.idmarcaTemp + "'"))
                            {
                                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Marcas','"+idmarcaTemp+"','" + marca + "','" + idUsuario + "',NOW(),'Actualización de Marca','" + empresa + "','" + area + "')");
                                if(!yaAparecioMensaje)MessageBox.Show("Marca Actualizada Existosamante", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                            }
                        }
                    }
                } else
                {
                    MessageBox.Show("No se Puede Actualizar la Marca", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("No se Puede Modificar una Marca Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            string marca =v.mayusculas(txtmarca.Text.ToLower());
            if (!string.IsNullOrWhiteSpace(marca))
            {
                if (!v.existeMarca(marca))
                {
                    if (c.insertar("INSERT INTO cmarcas(marca,personafkcpersonal) VALUES(LTRIM(RTRIM('" + marca + "')),'" + this.idUsuario + "')"))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Marcas',(SELECT idmarca FROM cmarcas WHERE marca='"+marca+"'),'"+marca+"','" + idUsuario + "',NOW(),'Inserción de Marca','" + empresa + "','" + area + "')");
                        Owner.marca =v.getaData("SELECT idmarca FROM cmarcas WHERE marca='" + marca + "'").ToString();
                        MessageBox.Show("Marca Agregada Existosamante", validaciones.MessageBoxTitle.Información.ToString() ,MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("La Marca no se Agrego", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("El Campo Marca no Puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void insertarums()
        {
            tbmarcas.Rows.Clear();
            string sql = "SELECT t1.idmarca,upper(t1.marca) as marca,t1.status, Upper(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as nombre FROM cmarcas as t1 INNER JOIN cpersonal as t2 On t1.personafkcpersonal= t2.idpersona";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbmarcas.Rows.Add(dr.GetString("idmarca"), dr.GetString("marca"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
            }
            dr.Close();
            c.dbconection().Close();
            tbmarcas.ClearSelection();
        }

        private void tbmarcas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbmarcas.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void btncancel_Click(object sender, EventArgs e)
        {
            if (!v.mayusculas(txtmarca.Text.ToLower()).Trim().Equals(marcaAnterior) && status==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button2_Click(null, e);
                }
                else
                {
                    limpiar();
                }
            } else
            {
                limpiar();
            }
        }

        private void txtmarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void tbmarcas_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void gbadd_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void tbmarcas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (idmarcaTemp>0 && !v.mayusculas(txtmarca.Text.ToLower()).Trim().Equals(marcaAnterior) && status==1 && Peditar)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button2_Click(null, e);
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

        private void txtmarca_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void guardarReporte(DataGridViewCellEventArgs e)
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
                        lbldeletefam.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar";

                    }
                    pdeletefam.Visible = true;
                }
                if (Peditar)
                {
                    txtmarca.Text = marcaAnterior = v.mayusculas(tbmarcas.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbadd.Text = "Actualizar Marca";
                    tbmarcas.ClearSelection();
                    btnsave.Visible = lblsave.Visible = false;

                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    texto = "¿Está Suguro Que Desea Reactivar la Marca";
                    status = 1;
                    msg = "Re";
                }
                else
                {
                    texto = "¿Está Suguro Que Desactivar la Marca?";
                    status = 0;
                    msg = "Des";

                }
                if (MessageBox.Show(texto, validaciones.MessageBoxTitle.Confirmar.ToString(),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                  == DialogResult.Yes)
                {
                    try
                    {
                        String sql = "UPDATE cmarcas SET status = " + status + " WHERE idmarca  = " + this.idmarcaTemp;
                        if (c.insertar(sql))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Marcas','" + idmarcaTemp + "','" + msg + "activación de Marca','" + idUsuario + "',NOW(),'" + msg + "activación de Marca','" + empresa + "','" + area + "')");
                            MessageBox.Show("La Marca ha sido " + msg+" Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);
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
                        MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbadd.Text =  "Agregar Marca";
                lblsave.Text = "Agregar";
                btnsave.Visible = lblsave.Visible = true;
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
            yaAparecioMensaje = false;
            btnsave.Visible = lblsave.Visible = true;
            marcaAnterior = null;
        
        }
    }
}

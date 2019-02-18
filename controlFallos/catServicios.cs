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
        int status,empresa,area;
        string nombreAnterior, descripcionAnterior,empresaAnterior;
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        public catServicios(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            dataGridView2.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel); v.iniCombos("SELECT idempresa,UPPER(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE status=1 ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--");
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
            mdr.Close();
            c.dbconection().Close();
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editarservice) {
                if (status == 1 && (cbempresa.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetclave.Text.Trim()) && !string.IsNullOrWhiteSpace(txtgetnombre_s.Text.Trim())) && (!empresaAnterior.Equals(cbempresa.SelectedValue.ToString()) || !nombreAnterior.Equals(v.mayusculas(txtgetclave.Text.Trim().ToLower())) || !descripcionAnterior.Equals(v.mayusculas(txtgetnombre_s.Text.Trim().ToLower())) ) )
                //if (status == 1 && ((!string.IsNullOrWhiteSpace(txtgetclave.Text) && nombreAnterior != v.mayusculas(txtgetclave.Text.ToLower().Trim()) && cbempresa.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtgetnombre_s.Text))  && ( !nombreAnterior.Equals(txtgetclave.Text.Trim()) || descripcionAnterior != v.mayusculas(txtgetnombre_s.Text.Trim().ToLower().Trim('-')) || !empresaAnterior.Equals(cbempresa.SelectedValue.ToString()))))
                {
                    btnsaves.Visible = lblsaves.Visible = true;
                } else
                {
                    btnsaves.Visible = lblsaves.Visible = false;
                }
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddservice.Visible = true;
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
            v.paraUnidades(e);
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
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void editar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string des = v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim('-');
            int empresa = Convert.ToInt32(cbempresa.SelectedValue);
            if (nombre.Equals(nombreAnterior) && des.Equals(descripcionAnterior))
            {
                MessageBox.Show("No se Han Realizado Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    limpiar();
                }
            } else {
                if (!v.formularioServicio(nombre, des,empresa) && !v.existeServicioActualizar(nombre,nombreAnterior)) {

                    String sql = "UPDATE cservicios SET Nombre = LTRIM(RTRIM('" + nombre + "')), Descripcion = LTRIM(RTRIM('" + des + "')) WHERE idservicio =" + this.idservicetemp;
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Servicios','"+idservicetemp+"','Nombre: "+nombreAnterior+";Descripción: "+descripcionAnterior+"','" + idUsuario + "',NOW(),'Actualización de Servicio','" + empresa + "','" + area + "')");
                        if (!yaAparecioMensaje)
                        {
                            MessageBox.Show("El Servicio Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
                gbaddservice.Text = "Agregar Servicio";
                lblsaves.Text = "Guardar";
            }
            if (pconsultar)
            {
                busqservices();

            }
            yaAparecioMensaje = false;
            btnsaves.Visible = lblsaves.Visible = true;
            pCancelar.Visible = false;
            idservicetemp = 0;
            pEliminarService.Visible = false;          
            txtgetclave.Clear();
            txtgetnombre_s.Clear();
         
            catUnidades cat = (catUnidades)Owner;
            if (cat.csetEmpresa.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idservicio, UPPER(nombre) as nombre FROM cservicios WHERE status=1 ORDER BY nombre ASC WHERE empresafkcempresas='" + cat.csetEmpresa.SelectedValue + "'", cat.cbservicio, "idservicio", "nombre", "--SELECCIONE UN SERVICIO--");

                cat.cbservicio.Enabled = true;
            }
            else
            {
                cat.cbservicio.DataSource = null;
                cat.cbservicio.Enabled = false;
            }
            cat.bunidades();
        }
        void insertar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string descripcion = v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim('-');
            int bussiness = Convert.ToInt32(cbempresa.SelectedValue);
            if (!v.formularioServicio(nombre, descripcion, bussiness) && !v.yaExisteServicio(nombre))
            {
                String sql = "INSERT INTO cservicios (Nombre,Descripcion,usuariofkcpersonal,empresafkcempresas) VALUES (LTRIM(RTRIM('" + nombre + "')),LTRIM(RTRIM('" + descripcion + "')),'" + this.idUsuario + "','"+bussiness+"')";
                if (c.insertar(sql))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Servicios',(SELECT idservicio FROM cservicios WHERE empresafkcempresas='"+bussiness+"' and Nombre='"+nombre+"' and Descripcion='"+descripcion+"'),'Inserción de Servicio','" + idUsuario + "',NOW(),'Inserción de Servicio','" + empresa + "','" + area + "')");
                    MessageBox.Show("El Servicio se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
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
                if (MessageBox.Show("¿Desea "+msg+"activar El Servicio?",validaciones.MessageBoxTitle.Confirmar.ToString(),
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
                {
                    String sql = "UPDATE cservicios SET status = '"+state+"' WHERE idservicio = '" + this.idservicetemp + "'";
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Servicios','" + idservicetemp + "','" + msg + "activación de Servicio','" + idUsuario + "',NOW(),'" + msg + "activación de Servicio','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Servicio se Ha "+msg+"activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
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
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public void busqservices()
        {
            dataGridView2.Rows.Clear();
            String sql = "SELECT t1.idservicio,UPPER(t1.Nombre) AS Nombre,UPPER(t3.nombreEmpresa) as Empresa,UPPER(t1.Descripcion) AS Descripcion,t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) AS persona,empresafkcempresas as fk FROM cservicios as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona INNER JOIN cempresas as t3 on t3.idempresa=t1.EmpresafkcEmpresas;";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView2.Rows.Add(dr.GetInt32("idservicio"), dr.GetString("Nombre"),dr.GetString("Empresa"), dr.GetString("Descripcion"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")), dr.GetString("fk"));
            }
            dataGridView2.ClearSelection();
            dr.Close();
            c.dbconection().Close();
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (idservicetemp>0 && peditar && (!v.mayusculas(txtgetclave.Text.ToLower()).Trim().Equals(nombreAnterior) || !v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim().Equals(descripcionAnterior)))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsaves_Click(null, e);
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
                idservicetemp = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt((string)dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    pEliminarService.Visible = true;
                }
                if (peditar)
                {
                    editarservice = true;
                    txtgetclave.Text = nombreAnterior = v.mayusculas(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    txtgetnombre_s.Text = descripcionAnterior = v.mayusculas(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    cbempresa.SelectedValue = empresaAnterior = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
                    dataGridView2.ClearSelection();
                    btnsaves.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsaves.Text = "Guardar";
                    gbaddservice.Text = "Actualizar Servicio";
                    pCancelar.Visible = true;
                    btnsaves.Visible = lblsaves.Visible = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btncancelar_Click(object sender, EventArgs e)
        {
            if ((!v.mayusculas(txtgetclave.Text.ToLower()).Trim().Equals(nombreAnterior) || !v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim().Equals(descripcionAnterior)) && status==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsaves_Click(null, e);
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

        private void dataGridView2_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetclave_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("--"))
            {
                txt.Text = txt.Text.Replace("--", "-").Trim();
            }
          
        }

        private void gbaddservice_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void cbempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void catServicios_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                busqservices();
             
            }
        }

        private void gbadd_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Estatus")
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
    }
}

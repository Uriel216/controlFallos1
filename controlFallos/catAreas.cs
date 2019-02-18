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
    public partial class catAreas : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        int idUsuario,statusAnterior,empresa,area;
        bool editar = false,yaAparecioMensaje=false;
        string idareaAnterior, empresaanterior, identificadorAnterior, nombreAreaAnterior;
        public catAreas(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbareas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbareas.ColumnHeadersDefaultCellStyle = d;
            d.Font = new Font("Garamond",12,FontStyle.Regular);
            tbareas.DefaultCellStyle = d;
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
            mdr.Close();
            c.dbconection().Close();
            mostrar();
        }
        void getCambios(object sender,EventArgs e)
        {
            if (editar) {
                if (statusAnterior == 1 && ((cbempresa.SelectedIndex > 0 && empresaanterior != cbempresa.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtid.Text) && identificadorAnterior != txtid.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtnombre.Text) && nombreAreaAnterior != v.mayusculas(txtnombre.Text.Trim().ToLower()))))
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                } else
                {

                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddarea.Visible = true;
            }
            if (pconsultar)
            {
                gbareas.Visible = true;

            }
            if (peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
        }

        private void catAreas_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pinsertar || peditar)
            {
                iniempresas();
            }
            if (pconsultar)
            {
                busqueda();
            }
        }

        private void txtid_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }
        void iniempresas()
        {
            DataTable dt = (DataTable)v.getData("SELECT idempresa, upper(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE status ='1' ORDER BY nombreEmpresa ASC");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idempresa"] = 0;
            nuevaFila["nombreEmpresa"] = "--Seleccione una empresa--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbempresa.DataSource = null;
            cbempresa.DataSource = dt;
            cbempresa.ValueMember = "idempresa";
            cbempresa.DisplayMember = "nombreEmpresa";
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            String nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();

            if (!v.formularioAreas(cbempresa.SelectedIndex, txtid.Text.Trim(), nombre) && !v.existeArea(cbempresa.SelectedValue.ToString(), txtid.Text.Trim(), nombre))
            {
                if (c.insertar("INSERT INTO careas (empresafkcempresas,identificador,nombreArea,usuariofkcpersonal) VALUES('" + cbempresa.SelectedValue + "','" + txtid.Text.Trim() + "','" + nombre + "','" + idUsuario + "')"))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Areas',(SELECT idarea FROM careas WHERE nombreArea='" + nombre+ "' and empresafkcempresas='"+cbempresa.SelectedValue+"'),'Inserción de Area','" + idUsuario + "',NOW(),'Inserción de Area','" + empresa + "','" + area + "')");
                    MessageBox.Show("El Area Se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }

        }
        void _editar()
        {
            string id = txtid.Text.Trim();
            string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
            if (id.Equals(identificadorAnterior) && nombre.Equals(nombreAreaAnterior))
            {
                if (MessageBox.Show("No Se Realizaron Cambios. \n ¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk)==DialogResult.Yes)
                {
                    limpiar();
                }
            }else
            {
                if (!v.formularioAreas(cbempresa.SelectedIndex, id, nombre) && !v.existeAreaActualizar(cbempresa.SelectedValue.ToString(), id, this.identificadorAnterior, nombre, nombreAreaAnterior))
                {
                    if (c.insertar("UPDATE careas SET empresafkcempresas='" + cbempresa.SelectedValue + "',identificador ='" + id + "',nombreArea = '" + nombre + "' WHERE idarea='"+idareaAnterior+"'"))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Areas','"+idareaAnterior+"','"+empresaanterior+";"+identificadorAnterior+";"+nombreAreaAnterior+"','" + idUsuario + "',NOW(),'Actualización de Area','" + empresa + "','" + area + "')");
                        if (!yaAparecioMensaje) {
                            MessageBox.Show("El Area Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                            limpiar();
                    }
                }
            }
            
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string id = txtid.Text.Trim();
            string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
            if (cbempresa.SelectedValue.Equals(empresaanterior) || !id.Equals(identificadorAnterior) || !nombre.Equals(nombreAreaAnterior) && statusAnterior==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
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

        private void cbempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbareas_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void btndelpa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idareaAnterior))
            {
                try
                {
                    string msg;
                    int status;
                    if (this.statusAnterior== 0)
                    {
                        msg = "Re";
                        status = 1;
                    }
                    else
                    {
                        msg = "Des";
                        status = 0;
                 }
                    if (this.statusAnterior==0 && Convert.ToInt32(v.getaData("SELECT status FROM cempresas WHERE idempresa=(SELECT empresafkcempresas FROM careas WHERE idarea='"+idareaAnterior+"')"))==0)
                    {
                        MessageBox.Show("No Se Puede Reactiva el Área debido a que La Empresa de Procedencia ha sido Desactivada",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                    } else {
                        if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Area?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var res = c.insertar("UPDATE careas SET status = '" + status + "' WHERE idarea='" + this.idareaAnterior + "'");
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Areas','" + idareaAnterior + "','" + msg + "activación de Area','" + idUsuario + "',NOW(),'" + msg + "activación de Area','" + empresa + "','" + area + "')");
                            MessageBox.Show("El Area se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void gbaddarea_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void txtid_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        void busqueda()
        {
            tbareas.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idarea,upper(t2.nombreEmpresa),UPPER(t1.identificador),UPPER(t1.nombreArea),UPPER(CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno)) as nombres, UPPER(if(t1.status=1,'Activo','Inactivo')),t1.empresafkcempresas FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas = t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idPersona");
            int numFilas = dt.Rows.Count;
            for (int i = 0; i < numFilas; i++)
            {

                tbareas.Rows.Add(dt.Rows[i].ItemArray);
            }

            tbareas.ClearSelection();
        }

        private void tbareas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                string id = txtid.Text.Trim();
                string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
                if (!string.IsNullOrWhiteSpace(idareaAnterior) && peditar && ( cbempresa.SelectedValue.Equals(empresaanterior) || !id.Equals(identificadorAnterior) || !nombre.Equals(nombreAreaAnterior)) && statusAnterior==1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null,e);
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
                idareaAnterior = tbareas.Rows[e.RowIndex].Cells[0].Value.ToString();
                statusAnterior = v.getStatusInt(tbareas.Rows[e.RowIndex].Cells[5].Value.ToString());

                if (pdesactivar)
                {
                    if (statusAnterior == 0)
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelpa.Text = "Reactivar";
                    }
                    else
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelpa.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                if (peditar)
                {
                    cbempresa.SelectedValue = empresaanterior = tbareas.Rows[e.RowIndex].Cells[6].Value.ToString();
                    txtid.Text = identificadorAnterior = tbareas.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtnombre.Text = nombreAreaAnterior =v.mayusculas(tbareas.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                    editar = true;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    if (cbempresa.SelectedIndex == -1)
                    {
                        cbempresa.SelectedIndex = 0;
                    }
                    pCancelar.Visible = true;
                    gbaddarea.Text = "Actualizar Área";
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar en Este Formulario", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tbareas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (tbareas.Columns[e.ColumnIndex].Name == "Estatus")
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
        void limpiar()
        {
            if (pinsertar || peditar) {
                cbempresa.SelectedIndex = 0;
            }
                txtid.Clear();
            txtnombre.Clear();
            pCancelar.Visible = false;
            idareaAnterior = null;
            nombreAreaAnterior = null;
            statusAnterior = 0;
            pdelete.Visible = false; btnsavemp.Visible = lblsavemp.Visible = true; yaAparecioMensaje = false;
            if (pconsultar)
            {
                busqueda();
            }
            if (pinsertar)
            {
                gbaddarea.Text = "Agregar Area";
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
            }
            catUnidades cat = (catUnidades)Owner;
            cat.busqempresas();
            cat.bunidades();
        }
    }
}

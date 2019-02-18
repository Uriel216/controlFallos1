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
        public bool Pinsertar{ set; get; }
        public bool Peditar{get;set;}
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        bool editar;
        public catPuestos(int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
           tbpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        public void privilegiosPuestos()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = '"+Name+"'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
            Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
            Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
            Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            mdr.Close();
            c.dbconection().Close();
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbpuesto.Visible = true;
            }
            if (Pconsultar)
            {
               tbpuestos.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsave.Text = "Editar Puesto";
                editar = true;
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbpuesto.Text =  "Agregar Puesto";
                lblsave.Text = "Agregar";
            }
            if (Pconsultar)
            {
                busquedapuestos();
            }
            btnsave.Visible = lblsave.Visible = true;
            txtgetpuesto.Clear();
            idpuesto = 0;
            pcancel.Visible = true;
            yaAparecioMensaje = false;
            catPersonal cat = (catPersonal)Owner;
            cat.busemp();
            cat.busqPuestos();
            if (cat.editar)
            {
                cat.csetpuestos.SelectedValue = cat.tipoTemp;
            }
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
            if (e.RowIndex>=0) {
                if (idpuesto>0 && !v.mayusculas(txtgetpuesto.Text.Trim().ToLower()).Equals(puestoAnterior) && Peditar && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsave_Click(null, e);
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
                idpuesto = Convert.ToInt32(tbpuestos.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(tbpuestos.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (Pdesactivar)
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
                    pdelete.Visible = true;
                }
                if (Peditar)
                {
                    txtgetpuesto.Text = puestoAnterior = v.mayusculas(tbpuestos.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    tbpuestos.ClearSelection();
                    gbpuesto.Visible = true;
                    pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    gbpuesto.Text = "Actualizar Puesto";
                    lblsave.Text = "Guardar";
                    btnsave.Visible = lblsave.Visible = false;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      

        private void txtgetpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void gbpuestos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
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
            privilegiosPuestos();
                if (Pconsultar)
            {

                busquedapuestos();
            }
        }

        private void gbpuestos_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
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
            if (MessageBox.Show("¿Esta Seguro de "+msg+"activar el Puesto?", validaciones.MessageBoxTitle.Confirmar.ToString(),
      MessageBoxButtons.YesNo, MessageBoxIcon.Question)
      == DialogResult.Yes)
            {
                try
                {
                    String sql = "UPDATE puestos SET status = " + state + " WHERE idpuesto  = " + idpuesto;
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Puestos','" + idpuesto + "','"+msg+"activación de Puesto','" + idUsuario + "',NOW(),'"+msg+"activación de Puesto','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Puesto ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (!v.mayusculas(txtgetpuesto.Text.Trim().ToLower()).Equals(puestoAnterior) && status==1)
            {
                if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsave_Click(null, e);
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

        private void txtgetpuesto_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void gbpuesto_Enter(object sender, EventArgs e)
        {

        }

        private void gbpuestos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetpuesto_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetpuesto_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (status==1 && (!string.IsNullOrWhiteSpace(txtgetpuesto.Text) && puestoAnterior!=v.mayusculas(txtgetpuesto.Text.ToLower().Trim())))
                {
                    btnsave.Visible = lblsave.Visible = true;
                }else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
        }

        private void gbpuesto_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender,e,this);
        }

        void _editar()
        {

            if (idpuesto > 0) {
                string puesto = v.mayusculas(txtgetpuesto.Text.ToLower()).Trim();
                if (!string.IsNullOrWhiteSpace(puesto)) {
                    if (!puesto.Equals(puestoAnterior)) {
                        if (!v.yaExistePuesto(puesto,empresa,area)) {
                            String sql = "UPDATE puestos SET puesto =LTRIM(RTRIM('" + puesto + "')) WHERE idpuesto = " + this.idpuesto;
                            if (c.insertar(sql))
                            {
                                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Puestos','" + idpuesto + "','"+puestoAnterior+"','" + idUsuario + "',NOW(),'Actualización de Puesto','" + empresa + "','" + area + "')");
                               if(!yaAparecioMensaje) MessageBox.Show("El Puesto Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                            }
                        }
                    }else
                    {
                        MessageBox.Show("No Se Realizó Ningún Cambio Al Puesto", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes ) {
                            limpiar();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El Nombre del Puesto no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                    MessageBox.Show("Seleccione un Puesto Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        void insertar()
        {
            string puesto = v.mayusculas(txtgetpuesto.Text.ToLower());
            if (!string.IsNullOrWhiteSpace(puesto)) {
                if (!v.yaExistePuesto(puesto,empresa,area))
                {

                    String sql = "INSERT INTO puestos (puesto,empresa,area,usuariofkcpersonal) VALUES(LTRIM(RTRIM('" + puesto + "')),'" + empresa + "','" + area + "','" + idUsuario + "')";
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Puestos',(SELECT idpuesto FROM puestos WHERE puesto='" + puesto+"' and empresa='"+empresa+"' and area='"+area+"'),'Inserción de Puesto','" + idUsuario + "',NOW(),'Inserción de Puesto','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Puesto Se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error");
                    }
                }
            }
            else
            {
                MessageBox.Show("El Nombre del Puesto no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        public void busquedapuestos()
        {
            tbpuestos.Rows.Clear();
            String sql = "SELECT t1.idpuesto as id, UPPER(t1.puesto) AS puesto, t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as persona  FROM puestos as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.empresa = '" + empresa + "' and t1.area ='"+area+"' ORDER BY puesto ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbpuestos.Rows.Add(dr.GetString("id"), dr.GetString("puesto"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")));
            }
            dr.Close();
            c.dbconection().Close();
            tbpuestos.ClearSelection();
        }
    }
}

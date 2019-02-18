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
using System.Globalization;
using System.IO;

namespace controlFallos
{
    public partial class catEmpresas : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        bool editbussines = false;
        int bussinestemp, idUsuario, status, empresa, area;
        bool yaAparecioMensaje = false;
        string nombreAnterior,imgserialAnt;
        public catEmpresas(int idUsuario, int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            busqempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel); DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            busqempresa.ColumnHeadersDefaultCellStyle = d;
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
            
            if (mdr.Read()) {
                pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
                pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
                peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
                pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            }
                mostrar();
            mdr.Close();
            c.dbcon.Close();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddbussiness.Visible = true;
            }
            if (pconsultar)
            {
                gbcempresa.Visible = true;
            }
            if (peditar)
            {
                label1.Visible = true;
                label23.Visible = true;
            }
        }

        private void txtgetnempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void txtgetcempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editbussines)
                {

                    insertarEmpresa();

                }
                else
                {
                    if (empresa==2 && area == 2)
                    {
                        editarlogo();
                    } else {
                        editarEmpresa();
                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void limpiar()
        {
            if (pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                gbaddbussiness.Text = "Agregar Empresa";
                editbussines = false;
            }
            if (pconsultar)
            {
                busqempr();
            }
            yaAparecioMensaje = false;
            txtgetnempresa.Clear();
            pEliminarEmpresa.Visible = false;
            pCancel.Visible = false;
            busqempresa.ClearSelection();
            bussinestemp = 0;
            status = 0;
            nombreAnterior = null;
            if (empresa==1 && area==1) {
                catUnidades cat = (catUnidades)this.Owner; btnsavemp.Visible = true;
                lblsavemp.Visible = true;
                cat.busqempresas();
                cat.bunidades();
                if (cat.editar)
                {
                    cat.csetEmpresa.SelectedValue = cat.EmpresaAnterior;
                }cat.limpiar();
            }
            else if (empresa==2 && area ==2)
            {
                lnkrestablecer.Visible = false;
                pblogo.BackgroundImage = controlFallos.Properties.Resources.image;
                imgserial = imgserialAnt = null;
            }
            }
        public void insertarEmpresa()
        {
           
            string nombre = v.mayusculas(txtgetnempresa.Text.ToLower()).Trim();
            if (!v.formularioEmpresa(nombre) && !v.existenombreEmpresa(nombre))
            {
                string sql = "";
                if (empresa == 2 & area==2) {
                   sql = "INSERT INTO cempresas (nombreEmpresa,usuariofkcpersonal,logo,empresa,area) VALUES ('" + nombre.Trim() + "','" + this.idUsuario + "','" + imgserial + "','" + empresa + "','" + area + "')";
                }else
                {
                    sql = "INSERT INTO cempresas (nombreEmpresa,usuariofkcpersonal,empresa,area) VALUES ('" + nombre.Trim() + "','" + this.idUsuario + "','" + empresa + "','" + area + "')";

                }
                if (c.insertar(sql))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Empresas',(SELECT idempresa FROM cempresas WHERE nombreEmpresa='" + nombre + "'),'Inserción de Empresa','" + idUsuario + "',NOW(),'Inserción de Empresa','" + empresa + "','" + area + "')");
                    if (empresa==2 && area ==2)
                    {
                        var id = v.getaData("SELECT idempresa FROM cempresas WHERE nombreEmpresa='"+nombre+"'");
                        OrdenDeCompra odc = (OrdenDeCompra)Owner;
                        odc.CargarEmpresas();
                        odc.comboBoxFacturar.SelectedValue = id;

                    }
                    MessageBox.Show("La Empresa se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }


            }
        }
        void editarlogo()
        {
            if (bussinestemp > 0)
            {
                if (status == 0)
                {
                    MessageBox.Show("No se Puede Actualizar una Empresa Desactivada Para El Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        limpiar();
                    }
                }
                else
                {

                    string nombre = v.mayusculas(txtgetnempresa.Text.ToLower());
                    if (!v.formularioEmpresa(nombre))
                    {
                        if (imgserial.Equals(imgserialAnt))
                        {
                            MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                limpiar();
                            }
                        }
                        else
                        {
                                String sql = "UPDATE cempresas SET logo =LTRIM(RTRIM('" + imgserial + "')) WHERE idEmpresa = " + this.bussinestemp;
                            if (c.insertar(sql))
                            {
                                var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','"+imgserialAnt+"','" + idUsuario + "',NOW(),'Actualización de Empresa','" + empresa + "','" + area + "')");
                                if (!yaAparecioMensaje)
                                {
                                    MessageBox.Show("Se Ha Actualizado La Empresa Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                limpiar();

                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una Empresa Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void editarEmpresa()
        {
            if (bussinestemp > 0) {
                if (status == 0)
                {
                    MessageBox.Show("No se Puede Actualizar una Empresa Desactivada Para El Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        limpiar();
                    }
                }
                else {

                    string nombre = v.mayusculas(txtgetnempresa.Text.ToLower());
                    if (!v.formularioEmpresa(nombre)) {
                        if (nombre.Equals(nombreAnterior))
                        {
                            MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                limpiar();
                            }
                        } else
                        {
                            if (!v.existeEmpresaActualizar(nombre, nombreAnterior)) {
                                String sql = "UPDATE cempresas SET nombreEmpresa =LTRIM(RTRIM('" + nombre.Trim() + "')) WHERE idEmpresa = " + this.bussinestemp;
                                if (c.insertar(sql))
                                {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','" + nombreAnterior + "','" + idUsuario + "',NOW(),'Actualización de Empresa','" + empresa + "','" + area + "')");
                                    if (!yaAparecioMensaje) {
                                        MessageBox.Show("Se Ha Actualizado La Empresa Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }   limpiar();
                                }
                            }
                        }
                    }
                }
            } else
            {
                MessageBox.Show("Seleccione una Empresa Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void busqempr()
        {
            busqempresa.Rows.Clear();
            String sql = "SELECT t1.idempresa,upper(t1.nombreEmpresa) as nombreEmpresa, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as persona, t1.status,t1.empresa,t1.area FROM cempresas as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona  ORDER BY nombreEmpresa ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqempresa.Rows.Add(dr.GetInt32("idempresa"), dr.GetString("nombreEmpresa"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")),dr.GetString("empresa"), dr.GetString("area"));
            }
            dr.Close();
            c.dbconection().Close();
            busqempresa.ClearSelection();
        }

        private void busqempresa_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (empresa ==1 && area == 1) {
                    if (bussinestemp > 0 && peditar && !v.mayusculas(txtgetnempresa.Text.ToLower()).Trim().Equals(nombreAnterior) && status == 1)
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnsavemp_Click(sender, e);

                        }
                    }
                }else
                {
                   if( bussinestemp >0 && peditar && !imgserialAnt.Equals(imgserial))
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnsavemp_Click(sender, e);

                        }
                    }
                }
                        guardarReporte(e);
                    
            }

        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                this.bussinestemp = Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(busqempresa.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (pdesactivar)
                {
                      if (status == 0)
                        {
                            btndelete.BackgroundImage = Properties.Resources.up;
                            lbldelete.Text = "Reactivar";
                        }
                        else
                        {
                            btndelete.BackgroundImage = Properties.Resources.delete__4_;
                            lbldelete.Text = "Desactivar";
                    }
                    if (Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[4].Value) == empresa && Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[5].Value) == area)
                    {

                        pEliminarEmpresa.Visible = true;
                    }else
                    {
                        pEliminarEmpresa.Visible = false;
                    }
                }
                if (peditar)
                {
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    gbaddbussiness.Text = "Actualizar Empresa";
                    txtgetnempresa.Text = nombreAnterior = v.mayusculas(busqempresa.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    editbussines = true;
                    pCancel.Visible = true;
                    busqempresa.ClearSelection();
                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                    if (Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[4].Value)!=empresa && Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[5].Value) != area)
                    {
                        txtgetnempresa.ReadOnly = true;
                    }else
                    {
                        txtgetnempresa.ReadOnly = false;
                    }
                    if (empresa == 2 && area == 2) 
                    {
                       imgserialAnt = imgserial = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa= '"+bussinestemp+"'").ToString();
                        if (imgserial!="")
                        {
                            pblogo.BackgroundImage = v.StringToImage2(imgserial);
                            lnkrestablecer.Visible = true;
                        }else
                        {
                            pblogo.BackgroundImage = controlFallos.Properties.Resources.image;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta con Privilegios para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (!v.mayusculas(txtgetnempresa.Text.ToLower()).Trim().Equals(nombreAnterior) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarEmpresa();
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

        private void button4_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (status==0)
            {
                state = 1;
                msg = "Re";
            }else
            {
                state = 0;
                msg = "Des";
            }

            if (MessageBox.Show("¿Desea "+msg+"activar la Empresa y las Areas Relacionadas a la Misma?", validaciones.MessageBoxTitle.Error.ToString(),
          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
          == DialogResult.Yes)
            {
                String sql = "UPDATE cempresas SET status = '"+state+"' WHERE idEmpresa = '" + this.bussinestemp + "'";
                if (c.insertar(sql))
                {
                    c.insertar("UPDATE careas SET status='"+state+"' WHERE empresafkcempresas='"+bussinestemp+"'");
                    c.insertar("UPDATE cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idarea INNER JOIN cempresas as t3 On t2.empresafkcempresas=t3.idempresa SET t1.status='"+state+"' WHERE t3.idempresa='"+bussinestemp+"'");
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','" + msg + "activación de Empresa','" + idUsuario + "',NOW(),'" + msg + "activación de Empresa','" + empresa + "','" + area + "')");
                    MessageBox.Show("La Empresa se ha "+msg+"activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Information);

                    limpiar();

                }
                else
                {
                    MessageBox.Show("Unidad no Desactivada");
                }
          ;
            }
        }

        private void busqempresa_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetnempresa_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetnempresa_TextChanged(object sender, EventArgs e)
        {
            if (editbussines) {
                if (status == 1 && (!string.IsNullOrWhiteSpace(txtgetnempresa.Text) && nombreAnterior != v.mayusculas(txtgetnempresa.Text.ToLower()).Trim()))
                {
                    btnsavemp.Visible = true;
                    lblsavemp.Visible = true;
                } else
                {

                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                }
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender,e,this);
        }
        string imgserial=null;
        private void pblogo_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Title = "Seleccione Logo Para Empresa";
                openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog1.InitialDirectory = "Documents";
                openFileDialog1.FileName = null;
                openFileDialog1.ShowDialog();
                imgserial = v.ImageToString(openFileDialog1.FileName);
                pblogo.BackgroundImage = v.StringToImage2(imgserial);
                lnkrestablecer.Visible = true;
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147024809) {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }
            }

        private void lnkrestablecer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            imgserial ="";
            pblogo.BackgroundImage = controlFallos.Properties.Resources.image;
            lnkrestablecer.Visible = false;
        }

        private void gbaddbussiness_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void pblogo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (editbussines)
            {
                if (status==1 && (v.ImageToString(pblogo.BackgroundImage) != imgserialAnt) )
                {
                    btnsavemp.Visible = true;
                    lblsavemp.Visible = true;
                }
                else
                {

                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                }
            }
        }

        private void gbEmp_Enter(object sender, EventArgs e)
        {

        }

        private void catEmpresas_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                busqempr();
            }
        }

        private void busqempresa_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (busqempresa.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

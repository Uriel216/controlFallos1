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
    public partial class catAnaqueles : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        string idpasilloTemp;
        string pasilloAnterior;
        string pasilloValueAnterior,nivelAnterior;
        bool editar;
        int _state;
        int idUsuario;
        int empresa, area;
        bool yaAparecioMensaje;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
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

                gbaddanaquel.Visible = true;
            }
            if (Pconsultar)
            {
                gbanaqueles.Visible = true;
            }
            if (Peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        void getCambios(object sender,EventArgs e)
        {
            if (editar) {
                int nivel;
                if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
                if (_state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel>0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                } else
                {
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }
        public catAnaqueles(int idUsuario,int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbpasillo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbubicaciones.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
           d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbubicaciones.ColumnHeadersDefaultCellStyle = d;
            cbnivel.DrawItem += new DrawItemEventHandler(v.combos_DrawItem);
            lbltitle.Left = (panel1.Width - lbltitle.Width) / 2;
        }
     
        void _insertarPasillo()
        {
            int nivel =Convert.ToInt32(cbnivel.SelectedValue);

            string anaquel = txtpasillo.Text.Trim();
            if (v.formularioAnaquees(Convert.ToInt32(cbpasillo.SelectedValue), nivel, anaquel) && !v.existeAnaquel(nivel.ToString(), anaquel))
            {
                string sql = "INSERT INTO canaqueles (nivelfkcniveles,anaquel,usuariofkcpersonal) VALUES(LTRIM(RTRIM('" + cbnivel.SelectedValue + "')),LTRIM(RTRIM('" + anaquel + "')),'" + idUsuario + "')";
                if (c.insertar(sql))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles',(SELECT idanaquel From canaqueles WHERE nivelfkcniveles='" + cbnivel.SelectedValue + "' AND anaquel='" + anaquel + "'),'','" + idUsuario + "',NOW(),'Inserción de Anaquel','" + empresa + "','" + area + "')");
                    ubicaciones u = (ubicaciones)Owner;
                   
                    MessageBox.Show("Anaquel Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
              
                    ubicaciones ub = (ubicaciones)this.Owner;
                    ub.busqUbic();
                    u.cbpasillo.SelectedValue = v.getaData("SELECT (SELECT pasillofkcpasillos FROM cniveles WHERE idnivel=nivelfkcniveles) FROM canaqueles WHERE idanaquel=(SELECT idanaquel From canaqueles WHERE nivelfkcniveles='" + cbnivel.SelectedValue + "' AND anaquel='" + anaquel + "')");
                    u.cbniveles.SelectedValue = v.getaData("SELECT nivelfkcniveles FROM canaqueles WHERE idanaquel = (SELECT idanaquel From canaqueles WHERE nivelfkcniveles='" + cbnivel.SelectedValue + "' AND anaquel='" + anaquel + "')");
                    u.cbanaquel.SelectedValue = v.getaData("SELECT idanaquel From canaqueles WHERE nivelfkcniveles='" + cbnivel.SelectedValue + "' AND anaquel='" + anaquel + "'");
                    limpiar();
                }
            }
            
        }
        public void busqUbic()
        {
            String sql = "SELECT idpasillo id, UPPER(pasillo) as nombre FROM cpasillos WHERE status = 1 ORDER BY pasillo ASC";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            DataRow nuevaFila = dt.NewRow();
            
            dt.Rows.InsertAt(nuevaFila, 0);
            cbpasillo.DataSource = null;
            AdaptadorDatos.Fill(dt);
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un Pasillo--".ToUpper();
            cbpasillo.DataSource = dt;
            cbpasillo.ValueMember = "id";
            cbpasillo.DisplayMember = "nombre";

            c.dbcon.Close();
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                gbaddanaquel.Text = "Agregar Anaquel";
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
            }
            if (Pconsultar)
            {

                insertarpasillos();
            }
            txtpasillo.Clear();
            idpasilloTemp = null;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            idpasilloTemp = null;
            pasilloAnterior = null;
            _state = 0;
            btnsavemp.Visible = lblsavemp.Visible = true;
            cbpasillo.SelectedIndex = 0;
            yaAparecioMensaje = false;
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            int nivel;
            if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
            if (_state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel > 0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
            { 
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender,e);
                }else
                {
                    limpiar();
                }
            }else
            {
                limpiar();
            }
        }
        void _editarPasillo()
        {
            if (!string.IsNullOrWhiteSpace(idpasilloTemp)) {
                string pasillo = txtpasillo.Text.Trim();
                if (v.formularioAnaquees(Convert.ToInt32(cbpasillo.SelectedValue), Convert.ToInt32(cbnivel.SelectedValue), pasillo) && !v.existeAnaquelActualizar(cbnivel.SelectedValue.ToString(), pasillo, pasilloAnterior))
                {
                    if (_state == 1)
                    {
                        string sql = "UPDATE canaqueles SET nivelfkcniveles='" + cbnivel.SelectedValue + "',  anaquel= '" + pasillo + "' WHERE idanaquel= '" + idpasilloTemp + "'";
                        if (c.insertar(sql))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles','" + idpasilloTemp + "','" + pasilloValueAnterior + ";" + pasilloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Anaquel','" + empresa + "','" + area + "')");
                            if (!yaAparecioMensaje) MessageBox.Show("Anaquel Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            limpiar();
                            ubicaciones u = (ubicaciones)Owner;
                            u.insertarUbicaciones();
                            u.busqUbic();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se Puede Actualizar un anaquel Desactivado Para el Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }else
            {
                MessageBox.Show("Seleccione un Anaquel de la Lista Deplegable Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        public void insertarpasillos()
        {
            try
            {
                tbubicaciones.Rows.Clear();
                DataTable anaqueles = (DataTable)v.getData("SELECT idanaquel,(SELECT upper(pasillo) from cpasillos where idpasillo=pasillofkcpasillos),upper(nivel),upper(anaquel),upper((SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal)), if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')),(SELECT upper(idpasillo) from cpasillos where idpasillo=pasillofkcpasillos),idnivel FROM canaqueles as t1 INNER JOIN cniveles as t2 ON t1.nivelfkcniveles =t2.idnivel");
                for (int i=0;i<anaqueles.Rows.Count;i++)
                {
                    tbubicaciones.Rows.Add(anaqueles.Rows[i].ItemArray);
                }
                tbubicaciones.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gbClasificacion_Enter(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            ubicaciones u = (ubicaciones)Owner;
            u.txtcharola.Focus();
        }

        private void catAnaqueles_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
            {

                insertarpasillos();
              
           
            }
            if (Pconsultar)
            {

                busqUbic();
            }
            ubicaciones u = (ubicaciones)Owner;
            if (!string.IsNullOrWhiteSpace(u.nivelTemp))
            {
                cbpasillo.SelectedValue = v.getaData("SELECT pasillofkcpasillos FROM cniveles WHERE idnivel='" + u.nivelTemp + "'");
                cbnivel.SelectedValue = u.nivelTemp;
                u.nivelTemp = null;
            }
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertarPasillo();
                }else {
                    _editarPasillo();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                int nivel;
                if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
                if (!string.IsNullOrWhiteSpace(idpasilloTemp) &&  _state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel > 0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
                { 
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
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
            if (e.RowIndex >= 0)
            {
                try
                {
                    idpasilloTemp = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                    _state = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[5].Value.ToString());
                    if (Pdesactivar)
                    {

                        if (_state == 0)
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
                    if (Peditar)
                    {
                        pasilloAnterior = txtpasillo.Text = v.mayusculas(tbubicaciones.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                        cbpasillo.SelectedValue = pasilloValueAnterior = tbubicaciones.Rows[e.RowIndex].Cells[6].Value.ToString();
                        cbnivel.SelectedValue = nivelAnterior = tbubicaciones.Rows[e.RowIndex].Cells[7].Value.ToString();
                        btnsavemp.Visible = false;
                        lblsavemp.Visible = false;
                        btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        lblsavemp.Text = "Guardar";
                        editar = true;
                        pCancelar.Visible = true;
                        gbaddanaquel.Text = "Actualizar Anaquel";

                        btnsavemp.Visible = lblsavemp.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btndelpa_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this._state == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán Las Charolas Asociadas al Anaquel";
                }

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Anaquel? \n " + msg2 + "", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = c.insertar("UPDATE canaqueles SET status = '" + status + "' WHERE idanaquel= " + this.idpasilloTemp);
                    var res1 = c.insertar("UPDATE ccharolas SET status ='" + status + "' WHERE anaquelfkcanaqueles =" + this.idpasilloTemp);
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles','" + idpasilloTemp + "','" + msg + "activación de Anaquel','" + idUsuario + "',NOW(),'"+msg+"activación de Anaquel','"+empresa+"','"+area+"')");


                    MessageBox.Show("El Anaquel se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msg = null;
                    status = 0;
                    msg2 = null;
                    ubicaciones u = (ubicaciones)Owner;
                    u.insertarUbicaciones();
                    u.busqUbic();
                    limpiar();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbubicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbubicaciones.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void tbubicaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void cbpasillo_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtpasillo_TextChanged(object sender, EventArgs e)
        {
            if (editar) {
                if (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text.Trim())
                {
                    btnsavemp.Visible = true;
                    lblsavemp.Visible = true;
                }else
                {

                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                }
            }
        }

        private void gbaddanaquel_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void txtpasillo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbpasillo.SelectedIndex>0)
            {
                v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE pasillofkcpasillos='" + cbpasillo.SelectedValue + "' and status='1'", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL-");
                cbnivel.Enabled = true;
            } else
            {
                cbnivel.DataSource = null;
                cbnivel.Enabled = false;
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
    }
}

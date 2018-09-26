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
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace controlFallos
{
    public partial class catProveedores : Form
    {
        int idUsuario;
        string _idproveedorTemp;
        bool _editar;
        bool reactivar;
        string _empresaAnterior;
        string _amAnterior;
        string _apAnterior;
        string _nombreAnterior;
        string _correoAnterior;
        string _telefonoAnterior;
        int status;
        public bool pinsertar { set; get;}
        public bool pconsultar { set; get; }
        public bool peditar { set; get; }
        public bool pdesactivar { set; get; }
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
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbinsertmodif.Visible = true;
                pbinsertmodif.Visible = true;
            }
            if (pconsultar)
            {
                gbbuscar.Visible = true;
                tbProveedores.Visible = true;
            }
            if (peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblguardar.Text = "Editar Proveedor";
                _editar = true;
            }
        }
        validaciones v = new validaciones();
        conexion c = new conexion();
        public catProveedores(int idUsuario, Image logo)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            pblogo.BackgroundImage = logo;
        }
        public void insertarums()
        {
            tbProveedores.Rows.Clear();
            string sql = "SELECT * FROM cproveedores";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbProveedores.Rows.Add(dr.GetString("idproveedor"), dr.GetString("empresa"), dr.GetString("aPaterno"), dr.GetString("aMaterno"), dr.GetString("nombres"), dr.GetString("correo"), dr.GetString("telefono"), v.getStatusString(dr.GetInt32("status")));
            }
            tbProveedores.ClearSelection();
        }
       
        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string empresa = v.mayusculas(txtgetempresa.Text.ToLower());
                string ap = v.mayusculas(txtgetap.Text.ToLower());
                string am = v.mayusculas(txtgetam.Text.ToLower());
                string nombre = v.mayusculas(txtgetnombre.Text.ToLower());
                string correo = txtgetemail.Text;
                string lada = txtlada.Text;
                string telefono = txtlada.Text + txtgettelefono.Text;
                if (!this._editar)
                {
                    if (!v.formularioProveedores(empresa, ap, am, nombre, correo, telefono,lada) && !v.existeProveedor(v.mayusculas(empresa), v.mayusculas(ap), v.mayusculas(am), v.mayusculas(nombre), v.mayusculas(correo), telefono))
                    {
                        string sql = "INSERT INTO cproveedores(aPaterno, aMaterno, nombres, correo, telefono, empresa) VALUES(LTRIM(RTRIM('" + v.mayusculas(ap) + "')),LTRIM(RTRIM('" + v.mayusculas(am) + "')),LTRIM(RTRIM('" + v.mayusculas(nombre) + "')),LTRIM(RTRIM('" + correo + "')),LTRIM(RTRIM('" + telefono + "')),LTRIM(RTRIM('" + v.mayusculas(empresa) + "')))";
                        if (c.insertar(sql))
                        {
                            MessageBox.Show("Datos Insertados Exitosamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            insertarums();
                        }
                        else
                        {
                            MessageBox.Show("Algo Pasó");
                        }
                    }
                }
                else
                {
                    if (v.mayusculas(empresa).Equals(v.mayusculas(_empresaAnterior)) && v.mayusculas(ap).Equals(v.mayusculas(_apAnterior)) && v.mayusculas(am).Equals(v.mayusculas(_amAnterior)) && v.mayusculas(v.mayusculas(nombre)).Equals(v.mayusculas(_nombreAnterior)) && v.mayusculas(correo).Equals(_correoAnterior) && telefono.Equals(_telefonoAnterior))
                    {
                        MessageBox.Show("No hay Modificaciones por hacer", "Control Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        borrar();
                    }
                    else
                    {
                        if (status == 0)
                        {
                            MessageBox.Show("No se Puede Modificar un Proveedor que ha Sido Desactivado", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                        else
                        {
                            if (v.validacionCorrero(correo) && !v.yaExisteProveedorActualizar(v.mayusculas(empresa), v.mayusculas(_empresaAnterior), v.mayusculas(ap), v.mayusculas(_apAnterior), v.mayusculas(am), v.mayusculas(_amAnterior), v.mayusculas(nombre), v.mayusculas(_nombreAnterior), correo, _correoAnterior, telefono, _telefonoAnterior))
                            {
                                string sql = "UPDATE cproveedores SET aPaterno='" + v.mayusculas(ap) + "', aMaterno='" + v.mayusculas(am) + "', nombres='" + v.mayusculas(nombre) + "', correo='" + correo + "', telefono='" + v.mayusculas(telefono) + "', empresa='" + v.mayusculas(empresa) + "' WHERE idproveedor = " + this._idproveedorTemp;
                                if (c.insertar(sql))
                                {
                                    MessageBox.Show("Datos Actualizados Exitosamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    limpiar();
                                    insertarums();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void borrar()
        {
            if(MessageBox.Show("¿Desea Limpiar Todos los Campos?","Control Fallos",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
            {
                limpiar();
            }
        }
        private void txtgetempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void txtgetap_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void txtgettelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgetemail_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }
        void limpiar()
        {
            txtgetempresa.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetemail.Clear();
            txtgettelefono.Clear();
            _idproveedorTemp = null;
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
            lblguardar.Text = "Agregar Proveedor";
            linkcancel.Visible = false;
            _idproveedorTemp = null;
            peliminarpro.Visible = false;
            txtlada.Clear();
        }

        private void catProveedores_Load(object sender, EventArgs e)
        {
            insertarums();
            privilegios();
        }

        private void tbProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             if (e.ColumnIndex == 5)
                {
                    try
                    {
                        Outlook.Application outlookApp = new Outlook.Application();
                        Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                        mailItem.Subject = "Orden de Compra: " + DateTime.Now.ToLongDateString();
                        mailItem.To = tbProveedores.Rows[e.RowIndex].Cells[5].Value.ToString();
                        mailItem.Body = "Adjunte el Archivo pdf Generado en la Orden de Compra Porfavor.";
                        mailItem.Importance = Outlook.OlImportance.olImportanceNormal;
                        mailItem.Display(false);
                    tbProveedores.ClearSelection();
                    }
        catch (Exception eX)
            {
                MessageBox.Show(eX.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           
        }

        private void tbProveedores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbProveedores.Columns[e.ColumnIndex].Name == "Status")
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

        private void tbProveedores_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (pdesactivar)
            {
               
                _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();

                status = v.getStatusInt(tbProveedores.Rows[e.RowIndex].Cells[7].Value.ToString());
                if (status == 0)
                {
                    reactivar = true;
                    btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                    lbldeleteuser.Text = "Reactivar Proveedor";
                }
                else
                {
                    reactivar = false;
                    btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lbldeleteuser.Text = "Desactivar Proveedor";
                }
                peliminarpro.Visible = true;
            }
            if (peditar)
            {

                try
                {
                   
                    _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();
                    _empresaAnterior = tbProveedores.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtgetempresa.Text = _empresaAnterior;
                    _apAnterior = tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtgetap.Text = _apAnterior;
                    _amAnterior = tbProveedores.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtgetam.Text = _amAnterior;
                    _nombreAnterior = tbProveedores.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtgetnombre.Text = _nombreAnterior;
                    _correoAnterior = tbProveedores.Rows[e.RowIndex].Cells[5].Value.ToString();
                    txtgetemail.Text = _correoAnterior;
                    _telefonoAnterior = tbProveedores.Rows[e.RowIndex].Cells[6].Value.ToString();
                    txtlada.Text = _telefonoAnterior.Substring(0, 3);
                    txtgettelefono.Text = _telefonoAnterior.Substring(3);
                    this._editar = true;
                    linkcancel.Visible = true;
                    btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblguardar.Text = "Editar Proveedor";
                    tbProveedores.ClearSelection();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }else
            {
                MessageBox.Show("Usted No Tiene Privilegios Para Editar Éste Catálogo","Control Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            int status;
            string msg;
            if (reactivar)
            {
                status = 1;
                msg = "Re";
            }
            else
            {
                status = 0;
                msg = "Des";
            }
            if (MessageBox.Show("¿Desea " + msg + "activar Al Proveedor?", "Control de Fallos",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
            {
                try
                {

                    String sql = "UPDATE cproveedores SET status = " + status + " WHERE idproveedor  = " + this._idproveedorTemp;
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("El Proveedor ha sido " + msg + "activado");
                        limpiar();
                        insertarums();
                    }
                    else
                    {
                        MessageBox.Show("El Proveedor no ha sido desactivado");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void linkcancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            limpiar();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                tbProveedores.Rows.Clear();
                string wheres = "";
                string sql = "SELECT * FROM cproveedores ";
                if (!string.IsNullOrWhiteSpace(txtbempresa.Text))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE empresa  LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%' ";
                    }
                    else
                    {
                        wheres += "OR empresa LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtbap.Text))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE aPaterno  LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%' ";
                    }
                    else
                    {
                        wheres += "OR aPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtbemail.Text))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE correo  LIKE '" + v.mayusculas(txtbemail.Text.ToLower()) + "%' ";
                    }
                    else
                    {
                        wheres += "OR correo  LIKE '" + v.mayusculas(txtbemail.Text.ToLower()) + "%'";
                    }
                }
                sql += wheres;
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                var res = Convert.ToInt32(cm.ExecuteScalar());
                if (res == 0)
                {
                    MessageBox.Show("No se Encontraron Resultados ", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        tbProveedores.Rows.Add(dr.GetString("idproveedor"), dr.GetString("empresa"), dr.GetString("aPaterno"), dr.GetString("aMaterno"), dr.GetString("nombres"), dr.GetString("correo"), dr.GetString("telefono"), v.getStatusString(dr.GetInt32("status")));
                    }
                }
                tbProveedores.ClearSelection();
                txtbempresa.Clear();
                txtbap.Clear();
                txtbemail.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarums();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (txtlada.Text.Length<3)
            {
                MessageBox.Show("Digite una Clave Lada Válida", "Control de Fallos", MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtlada_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgettelefono_Validating(object sender, CancelEventArgs e)
        {
            if (txtgettelefono.Text.Length<10)
            {
                MessageBox.Show("Digite un Número Telefónico Valido", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtgetemail_Validating(object sender, CancelEventArgs e)
        {
            if (txtgetemail.Text.Length==0 || !v.validacionCorrero(txtgetemail.Text))
            {
                MessageBox.Show("Escriba un correo electrónico válido", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel=true;
            }
        }
    }
}

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
       public bool _editar;
        bool reactivar;
        string _empresaAnterior, _amAnterior, _apAnterior, _nombreAnterior, _telefonoAnterior, _idladaanterior1, _idladaanterior2, _idladaanterior3, _idladaanterior4, _paginaweb, idSepomexAnterior, idSepomex, giroAnterior, _correoAnterior,ObservacionesAnterior,calleAnterior,NumeroAnterior,referenciasAnterior,extAnterior1, extAnterior2, extAnterior3, extAnterior4;
        string[] telefonosAnterioresEmpresa = new string[2], telefonsAnterioresContacto= new string[2];
        int status;
        bool yaAparecioMensaje;
        public bool pinsertar { set; get; }
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
            mdr.Close();
            c.dbcon.Close();
            mostrar();
        }
    void getCambios(object sender, EventArgs e)
        {
            try
            {
                if (_editar)
                {
                    string giro = "";
                    if (cbgiros.DataSource!=null)
                    {
                        giro = cbgiros.SelectedValue.ToString();
                    }else
                    {
                        giro = "0";
                    }
                    string asentemiento = "0";
                    if (cbasentamiento.DataSource != null) asentemiento = cbasentamiento.SelectedValue.ToString();
                     if (status == 1 && ((!string.IsNullOrWhiteSpace(txtgetempresa.Text) && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "0").Equals((giro)) || !_idladaanterior1.Equals(cmbTel_uno.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !_idladaanterior2.Equals(cbTel_dos.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals(asentemiento) || !calleAnterior.Equals(v.mayusculas(txtcalle.Text.Trim().ToLower())) || !NumeroAnterior.Equals(v.mayusculas(txtnum.Text.Trim().ToLower())) || !referenciasAnterior.Equals(v.mayusculas(txtreferencias.Text.Trim().ToLower())) || !_correoAnterior.Equals(txtgetemail.Text) || !_idladaanterior3.Equals(cbladas.SelectedValue.ToString()) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !_idladaanterior4.Equals(cbladas1.SelectedValue.ToString()) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text))))
                    { 
                        if (!string.IsNullOrWhiteSpace(txtweb.Text))
                        {
                            if (!v.paginaWebValida(txtweb.Text.Trim()))
                            {
                                btnguardar.Visible = lblguardar.Visible = true;
                            }
                            else
                            {
                                btnguardar.Visible = lblguardar.Visible = false;
                            }

                        }
                        else 
                        {
                            btnguardar.Visible = lblguardar.Visible = true;
                        }
                        if (!string.IsNullOrWhiteSpace(txtgetemail.Text))
                        {
                            if (v.validacionCorrero(txtgetemail.Text))
                            {
                                btnguardar.Visible = lblguardar.Visible = true;
                            }
                            else
                            {
                                btnguardar.Visible = lblguardar.Visible = false;
                            }

                        }
                        else
                        {
                            btnguardar.Visible = lblguardar.Visible = true;
                        }
                    }
                    else
                    {
                        btnguardar.Visible = lblguardar.Visible = false;
                    }
                } 
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbinsertmodif.Visible = true;
                lblopcionales.Visible = true;
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
        int empresa, area;
        validaciones v = new validaciones();
        conexion c = new conexion();
        public catProveedores(int idUsuario, Image logo,int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            pblogo.BackgroundImage = logo;
            cbladas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbasentamiento.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbProveedores.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbTel_dos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbTel_uno.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbgiros.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbgirosb.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            if (Convert.ToInt32(v.getaData("SELECT ver FROM privilegios WHERE namform='catGiros' AND usuariofkcpersonal='"+idUsuario+"'"))==1)
            {
                pGiros.Visible = true;
            }
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbProveedores.ColumnHeadersDefaultCellStyle = d;

        }
        public void insertarums()
        {
            tbProveedores.Rows.Clear();
            string sql = @"SELECT t1.idproveedor, UPPER(t1.empresa) AS empresa, COALESCE(t1.paginaweb, '') AS paginaweb, COALESCE(UPPER(t2.giro), '') AS giro, CONCAT(COALESCE((SELECT CONCAT('(+', t33.phonecode, ')', t1.telefonoEmpresaUno, IF(t1.ext1 is null, '', CONCAT(' Ext. ', t1.ext1))) FROM cladas AS t33 WHERE t1.idlada = t33.id), ''), COALESCE((SELECT CONCAT('  (+', t3.phonecode, ')', t1.telefonoEmpresaDos, IF(t1.ext2 is null, '', CONCAT(' Ext. ', t1.ext2))) FROM cladas AS t3 WHERE t1.idladados = t3.id), '')) AS telefonoEmpresa, COALESCE(UPPER(t1.observaciones), '') AS observaciones, COALESCE((SELECT UPPER(CONCAT('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', t2.tipo, ' ', t2.asentamiento, ', ', municipio, ', ', t2.estado, '. C. P. ', t2.cp)) FROM sepomex AS t2 WHERE t1.domiciliofksepomex = t2.id), '') AS domicilio, UPPER(t1.aPaterno)AS aPaterno, UPPER(t1.AMaterno) AS aMaterno, UPPER(t1.nombres) AS nombres, COALESCE(t1.correo, '') AS correo, CONCAT(COALESCE((SELECT  CONCAT('(+', t33.phonecode, ')', t1.telefonoContactoUno, IF(t1.ext3 is null, '', CONCAT(' Ext. ', t1.ext3))) FROM cladas AS t33 WHERE t1.idladatres = t33.id), ''), COALESCE((SELECT  CONCAT('  (+', t3.phonecode, ')', t1.telefonoContactoDos, IF(t1.ext1 is null, '', CONCAT(' Ext. ', t1.ext1))) FROM cladas AS t3 WHERE t1.idladacuatro = t3.id), '')) AS telefonoContacto, if(t1.status = 1,upper('Activo'),upper(CONCAT('No Activo'))), coalesce(t2.idgiro, '') as idgiro,t1.idlada as idlada1,coalesce(t1.telefonoEmpresaUno, '') as telempresa1,t1.idladados as idlada2,coalesce(t1.telefonoEmpresaDos, '') as telempresa2,COALESCE(t1.domiciliofksepomex, '') as iddomicilio,coalesce(calle, '') as calle,coalesce(numero, '') as numero ,coalesce(referencias, '') as referencias,t1.idladatres as idlada3,coalesce(t1.telefonoContactoUno, '') as telcontacto1,t1.idladacuatro as idlada4,coalesce(t1.telefonoContactoDos, '') as telcontacto2,COALESCE(ext1,'') AS ext1,COALESCE(ext2,'') AS ext2,COALESCE(ext3,'') AS ext3,COALESCE(ext4,'') AS ext4 FROM cproveedores AS t1 LEFT JOIN cgiros AS t2 ON t1.Clasificacionfkcgiros = t2.idgiro  ORDER BY t1.empresa ASC;";
            DataTable t = (DataTable)v.getData(sql);
            for (int i=0;i<t.Rows.Count;i++) tbProveedores.Rows.Add(t.Rows[i].ItemArray);
          
            tbProveedores.ClearSelection();
        }
        public void giros_desactivados(string giro)
        {
            MySqlCommand cmd = new MySqlCommand("Select idgiro, upper(giro) as giro from cgiros where idgiro='" +giro + "' and status='0'", c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cbgiros.DataSource = null;
                MySqlCommand cmd6 = new MySqlCommand("SELECT idgiro,upper(giro) as giro from cgiros where status='1' order by giro asc", c.dbconection());
                MySqlDataAdapter da = new MySqlDataAdapter(cmd6);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow row = dt.NewRow();
                DataRow row1 = dt.NewRow();
                row1["idgiro"] = dr["idgiro"];
                row1["giro"] = dr["giro"];
                row["idgiro"] = 0;
                row["giro"] = "-SELECIONE UN GIRO-";//agregamos una opción más
                dt.Rows.InsertAt(row, 0);
                dt.Rows.InsertAt(row1, 1);
                cbgiros.ValueMember = "idgiro";
                cbgiros.DisplayMember = "giro";
                cbgiros.DataSource = dt;
                cbgiros.SelectedIndex = 0;
                cbgiros.Text = dr["giro"].ToString();
                cbgiros.SelectedValue = giro;
            }
            else
            {
                iniCombos("SELECT idgiro,upper(giro) as giro from cgiros where status='1' order by giro asc", cbgiros, "idgiro", "giro", "-SELECIONE UN GIRO-");
                cbgiros.SelectedValue = giro;
            }
            dr.Close();
            c.dbconection().Close();
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
                string telefono =  txtgettelefono.Text;
                int asentamiento;
                try
                {
                    asentamiento = Convert.ToInt32(cbasentamiento.SelectedValue);
                }
                catch
                {
                    asentamiento = 0;
                }
                string Tel_E1 = txtTel_Uno.Text;
                string pagweb = txtweb.Text.Trim();
                string calle = v.mayusculas(txtcalle.Text.ToLower());
                string numero = v.mayusculas(txtnum.Text.ToLower());
                string giro = cbgiros.SelectedValue.ToString();
                string referencias = v.mayusculas(txtreferencias.Text.ToLower());
                string[] telefonosEmpresa = new string[2];
                string[] telefonosContacto= new string[2];
                string observaciones = txtobservaciones.Text.Trim();
                telefonosEmpresa[0] = txtTel_Uno.Text;
                telefonosEmpresa[1] = txtTel_dos.Text;
                telefonosContacto[0] = txtgettelefono.Text;
                telefonosContacto[1] = txtphone.Text;
                if (!this._editar)
                {
                    insertar(empresa,ap,am,nombre,correo,telefonosEmpresa,telefonosContacto, asentamiento,calle,nombre);
                }
                else
                {
                    if (v.formularioProveedores(empresa, ap, am, nombre, correo, telefonosEmpresa, telefonosContacto, cmbTel_uno.SelectedValue.ToString(), cbTel_dos.SelectedValue.ToString(), cbladas.SelectedValue.ToString(), cbladas1.SelectedValue.ToString(), asentamiento.ToString(), calle, numero) && !v.yaExisteProveedorActualizar(empresa,_empresaAnterior, ap,_apAnterior, am,_amAnterior, nombre, _nombreAnterior, correo, _correoAnterior))
                    {
                        string cambios = "empresa = '" +empresa+"'";
                        if(!_apAnterior.Equals(ap)) cambios +=", aPaterno='"+ap+"'";
                        if (!_amAnterior.Equals(am)) cambios += ", aMaterno='" + am + "'";
                        if(!_nombreAnterior.Equals(nombre)) cambios+=", nombres='" +nombre+"'";
                        if (!txtweb.Text.Equals(_paginaweb)) cambios += ", paginaweb = '"+txtweb.Text+"'";
                        if (!(giroAnterior??"0").Equals(cbgiros.SelectedValue.ToString())) cambios += ",Clasificacionfkcgiros = '"+cbgiros.SelectedValue+"'";
                        if (!cmbTel_uno.SelectedValue.ToString().Equals(_idladaanterior1)) cambios += ",idlada1 ='"+cmbTel_uno.SelectedValue+"'";
                        if (!telefonosEmpresa[0].Equals(telefonosAnterioresEmpresa[0])) cambios += ",telefonoEmpresaUno='"+telefonosEmpresa[0]+"'";
                        if (!extAnterior1.Equals(txtext1.Text)) cambios += ",ext1='"+txtext1.Text+"'";
                        if (!cbTel_dos.SelectedValue.ToString().Equals(_idladaanterior2)) cambios += ",idladados='"+cbTel_dos.SelectedValue.ToString()+"'";
                        if (!telefonosEmpresa[1].Equals(telefonosAnterioresEmpresa[1])) cambios += ",telefonoEmpresaDos='"+telefonosEmpresa[1]+"'";
                        if (!extAnterior2.Equals(txtext2.Text)) cambios += ",ext2='" + txtext2.Text + "'";
                        if (!ObservacionesAnterior.Equals(v.mayusculas(observaciones.ToLower()))) cambios += ",observaciones ='"+v.mayusculas(observaciones.ToLower())+"'";
                        if (!idSepomexAnterior.Equals(asentamiento.ToString())) cambios += ",domiciliofksepomex = '"+asentamiento+"'";
                        if (!calleAnterior.Equals(v.mayusculas(calle).ToLower())) cambios += ", Calle ='"+ v.mayusculas(calle.ToLower()) + "'";
                        if (!NumeroAnterior.Equals(v.mayusculas(numero).ToLower())) cambios += ", Numero ='"+ v.mayusculas(numero.ToLower()) + "'";
                        if (!referenciasAnterior.Equals(v.mayusculas(referencias).ToLower())) cambios += ", Referencias ='"+ v.mayusculas(referencias.ToLower()) + "'";
                        if (!_correoAnterior.Equals(correo)) cambios += ",correo='"+correo+"'";
                        if (!cbladas.SelectedValue.ToString().Equals(_idladaanterior3)) cambios += ",idlada3 ='" + cbladas.SelectedValue + "'";
                        if (!telefonosContacto[0].Equals(telefonsAnterioresContacto[0])) cambios += ",telefonoContactoUno='" + telefonosContacto[0] + "'";
                        if (!extAnterior3.Equals(txtext3.Text)) cambios += ",ext3='" + txtext3.Text + "'";
                        if (!cbladas1.SelectedValue.ToString().Equals(_idladaanterior4)) cambios += ",idladacuatro='" + cbladas1.SelectedValue.ToString() + "'";
                        if (!telefonosContacto[1].Equals(telefonsAnterioresContacto[1])) cambios += ",telefonoContactoDos='" + telefonosContacto[1] + "'";
                        if (!extAnterior4.Equals(txtext4.Text)) cambios += ",ext4='" + txtext4.Text + "'";
                        string sql = "UPDATE cproveedores SET " + cambios + " WHERE idproveedor='" + _idproveedorTemp + "'";
                        if (c.insertar(sql))
                        {
                            string extension = "";
                            if (extAnterior1!="")
                            {
                                extension = "Ext. "+extAnterior1;
                            }
                            var domicilio = "Calle: " + calle + ", Número: " + numero + "," + v.getaData("select concat( x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id='" + idSepomexAnterior+"'");
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Proveedores','" + _idproveedorTemp + "','"+_empresaAnterior+";"+_paginaweb+";"+giroAnterior+";(+"+ v.getaData("SELECT phonecode FROM cladas WHERE id='" + _idladaanterior1 + "'") + ")"+telefonosAnterioresEmpresa[0]+" "+extension+";"+observaciones+";"+domicilio+";"+(_nombreAnterior+" "+_apAnterior+" "+_amAnterior)+";"+_correoAnterior+"','" + idUsuario + "',NOW(),'Actualización de Proveedor','" + this.empresa + "','" + area + "')");
                            if (!yaAparecioMensaje) MessageBox.Show("Datos Actualizados Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            insertarums();
                        }
                    }

      
                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void insertar(string empresa,string ap, string am, string nombre, string correo,string[] telefonosEmpresa,string[] telefonosContacto, object asentamiento,string calle,string numero)
        {

            if (v.formularioProveedores(empresa, ap, am, nombre, correo, telefonosEmpresa, telefonosContacto, cmbTel_uno.SelectedValue.ToString(), cbTel_dos.SelectedValue.ToString(), cbladas.SelectedValue.ToString(), cbladas1.SelectedValue.ToString(), asentamiento.ToString(), calle, numero) && !v.existeProveedor(v.mayusculas(empresa), v.mayusculas(ap), v.mayusculas(am), v.mayusculas(nombre), v.mayusculas(correo)))
            {
                string campos = "";
                string valores = "";
                if (!string.IsNullOrWhiteSpace(txtgetap.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "aPaterno";
                        valores = "'" + txtgetap.Text + "'";
                    }
                    else
                    {
                        campos += ",aPaterno";
                        valores += ",'" + txtgetap.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetam.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "aMaterno";
                        valores = "'" + txtgetam.Text + "'";
                    }
                    else
                    {
                        campos += ",aMaterno";
                        valores += ",'" + txtgetam.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetnombre.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "nombres";
                        valores = "'" + txtgetnombre.Text + "'";
                    }
                    else
                    {
                        campos += ",nombres";
                        valores += ",'" + txtgetnombre.Text + "'";
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtobservaciones.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "observaciones";
                        valores = "'" + txtobservaciones.Text + "'";
                    }
                    else
                    {
                        campos += ",observaciones";
                        valores += ",'" + txtobservaciones.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetemail.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "correo";
                        valores = "'" + txtgetemail.Text + "'";
                    }
                    else
                    {
                        campos += ",correo";
                        valores += ",'" + txtgetemail.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtTel_Uno.Text))
                {
                    if (campos == "")
                    {
                        campos = "telefonoEmpresaUno";
                        valores = "'" + txtTel_Uno.Text + "'";
                    }
                    else
                    {
                        campos += ",telefonoEmpresaUno";
                        valores += ",'" + txtTel_Uno.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtTel_dos.Text))
                {
                    if (campos == "")
                    {
                        campos = "telefonoEmpresaDos";
                        valores = "'" + txtTel_dos.Text + "'";
                    }
                    else
                    {
                        campos += ",telefonoEmpresaDos";
                        valores += ",'" + txtTel_dos.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetempresa.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "empresa";
                        valores = "'" + txtgetempresa.Text + "'";
                    }
                    else
                    {
                        campos += ",empresa";
                        valores += ",'" + txtgetempresa.Text + "'";
                    }
                }

                if (!string.IsNullOrWhiteSpace(idSepomex))
                {
                    if (campos == "")
                    {
                        campos = "domiciliofksepomex";
                        valores = "'" + idSepomex + "'";
                    }
                    else
                    {
                        campos += ",domiciliofksepomex";
                        valores += ",'" + idSepomex + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtcalle.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Calle";
                        valores = "'" + txtcalle.Text + "'";
                    }
                    else
                    {
                        campos += ",Calle";
                        valores += ",'" + txtcalle.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtnum.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Numero";
                        valores = "'" + txtnum.Text + "'";
                    }
                    else
                    {
                        campos += ",Numero";
                        valores += ",'" + txtnum.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtweb.Text.Trim()))
                {
                    campos += ",paginaweb";
                    valores += ", '"+txtweb.Text.Trim()+"'";
                }
                if (!string.IsNullOrWhiteSpace(txtreferencias.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Referencias";
                        valores = "'" + txtreferencias.Text + "'";
                    }
                    else
                    {
                        campos += ",Referencias";
                        valores += ",'" + txtreferencias.Text + "'";
                    }
                }
                if (cbgiros.SelectedIndex > 0)
                {
                    if (campos == "")
                    {
                        campos = "Clasificacionfkcgiros";
                        valores = "'" + cbgiros.SelectedValue + "'";
                    }
                    else
                    {
                        campos += ",Clasificacionfkcgiros";
                        valores += ",'" + cbgiros.SelectedValue + "'";
                    }
                }
                if (cmbTel_uno.SelectedIndex > 0)
                {
                    campos += ",idlada";
                    valores += ",'" + cmbTel_uno.SelectedValue + "'";
                }
                if (cbTel_dos.SelectedIndex > 0)
                {
                    campos += ",idladados";
                    valores += ",'" + cbTel_dos.SelectedValue + "'";
                }
                if (cbladas.SelectedIndex > 0)
                {
                    campos += ",idladatres";
                    valores += ",'" + cbladas.SelectedValue + "'";
                }
                if (cbladas1.SelectedIndex > 0)
                {
                    campos += ",idladacuatro";
                    valores += ",'" + cbladas.SelectedValue + "'";
                }

                if (!string.IsNullOrWhiteSpace(txtgettelefono.Text))
                {
                    campos += ",telefonoContactoUno";
                    valores += ",'" + txtgettelefono.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtphone.Text))
                {
                    campos += ",telefonoContactoDos";
                    valores += ",'" + txtphone.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext1.Text))
                {
                    campos += ",ext1";
                    valores += ",'" + txtext1.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext2.Text))
                {
                    campos += ",ext2";
                    valores += ",'" + txtext2.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext3.Text))
                {
                    campos += ",ext3";
                    valores += ",'" + txtext3.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext4.Text))
                {
                    campos += ",ext4";
                    valores += ",'" + txtext4.Text + "'";
                }
                string sql = "INSERT INTO cproveedores(" + campos + ") VALUES(" + valores + ")";
                if (c.insertar(sql))
                {
                    if (c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Proveedores',(SELECT idproveedor FROM cproveedores WHERE empresa='" + empresa + "'),'Inserción de Proveedor','" + idUsuario + "',NOW(),'Inserción de Proveedor','" + this.empresa + "','" + area + "')"))
                    {
                        MessageBox.Show("El Proveedor se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        insertarums();
                    }
                }
            }
        }
        void borrar()
        {
            if(MessageBox.Show("¿Desea Limpiar Todos los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
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
            if (pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                gbinsertmodif.Text = "Agregar Proveedor";
                lblguardar.Text = "Agregar";
                _editar = false;
            }
            cbgiros.SelectedIndex = 0;
            txtTel_Uno.Clear();
            txtTel_dos.Clear();
            txtphone.Clear();
            cmbTel_uno.SelectedIndex = 0;
            cbTel_dos.SelectedIndex = 0;
            cbladas1.SelectedIndex = 0;
            txtcalle.Clear();
            txtnum.Clear();
            txtreferencias.Clear();
            txtgetempresa.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetemail.Clear();
            txtgettelefono.Clear();
            _idproveedorTemp = null;
            txtobservaciones.Clear();
            pcancel.Visible = false;
            peliminarpro.Visible = false;
            cbladas.SelectedIndex = 0;
            txtcp.Clear();
            lblEstado.Text = "";
            lblmunicipio.Text = "";
            lblmunicipio.Text = "";
            lblzona.Text= "";
            lbltipo.Text = "";
            txtext1.Clear();
            txtext2.Clear();
            telefonoEmpresa(true);
            telefonoContacto(true);
            txtext4.Clear();
            cbasentamiento.DataSource = null;
            cbasentamiento.Items.Clear();
            cbasentamiento.Enabled = false;
            idSepomex = null;
            txtweb.Clear();
            txtcp.Clear(); 
            yaAparecioMensaje = false;
            lblciudad.Text = ""; btnguardar.Visible = lblguardar.Visible = true;
            giroAnterior = null;
        }

        private void catProveedores_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'sistrefaccmantDataSet.cladas' Puede moverla o quitarla según sea necesario.

            privilegios();
            if (pconsultar)
            {

                insertarums();
            }
            if (pinsertar || peditar)
            {
                iniCombos("SELECT idgiro,upper(giro) as giro from cgiros where status='1' order by giro asc",cbgiros,"idgiro","giro","-SELECIONE UN GIRO-");
                iniCombos("SELECT idgiro,upper(giro) as giro from cgiros order by giro asc", cbgirosb, "idgiro", "giro", "-SELECIONE UN GIRO-");
                iniLadas();
            }
        }

        private void tbProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString()))
                    {
                        Process.Start(tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString());
                    }
                }
                if (e.ColumnIndex == 10)
                {
                    tbProveedores.ClearSelection();
                    try
                    {
                        Outlook.Application outlookApp = new Outlook.Application();
                        Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                        mailItem.Subject = "Orden de Compra: " + DateTime.Now.ToLongDateString();
                        mailItem.To = tbProveedores.Rows[e.RowIndex].Cells[10].Value.ToString();
                        mailItem.Body = "Adjunte el Archivo pdf Generado en la Orden de Compra.";
                        mailItem.Importance = Outlook.OlImportance.olImportanceNormal;
                        mailItem.Display(false);
                        tbProveedores.ClearSelection();
                    }
                    catch (Exception eX)
                    {
                        MessageBox.Show(eX.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\n\t Ocurrió Un Error!",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void tbProveedores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbProveedores.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void tbProveedores_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string asentamiento = "0";
                    if (cbasentamiento.DataSource != null) asentamiento = cbasentamiento.SelectedValue.ToString();
                    if (((!string.IsNullOrWhiteSpace(txtgetempresa.Text) && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "0").Equals(cbgiros.SelectedValue.ToString()) || !_idladaanterior1.Equals(cmbTel_uno.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !_idladaanterior2.Equals(cbTel_dos.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals(asentamiento) || !calleAnterior.Equals(v.mayusculas(txtcalle.Text.Trim().ToLower())) || !NumeroAnterior.Equals(v.mayusculas(txtnum.Text.Trim().ToLower())) || !referenciasAnterior.Equals(v.mayusculas(txtreferencias.Text.Trim().ToLower())) || !_correoAnterior.Equals(txtgetemail.Text) || !_idladaanterior3.Equals(cbladas.SelectedValue.ToString()) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !_idladaanterior4.Equals(cbladas1.SelectedValue.ToString()) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text))) && status == 1)
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnguardar_Click(null, e);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        } 

        /// <summary>
        /// Muestra en El Formulario Los Datos De La Celda Seleccionada
        /// </summary>
        /// <param name="e"> El Evento Resultante Al Dar Doble Clic En la Tabla</param>
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                limpiar();
                if (pdesactivar)
                {

                    _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();
                    status = v.getStatusInt(tbProveedores.Rows[e.RowIndex].Cells[12].Value.ToString());
                    if (status == 0) 
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeleteuser.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeleteuser.Text = "Desactivar";
                    }
                    peliminarpro.Visible = true;
                }
                if (peditar)
                {

                    try
                    {
                        
                        _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();
                        txtgetempresa.Text = _empresaAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                        txtweb.Text = _paginaweb = tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();

                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString()))
                        {
                            giros_desactivados(tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString());
                            cbgiros.SelectedValue = giroAnterior = tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString();
                        }
                        cmbTel_uno.SelectedValue =   _idladaanterior1 = tbProveedores.Rows[e.RowIndex].Cells[14].Value.ToString();
                        telefonosAnterioresEmpresa[0] = txtTel_Uno.Text = tbProveedores.Rows[e.RowIndex].Cells[15].Value.ToString();
                        cbTel_dos.SelectedValue = _idladaanterior2 = tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString();
                        telefonosAnterioresEmpresa[1] = txtTel_dos.Text = tbProveedores.Rows[e.RowIndex].Cells[17].Value.ToString();
                        extAnterior1 = txtext1.Text = tbProveedores.Rows[e.RowIndex].Cells[26].Value.ToString();
                        extAnterior2 = txtext2.Text = tbProveedores.Rows[e.RowIndex].Cells[27].Value.ToString();
                        ObservacionesAnterior = txtobservaciones.Text = tbProveedores.Rows[e.RowIndex].Cells[5].Value.ToString();
                        idSepomexAnterior = tbProveedores.Rows[e.RowIndex].Cells[18].Value.ToString();

                        calleAnterior =  txtcalle.Text = tbProveedores.Rows[e.RowIndex].Cells[19].Value.ToString();
                        NumeroAnterior = txtnum.Text = tbProveedores.Rows[e.RowIndex].Cells[20].Value.ToString();
                        referenciasAnterior = txtreferencias.Text = tbProveedores.Rows[e.RowIndex].Cells[21].Value.ToString(); ;
                        txtgetap.Text = _apAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[7].Value.ToString().ToLower());
                        txtgetam.Text = _amAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[8].Value.ToString().ToLower());
                        txtgetnombre.Text = _nombreAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[9].Value.ToString().ToLower());
                        txtgetemail.Text = _correoAnterior = tbProveedores.Rows[e.RowIndex].Cells[10].Value.ToString();
                        cbladas.SelectedValue = _idladaanterior3 = tbProveedores.Rows[e.RowIndex].Cells[22].Value.ToString();
                        telefonsAnterioresContacto[0] = txtgettelefono.Text = tbProveedores.Rows[e.RowIndex].Cells[23].Value.ToString();
                        cbladas1.SelectedValue = _idladaanterior4 = tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString();
                        telefonsAnterioresContacto[1] = txtphone.Text = tbProveedores.Rows[e.RowIndex].Cells[25].Value.ToString();
                        extAnterior3 = txtext3.Text = tbProveedores.Rows[e.RowIndex].Cells[28].Value.ToString();
                        extAnterior4 = txtext4.Text = tbProveedores.Rows[e.RowIndex].Cells[29].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(telefonosAnterioresEmpresa[1])) telefonoEmpresa(false);
                        if (!string.IsNullOrWhiteSpace(telefonsAnterioresContacto[1])) telefonoContacto(false);
                        if (!string.IsNullOrWhiteSpace(idSepomexAnterior)) {
                            if (Convert.ToInt32(idSepomexAnterior) > 0) {
                                txtcp.Text = v.getcpFromidSepomex(idSepomexAnterior);
                                button2_Click(null, e);
                                cbasentamiento.SelectedValue = idSepomexAnterior;
                            }
                        }

                        this._editar = true;
                        pcancel.Visible = true;
                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        gbinsertmodif.Text =  "Actualizar Proveedor";
                        lblguardar.Text = "Guardar";
                        tbProveedores.ClearSelection();
                        btnguardar.Visible = lblguardar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usted No Tiene Privilegios Para Editar Éste Catálogo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void telefonoEmpresa(bool aparecer)
        {
            p_AñadeTel.Visible = aparecer;
            psegundo_telefono.Visible = !aparecer;
        }
        void telefonoContacto(bool aparecer)
        {
            psecondphone.Visible =  !aparecer;
            paddPhone.Visible = aparecer;
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
            if (MessageBox.Show("¿Desea " + msg + "activar Al Proveedor?", validaciones.MessageBoxTitle.Confirmar.ToString(),
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
            {
                try
                {

                    String sql = "UPDATE cproveedores SET status = " + status + " WHERE idproveedor  = " + this._idproveedorTemp;
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Proveedores','" + _idproveedorTemp + "','" + msg + "activación de Proveedor','" + idUsuario + "',NOW(),'" + msg + "activación de Proveedor','" + empresa + "','" + area + "')");

                        MessageBox.Show("El Proveedor ha sido " + msg + "activado", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        insertarums();
                    }
                    else
                    {
                        MessageBox.Show("El Proveedor no ha sido desactivado",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkcancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
     
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                limpiar();
                tbProveedores.Rows.Clear();
                string wheres = "";
                string sql = "SELECT t1.idproveedor, UPPER(t1.empresa) AS empresa, COALESCE(t1.paginaweb, '') AS paginaweb, COALESCE(UPPER(t2.giro), '') AS giro, CONCAT(COALESCE((SELECT CONCAT('(+', t33.phonecode, ')', t1.telefonoEmpresaUno, IF(t1.ext1 is null, '', CONCAT(' Ext. ', t1.ext1))) FROM cladas AS t33 WHERE t1.idlada = t33.id), ''), COALESCE((SELECT CONCAT('  (+', t3.phonecode, ')', t1.telefonoEmpresaDos, IF(t1.ext2 is null, '', CONCAT(' Ext. ', t1.ext2))) FROM cladas AS t3 WHERE t1.idladados = t3.id), '')) AS telefonoEmpresa, COALESCE(upper(t1.observaciones), '') AS observaciones, COALESCE((SELECT UPPER(CONCAT('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', t2.tipo, ' ', t2.asentamiento, ', ', municipio, ', ', t2.estado, '. C. P. ', t2.cp)) FROM sepomex AS t2 WHERE t1.domiciliofksepomex = t2.id), '') AS domicilio, UPPER(t1.aPaterno)AS aPaterno, UPPER(t1.AMaterno) AS aMaterno, UPPER(t1.nombres) AS nombres, COALESCE(t1.correo, '') AS correo, CONCAT(COALESCE((SELECT  CONCAT('(+', t33.phonecode, ')', t1.telefonoContactoUno, IF(t1.ext3 is null, '', CONCAT(' Ext. ', t1.ext3))) FROM cladas AS t33 WHERE t1.idladatres = t33.id), ''), COALESCE((SELECT  CONCAT('  (+', t3.phonecode, ')', t1.telefonoContactoDos, IF(t1.ext1 is null, '', CONCAT(' Ext. ', t1.ext1))) FROM cladas AS t3 WHERE t1.idladacuatro = t3.id), '')) AS telefonoContacto, if(t1.status = 1,upper('Activo'),upper(CONCAT('No Activo'))), coalesce(t2.idgiro, '') as idgiro,t1.idlada as idlada1,coalesce(t1.telefonoEmpresaUno, '') as telempresa1,t1.idladados as idlada2,coalesce(t1.telefonoEmpresaDos, '') as telempresa2,COALESCE(t1.domiciliofksepomex, '') as iddomicilio,coalesce(calle, '') as calle,coalesce(numero, '') as numero ,coalesce(referencias, '') as referencias,t1.idladatres as idlada3,coalesce(t1.telefonoContactoUno, '') as telcontacto1,t1.idladacuatro as idlada4,coalesce(t1.telefonoContactoDos, '') as telcontacto2,COALESCE(ext1,'') AS ext1,COALESCE(ext2,'') AS ext2,COALESCE(ext3,'') AS ext3,COALESCE(ext4,'') AS ext4 FROM cproveedores AS t1 LEFT JOIN cgiros AS t2 ON t1.Clasificacionfkcgiros = t2.idgiro  ";
                if (!string.IsNullOrWhiteSpace(txtbempresa.Text))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE t1.empresa  LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%' ";
                    }
                    else
                    {
                        wheres += "AND t1.empresa LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%'";
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
                        wheres += "AND aPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtBNombre.Text))
                {
                    if (wheres == "")
                    {
                        wheres = "where nombres Like '" + v.mayusculas(txtBNombre.Text.ToLower()) + "%'";
                    }
                    else
                    {
                        wheres += "AND nombres Like '" + v.mayusculas(txtBNombre.Text.ToLower()) + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtbemail.Text))
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE correo  LIKE '" + txtbemail.Text + "%' ";
                    }
                    else
                    {
                        wheres += "AnD correo  LIKE '" + txtbemail.Text.ToLower() + "%'";
                    }
                }
                if (cbgirosb.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres = " Where Clasificacionfkcgiros='" + cbgirosb.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " And .Clasificacionfkcgiros='" + cbgirosb.SelectedValue + "'";
                    }
                }
                sql += wheres + " ORDER BY t1.empresa ASC";
                txtbempresa.Clear();
                txtbap.Clear();
                txtbemail.Clear();
                txtBNombre.Clear();
                cbgirosb.SelectedIndex = 0;
                DataTable dt = (DataTable)v.getData(sql);
                var res = dt.Rows.Count;
                if (res == 0)
                {
                    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    insertarums();
                }
                else
                {
                    for (int i = 0; i < res; i++)
                        tbProveedores.Rows.Add(dt.Rows[i].ItemArray);
                }
                tbProveedores.ClearSelection();
                txtbempresa.Clear();
                txtbap.Clear();
                txtbemail.Clear();
                pActualizar.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarums();
        }

        private void txtobservaciones_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetap_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void txtgetemail_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }

        private void txtlada_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string empresa = v.mayusculas(txtgetempresa.Text.ToLower());
            string ap = v.mayusculas(txtgetap.Text.ToLower());
            string am = v.mayusculas(txtgetam.Text.ToLower());
            string nombre = v.mayusculas(txtgetnombre.Text.ToLower());
            string correo = txtgetemail.Text;
            string lada = cbladas.SelectedValue.ToString();
            string telefono = txtgettelefono.Text;
            string pagweb = txtweb.Text.Trim();
            if (((!string.IsNullOrWhiteSpace(txtgetempresa.Text) && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "0").Equals(cbgiros.SelectedValue.ToString()) || !_idladaanterior1.Equals(cmbTel_uno.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !_idladaanterior2.Equals(cbTel_dos.SelectedValue.ToString()) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals(cbasentamiento.SelectedValue.ToString()) || !calleAnterior.Equals(txtcalle.Text.Trim()) || !NumeroAnterior.Equals(txtnum.Text.Trim()) || !referenciasAnterior.Equals(txtreferencias.Text.Trim()) || !_correoAnterior.Equals(txtgetemail.Text) || !_idladaanterior3.Equals(cbladas.SelectedValue.ToString()) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !_idladaanterior4.Equals(cbladas1.SelectedValue.ToString()) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text))) && status==1)
            {
             
                if (MessageBox.Show("Se Han Detectado Cambios En los Datos del Proveedor. \n¿Desea Guardarlos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnguardar_Click(null,e);
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string cp = txtcp.Text;
                lblEstado.Text = "";
                lblmunicipio.Text = "";
                lblzona.Text = "";
                lbltipo.Text = "";
                lblciudad.Text = "";
                if (!string.IsNullOrWhiteSpace(cp)) {
                    string sql = "SELECT  DISTINCT COUNT(id) as id, estado,municipio,COALESCE(ciudad,'') AS ciudad FROM sepomex WHERE cp='" + cp + "'";
                    MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                 
                    if (Convert.ToInt32(cm.ExecuteScalar())>0)
                    {
                        MySqlDataReader dr = cm.ExecuteReader();
                        dr.Read();
                       
                            lblEstado.Text = dr.GetString("estado").ToUpper();
                            lblmunicipio.Text = dr.GetString("municipio").ToUpper();
                        lblciudad.Text = dr.GetString("ciudad").ToUpper();
                        dr.Close();
                        c.dbcon.Close();
                            cbasentamiento.Enabled = true;
                            sql = "SELECT id, upper(asentamiento) as asentamiento FROM sepomex WHERE cp ='" + cp + "' order by asentamiento ASC";
                            DataTable dt1 = new DataTable();
                            MySqlCommand cm1 = new MySqlCommand(sql, c.dbconection());
                                              
                        MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm1);
                            AdaptadorDatos.Fill(dt1);
                            DataRow nuevaFila = dt1.NewRow();
                            nuevaFila["id"] = 0;
                            nuevaFila["asentamiento"] = "--Seleccione un Asentamiento--".ToUpper();
                            dt1.Rows.InsertAt(nuevaFila, 0);
                            cbasentamiento.DataSource = dt1;
                            cbasentamiento.ValueMember = "id";
                            cbasentamiento.DisplayMember = "asentamiento";
                        if (dt1.Rows.Count == 2)
                        {
                            cbasentamiento.SelectedIndex = 1;
                            cbasentamiento.Enabled = false;
                        }else
                        {
                            cbasentamiento.Enabled = true;
                        }
                        c.dbcon.Close();
  
                    }else
                    {
                        MessageBox.Show("No se Encontró el Código Postal", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        vaciarcombo();
                        txtcp.Clear();
                    }
                    
                }else
                {
                    MessageBox.Show("Teclee un Código Postal Válido", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
                    vaciarcombo();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        void vaciarcombo()
        {

            cbasentamiento.DataSource = null;
            cbasentamiento.Items.Clear();
            cbasentamiento.Enabled = false;
            idSepomex = null;
        }
        private void cbasentamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbasentamiento.SelectedIndex>0)
            {
                String sql="SELECT zona,tipo from sepomex WHERE id='"+cbasentamiento.SelectedValue+"'";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                if (dr.FieldCount > 0)
                {
                    dr.Read();}
                lblzona.Text = dr.GetString("zona").ToUpper();
                lbltipo.Text = dr.GetString("tipo").ToUpper();
                idSepomex = cbasentamiento.SelectedValue.ToString();
                dr.Close();
                c.dbconection().Close();
            } else
            {
                lblzona.Text = "";
                lbltipo.Text = "";
                idSepomex = null;
            }
         ;

        }

        private void txtweb_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraPaginasWeb(e);
        }

        private void txtreferencias_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasnumerosdiagonalypunto(e);
        }

        private void txtgetempresa_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void cbladas_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgetempresa_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender,e);
        }

        private void gbinsertmodif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            insertarums();
            pActualizar.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cbladas.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtgettelefono.Text) )
            {
                psecondphone.Visible = true;
                paddPhone.Visible = false;
            }else
            {
                MessageBox.Show("No se Puede Agregar Un Segundo Teléfono Si No Ha Completado el Primero",validaciones.MessageBoxTitle.Advertencia.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                psecondphone.Visible = false;
                paddPhone.Visible = true;

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            catGiros cat = new catGiros(idUsuario,empresa,area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbTel_uno.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtTel_Uno.Text))
            {
                psegundo_telefono.Visible = true;
                p_AñadeTel.Visible = false;
            
            }
            else
            {
                MessageBox.Show("No se Puede Agregar Un Segundo Teléfono Si No Ha Completado el Primero", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                psegundo_telefono.Visible = false;
                pAñade_Tel.Visible = true;
            }
        }

        private void tbProveedores_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void cbgiros_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgetemail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetemail.Text.Trim()))
            {
                if (!v.validacionCorrero(txtgetemail.Text.Trim()))
                {
                    e.Cancel = true;
                    MessageBox.Show("El formato del email ingresado es incorrecto", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtweb_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtweb.Text.Trim()))
            {
                if (v.paginaWebValida(txtweb.Text.Trim()))
                {
                    e.Cancel = true;
                    MessageBox.Show("Introduzca Una Página Web Válida.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        void iniLadas()
        {
            String sql = "SET lc_time_names = 'es_ES';SELECT id, UPPER(CONCAT('+',phonecode,' - ',nicename)) as lada FROM cladas ORDER BY nicename ASC";
            DataTable dt1 = new DataTable();
            MySqlCommand cm1 = new MySqlCommand(sql, c.dbconection());
         
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm1);
            AdaptadorDatos.Fill(dt1);
          
            DataRow nuevaFila = dt1.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["lada"] = "--SELECCIONE LADA--";
            dt1.Rows.InsertAt(nuevaFila, 0);
            DataTable t2 = dt1.Copy();
            DataTable t3 = dt1.Copy();
            DataTable t4 = dt1.Copy();
            cbladas.DataSource = dt1;
            cbladas1.ValueMember =  cbladas.ValueMember = "id";
            cbladas1.DisplayMember = cbladas.DisplayMember = "lada";
            cbladas1.DataSource = t2;
            cmbTel_uno.ValueMember = cbladas.ValueMember = "id";
            cmbTel_uno.DisplayMember = cbladas.DisplayMember = "lada";
            cmbTel_uno.DataSource = t3;
            cbTel_dos.ValueMember = cbladas.ValueMember = "id";
            cbTel_dos.DisplayMember = cbladas.DisplayMember = "lada";
            cbTel_dos.DataSource = t4;

            c.dbconection().Close();
        }
       public void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
        {
            cbx.DataSource = null;
            DataTable dt = (DataTable)v.getData(sql);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila[ValueMember] = 0;
            nuevaFila[DisplayMember] = TextoInicial.ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbx.DisplayMember = DisplayMember;
            cbx.ValueMember = ValueMember;
            cbx.DataSource = dt;

        }

        //void iniGiros()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("idnivel");
        //    dt.Columns.Add("Nombre");
        //    DataRow row = dt.NewRow();
        //    row["idnivel"] = 0;
        //    row["Nombre"] = "--Seleccione Un Giro--".ToUpper();
        //    dt.Rows.Add(row);
        //    row["idnivel"] = 1;
        //    row["Nombre"] = "Minería".ToUpper();
        //    dt.Rows.Add(row);
        //    row = dt.NewRow();
        //    row["idnivel"] = 2;
        //    row["Nombre"] = "Pesca".ToUpper();
        //    dt.Rows.Add(row);

        //    row = dt.NewRow();
        //    row["idnivel"] = 3;
        //    row["Nombre"] = "Bienes Raíces".ToUpper();
        //    dt.Rows.Add(row);

        //    row = dt.NewRow();
        //    row["idnivel"] = 4;
        //    row["Nombre"] = "Construcción".ToUpper();
        //    dt.Rows.Add(row);

        //    row = dt.NewRow();
        //    row["idnivel"] = 5;
        //    row["Nombre"] = "Ganadería".ToUpper();
        //    dt.Rows.Add(row);

        //    row = dt.NewRow();
        //    row["idnivel"] = 6;
        //    row["Nombre"] = "Transporte Aéreo".ToUpper();
        //    dt.Rows.Add(row);
        //    row = dt.NewRow();
        //    row["idnivel"] = 7;
        //    row["Nombre"] = "Turismo".ToUpper();
        //    dt.Rows.Add(row);
        //    row = dt.NewRow();
        //    row["idnivel"] = 8;
        //    row["Nombre"] = "Software".ToUpper();
        //    dt.Rows.Add(row);
        //    row = dt.NewRow();
        //    row["idnivel"] = 9;
        //    row["Nombre"] = "Telecomunicaciones".ToUpper();
        //    dt.Rows.Add(row);
        //    row = dt.NewRow();
        //    row["idnivel"] = 10;
        //    row["Nombre"] = "Metalurgia".ToUpper();
        //    dt.Rows.Add(row);
        //    cbgiros.ValueMember = "idnivel".ToUpper();
        //    cbgiros.DisplayMember = "Nombre";
        //    cbgiros.DataSource = dt;
        //}
    }

}

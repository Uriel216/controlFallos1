using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Globalization;

namespace controlFallos
{
    public partial class catPersonal : Form
    {
        int statusTemp, idUsuarioTemp,idUsuario,empresa,area;
        public int tipoTemp;
        string credencialAnterior = "",apAnterior = "",amAnterior = "",nombresAnterior = "",usuarioAnterior = "",passwordAnterior = "";
        public string puestoAnterior = "";
        bool reactivar, modifusupass, accesSistemaAnterior;
            public bool editar = false;
        
        conexion c = new conexion();
        validaciones v = new validaciones();
        public catPersonal(int idUsuario,int empresa,int area,Image logo)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            csetpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbaccess.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            csetbpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            busqEmpleados.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            busqEmpleados.ColumnAdded += new DataGridViewColumnEventHandler(v.paraDataGridViews_ColumnAdded);
            this.area = area;
            this.empresa = empresa;
            pblogo.BackgroundImage = logo;
        }

    
        private void catPersonal_Load(object sender, EventArgs e)
        {
            privilegiosPersonal();
            busqPuestos();
            if (Pconsultar) {
                busemp();
                Estatus();
            }

            iniacceso();
        }
        public void busqPuestos()
        {
            String sql = "SELECT idpuesto,UPPER(puesto) as puesto FROM puestos WHERE  empresa = " + empresa+" and area = '"+this.area+"' and status= 1 ORDER BY puesto ASC";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);

            csetbpuestos.ValueMember = "idpuesto";
            csetbpuestos.DisplayMember = "puesto";
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idpuesto"] = 0;
            nuevaFila["puesto"] = "--Seleccione un Puesto--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            csetbpuestos.DataSource = dt;
            csetpuestos.ValueMember = "idpuesto";
            csetpuestos.DisplayMember = "puesto";
            DataRow nuevaFila1 = dt1.NewRow();
            nuevaFila1["idpuesto"] = 0;
            nuevaFila1["puesto"] = "--Seleccione un Puesto--".ToUpper();
            dt1.Rows.InsertAt(nuevaFila1, 0);
            csetpuestos.DataSource = dt1;
            c.dbconection().Close();
        }
        void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "Activo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "No activo".ToUpper();
            dt.Rows.Add(row);
            cbstatus.ValueMember = "idnivel".ToUpper();
            cbstatus.DisplayMember = "Nombre";
            cbstatus.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string credencial = txtgetcredencial.Text;
                
                String ap = v.mayusculas(txtgetap.Text.ToLower());
                String am = v.mayusculas(txtgetam.Text.ToLower());
                string nombre = v.mayusculas(txtgetnombre.Text.ToLower());
                int puesto = Convert.ToInt32(csetpuestos.SelectedValue);

                if (!editar)
                {

                    insertar(credencial, ap, am, nombre, puesto);
                }
                else
                {
                    _editar(credencial, ap, am, nombre, puesto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void _editar(string credencial,string ap,string am,string nombre,int puesto)
        {
            if (statusTemp == 0)
            {
                MessageBox.Show("No Puede Modificar A Un Usuario Inactivo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (cbaccess.SelectedIndex == 2)
                {
                    if (credencial == credencialAnterior && (ap + " " + am + " " + nombre) == (apAnterior + " " + amAnterior + " " + nombresAnterior) && csetpuestos.Text == puestoAnterior && v.getAccesoSistemaInt(Convert.ToInt32(cbaccess.SelectedIndex)) == accesSistemaAnterior)
                    {
                        MessageBox.Show("No se Relizaron Cambios.", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar los Campos?.", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(usuarioAnterior))
                        {
                            actualizarDatosP(credencial, ap, am, nombre, puesto);
                            limpiar();

                        }
                        else
                        {
                            if (MessageBox.Show("Al Negar el Acceso a Sistema Se Borrarán el Usuario y la Contraseña de " + nombresAnterior + " " + apAnterior + " " + amAnterior + ".\n¿Desea Continuar?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                eliminarUsuario();
                                actualizarDatosP(credencial, ap, am, nombre, puesto);
                                limpiar();

                            }
                        }
                    }
                }
                else
                {

                    if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto) && !v.yaExisteActualizar(credencial, credencialAnterior, ap, apAnterior, am, amAnterior, nombre, nombresAnterior))
                    {

                        string usu = txtgetusu.Text.Trim();
                        string pass = txtgetpass.Text.Trim();
                        string pass2 = txtgetpass2.Text.Trim();
                        if (!v.formulariousu(usu, pass, pass2))
                        {
                            if (String.IsNullOrWhiteSpace(usuarioAnterior))
                            {
                                if (!v.existeusupass(pass))
                                {
                                    c.insertar("UPDATE cpersonal SET credencial=LTRIM(RTRIM('" + Convert.ToInt32(credencial) + "')),apPaterno=LTRIM(RTRIM('" + ap + "')),apMaterno=LTRIM(RTRIM('" + am + "')),nombres=LTRIM(RTRIM('" + nombre + "')),cargofkcargos=LTRIM(RTRIM('" + puesto + "')) WHERE idPersona =" +
                                      this.idUsuarioTemp);
                                    if (c.insertar("INSERT INTO datosistema(usuariofkcpersonal, usuario, password) VALUES('" + this.idUsuarioTemp + "','" + usu + "','" + v.Encriptar(pass) + "')"))
                                    {
                                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + usu + ";" + v.Encriptar(pass) + "','" + idUsuario + "',NOW(),'Inserción de Usuario','" + empresa + "','" + area + "')");


                                        if (!yaAparecioMensaje) MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        limpiar();
                                    }
                                }
                            }
                            else
                            {
                                if (!v.existeusupassActualizar(usu, usuarioAnterior, pass, v.Desencriptar(passwordAnterior)))
                                {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + usuarioAnterior + ";" + passwordAnterior + "','" + idUsuario + "',NOW(),'Actualización de Usuario','" + empresa + "','" + area + "')");
                                    c.insertar("UPDATE cpersonal SET credencial=LTRIM(RTRIM('" + Convert.ToInt32(credencial) + "')),apPaterno=LTRIM(RTRIM('" + ap + "')),apMaterno=LTRIM(RTRIM('" + am + "')),nombres=LTRIM(RTRIM('" + nombre + "')),cargofkcargos=LTRIM(RTRIM('" + puesto + "')) WHERE idPersona =" + this.idUsuarioTemp);
                                    var res = c.insertar("UPDATE datosistema SET usuario= '" + usu + "',password='" + v.Encriptar(pass) + "' WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");
                                    if (res)
                                    {

                                        if (!yaAparecioMensaje) MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        limpiar();
                                    }
                                }
                            }

                        }
                    }
                }
            
               
            }

        }
        bool yaAparecioMensaje=false;
        void eliminarUsuario()
        {
            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','"+usuarioAnterior+";"+passwordAnterior+"','" + idUsuario + "',NOW(),'Eliminación de Usuario','" + empresa + "','" + area + "')");
            v.EliminarPrivilegios(idUsuarioTemp);
        
            var res = c.insertar("DELETE FROM datosistema WHERE usuariofkcpersonal =" + idUsuarioTemp);
           
        }
        public void actualizarDatosP(string credencial,string ap,string am,string nombres,int puesto)
        {

            if (!v.camposvacioscPersonal(credencial, ap, am, nombres, puesto) && !v.yaExisteActualizar(credencial, credencialAnterior, ap, apAnterior, am, amAnterior, nombres, nombresAnterior))
            {
                if ((!credencial.Equals(credencialAnterior) || (ap + ' ' + am + ' ' + nombres).Equals(apAnterior + ' ' + amAnterior + ' ' + nombresAnterior) || !puesto.Equals(puestoAnterior)))
                {
                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + credencialAnterior + ";" + apAnterior + ";" + amAnterior + ";" + nombresAnterior + ";" + csetpuestos.SelectedValue + "','" + idUsuario + "',NOW(),'Actualización de Datos Personales','" + empresa + "','" + area + "')");
                    var res = c.insertar("UPDATE cpersonal SET credencial=LTRIM(RTRIM('" + credencial + "')),apPaterno=LTRIM(RTRIM('" + ap + "')),apMaterno=LTRIM(RTRIM('" + am + "')),nombres=LTRIM(RTRIM('" + nombres + "')),cargofkcargos=LTRIM(RTRIM('" + puesto + "')) WHERE idPersona =" +
                        this.idUsuarioTemp);
                    
                       
                    
                }
                   
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }
        public void limpiar()
        {
            if (Pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                lblguardar.Text = "Guardar";
                editar = false;
                gbemp.Text = "Agregar Nuevo Empleado";

            }
            else
            {
                gbemp.Enabled = false;  
                gbemp.Text = "";
            }
            btnguardar.Visible = true;
            lblguardar.Visible = true;
            txtgetcredencial.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetusu.Clear();
            txtgetpass.Clear();
            yaAparecioMensaje = false;
            pCancel.Visible = false;
            pprivilegios.Visible = false;
            peliminarusu.Visible = false;
            this.idUsuarioTemp = 0;           
            txtgetpass2.Clear();
            csetpuestos.SelectedIndex = 0;
            cbaccess.SelectedIndex = 0;
            btnlimpiar.BackgroundImage = Properties.Resources.eraser;
            lbllimpiar.Text = "Limpiar";
            pCancel.Visible = false;
            credencialAnterior = "";
            apAnterior = "";
            amAnterior = "";
            reactivar = false;
            nombresAnterior = "";
            puestoAnterior = "";
            usuarioAnterior = "";
            passwordAnterior = "";
            modifusupass = false;

            if (Pconsultar)
            {
                busemp();
            }
        }
        public void busemp()
        {
            busqEmpleados.Rows.Clear();
            String sql = "SELECT t1.idPersona as id, t1.credencial as cred, UPPER(t1.ApPaterno) as ap, UPPER(t1.ApMaterno) as am, UPPER(t1.nombres) as nom, UPPER(t2.puesto) as pu,(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',ApMaterno)) from cpersonal WHERE idPersona=t1.idPersonalaltafkpersona)as persona,t1.cargofkcargos as cargo, t1.area, t1.status,COALESCE(t3.usuario,'') as usu,COALESCE(t3.password,'') as pass FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona WHERE t1.empresa='"+this.empresa+"' and t1.area= '"+this.area+ "' AND (t1.idPersona != 1 AND t1.idPersona != 2 AND t1.idPersona != 3) ORDER BY t1.idPersona DESC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqEmpleados.Rows.Add(dr.GetString("id"), dr.GetString("cred"), dr.GetString("ap"), dr.GetString("am"), dr.GetString("nom"), dr.GetString("pu"), dr.GetString("persona"), dr.GetString("cargo"), dr.GetString("usu"), dr.GetInt32("area"), dr.GetString("pass"), v.getStatusString(dr.GetInt32("status")));
            }
            dr.Close();
            c.dbconection().Close();
            busqEmpleados.ClearSelection();

        }
        public void insertar(string credencial, string ap, string am, string nombre, int puesto)
        {
            if (cbaccess.SelectedIndex>0) {

                string usu = txtgetusu.Text.Trim();
                string pass1 = txtgetpass.Text.Trim();
                string pass2 = txtgetpass2.Text.Trim();
                if (v.getAccesoSistemaInt((int)cbaccess.SelectedIndex))
                {
                    if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto) && !v.yaExisteEmpleado(credencial, ap, am, nombre))
                    {
                        if (!v.formulariousu(usu, pass1, pass2) && !v.existeusupass(pass1))
                        {
                            string sql = "INSERT INTO cpersonal(credencial, ApPaterno, ApMaterno, nombres, cargofkcargos, empresa, idPersonalaltafkpersona, area) VALUES(LTRIM(RTRIM('" + credencial + "')),LTRIM(RTRIM('" + ap + "')),LTRIM(RTRIM('" + am + "')),LTRIM(RTRIM('" + nombre + "')),'" + puesto + "','" + empresa + "','" + idUsuario + "','" + area + "'); ";
                           
                            if (c.insertar(sql))
                            {

                                if (c.insertar("INSERT INTO datosistema(usuariofkcpersonal, usuario, password) VALUES(LTRIM(RTRIM('" + v.idPersonaparaUsuario(credencial) + "')), LTRIM(RTRIM('" + usu + "')), '" + v.Encriptar(pass1) + "')")) {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal',(SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'),'"+credencial+";"+ap+ ";" + am + ";" + nombre + ";" + puesto + ";" + usu + ";" + pass1 + "','" + idUsuario + "',NOW(),'Inserción de Empleado','" + empresa + "','" + area + "')");
                                    if (privilegios!=null)
                                    {
                                        for (int i = 0; i < privilegios.GetLength(0); i++)
                                        {
                                            string ver = privilegios[i, 0];
                                            string insertar = privilegios[i, 1];
                                            string consultar = privilegios[i, 2];
                                            string modificar = privilegios[i, 3];
                                            string eliminar = privilegios[i, 4];
                                            string nombref = privilegios[i, 5];
                                            v.insert(ver, insertar, consultar, modificar, eliminar, nombref, Convert.ToInt32(v.getaData("(SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "')")));
                                            
                                        }
                                        MessageBox.Show("El Empleado Junto Con Sus Privilegios se Han Agregado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("El Empleado ha sido Agregado Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    }
                                      
                                    limpiar();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ha Ocurrido Un Error", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                }
                else
                {
                    if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto) && !v.yaExisteEmpleado(credencial, ap, am, nombre))
                    {
                        string sql = "INSERT INTO cpersonal(credencial, ApPaterno, ApMaterno, nombres, cargofkcargos, empresa, idPersonalaltafkpersona, area) VALUES('" + credencial + "','" + ap + "','" + am + "','" + nombre + "','" + puesto + "','" + empresa + "','" + idUsuario + "','" + area + "')";
                        if (c.insertar(sql))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal',(SELECT idpersona FROM cpersonal WHERE credencial='" + credencial + "' AND CONCAT(apPaterno,apMaterno,nombres)=CONCAT('"+ap+am+nombre+ "')),'" + credencial + ";" + ap + ";" + am + ";" + nombre + ";" + puesto + ";" + usu + ";" + pass1 + "','" + idUsuario + "',NOW(),'Inserción de Empleado','" + empresa + "','" + area + "')");
                            MessageBox.Show("El Empleado ha sido Agregado Existosamente", "Datos Ingresados Existosamente", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            busemp();
                            limpiar();
                        }
                        else
                        {
                            MessageBox.Show("Ha Ocurrido Un Error", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }else
            {
                MessageBox.Show("Seleccione una Opcion en El Campo 'Acceso a Sistema'", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void busqpersonal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (idUsuarioTemp>0 && Peditar && (!v.mayusculas(txtgetcredencial.Text.Trim().ToLower()).Equals(credencialAnterior) || !(v.mayusculas(txtgetap.Text.Trim().ToLower() + " " + txtgetam.Text.Trim().ToLower() + " " + txtgetnombre.Text.Trim().ToLower())).Equals((apAnterior + " " + amAnterior + " " + nombresAnterior)) || !csetpuestos.SelectedValue.Equals(tipoTemp) || !v.getAccesoSistemaInt(Convert.ToInt32(cbaccess.SelectedIndex)).Equals(accesSistemaAnterior)) && statusTemp==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button1_Click(null, e);
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
        void guardarReporte(DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0)
            {
                limpiar();
                statusTemp = v.getStatusInt(busqEmpleados.Rows[e.RowIndex].Cells[11].Value.ToString());
                idUsuarioTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (this.Pdesactivar)
                {
                    if (!this.idUsuario.Equals(Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString())))
                    {
                        if (v.getStatusInt(busqEmpleados.Rows[e.RowIndex].Cells[11].Value.ToString()) == 0)
                        {
                            btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                            lbldeleteuser.Text = "Reactivar";
                            reactivar = true;
                        }
                        else
                        {
                            btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                            lbldeleteuser.Text = "Desactivar";
                            reactivar = false;
                        }
                        peliminarusu.Visible = true;
                    }
                    else
                    {
                        peliminarusu.Visible = !true;
                    }
                }
                if (Peditar)
                {
                    try
                    {

                        editar = true;
                        txtgetcredencial.Text = credencialAnterior = busqEmpleados.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtgetap.Text = apAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                        txtgetam.Text = amAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                        txtgetnombre.Text = nombresAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[4].Value.ToString().ToLower());
                        csetpuestos.SelectedValue = tipoTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[7].Value.ToString());
                        if (csetpuestos.SelectedIndex == -1)
                        {
                            csetpuestos.SelectedIndex = 0;
                            csetpuestos.Focus();
                            if (statusTemp == 1)
                            {
                                MessageBox.Show(v.mayusculas("El Puesto en el que se econtraba el Usuario Ha Sido Desactivado"), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        puestoAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[5].Value.ToString().ToLower());
                        txtgetusu.Text = usuarioAnterior = busqEmpleados.Rows[e.RowIndex].Cells[8].Value.ToString();

                        if (string.IsNullOrWhiteSpace(usuarioAnterior))
                        {
                            cbaccess.SelectedIndex = 2;
                           
                            accesSistemaAnterior = false;
                        }
                        else
                        {
                            cbaccess.SelectedIndex = 1;
                            mostrarUsuarioContrasena();
                            passwordAnterior = busqEmpleados.Rows[e.RowIndex].Cells[10].Value.ToString();
                            accesSistemaAnterior = true;
                        }

                        txtgetusu.Text = usuarioAnterior = busqEmpleados.Rows[e.RowIndex].Cells[8].Value.ToString();
                        btnlimpiar.BackgroundImage = Properties.Resources.add;
                        lbllimpiar.Text = "Nuevo";
                        busqEmpleados.ClearSelection();
                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        lblguardar.Text = "Guardar";
                        gbemp.Text = "Actualizar Información de: " + nombresAnterior + " " + apAnterior + " " + amAnterior;
                        pCancel.Visible = true;
                        gbemp.Enabled = true;
              
                        txtgetusu.Text = usuarioAnterior;
                        if (Pinsertar && Pconsultar && Peditar && Pdesactivar)
                        {
                            txtgetpass.Text = txtgetpass2.Text = v.Desencriptar(passwordAnterior);
                        }
                        if (statusTemp == 1 && cbaccess.SelectedIndex == 1)
                        {
                            pprivilegios.Visible = true;
                        }
                        else
                        {
                            pprivilegios.Visible = false;
                        }
                        btnguardar.Visible = false;
                        lblguardar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);


                    }
                }
                else
                {
                    MessageBox.Show("Usted No Tiene Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
 
        

        private void lblbuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbredencial.Text.Trim()) || !string.IsNullOrWhiteSpace(txtbap.Text.Trim()) || csetbpuestos.SelectedIndex>0 || cbstatus.SelectedIndex>=0) {
                limpiar();
                busqEmpleados.Rows.Clear();
                String sql = "SELECT t1.idPersona as id, t1.credencial as cred, UPPER(t1.ApPaterno) as ap, UPPER(t1.ApMaterno) as am, UPPER(t1.nombres) as nom, UPPER(t2.puesto) as pu,(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',ApMaterno)) from cpersonal WHERE idPersona=t1.idPersonalaltafkpersona)as persona,t1.cargofkcargos as cargo, t1.area, t1.status,COALESCE(t3.usuario,'') as usu,COALESCE(t3.password,'') as pass FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona WHERE t1.empresa='" + this.empresa + "' and t1.area= '" + this.area + "' AND (t1.idPersona != 1 AND t1.idPersona != 2 AND t1.idPersona != 3)";
                string wheres = "";
                if (!string.IsNullOrWhiteSpace(txtbredencial.Text.ToString()))
                {
                    if (wheres == "")
                    {

                        wheres += "AND (t1.credencial = '" + txtbredencial.Text + "'";
                    }
                    else
                    {
                        wheres += "AND t1.credencial = '" + txtbredencial.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtbap.Text.ToString()))
                {
                    if (wheres == "")
                    {
                        wheres += "AND (t1.apPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                    }
                    else
                    {
                        wheres += "AND t1.apPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                    }
                }
                if (csetbpuestos.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres += "AND ( cargofkcargos = '" + csetbpuestos.SelectedValue.ToString() + "'";

                    }
                    else
                    {
                        wheres += "AND cargofkcargos = '" + csetbpuestos.SelectedValue.ToString() + "'";
                    }
                }
           
                    if (wheres == "")
                    {
                        wheres += "AND ( t1.status = '" + v.statusinv(cbstatus.SelectedIndex)  + "'";

                    }
                    else
                    {
                        wheres += "AND t1.status = '" + v.statusinv(cbstatus.SelectedIndex) + "'";
                    }
                
                if (!string.IsNullOrWhiteSpace(wheres))
                {
                    sql += wheres + ") ORDER BY t1.apPaterno ASC";
                }

                txtbredencial.Clear();
                txtbap.Clear();
                csetbpuestos.SelectedIndex = 0;
                busqEmpleados.Rows.Clear();
                cbstatus.SelectedIndex = 0;
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cm.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    busemp();
                }
                else
                {
                    MySqlDataReader dr = cm.ExecuteReader();

                    while (dr.Read())
                    {
                        busqEmpleados.Rows.Add(dr.GetString("id"), dr.GetString("cred"), dr.GetString("ap"), dr.GetString("am"), dr.GetString("nom"), dr.GetString("pu"), dr.GetString("persona"), dr.GetString("cargo"), dr.GetString("usu"), dr.GetInt32("area"), dr.GetString("pass"), v.getStatusString(dr.GetInt32("status")));
                    }
                    dr.Close();
                    c.dbconection().Close();
                    busqEmpleados.ClearSelection();
                    pActualizar.Visible = true;
                }
            }else
            {
                MessageBox.Show("La Búsqueda Debe Contener Por Lo Menos Un Filtro",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        
        private void txtgetusu_TextChanged(object sender, EventArgs e)
        {

        }




        private void button1_Click_2(object sender, EventArgs e)
        {
            string msg;
            string texto;
            int status;
            if (reactivar)
            {
                if (v.yaExisteCredencialReactivar(credencialAnterior))
                {
                    recibirCredencial r = new recibirCredencial(credencialAnterior,idUsuarioTemp,idUsuario,empresa,area);
                r.ShowDialog();
                }
                texto = "¿Desea Reactivar al usuario?";
                status = 1;
                msg = "Re";
            }
            else
            {
                texto = "¿Desea Desactivar al usuario?";
                status = 0;
                msg = "Des";

            }
            if (MessageBox.Show(texto, "Control de Fallos",
       MessageBoxButtons.YesNo, MessageBoxIcon.Question)
       == DialogResult.Yes)
            {
                try
                {
                    String sql = "UPDATE cpersonal SET status = " + status + " WHERE idPersona  = " + this.idUsuarioTemp;
                    if (c.insertar(sql))
                    {
                        if (msg == "Des") v.EliminarPrivilegios(idUsuarioTemp);
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + msg + "activación de Empleado','" + idUsuario + "',NOW(),'" + msg + "activación de Empleado','"+empresa+"','"+area+"')");
                        MessageBox.Show("El empleado ha sido " + msg + "activado Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        busemp();
                        
                    }
                    else
                    {
                        MessageBox.Show("**El empleado no ha sido " + msg);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (editar) {
                if (this.idUsuarioTemp != idUsuario) {
                    Form ps;
                    if (empresa == 1) {

                        ps = new privilegiosSupervision(idUsuarioTemp);
                        ps.Owner = this;
                        ps.ShowDialog();
                    } else
                    {
                        if (area == 1)
                        {
                            ps = new privilegiosMantenimiento(idUsuarioTemp);
                            ps.Owner = this;
                            ps.ShowDialog();
                        } else
                        {
                            ps = new privilegiosAlmacen(idUsuarioTemp);
                            ps.Owner = this;
                            ps.ShowDialog();
                        }
                    }

                } else
                {
                    MessageBox.Show("Sólo otro Usuario con los mismos privilegios puede cambiar sus privilegios", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {

                if (empresa == 1)
                {

                    privilegiosSupervision ps = new privilegiosSupervision();
                    ps.Owner = this;
                    ps.lbltitle.Text = "Nombre del Empleado: " + (txtgetnombre.Text.Trim()+" "+txtgetap.Text.Trim()+" "+txtgetam.Text.Trim());
                    if (privilegios!=null)
                    {
                        ps.insertarPrivilegios(privilegios);
                    }
                    ps.ShowDialog();
                }
                else
                {
                    if (area == 1)
                    {
                      privilegiosMantenimiento  ps = new privilegiosMantenimiento(idUsuarioTemp);
                        ps.Owner = this;
                        if (privilegios != null)
                        {
                            ps.insertarPrivilegios(privilegios);
                        }
                        ps.ShowDialog();
                    }
                    else
                    {
                    privilegiosAlmacen  ps = new privilegiosAlmacen(idUsuarioTemp);
                        ps.Owner = this;
                        if (privilegios != null)
                        {
                            ps.insertarPrivilegios(privilegios);
                        }
                        ps.ShowDialog();
                    }
                }
            }
        }
        public string[,] privilegios=null;
        public void privilegiosPersonal()
        {
            string sql = "SELECT insertar,consultar,editar, desactivar  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = '" + Name + "'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
            Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
            Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
            Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            mdr.Close();
            c.dbconection().Close();
            mostrarPersonal();
            string mostrarCpuesto = "SELECT ver FROM privilegios WHERE usuariofkcpersonal = '"+idUsuario+"' and namform = 'catPuestos'";
            MySqlCommand cm = new MySqlCommand(mostrarCpuesto,c.dbconection());
            bool res = v.getBoolFromInt(Convert.ToInt32(cm.ExecuteScalar()));
            c.dbconection().Close();
            if (res)
            {
                ppuestos.Visible = true;
            }
        }
 
        void mostrarPersonal()
        {
            if (Pconsultar)
            {
                busqEmpleados.Visible = true;
                gbbuscar.Visible = true;
            }
            if (Pinsertar || Peditar)
            {
                 gbemp.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblguardar.Text = "Editar Empleado";
                gbemp.Enabled = false;
            }
            if (Pconsultar && Peditar && Pinsertar && Pdesactivar)
            {
                txtgetpass.UseSystemPasswordChar = false;
                txtgetpass2.UseSystemPasswordChar = false;
            }
        }

        private void gbemp_Enter(object sender, EventArgs e)
        {

        }

        public bool Pinsertar {set; get;}
        public bool Peditar{get;set;}

        private void csetpuestos_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void busqEmpleados_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void busqEmpleados_Leave(object sender, EventArgs e)
        {
            busqEmpleados.ClearSelection();
        }

        private void txtgetap_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetcredencial_TextChanged(object sender, EventArgs e)
        {
            bool acces = accesSistemaAnterior;
            bool access = v.getBoolFromInt(Convert.ToInt32(cbaccess.SelectedValue));
            if (editar)
            {
          
                if(statusTemp==1 && (!string.IsNullOrWhiteSpace(txtgetcredencial.Text) && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text) && csetpuestos.SelectedIndex > 0 && cbaccess.SelectedIndex > 0) && (credencialAnterior!=v.mayusculas(txtgetcredencial.Text.ToLower()) || apAnterior != v.mayusculas(txtgetap.Text.ToLower()).Trim() || amAnterior != v.mayusculas(txtgetam.Text.ToLower()).Trim() || nombresAnterior != v.mayusculas(txtgetnombre.Text.ToLower()).Trim() || tipoTemp != (int)csetpuestos.SelectedValue))
                 {
                    if (access)
                    {
                        if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()) && (v.Desencriptar(passwordAnterior) != txtgetpass.Text.Trim() || txtgetpass.Text.Trim() == txtgetpass2.Text)) lblguardar.Visible = btnguardar.Visible = true; else lblguardar.Visible = btnguardar.Visible = true;
                    }
                    else  lblguardar.Visible = btnguardar.Visible = true;
                }else
                {
                    if (access)
                    {
                        if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && ((txtgetpass.Text.Trim()!=v.Desencriptar(passwordAnterior) && v.Desencriptar(passwordAnterior) != txtgetpass.Text.Trim() && txtgetpass.Text.Trim() == txtgetpass2.Text && txtgetpass.Text.Trim().Length >= 8) || usuarioAnterior!=txtgetusu.Text.Trim())) lblguardar.Visible = btnguardar.Visible = true; else lblguardar.Visible = btnguardar.Visible = false;
                    }
                    else
                    {
                        pprivilegios.Visible = lblguardar.Visible = btnguardar.Visible = false;  
                    }
                    }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtgetcredencial.Text) || !string.IsNullOrWhiteSpace(txtgetap.Text) || !string.IsNullOrWhiteSpace(txtgetam.Text) || !string.IsNullOrWhiteSpace(txtgetnombre.Text) || csetpuestos.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetusu.Text) || !string.IsNullOrWhiteSpace(txtgetpass.Text) || !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()))
                {
                    pCancel.Visible = true;
                }else
                {
                    pCancel.Visible = false;
                }
                if ((!string.IsNullOrWhiteSpace(txtgetcredencial.Text) && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text) && csetpuestos.SelectedIndex > 0 && cbaccess.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()) && txtgetpass.Text.Trim().Length>=8 && txtgetpass.Text.Trim() == txtgetpass2.Text.Trim()) && v.getBoolFromInt(Convert.ToInt32(cbaccess.SelectedValue)))
                {
                    pprivilegios.Visible = true;
                    

                } else
                {
                    pprivilegios.Visible = false;
                    privilegios = null;
                }
            }


        }

        private void txtgetpass2_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbemp_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75,44,52), Color.FromArgb(75, 44, 52), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            busemp();
            pActualizar.Visible = false;
        }

        public bool Pconsultar {set; get;}
        public bool Pdesactivar {set; get;}
        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
  
        }
        private void csetpuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }
        void quitarUsuarioContrasena()
        {
            lblusu.Visible = false;
            lblgusu.Visible = false;
            txtgetusu.Visible = false;
            txtgetpass.Visible = false;
            lblpass.Visible = false;
            lblgpass.Visible = false;
            lblgetpass2.Visible = false;
            txtgetpass2.Visible = false;
            lblgpass2.Visible = false;
            txtgetusu.Clear();
            txtgetpass.Clear();
            txtgetpass2.Clear();
        }
        void mostrarUsuarioContrasena()
        {
            lblusu.Visible = true;
            lblgusu.Visible = true;
            txtgetusu.Visible = true;
            txtgetpass.Visible = true;
            lblpass.Visible = true;
            lblgpass.Visible = true;
            lblgetpass2.Visible = true;
            txtgetpass2.Visible = true;
            lblgpass2.Visible = true;
        }
      
        private void busqEmpleados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (busqEmpleados.Columns[e.ColumnIndex].Name == "Status")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }
        void iniacceso()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Nombre");
            DataRow row = dt.NewRow();
            row["id"] = 0;
            row["Nombre"] = "--Seleccione una Opcion--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 1;
            row["Nombre"] = "Si".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 2;
            row["Nombre"] = "No".ToUpper();
            dt.Rows.Add(row);
            cbaccess.ValueMember = "id";
            cbaccess.DisplayMember = "Nombre";
            cbaccess.DataSource = dt;
        }

        private void cbaccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editar) {
                if (Convert.ToInt32(cbaccess.SelectedValue) == 1)
                {
                    mostrarUsuarioContrasena();
                }
                else
                {
                    quitarUsuarioContrasena();
                }
            }else
            {
                if (Convert.ToInt32(cbaccess.SelectedValue) == 1)
                {
                    if (accesSistemaAnterior)
                    {
                        mostrarUsuarioContrasena();
                        txtgetusu.Text = usuarioAnterior;
                        txtgetpass2.Text = txtgetpass.Text =v.Desencriptar(passwordAnterior);
                    }else
                    {
                        mostrarUsuarioContrasena();
                        modifusupass = true;
                    }
                }else
                {
                    quitarUsuarioContrasena();
                    pprivilegios.Visible=false;
                }
                }
            txtgetcredencial_TextChanged(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (statusTemp == 1 && (!v.mayusculas(txtgetcredencial.Text.Trim().ToLower()).Equals(credencialAnterior) || !(v.mayusculas(txtgetap.Text.Trim().ToLower() + " " +txtgetam.Text.Trim().ToLower() + " " + txtgetnombre.Text.Trim().ToLower())).Equals((apAnterior + " " + amAnterior + " " + nombresAnterior)) || !csetpuestos.SelectedValue.Equals(tipoTemp) || !v.getAccesoSistemaInt(Convert.ToInt32(cbaccess.SelectedIndex)).Equals(accesSistemaAnterior)))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                   button1_Click(null,e);
                    limpiar();
                }
                else
                {
                    limpiar();
                }
            }else
            {
                limpiar();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            catPuestos cat = new catPuestos(idUsuario,empresa,area);
            cat.Owner = this;
            cat.ShowDialog();
        }

    }
}

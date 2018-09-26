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
        int statusTemp;
        public String idPuestoTemp = "";
        string credencialAnterior = "";
        string apAnterior = "";
        string amAnterior = "";
        bool reactivar;
       string nombresAnterior = "";
        string puestoAnterior = "";
        string usuarioAnterior = "";
        string passwordAnterior = "";
        bool modifusupass = false;
        int idUsuarioTemp = 0;
        bool editar = false;
        int idUsuario;
        int idPuesto;
        int tipoTemp = 0;
        int empresa;
        int area;
        conexion c = new conexion();
        validaciones v = new validaciones();
        public catPersonal(int idUsuario,int empresa,int puesto,Image logo)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.idPuesto = puesto;
            this.area = puesto;
            this.empresa = empresa;
            pblogo.BackgroundImage = logo;
        } 
        private void catPersonal_Load(object sender, EventArgs e)
        {
            privilegiosPersonal();
            busqPuestos();
            if (Pconsultar) {
                busemp();
            }

            iniacceso();
        }
        public void busqPuestos()
        {
            String sql = "SELECT idpuesto,puesto FROM puestos WHERE  empresa = "+empresa+" and area = '"+this.area+"' and status= 1";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            csetbpuestos.ValueMember = "idpuesto";
            csetbpuestos.DisplayMember = "puesto";
            csetbpuestos.DataSource = dt;
            csetpuestos.ValueMember = "idpuesto";
            csetpuestos.DisplayMember = "puesto";
            csetpuestos.DataSource = dt1;
        }    
        private void button1_Click(object sender, EventArgs e)
        {
           
                String credencial = txtgetcredencial.Text;
                String ap = txtgetap.Text;
                String am = txtgetam.Text;
                string nombre = txtgetnombre.Text;
                String puesto = csetpuestos.SelectedValue.ToString();
                String usu = txtgetusu.Text;
              
                if (!editar)
                {

                    insertar(credencial, ap, am, nombre, puesto);
                }
                else
            {
                _editar();
            }

        }
        void _editar()
        {
         //   if (statusTemp == 0)
         //   {
         //       MessageBox.Show("No Puede Modificar A Un Usuario Inactivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         //   }
         //   else
         //   {

            
         //      string sql = "";
         //if (!modifusupass)
         //           {
         //               if (credencial == credencialAnterior && (ap + " " + am + " " + nombre) == (apAnterior + " " + amAnterior + " " + nombresAnterior) && csetpuestos.Text == puestoAnterior)
         //              {
         //                       MessageBox.Show("No hay campo para modificar.", "Sin datos Para Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);

         //       }
                //        else
                //        {
                //            if (csetpuestos.Text != "Conductor")
                //            {
                //                if (!v.camposvaciospConductor(credencial, ap, am, nombre) && !v.yaExisteActualizar(credencial, credencialAnterior, ap, apAnterior, am, amAnterior, nombre, nombresAnterior, puesto, usu, usuarioAnterior, passwordAnterior, passwordAnterior))
                //                {

                //                    sql = "UPDATE cpersonal SET credencial = '" + credencial + "', apPaterno = '" + ap + "', apMaterno= '" + am + "', nombres = '" + nombre + "', cargofkcargos = '" + puesto + "', area = '" + area + "'  WHERE idPersona = " + idUsuarioTemp;

                //                    if (c.insertar(sql))
                //                    {
                //                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                //                        lblguardar.Text = "Agregar Empleado";
                //                        editar = false;
                //                        modifusupass = false;

                //                        MessageBox.Show("Registro Actualizado");
                //                        limpiar();
                //                        busemp();
                //                    }
                //                    else
                //                    {
                //                        MessageBox.Show("Registro No Actualizado");
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (!v.camposvaciospConductor(credencial, ap, am, nombre) && !v.yaExisteActualizar(credencial, credencialAnterior, ap, apAnterior, am, amAnterior, nombre, nombresAnterior, puesto, usu, usuarioAnterior, passwordAnterior, passwordAnterior))
                //                {

                //                    sql = "UPDATE cpersonal SET credencial = '" + credencial + "', apPaterno = '" + ap + "', apMaterno= '" + am + "', nombres = '" + nombre + "', cargofkcargos = '" + puesto + "', area = '" + area + "',  usuario = NULL, password= NULL WHERE idPersona = " + idUsuarioTemp;

                //                    if (c.insertar(sql))
                //                    {
                //                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                //                        lblguardar.Text = "Agregar Empleado";
                //                        editar = false;
                //                        modifusupass = false;
                //                        try
                //                        {
                //                            v.EliminarPrivilegios(idUsuarioTemp);
                //                        }
                //                        catch (Exception Ex)
                //                        {
                //                            Console.WriteLine(Ex.ToString());
                //                        }
                //                        MessageBox.Show("Registro Actualizado");
                //                        limpiar();
                //                        busemp();
                //                    }
                //                    else
                //                    {
                //                        MessageBox.Show("Registro No Actualizado");
                //                    }
                //                }
                //            }

                //        }
                //    }
                //    else
                //    {

                //        String pass1 = txtgetpass.Text;
                //        if (credencial == credencialAnterior && (ap + " " + am + " " + nombre) == (apAnterior + " " + amAnterior + " " + nombresAnterior) && pass1 == passwordAnterior)
                //        {

                //            MessageBox.Show("No hay campo para modificar.", "Sin datos Para Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //        }
                //        else
                //        {
                //            if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto, usu, pass1, txtgetpass2.Text, area) && !v.yaExisteActualizar(credencial, credencialAnterior, ap, apAnterior, am, amAnterior, nombre, nombresAnterior, puesto, usu, usuarioAnterior, pass1, passwordAnterior))
                //            {

                //                sql = "UPDATE cpersonal SET credencial = '" + credencial + "', apPaterno = '" + ap + "', apMaterno= '" + am + "', nombres = '" + nombre + "', cargofkcargos = '" + puesto + "',area = '" + area + "' , usuario = '" + usu + "', password = '" + v.Encriptar(pass1) + "' WHERE idPersona = " + this.idUsuarioTemp;
                //                if (c.insertar(sql))
                //                {
                //                    if (Pinsertar)
                //                    {
                //                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                //                        lblguardar.Text = "Agregar Empleado";
                //                    }
                //                    if (!Pinsertar)
                //                    {
                //                        gbemp.Enabled = false;
                //                    }
                //                    editar = false;
                //                    modifusupass = false;

                //                    MessageBox.Show("Registro Actualizado");
                //                    limpiar();
                //                    busemp();
                //                }
                //                else
                //                {
                //                    MessageBox.Show("Registro No Actualizado");
                //                }

                //            }
                //        }
                //    }
                //}
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
            txtgetcredencial.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetusu.Clear();
            txtgetpass.Clear();
            lnkmodifusupass.Visible = false;
            pCancel.Visible = false;
            pprivilegios.Visible = false;
            peliminarusu.Visible = false;
            this.idUsuarioTemp = 0;           
            txtgetpass2.Clear();
            csetpuestos.SelectedIndex = 0;
            cbaccess.SelectedIndex = 1;
            cbaccess.SelectedIndex = 0;
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
            lblguardar.Text = "Agregar Empleado";
            credencialAnterior = "";
            apAnterior = "";
            amAnterior = "";
            reactivar = false;
            nombresAnterior = "";
            puestoAnterior = "";
            usuarioAnterior = "";
            passwordAnterior = "";
            modifusupass = false;
            editar = false;

    }
        public void busemp()
        {
            busqEmpleados.Rows.Clear();
            String sql = "SELECT t1.idPersona as id, t1.credencial as cred, t1.ApPaterno as ap, t1.ApMaterno as am, t1.nombres as nom, t2.puesto as pu,(SELECT CONCAT(nombres,' ',apPaterno,' ',ApMaterno) from cpersonal WHERE idPersona=t1.idPersonalaltafkpersona)as persona,t1.cargofkcargos as cargo, t1.area, t1.status,COALESCE(t3.usuario,'') as usu,COALESCE(t3.password,'') as pass FROM cpersonal as t1 LEFT JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona WHERE t1.empresa='"+this.empresa+"' and t1.area= '"+this.area+"' ORDER BY t1.idPersona DESC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqEmpleados.Rows.Add(dr.GetString("id"), dr.GetString("cred"), dr.GetString("ap"), dr.GetString("am"), dr.GetString("nom"), dr.GetString("pu"), dr.GetString("persona"), dr.GetString("cargo"), dr.GetString("usu"), dr.GetInt32("area"), dr.GetString("pass"), v.getStatusString(dr.GetInt32("status")));
            }
            busqEmpleados.ClearSelection();

        }
        public void insertar(string credencial, string ap, string am, string nombre, string puesto)
        {
            if (v.getAccesoSistemaInt((int)cbaccess.SelectedIndex))
            {
                if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto) && !v.yaExisteEmpleado(credencial, ap, am, nombre))
                {
                    string usu = txtgetusu.Text;
                    string pass1 = txtgetpass.Text;
                    string pass2 = txtgetpass2.Text;
                    if (!v.formulariousu(usu,pass1,pass2) && !v.existeusupass(usu,pass1))
                    {
                        string sql = "INSERT INTO cpersonal(credencial, ApPaterno, ApMaterno, nombres, cargofkcargos, empresa, idPersonalaltafkpersona, area) VALUES('" + credencial + "','" + ap + "','" + am + "','" + nombre + "','" + puesto + "','" + empresa + "','" + idUsuario + "','" + area + "'); ";
                        if (c.insertar(sql))
                        {
                            if (c.insertar("INSERT INTO datosistema(usuariofkcpersonal, usuario, password) VALUES('" + v.idPersonaparaUsuario(credencial) + "', '"+usu+"', '"+v.Encriptar(pass1)+"')")) {
                                MessageBox.Show("El Empleado ha sido Agregado Existosamente", "Datos Ingresados Existosamente", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                busemp();
                                limpiar();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ha Ocurrido Un Error", "Error al Insertar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show("El Empleado ha sido Agregado Existosamente", "Datos Ingresados Existosamente", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            busemp();
                            limpiar();
                        }
                        else
                        {
                            MessageBox.Show("Ha Ocurrido Un Error", "Error al Insertar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                }
            }

        }
        void insertar_usu(int idPersona,string usu, string pass)
        {
        }
        private void busqpersonal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.Pdesactivar)
            {
                if (!this.idUsuario.Equals(Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString())))
                {
                    if (v.getStatusInt(busqEmpleados.Rows[e.RowIndex].Cells[11].Value.ToString()) == 0)
                    {
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeleteuser.Text = "Reactivar Empleado";
                        reactivar = true;
                    }else
                    {
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeleteuser.Text = "Desactivar Empleado";
                        reactivar = false;
                    }
                    peliminarusu.Visible = true;
                }
                else
                {
                    peliminarusu.Visible = !true;
                }
            }
            if (Peditar) {
                try
                {
                  
                    editar = true;
                    idUsuarioTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString());             
                    credencialAnterior = busqEmpleados.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtgetcredencial.Text = credencialAnterior;
                    apAnterior = busqEmpleados.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtgetap.Text = apAnterior;
                    amAnterior = busqEmpleados.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtgetam.Text = amAnterior;
                    nombresAnterior = busqEmpleados.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtgetnombre.Text = nombresAnterior;
                    tipoTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[7].Value.ToString());
                    csetpuestos.SelectedValue = tipoTemp;
                    usuarioAnterior = busqEmpleados.Rows[e.RowIndex].Cells[8].Value.ToString();
                    txtgetusu.Text = usuarioAnterior;
                    passwordAnterior = busqEmpleados.Rows[e.RowIndex].Cells[10].Value.ToString();
                    puestoAnterior = busqEmpleados.Rows[e.RowIndex].Cells[5].Value.ToString();
                    busqEmpleados.ClearSelection();
                    statusTemp = v.getStatusInt(busqEmpleados.Rows[e.RowIndex].Cells[11].Value.ToString());
                    btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblguardar.Text = "Editar Empleado";
                    pCancel.Visible = true;
                    gbemp.Enabled = true;
                    quitarUsuarioContrasena();
                    lnkmodifusupass.Visible = true;
                    pprivilegios.Visible = true;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);


                }
            }else
            {
                MessageBox.Show("Usted No Tiene Privilegios Para Editar","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
 

        private void lnkmodifusupass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarUsuarioContrasena();
            lnkmodifusupass.Visible = false;
            modifusupass = true;
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Desactivar al usuario?", "Control de Fallos",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
            {

                String sql = "UPDATE cpersonal SET status = 0 WHERE idPersona  = " + this.idUsuarioTemp;
                if (c.insertar(sql))
                {
                    MessageBox.Show("El empleado ha sido desactivado");
                    limpiar();
                    busemp();
                }
                else
                {
                    MessageBox.Show("El empleado ha sido desactivado");
                }
            }
        }

        private void lblbuscar_Click(object sender, EventArgs e)
        {
            busqEmpleados.Rows.Clear();
            String sql = "SELECT t1.idPersona as id, t1.credencial as cred, t1.ApPaterno as ap, t1.ApMaterno as am, t1.nombres as nom, t2.puesto as pu,(SELECT CONCAT(nombres,' ',ApPaterno,' ', ApMaterno)  FROM cpersonal WHERE idPersona = t1.idPersonalaltafkpersona) as persona,t1.cargofkcargos as cargo,t1.usuario as usu,t1.area FROM cpersonal as t1 INNER JOIN puestos as t2 ON t1.cargofkcargos = t2.idpuesto WHERE t1.status <2 and idPersona!=1 ";
            string wheres="";
            if (!string.IsNullOrWhiteSpace(txtbredencial.Text.ToString()))
            {
                if (wheres=="") {

                    wheres += "AND (t1.credencial LIKE '%" + txtbredencial.Text + "%'";
                }
                else
                {
                    wheres += "OR t1.credencial LIKE '%" + txtbredencial.Text + "%'";
                }
            }
            if (!string.IsNullOrWhiteSpace(txtbap.Text.ToString()))
            {
                if (wheres == "")
                {
                    wheres += "AND (t1.apPaterno LIKE '%" + txtbap.Text + "%'";
                }
                else
                {
                    wheres+= "or t1.apPaterno LIKE '%" + txtbap.Text + "%'";
                }
            }
            if (wheres == "")
            {
                wheres+= "AND ( cargofkcargos = '" + csetbpuestos.SelectedValue.ToString() + "'";
               
            }
            else
            {
                wheres+= "OR cargofkcargos = '" + csetbpuestos.SelectedValue.ToString() + "'";
            }
            sql += wheres + ")";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqEmpleados.Rows.Add(dr.GetString("id"), dr.GetString("cred"), dr.GetString("ap"), dr.GetString("am"), dr.GetString("nom"), dr.GetString("pu"), v.getAreaString(dr.GetInt32("area")), dr.GetString("persona"), dr.GetString("cargo"), dr.GetString("usu"), dr.GetInt32("area"));
            }
            busqEmpleados.ClearSelection();
        }
        
        private void txtgetusu_TextChanged(object sender, EventArgs e)
        {

        }

        private void bpuestos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    recibirCredencial r = new recibirCredencial(credencialAnterior);
                    DialogResult res = r.ShowDialog();
                    if (res== DialogResult.OK)
                    {
                        if (c.insertar("UPDATE cpersonal SET credencial='"+ r.txtgetcredencial.Text +"' WHERE idpersona ="+this.idUsuarioTemp))
                        {
                            MessageBox.Show("Credencial modificada Exitosamente.", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                texto = "¿Desea Reactivar al usuario?";
                status = 1;
                msg = "Reactivado";
            } else
            {
                texto = "¿Desea Desactivar al usuario?";
                status = 0;
                msg = "Desactivado";

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
                        if (v.EliminarPrivilegios(idUsuarioTemp))
                        {
                            MessageBox.Show("El empleado ha sido " + msg);
                            limpiar();
                            busemp();
                        }
                        else
                        {
                            MessageBox.Show("*El empleado no ha sido " + msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show("**El empleado no ha sido " + msg);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (this.idUsuarioTemp!= idUsuario) {
                Privilegios p = new Privilegios(idUsuarioTemp, empresa,area);
                p.Owner = this;
                p.ShowDialog();
            }else
            {
                MessageBox.Show("Sólo otro Usuario con los mismos privilegios puede cambiar sus privilegios","Advertencia",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

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
            mostrarPersonal();
        }
        public void privilegiosPuestos()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catPuestos'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            PconsultarPuesto = v.getBoolFromInt(mdr.GetInt32("consultar"));
            PinsertarPuesto = v.getBoolFromInt(mdr.GetInt32("insertar"));
            PeditarPuesto = v.getBoolFromInt(mdr.GetInt32("editar"));
            PdesactivarPuesto = v.getBoolFromInt(mdr.GetInt32("desactivar"));
       
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
        }
       
        public bool Pinsertar {set; get;}
        public bool Peditar{get;set;}
        public bool Pconsultar {set; get;}
        public bool Pdesactivar {set; get;}
        public bool PinsertarPuesto { set; get; }
        public bool PeditarPuesto
        {
            get;
            set;
        }
        public bool PconsultarPuesto { set; get; }
        public bool PdesactivarPuesto { set; get; }

    

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            busemp();
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
                if (Convert.ToString(e.Value) == "Activo")
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
            
            row["id"] = 1;
            row["Nombre"] = "Si";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 2;
            row["Nombre"] = "No";
            dt.Rows.Add(row);
           
            cbaccess.ValueMember = "id";
            cbaccess.DisplayMember = "Nombre";
            cbaccess.DataSource = dt;
        }
        void iniasigna()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();

            row["id"] = 1;
            row["Nombre"] = "Si";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 2;
            row["Nombre"] = "No";
            dt.Rows.Add(row);

            cbaccess.ValueMember = "id";
            cbaccess.DisplayMember = "Nombre";
            cbaccess.DataSource = dt;
        }

        private void cbaccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbaccess.SelectedValue)==1)
            {
                mostrarUsuarioContrasena();
            }
            else
            {
                quitarUsuarioContrasena();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                lblguardar.Text = "Agregar Empleado";
            }
            editar = false;
            modifusupass = false;
            limpiar();
            if (!Pinsertar)
            {
                gbemp.Enabled = false;
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

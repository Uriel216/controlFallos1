using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catUnidades : Form
    {
        int idUsuario,empresaa,area;
        String ecotemp = "";

        string ecoAnterior;
        string descAnterior;
       public int empresaAnterior, areaAnterior, servicioAnterior;
        int statusUnidad;
        bool yaAparecioMensaje = false;
        validaciones v = new validaciones();
       public bool editar = false;
        conexion c = new conexion();
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public int EmpresaAnterior
        {
            get
            {
                return empresaAnterior;
            }

            set
            {
                empresaAnterior = value;
            }
        }
        void iniecos()
        {
            cbeco.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--Seleccione un ECO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbeco.DisplayMember = "eco";
            cbeco.ValueMember = "idunidad";
            cbeco.DataSource = dt;
        }
    
        Thread exportar;
        public int ServicioAnterior
        {
            get
            {
                return servicioAnterior;
            }

            set
            {
                servicioAnterior = value;
            }
        }
        
        public catUnidades(int idUsuario,int empresaa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            csetEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbservicio.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbstatus.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbeco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dataGridView1.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresaa = empresaa;
            this.area = area;
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
            mdr.Close();
            c.dbconection().Close();
            mostrar();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddunidad.Visible = true;
            }
            if (pconsultar)
            {
                gbUnidades.Visible = true;
                gbbuscar.Visible = true;
            }
            if (peditar)
            {
                label12.Visible = true;
                label10.Visible = true;
            }
        }
        void iniareas()
        {
            if (csetEmpresa.SelectedIndex > 0)
            {
                String sql1= "SELECT idarea, upper(nombreArea) as nombreArea FROM careas where empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombreArea asc";
            MySqlCommand cmd = new MySqlCommand(sql1, c.dbconection());
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("No se encuentran áreas registradas con la empresa seleccionada", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbareas.Enabled = false;
                    cbareas.DataSource = null;
                }
                else
                {
                    cbareas.DataSource = null;
                    string sql = "SELECT idarea, upper(nombreArea) as nombreArea FROM careas WHERE status ='1' and empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombreArea asc";
                    DataTable dt = (DataTable)v.getData(sql);
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["idarea"] = 0;
                    nuevaFila["nombreArea"] = "--Seleccione un Area--".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    cbareas.DataSource = dt;
                    cbareas.ValueMember = "idarea";
                    cbareas.DisplayMember = "nombreArea";
                    cbareas.Enabled = true;
                }
            }
            else
            {
                cbareas.Enabled = false;
                cbareas.DataSource = null;
            }
        
        }

        public void busqserviciosaComboBox()
        {
            String sql = "SELECT idservicio as id,UPPER(CONCAT(Nombre , ': ',Descripcion)) as servicio FROM cservicios WHERE status = 1 ORDER BY CONCAT(Nombre , ': ',Descripcion) ASC";
            DataTable dt1 = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cm.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("No Hay Servicios Activos. Por lo Tanto No Podrá Insertar o Editar Unidades.", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }   
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt1);
            DataRow nuevaFila = dt1.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["servicio"] = "Sin Servicio Fijo".ToUpper();
            dt1.Rows.InsertAt(nuevaFila, 0);
            cbservicio.DataSource = dt1;
            cbservicio.ValueMember = "id";
            cbservicio.DisplayMember = "servicio";
            c.dbconection().Close();

        }
        public void bunidades()
        {
            dataGridView1.Rows.Clear();
            String sql = @"SELECT t1.idunidad, concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECO, UPPER(t1.descripcioneco) as descripcioneco,UPPER(t3.nombreEmpresa) as nombreEmpresa,UPPER(t2.nombreArea) as nombreArea,UPPER((select if(t1.serviciofkcservicios = 0, 'SIN SERVICIO FIJO', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))) as servicio, t1.Serviciofkcservicios as idservicio,t1.status, t3.idempresa as idcomboempresa, t1.areafkcareas as idcomboarea,t1.consecutivo FROM cunidades  as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idArea INNER JOIN cempresas as t3 ON t2.empresafkcempresas= t3.idEmpresa ORDER BY t1.areafkcareas,concat(t2.identificador,LPAD(consecutivo,4,'0')) ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("descripcioneco"), dr.GetString("nombreEmpresa"), dr.GetString("nombreArea"), dr.GetString("servicio"),v.getStatusString(dr.GetInt32("status")).ToUpper(), dr.GetString("idcomboempresa"), dr.GetString("idcomboarea"), dr.GetString("idservicio"), dr["consecutivo"]);

            }
            dr.Close();
            c.dbconection().Close();
            dataGridView1.ClearSelection();
        }
        private void btnsaveu_Click(object sender, EventArgs e)
        {
            string eco = txtgeteco.Text.Trim().TrimStart('0');
            string desc = txtgetdesc.Text.ToLower();
            int empresa = Convert.ToInt32(csetEmpresa.SelectedValue);
            while (eco.Length<4)
            {
                eco = "0" + eco;
            }
            object area = cbareas.SelectedValue;
            if (!editar)
            {
                if (!v.formularioUnidades(eco, empresa,Convert.ToInt32(area)) && !v.yaExisteECO(lblid.Text+eco))
                {
                    String sql;
                    if (cbservicio.SelectedIndex > 0)
                    {
                        sql = "INSERT INTO cunidades (consecutivo, descripcioneco, areafkcareas,serviciofkcservicios) VALUES ('" + eco.Trim() + "',LTRIM(RTRIM('" + v.mayusculas(desc) + "')),'" +area + "','" + cbservicio.SelectedValue + "')";
                   
                    }
                    else
                    {
                        sql = "INSERT INTO cunidades (consecutivo, descripcioneco, areafkcareas) VALUES ('" + eco.Trim()+ "',LTRIM(RTRIM('" + v.mayusculas(desc) + "')),'" +area + "')";
                    }
                    if (c.insertar(sql))
                    {
                        var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades',(SELECT idunidad FROM cunidades WHERE consecutivo='"+eco.Trim()+"' AND areafkcareas='"+area+"' and status=1 and serviciofkcservicios='"+cbservicio.SelectedValue+"'),'Inserción de Unidad','" + idUsuario + "',NOW(),'Inserción de Unidad','" + empresaa + "','" + area + "')");
                        MessageBox.Show("La Unidad se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("Unidad no Insertada");
                    }
                }
            }
            else
            {
                if (this.statusUnidad==0)
                {
                    MessageBox.Show("No se Puede Modificar Una Unidad Desactivada para el Sistema","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
             
                } else {
                    if (v.mayusculas(eco).Equals(ecoAnterior) && v.mayusculas(desc).Equals(descAnterior) && Convert.ToInt32(csetEmpresa.SelectedValue) == EmpresaAnterior && Convert.ToInt32(cbareas.SelectedValue) == areaAnterior && Convert.ToInt32(cbservicio.SelectedValue) == ServicioAnterior)
                    {
                        MessageBox.Show("No se Relizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else
                    {
                        if (!v.formularioUnidades(eco, empresa,Convert.ToInt32(cbareas.SelectedValue)) && !v.yaExisteECOActualizar(areaAnterior+ eco, lblid.Text+this.ecoAnterior))
                        {
                            try
                            {
                                string sql = "UPDATE cunidades SET consecutivo = '" + eco + "', descripcioneco = '" + v.mayusculas(desc) + "', areafkcareas ='" + cbareas.SelectedValue + "', serviciofkcservicios='" + cbservicio.SelectedValue.ToString() + "'  WHERE idUnidad= '" + ecotemp + "'";
                                if (c.insertar(sql))
                                {
                                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades','"+ecotemp+"','"+ecoAnterior+";"+areaAnterior+";"+v.mayusculas(desc)+";"+cbservicio.SelectedValue+"','" + idUsuario + "',NOW(),'Actualización de Unidad','" + empresaa + "','" + area + "')");
                                    if (!yaAparecioMensaje) {
                                        MessageBox.Show("La Unidad se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }  limpiar();
                                }
                                else
                                {
                                    MessageBox.Show("Unidad no actualizada");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                               
                            }

                        }
                    }
                }
            }
        }
       public void limpiar()
        {
            if (pinsertar)
            {
                lblsaveu.Text = "Agregar";
                btnsaveu.BackgroundImage = controlFallos.Properties.Resources.save;
                gbaddunidad.Text = "Agregar Unidad";
                editar = false;
            }
            if (pconsultar)
            {
                bunidades(); iniecos();
            }
            csetEmpresa.SelectedIndex = 0;
            cbservicio.SelectedIndex = 0;
            lblid.Text = "";
            pcancelu.Visible = false;
            ecotemp = null;
            peliminar.Visible = false;
            txtgeteco.Clear();
            txtgetdesc.Clear(); yaAparecioMensaje = false;
            txtgeteco.Visible = false;
            lblgeteco.Visible = false;
            btnsaveu.Visible = true;
            lblsaveu.Visible = true;
            cbareas.DataSource = null;
        }
        private void catUnidadess_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {

                bunidades();
                iniecos();
            }
            busqempresas();
            
            Estatus();
            catalogos();
        }
        void catalogos()
        {
            var consultaPrivilegios = "SELECT namForm FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver =1 and (namForm= 'catEmpresas' OR namForm ='catServicios' OR namForm ='catAreas')";
            MySqlCommand cm = new MySqlCommand(consultaPrivilegios, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                PrivilegiosVisibles(dr.GetString("namForm").ToLower());
            }
            dr.Close();
            c.dbconection().Close();
        }
        void  PrivilegiosVisibles(string nameform)
        {
            if (nameform=="catempresas")
            {
                pEmpresas.Visible = true;
            }
            if (nameform == "catservicios")
            {
                pServicios.Visible = true;
            }
            if (nameform == "catareas")
            {
                pAreas.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int stat;
                if (statusUnidad==1)
                {
                    msg = "Des";
                    stat = 0;
                } else
                {
                    msg = "Re";
                    stat = 1;
                }
                if (statusUnidad==0 && Convert.ToInt32(v.getaData("SELECT status FROM cempresas WHERE idempresa=(SELECT empresafkcempresas FROM careas WHERE idarea=(SELECT areafkcareas FROM cunidades WHERE idunidad='" + ecotemp+"'))"))==0)
                {
                    MessageBox.Show("No Se Puede Reactivar La Unidad Debido a que La Empresa Ha Sido Desactivada",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                }else if (statusUnidad==0 && Convert.ToInt32(v.getaData("SELECT status FROM careas WHERE idarea=(SELECT areafkcareas from cunidades WHERE idunidad='" + ecotemp+"')"))==0)
                {
                    MessageBox.Show("No Se Puede Reactivar La Unidad Debido a que El Area Ha Sido Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }else if (statusUnidad==0 && Convert.ToInt32(v.getaData("SELECT status FROM cservicios WHERE idservicio=(SELECT serviciofkcservicios FROM cunidades WHERE idunidad='"+ecotemp+"')"))==0)
                {
                    MessageBox.Show("No Se Puede Reactivar La Unidad Debido a que El Servicio Ha Sido Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    if (MessageBox.Show("¿Desea " + msg + "activar la Unidad?", validaciones.MessageBoxTitle.Confirmar.ToString(),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
                    {

                        String sql = "UPDATE cunidades SET status = " + stat + " WHERE idUnidad  = " + this.ecotemp;
                        if (c.insertar(sql))
                        {
                            var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades','" + ecotemp + "','" + msg + "activación de Unidad','" + idUsuario + "',NOW(),'" + msg + "activación de Unidad','" + empresaa + "','" + area + "')");
                            MessageBox.Show("La Unidad se " + msg + "activó Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            bunidades();
                        }
                        else
                        {
                            MessageBox.Show("hubo un Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btncancelu_Click(object sender, EventArgs e)
        {
            string eco = txtgeteco.Text.Trim().TrimStart('0');
            string desc = v.mayusculas(txtgetdesc.Text.ToLower());
           if ((!v.mayusculas(eco).Equals(ecoAnterior) || !v.mayusculas(desc).Equals(descAnterior) || Convert.ToInt32(csetEmpresa.SelectedValue) != EmpresaAnterior || Convert.ToInt32(cbservicio.SelectedValue) != ServicioAnterior) && statusUnidad==1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsaveu_Click(null, e);
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

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                string eco = txtgeteco.Text.Trim().TrimStart('0');
                string desc = v.mayusculas(txtgetdesc.Text.ToLower());
                if (statusUnidad==1 &&( !string.IsNullOrWhiteSpace(ecotemp) && peditar && ( !v.mayusculas(eco).Equals(ecoAnterior) || !v.mayusculas(desc).Equals(descAnterior) || Convert.ToInt32(csetEmpresa.SelectedValue) != EmpresaAnterior || Convert.ToInt32(cbservicio.SelectedValue) != ServicioAnterior)))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsaveu_Click(null, e);
                        guardarReporte(e);
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
        string U_eco;
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                if (pdesactivar)
                {
                    this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());

                    if (statusUnidad == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    peliminar.Visible = true;

                }
                if (peditar)
                {
                    try
                    {
                        statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                        this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        this.U_eco = v.getaData("Select consecutivo from cunidades where idunidad='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'").ToString();
                        csetEmpresa.SelectedValue = EmpresaAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
                        cbservicio.Enabled = true;
                        cbareas.SelectedValue = areaAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
                        if (csetEmpresa.SelectedIndex < 0)
                        {
                            csetEmpresa.SelectedIndex = 0;
                            csetEmpresa.Focus();
                            MessageBox.Show("La Empresa a la Que Pertenecía la Unidad ha Sido Desactivada. Seleccione Otra", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (cbareas.SelectedIndex < 0)
                        {
                            cbareas.SelectedIndex = 0;
                            cbareas.Focus();
                            MessageBox.Show("El Area Al Que Pertenecía la Unidad ha Sido Desactivada. Seleccione Otra", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        txtgeteco.Text = this.ecoAnterior = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
                        descAnterior = v.mayusculas(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                        txtgetdesc.Text = descAnterior;

                        cbservicio.SelectedValue = ServicioAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
                        if (cbservicio.SelectedIndex < 0)
                        {
                            cbservicio.SelectedIndex = 0;
                            cbservicio.Focus();
                            MessageBox.Show("El Servicio al Que Pertenecía la Unidad ha Sido Desactivado. Seleccione Otro", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        btnsaveu.Visible = false;
                        lblsaveu.Visible = false;
                        pcancelu.Visible = true;
                        btnsaveu.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        gbaddunidad.Text ="Actualizar Unidad: "+dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                        lblsaveu.Text = "Guardar";
                        editar = true;
                        dataGridView1.ClearSelection();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usted no tiene los privilegios para Modificar Unidades", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void busqempresas()
        {

            String sql = "SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status = '1' ORDER BY nombreEmpresa ASC";
            DataTable dt = new DataTable();
            DataTable dt1 =(DataTable)v.getData("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status = '0'  ORDER BY nombreEmpresa ASC");
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            if (Convert.ToInt32(cm.ExecuteScalar())==0)
            {
                MessageBox.Show("No Hay Empresas Activas. Por lo Tanto No Podrá Insertar o Editar Unidades.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            DataRow nuevaFila = dt1.NewRow();
            DataRow nuevaFila1 = dt.NewRow();
            csetEmpresa.DataSource = null;
            csetEmpresa.ValueMember = "idempresa";
            csetEmpresa.DisplayMember = "nombreEmpresa";
            nuevaFila["idempresa"] = 0;
            nuevaFila["nombreEmpresa"] = "--Seleccione una empresa--".ToUpper();
            dt1.Rows.InsertAt(nuevaFila, 0);
            nuevaFila1["idempresa"] = 0;
            nuevaFila1["nombreEmpresa"] = "--Seleccione una empresa--".ToUpper();
            dt.Rows.InsertAt(nuevaFila1, 0);
            cbempresa.ValueMember = "idempresa";
            cbempresa.DisplayMember = "nombreEmpresa";
            csetEmpresa.DataSource = dt;
            cbempresa.DataSource = dt1;
            c.dbconection().Close();
        }
        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgetdesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string wheres = "";
                String sql = @"SELECT t1.idunidad, concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECO, UPPER(t1.descripcioneco) as descripcioneco,UPPER(t3.nombreEmpresa) as nombreEmpresa,UPPER(t2.nombreArea) as nombreArea,UPPER((select if(t1.serviciofkcservicios = 0, 'SIN SERVICIO FIJO', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))) as servicio, t1.Serviciofkcservicios as idservicio,t1.status, t3.idempresa as idcomboempresa, t1.areafkcareas as idcomboarea,t1.consecutivo FROM cunidades  as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idArea INNER JOIN cempresas as t3 ON t2.empresafkcempresas= t3.idEmpresa WHERE ";
                if (Convert.ToInt32(cbeco.SelectedValue.ToString()) > 0)
                {
                    if (wheres == "")
                    {
                        wheres = "t1.idUnidad = '" + cbeco.SelectedValue + "' ";
                    }
                    else
                    {
                        wheres += " AND t1.idUnidad = '" + cbeco.SelectedValue + "' ";
                    }
                }
                if (Convert.ToInt32(cbempresa.SelectedValue) > 0)
                {
                    if (wheres=="")
                    {
                        wheres = "empresafkcempresas = '" + cbempresa.SelectedValue.ToString() + "'";
                    }else
                    {
                        wheres += "AND empresafkcempresas = '" + cbempresa.SelectedValue.ToString() + "'";
                    }
                }
                if (cbstatus.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres = " t1.status = " + v.statusinv(cbstatus.SelectedIndex -1) + " ORDER BY t1.areafkcareas,concat(t2.identificador,LPAD(consecutivo,4,'0')) DESC";
                    }
                    else
                    {
                        wheres += "and  t1.status = " + v.statusinv(cbstatus.SelectedIndex -1)+ " ORDER BY t1.areafkcareas,concat(t2.identificador,LPAD(consecutivo,4,'0')) DESC";
                    }
                }
                sql += wheres;
                cbeco.SelectedIndex = 0;
                cbempresa.SelectedIndex = 0;
                cbstatus.SelectedIndex = 0;
                dataGridView1.Rows.Clear();
                cbstatus.SelectedIndex = 0;
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cm.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bunidades();
                }
                else
                {
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("descripcioneco"), dr.GetString("nombreEmpresa"), dr.GetString("nombreArea"), dr.GetString("servicio"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idcomboempresa"), dr.GetString("idcomboarea"), dr.GetString("idservicio"), dr["consecutivo"]);
                    }
                    dr.Close();
                    c.dbconection().Close();
                    dataGridView1.ClearSelection();
                    pActualizar.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            }

     void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["idnivel"] = 0;
            row["Nombre"] = "--SELECCIONE--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "activo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "inactivo".ToUpper();
            dt.Rows.Add(row);
            cbstatus.ValueMember = "idnivel".ToUpper();
            cbstatus.DisplayMember = "Nombre";
            cbstatus.DataSource = dt;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void gbaddunidad_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            catEmpresas cat = new catEmpresas(idUsuario,empresaa,area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            catServicios cat = new catServicios(idUsuario,empresaa,area);
            cat.Owner = this;
            if (csetEmpresa.SelectedIndex>0)
            {
                cat.cbempresa.SelectedValue = csetEmpresa.SelectedValue;
            }
            cat.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            catAreas cat = new catAreas(idUsuario,empresaa,area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void cbservicio_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgeteco_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetdesc_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void getCambios(object sender,EventArgs e)
        {
            if (editar) {
                //if (statusUnidad==1 &&((cbareas.SelectedIndex > 0 && areaAnterior != (int)cbareas.SelectedValue) || csetEmpresa.SelectedIndex > 0 && empresaAnterior != (int)csetEmpresa.SelectedValue  || (!string.IsNullOrWhiteSpace(txtgeteco.Text) && ecotemp != txtgeteco.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgetdesc.Text) && descAnterior != v.mayusculas(txtgetdesc.Text.Trim().ToLower())) || (ServicioAnterior!= (int) cbservicio.SelectedValue)))
                int areaActual;
                try
                {
                    areaActual = Convert.ToInt32(cbareas.SelectedValue);
                }
                catch
                {
                    areaActual = 0;
                }
                if (statusUnidad==1 &&((areaAnterior != areaActual || empresaAnterior != (int)csetEmpresa.SelectedValue || ServicioAnterior != (int)cbservicio.SelectedValue || U_eco != txtgeteco.Text.Trim() || descAnterior != v.mayusculas(txtgetdesc.Text.Trim().ToLower()))) &&( !string.IsNullOrWhiteSpace(txtgeteco.Text) && cbareas.SelectedIndex>0 && csetEmpresa.SelectedIndex > 0))
                {
                    btnsaveu.Visible = true;
                    lblsaveu.Visible = true;
                } else
                {
                    btnsaveu.Visible = false;
                    lblsaveu.Visible = false;
                }
            }
        }
        void Selecciona_unidades()
        {
            if (cbempresa.SelectedIndex > 0)
            {
                string sql = "SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cbempresa.Text + "';";
                MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cmd.ExecuteScalar())!= 0)
                {
                    cbeco.DataSource = null;
                    DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cbempresa.Text + "';");
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["idunidad"] = 0;
                    nuevaFila["eco"] = "--Seleccione un ECO--".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    cbeco.DisplayMember = "eco";
                    cbeco.ValueMember = "idunidad";
                    cbeco.DataSource = dt;
                    cbeco.Enabled = true;
                }
                else
                {
                    MessageBox.Show("La empresa seleccionada no cuenta con unidades registradas en el sistema",validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbempresa.SelectedIndex = 0;
                }
            }
            else
            {
                iniecos();
            }
        }
        private void cbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selecciona_unidades();
        }

        private void gbaddunidad_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bunidades();
            pActualizar.Visible = false;
        }
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "EXPORTANDO";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "EXPORTAR";
        }
        void ExportarExcel()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);

                }
                v.exportaExcel(dataGridView1);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button5_Click(object sender, EventArgs e) 
        {
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void csetEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniareas();
            if (csetEmpresa.SelectedIndex>0)
            {
                v.iniCombos("SELECT idservicio, UPPER(nombre) as nombre FROM cservicios WHERE status=1 and empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombre ASC ", cbservicio, "idservicio", "nombre", "--SELECCIONE UN SERVICIO--");

                cbservicio.Enabled = true;
            }else
            {
                cbservicio.DataSource = null;
                cbservicio.Enabled = false;
            }
        }

        private void cbareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
             
                    if (cbareas.SelectedIndex > 0)
                    {
                    
                        lblid.Text = v.getaData("SELECT COALESCE(identificador,0) FROM careas WHERE idarea='" + cbareas.SelectedValue + "'").ToString();
                    if (!editar)
                    {
                        //txtgeteco.Text = v.getaData("SELECT COALESCE(Max(consecutivo),0)+1 as id FROM cunidades WHERE areafkcareas ='" + cbareas.SelectedValue + "'").ToString();
                } else
                    {
                        if (areaAnterior==(int)cbareas.SelectedValue)
                        {
                            txtgeteco.Text = ecoAnterior;
                        }else
                        {
                            //txtgeteco.Text = v.getaData("SELECT COALESCE(Max(consecutivo),0)+1 as id FROM cunidades WHERE areafkcareas ='" + cbareas.SelectedValue + "'").ToString();
                        }
                    }
                        txtgeteco.Visible = true;
                        lblgeteco.Visible = true;
                    }
                    else
                    {
                        lblid.Text = "";
                        txtgeteco.Clear();
                        txtgeteco.Visible = false;
                        lblgeteco.Visible = false;
                    }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),v.sistema(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
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

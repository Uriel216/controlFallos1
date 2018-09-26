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
    public partial class catUnidades : Form
    {
        int idUsuario;
        String ecotemp = "";
        string ecoAnterior;
        string descAnterior;
        int empresaAnterior, servicioAnterior;
        int statusUnidad;
        validaciones v = new validaciones();
        bool editar = false;
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
        
        public catUnidades(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
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

        public void busqserviciosaComboBox()
        {
            String sql = "SELECT idservicio as id,CONCAT(Nombre , ': ',Descripcion) as servicio FROM cservicios WHERE status = 1";
            DataTable dt1 = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt1);
            DataRow nuevaFila = dt1.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["servicio"] = "Sin Servicio Fijo";
            dt1.Rows.InsertAt(nuevaFila, 0);
            cbservicio.DataSource = dt1;
            cbservicio.ValueMember = "id";
            cbservicio.DisplayMember = "servicio";

        }
        public void bunidades()
        {
            dataGridView1.Rows.Clear();
            String sql = @"SELECT t1.idunidad, t1.ECO, t1.descripcioneco, t2.nombreEmpresa, t1.empresafkcempresas as idcombo,(select if(t1.serviciofkcservicios = 0, ' ', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad))) as servicio, t1.Serviciofkcservicios as idservicio,t1.status FROM cunidades  as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas = t2.idEmpresa ORDER BY t1.ECO DESC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("descripcioneco"), dr.GetString("nombreEmpresa"), dr.GetString("idcombo"), dr.GetString("servicio"), dr["idservicio"],v.getStatusString(dr.GetInt32("status")));
            }
            dataGridView1.ClearSelection();
        }
        private void btnsaveu_Click(object sender, EventArgs e)
        {
            string eco = txtgeteco.Text;
            string desc = txtgetdesc.Text.ToLower();
            if (!editar)
            {
                if (!v.formularioUnidades(eco, desc) && !v.yaExisteECO(txtgeteco.Text))
                {
                    String sql;
                    if (cbservicio.SelectedIndex > 0)
                    {
                        sql = "INSERT INTO cunidades (ECO, descripcioneco, empresafkcempresas,serviciofkcservicios) VALUES ('" + v.mayusculas(eco) + "','" + v.mayusculas(desc) + "','" + csetEmpresa.SelectedValue.ToString() + "','" + cbservicio.SelectedValue.ToString() + "')";
                    }
                    else
                    {
                        sql = "INSERT INTO cunidades (ECO, descripcioneco, empresafkcempresas) VALUES ('" + v.mayusculas(eco) + "','" + v.mayusculas(desc) + "','" + csetEmpresa.SelectedValue.ToString() + "')";
                    }
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("Unidad Insertada");
                        txtgeteco.Clear();
                        txtgetdesc.Clear();
                        cbempresa.SelectedValue = 0;
                        bunidades(); txtgeteco.Clear();
                        txtgetdesc.Clear();
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
                    if (v.mayusculas(eco).Equals(ecoAnterior) && v.mayusculas(desc).Equals(descAnterior) && Convert.ToInt32(csetEmpresa.SelectedValue) == EmpresaAnterior && Convert.ToInt32(cbservicio.SelectedValue) == ServicioAnterior)
                    {
                        MessageBox.Show("No hay Campo Para Modificar", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else
                    {
                        if (!v.formularioUnidades(eco, desc) && !v.yaExisteECOActualizar(eco, this.ecoAnterior))
                        {
                            try
                            {
                                String sql = "UPDATE cunidades SET ECO = '" + v.mayusculas(eco) + "', descripcioneco = '" + v.mayusculas(desc) + "', empresafkcempresas ='" + csetEmpresa.SelectedValue.ToString() + "', serviciofkcservicios='" + cbservicio.SelectedValue.ToString() + "'  WHERE idUnidad= '" + ecotemp + "'";
                                if (c.insertar(sql))
                                {
                                    MessageBox.Show("Unidad actualizada");
                                    limpiar();
                                }
                                else
                                {
                                    MessageBox.Show("Unidad no actualizada");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("El ECO ya ha sido definido anteriormente. Intente de nuevo");
                                Console.WriteLine(ex);
                            }

                        }
                    }
                }
            }
        }
        void limpiar()
        {
            pcancelu.Visible = false;
            ecotemp = null;
            bunidades();
            btnsaveu.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsaveu.Text = "Agregar Unidad";
            editar = false;
            peliminar.Visible = false;
            pcancelu.Visible = false;
            txtgeteco.Clear();
            txtgetdesc.Clear();
        }
        private void catUnidadess_Load(object sender, EventArgs e)
        {
            bunidades();
            busqserviciosaComboBox();
            busqempresas();
            Estatus();
            privilegios();
            catalogos();
        }
        void catalogos()
        {
            var consultaPrivilegios = "SELECT namForm FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver > 0 and (namForm= 'catEmpresas' OR namForm ='catServicios')";
            MySqlCommand cm = new MySqlCommand(consultaPrivilegios, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                PrivilegiosVisibles(dr.GetString("namForm").ToLower());
            }
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
                if (MessageBox.Show("¿Desea "+msg+"activar la Unidad?", "Control de Fallos",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            == DialogResult.Yes)
                {

                    String sql = "UPDATE cunidades SET status = "+stat+" WHERE idUnidad  = " + this.ecotemp;
                    if (c.insertar(sql))
                    {
                        MessageBox.Show("La Unidad se "+msg+"activó Existosamente");
                        limpiar();
                        bunidades();
                    }
                    else
                    {
                        MessageBox.Show("hubo un Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btncancelu_Click(object sender, EventArgs e)
        {

            ecotemp = null;
            bunidades();
            btnsaveu.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsaveu.Text = "Guardar Unidad";
            editar = false;
            peliminar.Visible = false;
            pcancelu.Visible = false;
            txtgeteco.Clear();
            txtgetdesc.Clear();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (pdesactivar)
            {
                this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                
                if (statusUnidad==0)
                {
                    btnsaveu.BackgroundImage = controlFallos.Properties.Resources.up;
                    lblsaveu.Text = "Reactivar Unidad";
                }else
                {
                    btnsaveu.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lblsaveu.Text = "Reactivar Unidad";
                }
                peliminar.Visible = true;

            }
            if (peditar) {
                try
                {
                    this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    this.ecoAnterior = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtgeteco.Text = ecoAnterior;
                    descAnterior = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtgetdesc.Text = descAnterior;
                    csetEmpresa.SelectedValue = this.EmpresaAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
                    if (csetEmpresa.SelectedIndex <0)
                    {
                        csetEmpresa.SelectedIndex = 0;
                        csetEmpresa.Focus();
                        MessageBox.Show("La Empresa a la Que Pertenecía la Unidad ha Sido Desactivada. Seleccione Otra","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    cbservicio.SelectedValue = ServicioAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                    if (cbservicio.SelectedIndex < 0)
                    {
                        cbservicio.SelectedIndex = 0;
                        cbservicio.Focus();
                        MessageBox.Show("El Servicio al Que Pertenecía la Unidad ha Sido Desactivado. Seleccione Otro", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                    pcancelu.Visible = true;
                    btnsaveu.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsaveu.Text = "Editar Unidad";
                    editar = true;
                    dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.ToString(), "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Usted no tiene los privilegios par amodificar Unidades");
            }
        }
        public void busqempresas()
        {

            String sql = "SELECT * FROM cempresas WHERE status = 1";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            DataRow nuevaFila = dt1.NewRow();


            csetEmpresa.DataSource = dt;
            csetEmpresa.ValueMember = "idempresa";
            csetEmpresa.DisplayMember = "nombreEmpresa";
            nuevaFila["idempresa"] = 0;
            nuevaFila["nombreEmpresa"] = "--Seleccione una empresa--";
            dt1.Rows.InsertAt(nuevaFila, 0);

            cbempresa.DataSource = dt1;
            cbempresa.ValueMember = "idempresa";
            cbempresa.DisplayMember = "nombreEmpresa";
        }
        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void txtgetdesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string wheres = "";
           String sql = "SELECT t1.idunidad, t1.ECO, t1.descripcioneco, t2.nombreEmpresa, t1.empresafkcempresas as idcombo,(select if(t1.serviciofkcservicios = 0, ' ', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad))) as servicio, t1.Serviciofkcservicios as idservicio,t1.status FROM cunidades  as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas = t2.idEmpresa WHERE ";
            if (Convert.ToInt32(cbempresa.SelectedValue)>0)
            {

                wheres += " empresafkcempresas = '" + cbempresa.SelectedValue.ToString() + "'";

            }
            if (!string.IsNullOrWhiteSpace(txtgetecoBusq.Text))
            {
                if (wheres == "")
                {
                    wheres = "ECO LIKE '" + txtgetecoBusq.Text + "%' ";
                }else
                {
                    wheres += "OR ECO LIKE '" + txtgetecoBusq.Text + "%' ";
                }
            }

            if (wheres =="")
            {
               wheres ="t1.status = " + v.getStatusFromCombos(cbstatus.SelectedValue.ToString()) + " and t2.status = 1 ORDER BY t1.ECO DESC";
            }
            else
            {
                wheres += "and  t1.status = " + v.getStatusFromCombos(cbstatus.SelectedValue.ToString()) + " and t2.status= 1 ORDER BY t1.ECO DESC";
            }


            sql += wheres;
            txtgetecoBusq.Clear();
            cbempresa.SelectedIndex = 0;
            cbstatus.SelectedIndex= 0;
            dataGridView1.Rows.Clear();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("descripcioneco"), dr.GetString("nombreEmpresa"), dr.GetString("idcombo"), dr.GetString("servicio"), dr["idservicio"], v.getStatusString(dr.GetInt32("status")));
            }
                dataGridView1.ClearSelection();
            lnkrestablecerTabla.Visible = true;
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bunidades();
            lnkrestablecerTabla.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "Activo";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "Inactivo";
            dt.Rows.Add(row);
            cbstatus.ValueMember = "idnivel";
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
            catEmpresas cat = new catEmpresas(idUsuario);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            catServicios cat = new catServicios(idUsuario);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
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
    }
}

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
    public partial class catUnidaesTRI : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        string binanterior,nmotoranterior,ntransmisionAnterior,modeloAnterior,marcaAnterior;
        private string idUnidadTemp
        {
            get;

            set;
        }

        public catUnidaesTRI(Image logo)
        {
            InitializeComponent();
            pblogo.BackgroundImage = logo;
        }

        private void gbUnidades_Enter(object sender, EventArgs e)
        {

        }
        public void bunidades()
        {
            try
            {
                dataGridView1.Rows.Clear();
                String sql = @"SELECT idunidad, ECO, COALESCE(bin,'') as bin,COALESCE(nmotor,'') as nmotor, COALESCE(ntransmision,'') as ntransmision, COALESCE(modelo,'') as modelo, COALESCE(Marca,'') as marca FROM cunidades WHERE status =1 ORDER BY ECO DESC";
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("bin"), dr.GetString("nmotor"), dr.GetString("ntransmision"), dr.GetString("modelo"), dr.GetString("marca"));
                }
                dataGridView1.ClearSelection();
            }catch(Exception ex){
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void catUnidaesTRI_Load(object sender, EventArgs e)
        {
            bunidades();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            gbECO.Text = "Especificaciones de ECO: " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                idUnidadTemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.binanterior = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtgetbin.Text = binanterior;
                this.nmotoranterior = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtgetnmotor.Text = nmotoranterior;
                this.ntransmisionAnterior = txtgettransmision.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
               this.modeloAnterior = txtgetmodelo.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                this.marcaAnterior = txtgetmarca.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                gbECO.Visible = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string eco = txtgetecoBusq.Text;
                string vin = txtgetbinBusq.Text;
                string motor = txtgetnmotorbusq.Text.ToLower();
                string trans = txtgettransmisionbusq.Text.ToLower();
                string modelo = txtgetmodelobusq.Text.ToLower();
                string marca = txtgetmarcaBusq.Text.ToLower();
                dataGridView1.Rows.Clear();
                string wheres = "";
                String sql = @"SELECT idunidad, ECO, COALESCE(bin,'') as bin,COALESCE(nmotor,'') as nmotor, COALESCE(ntransmision,'') as ntransmision, COALESCE(modelo,'') as modelo, COALESCE(Marca,'') as marca FROM cunidades WHERE status =1 ";
                if (!string.IsNullOrWhiteSpace(eco))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (ECO LIKE '" + eco + "%'";
                    }
                    else
                    {
                        wheres += " OR ECO LIKE '" + eco + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(vin))
                {
                    if (wheres=="")
                    {
                        wheres = "AND (bin LIKE '"+vin+"%'";
                    }else
                    {
                        wheres += " OR bin LIKE '" + vin + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(motor))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (nmotor LIKE '" + motor + "%'";
                    }
                    else
                    {
                        wheres += " OR nmotor LIKE '" + motor + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(trans))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (ntransmision LIKE '" + trans + "%'";
                    }
                    else
                    {
                        wheres += " OR ntransmision LIKE '" + trans + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(modelo))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (modelo LIKE '" + modelo + "%'";
                    }
                    else
                    {
                        wheres += " OR modelo LIKE '" + modelo + "%'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(marca))
                {
                    if (wheres == "")
                    {
                        wheres = "AND (marca '" + marca + "%'";
                    }
                    else
                    { 
                        wheres += " OR  marca LIKE'" + marca + "%'";
                    }
                }
                if (wheres!="")
                {
                    wheres += ")";
                }
                sql += wheres + " ORDER BY ECO DESC";

               
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                var res = cm.ExecuteScalar();
                if (Convert.ToInt32(res)>0) {
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr.GetString("idUnidad"), dr.GetString("ECO"), dr.GetString("bin"), dr.GetString("nmotor"), dr.GetString("ntransmision"), dr.GetString("modelo"), dr.GetString("marca"));
                    }
                    dataGridView1.ClearSelection();
                }else
                {
                    MessageBox.Show("No se Encontraron Resultados","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    bunidades();
                }
                txtgetecoBusq.Clear();
                txtgetbinBusq.Clear();
                txtgetnmotor.Clear();
                txtgettransmisionbusq.Clear();
                txtgetmodelobusq.Clear();
                txtgetmarcaBusq.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bunidades();
        }

        private void txtgetecoBusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void txtgetbin_Validating(object sender, CancelEventArgs e)
        {
            if (txtgetbin.Text.Length<17)
            {
                MessageBox.Show("La Longitud del Vin Debe Ser de 17 Caracteres","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Cancel = true ;
            }
        }

        private void txtgetnmotor_Validating(object sender, CancelEventArgs e)
        {

            if (txtgetbin.Text.Length < 17)
            {
                MessageBox.Show("La Longitud del N° De Serie del Motor Debe Ser de 17 Caracteres", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtgettransmision_Validating(object sender, CancelEventArgs e)
        {

            if (txtgetbin.Text.Length < 17)
            {
                MessageBox.Show("La Longitud del N° De Serie de la Transmisión Debe Ser de 17 Caracteres", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtgetbin_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtgetnmotor_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string vin = txtgetbin.Text;
                string motor = txtgetnmotor.Text.ToLower();
                string trans = txtgettransmision.Text.ToLower();
                string modelo = txtgetmodelo.Text.ToLower();
                string marca = txtgetmarca.Text.ToLower();
                if (!v.formularioUnidadesTRI(vin, motor, trans, modelo,marca) && !v.existeUnidadTRI(vin, this.binanterior, motor, this.nmotoranterior, trans, this.ntransmisionAnterior))
                {
                    if (v.mayusculas(vin).Equals(this.binanterior) && v.mayusculas(motor).Equals(this.nmotoranterior) && v.mayusculas(trans).Equals(this.ntransmisionAnterior) && v.mayusculas(modeloAnterior).Equals(modelo))
                    {
                        MessageBox.Show("No se hicieron Modificaciones", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Todos los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            restablecer();
                        }
                    }
                    else
                    {
                        c.insertar("UPDATE cunidades SET bin=LTRIM(RTRIM('" + v.mayusculas(vin) + "')), nmotor=LTRIM(RTRIM('" + v.mayusculas(motor) + "')) ,ntransmision=LTRIM(RTRIM('" + v.mayusculas(trans) + "')), modelo=LTRIM(RTRIM('" + v.mayusculas(modelo) + "')), Marca=LTRIM(RTRIM('" + v.mayusculas(marca) + "')) WHERE idunidad='"+this.idUnidadTemp+"'");
                        restablecer();
                        MessageBox.Show("Especificaciones Guardadas","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            txtgetbin.Clear();
            txtgetnmotor.Clear();
            txtgettransmision.Clear();
            txtgetmodelo.Clear();
            txtgetmarca.Clear();
            binanterior = null;
            nmotoranterior = null;
            ntransmisionAnterior = null;
            modeloAnterior = null;
            idUnidadTemp = null;
            bunidades();
            gbECO.Visible = false;
        }
    }
}

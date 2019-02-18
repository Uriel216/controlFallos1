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
    public partial class catIVA : Form
    {
        int empresa, area;
        public catIVA(int empresa,int area)
        {
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
        }

        conexion co = new conexion();
        validaciones val = new validaciones();
        int idusu = 0; 
        double ivabd;

        private void catIVA_Load(object sender, EventArgs e)
        {
            tiva();
            if(dataGridViewIVA.Rows.Count == 0)
            {
                buttonGuardar.Visible = true;
                label8.Visible = true;
                buttonEditar.Visible = false;
                label3.Visible = false;
                textBoxUsuario.Enabled = true;
                textBoxIVA.Enabled = true;
            }
            else
            {
                buttonGuardar.Visible = false;
                label8.Visible = false;
                buttonEditar.Visible = true;
                label3.Visible = true;
                textBoxUsuario.Enabled = false;
                textBoxIVA.Enabled = false;
            }
        }

        // TODOS LOS MÉTODOS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void limpiar()
        {
            textBoxIVA.Text = "";
            textBoxUsuario.Text = "";
            buttonGuardar.Visible = false;
            label8.Visible = false;
            buttonEditar.Visible = true;
            label3.Visible = true;
        }

        public void tiva()
        {
            dataGridViewIVA.DataSource = val.getData("SELECT t1.iva AS 'IVA (%)', UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres)) AS 'USUARIO QUE DIO DE ALTA', t1.Fecha AS 'FECHA Y HORA DE ALTA' FROM civa AS t1 INNER JOIN cpersonal AS t2 ON t1.personaFKcpersonal = t2.idpersona ORDER BY t1.Fecha");
        }

    

        // ACCIONES CON LOS BOTONES ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textBoxIVA.Text))
            {
                if (idusu != 0)
                {
                    if (string.IsNullOrWhiteSpace(textBoxIVA.Text))
                    {
                        textBoxIVA.Text = "0";
                    }
                    if (Convert.ToDouble(textBoxIVA.Text) != 0)
                    {
                        if (Convert.ToDouble(textBoxIVA.Text) != ivabd)
                        {
                            if (dataGridViewIVA.Rows.Count == 0)
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO civa(iva, personaFKcpersonal, Fecha) VALUES('" + Convert.ToDouble(textBoxIVA.Text) + "', '" + idusu + "', Now())", co.dbconection());
                                cmd.ExecuteNonQuery();
                                co.dbconection().Close();
                                idusu = 0;
                            }
                            else
                            {
                                MySqlCommand cmd = new MySqlCommand("UPDATE civa SET iva = '" + Convert.ToDouble(textBoxIVA.Text) + "'", co.dbconection());
                                cmd.ExecuteNonQuery();
                                co.dbconection().Close();

                                MySqlCommand cmd00 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimamodificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Catalogo De I.V.A.', '1', '" + ivabd + "', '" + idusu + "', now(), 'Modificacion Del I.V.A.', '2', '2')", co.dbconection());
                                cmd00.ExecuteNonQuery();
                                co.dbconection().Close();
                                idusu = 0;
                            }
                            tiva();
                            limpiar();
                            textBoxUsuario.Enabled = false;
                            textBoxIVA.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("No se reconocio ningun cambio", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El IVA debe de ser mayor que 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("La contraseña ingresada es incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }      
            }
            else
            {
                MessageBox.Show("Escriba un  numero para editar el IVA", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {

                ivabd = Convert.ToDouble(val.getaData("SELECT iva FROM civa"));
            }
            catch 
            {
                ivabd = 0;
            }
            textBoxIVA.Text = ivabd.ToString();
            buttonGuardar.Visible = true;
            label8.Visible = true;
            buttonEditar.Visible = false;
            label3.Visible = false;
            textBoxUsuario.Enabled = true;
            textBoxIVA.Enabled = true;
        }

        // VALIDACIONES EN LAS CAJAS DE TEXTO Y/O LISTAS DESPLEGABLES /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxUsuario_Leave(object sender, EventArgs e)
        {
            idusu = Convert.ToInt32(val.getaData("SELECT t1.idPersona FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos AS t3 ON t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxUsuario.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "'"));
         }

        private void textBoxIVA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se admiten números y un solo punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("No puede poner otro punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // DISEÑO DE TODO EL FORMULARIO ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonGuardar.Size = new Size(59, 56);
        }

        private void buttonGuardar_MouseLeave(object sender, EventArgs e)
        {
            buttonGuardar.Size = new Size(54, 51);
        }


        private void buttonEditar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonEditar.Size = new Size(59, 56);
        }

        private void buttonEditar_MouseLeave(object sender, EventArgs e)
        {
            buttonEditar.Size = new Size(54, 51);
        }

        private void groupBoxEdicion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            val.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
    }
}

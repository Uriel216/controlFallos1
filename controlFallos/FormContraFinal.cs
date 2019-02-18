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
    public partial class FormContraFinal : Form
    {
        validaciones val = new validaciones();
        conexion co = new conexion();
        FormFallasMantenimiento FFM;
        OrdenDeCompra ODC ;
        Form f;
        int empresa, area;
        public FormContraFinal(int empresa,int area,Form F)
        {
            InitializeComponent();

            if (empresa == 2 && area == 1) FFM = (FormFallasMantenimiento)F;
            else ODC = (OrdenDeCompra)F;
                this.empresa = empresa;
            this.area = area;
            f = F;
        }


        public void buttonAceptar_Click(object sender, EventArgs e)
        {

            if ((labelNomFin.Text != "."))
            {
                if (empresa == 2 && area == 1)
                {
                    FFM.labelidFinal.Text = labelidFin.Text;
                    this.Close();
                }

                else
                {
                    ODC.labelidFinal.Text = labelidFin.Text;
                    this.Close();
                }
                labelbtn.Text = "1";
            }
            else
            {
                MessageBox.Show("Contraseña Incorrrecta", validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                labelidFin.Text = "";
                DialogResult = DialogResult.None;
                DialogResult = DialogResult.None;

            }
        }

        public void buttonCancelar_Click(object sender, EventArgs e)
        {
            labelbtn.Text = "2";
        }

        public void textBoxUsuFinal_TextChanged(object sender, EventArgs e)
        {
            if(empresa == 2 && area == 1)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxUsuFinal.Text) + "' AND t3.puesto LIKE  'Mecánico%' AND t1.status = '1'", co.dbconection());
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    labelidFin.Text = dr["idPersona"].ToString();
                    labelNomFin.Text = dr["Nombre"] as string;
                }
                else
                {
                    labelNomFin.Text = ".";
                }
                dr.Close();
                co.dbconection().Close();
            }
            else 
            {
                MySqlCommand cmd2 = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxUsuFinal.Text) + "'AND t3.puesto LIKE 'Almacen%' AND t1.status = '1'", co.dbconection());
                MySqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.Read())
                {
                    labelidFin.Text = dr2["idPersona"].ToString();
                    labelNomFin.Text = dr2["Nombre"] as string;
                }
                else
                {
                    labelNomFin.Text = ".";
                }
                dr2.Close();
                co.dbconection().Close();
            }
        }

        public void buttonCancelar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonCancelar.Size = new Size(46, 45);
        }

        public void buttonCancelar_MouseLeave(object sender, EventArgs e)
        {
            buttonCancelar.Size = new Size(41, 40);
        }

        public void buttonAceptar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonAceptar.Size = new Size(46, 45);
        }

        public void buttonAceptar_MouseLeave(object sender, EventArgs e)
        {
            buttonAceptar.Size = new Size(41, 40);
        }
    }
}

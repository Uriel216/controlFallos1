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
 
    public partial class catRefacciones : Form
    {
        Form form;
        Size sizqPanel = new Size(1188, 595);
        Point locationPanel = new Point(236, 117);
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario;
       
        public catRefacciones(Image logo,int idUsuario)
        {
            InitializeComponent();
            pblogo.BackgroundImage = logo;
            this.idUsuario = idUsuario;
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            cerrar();
            pContenedor.Dock = DockStyle.Fill;
            gbsubmenu.Visible = false;
            var form = Application.OpenForms.OfType<nuevaRefaccion>().FirstOrDefault();
            nuevaRefaccion hijo = form ?? new nuevaRefaccion(idUsuario);
            AddFormInPanel(hijo);
        }
      
        private void button12_Click(object sender, EventArgs e)
        {
            if (this.pContenedor.Controls.Count != 0)
            {
                if (MessageBox.Show("¿Está Seguro que Desea Salir?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.pContenedor.Controls.RemoveAt(0);
                    this.form.Close();
                    gbsubmenu.Visible = true;
                    pContenedor.Dock = DockStyle.None;
                    pContenedor.Size = sizqPanel;
                    pContenedor.Location = locationPanel;
                    var form = Application.OpenForms.OfType<ubicaciones>().FirstOrDefault();
                    ubicaciones hijo = form ?? new ubicaciones(this.idUsuario);
                    AddFormInPanel(hijo);
                }

            }
           
        }
        public void AddFormInPanel(Form fh)
        {
            this.form = fh;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.pContenedor.Controls.Add(fh);
            this.pContenedor.Tag = fh;
            fh.Show();
        }
        public void cerrar()
        {
            if (this.pContenedor.Controls.Count != 0)
            {
                if (MessageBox.Show("¿Está Seguro que Desea Salir?","Control de Fallos",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    this.pContenedor.Controls.RemoveAt(0);
                    form.Close();
                }

            }
        }

        private void catRefacciones_Load(object sender, EventArgs e)
        {
         
            cerrar();
            var form = Application.OpenForms.OfType<nuevaRefaccion>().FirstOrDefault();
            nuevaRefaccion hijo = form ?? new nuevaRefaccion(this.idUsuario);
          
            AddFormInPanel(hijo);

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            cerrar(); var form = Application.OpenForms.OfType<ubicaciones>().FirstOrDefault();
            ubicaciones hijo = form ?? new ubicaciones(this.idUsuario);
            AddFormInPanel(hijo);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            cerrar();
            var form = Application.OpenForms.OfType<familias>().FirstOrDefault();
            familias hijo = form ?? new familias(this.idUsuario);
            AddFormInPanel(hijo);
        }

        private void gbsubmenu_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            cerrar();
            var form = Application.OpenForms.OfType<ums>().FirstOrDefault();
           ums hijo = form ?? new ums(this.idUsuario);
            AddFormInPanel(hijo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cerrar();
            var form = Application.OpenForms.OfType<marcas>().FirstOrDefault();
            marcas hijo = form ?? new marcas(this.idUsuario);
            AddFormInPanel(hijo);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class recibirCredencial : Form
    {
        validaciones v = new validaciones();
        public recibirCredencial(string credencial)
        {
            InitializeComponent();
            lblmsg.Text = "La Credencial Num. "+credencial+" ya está en uso. Ingrese una nueva";
        }

        private void recibirCredencial_Load(object sender, EventArgs e)
        {

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtgetcredencial_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }
    }
}

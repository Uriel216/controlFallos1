using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class ubicaciones : Form
    {
        string idCharolaAnterior;
        string pasilloAnterior;
        string anaquelAnterior;
        string charolaAnterior;
        int _status;
        bool editar;
        int idUsuario;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        conexion c = new conexion();
        validaciones v = new validaciones();
        public ubicaciones(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario=idUsuario;
        }

        public void establecerPrivilegios()
        {
            string sql = "SELECT insertar,consultar,editar, desactivar  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
                Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
                Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
                Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            }
            c.dbcon.Close();
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddubicacion.Visible = true;
            }
            if (Pconsultar)
            {
                gbubicaciones.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            catPasillos cP = new catPasillos(this.idUsuario);
            cP.Owner = this;
            cP.ShowDialog();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void ubicaciones_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
            {
                busqUbic();
            }
            if (Pconsultar)
            {
                insertarUbicaciones();
            }
        }
       public  void busqUbic()
        {
           string sql = "SELECT idpasillo,pasillo FROM cpasillos WHERE status='1'";
                DataTable dt = new DataTable();
                DataRow nuevaFila = dt.NewRow();
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
                AdaptadorDatos.Fill(dt);
            c.dbcon.Close(); ;
                cbpasillo.ValueMember = "idpasillo";
                cbpasillo.DisplayMember = "pasillo";
                nuevaFila["idpasillo"] = 0;
                nuevaFila["pasillo"] = "--Seleccione pasillo--";
                dt.Rows.InsertAt(nuevaFila, 0);
                cbpasillo.DataSource = dt;
                   }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertarCharola();
                }
                else
                {
                    _editarCharola();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void _insertarCharola()
        {
            string charola = txtcharola.Text;
            if (cbpasillo.SelectedIndex > 0) {
                if (cbanaquel.SelectedIndex>0) {
                    if (!v.existeCharola(cbanaquel.SelectedValue.ToString(), charola)) {
                        if (!v.formularioUbicaciones(null, null, charola, 2)) {
                            if (c.insertar("INSERT INTO ccharolas (charola, anaquelfkcanaqueles) VALUES ('" + charola + "','" + cbanaquel.SelectedValue + "')"))
                            {
                                MessageBox.Show("Ubicacion Insertada");
                               
                                busqUbic();
                                limpiar();
                    
                            }

                            else
                            {
                                limpiar();
                            }
                        }

                    }
                }else
                {

                    MessageBox.Show("Seleccione un Anaquel de la Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {

                MessageBox.Show("Seleccione un Pasillo de la Lista desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  
        void _editarCharola()
        {
            if (!string.IsNullOrWhiteSpace(idCharolaAnterior)) {
                string anaquel = cbanaquel.SelectedValue.ToString();
                string charola = txtcharola.Text;
                if (!v.formularioUbicaciones(null, null, charola, 2))
                {
                    if (anaquel.Equals(this.anaquelAnterior) && charola.Equals(this.charolaAnterior))
                    {
                        MessageBox.Show("No Se Realizaron Modificaciones", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("Desea Limpiar Los Campos", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    } else {
                        if (!v.existeCharolaActualizar(anaquel, charola, this.charolaAnterior)) {
                            var res = c.insertar("UPDATE ccharolas SET anaquelfkcanaqueles='" + anaquel + "', charola= '" + charola + "' WHERE idcharola ='" + idCharolaAnterior + "'");
                            MessageBox.Show("Ubicación Actualizada", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                }
            }else
            {
                MessageBox.Show("Seleccione una Charola Para Editar","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT idanaquel,anaquel FROM canaqueles WHERE status='1' and pasillofkcpasillos = '"+cbpasillo.SelectedValue+"'";
            DataTable dt = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbanaquel.ValueMember = "idanaquel";
            cbanaquel.DisplayMember = "anaquel";
            nuevaFila["idanaquel"] = 0;
            nuevaFila["anaquel"] = "--Seleccione Anaquel--";
            dt.Rows.InsertAt(nuevaFila, 0);
            cbanaquel.DataSource = dt;
        }

        private void btnaddanaquel_Click(object sender, EventArgs e)
        {
            catAnaqueles cat = new catAnaqueles(this.idUsuario);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void txtcharola_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        void limpiar()
        {
            cbpasillo.SelectedIndex = 0;
            cbanaquel.SelectedIndex = 0;
            txtcharola.Clear();

            pdelete.Visible = false;
            padd.Visible = false;
            idCharolaAnterior = null;
            pasilloAnterior = null;
            anaquelAnterior = null;
            charolaAnterior = null;
           
            if (Pinsertar)
            {
                btnsave.BackgroundImage = Properties.Resources.pencil;
                lbladdcharola.Text = "Agregar Charola";
                editar = false;
            }
            if (Pconsultar)
            {
                insertarUbicaciones();
            }  
           
          
        }

        public void insertarUbicaciones()
        {
            tbubicaciones.Rows.Clear();
            string sql = "SELECT t1.idcharola as id,t3.pasillo as p,t2.anaquel as a,t1.charola as c,t1.status,t3.idpasillo,t2.idanaquel FROM ccharolas AS t1 INNER JOIN canaqueles as t2 ON t1.anaquelfkcanaqueles = t2.idanaquel INNER JOIN cpasillos as t3 ON t2.pasillofkcpasillos=t3.idpasillo  ORDER BY pasillo,anaquel,charola ASC";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbubicaciones.Rows.Add(dr.GetString("id"), dr.GetString("p"), dr.GetString("a"), dr.GetString("c"), v.getStatusString(dr.GetInt32("status")),dr.GetString("idpasillo"), dr.GetString("idanaquel"));
            }
            tbubicaciones.ClearSelection();
        }

       

        private void tbubicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbubicaciones.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void pEliminarClasificacion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btndelcla_Click(object sender, EventArgs e)
        {
            if (Pdesactivar)
            {
                if (!string.IsNullOrWhiteSpace(idCharolaAnterior))
                {


                    try
                    {
                        string msg;
                        int status;

                        if (this._status == 0)
                        {
                            msg = "Re";
                            status = 1;
                        }
                        else
                        {
                            msg = "Des";

                            status = 0;
                        }
                        if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar La Ubicacion?\n", "Control de  Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            c.insertar("UPDATE ccharolas SET status = '" + status + "' WHERE idcharola=" + this.idCharolaAnterior);
                            limpiar();
                            MessageBox.Show("La Ubicacion se ha " + msg + "activado Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }else
                {
                    MessageBox.Show("Seleccione una Ubicacion para Desactivar","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idCharolaAnterior = (string)tbubicaciones.Rows[e.RowIndex].Cells[0].Value;
                _status = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar) {
                    if (_status == 0)
                    {

                        btndelcha.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelcha.Text = "Reactivar Charola";
                    }
                    else
                    {

                        btndelcha.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelcha.Text = "Desactivar Charola";
                        pdelete.Visible = true;
                    }
                }
                if (Peditar)
                {
                    cbpasillo.SelectedValue = pasilloAnterior = (string)tbubicaciones.Rows[e.RowIndex].Cells[5].Value;
                    cbanaquel.SelectedValue = anaquelAnterior = (string)tbubicaciones.Rows[e.RowIndex].Cells[6].Value;
                    txtcharola.Text = charolaAnterior = (string)tbubicaciones.Rows[e.RowIndex].Cells[3].Value;
                    editar = true;
                    padd.Visible = true;
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lbladdcharola.Text = "Editar Charola";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}

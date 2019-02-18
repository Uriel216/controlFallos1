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
    public partial class catNiveles : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        bool editar;
        string idnivelAnterior,idpasilloAnterior,NivelAnterior;
        int idUsuario, empresa, area;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        int _state;
        public catNiveles(int empresa,int area, int idUsuario)
        {
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
        }

     
        public void establecerPrivilegios()
        {
            string[] privilegios  = v.getaData("SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar)  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'").ToString().Split(';');
            
                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegios[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegios[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegios[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegios[3]));
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddnivel.Visible = true;
            }
            if (Pconsultar)
            {
                gbniveles.Visible = true;
            }
            if (Peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
        }
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }else
                {
                    Editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        void Editar()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            string nivel = v.mayusculas(txtnivel.Text.Trim().ToLower());
            if (!v.existeNivelActualizar(pasillo, Convert.ToInt32(idpasilloAnterior),nivel,NivelAnterior))
            {
                if (c.insertar("UPDATE cniveles SET pasillofkcpasillos='" + pasillo + "', nivel='" + nivel + "' WHERE idnivel='" + idnivelAnterior + "'")) { if (!yaAparecioMensaje) MessageBox.Show("El Nivel Ha Sido Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); limpiar(); }
            }
                
        }
        private void cbpasillo_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        void cargarNiveles()
        {
            dtniveles.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idnivel,upper(t2.pasillo),upper(t1.nivel),(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')),idpasillo FROM cniveles as t1 INNER JOIN cpasillos AS t2 ON t1.pasillofkcpasillos=t2.idpasillo");
            for (int i=0;i<dt.Rows.Count;i++)
            {
                dtniveles.Rows.Add(dt.Rows[i].ItemArray);
            }
            dtniveles.ClearSelection();
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false; btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
            }
            cbpasillo.SelectedIndex = 0;
            txtnivel.Clear();
            yaAparecioMensaje = false;
            if (Pconsultar) cargarNiveles();
            ubicaciones u = (ubicaciones)Owner;
            u.insertarUbicaciones();
            u.busqUbic();
            pdelete.Visible = false;
            pCancelar.Visible = false;
            idnivelAnterior = null;
            NivelAnterior = null;
            idpasilloAnterior = null;
        }

        private void dtniveles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtniveles.Columns[e.ColumnIndex].Name == "estatus")
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

        private void btndelpa_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this._state == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán Los Anaqueles y Charolas Asociados a A él";
                }

                if (MessageBox.Show("¿Está Seguro que Desea " + msg + "activar El Nivel? \n " + msg2 + "", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res0 = c.insertar("UPDATE cniveles SET status='"+status+"' WHERE idnivel='"+idnivelAnterior+"'");
                    var res = c.insertar("UPDATE canaqueles SET status = '" + status + "' WHERE nivelfkcniveles= " + this.idnivelAnterior);
                    var res1 = c.insertar("UPDATE ccharolas as t1 INNER JOIN canaqueles as t2 ON t1.anaquelfkcanaqueles=t2.idanaquel SET t1.status ='" + status + "' WHERE t2.nivelfkcniveles='" + idnivelAnterior + "'");
                    

                    var res2 = c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles','" + idnivelAnterior + "','" + msg + "activación de Anaquel','" + idUsuario + "',NOW(),'" + msg + "activación de Anaquel','" + empresa + "','" + area + "')");


                    MessageBox.Show("El Anaquel se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msg = null;
                    status = 0;
                    msg2 = null;
                   
                    limpiar();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) v.mover(sender,e,this);
        }

        private void getCambios(object sender, EventArgs e)
        {
            if (editar) {
                if (_state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim()))) btnsavemp.Visible = lblsavemp.Visible = true; else btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (_state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim())))
            {
                if (MessageBox.Show("Desea Guardar La Información",validaciones.MessageBoxTitle.Confirmar.ToString(),MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                }
            }

                limpiar();
        }

        private void txtnivel_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void dtniveles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (!string.IsNullOrWhiteSpace(idnivelAnterior) && editar && _state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim())))
                {
                    if (MessageBox.Show("Desea Guardar La Información", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
                    }
                }
                guardarReporte(e);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                idnivelAnterior = dtniveles.Rows[e.RowIndex].Cells[0].Value.ToString();
                _state = v.getStatusInt(dtniveles.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar)
                {

                    if (_state == 0)
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelpa.Text = "Reactivar";
                    }
                    else
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelpa.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                if (Peditar)
                {
                     cbpasillo.SelectedValue = idpasilloAnterior = dtniveles.Rows[e.RowIndex].Cells[5].Value.ToString();
                   txtnivel.Text =  NivelAnterior = dtniveles.Rows[e.RowIndex].Cells[2].Value.ToString();
                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    pCancelar.Visible = true;
                    gbaddnivel.Text = "Actualizar Nivel";

                    btnsavemp.Visible = lblsavemp.Visible = false;
                }

                if(!Pdesactivar && !Peditar)
                { 
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar o Desactivar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            string nivel = v.mayusculas(txtnivel.Text.Trim().ToLower());
            if (v.formularioNiveles(pasillo,nivel) && !v.existeNivel(pasillo, nivel))
            {
                if (c.insertar("INSERT INTO cniveles(nivel, pasillofkcpasillos, usuariofkcpersonal) VALUES('"+nivel+"','"+pasillo+"','"+idUsuario+"')"))
                {
                    if (c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES ('Catálogo de Refacciones - Ubicaciones - Niveles',(SELECT idnivel FROM cniveles WHERE pasillofkcpasillos='"+pasillo+"' and nivel='"+nivel+"'),'Inserción de Nivel','"+idUsuario+"',NOW(),'Inserción de Nivel','"+empresa+"','"+area+"')"))
                    {
                        ubicaciones u = (ubicaciones)Owner;
                        u.nivelTemp = v.getaData("SELECT idnivel FROM cniveles WHERE pasillofkcpasillos='" + pasillo + "' and nivel='" + nivel + "'").ToString();
                        MessageBox.Show("El Nivel se Ha Agregado Correctamente",validaciones.MessageBoxTitle.Información.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Information);
                        limpiar();
                    }
                }
            }
        }

        private void catNiveles_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
            {
                v.iniCombos("SELECT idpasillo,UPPER(pasillo) as pasillo FROM cpasillos WHERE status=1", cbpasillo, "idpasillo", "pasillo", "--SELECCIONE UN PASILLO--");
                ubicaciones u = (ubicaciones)Owner;
                if (!string.IsNullOrWhiteSpace(u.pasilloTemp))
                {
                    cbpasillo.SelectedValue = u.pasilloTemp;
                    u.pasilloTemp = null;
                }
            }
            if (Pconsultar) cargarNiveles();
        }
    }
}

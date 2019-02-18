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
    public partial class privilegiosMantenimiento : Form
    {
        validaciones v = new validaciones();
        int idUsuario;
        bool editar=false;
        DataTable t;
        string[] id;
        public privilegiosMantenimiento(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        public privilegiosMantenimiento()
        {
            InitializeComponent();
          
        }
        void buscarNombre()
        {

            lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(apPaterno,' ',apMaterno,' ',nombres) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'");
            lbltitle.Left = (panel1.Width - lbltitle.Size.Width) / 2;
        }
        private void CambiarEstado_Click(object sender, EventArgs e)
        {
            v.CambiarEstado_Click(sender, e);
        }
    
        private void btnconsultarempleado_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempleado.BackgroundImage) != v.check)
            {
                btneliminarempleado.BackgroundImage = btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = false;
            }
            else
            {
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = true;
            }
        }

    
        private void btnconsultarcargo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcargo.BackgroundImage) != v.check)
            {
                btneliminarcargo.BackgroundImage = btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = false;
            }
            else
            {
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = true;
            }
        }
     

        private void btnconsultarunidad_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarunidad.BackgroundImage) != v.check)
            {
                btneliminarunidad.BackgroundImage = btnmodificarunidad.BackgroundImage = Properties.Resources.uncheck;
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = false;
            }
            else
            {
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = true;
            }
        }

        private void btnconsultarfallo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarfallo.BackgroundImage) != v.check)
            {

                btneliminarfallo.BackgroundImage = btnmodificarfallo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarfallo.Enabled = btnmodificarfallo.Enabled = false;
            }else
            {
                btneliminarfallo.Enabled = btnmodificarfallo.Enabled = true;
            }
        }


        private void btnconsultarmante_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarmante.BackgroundImage) != v.check)
            {
                btneliminarmante.BackgroundImage = btnmodificarmante.BackgroundImage = Properties.Resources.uncheck;
                btneliminarmante.Enabled = btnmodificarmante.Enabled = false;
            }
            else
            {
                btneliminarmante.Enabled = btnmodificarmante.Enabled = true;
            }
        }
       

        private void button33_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panel2.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.BackgroundImage = controlFallos.Properties.Resources.uncheck;
                }
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string[,] privilegios = new string[6,6];
            string[,] respaldo = new string[6, 5];
            respaldo[0, 0] = privilegios[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarfallo.BackgroundImage) == v.check || v.ImageToString(btnconsultarfallo.BackgroundImage) == v.check || v.ImageToString(btnmodificarfallo.BackgroundImage) == v.check || v.ImageToString(btneliminarfallo.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = privilegios[0, 1] = v.Checked(btninsertarfallo.BackgroundImage).ToString();
            respaldo[0, 2] = privilegios[0, 2] = v.Checked(btnconsultarfallo.BackgroundImage).ToString();
            respaldo[0, 3] = privilegios[0, 3] = v.Checked(btnmodificarfallo.BackgroundImage).ToString();
            respaldo[0, 4] = privilegios[0, 4] = v.Checked(btneliminarfallo.BackgroundImage).ToString();
            privilegios[0, 5] = "catfallosGrales";
            respaldo[1, 0] = privilegios[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = privilegios[1, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
            respaldo[1, 2] = privilegios[1, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
            respaldo[1, 3] = privilegios[1, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
            respaldo[1, 4] = privilegios[1, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
            privilegios[1, 5] = "catPersonal";
            respaldo[2, 0] = privilegios[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = privilegios[2, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
            respaldo[2, 2] = privilegios[2, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
            respaldo[2, 3] = privilegios[2, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
            respaldo[2, 4] = privilegios[2, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
            privilegios[2, 5] = "catPuestos";
            respaldo[3, 0] = privilegios[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = privilegios[3, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
            respaldo[3, 2] = privilegios[3, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
            respaldo[3, 3] = privilegios[3, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
            respaldo[3, 4] = privilegios[3, 4] = v.Checked(btneliminarunidad.BackgroundImage).ToString();
            privilegios[3, 5] = "catUnidades";
            respaldo[4, 0] = privilegios[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarmante.BackgroundImage) == v.check || v.ImageToString(btnconsultarmante.BackgroundImage) == v.check || v.ImageToString(btnmodificarmante.BackgroundImage) == v.check || v.ImageToString(btneliminarmante.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = privilegios[4, 1] = v.Checked(btninsertarmante.BackgroundImage).ToString();
            respaldo[4, 2] = privilegios[4, 2] = v.Checked(btnconsultarmante.BackgroundImage).ToString();
            respaldo[4, 3] = privilegios[4, 3] = v.Checked(btnmodificarmante.BackgroundImage).ToString();
            respaldo[4, 4] = privilegios[4, 4] = v.Checked(btneliminarmante.BackgroundImage).ToString();
            privilegios[4, 5] = "Mantenimiento";
            respaldo[5, 0] = privilegios[5, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check ).ToString();
            respaldo[5, 1] = privilegios[5, 1] = "0";
            respaldo[5, 2] = privilegios[5, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[5, 3] = privilegios[5, 3] = "0";
            respaldo[5, 4] = privilegios[5, 4] = "0";
            privilegios[5, 5] = "historial";
            if (!v.todosFalsos(respaldo))
            {
                if (!editar)
            {

                    if (idUsuario>0) {
                        for (int i = 0; i < 6; i++)
                        {
                            string ver = privilegios[i, 0];
                            string insertar = privilegios[i, 1];
                            string consultar = privilegios[i, 2];
                            string modificar = privilegios[i, 3];
                            string eliminar = privilegios[i, 4];
                            string nombre = privilegios[i, 5];
                            v.insert(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
                        }

                        MessageBox.Show("Se Han Asignado los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }else
                    {
                        catPersonal cat = (catPersonal)Owner;
                        cat.privilegios = privilegios;
                    }
            }
            else
            {
                if (sehicieronModificaciones(t, respaldo))
                {
                    for (int i = 0; i < privilegios.GetLength(0); i++)
                    {
                        string ver = privilegios[i, 0];
                        string insertar = privilegios[i, 1];
                        string consultar = privilegios[i, 2];
                        string modificar = privilegios[i, 3];
                        string eliminar = privilegios[i, 4];
                        string nombre = privilegios[i, 5];
                        v.edit(id[i], ver, insertar, consultar, modificar, eliminar);
                    }
                    MessageBox.Show("Se Han Actualizado los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else
                {
                    MessageBox.Show("No se Realizaron Modificaciones", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                }
            }
            
         else
                {
                if (!editar) {
                    catPersonal Cat = (catPersonal)Owner;
        Cat.privilegios = null;

                }else
                {
                    v.EliminarPrivilegios(idUsuario);
                    MessageBox.Show("Se Han Eliminado Los Privilegios",validaciones.MessageBoxTitle.Advertencia.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
        }
        bool sehicieronModificaciones(DataTable a, string[,] b)
        {
            bool res = false;
            a.Columns.RemoveAt(0);
            for (int i = 0; i <6 ; i++)
            {
                object[] c = a.Rows[i].ItemArray;
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j].ToString() != b[i, j])
                    {
                        res = true;
                    }
                }
            }

            return res;
        }
        private void privilegiosMantenimiento_Load(object sender, EventArgs e)
        {
            buscarNombre();
            exitenPrivilegios();
        }
        public void insertarPrivilegios(string[,] privilegios)
        {
                            btninsertarfallo.BackgroundImage = v.Checked(privilegios[0,1]);
            btnconsultarfallo.BackgroundImage = v.Checked(privilegios[0,2]);
            btnmodificarfallo.BackgroundImage = v.Checked(privilegios[0,3]);
            btneliminarfallo.BackgroundImage = v.Checked(privilegios[0,4]);
            btninsertarempleado.BackgroundImage = v.Checked(privilegios[1,1]);
            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[1,2]);
            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[1,3]);
            btneliminarempleado.BackgroundImage = v.Checked(privilegios[1,4]);
            btninsertarcargo.BackgroundImage = v.Checked(privilegios[2,1]);
            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[2,2]);
            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[2,3]);
            btneliminarcargo.BackgroundImage = v.Checked(privilegios[2,4]);
            btninsertarunidad.BackgroundImage = v.Checked(privilegios[3,1]);
            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[3,2]);
            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[3,3]);
            btneliminarunidad.BackgroundImage = v.Checked(privilegios[3,4]);
            btninsertarmante.BackgroundImage = v.Checked(privilegios[4,1]);
            btnconsultarmante.BackgroundImage = v.Checked(privilegios[4,2]);
            btnmodificarmante.BackgroundImage = v.Checked(privilegios[4,3]);
            btneliminarmante.BackgroundImage = v.Checked(privilegios[4,4]);
            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[5,2]);
        }
        void exitenPrivilegios()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'")) > 0)
            {
                lbltexto.Text = "Actualizar Privilegios";
                t = (DataTable)v.getData("SELECT namform,ver,insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'");
                id = v.getaData("SELECT group_concat(idprivilegio separator ';') from privilegios WHERE usuariofkcpersonal='" + idUsuario + "';").ToString().Split(';');
                editar = true;
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    object[] privilegios = t.Rows[i].ItemArray;
                    switch (privilegios[0].ToString())
                    {
                        case "catfallosGrales":
                           
                            btninsertarfallo.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarfallo.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarfallo.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarfallo.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catPersonal":
                      
                            btninsertarempleado.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarempleado.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catPuestos":
                            
                            btninsertarcargo.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarcargo.BackgroundImage = v.Checked(privilegios[5]);
                            break;               
                        case "catUnidades":
                            
                            btninsertarunidad.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarunidad.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "Mantenimiento":
            
                            btninsertarmante.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarmante.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarmante.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarmante.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "historial":
                       
                            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[3]);
                           
                            break;
                    }
                }
            }
        }
        private void btnconsultarhistorial_BackgroundImageChanged(object sender, EventArgs e)
        {
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
       
                string[,] respaldo = new string[6, 5];
                respaldo[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarfallo.BackgroundImage) == v.check || v.ImageToString(btnconsultarfallo.BackgroundImage) == v.check || v.ImageToString(btnmodificarfallo.BackgroundImage) == v.check || v.ImageToString(btneliminarfallo.BackgroundImage) == v.check)).ToString();
                respaldo[0, 1] = v.Checked(btninsertarfallo.BackgroundImage).ToString();
                respaldo[0, 2] = v.Checked(btnconsultarfallo.BackgroundImage).ToString();
                respaldo[0, 3] = v.Checked(btnmodificarfallo.BackgroundImage).ToString();
                respaldo[0, 4] = v.Checked(btneliminarfallo.BackgroundImage).ToString();
                respaldo[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
                respaldo[1, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
                respaldo[1, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
                respaldo[1, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
                respaldo[1, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
                respaldo[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
                respaldo[2, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
                respaldo[2, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
                respaldo[2, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
                respaldo[2, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();

                respaldo[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
                respaldo[3, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
                respaldo[3, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
                respaldo[3, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
                respaldo[3, 4] = v.Checked(btneliminarunidad.BackgroundImage).ToString();
                respaldo[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarmante.BackgroundImage) == v.check || v.ImageToString(btnconsultarmante.BackgroundImage) == v.check || v.ImageToString(btnmodificarmante.BackgroundImage) == v.check || v.ImageToString(btneliminarmante.BackgroundImage) == v.check)).ToString();
                respaldo[4, 1] = v.Checked(btninsertarmante.BackgroundImage).ToString();
                respaldo[4, 2] = v.Checked(btnconsultarmante.BackgroundImage).ToString();
                respaldo[4, 3] = v.Checked(btnmodificarmante.BackgroundImage).ToString();
                respaldo[4, 4] = v.Checked(btneliminarmante.BackgroundImage).ToString();
                respaldo[5, 0] = v.getIntFrombool((v.ImageToString(btnconsultarhistorial.BackgroundImage)== v.check)).ToString();
                respaldo[5, 1] = "0";
                respaldo[5, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
                respaldo[5, 3] = "0";
                respaldo[5, 4] = "0";
            if (editar)
            {
                if (sehicieronModificaciones(t, respaldo))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
            }else
            {
                if (v.seDetectaronModificaciones(respaldo))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
            }
        }
    }
}

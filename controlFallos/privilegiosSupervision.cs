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
    public partial class privilegiosSupervision : Form
    {
        int idUsuario;
        bool editar = false;
        DataTable t;
        conexion c = new conexion();
        validaciones v = new validaciones();
        string[] id;
        void buscarNombre()
        {
           
                lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'");
            lbltitle.Left = (panel1.Width - lbltitle.Size.Width) / 2;
        }
        public privilegiosSupervision(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        public privilegiosSupervision()
        {
            InitializeComponent();
           
        }
        private void CambiarEstado_Click(object sender, EventArgs e)
        {
            v.CambiarEstado_Click(sender, e);
    
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

        private void privilegiosSupervision_Load(object sender, EventArgs e)
        {
            buscarNombre();
            exitenPrivilegios();
        }

      

        private void btnconsultararea_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultararea.BackgroundImage) != v.check)
            { 
                btneliminararea.Enabled = btnmodificararea.Enabled = false;
                btneliminararea.BackgroundImage = btnmodificararea.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {            
                btneliminararea.Enabled = btnmodificararea.Enabled = true;
            }
        }

      

        private void btnconsultarempresa_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempresa.BackgroundImage) != v.check)
            {        
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = false;
                btneliminarempresa.BackgroundImage = btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {        
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = true;
            }
        }

      

        private void btnconsultarempleado_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempleado.BackgroundImage) != v.check)
            { 
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = false;
                btneliminarempleado.BackgroundImage = btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = true;
            }
        }

     

        private void btnconsultarcargo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcargo.BackgroundImage)!=v.check)
            {
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = false;
                btneliminarcargo.BackgroundImage = btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = true;
            }
        }

       
        private void btnconsultarservicio_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarservicio.BackgroundImage) != v.check)
            {
                btneliminarservicio.Enabled = btnmodificarservicio.Enabled = false;
                btneliminarservicio.BackgroundImage = btnmodificarservicio.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {
                btneliminarservicio.Enabled = btnmodificarservicio.Enabled = true;
            }
        }

        private void btnconsultarunidad_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarunidad.BackgroundImage) != v.check)
            {
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = false;
                btneliminarunidad.BackgroundImage = btnmodificarunidad.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = true;
            }
        }

       

        private void btnconsultarsuper_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarsuper.BackgroundImage) != v.check)
            {
                btneliminarsuper.Enabled = btnmodificarsuper.Enabled = false;
                btneliminarsuper.BackgroundImage = btnmodificarsuper.BackgroundImage = Properties.Resources.uncheck;
            }
            else { 
                btneliminarsuper.Enabled = btnmodificarsuper.Enabled = true;
            }
        }



        private void button32_Click(object sender, EventArgs e)
        {
            string[,] privilegios = new string[8, 6];
            string[,] respaldo = new string[8, 5];
            respaldo[0, 0] = privilegios[0, 0] = v.getIntFrombool((v.ImageToString(btninsertararea.BackgroundImage) == v.check || v.ImageToString(btnconsultararea.BackgroundImage) == v.check || v.ImageToString(btnmodificararea.BackgroundImage) == v.check || v.ImageToString(btneliminararea.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = privilegios[0, 1] = v.Checked(btninsertararea.BackgroundImage).ToString();
            respaldo[0, 2] = privilegios[0, 2] = v.Checked(btnconsultararea.BackgroundImage).ToString();
            respaldo[0, 3] = privilegios[0, 3] = v.Checked(btnmodificararea.BackgroundImage).ToString();
            respaldo[0, 4] = privilegios[0, 4] = v.Checked(btneliminararea.BackgroundImage).ToString();
            privilegios[0, 5] = "catAreas";
            respaldo[1, 0] = privilegios[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = privilegios[1, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString();
            respaldo[1, 2] = privilegios[1, 2] = v.Checked(btnconsultarempresa.BackgroundImage).ToString();
            respaldo[1, 3] = privilegios[1, 3] = v.Checked(btnmodificarempresa.BackgroundImage).ToString();
            respaldo[1, 4] = privilegios[1, 4] = v.Checked(btneliminarempresa.BackgroundImage).ToString();
            privilegios[1, 5] = "catEmpresas";
            respaldo[2, 0] = privilegios[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = privilegios[2, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
            respaldo[2, 2] = privilegios[2, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
            respaldo[2, 3] = privilegios[2, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
            respaldo[2, 4] = privilegios[2, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
            privilegios[2, 5] = "catPersonal";
            respaldo[3, 0] = privilegios[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = privilegios[3, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
            respaldo[3, 2] = privilegios[3, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
            respaldo[3, 3] = privilegios[3, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
            respaldo[3, 4] = privilegios[3, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
            privilegios[3, 5] = "catPuestos";
            respaldo[4, 0] = privilegios[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarservicio.BackgroundImage) == v.check || v.ImageToString(btnconsultarservicio.BackgroundImage) == v.check || v.ImageToString(btnmodificarservicio.BackgroundImage) == v.check || v.ImageToString(btneliminarservicio.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = privilegios[4, 1] = v.Checked(btninsertarservicio.BackgroundImage).ToString();
            respaldo[4, 2] = privilegios[4, 2] = v.Checked(btnconsultarservicio.BackgroundImage).ToString();
            respaldo[4, 3] = privilegios[4, 3] = v.Checked(btnmodificarservicio.BackgroundImage).ToString();
            respaldo[4, 4] = privilegios[4, 4] = v.Checked(btneliminarservicio.BackgroundImage).ToString();
            privilegios[4, 5] = "catServicios";
            respaldo[5, 0] = privilegios[5, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = privilegios[5, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
            respaldo[5, 2] = privilegios[5, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
            respaldo[5, 3] = privilegios[5, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
            respaldo[5, 4] = privilegios[5, 4] = v.Checked(btneliminarunidad.BackgroundImage).ToString();
            privilegios[5, 5] = "catUnidades";
            respaldo[6, 0] = privilegios[6, 0] = v.getIntFrombool((v.ImageToString(btninsertarsuper.BackgroundImage) == v.check || v.ImageToString(btnconsultarsuper.BackgroundImage) == v.check || v.ImageToString(btnmodificarsuper.BackgroundImage) == v.check || v.ImageToString(btneliminarsuper.BackgroundImage) == v.check)).ToString();
            respaldo[6, 1] = privilegios[6, 1] = v.Checked(btninsertarsuper.BackgroundImage).ToString();
            respaldo[6, 2] = privilegios[6, 2] = v.Checked(btnconsultarsuper.BackgroundImage).ToString();
            respaldo[6, 3] = privilegios[6, 3] = v.Checked(btnmodificarsuper.BackgroundImage).ToString();
            respaldo[6, 4] = privilegios[6, 4] = v.Checked(btneliminarsuper.BackgroundImage).ToString();
            privilegios[6, 5] = "Form1";
            respaldo[7, 0] = privilegios[7, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check).ToString();
            respaldo[7, 1] = privilegios[7, 1] = "0";
            respaldo[7, 2] = privilegios[7, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[7, 3] = privilegios[7, 3] = "0";
            respaldo[7, 4] = privilegios[7, 4] = "0";
            privilegios[7, 5] = "historial";
            string mensaje = "";
            if (!v.todosFalsos(respaldo))
            {

                if (!editar)
                {


                    if (idUsuario > 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            string ver = privilegios[i, 0];
                            string insertar = privilegios[i, 1];
                            string consultar = privilegios[i, 2];
                            string modificar = privilegios[i, 3];
                            string eliminar = privilegios[i, 4];
                            string nombre = privilegios[i, 5];
                            v.insert(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
                        }
                        mensaje = "Asignado";
                        MessageBox.Show("Se Han " + mensaje + " los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        catPersonal Cat = (catPersonal)Owner;
                        Cat.privilegios = privilegios;
                    }
                   
                }
                else
                {
                    if (sehicieronModificaciones(t, respaldo))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            string ver = privilegios[i, 0];
                            string insertar = privilegios[i, 1];
                            string consultar = privilegios[i, 2];
                            string modificar = privilegios[i, 3];
                            string eliminar = privilegios[i, 4];
                            string nombre = privilegios[i, 5];
                            v.edit(id[i], ver, insertar, consultar, modificar, eliminar);



                        }
                        mensaje = "Actualizado";
                        MessageBox.Show("Se Han " + mensaje + " los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    else
                    {
                        MessageBox.Show("No se Realizaron Modificaciones", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
            else
            {
                if (!editar)
                {
                    if (idUsuario>0) {
                        catPersonal Cat = (catPersonal)Owner;
                        Cat.privilegios = null;
                    }
                    }
                else
                {
                    v.EliminarPrivilegios(idUsuario);
                    MessageBox.Show("Se Han Eliminado Los Privilegios", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        bool sehicieronModificaciones(DataTable a, string[,] b)
        {
            bool res = false;
            a.Columns.RemoveAt(0);
            for (int i = 0; i < 8; i++)
            {
             object[] c = a.Rows[i].ItemArray;
                for (int j=0; j<c.Length; j++)
                {
                    if (c[j].ToString()!=b[i,j])
                    {
                        res = true;
                    }
                }
            }
            return res;
        }
       public void insertarPrivilegios(string[,] privilegios)
        {
            btninsertararea.BackgroundImage = v.Checked(privilegios[0,1]);
            btnconsultararea.BackgroundImage = v.Checked(privilegios[0, 2]);
            btnmodificararea.BackgroundImage = v.Checked(privilegios[0, 3]);
            btneliminararea.BackgroundImage = v.Checked(privilegios[0, 4]);
            btninsertarempresa.BackgroundImage = v.Checked(privilegios[1,1]);
            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[1,2]);
            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[1,3]);
            btneliminarempresa.BackgroundImage = v.Checked(privilegios[1,4]);
            btninsertarempleado.BackgroundImage = v.Checked(privilegios[2,1]);
            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[2,2]);
            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[2,3]);
            btneliminarempleado.BackgroundImage = v.Checked(privilegios[2,4]);
            btninsertarcargo.BackgroundImage = v.Checked(privilegios[3,1]);
            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[3,2]);
            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[3,3]);
            btneliminarcargo.BackgroundImage = v.Checked(privilegios[3,4]);
            btninsertarservicio.BackgroundImage = v.Checked(privilegios[4,1]);
            btnconsultarservicio.BackgroundImage = v.Checked(privilegios[4,2]);
            btnmodificarservicio.BackgroundImage = v.Checked(privilegios[4,3]);
            btneliminarservicio.BackgroundImage = v.Checked(privilegios[4,4]);
            btninsertarunidad.BackgroundImage = v.Checked(privilegios[5,1]);
            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[5,2]);
            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[5,3]);
            btneliminarunidad.BackgroundImage = v.Checked(privilegios[5,4]);
            btninsertarsuper.BackgroundImage = v.Checked(privilegios[6,1]);
            btnconsultarsuper.BackgroundImage = v.Checked(privilegios[6,2]);
            btnmodificarsuper.BackgroundImage = v.Checked(privilegios[6,3]);
            btneliminarsuper.BackgroundImage = v.Checked(privilegios[6,4]);
            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[7,2]);

        }
        void exitenPrivilegios()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'")) > 0)
            {
                lbltexto.Text = "Actualizar Privilegios";
              t = (DataTable)v.getData("SELECT namform, ver,insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'");
                id = v.getaData("SELECT group_concat(idprivilegio separator ';') from privilegios WHERE usuariofkcpersonal='"+idUsuario+"';").ToString().Split(';');
                editar = true;
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    object[] privilegios = t.Rows[i].ItemArray;
                    switch (privilegios[0].ToString())
                    {
                        case "catAreas":
                          
                            btninsertararea.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultararea.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificararea.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminararea.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catEmpresas":
                          
                            btninsertarempresa.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarempresa.BackgroundImage = v.Checked(privilegios[5]);
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
                        case "catServicios":
                    
                            btninsertarservicio.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarservicio.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarservicio.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarservicio.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catUnidades":
                     
                            btninsertarunidad.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarunidad.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "Form1":
          
                            btninsertarsuper.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarsuper.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarsuper.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarsuper.BackgroundImage = v.Checked(privilegios[5]);
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
         
                string[,] respaldo = new string[8, 5];
                respaldo[0, 0] = v.getIntFrombool((v.ImageToString(btninsertararea.BackgroundImage) == v.check || v.ImageToString(btnconsultararea.BackgroundImage) == v.check || v.ImageToString(btnmodificararea.BackgroundImage) == v.check || v.ImageToString(btneliminararea.BackgroundImage) == v.check)).ToString();
                respaldo[0, 1] = v.Checked(btninsertararea.BackgroundImage).ToString();
                respaldo[0, 2] = v.Checked(btnconsultararea.BackgroundImage).ToString();
                respaldo[0, 3] = v.Checked(btnmodificararea.BackgroundImage).ToString();
                respaldo[0, 4] = v.Checked(btneliminararea.BackgroundImage).ToString();
                respaldo[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
                respaldo[1, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString();
                respaldo[1, 2] = v.Checked(btnconsultarempresa.BackgroundImage).ToString();
                respaldo[1, 3] = v.Checked(btnmodificarempresa.BackgroundImage).ToString();
                respaldo[1, 4] = v.Checked(btneliminarempresa.BackgroundImage).ToString();
                respaldo[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
                respaldo[2, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
                respaldo[2, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
                respaldo[2, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
                respaldo[2, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
                respaldo[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
                respaldo[3, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
                respaldo[3, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
                respaldo[3, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
                respaldo[3, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
                respaldo[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarservicio.BackgroundImage) == v.check || v.ImageToString(btnconsultarservicio.BackgroundImage) == v.check || v.ImageToString(btnmodificarservicio.BackgroundImage) == v.check || v.ImageToString(btneliminarservicio.BackgroundImage) == v.check)).ToString();
                respaldo[4, 1] = v.Checked(btninsertarservicio.BackgroundImage).ToString();
                respaldo[4, 2] = v.Checked(btnconsultarservicio.BackgroundImage).ToString();
                respaldo[4, 3] = v.Checked(btnmodificarservicio.BackgroundImage).ToString();
                respaldo[4, 4] = v.Checked(btneliminarservicio.BackgroundImage).ToString();
                respaldo[5, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
                respaldo[5, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
                respaldo[5, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
                respaldo[5, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
                respaldo[5, 4] = v.Checked(btneliminarunidad.BackgroundImage).ToString();
                respaldo[6, 0] = v.getIntFrombool((v.ImageToString(btninsertarsuper.BackgroundImage) == v.check || v.ImageToString(btnconsultarsuper.BackgroundImage) == v.check || v.ImageToString(btnmodificarsuper.BackgroundImage) == v.check || v.ImageToString(btneliminarsuper.BackgroundImage) == v.check)).ToString();
                respaldo[6, 1] = v.Checked(btninsertarsuper.BackgroundImage).ToString();
                respaldo[6, 2] = v.Checked(btnconsultarsuper.BackgroundImage).ToString();
                respaldo[6, 3] = v.Checked(btnmodificarsuper.BackgroundImage).ToString();
                respaldo[6, 4] = v.Checked(btneliminarsuper.BackgroundImage).ToString();

                respaldo[7, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check).ToString();
                respaldo[7, 1] = "0";
                respaldo[7, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[7, 3] = "0";
                respaldo[7, 4] = "0";
            if (editar)
            {
                if (sehicieronModificaciones(t, respaldo))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
            }
            else
            {
                if (!v.todosFalsos(respaldo))
                {
                  
                        button32_Click(sender, e);
                    
                }
            }
        
        }
    }
}

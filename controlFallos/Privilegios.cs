using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class Privilegios : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario;
        int empresa,area;
        int total = 0;
        bool editar = false;
        public Privilegios(int idUsuario, int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            rellenar();
            yaExiste();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Dispose();
            Close();

        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int Wmsg, int Param, int IParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public void rellenar()
        {
            if (this.empresa == 1)
            {
                tbprivilegios.Rows.Add("1", "Catálogo de Empleados", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                tbprivilegios.Rows.Add("2", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                tbprivilegios.Rows.Add("3", "Catálogo de Unidades", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catUnidades");
                tbprivilegios.Rows.Add("4", "Catálogo de Servicios", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catServicios");
                tbprivilegios.Rows.Add("5", "Catálogo de Empresas", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catEmpresas");
                tbprivilegios.Rows.Add("6", "Reporte Supervisión", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Form1");
                total = 6;
                
            }
            else if (this.empresa == 2)
            {
                if (area==1)
                {
                    tbprivilegios.Rows.Add("1", "Catálogo de Fallos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catfallosGrales");
                    tbprivilegios.Rows.Add("2", "Catálogo de Empleados", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                    tbprivilegios.Rows.Add("3", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                    tbprivilegios.Rows.Add("4", "Catálogo de Unidades", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catUnidades");
                    tbprivilegios.Rows.Add("5", "Catálogo de Refacciones", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catRefacciones");
                    tbprivilegios.Rows.Add("6", "Reporte Mantenimiento", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Mantenimiento");
                    total = 6;
                }else if (area==2)
                {
                    tbprivilegios.Rows.Add("1", "Catálogo de Empleados", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                    tbprivilegios.Rows.Add("2", "Catálogo de Proveedores", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catProveedores");
                    tbprivilegios.Rows.Add("3", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                    tbprivilegios.Rows.Add("4", "Catálogo de Refacciones", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catRefacciones");
                    tbprivilegios.Rows.Add("5", "Generación de Requisiciones", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "ordencompra");
                    tbprivilegios.Rows.Add("6", "Reporte Almacén", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Almacen");
                    total = 6;
                }

               
                
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < total; x++)
            {

                for (int y = 2; y < 7; y++)
                {

                    tbprivilegios.Rows[x].Cells[y].Value = false.ToString();

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                insertar();
            }
            else
            {
                editarprivilegios();
            }
            tbprivilegios.ClearSelection();
            yaExiste();
            System.Threading.Thread.Sleep(2000);
            this.Close();
        }
        public void insertar()
        {
            string[,] privilegios = new string[total, 8];
            for (int x = 0; x < total; x++)
            {
                for (int y = 2; y < 8; y++)
                {
                    if (y == 2)
                    {
                        bool res = Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value);
                        if (!res)
                        {
                            for (int y1 = 3; y1 < 7; y1++)
                            {

                                tbprivilegios.Rows[x].Cells[y1].Value = false.ToString();

                            }
                        }

                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[3].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[4].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[5].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[6].Value))
                        {

                            tbprivilegios.Rows[x].Cells[y].Value = false;
                        }
                    }
                    if (y == 4)
                    {
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y + 1].Value = false.ToString();
                            tbprivilegios.Rows[x].Cells[y + 2].Value = false.ToString();
                        }
                    }

                    privilegios[x, y] = tbprivilegios.Rows[x].Cells[y].Value.ToString();


                }
            }
            try
            {
                for (int y = 0; y < total; y++)
                {
                    conexion c = new conexion();

                    string _ver = privilegios[y, 2].ToLower();
                    string _insertar = privilegios[y, 3].ToLower();
                    string _consultar = privilegios[y, 4].ToLower();
                    string _editar = privilegios[y, 5].ToLower();
                    string _desactivar = privilegios[y, 6].ToLower();
                    string _pClave = privilegios[y, 7];
                    String sql = @"INSERT INTO privilegios (usuariofkcpersonal,namform,ver,insertar,consultar,editar,desactivar) VALUES('";
                    sql += this.idUsuario + "','";
                    sql += _pClave + "','";
                    sql += v.getIntFrombool(Convert.ToBoolean(_ver)) + "',";
                    sql += "'" + v.getIntFrombool(Convert.ToBoolean(_insertar)) + "','";
                    sql += v.getIntFrombool(Convert.ToBoolean(_consultar)) + "','";
                    sql += v.getIntFrombool(Convert.ToBoolean(_editar));
                    sql += "','" + v.getIntFrombool(Convert.ToBoolean(_desactivar)) + "')";

                    bool res = c.insertar(sql);

                }
                MessageBox.Show("Se han Asignado los privilegios Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void editarprivilegios()
        {
            string[,] privilegios = new string[total, 8];
            for (int x = 0; x < total; x++)
            {

                for (int y = 0; y < 8; y++)
                {
                    if (y == 2)
                    {
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            for (int y1 = 3; y1 < 7; y1++)
                            {

                                tbprivilegios.Rows[x].Cells[y1].Value = false.ToString();

                            }
                        }
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[3].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[4].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[5].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[6].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y].Value = false;
                        }
                    }
                    if (y == 4)
                    {
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y + 1].Value = false.ToString();
                            tbprivilegios.Rows[x].Cells[y + 2].Value = false.ToString();
                        }
                    }

                    privilegios[x, y] = tbprivilegios.Rows[x].Cells[y].Value.ToString();


                }
            }
            try
            {
                for (int y = 0; y < total; y++)
                {
                    conexion c = new conexion();
                    String sql = @"UPDATE privilegios SET ver ='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 2])) + "',insertar ='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 3])) +
                                 "',consultar='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 4])) + "',editar='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 5])) + "',desactivar ='" +
                                 v.getIntFrombool(Convert.ToBoolean(privilegios[y, 6])) + "' WHERE idprivilegio = " + privilegios[y, 0] + " and usuariofkcpersonal = " + this.idUsuario;
                    c.insertar(sql);

                }

                MessageBox.Show("Se han Actualizado los Privilegios Correctamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void tbprivilegios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int row = e.RowIndex;
            if (e.ColumnIndex == 2)
            {

                for (int i = 2; i < 7; i++)
                {
                    if (Convert.ToBoolean(tbprivilegios.Rows[row].Cells[i].Value))
                    {
                        tbprivilegios.Rows[row].Cells[i].Value = false;
                    }
                    else
                    {
                        tbprivilegios.Rows[row].Cells[i].Value = true;
                    }

                }
            }
        }

        public void yaExiste()
        {
            String sql = "SELECT count(idprivilegio) as cuenta FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            if (dr.GetInt32("cuenta") > 0)
            {
                editar = true;
                int contadorIndice = 0;
                string con = "SELECT idprivilegio,ver,insertar,consultar,editar,desactivar FROM sistrefaccmant.privilegios WHERE usuariofkcpersonal='" + idUsuario + "' and (namform ='catfallosGrales' OR namform ='catPersonal' OR namform ='catpuestos' OR namform ='catUnidades' OR namform ='catServicios' OR namform ='catEmpresas' OR namform ='Form1' OR namform='catRefacciones' OR namform ='ordencompra' OR namform ='Mantenimiento'  OR namform ='Almacen'  ) ORDER BY idprivilegio ASC";
                MySqlCommand mc = new MySqlCommand(con, c.dbconection());
                MySqlDataReader mdr = mc.ExecuteReader();
                while (mdr.Read())
                {
                    tbprivilegios.Rows[contadorIndice].Cells[0].Value = mdr.GetInt32("idprivilegio").ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[2].Value = v.getBoolFromInt(mdr.GetInt32("ver")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[3].Value = v.getBoolFromInt(mdr.GetInt32("insertar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[4].Value = v.getBoolFromInt(mdr.GetInt32("consultar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[5].Value = v.getBoolFromInt(mdr.GetInt32("editar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[6].Value = v.getBoolFromInt(mdr.GetInt32("desactivar")).ToString();
                    contadorIndice++;


                }
            }
            else
            {
                c.dbcon.Close();
            }
        }
    }
}

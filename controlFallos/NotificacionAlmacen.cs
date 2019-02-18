using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class NotificacionAlmacen : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        bool res = true;
        Thread t; ThreadStart tS;
        public NotificacionAlmacen()
        {
            InitializeComponent();
        }
       
        void busqnotificacionesFolio()
        {
            tbnotiffolios.Rows.Clear();
            string sql = "SELECT t2.idReporteSupervicion as id, t2.Folio as folio,concat(t5.identificador,LPAD(t3.consecutivo,4,'0')) as eco, CONCAT(T4.nombres,' ',T4.ApPaterno) as mecanico,t1.FechaReporteM as fechas,(SELECT count(idPedRef) FROM pedidosrefaccion WHERE FolioPedfkSupervicion= t2.idReporteSupervicion) as total FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion= t2.idReporteSupervicion INNER JOIN cunidades as t3 ON t2.UnidadfkCUnidades = t3.idunidad INNER JOIN cpersonal AS t4 ON t1.MecanicofkPersonal = t4.idPersona INNER JOIN careas as t5 ON t3.areafkcareas=t5.idarea  WHERE StatusRefacciones = 'Se Requieren Refacciones' and t1.seenAlmacen=0 AND t2.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string[] filas = { dr.GetString("id"), dr.GetString("folio"), dr.GetString("eco"), dr.GetString("mecanico"),DateTime.Parse(dr.GetString("fechas")).ToLongDateString(), dr.GetString("total") };
                tbnotiffolios.Rows.Add(filas);
            }
            dr.Close();
            c.dbconection().Close();
            tbnotiffolios.ClearSelection();
        }
         void busqnotificacionesAlertas()
        {
            tbnotifrefacc.Rows.Clear();
            string sql = "SELECT t1.idrefaccion,t1.codrefaccion,t1.nombreRefaccion,t1.modeloRefaccion,T1.proximoAbastecimiento, CONCAT(t1.existencias,' ',t2.Simbolo) AS existencias, t1.existencias as exist, t1.media AS media,t1.abastecimiento AS abastecimiento,datediff(t1.proximoAbastecimiento,curdate()) as dif FROM crefacciones as t1 INNER JOIN cunidadmedida as t2 ON t1.umfkcunidadmedida=t2.idunidadmedida WHERE t1.existencias <=t1.media OR t1.existencias<= t1.abastecimiento OR datediff(t1.proximoAbastecimiento,curdate()) <=20 and t1.status=1";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string[] filas = { dr.GetString("idrefaccion"), dr.GetString("codrefaccion"), dr.GetString("nombreRefaccion"), dr.GetString("modeloRefaccion"), DateTime.Parse(dr.GetString("proximoAbastecimiento")).ToLongDateString(), dr.GetString("existencias"),dr.GetString("exist"), dr.GetString("media"), dr.GetString("abastecimiento"),dr.GetString("dif") };
                tbnotifrefacc.Rows.Add(filas);
            }
            dr.Close();
            c.dbconection().Close();
            tbnotifrefacc.ClearSelection();
            colorearCeldas();
    }

        private void NotificacionAlmacen_Load(object sender, EventArgs e)
        {
            busqnotificacionesFolio();
            busqnotificacionesAlertas();
            tbnotifrefacc.ClearSelection();
            tS = new ThreadStart(notif);
            t = new Thread(tS);
            t.Start();
        }
        void notif()
        {
            while (res)
            {
                muestra();
                Thread.Sleep(500);
            }
        }
        delegate void internotif();
        void muestra()
        {
            if (InvokeRequired)
            {
                internotif t = new internotif(muestra);
                Invoke(t);

            }
            else
            {
                string[] arreglo = c.conex().Split(';');
                string server = v.Desencriptar(arreglo[0]);
                string user = v.Desencriptar(arreglo[1]);
                string password = v.Desencriptar(arreglo[2]);
                string database = v.Desencriptar(arreglo[3]);
                MySqlConnection dbcon = new MySqlConnection("Server = " + server + "; user=" + user + "; password = " + password + "; database = " + database + ";");
                dbcon.Open();
                MySqlCommand cm = new MySqlCommand("SELECT COUNT(*) FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion= t2.idReporteSupervicion INNER JOIN cunidades as t3 ON t2.UnidadfkCUnidades = t3.idunidad INNER JOIN cpersonal AS t4 ON t1.MecanicofkPersonal = t4.idPersona INNER JOIN careas as t5 ON t3.areafkcareas=t5.idarea  WHERE StatusRefacciones = 'Se Requieren Refacciones' and t1.seenAlmacen=0 AND t2.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();", dbcon);
                var res = cm.ExecuteScalar();
                dbcon.Close();
                if (Convert.ToInt32(res) != tbnotiffolios.Rows.Count)
                {
                    busqnotificacionesFolio();

                }
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void rbreportes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbreportes.Checked)
            {
                gbreportes.Visible = true;
                gbrefacciones.Visible = false;
            }
        }

        private void rbrefacciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rbrefacciones.Checked)
            {
                gbreportes.Visible = false;
                gbrefacciones.Visible = true;
            }
        }
        void colorearCeldas()
        {
            for (int x= 0;x<tbnotifrefacc.Rows.Count;x++)
            {
                if (Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[6].Value)<= Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[7].Value) && Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[6].Value)>= Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[8].Value))
                {
                    tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.Khaki;

                }
                else if (Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[6].Value) <= Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[8].Value))
                {
                    tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.LightCoral;
                }else
                {
                    if (Convert.ToInt32(tbnotifrefacc.Rows[x].Cells[9].Value) >= 10)
                    {
                        tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.Khaki;
                    }else
                    {
                        tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(120, 85, 105);
                        tbnotifrefacc.Rows[x].DefaultCellStyle.ForeColor = Color.FromArgb(200,200,200);
                    }
                }

               
            }
            tbnotifrefacc.ClearSelection();
        }

        private void tbnotifrefacc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbnotifrefacc.ClearSelection();
        }

        private void tbnotifrefacc_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1) {
                string idRefaccion = tbnotifrefacc.Rows[e.RowIndex].Cells[0].Value.ToString();
                menuPrincipal menu = (menuPrincipal)Owner;
                menu.irArefacciones(idRefaccion);
                this.Close();
            }
        }

        private void tbnotiffolios_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1) {
                menuPrincipal m = (menuPrincipal)Owner;
                m.TraerVariable(tbnotiffolios.Rows[e.RowIndex].Cells[0].Value.ToString());
                this.Close();
            }
        }

        private void tbnotifrefacc_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void tbnotiffolios_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void NotificacionAlmacen_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
            res = false;
        }

        private void tbnotifrefacc_ColumnAdded_1(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }
    }
}

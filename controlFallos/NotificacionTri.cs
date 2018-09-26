using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace controlFallos
{
    public partial class NotificacionTri : Form
    {
        conexion c = new conexion();
        int tipoFalla;
        public NotificacionTri(int tipoFalla)
        {
            InitializeComponent();
            this.tipoFalla = tipoFalla;
            busqnotificaciones(consulta(tipoFalla));
            timer1.Start();

        }
        public String consulta(int tipoFalla) {
            string sql = "";
            switch (tipoFalla)
            {
                case 1:
                    sql = "SELECT t3.Folio, t4.ECO, t1.Estatus, concat(t2.nombres,' ', t2.ApPaterno) as 'Nombre de Mecánico', t3.TipoFallo as 'Tipo de Fallo', t3.FechaReporte as 'Fecha de Elaboración', t3.HoraEntrada as 'Hora de Entrada' , t1.HoraTerminoM as 'Hora de Término de Mnatenimiento' FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad WHERE t1.seen = 0 and (t1.Estatus='Liberada' or t1.Estatus='Reprogramada' )";
                    break;
                case 2:
                    sql = "SELECT T1.Folio,t2.ECO,T1.TipoFallo as 'Tipo de Fallo', CONCAT(t1.FechaReporte, ' ',t1.HoraEntrada) as 'Entrada' ,CONCAT(t3.nombres, ' ', t3.ApPaterno) as 'Supervisor que Elaboró', IF(t1.DescFalloNoCod ='' ,(select CONCAT(tab2.falloesp,tab1.DescFalloNoCod) FROM reportesupervicion as tab1 INNER JOIN cfallosesp as tab2 ON tab1.CodFallofkcfallosesp = tab2.idfalloEsp WHERE tab1.idReporteSupervicion = t1.idReporteSupervicion), (SELECT DescFalloNoCod FROM reportesupervicion WHERE idReporteSupervicion=t1.idReporteSupervicion)) as 'Fallo Detectado', t1.ObservacionesSupervision as 'Observaciones del Supervisor' FROM reportesupervicion AS t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cpersonal as t3 ON t1.SupervisorFKCPersonal = t3.idpersona WHERE t1.seen =0";
                    break;
                case 3:
                    sql = "SELECT t2.Folio,t3.ECO, CONCAT(T4.nombres,' ',T4.ApPaterno) as 'Mecanico que Solicita',t1.FechaReporteM as 'Fecha de Solicitud de Refaccion'  FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion= t2.idReporteSupervicion INNER JOIN cunidades as t3 ON t2.UnidadfkCUnidades = t3.idunidad INNER JOIN cpersonal AS t4 ON t1.MecanicofkPersonal = t4.idPersona  WHERE StatusRefacciones = 'Se Requieren Refacciones' and t1.seenAlmacen=0";
                    break;
            }
            return sql;

        }
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            menuPrincipal m = (menuPrincipal)this.Owner;
            string idfolio = tbnotif.Rows[e.RowIndex].Cells[0].Value.ToString();
            m.TraerVariable(idfolio);
            this.Close();

        }
        public void busqnotificaciones(string sql)
        {
                tbnotif.ClearSelection();
            DataSet ds = new DataSet();
            MySqlDataAdapter cargar = new MySqlDataAdapter(sql, c.dbconection());
            cargar.Fill(ds);
            tbnotif.DataSource = ds.Tables[0];
            tbnotif.ClearSelection();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void tbnotif_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbnotif.Columns[e.ColumnIndex].Name == "Tipo de Fallo")
            {
                if (Convert.ToString(e.Value) == "Preventivo")
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "Correctivo")
                    {

                        e.CellStyle.BackColor = Color.Khaki;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "Reiterativo")
                        {

                            e.CellStyle.BackColor = Color.LightCoral;

                        }
                    }
                }
            }
            if (this.tbnotif.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "En Proceso")
                {

                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "Liberada")
                    {

                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "Reprogramada")
                        {

                            e.CellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            busqnotificaciones(consulta(this.tipoFalla));
        }
    }

    }

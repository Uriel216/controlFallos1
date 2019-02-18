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
    public partial class modificaciones : Form
    {
        
        validaciones v = new validaciones();
        int empresa, area;
        public modificaciones(int empresa,int area)
        {
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            cbapartado.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbusuario.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbtipo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }

        void inimodificaciones()
        {
            tbmodif.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SET lc_time_names='es_ES';SELECT DISTINCT t1.idmodificacion, t1.idregistro,(SELECT UPPER(t1.form)) as form,UPPER((SELECT DATE_FORMAT(MAX(fechaHora),'%W, %d de %M del %Y / %H:%i:%s') FROM modificaciones_sistema WHERE idregistro=t1.idregistro and form=t1.form and empresa='" + empresa+"' and area='"+area+"')) as fecha,upper(Tipo) as ultima,UPPER(Concat('Mostrar Más Información')) as m  FROM modificaciones_sistema as t1 WHERE t1.empresa='"+empresa+"' and t1.area='"+area+"' and DATE_FORMAT( t1.fechaHora,'%Y/%m/%d') = DATE_FORMAT( NOW(),'%Y/%m/%d') GROUP BY idregistro,t1.form ORDER BY time((SELECT MAX(fechaHora) FROM modificaciones_sistema WHERE empresa='"+empresa+"' and area='"+area+"' and form= t1.form and idregistro=t1.idregistro)) DESC ;");
            int numFilas = dt.Rows.Count;
            for (int i = 0; i < numFilas; i++)
            {
        
                tbmodif.Rows.Add(dt.Rows[i].ItemArray);
            }
            for (int i= 0; i<numFilas;i++)
            {
                tbmodif.Rows[i].Cells[3].Value = v.mayusculas(tbmodif.Rows[i].Cells[3].Value.ToString());
            }
            tbmodif.ClearSelection();
        }
        void iniusuarios()
        {
            DataTable dr = (DataTable)v.getData("SELECT idpersona,UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) as nombre FROM cpersonal WHERE empresa='"+empresa+"' && area ='"+area+"' and status=1 and(idpersona!=1 and idpersona!=20 and idpersona!=21)");
            DataRow nuevaFila = dr.NewRow();
            nuevaFila["idpersona"] = 0;
            nuevaFila["nombre"] = "--Seleccione un Empleado--".ToUpper();
            dr.Rows.InsertAt(nuevaFila, 0);
            cbusuario.ValueMember = "idpersona";
            cbusuario.DisplayMember = "nombre";
            cbusuario.DataSource = dr;
        }
        private void modificaciones_Load(object sender, EventArgs e)
        {
            inimodificaciones();
            iniusuarios();
            iniapartado();
            iniModificaciones();
            inimeses();
            dtpd.MaxDate = DateTime.Now;
            dtpa.MaxDate = DateTime.Now;
            valoresDTP();
        }
        void valoresDTP()
        {
            dtpd.Value = DateTime.Now.Subtract(TimeSpan.Parse("1"));
            dtpa.Value = dtpa.MaxDate;
        }
        void iniapartado()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["id"] = 0;
            row["Nombre"] = "--Seleccione un Apartado--".ToUpper();
            dt.Rows.Add(row);
      
           if (empresa == 1)
            {
                row = dt.NewRow();
                row["id"] = 1;
                row["Nombre"] = "Catálogo de Areas".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 2;
                row["Nombre"] = "Catálogo de Personal".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 3;
                row["Nombre"] = "Catálogo de Empresas".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 4;
                row["Nombre"] = "Catálogo de Puestos".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 5;
                row["Nombre"] = "Catálogo de Servicios".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 6;
                row["Nombre"] = "Catálogo de Unidades".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 7;
                row["Nombre"] = "Reporte de Supervisión".ToUpper();
                dt.Rows.Add(row);
            }
            else
            {
                if (area == 1)
                {
                    row = dt.NewRow();
                    row["id"] = 1;
                    row["Nombre"] = "Catálogo de Fallos".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 2;
                    row["Nombre"] = "Catálogo de Personal".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 3;
                    row["Nombre"] = "Catálogo de Puestos".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 4;
                    row["Nombre"] = "Catálogo de Unidades".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 5;
                    row["Nombre"] = "Reporte de Mantenimiento".ToUpper();
                    dt.Rows.Add(row);
                }
                else
                {
                    row = dt.NewRow();
                    row["id"] = 1;
                    row["Nombre"] = "Catálogo de Personal".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 3;
                    row["Nombre"] = "Catálogo de Proveedores".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 4;
                    row["Nombre"] = "Catálogo de Puestos".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 5;
                    row["Nombre"] = "Catálogo de Refacciones".ToUpper();
                    dt.Rows.Add(row);
                    row = dt.NewRow();
                    row["id"] = 6;
                    row["Nombre"] = "Reporte de Almacén".ToUpper();
                    dt.Rows.Add(row);
                }
            }
            {

            }
            cbapartado.ValueMember = "id";
            cbapartado.DisplayMember = "Nombre";
            cbapartado.DataSource = dt;

        }
        void inimeses()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Nombre");
            DataRow row = dt.NewRow();
            row["id"] = 0;
            row["Nombre"] = "--Seleccione una Mes--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 1;
            row["Nombre"] = "Enero".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 2;
            row["Nombre"] = "Febrero".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 3;
            row["Nombre"] = "Marzo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 4;
            row["Nombre"] = "Abril".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 5;
            row["Nombre"] = "Mayo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 6;
            row["Nombre"] = "Junio".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 7;
            row["Nombre"] = "Julio".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 8;
            row["Nombre"] = "Agosto".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 9;
            row["Nombre"] = "Septiembre".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 10;
            row["Nombre"] = "Octubre".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 11;
            row["Nombre"] = "Noviembre".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 12;
            row["Nombre"] = "Diciembre".ToUpper();
            dt.Rows.Add(row);
            cbmes.ValueMember = "id";
            cbmes.DisplayMember = "Nombre";
            cbmes.DataSource = dt;
        }
        private void cbapartado_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniModificaciones();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb1.Checked && DateTime.Parse(dtpd.Value.ToString("yyyy-MM-dd")) > DateTime.Parse(dtpa.Value.ToString("yyyy-MM-dd")))
                {
                    MessageBox.Show("Las Fechas Son Incorrectas. Verifique", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valoresDTP();
                }
                else
                {
                    tbmodif.Rows.Clear();
                    string sql = "SET lc_time_names='es_ES';SELECT DISTINCT t1.idmodificacion, t1.idregistro,(SELECT UPPER(t1.form)) as form,UPPER((SELECT DATE_FORMAT(MAX(fechaHora),'%W, %d de %M del %Y / %H:%i:%s') FROM modificaciones_sistema WHERE idregistro=t1.idregistro and form=t1.form and empresa='" + empresa + "' and area='" + area + "')) as fecha,upper(Tipo) as ultima,UPPER(Concat('Mostrar Más Información')) as m  FROM modificaciones_sistema as t1";
                    string wheres = "";
                    if (cbapartado.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE t1.form LIKE '" + cbapartado.Text + "%' ";
                        }
                        else
                        {
                            wheres += "AND t1.form LIKE '" + cbapartado.Text + "%' ";
                        }
                    }

                    if (cbtipo.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE t1.Tipo LIKE '" + cbtipo.Text + "%' ";
                        }
                        else
                        {
                            wheres += "AND t1.Tipo LIKE '" + cbtipo.Text + "%' ";
                        }
                    }

                    if (cbusuario.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE t1.usuariofkcpersonal LIKE '" + cbusuario.SelectedValue + "%' ";
                        }
                        else
                        {
                            wheres += "AND t1.usuariofkcpersonal LIKE '" + cbusuario.SelectedValue + "%' ";
                        }

                    }
                    if (cbmes.SelectedIndex>0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE MONTH(t1.fechaHora) = '"+cbmes.SelectedValue +"' AND YEAR(t1.fechaHora) = '"+DateTime.Now.ToString("yyyy")+"'";
                        }
                        else
                        {
                            wheres += " AND MONTH(t1.fechaHora) = '" + cbmes.SelectedValue + "' AND YEAR(t1.fechaHora) = '" + DateTime.Now.ToString("yyyy") + "'";
                        }
                    }
                    else if (cb1.Checked)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE DATE(t1.fechaHora) BETWEEN '" + dtpd.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpa.Value.ToString("yyyy-MM-dd") + "'";
                        }
                        else
                        {
                            wheres += " AND  DATE(t1.fechaHora) BETWEEN '" + dtpd.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpa.Value.ToString("yyyy-MM-dd") + "'";
                        }
                    }
                    else
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE DATE(t1.fechaHora) = DATE(NOW())";
                        }
                        else
                        {
                            wheres += " AND DATE(t1.fechaHora) = DATE(NOW())";
                        }
                    }
                    if (wheres == "")
                    {
                        wheres = " WHERE t1.empresa='" + empresa + "' and t1.area='" + area + "'  GROUP BY t1.idregistro,t1.form ORDER BY ((SELECT MAX(date(fechaHora)) FROM modificaciones_sistema WHERE idregistro=t1.idregistro)) ASC, Time((SELECT MAX(fechaHora) FROM modificaciones_sistema WHERE idregistro=t1.idregistro))DESC";
                    }
                    else
                    {
                        wheres += " AND t1.empresa='" + empresa + "' and t1.area='" + area + "' GROUP BY t1.idregistro,t1.form ORDER BY ((SELECT MAX(date(fechaHora)) FROM modificaciones_sistema WHERE idregistro=t1.idregistro)) ASC, Time((SELECT MAX(fechaHora) FROM modificaciones_sistema WHERE idregistro=t1.idregistro))DESC";
                    }
                    sql += wheres;

                    DataTable dt = (DataTable)v.getData(sql);
                    int numFilas = dt.Rows.Count;
                    cbapartado.SelectedIndex = 0;
                    cbusuario.SelectedIndex = 0;
                    cbtipo.SelectedIndex = 0;
                    cbmes.SelectedIndex = 0;
                    valoresDTP();
                    cb1.Checked = false;
                    if (numFilas > 0)
                    {
                        for (int i = 0; i < numFilas; i++)
                        {

                            tbmodif.Rows.Add(dt.Rows[i].ItemArray);
                        }
                        for (int i = 0; i < numFilas; i++)
                        {
                            tbmodif.Rows[i].Cells[3].Value = v.mayusculas(tbmodif.Rows[i].Cells[3].Value.ToString());
                        }
                        tbmodif.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        inimodificaciones();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            inimodificaciones();
        }

        private void tbmodif_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==5)
            {
                moreinfo m = new moreinfo(tbmodif.Rows[e.RowIndex].Cells[1].Value.ToString(), v.mayusculas(tbmodif.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower()),empresa,area);
                tbmodif.ClearSelection();
                m.Owner = this;
                m.ShowDialog();
            }
        }

        private void cbapartado_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void dtpd_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox) sender).Checked)
            {
                dtpd.Enabled = true;
                dtpa.Enabled = true;
            }else
            {
                dtpd.Enabled = false;
                dtpa.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inimodificaciones();
        }

        private void button1_Enter(object sender, EventArgs e)
        {
    
        }

        private void button1_Leave(object sender, EventArgs e)
        {
           
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
       
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
        
        }

        private void cbmes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbmes.SelectedIndex>0)
            {
                cb1.Checked = false;
                cb1.Enabled = false;
            }else
            {
                cb1.Enabled = true;
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        void iniModificaciones()
        {

            var res = cbapartado.Text;
            cbtipo.DataSource = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("Nombre");
                DataRow row = dt.NewRow();
                row["id"] = 0;
                row["Nombre"] = "--Seleccione un Tipo--".ToUpper();
                dt.Rows.Add(row);

            if (res == "Catálogo de Personal".ToUpper())
            {
                row = dt.NewRow();
                row["id"] = 1;
                row["Nombre"] = "Eliminación de Usuario".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 2;
                row["Nombre"] = "Inserción de Usuario".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 3;
                row["Nombre"] = "Inserción de Empleado".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 4;
                row["Nombre"] = "Actualización de Datos Personales".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 5;
                row["Nombre"] = "Actualización de Usuario".ToUpper();
                dt.Rows.Add(row);
            }
            else
            {
                row = dt.NewRow();
                row["id"] = 1;
                row["Nombre"] = "Desactivación".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 2;
                row["Nombre"] = "Reactivación".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["id"] = 3;
                row["Nombre"] = "Actualización".ToUpper();
                dt.Rows.Add(row);
                row = dt.NewRow();
            }
                    

            cbtipo.ValueMember = "id";
            cbtipo.DisplayMember = "Nombre";
            cbtipo.DataSource = dt;
        }
    }
}

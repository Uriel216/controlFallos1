using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class nuevaRefaccion : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario, status;
        string idRefaccionTemp,codrefAnterior, nomrefanterior, modrefanterior, familiaanterior, umAnterior, marcaAnterior, nivelAnterior, charolaAnterior,ultimoabastecimiento, mediaAnterior, abastecimientoAnterior;
        bool editar;
        decimal ultimacantidad;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        public nuevaRefaccion(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
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

                gbaddrefaccion.Visible = true;
            }
            if (Pconsultar)
            {
                gbconsultar.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
        }
        void iniubicaciones()
        {
            string sq = "SELECT COUNT(idpasillo) as cuenta FROM cpasillos WHERE status='1'";
            MySqlCommand cmd = new MySqlCommand(sq, c.dbconection());
            int numFilas = Convert.ToInt32(cmd.ExecuteScalar().ToString());


            if (numFilas > 0)
            {
                string sql = "SELECT idpasillo,pasillo FROM cpasillos WHERE status='1'";
                DataTable dt = new DataTable();
                DataRow nuevaFila = dt.NewRow();
                MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
                MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
                AdaptadorDatos.Fill(dt);
                cbpasillo.ValueMember = "idpasillo";
                cbpasillo.DisplayMember = "pasillo";
                nuevaFila["idpasillo"] = 0;
                nuevaFila["pasillo"] = "--Seleccione pasillo--";
                dt.Rows.InsertAt(nuevaFila, 0);
                cbpasillo.DataSource = dt;
                string sql1 = "SELECT idanaquel,anaquel FROM canaqueles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "'";
                DataTable dt1 = new DataTable();
                DataRow nuevaFila1 = dt1.NewRow();
                MySqlCommand cm1 = new MySqlCommand(sql1, c.dbconection());
                MySqlDataAdapter AdaptadorDatos1 = new MySqlDataAdapter(cm1);
                AdaptadorDatos1.Fill(dt1);
                cbanaquel.ValueMember = "idanaquel";
                cbanaquel.DisplayMember = "anaquel";
                nuevaFila1["idanaquel"] = 0;
                nuevaFila1["anaquel"] = "--Seleccione Anaquel--";
                dt1.Rows.InsertAt(nuevaFila1, 0);
                cbanaquel.DataSource = dt1;
                c.dbcon.Close();
            }
        }
        void iniFamilias()
        {
            string sql = "SELECT idfamilia,CONCAT(familia,' - ',descripcionFamilia) as familia FROM cfamilias WHERE status='1'";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila1 = dt1.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            cbfamilia.ValueMember = "idfamilia";
            cbfamilia.DisplayMember = "familia";
            cbfamiliabusq.ValueMember = "idfamilia";
            cbfamiliabusq.DisplayMember = "familia";
            nuevaFila["idfamilia"] = 0;
            nuevaFila["familia"] = "--Seleccione Familia--";
            nuevaFila1["idfamilia"] = 0;
            nuevaFila1["familia"] = "--Seleccione Familia--";
            dt.Rows.InsertAt(nuevaFila, 0);
            dt1.Rows.InsertAt(nuevaFila1, 0);
            cbfamilia.DataSource = dt;
            cbfamiliabusq.DataSource = dt1;
        }
        void iniUnidadMedida()
        {
            string sql = "SELECT idunidadmedida,CONCAT(Simbolo, ' - ',Nombre) as um FROM cunidadmedida WHERE status='1'";
            DataTable dt = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbum.ValueMember = "idunidadmedida";
            cbum.DisplayMember = "um";
            nuevaFila["idunidadmedida"] = 0;
            nuevaFila["um"] = "--Seleccione Unidad de Medida--";
            dt.Rows.InsertAt(nuevaFila, 0);
            cbum.DataSource = dt;
        }
        void inimarcas()
        {
            string sql = "SELECT idmarca,marca FROM cmarcas WHERE status='1'";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila1 = dt1.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            AdaptadorDatos.Fill(dt1);
            cbmarcas.ValueMember = "idmarca";
            cbmarcas.DisplayMember = "marca";
            cbmarcasbusq.ValueMember = "idmarca";
            cbmarcasbusq.DisplayMember = "marca";
            nuevaFila["idmarca"] = 0;
            nuevaFila["marca"] = "--Seleccione marca--";
            nuevaFila1["idmarca"] = 0;
            nuevaFila1["marca"] = "--Seleccione marca--";
            dt.Rows.InsertAt(nuevaFila, 0);
            dt1.Rows.InsertAt(nuevaFila1, 0);
            cbmarcas.DataSource = dt;
            cbmarcasbusq.DataSource = dt1;
        }
        void ininivel()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["idnivel"] = 0;
            row["Nombre"] = "--Seleccione Nivel--";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "Superior";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "Inferior";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 3;
            row["Nombre"] = "Izquierdo";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 4;
            row["Nombre"] = "Derecho";
            dt.Rows.Add(row);
            cbnivel.ValueMember = "idnivel";
            cbnivel.DisplayMember = "Nombre";
            cbnivel.DataSource = dt;
        }
        private void nuevaRefaccion_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar) {
                iniFamilias();
                iniubicaciones();
                iniUnidadMedida();
                inimarcas();
                ininivel();
                proxabastecimiento.Value = DateTime.Now;
            }
            if (Pconsultar) {
                insertarRefacciones();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tbrefaccion.Rows.Clear();
            string sql = @"SELECT t1.idrefaccion,t1.codrefaccion,t1.nombreRefaccion,t1.modeloRefaccion,CONCAT(t2.familia,' - ',t2.descripcionFamilia) as familia,CONCAT(t3.Simbolo, ' - ',t3.Nombre) as um, t8.marca,t1.nivel,concat(t7.nombres,' ',t7.ApPaterno,' ',t7.ApMaterno) as nombre, (SELECT CONCAT('Pasillo: ',tabla4.pasillo, '; Anaquel: ', tabla3.anaquel,'; Charola:',tabla2.charola) FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as Ubicacion, CONCAT(t1.existencias, ' ',t3.Simbolo) AS existencias,CONCAT(t1.media,' ',t3.Simbolo) AS media,t1.media as idmedia,CONCAT(t1.abastecimiento,' ',t3.Simbolo) AS abastecimiento,t1.abastecimiento as idabastecimiento,t1.fechaHoraalta as fecha,t1.status,t2.idfamilia,t3.idunidadmedida, t8.idmarca,(SELECT tabla4.idpasillo FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idpasillo,(SELECT tabla3.idanaquel FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idanaquel, t1.charolafkcharolas as idcharola, DATE(t1.proximoAbastecimiento) as proxabast FROM crefacciones as t1 INNER JOIN cfamilias as t2 ON t1.familiafkcfamilias=t2.idfamilia INNER JOIN cunidadmedida as t3 ON t1.umfkcunidadmedida=t3.idunidadmedida INNER JOIN cpersonal as t7 ON t1.usuarioaltafkcpersonal= t7.idPersona INNER JOIN cmarcas as t8 ON t1. marcafkcmarcas = t8.idmarca ";
            string wheres = "";
            if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text))
            {
                if (wheres == "")
                {
                    wheres = "WHERE nombreRefaccion ='"+ txtnombrereFaccionbusq.Text + "' ";
                }
                else
                {
                    wheres += " OR nombreRefaccion ='" + txtnombrereFaccionbusq.Text + "' ";
                }
            }
            if (cbfamiliabusq.SelectedIndex>0)
            {
                if (wheres == "")
                {
                    wheres = "WHERE familiafkcfamilias ='" + cbfamiliabusq.SelectedValue + "' ";
                }
                else
                {
                    wheres += " OR familiafkcfamilias ='" + cbfamiliabusq.SelectedValue + "' ";
                }
            }
            if (cbmarcasbusq.SelectedIndex > 0)
            {
                if (wheres == "")
                {
                    wheres = "WHERE marcafkcmarcas ='" + cbmarcasbusq.SelectedValue + "' ";
                }
                else
                {
                    wheres += " OR marcafkcmarcas ='" + cbmarcasbusq.SelectedValue + "' ";
                }
            }
            sql += wheres;
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbrefaccion.Rows.Add(dr.GetString("idrefaccion"), dr.GetString("codrefaccion"), dr.GetString("nombreRefaccion"), dr.GetString("modeloRefaccion"), dr.GetString("familia"), dr.GetString("um"), dr.GetString("marca"), v.getNivelFromID(dr.GetInt32("nivel")), dr.GetString("Ubicacion"), dr.GetString("existencias"), dr.GetString("media"), dr.GetString("abastecimiento"), dr.GetDateTime("proxabast").ToShortDateString(), dr.GetString("nombre"), dr.GetString("fecha"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idfamilia"), dr.GetString("idunidadmedida"), dr.GetString("idmarca"), dr.GetString("nivel"), dr.GetString("idpasillo"), dr.GetString("idanaquel"), dr.GetString("idcharola"), dr.GetString("nivel"), dr.GetString("idmedia"), dr.GetString("idabastecimiento"));
            }
            tbrefaccion.ClearSelection();
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarRefacciones();
        }

        private void cbanaquel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT idcharola,charola FROM ccharolas WHERE status='1' and anaquelfkcanaqueles = '" + cbanaquel.SelectedValue + "'";
            DataTable dt = new DataTable();
            DataRow nuevaFila = dt.NewRow();
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            AdaptadorDatos.Fill(dt);
            cbcharola.ValueMember = "idcharola";
            cbcharola.DisplayMember = "charola";
            nuevaFila["idcharola"] = 0;
            nuevaFila["charola"] = "--Seleccione Charola--";
            dt.Rows.InsertAt(nuevaFila, 0);
            cbcharola.DataSource = dt;
        }

        private void txtcodrefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.codrefaccionValido(txtcodrefaccion.Text);
        }

        private void txtnombrereFaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!v.palabraValida(txtnombrereFaccion.Text))
            {
                MessageBox.Show(txtnombrereFaccion.Text+" No es Un Nombre de Refacción Válido","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void txtmodeloRefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.codrefaccionValido(txtmodeloRefaccion.Text);
        }

        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT idanaquel,anaquel FROM canaqueles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "'";
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

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btndelcla_Click(object sender, EventArgs e)
        {
            try
            {
                int status;
                string msg;
                if (this.status == 0)
                {
                    status = 1;
                    msg = "Re";
                } else
                {
                    status = 0;
                    msg = "Des";
                }

                if (MessageBox.Show("¿Está Seguro que desea " + msg + "activar la Refacción?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (c.insertar("UPDATE crefacciones SET status ='" + status + "' WHERE  idrefaccion='" + this.idRefaccionTemp + "'"))
                    {
                        MessageBox.Show("Se ha " + msg + "activado la Refacción Correctamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtcodrefaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paracodrefaccion(e);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }else
                 {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        } 
        void _editar()
        {
            string codrefaccion =txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            string proxabastParaValidacion = proxabastecimiento.Value.ToShortDateString();
            int familia = Convert.ToInt32(cbfamilia.SelectedValue.ToString());
            int um = Convert.ToInt32(cbum.SelectedValue.ToString());
            int marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int nivel = Convert.ToInt32(cbnivel.SelectedValue.ToString());
            int charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue.ToString());
            int cantidad = Convert.ToInt32(cantidada.Value.ToString());
            int media = Convert.ToInt32(notifmedia.Value.ToString());
            int abastecimiento = Convert.ToInt32(notifabastecimiento.Value.ToString());
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, familia, um, marca, nivel, Convert.ToInt32(cbpasillo.SelectedValue), Convert.ToInt32(cbanaquel.SelectedValue), charolafkccharolas, proxabastecimiento.Value) )
            {
                if (codrefaccion.Equals(codrefAnterior) && nombreRefaccion.Equals(nomrefanterior) && modeloRefaccion.Equals(modrefanterior) && proxabastParaValidacion.Equals(ultimoabastecimiento) && familia.ToString().Equals(familiaanterior) && um.ToString().Equals(umAnterior) && marca.ToString().Equals(marcaAnterior) && nivel.ToString().Equals(nivelAnterior) && charolafkccharolas.ToString().Equals(charolaAnterior) && media.ToString().Equals(mediaAnterior) && abastecimiento.ToString().Equals(abastecimientoAnterior) && Convert.ToInt32(cantidada.Value)==1)
                {
                    MessageBox.Show("No se Realizaron Cambios","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desea Limpiar los Campos?", "Control de Fallos", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        limpiar();
                    }
                }else
                {
                    if (!v.existeRefaccionActualizar(codrefaccion,codrefAnterior,nombreRefaccion,nomrefanterior,modeloRefaccion,modrefanterior))
                    {
                        if (status==1) {
                            int exist = cantidad + Convert.ToInt32(ultimacantidad);
                            if (c.insertar(@"UPDATE crefacciones SET codrefaccion = '" + codrefaccion + "', nombreRefaccion = '" + nombreRefaccion + "', modeloRefaccion = '" + modeloRefaccion + "', proximoAbastecimiento = '" + proxabast + "', familiafkcfamilias = '" + familia + "', umfkcunidadmedida = '" + um + "', charolafkcharolas = '" + charolafkccharolas + "', existencias = '" + exist + "', marcafkcmarcas = '" + marca + "', nivel = '" + nivel + "', media = '" + media + "', abastecimiento = '" + abastecimiento + "' WHERE idrefaccion = '" + this.idRefaccionTemp + "'"))
                            {
                                MessageBox.Show("Refacción Actualizada Exitosamente", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                            }
                        }else
                        {
                            MessageBox.Show("No se Puede Actualizar una Refacción Desactivada","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }

                }
                

                
            }
        }
        void insertar()
        {
            string codrefaccion = txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            int familia = Convert.ToInt32(cbfamilia.SelectedValue.ToString());
            int um = Convert.ToInt32(cbum.SelectedValue.ToString());
            int marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int nivel = Convert.ToInt32(cbnivel.SelectedValue.ToString());
            int charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue.ToString());
            decimal cantidad = Convert.ToDecimal(cantidada.Value.ToString());
            decimal media = Convert.ToDecimal(notifmedia.Value.ToString());
            decimal abastecimiento = Convert.ToDecimal(notifabastecimiento.Value.ToString());
            DateTime paraValidacion = proxabastecimiento.Value;
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, familia, um, marca, nivel, Convert.ToInt32(cbpasillo.SelectedValue), Convert.ToInt32(cbanaquel.SelectedValue), charolafkccharolas,paraValidacion) && !v.existeRefaccion(codrefaccion, nombreRefaccion, modeloRefaccion) && !v.NumericsUpDownRefaccion(Convert.ToInt32(cantidad), Convert.ToInt32(media), Convert.ToInt32(abastecimiento)))
            {
                if (c.insertar(@"INSERT INTO crefacciones(codrefaccion, nombreRefaccion, modeloRefaccion, proximoAbastecimiento, familiafkcfamilias, umfkcunidadmedida, charolafkcharolas, existencias, marcafkcmarcas, nivel, fechaHoraalta,
                                usuarioaltafkcpersonal, media, abastecimiento)  VALUES ('" + codrefaccion + "','" + nombreRefaccion + "','" + modeloRefaccion + "','" + proxabast + "','" + familia + "','" + um + "','" + charolafkccharolas + "','" + cantidad + "','" + marca + "','" + nivel + "',NOW(),'" + idUsuario + "','" + media + "','" + abastecimiento + "')"))
                {
                    MessageBox.Show("Refacción Agregada Exitosamente","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void limpiar()
        {
            txtcodrefaccion.Clear();
            txtnombrereFaccion.Clear();
            txtmodeloRefaccion.Clear();
            editar = false;
            proxabastecimiento.Value = DateTime.Now.Date;
            cbfamilia.SelectedIndex = 0;
            cbum.SelectedIndex = 0;
            cbmarcas.SelectedIndex = 0;
            cbnivel.SelectedIndex = 0;
            cbpasillo.SelectedIndex = 0;
            cbanaquel.SelectedIndex = 0;
            cbcharola.SelectedIndex = 0;
            cantidada.Value = cantidada.Minimum;
            notifmedia.Value = notifmedia.Minimum;
            notifabastecimiento.Value = notifabastecimiento.Minimum;
            pCancelar.Visible = false;
            pdelref.Visible = false;
            insertarRefacciones();
            btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
            lblsave.Text = "Agregar Refacción";
            idRefaccionTemp = null;
            codrefAnterior = null;
            nomrefanterior = null;
            modrefanterior = null;
            familiaanterior = null;
            umAnterior = null;
            marcaAnterior = null;
            nivelAnterior = null;
            charolaAnterior = null;
            ultimoabastecimiento = null;
            mediaAnterior = null;
            abastecimientoAnterior = null;
            lblexistencias.Text = null;
            pExistencias.Visible = false;
        }

        public void insertarRefacciones()
        {
            tbrefaccion.Rows.Clear();
            string sql = @"SELECT t1.idrefaccion,t1.codrefaccion,t1.nombreRefaccion,t1.modeloRefaccion,CONCAT(t2.familia,' - ',t2.descripcionFamilia) as familia,CONCAT(t3.Simbolo, ' - ',t3.Nombre) as um, t8.marca,t1.nivel,concat(t7.nombres,' ',t7.ApPaterno,' ',t7.ApMaterno) as nombre, (SELECT CONCAT('Pasillo: ',tabla4.pasillo, '; Anaquel: ', tabla3.anaquel,'; Charola:',tabla2.charola) FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as Ubicacion, CONCAT(t1.existencias, ' ',t3.Simbolo) AS existencias,CONCAT(t1.media,' ',t3.Simbolo) AS media,t1.media as idmedia,CONCAT(t1.abastecimiento,' ',t3.Simbolo) AS abastecimiento,t1.abastecimiento as idabastecimiento,t1.fechaHoraalta as fecha,t1.status,t2.idfamilia,t3.idunidadmedida, t8.idmarca,(SELECT tabla4.idpasillo FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idpasillo,(SELECT tabla3.idanaquel FROM crefacciones as tabla1 INNER JOIN  ccharolas as tabla2 ON tabla1.charolafkcharolas=tabla2.idcharola INNER JOIN canaqueles as tabla3 ON tabla2.anaquelfkcanaqueles = tabla3.idanaquel INNER JOIN cpasillos as tabla4 ON tabla3.pasillofkcpasillos=tabla4.idpasillo WHERE tabla1.idrefaccion = t1.idrefaccion) as idanaquel, t1.charolafkcharolas as idcharola, DATE(t1.proximoAbastecimiento) as proxabast, t1.existencias as ultim FROM crefacciones as t1 INNER JOIN cfamilias as t2 ON t1.familiafkcfamilias=t2.idfamilia INNER JOIN cunidadmedida as t3 ON t1.umfkcunidadmedida=t3.idunidadmedida INNER JOIN cpersonal as t7 ON t1.usuarioaltafkcpersonal= t7.idPersona INNER JOIN cmarcas as t8 ON t1. marcafkcmarcas = t8.idmarca";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
             MySqlDataReader dr = cm.ExecuteReader();
             while (dr.Read())
             {
                tbrefaccion.Rows.Add(dr.GetString("idrefaccion"), dr.GetString("codrefaccion"), dr.GetString("nombreRefaccion"), dr.GetString("modeloRefaccion"), dr.GetString("familia"), dr.GetString("um"), dr.GetString("marca"), v.getNivelFromID(dr.GetInt32("nivel")), dr.GetString("Ubicacion"), dr.GetString("existencias"), dr.GetString("media"), dr.GetString("abastecimiento"), dr.GetDateTime("proxabast").ToShortDateString(), dr.GetString("nombre"), dr.GetString("fecha"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idfamilia"), dr.GetString("idunidadmedida"), dr.GetString("idmarca"), dr.GetString("nivel"),dr.GetString("idpasillo"), dr.GetString("idanaquel"), dr.GetString("idcharola"), dr.GetString("nivel"), dr.GetString("idmedia"), dr.GetString("idabastecimiento"),dr.GetString("ultim"));
             }
             tbrefaccion.ClearSelection();
        }

        private void tbrefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbrefaccion.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void tbrefaccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                idRefaccionTemp = (string) tbrefaccion.Rows[e.RowIndex].Cells[0].Value;
                status = v.getStatusInt(tbrefaccion.Rows[e.RowIndex].Cells[15].Value.ToString());
                pExistencias.Visible = true;
                if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelref.Text = "Reactivar Refaccion";
                    }
                    else
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelref.Text = "Desactivar Refaccion";
                    }
                   
                    pCancelar.Visible = true;
                    pdelref.Visible = true;
                }
                if (Peditar) {
                    lblexistencias.Text = v.getExistenciasFromIDRefaccion((string)tbrefaccion.Rows[e.RowIndex].Cells[0].Value);
                    txtcodrefaccion.Text = codrefAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[1].Value;
                    txtnombrereFaccion.Text = nomrefanterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[2].Value;
                    txtmodeloRefaccion.Text = modrefanterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[3].Value;
                    proxabastecimiento.Value = DateTime.Parse(ultimoabastecimiento = (string)tbrefaccion.Rows[e.RowIndex].Cells[12].Value);
                    cbfamilia.SelectedValue = familiaanterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[16].Value;
                    cbum.SelectedValue = umAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[17].Value;
                    if (cbum.SelectedIndex==-1)
                    {
                        cbum.SelectedIndex = 0;
                        cbum.Focus();
                        MessageBox.Show("La Unidad de Medida Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable","Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    cbmarcas.SelectedValue = marcaAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[18].Value;
                    if (cbmarcas.SelectedIndex == -1)
                    {
                        cbmarcas.SelectedIndex = 0;
                        cbmarcas.Focus();
                        MessageBox.Show("La Marca Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbnivel.SelectedValue = nivelAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[23].Value;
                    cbpasillo.SelectedValue = tbrefaccion.Rows[e.RowIndex].Cells[20].Value;
                    if (cbpasillo.SelectedIndex == -1)
                    {
                        cbpasillo.SelectedIndex = 0;
                        cbpasillo.Focus();
                        MessageBox.Show("El Pasillo Asociado a la Refacción ha sido desactivado.\nSeleccione uno de la Lista Desplegable", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbanaquel.SelectedValue = tbrefaccion.Rows[e.RowIndex].Cells[21].Value;
                    if (cbanaquel.SelectedIndex == -1)
                    {
                        cbanaquel.SelectedIndex = 0;
                        cbanaquel.Focus();
                        MessageBox.Show("El Anaquel Asociado a la Refacción ha sido desactivado.\nSeleccione uno de la Lista Desplegable", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cbcharola.SelectedValue = charolaAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[22].Value;
                    if (cbcharola.SelectedIndex == -1)
                    {
                        cbcharola.SelectedIndex = 0;
                        cbcharola.Focus();
                        MessageBox.Show("La Charola Asociada a la Refacción ha sido desactivada.\nSeleccione una de la Lista Desplegable", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    notifmedia.Value = Convert.ToDecimal(mediaAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[24].Value);
                    notifabastecimiento.Value = Convert.ToDecimal(abastecimientoAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[25].Value);
                    ultimacantidad = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[26].Value);
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Editar Refacción";
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Control de Fallos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}

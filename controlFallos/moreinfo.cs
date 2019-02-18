using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class moreinfo : Form
    {
        validaciones v = new validaciones();
        int? y1 = null;
        string id,ct;
        int empresa, area;
        string sqlFolio;
        public moreinfo(string id,string ct,int empresa,int area)
        {
            InitializeComponent();
            this.id = id;
            this.ct = ct;
            this.empresa = empresa;
            this.area = area;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
        void Buscar()
        {
            int num_Modif = Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM modificaciones_sistema WHERE idregistro='" + id + "' and form='"+ct+"'"));
            if (num_Modif > 1)
            {
                gmmodifs.Visible = true;
                tbmodif.Rows.Clear();
                tbmodif.DataSource = v.getData("SET lc_time_names='es_ES';SELECT t1.idmodificacion as 'Identificador', concat(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as 'Usuario que Modificó',  DATE_FORMAT( t1.fechaHora,'%W, %d de %M del %Y') as 'Fecha de Modificación', time(t1.fechaHora) as 'Hora de Modificación',t1.Tipo as 'Tipo de Modificación' FROM modificaciones_sistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona WHERE idregistro='" + id + "' and form='" + v.mayusculas(ct.ToLower()) + "' and t1.empresa='"+empresa+"' and t1.area='"+area+"'");
                tbmodif.Columns[0].Visible = false;
                tbmodif.ClearSelection();
            }
            else
            {
                gbadd.Dock = DockStyle.Fill;
                this.Size = new Size(1225, 517);
                lbltitle.Location = new Point(0, 0);
                lbltitle.Left = (this.panel1.Width - lbltitle.Size.Width) / 2;
                lbltitle.Top = (panel1.Height - lbltitle.Size.Height) / 2;
                CenterToParent();
                gbadd.Dock = DockStyle.Fill;
                string rec = v.getaData("SELECT CONCAT(Tipo,';',idmodificacion) FROM modificaciones_sistema WHERE idregistro= '" + id + "' and form='" + ct + "'").ToString();
                string[] dos = rec.Split(';');
                crear(dos[0], dos[1]);
            }
        }

        private void moreinfo_Load(object sender, EventArgs e)
        {
            Buscar();
            acomodarLabel();
        }
        void acomodarLabel()
        {
            lbltitle.Left = (panel1.Width - lbltitle.Width) / 2;
            lbltitle.Top = (panel1.Height - lbltitle.Height) / 2;
        }
        //void crearLabel()
        //{

        //   lbl.Text = "Creación de Label desde Código";
        //    lbl.AutoSize = true;
        //    lbl.Name = "lbl1s";
        //    lbl.Location = new Point(500, 500);

        //    panel2.Controls.Add(lbl);

        //}

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        void crearListBox(string[] data)
        {
            cerrar();
            Label lbl = new Label();
            lbl.Text = ("Folios De Reportes Que Fueron Exportados a Formato Excel:").ToUpper();
              lbl.AutoSize = true;
              lbl.Name = "lbl1s";
            lbl.Font = new Font(new FontFamily("Garamond"), 16, FontStyle.Bold);
            lbl.Location = new Point(40, 30);
             gbadd.Controls.Add(lbl);
           int y = 330;
            for (int i = 0; i < data.Length-1; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                l.Text = v.mayusculas(data[i]);
                l.AutoSize = true;
               
                l.Location = new Point(((gbadd.Size.Width - l.Size.Width) / 2), y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;          
                gbadd.Controls.Add(l);
                l.Left = (gbadd.Width - l.Size.Width) / 2;
            }
            string[] ids = data[data.Length - 1].Split(';');
            string[] folio = new string[ids.Length];
            for (int i=0;i<ids.Length;i++)
            {
                folio[i] = v.getaData(sqlFolio + "'"+ids[i]+"'").ToString();
            }
            folio = MetodoBurbuja(folio);
            ListBox list = new ListBox();
            list.Location = new Point(10,100);
            list.Size = new Size(gbadd.Width-50,250);      
            list.BackColor = Color.FromArgb(200, 200, 200);
            list.ForeColor = Color.FromArgb(75, 44, 52);
            list.BorderStyle = BorderStyle.None;
            list.MultiColumn = true;
            list.DrawMode = DrawMode.OwnerDrawFixed;
            list.Name = "listfolios";
            list.DrawItem += new DrawItemEventHandler(v.listbox_DrawItem);
            list.ItemHeight = 25;
          //  list.DoubleClick += new EventHandler(MostrarDatosEspecificosListBox);
            for (int i=0;i<folio.Length;i++)
            {
                list.Items.Add(folio[i]);
            }
            gbadd.Controls.Add(list);
            
        }

        private void MostrarDatosEspecificosListBox(object sender, EventArgs e)
        {
            var res = ((ListBox)sender).SelectedItem;
            mostrarFolio m = new mostrarFolio(res.ToString(),empresa,area);
            m.Owner = this;
            m.ShowDialog();
            
        }
        private void tbmodif_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row > -1) {
                try
                {
                    var tipo = tbmodif.Rows[e.RowIndex].Cells[4].Value.ToString();
                    var id = tbmodif.Rows[e.RowIndex].Cells[0].Value.ToString();
                    crear(tipo, id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
          }
        int tempp4;
        void paraNuevasRefacciones(string[,] data)
        {
            DataGridView tbrefacciones = new DataGridView();
            tbrefacciones.BackgroundColor = Color.FromArgb(200, 200, 200);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.BackColor = Color.FromArgb(180, 180, 180);
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 12, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True;
            tbrefacciones.AlternatingRowsDefaultCellStyle = d;
            tbrefacciones.BorderStyle = BorderStyle.None;
            tbrefacciones.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefacciones.ColumnHeadersDefaultCellStyle = d;
            tbrefacciones.EnableHeadersVisualStyles = false;
            tbrefacciones.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            tbrefacciones.RowHeadersDefaultCellStyle = d;
            tbrefacciones.RowHeadersVisible = false;
            tbrefacciones.RowsDefaultCellStyle = d;
            tbrefacciones.AllowDrop = false;
            tbrefacciones.AllowUserToAddRows = false;
            tbrefacciones.AllowUserToDeleteRows = false;
            tbrefacciones.AllowUserToOrderColumns = false;
            tbrefacciones.AllowUserToResizeColumns = false;
            tbrefacciones.AllowUserToResizeRows = false;
            tbrefacciones.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            tbrefacciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbrefacciones.EditMode = DataGridViewEditMode.EditProgrammatically;
            tbrefacciones.MultiSelect = false;
            tbrefacciones.ReadOnly = true;
            tbrefacciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbrefacciones.Name = "tbrefacciones";
            tbrefacciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbrefacciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbrefacciones.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView2_CellFormatting);
            tbrefacciones.CellClick += new DataGridViewCellEventHandler(clear);
            tbrefacciones.Columns.Add("codref", "Código de Refacción".ToUpper());
            tbrefacciones.Columns.Add("nomref", "Nombre de Refacción".ToUpper());
            tbrefacciones.Columns.Add(v.mayusculas("ESTATUS DE REFACCIÓN".ToLower()), "Existencia de Refacción".ToUpper());
            tbrefacciones.Columns.Add("cantidadentregada", "Cantidad Entregada".ToUpper());
            gbadd.Controls.Add(tbrefacciones);
            for (int i=0;i<data.GetLength(0);i++)
            {
                tbrefacciones.Rows.Add(data[i,0], data[i, 1], data[i, 2], data[i, 3]);
            }
            tbrefacciones.Size = new Size(gbadd.Width - 180, 100);
            tbrefacciones.Location = new Point(0, 200);
            tbrefacciones.Left = (gbadd.Width - tbrefacciones.Width) / 2;
            tbrefacciones.ClearSelection();

        }
        void CrearAntesDespuesLabels()
        {
            FontFamily fontFamily = new FontFamily("Garamond");
            Label lbl = new Label();
            lbl.Font = new Font(fontFamily, 16, FontStyle.Bold);
            lbl.Text = v.mayusculas("Anterior").ToUpper();
            lbl.AutoSize = true;
            lbl.Location = new Point(200,y1??40);
            lbl.Name = "lblAntes";
            gbadd.Controls.Add(lbl);
            Label l1 = new Label();
            l1.Text = v.mayusculas("Actual").ToUpper();
            l1.AutoSize = true;
            l1.Location = new Point(700,y1??40);
            l1.Font = new Font(fontFamily,16, FontStyle.Bold);
            l1.Name = "lblActual";
            gbadd.Controls.Add(l1);
        }
        void crearFormularioPersonalcambios(string[] arreglo)
        {
            cerrar();
            int x = 10;
            int y = 40;

            for (int i = 0; i < 5; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                if(arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;

                gbadd.Controls.Add(l);
                if (arreglo.Length <= 5)
                {
                    l.Left = (gbadd.Width - l.Size.Width) / 2;
                }
            }
            if (arreglo.Length > 5)
            {


                x = 600;
                y = 40;
                for (int i = 5; i < arreglo.Length; i++)
                {
                    y += 40;
                    Label l = new Label();
                    l.UseMnemonic = false;
                    if (arreglo[i].Length > 40)
                    {
                        int c = 0;
                        string temp2 = "";
                        string temp = "";
                        string[] temp4 = arreglo[i].Split(' ');
                        for (int j = 0; j < temp4.Length; j++)
                        {
                            if ((temp + " " + temp4[j]).Length < 40)
                            {
                                temp += " " + temp4[j];
                            }
                            else
                            {
                                temp2 += temp + Environment.NewLine;
                                temp = "";
                                j--;
                                c += 25;
                            }
                        }
                        if (temp != null)
                        {
                            temp2 += temp;
                            temp = null;
                        }
                        l.Text = v.mayusculas(temp2);

                    }
                    else
                    {
                        l.Text = v.mayusculas(arreglo[i]);
                    }
                    l.AutoSize = true;
                    l.Location = new Point(x, y);
                    l.Font = new Font(this.Font, FontStyle.Bold);
                    l.Name = "lbl" + i;
                    gbadd.Controls.Add(l);
                }
            }
        }
        void mitad1mitad(string[] arreglo)
        {
            cerrar();
            int x = 80;
            int y = y1??40;

            for (int i = 0; i < arreglo.Length/2; i++)
            {
                y += 40;
                Label l = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;

                gbadd.Controls.Add(l);
                if (arreglo.Length <= 5)
                {
                    l.Left = (gbadd.Width - l.Size.Width) / 2;
                }
            }


                x = 600;
                y = y1??40;
                for (int i =(arreglo.Length / 2) ; i < arreglo.Length; i++)
                {
                    y += 40;
                    Label l = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                    l.Location = new Point(x, y);
                    l.Font = new Font(this.Font, FontStyle.Bold);
                    l.Name = "lbl" + i;
                    gbadd.Controls.Add(l);
                }
            
        }
        void mitad1mitad2(string[] arreglo)
        {
         
            int x = 80;
            int y = y1 ?? 40;

            for (int i = 0; i < arreglo.Length / 2; i++)
            {

                y += 40;
                if (tempp4 > 0)
                {
                    y += tempp4;
                    tempp4 = 0;
                }
                Label l = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                        tempp4 = 20;
                    }
                    l.Text = v.mayusculas(temp2);
               
                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;
                gbadd.Controls.Add(l);
             
            }


            x = 600;
            y = y1 ?? 40;
            for (int i = (arreglo.Length / 2); i < arreglo.Length; i++)
            {
                y += 40;
                if (tempp4 > 0)
                {
                    y += tempp4;
                    tempp4 = 0;
                }
                Label l = new Label();

                if (arreglo[i].Length > 45)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);
                    tempp4 = 40;
                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                gbadd.Controls.Add(l);
            }

        }
        void crearCatalogoPuesto(string[] arreglo)
        {
            cerrar();
            int x = 40;
            int y = 0;

            for (int i = 0; i < arreglo.Length; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                l.Text = arreglo[i];
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;
           
                gbadd.Controls.Add(l);

                l.Left = (this.gbadd.Width - l.Size.Width) / 2;
            }
        }
         void paralabelsImpares(string[] arreglo)
        {
            cerrar();
            int x = 40;
            int y = 40;
            int media= (int)Math.Floor(Decimal.Parse((arreglo.Length / 2).ToString()));
            for (int i = 0; i < media; i++)
            {

                y += 40;
                Label label = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c=0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    label.Text = v.mayusculas(temp2);

                }
                else
                {
                    label.Text = v.mayusculas(arreglo[i]);
                }
                label.AutoSize = true;
                label.Location = new Point(x, y);
                label.Font = new Font(this.Font, FontStyle.Bold);
                label.Name = "lbl" + i;
                label.TabIndex = i;

                gbadd.Controls.Add(label);
              
            }
            x = 600;
            y = 40;
                for(int i = media; i<arreglo.Length-1; i++)
            {
                y += 40;
                Label label = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    label.Text = v.mayusculas(temp2);

                }
                else
                {
                    label.Text = v.mayusculas(arreglo[i]);
                }
                label.AutoSize = true;
                label.Location = new Point(x, y);
                label.Font = new Font(this.Font, FontStyle.Bold);
                label.Name = "lbl" + i;
                label.TabIndex = i;
                gbadd.Controls.Add(label);
                    }
            Label l = new Label();
            l.Text = v.mayusculas(arreglo[arreglo.Length-1]);
            l.AutoSize = true;
            l.Location = new Point(600, y+40);
           // l.Left = (this.gbadd.Width - l.Size.Width) / 3;
            l.Font = new Font(Font, FontStyle.Bold);
            l.Name = "lbl" +( arreglo.Length-1);         
            gbadd.Controls.Add(l);
           
        }
        void crearDataGrid(DataTable dt,Point p)
        {
            DataGridView tbrefacciones = new DataGridView();     
            tbrefacciones.BackgroundColor = Color.FromArgb(200,200,200);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.BackColor = Color.FromArgb(180, 180, 180);
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond",12,FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True;
            tbrefacciones.AlternatingRowsDefaultCellStyle = d;
            tbrefacciones.BorderStyle = BorderStyle.None;
            tbrefacciones.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            d.BackColor = Color.FromArgb(200,200,200);
            tbrefacciones.ColumnHeadersDefaultCellStyle = d;
            tbrefacciones.EnableHeadersVisualStyles = false;
            tbrefacciones.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            tbrefacciones.RowHeadersDefaultCellStyle = d;
            tbrefacciones.RowHeadersVisible = false;
            tbrefacciones.RowsDefaultCellStyle = d;
            tbrefacciones.AllowDrop = false;
            tbrefacciones.AllowUserToAddRows = false;
            tbrefacciones.AllowUserToDeleteRows = false;
            tbrefacciones.AllowUserToOrderColumns = false;
            tbrefacciones.AllowUserToResizeColumns = false;
            tbrefacciones.AllowUserToResizeRows = false;
            tbrefacciones.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            tbrefacciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbrefacciones.EditMode = DataGridViewEditMode.EditProgrammatically;
            tbrefacciones.MultiSelect = false;
            tbrefacciones.ReadOnly = true;
            tbrefacciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbrefacciones.Name = "tbrefacciones";
            tbrefacciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbrefacciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbrefacciones.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView2_CellFormatting);
            tbrefacciones.CellClick += new DataGridViewCellEventHandler(clear);
            tbrefacciones.DataSource = dt;
            gbadd.Controls.Add(tbrefacciones);
            tbrefacciones.Size = new Size(gbadd.Width-180,200);
            tbrefacciones.Location = p;
            tbrefacciones.Left = (gbadd.Width - tbrefacciones.Width) / 2;
            tbrefacciones.ClearSelection();

        }
        void crearPicturesBox(string[] imagenes)
        {
            int x = 200;
            for (int i=0;i<imagenes.Length;i++)
            {
                if (imagenes[i]!="") {
                    PictureBox p1 = new PictureBox();
                    p1.BackgroundImageLayout = ImageLayout.Stretch;
                    p1.Size = new Size(100, 100);

                    p1.Location = new Point(x, 300);
                    p1.BackgroundImage = v.StringToImage(imagenes[0]);
                    gbadd.Controls.Add(p1);
                    if (imagenes.Length==1)
                    {
                        p1.Left = (gbadd.Width - p1.Width) / 2;
                    }
                }
                x += 400;
            }
        }
         private void clear(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView tbRefacciones = sender as DataGridView;
            tbRefacciones.ClearSelection();
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView tbRefacciones = sender as DataGridView;
            if (tbRefacciones.Columns[e.ColumnIndex].Name == v.mayusculas("ESTATUS DE REFACCIÓN".ToLower()))
            {
                if (Convert.ToString(e.Value) == "EXISTENCIA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "SIN EXISTENCIA")
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
            if (tbRefacciones.Columns[e.ColumnIndex].Name == v.mayusculas("CANTIDAD FALTANTE".ToLower()))
            {
                if (Convert.ToInt32(e.Value??0) > 0)
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToInt32(e.Value) == 0)
                    {
                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                }
            }
        }
        void crear(string tipo, string id)
        {
            try
            {
                switch (tipo)
                {
                    case "Actualización de Datos Personales":
                       crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'))) FROM modificaciones_sistema  as t1 INNER JOIN cpersonal as t2 On t1.idregistro = t2.idpersona WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        string cadena = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(concat(t1.ultimamodificacion, ';CREDENCIAL: ', t2.credencial, ';APELLIDO P: ', t2.apPaterno, ';APELLIDO M: ', t2.apMaterno, ';nombre: ', t2.nombres, ';PUESTO: ', (select puesto FROM puestos WHERE idpuesto = t2.cargofkcargos))) FROM modificaciones_sistema  as t1 INNER JOIN cpersonal as t2 On t1.idregistro = t2.idpersona WHERE idmodificacion = '" + id + "'").ToString();
                        string[] datos = cadena.Split(';');
                        datos[0] = ("CREDENCIAL: " + datos[0]).ToUpper();
                        datos[1] = ("APELLIDO P. : " + datos[1]).ToUpper();
                        datos[2] = ("APELLIDO M. : " + datos[2]).ToUpper();
                        datos[3] = ("NOMBRE: " + datos[3]).ToUpper();
                        datos[4] = ("PUESTO: " + v.getaData("SELECT puesto FROM puestos WHERE idpuesto='" + datos[4] + "'").ToString()).ToUpper();
                        datos[5] = ("" + datos[5]).ToUpper();
                        datos[6] = ("" + datos[6]).ToUpper();
                        datos[7] = ("" + datos[7]).ToUpper();
                        datos[8] = ("" + datos[8]).ToUpper();
                        y1 = 250;

                        CrearAntesDespuesLabels();
                        y1 = 270;
                        mitad1mitad2(datos);
                        y1 = null;
                      

                        break;
                    case "Actualización de Usuario":
                        string[] actusuario = v.getaData(@"SET lc_time_names = 'es_ES';SELECT CONCAT(CONCAT('CREDENCIAL: ', t2.credencial, ';'),UPPER(CONCAT('NOMBRE: ',t2.nombres,' ',t2.apPaterno,' ', t2.apMaterno,';')),  UPPER(CONCAT('Puesto: ', t3.puesto,';')), t1.ultimamodificacion, (SELECT  CONCAT(upper(';Usuario Actual: '), usuario,';')  FROM  datosistema WHERE usuariofkcpersonal = t1.idregistro), (SELECT CONCAT(password) FROM datosistema WHERE usuariofkcpersonal = t1.idregistro),';USUARIO QUE MODIFICÓ: ',(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ', apMaterno)) FROM cpersonal WHERE idpersona= t1.usuariofkcpersonal),'; FECHA / HORA: ', UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'))) FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        actusuario[3] = "Usuario Anterior: ".ToUpper() + actusuario[3];
                        actusuario[4] = "Contraseña Anterior: ".ToUpper() + v.Desencriptar(actusuario[4]);
                        actusuario[6] = "Contraseña Actual: ".ToUpper() + v.Desencriptar(actusuario[6]);
                        crearFormularioPersonalcambios(actusuario);

                        break;
                    case "Desactivación de Empleado":
                    case "Reactivación de Empleado":
                    case "Desactivación":
                    case "Reactivación":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT( (SELECT concat('Credencial: ',credencial) FROM cpersonal WHERE idpersona=t1.idregistro),';',(SELECT CONCAT('Nombre: ',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t1.idregistro) ,';',(SELECT CONCAT('Cargo: ',t2.puesto) FROM cpersonal as t1 inner JOIN  puestos as t2 On t1.cargofkcargos=t2.idpuesto  WHERE idpersona=t1.idregistro),';', (SELECT CONCAT('Estatus Actual: ',if(status=1,'Activo', 'Inactivo')) FROM cpersonal WHERE idpersona=t1.idregistro),';',(SELECT  UPPER(CONCAT('NOMBRE: ',nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona=t1.usuariofkcpersonal),'; FECHA / HORA: ', UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')),UPPER( CONCAT(';Tipo de Modificación: ',t1.ultimamodificacion)))) AS M FROM modificaciones_sistema as t1 WHERE idmodificacion ='" + id + "'").ToString().Split(';'));
                        break;

                    case "Inserción de Empleado":
                        string[] empleado = v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT(ultimamodificacion,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',Tipo)) FROM modificaciones_sistema WHERE idmodificacion = '" + id + "'").ToString().Split(';');
                        empleado[0] = ("Credencial: " + empleado[0]).ToUpper();
                        empleado[1] = ("Apellido Paterno: " + empleado[1]).ToUpper();
                        empleado[2] = ("Apellido Materno: " + empleado[2]).ToUpper();
                        empleado[3] = ("Nombres: " + empleado[3]).ToUpper();
                        empleado[4] = ("Cargo: " + v.getaData("SELECT puesto From puestos WHERE idpuesto='" + empleado[4] + "'")).ToUpper();
                        if (empleado[5] == "") empleado[5] = null;
                        empleado[5] = ("Usuario: " + (empleado[5] ?? "\"Sin Usuario\"")).ToUpper();
                        if (empleado[6] == "") empleado[6] = null;
                        empleado[6] = ("Contraseña: " + (empleado[6] ?? "\"Sin Contraseña\"")).ToUpper();
                        crearCatalogoPuesto(empleado);
                        break;
                    case "Inserción de Usuario":
                        string[] dato = v.getaData("SET lc_time_names = 'es_ES';SELECT CONCAT(upper(CONCAT('Credencial: ', t2.credencial, ';','Nombre: ',  t2.nombres,  ';Apellido P.:',   t2.apPaterno,';Apellido M: ', t2.apMaterno,';Puesto: ', t3.puesto,';')),ultimamodificacion,UPPER(CONCAT('; Usuario que Modificó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) From CPERSONAL WHERE idpersona=t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) AS M FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        if (dato[5] != "") {
                            dato[5] = "USUARIO: " + dato[5];
                            dato[6] = "CONTRASEÑA: " + v.Desencriptar(dato[6]);
                        }
                        crearCatalogoPuesto(dato);
                        break;
                    case "Eliminación de Usuario":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT(CONCAT('Credencial: ', t2.credencial, ';'),CONCAT('Nombre: ',  t2.nombres,  ';Apellido P.:',   t2.apPaterno,';Apellido M: ', t2.apMaterno,   ';'),  CONCAT('Puesto: ', t3.puesto),';Usuario que Modificó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) From CPERSONAL WHERE idpersona=t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'))) AS M FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';'));

                        break;
                    case "Desactivación de Puesto":
                    case "Reactivación de Puesto":
                        string[] puesto = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Nombre de Puesto: ', t2.puesto, ';Usuario que Modificó: ', CONCAT(t3.nombres, ' ', t3.apPaterno, ' ', t3.apMaterno), ';Fecha: ', DATE_FORMAT(t1.fechaHora, '%W, %d de %M del %Y'), ';Hora: ', time(t1.fechaHora), ';Tipo: ', t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro = t2.idpuesto INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idpersona WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');

                        crearCatalogoPuesto(puesto);
                        break;
                    case "Actualización de Puesto":

                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(concat('Nombre de Puesto Anterior: ',t1.ultimaModificacion,';Nombre de Puesto Actual: ',t2.puesto,'; Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';FECHA / HORA: ', UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro = t2.idpuesto WHERE idmodificacion='" + id + "'").ToString().Split(';'));

                        break;
                    case "Inserción de Puesto":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT CONCAT('Nombre del Puesto: ',t2.puesto,';','Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),'; FECHA / HORA: ', UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'))) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro= t2.idpuesto WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Desactivación de Unidad":
                    case "Reactivación de Unidad":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT  UPPER(CONCAT('ECO: ', (SELECT concat(t22.identificador, LPAD(t12.consecutivo, 4, '0'), ';Empresa de Procedencia: ', t32.nombreEmpresa, ';') FROM cunidades as t12 INNER JOIN careas as t22 On t12.areafkcareas = t22.idarea INNER JOIN cempresas as t32 ON t22.empresafkcempresas = t32.idempresa WHERE t12.idunidad = t1.idregistro), 'Tipo de Modificación: ', t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),'; Usuario que Modificó: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),'; FECHA / HORA: ', UPPER(DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN cunidades as t2 On t1.idregistro = t2.idunidad WHERE idmodificacion = '" + id + "'; ").ToString().Split(';'));
                        break;
                    case "Inserción de Unidad":
                        crearFormularioPersonalcambios(v.getaData("SET lc_time_names='es_ES';SELECT CONCAT((SELECT CONCAT('ECO: ',concat(t2.identificador,LPAD(consecutivo,4,'0')),';Descripcion: ',t1.descripcioneco,';Empresa: ',t3.nombreEmpresa,';Area: ',t2.nombreArea,(select if(t1.serviciofkcservicios = 0, '',(select CONCAT(';Servicio: ',t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))) FROM cunidades  as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idArea INNER JOIN cempresas as t3 ON t2.empresafkcempresas= t3.idEmpresa WHERE idunidad=modsis.idregistro),';Usuario que Modificó: ',CONCAT(cp.nombres,' ',cp.apPaterno,' ',cp.apMaterno),';Fecha: ',DATE_FORMAT( modsis.fechaHora,'%W, %d de %M del %Y'),';Hora: ',time(modsis.fechaHora),';Tipo: ',modsis.Tipo) FROM modificaciones_sistema as modsis INNER JOIN cpersonal as cp ON modsis.usuariofkcpersonal=cp.idpersona where idmodificacion= '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Unidad":
                        if (empresa == 1) {
                            string[] unidad = v.getaData("SET lc_time_names='es_ES';select upper(CONCAT(t1.ultimamodificacion, (SELECT CONCAT(';ECONÓMICO: ', concat(t2.identificador, LPAD(consecutivo, 4, '0')), ';Descripción: ', t1.descripcioneco, ';Área: ', t3.nombreEmpresa, ' -> ', t2.nombreArea, (select if (t1.serviciofkcservicios = 0, '',(select UPPER(CONCAT(';Servicio: ', t22.Nombre)) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad = t1.idunidad)))) FROM cunidades  as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idArea INNER JOIN cempresas as t3 ON t2.empresafkcempresas = t3.idEmpresa WHERE idunidad = t1.idregistro),';Usuario que Modificó: ',upper(CONCAT(cp.nombres, ' ', cp.apPaterno, ' ', cp.apMaterno)),'; FECHA / HORA:', UPPER(DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN cpersonal as cp ON t1.usuariofkcpersonal = cp.idpersona WHERE idmodificacion = '" + id + "'").ToString().Split(';');
                            unidad[0] = "ECONÓMICO: " + v.getaData("SELECT concat(identificador, LPAD('" + unidad[0] + "', 4, '0')) from careas WHERE idarea='" + unidad[1] + "';");
                            unidad[1] = ("Área: " + v.getaData("SELECT concat(t2.nombreEmpresa,' -> ',t1.nombreArea) FROM careas as t1 INNER JOIN cempresas as t2 On t1.empresafkcempresas=t2.idempresa WHERE t1.idarea='" + unidad[1] + "'")).ToUpper();
                            string temp = unidad[1];
                            unidad[1] = ("DescripciÓn: " + unidad[2]).ToUpper();
                            unidad[2] = temp;
                            unidad[3] = ("Servicio: " + v.getaData("SELECT Nombre FROM cservicios where idservicio = '" + unidad[3] + "';")).ToUpper();
                            paralabelsImpares(unidad);
                            CrearAntesDespuesLabels();
                        } else
                        {
                            string[] unidadtri = v.getaData("SET lc_time_names='es_ES';select upper(concat('ECONÓMICO:',(SELECT concat(tt2.identificador, LPAD(tt1.consecutivo, 4, '0'),';',t1.ultimaModificacion,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';VIN:',t2.bin,';No. De Serie de Motor: ',t2.nmotor,';No. de Serie de Transmisión: ',t2.ntransmision,';Modelo: ',t2.modelo,';Marca: ',t2.marca,';Tipo: ',t1.Tipo,';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'),';Estatus Acual: ',if (t2.status = 1,'Activo','Inactivo')) from cunidades as tt1 INNER JOIN careas as tt2 On tt1.areafkcareas=tt2.idarea WHERE tt1.idunidad=t2.idunidad))) FROM modificaciones_sistema as t1 INNER JOIN cunidades as t2 on t1.idregistro =t2.idunidad WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                            unidadtri[1] = ("VIN: " + unidadtri[1]).ToUpper();
                            unidadtri[2] = ("No. Motor: " + unidadtri[2]).ToUpper();
                            unidadtri[3] = ("No. Transmisión: " + unidadtri[3]).ToUpper();
                            unidadtri[4] = ("Modelo: " + unidadtri[4]).ToUpper();
                            unidadtri[5] = ("Marca: " + unidadtri[5]).ToUpper();
                            paralabelsImpares(unidadtri);
                            CrearAntesDespuesLabels();
                        }
                        break;
                    case "Desactivación de Empresa":
                    case "Reactivación de Empresa":
                    case "Inserción de Empresa":
                        if (empresa==1 && area ==1) {
                            crearFormularioPersonalcambios(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Empresa: ', t2.nombreEmpresa, ';Usuario que Modificó: ', CONCAT(t3.nombres, ' ', t3.apPaterno, ' ', t3.apMaterno),';Tipo de Modificación: ', t1.Tipo, ';Estatus Actual: ', if (t2.status = 1,'Activo','Inactivo'),';FECHA / HORA: ',UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN cempresas as t2 on t1.idregistro = t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idpersona WHERE idmodificacion = '" + id + "' ").ToString().Split(';'));
                        }else
                        {
                            crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('Nombre de Empresa Anterior: ',t1.ultimamodificacion,';Nombre de Empresa Actual: ',t2.nombreEmpresa,';Usuario que modificó: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';FECHA / HORA: ',UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')),';Tipo de Modificación: ',t1.Tipo,';Estatus Actual: ', if(t2.status=1,'Activo','Inactivo'),';Logo:')) FROM  modificaciones_sistema as t1 INNER JOIN cempresas as t2 On t1.idregistro= t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion='" + id + "'").ToString().Split(';'));
                            string[] imagenes = v.getaData("SELECT CONCAT(COALESCE(logo,'')) AS M FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='" + id + "' ;").ToString().Split('*');
                            crearPicturesBox(imagenes);
                        }
                            break;
                    case "Actualización de Empresa":

                        if (empresa==1 && area==1) {
                            crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('Nombre de Empresa Anterior: ',t1.ultimamodificacion,';Nombre de Empresa Actual: ',t2.nombreEmpresa,';Usuario que modificó: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Fecha/Hora: ',';FECHA / HORA: ',UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')),';Tipo de Modificación: ',t1.Tipo,';Estatus Actual: ', if(t2.status=1,'Activo','Inactivo'))) FROM  modificaciones_sistema as t1 INNER JOIN cempresas as t2 On t1.idregistro= t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion='" + id + "'").ToString().Split(';'));
                        }else if (empresa == 2 && area==2)
                        {
                            crearCatalogoPuesto(v.getaData("SET lc_time_names ='es_ES';SELECT upper(CONCAT('Nombre de Empresa: ',nombreEmpresa,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal), ';Fecha / Hora',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %h:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='"+id+"' ;").ToString().Split(';'));
                            y1 = 250;
                            CrearAntesDespuesLabels();
                            string[] imagenes = v.getaData("SELECT CONCAT(COALESCE(ultimaModificacion,''),'*',COALESCE(logo,'')) AS M FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='" + id + "' ;").ToString().Split('*');
                            crearPicturesBox(imagenes);
                        }
                        break;
                    case "Desactivación de Area":
                    case "Reactivación de Area":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT upper(CONCAT('Empresa: ',(SELECT nombreEmpresa FROM cempresas WHERE idempresa=t2.empresafkcempresas),';Area: ',t2.nombreArea,';Usuario que Modificó: ', CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo de Modificación: ',t1.Tipo,';Estatus Actual: ', if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN careas as t2 On t1.idregistro=t2.idarea Inner join cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        break;
                    case "Inserción de Area":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT('Empresa: ', (SELECT nombreEmpresa FROM cempresas WHERE idempresa = t2.empresafkcempresas), ';Area: ', t2.nombreArea, ';Usuario que Insertó: ', CONCAT(t3.nombres, ' ', t3.apPaterno, ' ', t3.apMaterno), ';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'), ';Tipo de Modificación: ', t1.Tipo, ';Estatus Actual: ', if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN careas as t2 On t1.idregistro = t2.idarea INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idpersona WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Area":

                        string[] datosArea = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat(t1.ultimaModificacion,';Usuario que Modificó: ',cONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Tipo: ',t1.Tipo,';Empresa:',(SELECT nombreEmpresa FROM cempresas WHERE idempresa=t2.empresafkcempresas),';Identificador: ',t2.identificador,';Area Actual: ',t2.nombreArea,';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN careas as t2 On t1.idregistro=t2.idarea INNER JOIN cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion ='" + id + "' ;").ToString().Split(';');
                        datosArea[0] = ("Empresa: " + v.getaData("SELECT nombreEmpresa FROM cempresas where idempresa='" + datosArea[0] + "'")).ToUpper();
                        datosArea[1] = ("Identificador: " + datosArea[1]).ToUpper();
                        datosArea[2] = ("Nombre de Area: " + datosArea[2]).ToUpper();
                        crearFormularioPersonalcambios(datosArea);
                        CrearAntesDespuesLabels();
                        break;
                    case "Desactivación de Servicio":
                    case "Reactivación de Servicio":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Servicio: ', t2.nombre,';Descripción de Servicio: ',t2.descripcion,';Usuario que Modificó: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 On t1.idregistro = t2.idservicio INNER JOIN cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        break;
                    case "Inserción de Servicio":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Servicio: ', t2.Nombre,';Descripción de Servicio: ',t2.descripcion,';Usuario que Insertó: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Tipo: ',t1.Tipo,';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 On t1.idregistro=t2.idservicio INNER JOIN  cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Servicio":

                        paralabelsImpares(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat(t1.ultimaModificacion,';Usuario que Modificó: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Fecha: ',DATE_FORMAT( t1.fechaHora,'%W, %d de %M del %Y'),';Nombre: ',t2.Nombre,';Descripción: ',t2.Descripcion,';Hora: ',time(t1.fechaHora),';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 ON t1.idregistro=t2.idservicio INNER JOIN cpersonal AS t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        CrearAntesDespuesLabels();
                        break;

                    case "Exportación a PDF de reporte de supervisión":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.Folio,';',(SELECT concat('ECO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),'; Fecha: ', DATE_FORMAT(t2.FechaReporte, '%W, %d de %M del %Y'),';Supervisor: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.SupervisorfkCPersonal),';Conductor: ',(SELECT CONCAT(credencial,': ',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.CredencialConductorfkCPersonal),';Servicio: ',(select concat(nombre,': ',descripcion) FROM cservicios WHERE idservicio=t2.Serviciofkcservicios),';Hora de Entrada: ',t2.HoraEntrada,';Kilometraje: ',t2.KmEntrada,';Tipo de Fallo:',t2.TipoFallo,if((SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo) is null, Concat(';Fallo No Codificado: ',DescFalloNoCod), CONCAT(';Descripción de Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo),';Fallo: ',(select concat(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp))),';Observaciones: ',coalesce(if(LENGTH(t2.ObservacionesSupervision)>=25,CONCAT(SUBSTRING(t2.ObservacionesSupervision,1,25),'...'),ObservacionesSupervision),'null'),';Tipo: " + tipo.ToUpper() + "' )) FROM modificaciones_sistema as t1 INNER join reportesupervicion as t2 on t1.idregistro=t2.idreportesupervicion WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Exportación a Excel de reportes de supervisión":
                    case "Exportación a Excel de reportes de almacén":
                    case "Exportación a Excel de reportes de mantenimiento":
                    case "Exportación a Excel de ordenes de compra":
                        if (tipo != "Exportación a Excel de ordenes de compra")
                        {
                            sqlFolio = "SELECT folio FROM reportesupervicion WHERE idReporteSupervicion= ";
                        } else
                        {
                            sqlFolio = "SELECT FolioOrdCompra FROM ordencompra where idOrdCompra=";
                        }
                        crearListBox(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Usuario que Exportó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),'^Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%m:%s'), '^Tipo: ', Tipo, '^', ultimaModificacion)) FROM modificaciones_sistema WHERE idmodificacion = '" + id + "'; ").ToString().Split('^'));
                        sqlFolio = null;
                        break;
                    case "Actualización de Reporte de Supervisión":
                        string[] data = v.getaData("SET lc_time_names='es_ES';select upper(CONCAT('FOLIO: ',t2.folio,';',ultimaModificacion,';ECONÓMICO: ',concat(t4.identificador,LPAD(t3.consecutivo,4,'0')),';Supervisor: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.supervisorfkcpersonal),';Credencial :',(SELECT CONCAT(credencial,' - ',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idPersona=t2.CredencialConductorfkCPersonal),';Servicio: ',(SELECT Nombre FROM cservicios WHERE idservicio=t2.Serviciofkcservicios),';Hora de Entrada: ',t2.HoraEntrada,';Kilometraje: ', t2.kmEntrada,';Tipo de Fallo: ',t2.TipoFallo,if((SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo) is null, Concat(';Fallo No Codificado: ',DescFalloNoCod), CONCAT(';Descripción de Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo),';Fallo: ',(select concat(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp))),';Observaciones: ',coalesce(if(LENGTH(t2.ObservacionesSupervision)>=25,CONCAT(SUBSTRING(t2.ObservacionesSupervision,1,25),'...'),ObservacionesSupervision),'null')) ) as m FROM modificaciones_sistema as t1 INNER JOIN reportesupervicion as t2 ON t1.idregistro=t2.idreportesupervicion INNER JOIN cunidades as t3 On t2.unidadfkcunidades=t3.idunidad INNER JOIN careas as t4 ON t3.areafkcareas=t4.idarea WHERE idmodificacion= '" + id + "';").ToString().Split(';');
                        data[1] = ("Supervisor: " + data[1]).ToUpper();
                        data[2] = ("Credencial: " + data[2] + " - " + v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE credencial='" + data[2] + "'")).ToUpper();
                        data[3] = ("Servicio: " + data[3]).ToUpper();
                        data[4] = ("Kilometraje: " + data[4]).ToUpper();
                        data[5] = ("Tipo de Fallo: " + data[5]).ToUpper();
                        if (data.Length == 18)
                        {
                            data[6] = ("Fallo No Codificado: " + data[6]).ToUpper();
                            data[7] = ("Observaciones: " + data[7]).ToUpper();
                        }
                        else
                        {
                            data[6] = ("Descripción de Fallo: " + data[6]).ToUpper();
                            data[7] = ("Nombre de Fallo: " + data[7] + " - " + v.getaData("SELECT falloesp FROM cfallosesp WHERE codfallo='" + data[7] + "'")).ToUpper();
                            data[8] = ("Observaciones: " + data[8]).ToUpper();
                        }
                        paralabelsImpares(data);

                        CrearAntesDespuesLabels();
                        break;
                    case "Inserción de Clasificación":
                    case "Desactivación de Clasificación":
                    case "Reactivación de Clasificación":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Fallo: ',t2.nombreFalloGral,';Usuario que Insertó: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) AS M FROM sistrefaccmant.modificaciones_sistema AS t1 INNER JOIN cfallosgrales AS t2 ON t1.idregistro = t2.idfallogral WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        break;

                    case "Actualización de Clasificación":
                        mitad1mitad(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Fallo: ',t1.ultimaModificacion,';Usuario que Modificó: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'),';Clasificación de Fallo: ',t2.nombreFalloGral,';Tipo: ',t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) AS M FROM sistrefaccmant.modificaciones_sistema AS t1 INNER JOIN cfallosgrales AS t2 ON t1.idregistro = t2.idfallogral WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        CrearAntesDespuesLabels();
                        break;
                    case "Inserción de Descripción":
                    case "Desactivación de Descripción":
                    case "Reactivación de Descripción":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Fallo: ',(SELECT nombreFalloGral from cfallosgrales WHERE idfallogral = t2.falloGralfkcfallosgrales),';Descripción de Fallo: ',t2.descfallo,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo')))  FROM modificaciones_sistema as t1 INNER JOIN cdescfallo as t2 On t1.idregistro=t2.iddescfallo WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Descripción":
                        string[] datoss = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimaModificacion, ';Usuario que Modificó: ', (SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal), ';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'), ';Clasificación de Fallo: ', (SELECT nombreFalloGral from cfallosgrales WHERE idfallogral = t2.falloGralfkcfallosgrales), ';Descripción de Fallo: ', t2.descfallo, ';Tipo: ', t1.Tipo, ';Estatus Acual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cdescfallo as t2 On t1.idregistro = t2.iddescfallo WHERE idmodificacion = '" + id + "'; ").ToString().Split(';');
                        datoss[0] = ("Clasificación de Fallo: " + v.getaData("SELECT nombrefalloGral From cfallosgrales Where idfallogral='" + datoss[0] + "'")).ToUpper();
                        datoss[1] = ("Descripción de Fallo: " + datoss[1]).ToUpper();
                        mitad1mitad(datoss);
                        CrearAntesDespuesLabels();
                        break;
                    case "Inserción de Nombre de Fallo":
                    case "Desactivación de Nombre de Fallo":
                    case "Reactivación de Nombre de Fallo":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT('Clasificación de Fallo: ', (SELECT CONCAT((SELECT nombrefallogral From cfallosgrales where idfallogral = x1.fallogralfkcfallosgrales), ';Descripción de Fallo: ', x1.descfallo) FROM cdescfallo as x1 WHERE x1.iddescfallo = t2.descfallofkcdescfallo),';Código de Fallo: ',t2.codfallo,'; Nombre de Fallo: ',falloesp,';Usuario que Modificó: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cfallosesp as t2 On t1.idregistro = t2.idfalloesp WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Nombre de Fallo":
                        string[] nombrefallo = v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT(t1.ultimaModificacion,';Usuario que Modificó ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';',(SELECT CONCAT('Descripción de Fallo: ',x1.descfallo) FROM cdescfallo as x1 WHERE x1.iddescfallo=t2.descfallofkcdescfallo),';Código de Fallo: ',t2.codfallo,'; Nombre de Fallo: ',falloesp,';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cfallosesp as t2 On t1.idregistro= t2.idfalloesp WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        nombrefallo[0] = ("Descripción de Fallo: " + v.getaData("SELECT CONCAT(t1.descfallo) FROM cdescfallo as t1 INNER JOIN cfallosgrales as t2 On t1.fallogralfkcfallosgrales=t2.idfallogral WHERE t1.iddescfallo='" + nombrefallo[0] + "'")).ToUpper();
                        nombrefallo[1] = ("Código de Fallo: " + nombrefallo[1]).ToUpper();
                        nombrefallo[2] = ("Nombre de Fallo: " + nombrefallo[2]).ToUpper();
                        mitad1mitad(nombrefallo);
                        CrearAntesDespuesLabels();
                        break;
                    case "Inserción de Proveedor":
                 
                    case "Desactivación de Proveedor":
                    case "Reactivación de Proveedor":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT CONCAT(upper(concat('Empresa: ',(select t1.empresa))), CONCAT(';PÁGINA WEB: ',(select coalesce(t1.paginaweb,''))), upper(concat(';Clasificación: ',(select giro from cgiros as x1 where x1.idgiro=t1.Clasificacionfkcgiros),';Telefonos de empresa: ',( CONCAT(COALESCE((SELECT CONCAT('(+', t33.phonecode, ')', t1.telefonoEmpresaUno, IF(t1.ext1 is null, '', CONCAT(' Ext. ', t1.ext1))) FROM cladas AS t33 WHERE t1.idlada = t33.id), ''), COALESCE((SELECT CONCAT('  (+', t3.phonecode, ')', t1.telefonoEmpresaDos, IF(t1.ext2 is null, '', CONCAT(' Ext. ', t1.ext2))) FROM cladas AS t3 WHERE t1.idladados = t3.id), ''))),';Observaciones: ',(select coalesce(t1.observaciones,'')),';Domicilio: ',(select concat('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id=t1.domiciliofksepomex ),';Persona de contacto: ',concat(t1.aPaterno,' ',t1.aMaterno,' ',t1.nombres))),CONCAT(';CORREO ELECTRÓNICO: ',coalesce(t1.correo,'')),UPPER(CONCAT(';Telefonos de contacto: ',( CONCAT(COALESCE((SELECT CONCAT('(+', t33.phonecode, ')', t1.TelefonoContactoUno, IF(t1.ext3 is null, '', CONCAT(' Ext. ', t1.ext3))) FROM cladas AS t33 WHERE t1.idladatres = t33.id), ''), COALESCE((SELECT CONCAT('  (+', t3.phonecode, ')', t1.TelefonoContactoDos, IF(t1.ext4 is null, '', CONCAT(' Ext. ', t1.ext4))) FROM cladas AS t3 WHERE t1.idladacuatro = t3.id), ''))),';Estatus Actual: ',if (t1.status = 1,'Activo','Inactivo'),';Tipo: ',t2.Tipo))) from cproveedores as t1 inner join modificaciones_sistema as t2 on t2.idregistro=t1.idproveedor where idmodificacion='"+id+"'; ").ToString().Split(';'));
                        break;
                
                    
                    case "Actualización de Proveedor":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Usuario que Actualizó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo'))) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal))) FROM modificaciones_sistema as t1 INNER JOIN cproveedores as t2 ON t1.idregistro=t2.idproveedor WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        string[] actuprov = v.getaData("SET lc_time_names = 'es_ES'; SELECT CONCAT(t1.ultimaModificacion, UPPER(CONCAT(';Empresa: ', t2.empresa)), concat(';PÁGINA WEB: ', t2.paginaweb), upper(concat(';clasificación: ', (select giro from cgiros as x1 where x1.idgiro = t2.Clasificacionfkcgiros), ';Telefonos de empresa: ', (CONCAT(COALESCE((SELECT CONCAT('(+', t33.phonecode, ')', t2.telefonoEmpresaUno, IF(t2.ext1 is null, '', CONCAT(' Ext. ', t2.ext1))) FROM cladas AS t33 WHERE t2.idlada = t33.id), ''), COALESCE((SELECT CONCAT('  (+', t3.phonecode, ')', t2.telefonoEmpresaDos, IF(t2.ext2 is null, '', CONCAT(' Ext. ', t2.ext2))) FROM cladas AS t3 WHERE t2.idladados = t3.id), ''))),';Observaciones: ',coalesce(t2.observaciones, ''),';Domicilio:',(select concat('Calle: ', t2.calle, ', Número: ', t2.Numero, ', ', x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id = t2.domiciliofksepomex ))),';PERSONA DE CONTACTO: ',concat(t2.nombres, ' ', t2.apaterno, ' ', t2.aMaterno),concat(';CORREO ELECTRÓNICO: ', coalesce(t2.correo, ''))) AS cs FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cproveedores as t2 On T1.idregistro = t2.idproveedor WHERE idmodificacion = '"+id+"'; ").ToString().Split(';');
                        actuprov[0] = UPPER("EMPRESA: "+actuprov[0]);
                        actuprov[1] = UPPER("Pagina Web: ") +actuprov[1];
                        actuprov[2] = UPPER("Clasificación: "+v.getaData("SELECT giro FROM cgiros WHERE idgiro='"+actuprov[2]+"'"));
                        actuprov[3] = UPPER("Teléfono: "+actuprov[3]);
                        actuprov[4] = UPPER("Observaciones: "+ actuprov[4]);
                        actuprov[5] = UPPER("Domicilio: "+actuprov[5]);
                        actuprov[6] = UPPER("Nombre del Contacto: "+actuprov[6]);
                        actuprov[7] = UPPER("Correo Electrónico: ") +actuprov[7];
                        y1 = 180;
                        CrearAntesDespuesLabels();
                        y1 = 180;
                        mitad1mitad2(actuprov);
                        y1 = null;
                        break;
                    case "Desactivación de Refacción":
                    case "Reactivación de Refacción":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 650); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Código de Refacción: ',t2.codrefaccion,';Nombre de Refacción: ',t2.nombreRefaccion,';Modelo de Refacción: ',t2.modeloRefaccion,';Familia: ',(SELECT familia FROM cfamilias WHERE idfamilia=t2.familiafkcfamilias),';Unidad de Medida: ',(SELECT CONCAT(Nombre,'(',Simbolo,')') FROM cunidadmedida WHERE idunidadmedida=t2.umfkcunidadmedida),';Ubicacion: ',(SELECT CONCAT((SELECT CONCAT((SELECT CONCAT('Pasillo: ',pasillo) FROM cpasillos WHERE idpasillo=pasillofkcpasillos),',Anaquel: ',anaquel) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles),',Charola: ',charola) FROM ccharolas WHERE idcharola=t2.charolafkcharolas),';Marca: ',(SELECT marca FROM cmarcas WHERE idmarca = t2.marcafkcmarcas),';Nivel: ',if(t2.nivel='1','Superior',if(t2.nivel='2','Inferior',if(t2.nivel='3','Izquierdo',if(t2.nivel='4','Derecho','')))),';Descripción de Refacción: ',COALESCE(t2.descripcionRefaccion,'\"Sin Descripción\"'),';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN crefacciones as t2 On t1.idregistro=t2.idrefaccion WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Inserción de Refacción":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 750); CenterToParent(); acomodarLabel();
                        }
                        string[] ress = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Usuario Que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ', DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from crefacciones WHERE idrefaccion=idregistro),';Tipo: ',Tipo)) FROM modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        ress[0] = ("Código de Refacción: " + ress[0]).ToUpper();
                        ress[1] = ("Nombre de Refacción: " + ress[1]).ToUpper();
                        ress[2] = ("Modelo de Refacción: " + ress[2]).ToUpper();
                        ress[3] = ("Próximo Abastecimiento: " + DateTime.Parse(ress[3]).ToString("dddd, dd MMMM yyyy")).ToUpper();
                        ress[4] = ("Familia: " + v.getaData("SELECT familia FROM cfamilias WHERE idfamilia='" + ress[4] + "'")).ToUpper();
                        ress[5] = ("Unidad de Medida: " + v.getaData("SELECT CONCAT(Nombre,'(',Simbolo,')') FROM cunidadmedida WHERE idunidadmedida='" + ress[5] + "'")).ToUpper();
                        ress[6] = ("Marca: " + v.getaData("SELECT marca FROM cmarcas WHERE idmarca = '" + ress[6] + "'")).ToUpper();
                        ress[7] = ("Nivel: " + v.getNivelFromID(Convert.ToInt32(ress[7]))).ToUpper();
                        ress[8] = ("Ubicación: " + v.getaData("SELECT CONCAT((SELECT CONCAT((SELECT CONCAT('Pasillo: ',pasillo) FROM cpasillos WHERE idpasillo=pasillofkcpasillos),',Anaquel: ',anaquel) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles),',Charola: ',charola) FROM ccharolas WHERE idcharola='" + ress[8] + "'")).ToUpper();
                        ress[9] = ("Cantidad Que Ingresó a Almacén: " + ress[9]).ToUpper();
                        ress[10] = "NOTIFICACIÓN DE MEDIA: " + ress[10];
                        ress[11] = "NOTIFICACIÓN DE ABASTECIMIENTO: " + ress[11];
                        if (ress[12] == "") ress[12] = null;
                        ress[12] = ("Descripción de Refacción: " + (ress[12] ?? "\"Sin Descripción\"")).ToUpper();
                        crearCatalogoPuesto(ress);
                        break;
                    case "Actualización de Refacción":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 750); CenterToParent(); acomodarLabel();
                        }
                        string[] resul = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Tipo: ',t1.Tipo,';Código de Refacción: ',t2.codrefaccion,';Nombre de Refacción: ',t2.nombreRefaccion,';Modelo de Refacción: ',t2.modeloRefaccion,';Próximo Abastecimiento: ',DATE_FORMAT(t2.proximoAbastecimiento,'%W, %d de %M del %Y'),';Familia: ',(SELECT familia FROM cfamilias WHERE idfamilia=t2.familiafkcfamilias),';Unidad de Medida: ',(SELECT CONCAT(Nombre,'(',Simbolo,')') FROM cunidadmedida WHERE idunidadmedida=t2.umfkcunidadmedida),';Marca: ',(SELECT marca FROM cmarcas WHERE idmarca = t2.marcafkcmarcas),';Nivel: ',if(t2.nivel='1','Superior',if(t2.nivel='2','Inferior',if(t2.nivel='3','Izquierdo',if(t2.nivel='4','Derecho','')))),';Ubicacion: ',(SELECT CONCAT((SELECT CONCAT((SELECT CONCAT('Pasillo: ',pasillo) FROM cpasillos WHERE idpasillo=pasillofkcpasillos),',Anaquel: ',anaquel) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles),',Charola: ',charola) FROM ccharolas WHERE idcharola=t2.charolafkcharolas),';Notificación de Media',t2.media,';Notificación de Abastecimiento: ',t2.abastecimiento,';Descripción de Refacción: ',COALESCE(t2.descripcionRefaccion,'\"Sin Descripción\"'),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN crefacciones as t2 ON t1.idregistro=t2.idrefaccion WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        resul[0] = ("Código de Refacción: " + resul[0]).ToUpper();
                        resul[1] = ("Nombre de Refacción: " + resul[1]).ToUpper();
                        resul[2] = ("Modelo de Refacción: " + resul[2]).ToUpper();
                        resul[3] = ("Próximo Abastecimiento: " + DateTime.Parse(resul[3]).ToString("dddd, dd " + 'D' + "e MMMM " + 'D' + "el yyyy")).ToUpper();
                        resul[4] = ("Familia: " + v.getaData("SELECT familia FROM cfamilias WHERE idfamilia='" + resul[4] + "'")).ToUpper();
                        resul[5] = ("Unidad de Medida: " + v.getaData("SELECT CONCAT(Nombre,'(',Simbolo,')') FROM cunidadmedida WHERE idunidadmedida='" + resul[5] + "'")).ToUpper();
                        resul[6] = ("Marca: " + v.getaData("SELECT marca FROM cmarcas WHERE idmarca = '" + resul[6] + "'")).ToUpper();
                        resul[7] = ("Nivel: " + v.getNivelFromID(Convert.ToInt32(resul[7]))).ToUpper();
                        resul[8] = ("Ubicación: " + v.getaData("SELECT CONCAT((SELECT CONCAT((SELECT CONCAT('Pasillo: ',pasillo) FROM cpasillos WHERE idpasillo=pasillofkcpasillos),',Anaquel: ',anaquel) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles),',Charola: ',charola) FROM ccharolas WHERE idcharola='" + resul[8] + "'")).ToUpper();
                        resul[9] = "NOTIFICACIÓN DE MEDIA: " + resul[10];
                        resul[10] = "NOTIFICACIÓN DE ABASTECIMIENTO: " + resul[11];
                        if (resul[11] == "") resul[11] = null;
                        resul[11] = ("Descripción de Refacción: " + (resul[11] ?? "\"Sin Descripción\"")).ToUpper();
                        mitad1mitad(resul);
                        CrearAntesDespuesLabels();
                        break;
                    case "Desactivación de Marca":
                    case "Reactivación de Marca":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Marca: ',t2.marca,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cmarcas as t2 ON t1.idregistro=t2.idmarca WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Inserción de Marca":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Marca: ',UltimaModificacion,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cmarcas WHERE idmarca=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Marca":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Marca Anterior: ', t1.ultimaModificacion, ';Nombre de Marca Actual: ', t2.marca, ';Usuario que Modificó: ', (SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal), ';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'), ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cmarcas as t2 ON t1.idregistro = t2.idmarca WHERE idmodificacion = '" + id + "'; ").ToString().Split(';'));
                        break;
                    case "Inserción de Unidad de Medida":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 350); CenterToParent(); acomodarLabel();
                        }
                        string[] uno = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(UltimaModificacion,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cunidadmedida WHERE idunidadmedida=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        uno[0] = UPPER("Nombre de Unidad de Medida: " + uno[0]);
                        uno[1] = UPPER("Simbolo: " + uno[1]);
                        crearCatalogoPuesto(uno);
                        break;
                    case "Desactivación de Unidad de Medida":
                    case "Reactivación de Unidad de Medida":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Unidad de Medida: ',t2.Nombre,';Símbolo: ',t2.Simbolo,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cunidadmedida as t2 ON t1.idregistro=t2.idunidadmedida WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;

                    case "Actualización de Unidad de Medida":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        string[] dos = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Nombre de Unidad de Medida Actual: ',t2.Nombre,';Símbolo Actual: ',t2.Simbolo,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cunidadmedida as t2 ON t1.idregistro=t2.idunidadmedida WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        dos[0] = UPPER("Nombre de Unidad de Medida Anterior: " + dos[0]);
                        dos[1] = UPPER("Simbolo Anterior: " + dos[1]);
                        crearCatalogoPuesto(dos);
                        break;
                    case "Desactivación de Familia":
                    case "Reactivación de Familia":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Familia: ',t2.familia,';Descripción: ',t2.descripcionFamilia,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cfamilias as t2 ON t1.idregistro=t2.idfamilia WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Inserción de Familia":
                        string[] sdf = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(UltimaModificacion,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cunidadmedida WHERE idunidadmedida=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        sdf[0] = UPPER("Nombre de Familia: " + sdf[0]);
                        sdf[1] = UPPER("DESCRIPCION: " + sdf[1]);
                        crearCatalogoPuesto(sdf);
                        break;
                    case "Actualización de Familia":
                        string[] tres = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Nombre de Familia Actual: ',t2.familia,';Descripción Actual: ',t2.descripcionfamilia,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cfamilias t2 ON t1.idregistro=t2.idfamilia WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        tres[0] = UPPER("Nombre de Familia Anterior: " + tres[0]);
                        tres[1] = UPPER("Descrpición Anterior: " + tres[1]);
                        crearCatalogoPuesto(tres);
                        break;
                    case "Desactivación de Pasillo":
                    case "Reactivación de Pasillo":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t2.pasillo,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cpasillos as t2 ON t1.idregistro=t2.idpasillo WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Inserción de Pasillo":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',UltimaModificacion,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cpasillos WHERE idpasillo=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Pasillo":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo Anterior: ', t1.ultimaModificacion, ';Pasillo Actual: ', t2.pasillo, ';Usuario que Modificó: ', (SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal), ';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'), ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cpasillos as t2 ON t1.idregistro = t2.idpasillo WHERE idmodificacion = '" + id + "'; ").ToString().Split(';'));
                        break;
                    case "Desactivación de Anaquel":
                    case "Reactivación de Anaquel":
                    case "Inserción de Anaquel":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        string[] cuatro = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t4.pasillo,' - Nivel: ',t3.nivel,' - Anaquel: ',t2.anaquel,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from canaqueles WHERE idanaquel=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN canaqueles AS t2 On t1.idregistro=t2.idanaquel INNER JOIN cniveles as t3 ON t2.nivelfkcniveles=t3.idnivel INNER JOIN cpasillos as t4 On t3.pasillofkcpasillos=t4.idpasillo WHERE idmodificacion='"+id+"';").ToString().Split(';');
                 
                    
                        crearCatalogoPuesto(cuatro);
                        break;
                    case "Inserción de Nivel":
                    case "Reactivación de Nivel":
                    case "Desactivación de Nivel":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t4.pasillo,' - Nivel: ',t3.nivel,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t3.status = 1,'Activo','Inactivo'),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cniveles AS t3 On t1.idregistro=t3.idnivel INNER JOIN cpasillos as t4 On t3.pasillofkcpasillos=t4.idpasillo WHERE idmodificacion='"+id+"';").ToString().Split(';'));

                        break;
                    case "Actualización de Anaquel":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        string[] cinco = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(UltimaModificacion,';Pasillo Actual: ',(SELECT pasillo FROM cpasillos WHERE idpasillo=t2.pasillofkcpasillos),';Anaquel Actual: ',t2.anaquel,';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from canaqueles WHERE idanaquel=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN canaqueles as t2 ON t1.idregistro=t2.idanaquel WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        cinco[0] = UPPER("Pasillo Anterior: " + cinco[0]);
                        cinco[1] = UPPER("Anaquel Anterior: " + cinco[1]);
                        crearCatalogoPuesto(cinco);
                        break;
                   
                
                    case "Inserción de Ubicación":
                        string[] seis = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t5.pasillo,' - Nivel: ',t4.nivel,' - Anaquel: ',t3.anaquel,' - Charola: ',t2.charola,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona =t1. usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ccharolas as t2 ON t1.idregistro=t2.idcharola INNER JOIN canaqueles as t3 ON t2.anaquelfkcanaqueles = t3.idanaquel INNER JOIN cniveles as t4 ON t3.nivelfkcniveles=t4.idnivel INNER JOIN cpasillos as t5 ON  t4.pasillofkcpasillos = t5.idpasillo WHERE idmodificacion='"+id+"';").ToString().Split(';');
                       
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(seis);
                        break;
                    case "Actualización de Ubicación":
                        string[] siete = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.UltimaModificacion,';Pasillo - Anaquel Actual: ',(SELECT CONCAT((SELECT pasillo FROM cpasillos WHERE idpasillo = pasillofkcpasillos),' - ',anaquel) FROM canaqueles WHERE idanaquel=t2.anaquelfkcanaqueles),';Charola: ',t2.charola,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from ccharolas WHERE idcharola=t1.idregistro),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ccharolas as t2 On t1.idregistro=t2.idcharola WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        siete[0] = UPPER("Pasillo - Anaquel ANTERIOR: " + v.getaData("SELECT (SELECT pasillo FROM cpasillos WHERE idpasillo = pasillofkcpasillos) FROM canaqueles WHERE idanaquel='" + siete[0] + "'") + " - " + siete[0]);
                        siete[1] = UPPER("Charola: " + siete[1]);
                        crearCatalogoPuesto(siete);
                        break;
                    case "Desactivación de Ubicación":
                    case "Reactivación de Ubicación":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo - Anaquel: ',(SELECT CONCAT((SELECT pasillo FROM cpasillos WHERE idpasillo = pasillofkcpasillos),' - ',anaquel) FROM canaqueles WHERE idanaquel=t2.anaquelfkcanaqueles),';Charola: ',t2.charola,';Usuario que Insertó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from ccharolas WHERE idcharola=t1.idregistro),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ccharolas as t2 On t1.idregistro=t2.idcharola WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        break;
                    case "Inserción de reporte de almacén":
                    case "Validación De Refacciones":
                    case "Exportación a PDF de reporte de almacén":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = (this.Size + new Size(0, 150)); CenterToParent(); acomodarLabel();
                        } else
                        {
                            if (Size.Height < 500) {
                                this.Size = (this.Size + new Size(0, 100));
                                gbadd.Size = (gbadd.Size + new Size(0, 100));
                            }
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',T3.Folio,';UNIDAD: ',(SELECT concat('ECO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Fecha De Solicitud de Refacción: ',DATE_FORMAT(t4.FechaReporteM,'%W, %d de %M del %Y'),';Mecánico Que Solicita La Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t4.MecanicofkPersonal),';Folio de Factura Emitido Por Mantenimiento: ',if(t4.FolioFactura ='','Sin Folio de Factura Aún',t4.FolioFactura),';Folio de Factura Emitido Por Almacén: ',t2.FolioFactura,';Fecha de Entrega de Refacción: ',DATE_FORMAT(t2.FechaEntrega,'%W, %d de %M del %Y'),';Persona Que Entregó la Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.PersonaEntregafkcpersonal),';Observaciones: ',if(t2.ObservacionesTrans='','\"Sin Observaciones\"',t2.ObservacionesTrans)))AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion =t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        crearDataGrid((DataTable)v.getData("SELECT(SELECT codrefaccion FROM crefacciones WHERE idrefaccion = t3.RefaccionfkCRefaccion) as 'Código de Refacción', (SELECT nombreRefaccion FROM crefacciones WHERE idrefaccion = t3.RefaccionfkCRefaccion) as 'Nombre de Refacción', t3.Cantidad as 'Cantidad Solicitada', COALESCE(t3.CantidadEntregada,'0') as 'Cantidad Entregada', COALESCE(t3.EstatusRefaccion,'SIN EXISTENCIA') as 'Estatus De Refacción', COALESCE((t3.Cantidad - t3.CantidadEntregada),Cantidad) as 'Cantidad Faltante' FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro = t2.idReporteTransinsumos INNER JOIN pedidosrefaccion as t3 ON t3.FolioPedfkSupervicion = t2.idreportemfkreportemantenimiento WHERE idmodificacion = '" + id + "'; "), new Point(0, 400));
                        break;
                    case "Actualización de Reporte de Almacén":
                        string[] nueve = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Folio de Factura: ',t2.FolioFactura,';Persona Que Entregó la Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.PersonaEntregafkcpersonal),';Observaciones: ',t2.ObservacionesTrans,'\"Sin Observaciones\"')) AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion =t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        string[] diez = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',T3.Folio,';UNIDAD: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Fecha de Solicitud de Refaccion: ',t4.FechaReporteM)) AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion =t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        crearCatalogoPuesto(diez);
                        nueve[0] = UPPER("Folio de Factura: " + nueve[0]);
                        nueve[1] = UPPER("Persona que Entregó la Refacción: " + (v.getaData("(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = '" + nueve[1] + "')") ?? nueve[1]));
                        nueve[2] = UPPER("oBSERVACIONES DE SUPervisión: " + nueve[2]);
                        y1 = 200;
                        CrearAntesDespuesLabels();
                        y1 = 250;
                        mitad1mitad2(nueve);
                        y1 = null;
                        break;
                    case "Exportación a PDF de reporte en Mantenimiento":
                        string[] once = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('folio: ',t2.Folio,';Unidad: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),';Fecha de Reporte: ',DATE_FORMAT(t2.FechaReporte,'%W, %d de %M del %Y'),';Estatus Del Mantenimiento: ',t1.Estatus,';Código de Fallo: ',(SELECT CONCAT(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp),';Fecha de Reporte de Mantenimiento: ',DATE_FORMAT(t1.FechaReporteM,'%W, %d de %M del %Y'),';Mecánico: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal),';Mecánico de Apoyo: ',if(t1.MecanicoApoyofkPersonal is null,'\"Sin Mecánico de Apoyo\"',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal)),';Supervisor: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.SupervisorfkCPersonal),';Hora de Entrada: ',t2.HoraEntrada,';Tipo de Fallo: ',t2.TipoFallo,if(t2.DescrFallofkcdescfallo is null,CONCAT(';Descripción de Fallo No Codificado:',t2.DescFalloNoCod),CONCAT('DeSCRIPCIÓN DE Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo))),';Observaciones de Supervisión: ',t2.ObservacionesSupervision,';')) FROM modificaciones_sistema as mega INNER JOIN reportesupervicion as t2 ON mega.idregistro = t2.idreportesupervicion INNER JOIN sistrefaccmant.reportemantenimiento as t1 on t2.idreportesupervicion = t1.FoliofkSupervicion  WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        crearCatalogoPuesto(once);
                        break;
                    case "Actualización de Reporte de Mantenimiento":
                        string[] doce = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t3.Folio,';Económico: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportemantenimiento as t2 ON t1.idregistro=t2.IdReporte INNER JOIN reportesupervicion as t3 ON t2.FoliofkSupervicion=t3.idreportesupervicion WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        string[] trece = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimaModificacion,';Clasificación de Fallo: ',(SELECT nombreFalloGral FROM cfallosgrales WHERE idfallogral=t2.FalloGralfkFallosGenerales),';Folio de Factura: ',t2.FolioFactura,';Trabajo Realizado: ',t2.TrabajoRealizado,';Observaciones: ',t2.ObservacionesM)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportemantenimiento as t2 ON t1.idregistro=t2.IdReporte INNER JOIN reportesupervicion as t3 ON t2.FoliofkSupervicion=t3.idreportesupervicion WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        trece[0] = UPPER("Clasificación de Fallo: " + v.getaData("SELECT nombreFalloGral FROM cfallosgrales WHERE idfallogral='" + trece[0] + "'"));
                        trece[1] = UPPER("Folio de Factura: " + trece[1]);
                        trece[2] = UPPER("Trabajo Realizado: " + trece[2]);
                        trece[3] = UPPER("Observaciones: " + trece[3]);

                        y1 = 200;
                        crearCatalogoPuesto(doce);
                        CrearAntesDespuesLabels();
                        y1 = 250;
                        mitad1mitad2(trece);
                        y1 = null;
                        break;
                    case "Exportación a PDF de orden de compra de almacen":
                        y1 = 200;
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 700); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';Proveedor: ',(SELECT empresa FROM cproveedores WHERE idproveedor=t2.ProveedorfkCProveedores),';Empresa a Facturar: ',(select nombreEmpresa FROM cempresas WHERE idempresa=t2.FacturadafkCEmpresas),';Fecha de Orden de Compra: ',DATE_FORMAT(t2.FechaOCompra,'%W, %d de %M del %Y'),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';IVA: ',t2.IVA,';Estatus: ',t2.Estatus)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        y1 = 500;
                        crearDataGrid((DataTable)v.getData("SET lc_time_names='es_ES';SELECT NumRefacc as 'Núm. Refaccion',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=ClavefkCRefacciones) as 'Refacción',Cantidad,Precio,Total,ObservacionesRefacc as 'Observaciones' FROM sistrefaccmant.detallesordencompra WHERE OrdfkOrdenCompra='" + v.getaData("SELECT idregistro FROM modificaciones_sistema WHERE idmodificacion='" + id + "'") + "';"), new Point(0, 300));
                        break;
                    case "Actualización de Orden de Compra":
                        y1 = 250;
                        string[] catorce = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';Fecha de Orden de Compra: ',DATE_FORMAT(t2.FechaOCompra,'%W, %d de %M del %Y'),';IVA: ',t2.IVA,';Estatus: ',COALESCE(t2.Estatus,'EN PROCESO'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        string[] quince = v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT(t1.ultimaModificacion,';Proveedor: ',(SELECT empresa FROM cproveedores WHERE idproveedor=t2.ProveedorfkCProveedores),';Empresa a Facturar: ',(select nombreEmpresa FROM cempresas WHERE idempresa=t2.FacturadafkCEmpresas),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';Observaciones: ',t2.ObservacionesOC))FROM modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        quince[0] = UPPER("Proveedor: " + v.getaData("Select empresa From cproveedores WHERE idproveedor='" + quince[0] + "'"));
                        quince[1] = UPPER("Empresa a Facturar " + v.getaData("SELECT nombreEmpresa FROM cempresas WHERE idempresa='" + quince[1] + "'"));
                        quince[2] = UPPER("FECHA DE ENTREGA APROXIMADA: "+ DateTime.Parse(quince[2]).ToString("dddd, MMMM dd yyyy"));
                        quince[3] = UPPER("Observaciones: " + quince[3]);
                        y1 = 300;
                        crearCatalogoPuesto(catorce);
                        CrearAntesDespuesLabels();
                        y1 = 310;
                        mitad1mitad2(quince);
                        y1 = null;
                        break;
                    case "Actualización de Refacción de Orden de Compra":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600); CenterToParent(); acomodarLabel();
                        }
                        string[] diesciseis = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';Fecha de Orden de Compra: ',DATE_FORMAT(t2.FechaOCompra,'%W, %d de %M del %Y'),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';IVA: ',CONCAT(COALESCE(t2.IVA,'0.0'),'%'),';Estatus: ',COALESCE(t2.Estatus,''))) FROM modificaciones_sistema as t1 INNER JOIN detallesordencompra as det ON t1.idregistro=det.idDetOrdCompra INNER JOIN ordencompra as t2 ON det.OrdfkOrdenCompra = t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        string[] diescisiete = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Refacción: ',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=ClavefkCRefacciones),';Precio: ',COALESCE(Precio,'0.0'),';Cantidad: ',Cantidad,';SubTotal: ',COALESCE(Total,'0.0'),';Observaciones: ',COALESCE(ObservacionesRefacc,''))) FROM modificaciones_sistema as t1 INNER JOIN detallesordencompra AS t2 On t1.idregistro=t2.idDetOrdCompra WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        float var = float.Parse(diescisiete[1]);
                        var = var * float.Parse(diescisiete[2]);
                        diescisiete[0] = UPPER("Refacción: " + v.getaData("SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion='" + diescisiete[0] + "'"));
                        diescisiete[1] = UPPER("Precio: " + diescisiete[1]);
                        diescisiete[2] = UPPER("Cantidad: " + diescisiete[2]);
                        diescisiete[3] = UPPER("Subtotal: " + var);
                        diescisiete[4] = UPPER( diescisiete[4]);
                        y1 = 250;
                      crearCatalogoPuesto(diesciseis);
                        CrearAntesDespuesLabels();
                        y1 = 300;
                        mitad1mitad2(diescisiete);
                        y1 = null;
                        break;
                    case "Actualización de Refacción en Reporte de Mantenimiento":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600); CenterToParent(); acomodarLabel();
                        }

                        y1 = 250; crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',T3.Folio,';ECONÓMICO: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Usuario que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'))) FROM  sistrefaccmant.modificaciones_sistema as t1 Inner join pedidosrefaccion as t2 ON t1.idregistro=t2.idPedRef inner join reportesupervicion as t3 ON t2.FolioPedfkSupervicion=t3.idReporteSupervicion WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        string[] diescinueve = v.getaData("SELECT upper(CONCAT(ultimaModificacion,';Refacción: ',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=t2.RefaccionfkCRefaccion) ,';Cantidad: ',t2.Cantidad)) FROM sistrefaccmant.modificaciones_sistema as t1 Inner join pedidosrefaccion as t2 ON t1.idregistro=t2.idPedRef WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        diescinueve[0] = UPPER("Refacción: " + v.getaData("SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion='" + diescinueve[0] + "'"));
                        diescinueve[1] = UPPER("Cantidad: " + diescinueve[1]);
                        CrearAntesDespuesLabels();
                        y1 = 270;
                        mitad1mitad2(diescinueve);
                        y1 = null;
                        break;
                    case "Reactivación de Clasificación de Empresa":
                    case "Desactivación de Clasificación de Empresa":
                    case "Inserción de Clasificación de Empresa":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Empresa: ',t2.giro,';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Usuario que Insertó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo'))) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal))) FROM modificaciones_sistema as t1 INNER JOIN cgiros as t2 ON t1.idregistro=t2.idgiro WHERE idmodificacion='"+id+"'").ToString().Split(';'));
                        break;
                    case "Actualización de Clasificación de Empresa":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Usuario que Actualizó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo'))) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal))) FROM modificaciones_sistema as t1 INNER JOIN cgiros as t2 ON t1.idregistro=t2.idgiro WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        string[] clasificaciones = v.getaData("SELECT UPPER(CONCAT(t1.ultimamodificacion,';',t2.giro)) FROM modificaciones_sistema as t1 INNER JOIN cgiros as t2 ON t1.idregistro=t2.idgiro WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        y1 = 200;
                        CrearAntesDespuesLabels();
                        y1 = 220;
                        mitad1mitad2(clasificaciones);
                        y1 = null;
                        break;
                    case "Edición del IVA":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('IVA Anterior: ',t1.ultimamodificacion,' %;IVA Actual: ',t2.iva,' %;Usuario Que Modificó: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo)) FROM modificaciones_sistema as t1 INNER join civa as t2 ON t1.idregistro=t2.idiva WHERE idmodificacion = '"+id+"';").ToString().Split(';'));
                        break;
                    default:
                        Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string UPPER(string str)
        {
            return str.ToUpper();
        }
        public string[] MetodoBurbuja(string[] vector)
        {
            string t;
            for (int a = 1; a < vector.Length; a++)
                for (int b = vector.Length - 1; b >= a; b--)
                {
                    string temp = vector[b - 1].Substring(vector[b - 1].Length-7);
                    int primero = Convert.ToInt32(temp);
                    temp = vector[b].Substring(vector[b].Length-7);
                    int segundo = Convert.ToInt32(temp);
                    if (primero > segundo)
                    {
                        t = vector[b - 1];
                        vector[b - 1] = vector[b];
                        vector[b] = t;
                    }
                }
            return vector;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        void cerrar()
        {
            gbadd.Controls.Clear();
        }
       
    }
}
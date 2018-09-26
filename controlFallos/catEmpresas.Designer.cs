namespace controlFallos
{
    partial class catEmpresas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbcempresa = new System.Windows.Forms.GroupBox();
            this.busqempresa = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Usu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbadd = new System.Windows.Forms.GroupBox();
            this.pCancel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.pEliminarEmpresa = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.txtgetnempresa = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtgetcempresa = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbcempresa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.busqempresa)).BeginInit();
            this.gbadd.SuspendLayout();
            this.pCancel.SuspendLayout();
            this.pEliminarEmpresa.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbcempresa
            // 
            this.gbcempresa.Controls.Add(this.busqempresa);
            this.gbcempresa.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbcempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbcempresa.Location = new System.Drawing.Point(578, 27);
            this.gbcempresa.Name = "gbcempresa";
            this.gbcempresa.Size = new System.Drawing.Size(850, 357);
            this.gbcempresa.TabIndex = 28;
            this.gbcempresa.TabStop = false;
            this.gbcempresa.Text = "Consulta de Empresas";
            // 
            // busqempresa
            // 
            this.busqempresa.AllowUserToAddRows = false;
            this.busqempresa.AllowUserToDeleteRows = false;
            this.busqempresa.AllowUserToResizeColumns = false;
            this.busqempresa.AllowUserToResizeRows = false;
            this.busqempresa.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.busqempresa.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.busqempresa.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.busqempresa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.busqempresa.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.busqempresa.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.busqempresa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.busqempresa.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.Usu,
            this.Estatus});
            this.busqempresa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.busqempresa.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.busqempresa.EnableHeadersVisualStyles = false;
            this.busqempresa.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.busqempresa.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.busqempresa.Location = new System.Drawing.Point(3, 27);
            this.busqempresa.MultiSelect = false;
            this.busqempresa.Name = "busqempresa";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.busqempresa.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.busqempresa.RowHeadersVisible = false;
            this.busqempresa.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.busqempresa.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.busqempresa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.busqempresa.ShowCellErrors = false;
            this.busqempresa.ShowCellToolTips = false;
            this.busqempresa.ShowEditingIcon = false;
            this.busqempresa.ShowRowErrors = false;
            this.busqempresa.Size = new System.Drawing.Size(844, 327);
            this.busqempresa.TabIndex = 0;
            this.busqempresa.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.busqempresa_CellContentDoubleClick);
            this.busqempresa.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.busqempresa_CellFormatting);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "idempresa";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Clave Empresa";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 146;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Nombre Empresa";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 168;
            // 
            // Usu
            // 
            this.Usu.HeaderText = "Persona que Dió de  Alta";
            this.Usu.Name = "Usu";
            this.Usu.Width = 162;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "Estatus";
            this.Estatus.Name = "Estatus";
            this.Estatus.Width = 96;
            // 
            // gbadd
            // 
            this.gbadd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbadd.Controls.Add(this.pCancel);
            this.gbadd.Controls.Add(this.pEliminarEmpresa);
            this.gbadd.Controls.Add(this.txtgetnempresa);
            this.gbadd.Controls.Add(this.label16);
            this.gbadd.Controls.Add(this.label19);
            this.gbadd.Controls.Add(this.lblsavemp);
            this.gbadd.Controls.Add(this.btnsavemp);
            this.gbadd.Controls.Add(this.txtgetcempresa);
            this.gbadd.Controls.Add(this.label21);
            this.gbadd.Controls.Add(this.label22);
            this.gbadd.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbadd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbadd.Location = new System.Drawing.Point(12, 28);
            this.gbadd.Name = "gbadd";
            this.gbadd.Size = new System.Drawing.Size(559, 356);
            this.gbadd.TabIndex = 27;
            this.gbadd.TabStop = false;
            this.gbadd.Text = "Nueva Empresa";
            this.gbadd.Enter += new System.EventHandler(this.gbEmp_Enter);
            // 
            // pCancel
            // 
            this.pCancel.Controls.Add(this.label2);
            this.pCancel.Controls.Add(this.btnCancelEmpresa);
            this.pCancel.Location = new System.Drawing.Point(370, 213);
            this.pCancel.Name = "pCancel";
            this.pCancel.Size = new System.Drawing.Size(148, 100);
            this.pCancel.TabIndex = 0;
            this.pCancel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(3, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 24);
            this.label2.TabIndex = 26;
            this.label2.Text = "Nueva Empresa";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(49, 14);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // pEliminarEmpresa
            // 
            this.pEliminarEmpresa.Controls.Add(this.lbldelete);
            this.pEliminarEmpresa.Controls.Add(this.btndelete);
            this.pEliminarEmpresa.Location = new System.Drawing.Point(2, 213);
            this.pEliminarEmpresa.Name = "pEliminarEmpresa";
            this.pEliminarEmpresa.Size = new System.Drawing.Size(169, 100);
            this.pEliminarEmpresa.TabIndex = 0;
            this.pEliminarEmpresa.Visible = false;
            // 
            // lbldelete
            // 
            this.lbldelete.AutoSize = true;
            this.lbldelete.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.Location = new System.Drawing.Point(-4, 67);
            this.lbldelete.Name = "lbldelete";
            this.lbldelete.Size = new System.Drawing.Size(177, 24);
            this.lbldelete.TabIndex = 26;
            this.lbldelete.Text = "Desactivar Empresa";
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.Location = new System.Drawing.Point(57, 14);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(50, 50);
            this.btndelete.TabIndex = 25;
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtgetnempresa
            // 
            this.txtgetnempresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetnempresa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetnempresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetnempresa.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetnempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetnempresa.Location = new System.Drawing.Point(222, 130);
            this.txtgetnempresa.MaxLength = 60;
            this.txtgetnempresa.Name = "txtgetnempresa";
            this.txtgetnempresa.ShortcutsEnabled = false;
            this.txtgetnempresa.Size = new System.Drawing.Size(216, 22);
            this.txtgetnempresa.TabIndex = 2;
            this.txtgetnempresa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetnempresa_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(57, 128);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(159, 24);
            this.label16.TabIndex = 0;
            this.label16.Text = "Nombre Empresa";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(220, 145);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(221, 9);
            this.label19.TabIndex = 0;
            this.label19.Text = "______________________________________________________";
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(190, 280);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(153, 24);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Agregar Empresa";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(240, 227);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 3;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtgetcempresa
            // 
            this.txtgetcempresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetcempresa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetcempresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetcempresa.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetcempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetcempresa.Location = new System.Drawing.Point(222, 56);
            this.txtgetcempresa.MaxLength = 5;
            this.txtgetcempresa.Name = "txtgetcempresa";
            this.txtgetcempresa.ShortcutsEnabled = false;
            this.txtgetcempresa.Size = new System.Drawing.Size(216, 22);
            this.txtgetcempresa.TabIndex = 1;
            this.txtgetcempresa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetcempresa_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(79, 54);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(135, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Clave Empresa";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(220, 71);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(221, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "______________________________________________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(52, 357);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 18);
            this.label1.TabIndex = 66;
            this.label1.Text = "Nota:";
            this.label1.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(91, 357);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1434, 27);
            this.panel1.TabIndex = 67;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1387, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(476, 1);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(451, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Sistema de Reporte de Fallos - Catálogo de Empresas";
            // 
            // catEmpresas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1434, 386);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbcempresa);
            this.Controls.Add(this.gbadd);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "catEmpresas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catEmpresas";
            this.Load += new System.EventHandler(this.catEmpresas_Load);
            this.gbcempresa.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.busqempresa)).EndInit();
            this.gbadd.ResumeLayout(false);
            this.gbadd.PerformLayout();
            this.pCancel.ResumeLayout(false);
            this.pCancel.PerformLayout();
            this.pEliminarEmpresa.ResumeLayout(false);
            this.pEliminarEmpresa.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbcempresa;
        private System.Windows.Forms.DataGridView busqempresa;
        private System.Windows.Forms.GroupBox gbadd;
        private System.Windows.Forms.Panel pEliminarEmpresa;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.TextBox txtgetnempresa;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtgetcempresa;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel pCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Usu;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}
namespace controlFallos
{
    partial class catDescFallos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbfallos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idclasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbClasificacion = new System.Windows.Forms.GroupBox();
            this.cbclasificacion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.pEliminarClasificacion = new System.Windows.Forms.Panel();
            this.lbldeletedesc = new System.Windows.Forms.Label();
            this.btndeletedesc = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtgetdescfallo = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).BeginInit();
            this.gbClasificacion.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pEliminarClasificacion.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbfallos);
            this.groupBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBox1.Location = new System.Drawing.Point(752, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1006, 620);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consulta Descripciones de Fallo";
            // 
            // tbfallos
            // 
            this.tbfallos.AllowUserToAddRows = false;
            this.tbfallos.AllowUserToDeleteRows = false;
            this.tbfallos.AllowUserToResizeColumns = false;
            this.tbfallos.AllowUserToResizeRows = false;
            this.tbfallos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfallos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfallos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfallos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Descripcion,
            this.usuario,
            this.Estatus,
            this.idclasif});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbfallos.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbfallos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbfallos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbfallos.EnableHeadersVisualStyles = false;
            this.tbfallos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbfallos.Location = new System.Drawing.Point(3, 25);
            this.tbfallos.MultiSelect = false;
            this.tbfallos.Name = "tbfallos";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tbfallos.RowHeadersVisible = false;
            this.tbfallos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.tbfallos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tbfallos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfallos.ShowCellErrors = false;
            this.tbfallos.ShowCellToolTips = false;
            this.tbfallos.ShowEditingIcon = false;
            this.tbfallos.ShowRowErrors = false;
            this.tbfallos.Size = new System.Drawing.Size(1000, 592);
            this.tbfallos.TabIndex = 1;
            this.tbfallos.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfallos_CellContentDoubleClick);
            this.tbfallos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfallos_CellFormatting);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idclasificación";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Clasificación de Fallo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // Descripcion
            // 
            this.Descripcion.HeaderText = "Descripción de Fallo";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.Width = 250;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "Persona que Dio de Alta";
            this.usuario.Name = "usuario";
            this.usuario.Width = 300;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "Estatus";
            this.Estatus.Name = "Estatus";
            this.Estatus.Width = 200;
            // 
            // idclasif
            // 
            this.idclasif.HeaderText = "idclasif";
            this.idclasif.Name = "idclasif";
            this.idclasif.Visible = false;
            // 
            // gbClasificacion
            // 
            this.gbClasificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbClasificacion.Controls.Add(this.label3);
            this.gbClasificacion.Controls.Add(this.label23);
            this.gbClasificacion.Controls.Add(this.cbclasificacion);
            this.gbClasificacion.Controls.Add(this.label2);
            this.gbClasificacion.Controls.Add(this.pCancelar);
            this.gbClasificacion.Controls.Add(this.pEliminarClasificacion);
            this.gbClasificacion.Controls.Add(this.lblsavemp);
            this.gbClasificacion.Controls.Add(this.btnsavemp);
            this.gbClasificacion.Controls.Add(this.txtgetdescfallo);
            this.gbClasificacion.Controls.Add(this.label21);
            this.gbClasificacion.Controls.Add(this.label22);
            this.gbClasificacion.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbClasificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbClasificacion.Location = new System.Drawing.Point(0, 30);
            this.gbClasificacion.Name = "gbClasificacion";
            this.gbClasificacion.Size = new System.Drawing.Size(746, 620);
            this.gbClasificacion.TabIndex = 30;
            this.gbClasificacion.TabStop = false;
            this.gbClasificacion.Text = "Agregar Descripción de Fallo";
            // 
            // cbclasificacion
            // 
            this.cbclasificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbclasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbclasificacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbclasificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbclasificacion.FormattingEnabled = true;
            this.cbclasificacion.Location = new System.Drawing.Point(319, 163);
            this.cbclasificacion.Name = "cbclasificacion";
            this.cbclasificacion.Size = new System.Drawing.Size(304, 29);
            this.cbclasificacion.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 21);
            this.label2.TabIndex = 30;
            this.label2.Text = "Clasificación de Fallo";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(487, 392);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(199, 129);
            this.pCancelar.TabIndex = 29;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Nueva Descripción";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(75, 25);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // pEliminarClasificacion
            // 
            this.pEliminarClasificacion.Controls.Add(this.lbldeletedesc);
            this.pEliminarClasificacion.Controls.Add(this.btndeletedesc);
            this.pEliminarClasificacion.Location = new System.Drawing.Point(25, 392);
            this.pEliminarClasificacion.Name = "pEliminarClasificacion";
            this.pEliminarClasificacion.Size = new System.Drawing.Size(196, 129);
            this.pEliminarClasificacion.TabIndex = 28;
            this.pEliminarClasificacion.Visible = false;
            // 
            // lbldeletedesc
            // 
            this.lbldeletedesc.AutoSize = true;
            this.lbldeletedesc.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletedesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletedesc.Location = new System.Drawing.Point(7, 86);
            this.lbldeletedesc.Name = "lbldeletedesc";
            this.lbldeletedesc.Size = new System.Drawing.Size(184, 21);
            this.lbldeletedesc.TabIndex = 26;
            this.lbldeletedesc.Text = "Desactivar Descripción";
            // 
            // btndeletedesc
            // 
            this.btndeletedesc.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeletedesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeletedesc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeletedesc.FlatAppearance.BorderSize = 0;
            this.btndeletedesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeletedesc.Location = new System.Drawing.Point(68, 25);
            this.btndeletedesc.Name = "btndeletedesc";
            this.btndeletedesc.Size = new System.Drawing.Size(50, 50);
            this.btndeletedesc.TabIndex = 25;
            this.btndeletedesc.UseVisualStyleBackColor = true;
            this.btndeletedesc.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(270, 478);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(164, 21);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Agregar Descripcion";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(326, 417);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 6;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtgetdescfallo
            // 
            this.txtgetdescfallo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetdescfallo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetdescfallo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetdescfallo.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetdescfallo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetdescfallo.Location = new System.Drawing.Point(327, 252);
            this.txtgetdescfallo.MaxLength = 40;
            this.txtgetdescfallo.Name = "txtgetdescfallo";
            this.txtgetdescfallo.ShortcutsEnabled = false;
            this.txtgetdescfallo.Size = new System.Drawing.Size(296, 22);
            this.txtgetdescfallo.TabIndex = 2;
            this.txtgetdescfallo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetclasificacion_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(81, 250);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(163, 21);
            this.label21.TabIndex = 0;
            this.label21.Text = "Descripción de Fallo";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(324, 269);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(301, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "__________________________________________________________________________";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1758, 27);
            this.panel1.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1711, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(561, 3);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(550, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Sistema de Reporte de Fallos - Catálogo de Descripción de Fallos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(227, 557);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 18);
            this.label3.TabIndex = 66;
            this.label3.Text = "Nota:";
            this.label3.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(266, 557);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // catDescFallos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1758, 650);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbClasificacion);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "catDescFallos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catDescFallos";
            this.Load += new System.EventHandler(this.catDescFallos_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).EndInit();
            this.gbClasificacion.ResumeLayout(false);
            this.gbClasificacion.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pEliminarClasificacion.ResumeLayout(false);
            this.pEliminarClasificacion.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView tbfallos;
        private System.Windows.Forms.GroupBox gbClasificacion;
        private System.Windows.Forms.ComboBox cbclasificacion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pEliminarClasificacion;
        private System.Windows.Forms.Label lbldeletedesc;
        private System.Windows.Forms.Button btndeletedesc;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtgetdescfallo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idclasif;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
    }
}
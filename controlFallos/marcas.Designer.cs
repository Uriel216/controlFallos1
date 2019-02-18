namespace controlFallos
{
    partial class marcas
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
            this.gbadd = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtmarca = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pdeletefam = new System.Windows.Forms.Panel();
            this.lbldeletefam = new System.Windows.Forms.Label();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.gbconsulta = new System.Windows.Forms.GroupBox();
            this.tbmarcas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuariofkcpersonal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbadd.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.pdeletefam.SuspendLayout();
            this.gbconsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbmarcas)).BeginInit();
            this.SuspendLayout();
            // 
            // gbadd
            // 
            this.gbadd.Controls.Add(this.label2);
            this.gbadd.Controls.Add(this.label3);
            this.gbadd.Controls.Add(this.pcancel);
            this.gbadd.Controls.Add(this.lblsave);
            this.gbadd.Controls.Add(this.btnsave);
            this.gbadd.Controls.Add(this.txtmarca);
            this.gbadd.Controls.Add(this.label22);
            this.gbadd.Controls.Add(this.label23);
            this.gbadd.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbadd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbadd.Location = new System.Drawing.Point(0, 0);
            this.gbadd.Name = "gbadd";
            this.gbadd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbadd.Size = new System.Drawing.Size(580, 595);
            this.gbadd.TabIndex = 0;
            this.gbadd.TabStop = false;
            this.gbadd.Text = "Agregar Marca";
            this.gbadd.Visible = false;
            this.gbadd.Paint += new System.Windows.Forms.PaintEventHandler(this.gbadd_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(27, 424);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 18);
            this.label2.TabIndex = 66;
            this.label2.Text = "Nota:";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(66, 424);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(474, 18);
            this.label3.TabIndex = 65;
            this.label3.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label3.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label1);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(340, 311);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(170, 85);
            this.pcancel.TabIndex = 44;
            this.pcancel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(54, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 24;
            this.label1.Text = "Nuevo";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(63, 3);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 44;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblsave.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsave.Location = new System.Drawing.Point(223, 366);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(80, 24);
            this.lblsave.TabIndex = 0;
            this.lblsave.Text = "Agregar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.btnsave.Location = new System.Drawing.Point(237, 314);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 2;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtmarca
            // 
            this.txtmarca.AllowDrop = true;
            this.txtmarca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtmarca.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtmarca.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtmarca.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtmarca.Location = new System.Drawing.Point(254, 207);
            this.txtmarca.MaxLength = 80;
            this.txtmarca.Name = "txtmarca";
            this.txtmarca.ShortcutsEnabled = false;
            this.txtmarca.Size = new System.Drawing.Size(203, 18);
            this.txtmarca.TabIndex = 1;
            this.txtmarca.TextChanged += new System.EventHandler(this.getCambios);
            this.txtmarca.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtmarca_KeyPress);
            this.txtmarca.Validating += new System.ComponentModel.CancelEventHandler(this.txtmarca_Validating);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label22.Location = new System.Drawing.Point(252, 220);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(205, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "__________________________________________________";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(114, 205);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(60, 21);
            this.label23.TabIndex = 0;
            this.label23.Text = "Marca:";
            // 
            // pdeletefam
            // 
            this.pdeletefam.Controls.Add(this.lbldeletefam);
            this.pdeletefam.Controls.Add(this.btndeleteuser);
            this.pdeletefam.Location = new System.Drawing.Point(20, 314);
            this.pdeletefam.Name = "pdeletefam";
            this.pdeletefam.Size = new System.Drawing.Size(170, 82);
            this.pdeletefam.TabIndex = 43;
            this.pdeletefam.Visible = false;
            // 
            // lbldeletefam
            // 
            this.lbldeletefam.AutoSize = true;
            this.lbldeletefam.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletefam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletefam.Location = new System.Drawing.Point(32, 52);
            this.lbldeletefam.Name = "lbldeletefam";
            this.lbldeletefam.Size = new System.Drawing.Size(105, 24);
            this.lbldeletefam.TabIndex = 24;
            this.lbldeletefam.Text = "Desactivar";
            // 
            // btndeleteuser
            // 
            this.btndeleteuser.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeleteuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeleteuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteuser.FlatAppearance.BorderSize = 0;
            this.btndeleteuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeleteuser.Location = new System.Drawing.Point(57, 3);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 23;
            this.btndeleteuser.UseVisualStyleBackColor = true;
            this.btndeleteuser.Click += new System.EventHandler(this.btndeleteuser_Click);
            // 
            // gbconsulta
            // 
            this.gbconsulta.Controls.Add(this.tbmarcas);
            this.gbconsulta.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbconsulta.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbconsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbconsulta.Location = new System.Drawing.Point(583, 0);
            this.gbconsulta.Name = "gbconsulta";
            this.gbconsulta.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbconsulta.Size = new System.Drawing.Size(605, 595);
            this.gbconsulta.TabIndex = 0;
            this.gbconsulta.TabStop = false;
            this.gbconsulta.Text = "Consulta de Marcas de Refacciones";
            this.gbconsulta.Visible = false;
            this.gbconsulta.Paint += new System.Windows.Forms.PaintEventHandler(this.gbadd_Paint);
            // 
            // tbmarcas
            // 
            this.tbmarcas.AllowUserToAddRows = false;
            this.tbmarcas.AllowUserToDeleteRows = false;
            this.tbmarcas.AllowUserToResizeColumns = false;
            this.tbmarcas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbmarcas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbmarcas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbmarcas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbmarcas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbmarcas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbmarcas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbmarcas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbmarcas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.usuariofkcpersonal,
            this.Estatus});
            this.tbmarcas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbmarcas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.tbmarcas.EnableHeadersVisualStyles = false;
            this.tbmarcas.GridColor = System.Drawing.Color.White;
            this.tbmarcas.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbmarcas.Location = new System.Drawing.Point(3, 27);
            this.tbmarcas.MultiSelect = false;
            this.tbmarcas.Name = "tbmarcas";
            this.tbmarcas.ReadOnly = true;
            this.tbmarcas.RowHeadersVisible = false;
            this.tbmarcas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbmarcas.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbmarcas.RowTemplate.ReadOnly = true;
            this.tbmarcas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbmarcas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbmarcas.ShowCellErrors = false;
            this.tbmarcas.ShowCellToolTips = false;
            this.tbmarcas.ShowEditingIcon = false;
            this.tbmarcas.ShowRowErrors = false;
            this.tbmarcas.Size = new System.Drawing.Size(599, 565);
            this.tbmarcas.TabIndex = 110;
            this.tbmarcas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbmarcas_CellContentDoubleClick);
            this.tbmarcas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbmarcas_CellFormatting);
            this.tbmarcas.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbmarcas_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idmarca";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "MARCA";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 180;
            // 
            // usuariofkcpersonal
            // 
            this.usuariofkcpersonal.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuariofkcpersonal.Name = "usuariofkcpersonal";
            this.usuariofkcpersonal.ReadOnly = true;
            this.usuariofkcpersonal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.usuariofkcpersonal.Width = 310;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // marcas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1188, 595);
            this.Controls.Add(this.pdeletefam);
            this.Controls.Add(this.gbadd);
            this.Controls.Add(this.gbconsulta);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "marcas";
            this.Text = "marcas";
            this.Load += new System.EventHandler(this.marcas_Load);
            this.gbadd.ResumeLayout(false);
            this.gbadd.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.pdeletefam.ResumeLayout(false);
            this.pdeletefam.PerformLayout();
            this.gbconsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbmarcas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbadd;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtmarca;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox gbconsulta;
        private System.Windows.Forms.DataGridView tbmarcas;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Panel pdeletefam;
        private System.Windows.Forms.Label lbldeletefam;
        private System.Windows.Forms.Button btndeleteuser;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuariofkcpersonal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}
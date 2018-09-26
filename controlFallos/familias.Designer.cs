namespace controlFallos
{
    partial class familias
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
            this.gbfamilias = new System.Windows.Forms.GroupBox();
            this.tbfamilias = new System.Windows.Forms.DataGridView();
            this.idfamilia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.familia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddfamilia = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.pdeletefam = new System.Windows.Forms.Panel();
            this.lbldeletefam = new System.Windows.Forms.Label();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.txtdescfamilia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.gbfamilias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilias)).BeginInit();
            this.gbaddfamilia.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.pdeletefam.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbfamilias
            // 
            this.gbfamilias.Controls.Add(this.tbfamilias);
            this.gbfamilias.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbfamilias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbfamilias.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbfamilias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbfamilias.Location = new System.Drawing.Point(588, 0);
            this.gbfamilias.Name = "gbfamilias";
            this.gbfamilias.Size = new System.Drawing.Size(600, 595);
            this.gbfamilias.TabIndex = 0;
            this.gbfamilias.TabStop = false;
            this.gbfamilias.Text = "Consulta de Familias de Refacciones";
            this.gbfamilias.Visible = false;
            // 
            // tbfamilias
            // 
            this.tbfamilias.AllowUserToAddRows = false;
            this.tbfamilias.AllowUserToDeleteRows = false;
            this.tbfamilias.AllowUserToResizeColumns = false;
            this.tbfamilias.AllowUserToResizeRows = false;
            this.tbfamilias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tbfamilias.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfamilias.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfamilias.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfamilias.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfamilias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfamilias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idfamilia,
            this.familia,
            this.desc,
            this.alta,
            this.Estatus});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbfamilias.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbfamilias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbfamilias.EnableHeadersVisualStyles = false;
            this.tbfamilias.Location = new System.Drawing.Point(3, 27);
            this.tbfamilias.Name = "tbfamilias";
            this.tbfamilias.ReadOnly = true;
            this.tbfamilias.RowHeadersVisible = false;
            this.tbfamilias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfamilias.Size = new System.Drawing.Size(594, 565);
            this.tbfamilias.TabIndex = 0;
            this.tbfamilias.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfamilias_CellContentDoubleClick);
            this.tbfamilias.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfamilias_CellFormatting);
            // 
            // idfamilia
            // 
            this.idfamilia.HeaderText = "Column1";
            this.idfamilia.Name = "idfamilia";
            this.idfamilia.ReadOnly = true;
            this.idfamilia.Visible = false;
            this.idfamilia.Width = 111;
            // 
            // familia
            // 
            this.familia.HeaderText = "Familia";
            this.familia.Name = "familia";
            this.familia.ReadOnly = true;
            this.familia.Width = 96;
            // 
            // desc
            // 
            this.desc.HeaderText = "Descripción";
            this.desc.Name = "desc";
            this.desc.ReadOnly = true;
            this.desc.Width = 136;
            // 
            // alta
            // 
            this.alta.HeaderText = "Persona que Dió de Alta";
            this.alta.Name = "alta";
            this.alta.ReadOnly = true;
            this.alta.Width = 162;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "Estatus";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.Width = 96;
            // 
            // gbaddfamilia
            // 
            this.gbaddfamilia.Controls.Add(this.label22);
            this.gbaddfamilia.Controls.Add(this.label23);
            this.gbaddfamilia.Controls.Add(this.pcancel);
            this.gbaddfamilia.Controls.Add(this.pdeletefam);
            this.gbaddfamilia.Controls.Add(this.txtdescfamilia);
            this.gbaddfamilia.Controls.Add(this.label4);
            this.gbaddfamilia.Controls.Add(this.label9);
            this.gbaddfamilia.Controls.Add(this.lblsave);
            this.gbaddfamilia.Controls.Add(this.btnsave);
            this.gbaddfamilia.Controls.Add(this.txtnombre);
            this.gbaddfamilia.Controls.Add(this.label17);
            this.gbaddfamilia.Controls.Add(this.label18);
            this.gbaddfamilia.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbaddfamilia.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddfamilia.Location = new System.Drawing.Point(0, 0);
            this.gbaddfamilia.Name = "gbaddfamilia";
            this.gbaddfamilia.Size = new System.Drawing.Size(582, 595);
            this.gbaddfamilia.TabIndex = 0;
            this.gbaddfamilia.TabStop = false;
            this.gbaddfamilia.Text = "Agregar Familia de Refacciones";
            this.gbaddfamilia.Visible = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(42, 544);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 18);
            this.label22.TabIndex = 66;
            this.label22.Text = "Nota:";
            this.label22.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(81, 544);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label1);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(368, 362);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(147, 88);
            this.pcancel.TabIndex = 42;
            this.pcancel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(3, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 24);
            this.label1.TabIndex = 24;
            this.label1.Text = "Nueva Familia";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(52, 3);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 42;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // pdeletefam
            // 
            this.pdeletefam.Controls.Add(this.lbldeletefam);
            this.pdeletefam.Controls.Add(this.btndeleteuser);
            this.pdeletefam.Location = new System.Drawing.Point(6, 363);
            this.pdeletefam.Name = "pdeletefam";
            this.pdeletefam.Size = new System.Drawing.Size(180, 80);
            this.pdeletefam.TabIndex = 41;
            this.pdeletefam.Visible = false;
            // 
            // lbldeletefam
            // 
            this.lbldeletefam.AutoSize = true;
            this.lbldeletefam.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletefam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletefam.Location = new System.Drawing.Point(2, 55);
            this.lbldeletefam.Name = "lbldeletefam";
            this.lbldeletefam.Size = new System.Drawing.Size(177, 24);
            this.lbldeletefam.TabIndex = 24;
            this.lbldeletefam.Text = "Desactivar Familia";
            // 
            // btndeleteuser
            // 
            this.btndeleteuser.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeleteuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeleteuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteuser.FlatAppearance.BorderSize = 0;
            this.btndeleteuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeleteuser.Location = new System.Drawing.Point(67, 2);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 23;
            this.btndeleteuser.UseVisualStyleBackColor = true;
            this.btndeleteuser.Click += new System.EventHandler(this.btndeleteuser_Click);
            // 
            // txtdescfamilia
            // 
            this.txtdescfamilia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtdescfamilia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtdescfamilia.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtdescfamilia.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdescfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtdescfamilia.Location = new System.Drawing.Point(260, 234);
            this.txtdescfamilia.MaxLength = 40;
            this.txtdescfamilia.Name = "txtdescfamilia";
            this.txtdescfamilia.ShortcutsEnabled = false;
            this.txtdescfamilia.Size = new System.Drawing.Size(255, 22);
            this.txtdescfamilia.TabIndex = 2;
            this.txtdescfamilia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnombre_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(258, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 9);
            this.label4.TabIndex = 0;
            this.label4.Text = "_______________________________________________________________";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label9.Location = new System.Drawing.Point(38, 239);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(202, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "Descripción de Familia";
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblsave.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsave.Location = new System.Drawing.Point(208, 420);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(140, 21);
            this.lblsave.TabIndex = 0;
            this.lblsave.Text = "Agregar Familia";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.btnsave.Location = new System.Drawing.Point(255, 364);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 3;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.button10_Click);
            // 
            // txtnombre
            // 
            this.txtnombre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtnombre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtnombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtnombre.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtnombre.Location = new System.Drawing.Point(260, 150);
            this.txtnombre.MaxLength = 20;
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.ShortcutsEnabled = false;
            this.txtnombre.Size = new System.Drawing.Size(255, 22);
            this.txtnombre.TabIndex = 1;
            this.txtnombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnombre_KeyPress);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label17.Location = new System.Drawing.Point(258, 165);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(257, 9);
            this.label17.TabIndex = 0;
            this.label17.Text = "_______________________________________________________________";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label18.Location = new System.Drawing.Point(39, 150);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(171, 24);
            this.label18.TabIndex = 0;
            this.label18.Text = "Nombre de Familia";
            // 
            // familias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1188, 595);
            this.Controls.Add(this.gbfamilias);
            this.Controls.Add(this.gbaddfamilia);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "familias";
            this.Text = "familias";
            this.Load += new System.EventHandler(this.familias_Load);
            this.gbfamilias.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilias)).EndInit();
            this.gbaddfamilia.ResumeLayout(false);
            this.gbaddfamilia.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.pdeletefam.ResumeLayout(false);
            this.pdeletefam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbfamilias;
        private System.Windows.Forms.DataGridView tbfamilias;
        private System.Windows.Forms.GroupBox gbaddfamilia;
        private System.Windows.Forms.TextBox txtdescfamilia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pdeletefam;
        private System.Windows.Forms.Label lbldeletefam;
        private System.Windows.Forms.Button btndeleteuser;
        private System.Windows.Forms.DataGridViewTextBoxColumn idfamilia;
        private System.Windows.Forms.DataGridViewTextBoxColumn familia;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn alta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
    }
}
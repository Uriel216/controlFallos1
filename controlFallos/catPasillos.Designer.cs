﻿namespace controlFallos
{
    partial class catPasillos
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbPasillo = new System.Windows.Forms.GroupBox();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelpa = new System.Windows.Forms.Label();
            this.btndelpa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtpasillo = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbubicaciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbPasillo.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pdelete.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbubicaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(854, 27);
            this.panel1.TabIndex = 31;
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
            this.button1.Location = new System.Drawing.Point(807, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
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
            this.lbltitle.Location = new System.Drawing.Point(203, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(432, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Sistema de Reporte de Fallos - Catálogo de Pasillos";
            // 
            // gbPasillo
            // 
            this.gbPasillo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbPasillo.Controls.Add(this.label2);
            this.gbPasillo.Controls.Add(this.label23);
            this.gbPasillo.Controls.Add(this.pCancelar);
            this.gbPasillo.Controls.Add(this.pdelete);
            this.gbPasillo.Controls.Add(this.lblsavemp);
            this.gbPasillo.Controls.Add(this.btnsavemp);
            this.gbPasillo.Controls.Add(this.txtpasillo);
            this.gbPasillo.Controls.Add(this.label21);
            this.gbPasillo.Controls.Add(this.label22);
            this.gbPasillo.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPasillo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbPasillo.Location = new System.Drawing.Point(0, 27);
            this.gbPasillo.Name = "gbPasillo";
            this.gbPasillo.Size = new System.Drawing.Size(440, 278);
            this.gbPasillo.TabIndex = 32;
            this.gbPasillo.TabStop = false;
            this.gbPasillo.Text = "Agregar Pasillo";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(292, 175);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(105, 63);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(2, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nuevo Pasillo";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(36, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(35, 35);
            this.btnCancelEmpresa.TabIndex = 0;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelpa);
            this.pdelete.Controls.Add(this.btndelpa);
            this.pdelete.Location = new System.Drawing.Point(0, 171);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(144, 65);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // lbldelpa
            // 
            this.lbldelpa.AutoSize = true;
            this.lbldelpa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelpa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelpa.Location = new System.Drawing.Point(9, 41);
            this.lbldelpa.Name = "lbldelpa";
            this.lbldelpa.Size = new System.Drawing.Size(122, 18);
            this.lbldelpa.TabIndex = 0;
            this.lbldelpa.Text = "Desactivar Pasillo";
            this.lbldelpa.Click += new System.EventHandler(this.label9_Click);
            // 
            // btndelpa
            // 
            this.btndelpa.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelpa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelpa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelpa.FlatAppearance.BorderSize = 0;
            this.btndelpa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelpa.Location = new System.Drawing.Point(51, 3);
            this.btndelpa.Name = "btndelpa";
            this.btndelpa.Size = new System.Drawing.Size(35, 35);
            this.btndelpa.TabIndex = 0;
            this.btndelpa.UseVisualStyleBackColor = true;
            this.btndelpa.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsavemp.Location = new System.Drawing.Point(156, 213);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(103, 18);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Agregar Pasillo";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(195, 175);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(35, 35);
            this.btnsavemp.TabIndex = 2;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtpasillo
            // 
            this.txtpasillo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtpasillo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtpasillo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtpasillo.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpasillo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtpasillo.Location = new System.Drawing.Point(195, 98);
            this.txtpasillo.MaxLength = 3;
            this.txtpasillo.Name = "txtpasillo";
            this.txtpasillo.ShortcutsEnabled = false;
            this.txtpasillo.Size = new System.Drawing.Size(75, 22);
            this.txtpasillo.TabIndex = 1;
            this.txtpasillo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtpasillo_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(124, 98);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Pasillo";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(193, 113);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "__________________";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbubicaciones);
            this.groupBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBox1.Location = new System.Drawing.Point(446, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 278);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consulta de Pasillos";
            // 
            // tbubicaciones
            // 
            this.tbubicaciones.AllowUserToAddRows = false;
            this.tbubicaciones.AllowUserToDeleteRows = false;
            this.tbubicaciones.AllowUserToResizeColumns = false;
            this.tbubicaciones.AllowUserToResizeRows = false;
            this.tbubicaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tbubicaciones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbubicaciones.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbubicaciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbubicaciones.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbubicaciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tbubicaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbubicaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.usuario,
            this.Estatus});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbubicaciones.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbubicaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbubicaciones.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbubicaciones.EnableHeadersVisualStyles = false;
            this.tbubicaciones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbubicaciones.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbubicaciones.Location = new System.Drawing.Point(3, 25);
            this.tbubicaciones.MultiSelect = false;
            this.tbubicaciones.Name = "tbubicaciones";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbubicaciones.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tbubicaciones.RowHeadersVisible = false;
            this.tbubicaciones.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.tbubicaciones.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tbubicaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbubicaciones.ShowCellErrors = false;
            this.tbubicaciones.ShowCellToolTips = false;
            this.tbubicaciones.ShowEditingIcon = false;
            this.tbubicaciones.ShowRowErrors = false;
            this.tbubicaciones.Size = new System.Drawing.Size(396, 250);
            this.tbubicaciones.TabIndex = 0;
            this.tbubicaciones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbubicaciones_CellContentClick);
            this.tbubicaciones.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbubicaciones_CellContentDoubleClick);
            this.tbubicaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbubicaciones_CellFormatting);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idpasillo";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 75;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Pasillo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 81;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "Persona que Dió de Alta";
            this.usuario.Name = "usuario";
            this.usuario.Width = 150;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "Estatus";
            this.Estatus.Name = "Estatus";
            this.Estatus.Width = 90;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(12, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 18);
            this.label2.TabIndex = 66;
            this.label2.Text = "Nota:";
            this.label2.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(-3, 257);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(434, 17);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // catPasillos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(854, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPasillo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "catPasillos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catPasillos";
            this.Load += new System.EventHandler(this.catPasillos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbPasillo.ResumeLayout(false);
            this.gbPasillo.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbubicaciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.GroupBox gbPasillo;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelpa;
        private System.Windows.Forms.Button btndelpa;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtpasillo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView tbubicaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label23;
    }
}
﻿namespace controlFallos
{
    partial class catfallosGrales
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbconsulta = new System.Windows.Forms.GroupBox();
            this.gbbuscar = new System.Windows.Forms.GroupBox();
            this.cbnombrefb = new System.Windows.Forms.ComboBox();
            this.cbDescFallob = new System.Windows.Forms.ComboBox();
            this.cbClasificacionb = new System.Windows.Forms.ComboBox();
            this.pActualizar = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtgetcodbusq = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.LblExcel = new System.Windows.Forms.Label();
            this.tbfallos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codfallo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomfallo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idclasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iddesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddnomfallo = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblcodfallo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbdescripcion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbclasificacion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.txtgetdescfallo = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.pEliminarClasificacion = new System.Windows.Forms.Panel();
            this.lbldeletedesc = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btndeletedesc = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExcel = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.gbconsulta.SuspendLayout();
            this.gbbuscar.SuspendLayout();
            this.pActualizar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).BeginInit();
            this.gbaddnomfallo.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pEliminarClasificacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // gbconsulta
            // 
            this.gbconsulta.Controls.Add(this.gbbuscar);
            this.gbconsulta.Controls.Add(this.tbfallos);
            this.gbconsulta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbconsulta.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbconsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbconsulta.Location = new System.Drawing.Point(0, 519);
            this.gbconsulta.Name = "gbconsulta";
            this.gbconsulta.Size = new System.Drawing.Size(1908, 418);
            this.gbconsulta.TabIndex = 33;
            this.gbconsulta.TabStop = false;
            this.gbconsulta.Text = "Consulta Clasificaciones de Fallo";
            this.gbconsulta.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddnomfallo_Paint);
            // 
            // gbbuscar
            // 
            this.gbbuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbbuscar.Controls.Add(this.cbnombrefb);
            this.gbbuscar.Controls.Add(this.btnExcel);
            this.gbbuscar.Controls.Add(this.cbDescFallob);
            this.gbbuscar.Controls.Add(this.cbClasificacionb);
            this.gbbuscar.Controls.Add(this.pActualizar);
            this.gbbuscar.Controls.Add(this.label8);
            this.gbbuscar.Controls.Add(this.txtgetcodbusq);
            this.gbbuscar.Controls.Add(this.label18);
            this.gbbuscar.Controls.Add(this.label19);
            this.gbbuscar.Controls.Add(this.label17);
            this.gbbuscar.Controls.Add(this.label16);
            this.gbbuscar.Controls.Add(this.button3);
            this.gbbuscar.Controls.Add(this.label15);
            this.gbbuscar.Controls.Add(this.LblExcel);
            this.gbbuscar.Controls.Add(this.pictureBox2);
            this.gbbuscar.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbbuscar.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbbuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbbuscar.Location = new System.Drawing.Point(3, 25);
            this.gbbuscar.Name = "gbbuscar";
            this.gbbuscar.Size = new System.Drawing.Size(1902, 150);
            this.gbbuscar.TabIndex = 19;
            this.gbbuscar.TabStop = false;
            this.gbbuscar.Text = "Buscar";
            // 
            // cbnombrefb
            // 
            this.cbnombrefb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbnombrefb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbnombrefb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbnombrefb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbnombrefb.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbnombrefb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbnombrefb.FormattingEnabled = true;
            this.cbnombrefb.Location = new System.Drawing.Point(203, 85);
            this.cbnombrefb.Name = "cbnombrefb";
            this.cbnombrefb.Size = new System.Drawing.Size(444, 26);
            this.cbnombrefb.TabIndex = 81;
            this.cbnombrefb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbnombrefb.SelectedIndexChanged += new System.EventHandler(this.cbnombrefb_SelectedIndexChanged);
            // 
            // cbDescFallob
            // 
            this.cbDescFallob.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbDescFallob.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDescFallob.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDescFallob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDescFallob.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDescFallob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbDescFallob.FormattingEnabled = true;
            this.cbDescFallob.Location = new System.Drawing.Point(917, 24);
            this.cbDescFallob.Name = "cbDescFallob";
            this.cbDescFallob.Size = new System.Drawing.Size(399, 26);
            this.cbDescFallob.TabIndex = 78;
            this.cbDescFallob.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbDescFallob.SelectedIndexChanged += new System.EventHandler(this.cbDescFallob_SelectedIndexChanged);
            // 
            // cbClasificacionb
            // 
            this.cbClasificacionb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbClasificacionb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbClasificacionb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClasificacionb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbClasificacionb.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbClasificacionb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbClasificacionb.FormattingEnabled = true;
            this.cbClasificacionb.Location = new System.Drawing.Point(203, 28);
            this.cbClasificacionb.Name = "cbClasificacionb";
            this.cbClasificacionb.Size = new System.Drawing.Size(444, 26);
            this.cbClasificacionb.TabIndex = 40;
            this.cbClasificacionb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbClasificacionb.SelectedIndexChanged += new System.EventHandler(this.cbClasificacionb_SelectedIndexChanged);
            // 
            // pActualizar
            // 
            this.pActualizar.Controls.Add(this.button4);
            this.pActualizar.Controls.Add(this.label11);
            this.pActualizar.Location = new System.Drawing.Point(1721, 28);
            this.pActualizar.Name = "pActualizar";
            this.pActualizar.Size = new System.Drawing.Size(117, 69);
            this.pActualizar.TabIndex = 77;
            this.pActualizar.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(-2, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 24);
            this.label11.TabIndex = 56;
            this.label11.Text = "Mostrar Todo";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(9, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 24);
            this.label8.TabIndex = 74;
            this.label8.Text = "Nombre de Fallo:";
            // 
            // txtgetcodbusq
            // 
            this.txtgetcodbusq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetcodbusq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetcodbusq.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetcodbusq.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetcodbusq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetcodbusq.Location = new System.Drawing.Point(898, 97);
            this.txtgetcodbusq.MaxLength = 10;
            this.txtgetcodbusq.Name = "txtgetcodbusq";
            this.txtgetcodbusq.ShortcutsEnabled = false;
            this.txtgetcodbusq.Size = new System.Drawing.Size(216, 18);
            this.txtgetcodbusq.TabIndex = 66;
            this.txtgetcodbusq.TextChanged += new System.EventHandler(this.txtgetcodbusq_TextChanged);
            this.txtgetcodbusq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetdescfallo_KeyPress);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(896, 110);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(221, 9);
            this.label18.TabIndex = 65;
            this.label18.Text = "______________________________________________________";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(748, 93);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(147, 24);
            this.label19.TabIndex = 64;
            this.label19.Text = "Código de Fallo:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(723, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(188, 24);
            this.label17.TabIndex = 61;
            this.label17.Text = "Descripción de Fallo:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1580, 77);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 24);
            this.label16.TabIndex = 60;
            this.label16.Text = "Buscar";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(9, 31);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(193, 24);
            this.label15.TabIndex = 43;
            this.label15.Text = "Clasificación de Fallo:";
            // 
            // LblExcel
            // 
            this.LblExcel.AutoSize = true;
            this.LblExcel.Location = new System.Drawing.Point(1412, 76);
            this.LblExcel.Name = "LblExcel";
            this.LblExcel.Size = new System.Drawing.Size(85, 24);
            this.LblExcel.TabIndex = 79;
            this.LblExcel.Text = "Exportar";
            // 
            // tbfallos
            // 
            this.tbfallos.AllowUserToAddRows = false;
            this.tbfallos.AllowUserToDeleteRows = false;
            this.tbfallos.AllowUserToResizeColumns = false;
            this.tbfallos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfallos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbfallos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfallos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfallos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbfallos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfallos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Descripcion,
            this.codfallo,
            this.nomfallo,
            this.usuario,
            this.Estatus,
            this.idclasif,
            this.iddesc});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.DefaultCellStyle = dataGridViewCellStyle3;
            this.tbfallos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbfallos.EnableHeadersVisualStyles = false;
            this.tbfallos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbfallos.Location = new System.Drawing.Point(3, 181);
            this.tbfallos.MultiSelect = false;
            this.tbfallos.Name = "tbfallos";
            this.tbfallos.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tbfallos.RowHeadersVisible = false;
            this.tbfallos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.tbfallos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfallos.ShowCellErrors = false;
            this.tbfallos.ShowCellToolTips = false;
            this.tbfallos.ShowEditingIcon = false;
            this.tbfallos.ShowRowErrors = false;
            this.tbfallos.Size = new System.Drawing.Size(1910, 228);
            this.tbfallos.TabIndex = 1;
            this.tbfallos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfallos_CellContentDoubleClick);
            this.tbfallos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfallos_CellFormatting);
            this.tbfallos.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbfallos_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idclasificación";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "CLASIFICACIÓN DE FALLO";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Descripcion
            // 
            this.Descripcion.HeaderText = "DESCRIPCIÓN DE FALLO";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            this.Descripcion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // codfallo
            // 
            this.codfallo.HeaderText = "CÓDIGO DE FALLO";
            this.codfallo.Name = "codfallo";
            this.codfallo.ReadOnly = true;
            this.codfallo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nomfallo
            // 
            this.nomfallo.HeaderText = "NOMBRE DE FALLO";
            this.nomfallo.Name = "nomfallo";
            this.nomfallo.ReadOnly = true;
            this.nomfallo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // idclasif
            // 
            this.idclasif.HeaderText = "idclasif";
            this.idclasif.Name = "idclasif";
            this.idclasif.ReadOnly = true;
            this.idclasif.Visible = false;
            // 
            // iddesc
            // 
            this.iddesc.HeaderText = "iddesc";
            this.iddesc.Name = "iddesc";
            this.iddesc.ReadOnly = true;
            this.iddesc.Visible = false;
            // 
            // gbaddnomfallo
            // 
            this.gbaddnomfallo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddnomfallo.Controls.Add(this.label6);
            this.gbaddnomfallo.Controls.Add(this.button2);
            this.gbaddnomfallo.Controls.Add(this.label5);
            this.gbaddnomfallo.Controls.Add(this.button1);
            this.gbaddnomfallo.Controls.Add(this.lblcodfallo);
            this.gbaddnomfallo.Controls.Add(this.label4);
            this.gbaddnomfallo.Controls.Add(this.cbdescripcion);
            this.gbaddnomfallo.Controls.Add(this.label3);
            this.gbaddnomfallo.Controls.Add(this.cbclasificacion);
            this.gbaddnomfallo.Controls.Add(this.label2);
            this.gbaddnomfallo.Controls.Add(this.pCancelar);
            this.gbaddnomfallo.Controls.Add(this.lblsavemp);
            this.gbaddnomfallo.Controls.Add(this.btnsavemp);
            this.gbaddnomfallo.Controls.Add(this.txtgetdescfallo);
            this.gbaddnomfallo.Controls.Add(this.label21);
            this.gbaddnomfallo.Controls.Add(this.label22);
            this.gbaddnomfallo.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddnomfallo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddnomfallo.Location = new System.Drawing.Point(552, 12);
            this.gbaddnomfallo.Name = "gbaddnomfallo";
            this.gbaddnomfallo.Size = new System.Drawing.Size(732, 507);
            this.gbaddnomfallo.TabIndex = 32;
            this.gbaddnomfallo.TabStop = false;
            this.gbaddnomfallo.Text = "Agregar Nombre de Fallo";
            this.gbaddnomfallo.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddnomfallo_Paint);
            this.gbaddnomfallo.Enter += new System.EventHandler(this.gbClasificacion_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(566, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 21);
            this.label6.TabIndex = 39;
            this.label6.Text = "Ir a Catálogo";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 21);
            this.label5.TabIndex = 37;
            this.label5.Text = "Ir a Catálogo";
            // 
            // lblcodfallo
            // 
            this.lblcodfallo.AutoSize = true;
            this.lblcodfallo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcodfallo.Location = new System.Drawing.Point(256, 312);
            this.lblcodfallo.Name = "lblcodfallo";
            this.lblcodfallo.Size = new System.Drawing.Size(0, 18);
            this.lblcodfallo.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 21);
            this.label4.TabIndex = 34;
            this.label4.Text = "Código de Fallo:";
            // 
            // cbdescripcion
            // 
            this.cbdescripcion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbdescripcion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbdescripcion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdescripcion.Enabled = false;
            this.cbdescripcion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbdescripcion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbdescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbdescripcion.FormattingEnabled = true;
            this.cbdescripcion.Location = new System.Drawing.Point(256, 162);
            this.cbdescripcion.Name = "cbdescripcion";
            this.cbdescripcion.Size = new System.Drawing.Size(304, 26);
            this.cbdescripcion.TabIndex = 33;
            this.cbdescripcion.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbdescripcion.SelectedIndexChanged += new System.EventHandler(this.cbdescripcion_SelectedIndexChanged);
            this.cbdescripcion.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 21);
            this.label3.TabIndex = 32;
            this.label3.Text = "Descripción de Fallo:";
            // 
            // cbclasificacion
            // 
            this.cbclasificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbclasificacion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbclasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbclasificacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbclasificacion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbclasificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbclasificacion.FormattingEnabled = true;
            this.cbclasificacion.Location = new System.Drawing.Point(256, 80);
            this.cbclasificacion.Name = "cbclasificacion";
            this.cbclasificacion.Size = new System.Drawing.Size(304, 26);
            this.cbclasificacion.TabIndex = 31;
            this.cbclasificacion.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbclasificacion.SelectedIndexChanged += new System.EventHandler(this.cbclasificacion_SelectedIndexChanged);
            this.cbclasificacion.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 21);
            this.label2.TabIndex = 30;
            this.label2.Text = "Clasificación de Fallo:";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(488, 406);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(166, 95);
            this.pCancelar.TabIndex = 29;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(56, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Nuevo";
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(326, 469);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(72, 21);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Guardar";
            // 
            // txtgetdescfallo
            // 
            this.txtgetdescfallo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetdescfallo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetdescfallo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetdescfallo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetdescfallo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetdescfallo.Location = new System.Drawing.Point(256, 243);
            this.txtgetdescfallo.MaxLength = 80;
            this.txtgetdescfallo.Name = "txtgetdescfallo";
            this.txtgetdescfallo.ShortcutsEnabled = false;
            this.txtgetdescfallo.Size = new System.Drawing.Size(304, 18);
            this.txtgetdescfallo.TabIndex = 2;
            this.txtgetdescfallo.TextChanged += new System.EventHandler(this.txtgetdescfallo_TextChanged);
            this.txtgetdescfallo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetdescfallo_KeyPress);
            this.txtgetdescfallo.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetdescfallo_Validating);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(44, 243);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(141, 21);
            this.label21.TabIndex = 0;
            this.label21.Text = "Nombre de Fallo:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(254, 256);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(309, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "____________________________________________________________________________";
            // 
            // pEliminarClasificacion
            // 
            this.pEliminarClasificacion.Controls.Add(this.lbldeletedesc);
            this.pEliminarClasificacion.Controls.Add(this.btndeletedesc);
            this.pEliminarClasificacion.Location = new System.Drawing.Point(616, 418);
            this.pEliminarClasificacion.Name = "pEliminarClasificacion";
            this.pEliminarClasificacion.Size = new System.Drawing.Size(162, 95);
            this.pEliminarClasificacion.TabIndex = 28;
            this.pEliminarClasificacion.Visible = false;
            // 
            // lbldeletedesc
            // 
            this.lbldeletedesc.AutoSize = true;
            this.lbldeletedesc.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletedesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletedesc.Location = new System.Drawing.Point(32, 63);
            this.lbldeletedesc.Name = "lbldeletedesc";
            this.lbldeletedesc.Size = new System.Drawing.Size(89, 21);
            this.lbldeletedesc.TabIndex = 26;
            this.lbldeletedesc.Text = "Desactivar";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Crimson;
            this.label9.Location = new System.Drawing.Point(1391, 505);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 18);
            this.label9.TabIndex = 66;
            this.label9.Text = "Nota:";
            this.label9.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(1430, 505);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // btndeletedesc
            // 
            this.btndeletedesc.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeletedesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeletedesc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeletedesc.FlatAppearance.BorderSize = 0;
            this.btndeletedesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeletedesc.Location = new System.Drawing.Point(54, 3);
            this.btndeletedesc.Name = "btndeletedesc";
            this.btndeletedesc.Size = new System.Drawing.Size(50, 50);
            this.btndeletedesc.TabIndex = 25;
            this.btndeletedesc.UseVisualStyleBackColor = true;
            this.btndeletedesc.Click += new System.EventHandler(this.btndeletedesc_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::controlFallos.Properties.Resources.Imagen2;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(1418, 92);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(384, 141);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // btnExcel
            // 
            this.btnExcel.BackgroundImage = global::controlFallos.Properties.Resources.sobresalir;
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Location = new System.Drawing.Point(1428, 31);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(50, 50);
            this.btnExcel.TabIndex = 57;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::controlFallos.Properties.Resources._1491313940_repeat_82991;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(35, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 50);
            this.button4.TabIndex = 55;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::controlFallos.Properties.Resources.xmag_search_find_export_locate_5984;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(1584, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(47, 37);
            this.button3.TabIndex = 59;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Location = new System.Drawing.Point(1445, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 80;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::controlFallos.Properties.Resources._goto;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(597, 146);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 40);
            this.button2.TabIndex = 38;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources._goto;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(597, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 36;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(60, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(342, 409);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 6;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // catfallosGrales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1908, 937);
            this.Controls.Add(this.pEliminarClasificacion);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gbconsulta);
            this.Controls.Add(this.gbaddnomfallo);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "catfallosGrales";
            this.Text = "catNombresFallos";
            this.Load += new System.EventHandler(this.catNombresFallos_Load);
            this.gbconsulta.ResumeLayout(false);
            this.gbbuscar.ResumeLayout(false);
            this.gbbuscar.PerformLayout();
            this.pActualizar.ResumeLayout(false);
            this.pActualizar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).EndInit();
            this.gbaddnomfallo.ResumeLayout(false);
            this.gbaddnomfallo.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pEliminarClasificacion.ResumeLayout(false);
            this.pEliminarClasificacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbconsulta;
        private System.Windows.Forms.DataGridView tbfallos;
        private System.Windows.Forms.GroupBox gbaddnomfallo;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblcodfallo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cbclasificacion;
        public System.Windows.Forms.ComboBox cbdescripcion;
        private System.Windows.Forms.GroupBox gbbuscar;
        private System.Windows.Forms.TextBox txtgetcodbusq;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn codfallo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomfallo;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idclasif;
        private System.Windows.Forms.DataGridViewTextBoxColumn iddesc;
        private System.Windows.Forms.Panel pActualizar;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox cbDescFallob;
        public System.Windows.Forms.ComboBox cbClasificacionb;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Label LblExcel;
        private System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.ComboBox cbnombrefb;
    }
}
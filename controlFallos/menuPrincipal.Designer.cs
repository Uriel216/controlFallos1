namespace controlFallos
{
    partial class menuPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(menuPrincipal));
            this.lblnotif = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.pbnotif = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.catálogosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeFallosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDePersonalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeProveedoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeUnidadesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeRefaccionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creaciónDeReportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteNivelSupervisiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteNivelMantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteNivelTransisumosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requisicionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarOrdenDeCompraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lblnumnotificaciones = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnotif)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblnotif
            // 
            this.lblnotif.AutoSize = true;
            this.lblnotif.BackColor = System.Drawing.Color.Transparent;
            this.lblnotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblnotif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblnotif.Font = new System.Drawing.Font("Garamond", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnotif.ForeColor = System.Drawing.Color.White;
            this.lblnotif.Location = new System.Drawing.Point(71, 13);
            this.lblnotif.Name = "lblnotif";
            this.lblnotif.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblnotif.Size = new System.Drawing.Size(0, 30);
            this.lblnotif.TabIndex = 2;
            this.toolTip1.SetToolTip(this.lblnotif, "Notificaciones");
            this.lblnotif.Visible = false;
            this.lblnotif.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(665, 11);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(392, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Sistema de Reporte de Fallos - Menú Principal";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Crimson;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Garamond", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1849, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "_";
            this.toolTip1.SetToolTip(this.label3, "Minimizar");
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Le han Llegado Nuevos Reportes (1)";
            this.notifyIcon1.BalloonTipTitle = "Nuevos Reportes de Mantenimiento";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Nuevos Reportes";
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Crimson;
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.lblnotif);
            this.panel3.Controls.Add(this.pbnotif);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 990);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1920, 50);
            this.panel3.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Crimson;
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.home_icon_silhouette;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(940, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 35);
            this.button1.TabIndex = 12;
            this.toolTip1.SetToolTip(this.button1, "Regresar a Menu Principal");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbnotif
            // 
            this.pbnotif.BackColor = System.Drawing.Color.Crimson;
            this.pbnotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbnotif.Image = global::controlFallos.Properties.Resources.notification__3_;
            this.pbnotif.Location = new System.Drawing.Point(64, 0);
            this.pbnotif.Name = "pbnotif";
            this.pbnotif.Size = new System.Drawing.Size(58, 50);
            this.pbnotif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbnotif.TabIndex = 1;
            this.pbnotif.TabStop = false;
            this.toolTip1.SetToolTip(this.pbnotif, "Notificaciones");
            this.pbnotif.Visible = false;
            this.pbnotif.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Crimson;
            this.menuStrip1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.catálogosToolStripMenuItem,
            this.reportesToolStripMenuItem,
            this.requisicionesToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1920, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // catálogosToolStripMenuItem
            // 
            this.catálogosToolStripMenuItem.AutoToolTip = true;
            this.catálogosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.catálogoDeFallosToolStripMenuItem,
            this.catálogoDePersonalToolStripMenuItem,
            this.catálogoDeProveedoresToolStripMenuItem,
            this.catálogoDeUnidadesToolStripMenuItem,
            this.catálogoDeRefaccionesToolStripMenuItem});
            this.catálogosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogosToolStripMenuItem.Image = global::controlFallos.Properties.Resources.catalog__1_;
            this.catálogosToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogosToolStripMenuItem.Name = "catálogosToolStripMenuItem";
            this.catálogosToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 10);
            this.catálogosToolStripMenuItem.Size = new System.Drawing.Size(135, 46);
            this.catálogosToolStripMenuItem.Text = "Catálogos";
            this.catálogosToolStripMenuItem.Visible = false;
            // 
            // catálogoDeFallosToolStripMenuItem
            // 
            this.catálogoDeFallosToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeFallosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeFallosToolStripMenuItem.Image = global::controlFallos.Properties.Resources.bug__1_;
            this.catálogoDeFallosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeFallosToolStripMenuItem.Name = "catálogoDeFallosToolStripMenuItem";
            this.catálogoDeFallosToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeFallosToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.catálogoDeFallosToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDeFallosToolStripMenuItem.Size = new System.Drawing.Size(295, 39);
            this.catálogoDeFallosToolStripMenuItem.Text = "Catálogo de Fallos";
            this.catálogoDeFallosToolStripMenuItem.Visible = false;
            this.catálogoDeFallosToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeFallosToolStripMenuItem_Click);
            // 
            // catálogoDePersonalToolStripMenuItem
            // 
            this.catálogoDePersonalToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDePersonalToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDePersonalToolStripMenuItem.Image = global::controlFallos.Properties.Resources.presentation;
            this.catálogoDePersonalToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDePersonalToolStripMenuItem.Name = "catálogoDePersonalToolStripMenuItem";
            this.catálogoDePersonalToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDePersonalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.catálogoDePersonalToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDePersonalToolStripMenuItem.Size = new System.Drawing.Size(295, 39);
            this.catálogoDePersonalToolStripMenuItem.Text = "Catálogo de Personal";
            this.catálogoDePersonalToolStripMenuItem.Visible = false;
            this.catálogoDePersonalToolStripMenuItem.Click += new System.EventHandler(this.catálogoDePersonalToolStripMenuItem_Click);
            // 
            // catálogoDeProveedoresToolStripMenuItem
            // 
            this.catálogoDeProveedoresToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeProveedoresToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeProveedoresToolStripMenuItem.Image = global::controlFallos.Properties.Resources.businessman__4_;
            this.catálogoDeProveedoresToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeProveedoresToolStripMenuItem.Name = "catálogoDeProveedoresToolStripMenuItem";
            this.catálogoDeProveedoresToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeProveedoresToolStripMenuItem.Size = new System.Drawing.Size(295, 39);
            this.catálogoDeProveedoresToolStripMenuItem.Text = "Catálogo de Proveedores";
            this.catálogoDeProveedoresToolStripMenuItem.Visible = false;
            this.catálogoDeProveedoresToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeProveedoresToolStripMenuItem_Click);
            // 
            // catálogoDeUnidadesToolStripMenuItem
            // 
            this.catálogoDeUnidadesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeUnidadesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeUnidadesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.delivery_truck;
            this.catálogoDeUnidadesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeUnidadesToolStripMenuItem.Name = "catálogoDeUnidadesToolStripMenuItem";
            this.catálogoDeUnidadesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeUnidadesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
            this.catálogoDeUnidadesToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDeUnidadesToolStripMenuItem.Size = new System.Drawing.Size(295, 39);
            this.catálogoDeUnidadesToolStripMenuItem.Text = "Catálogo de Unidades";
            this.catálogoDeUnidadesToolStripMenuItem.Visible = false;
            this.catálogoDeUnidadesToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeUnidadesToolStripMenuItem_Click);
            // 
            // catálogoDeRefaccionesToolStripMenuItem
            // 
            this.catálogoDeRefaccionesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeRefaccionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeRefaccionesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.robber_silhouette_trying_to_steal_car_part__1_;
            this.catálogoDeRefaccionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeRefaccionesToolStripMenuItem.Name = "catálogoDeRefaccionesToolStripMenuItem";
            this.catálogoDeRefaccionesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeRefaccionesToolStripMenuItem.Size = new System.Drawing.Size(295, 39);
            this.catálogoDeRefaccionesToolStripMenuItem.Text = "Catálogo de Refacciones";
            this.catálogoDeRefaccionesToolStripMenuItem.Visible = false;
            this.catálogoDeRefaccionesToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeRefaccionesToolStripMenuItem_Click);
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.creaciónDeReportesToolStripMenuItem});
            this.reportesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reportesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.newspaper;
            this.reportesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 10);
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(120, 43);
            this.reportesToolStripMenuItem.Text = "Reportes";
            this.reportesToolStripMenuItem.Visible = false;
            // 
            // creaciónDeReportesToolStripMenuItem
            // 
            this.creaciónDeReportesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.creaciónDeReportesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporteNivelSupervisiónToolStripMenuItem,
            this.reporteNivelMantenimientoToolStripMenuItem,
            this.reporteNivelTransisumosToolStripMenuItem});
            this.creaciónDeReportesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.creaciónDeReportesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.report;
            this.creaciónDeReportesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.creaciónDeReportesToolStripMenuItem.Name = "creaciónDeReportesToolStripMenuItem";
            this.creaciónDeReportesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.creaciónDeReportesToolStripMenuItem.Size = new System.Drawing.Size(266, 39);
            this.creaciónDeReportesToolStripMenuItem.Text = "Creación de Reportes";
            this.creaciónDeReportesToolStripMenuItem.Visible = false;
            // 
            // reporteNivelSupervisiónToolStripMenuItem
            // 
            this.reporteNivelSupervisiónToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteNivelSupervisiónToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteNivelSupervisiónToolStripMenuItem.Image = global::controlFallos.Properties.Resources.manager;
            this.reporteNivelSupervisiónToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteNivelSupervisiónToolStripMenuItem.Name = "reporteNivelSupervisiónToolStripMenuItem";
            this.reporteNivelSupervisiónToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteNivelSupervisiónToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.reporteNivelSupervisiónToolStripMenuItem.ShowShortcutKeys = false;
            this.reporteNivelSupervisiónToolStripMenuItem.Size = new System.Drawing.Size(275, 39);
            this.reporteNivelSupervisiónToolStripMenuItem.Text = "Reporte Supervisión";
            this.reporteNivelSupervisiónToolStripMenuItem.Visible = false;
            this.reporteNivelSupervisiónToolStripMenuItem.Click += new System.EventHandler(this.reporteNivelSupervisiónToolStripMenuItem_Click);
            // 
            // reporteNivelMantenimientoToolStripMenuItem
            // 
            this.reporteNivelMantenimientoToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteNivelMantenimientoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteNivelMantenimientoToolStripMenuItem.Image = global::controlFallos.Properties.Resources.construction;
            this.reporteNivelMantenimientoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteNivelMantenimientoToolStripMenuItem.Name = "reporteNivelMantenimientoToolStripMenuItem";
            this.reporteNivelMantenimientoToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteNivelMantenimientoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.reporteNivelMantenimientoToolStripMenuItem.ShowShortcutKeys = false;
            this.reporteNivelMantenimientoToolStripMenuItem.Size = new System.Drawing.Size(275, 39);
            this.reporteNivelMantenimientoToolStripMenuItem.Text = "Reporte Mantenimiento";
            this.reporteNivelMantenimientoToolStripMenuItem.Visible = false;
            this.reporteNivelMantenimientoToolStripMenuItem.Click += new System.EventHandler(this.reporteNivelMantenimientoToolStripMenuItem_Click);
            // 
            // reporteNivelTransisumosToolStripMenuItem
            // 
            this.reporteNivelTransisumosToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteNivelTransisumosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteNivelTransisumosToolStripMenuItem.Image = global::controlFallos.Properties.Resources.car_parts;
            this.reporteNivelTransisumosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteNivelTransisumosToolStripMenuItem.Name = "reporteNivelTransisumosToolStripMenuItem";
            this.reporteNivelTransisumosToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteNivelTransisumosToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.reporteNivelTransisumosToolStripMenuItem.ShowShortcutKeys = false;
            this.reporteNivelTransisumosToolStripMenuItem.Size = new System.Drawing.Size(275, 39);
            this.reporteNivelTransisumosToolStripMenuItem.Text = "Reporte Almacén (TRI)";
            this.reporteNivelTransisumosToolStripMenuItem.Visible = false;
            this.reporteNivelTransisumosToolStripMenuItem.Click += new System.EventHandler(this.reporteNivelTransisumosToolStripMenuItem_Click);
            // 
            // requisicionesToolStripMenuItem
            // 
            this.requisicionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarOrdenDeCompraToolStripMenuItem});
            this.requisicionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.requisicionesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.shopping_cart;
            this.requisicionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.requisicionesToolStripMenuItem.Name = "requisicionesToolStripMenuItem";
            this.requisicionesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.requisicionesToolStripMenuItem.Size = new System.Drawing.Size(159, 43);
            this.requisicionesToolStripMenuItem.Text = "Requisiciones";
            this.requisicionesToolStripMenuItem.Visible = false;
            // 
            // generarOrdenDeCompraToolStripMenuItem
            // 
            this.generarOrdenDeCompraToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.generarOrdenDeCompraToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.generarOrdenDeCompraToolStripMenuItem.Image = global::controlFallos.Properties.Resources.shopping_cart__1_;
            this.generarOrdenDeCompraToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.generarOrdenDeCompraToolStripMenuItem.Name = "generarOrdenDeCompraToolStripMenuItem";
            this.generarOrdenDeCompraToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.generarOrdenDeCompraToolStripMenuItem.Size = new System.Drawing.Size(312, 39);
            this.generarOrdenDeCompraToolStripMenuItem.Text = "Generar Orden de Compra";
            this.generarOrdenDeCompraToolStripMenuItem.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImage = global::controlFallos.Properties.Resources.logout;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Crimson;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Crimson;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Crimson;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(1882, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 29);
            this.button2.TabIndex = 13;
            this.toolTip1.SetToolTip(this.button2, "Cerrar Sesión");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lblnumnotificaciones
            // 
            this.lblnumnotificaciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblnumnotificaciones.BackgroundImage = global::controlFallos.Properties.Resources.Imagen2;
            this.lblnumnotificaciones.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblnumnotificaciones.Location = new System.Drawing.Point(0, 53);
            this.lblnumnotificaciones.Name = "lblnumnotificaciones";
            this.lblnumnotificaciones.Size = new System.Drawing.Size(1920, 937);
            this.lblnumnotificaciones.TabIndex = 8;
            // 
            // menuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Crimson;
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblnumnotificaciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "menuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "menuPrincipal";
            this.Load += new System.EventHandler(this.menuPrincipal_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnotif)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Panel lblnumnotificaciones;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.PictureBox pbnotif;
        public System.Windows.Forms.Label lblnotif;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem catálogosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeFallosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDePersonalToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeUnidadesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem creaciónDeReportesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reporteNivelSupervisiónToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reporteNivelMantenimientoToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reporteNivelTransisumosToolStripMenuItem;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeRefaccionesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem requisicionesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem generarOrdenDeCompraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem catálogoDeProveedoresToolStripMenuItem;
    }
}
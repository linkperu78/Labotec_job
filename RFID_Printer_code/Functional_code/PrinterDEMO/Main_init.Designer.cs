namespace PrinterDEMO
{
    partial class Main_init
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_init));
            this.btn_exit = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dPIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baseDeDatosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_restart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_pic = new System.Windows.Forms.Button();
            this.Btn_NewFile = new System.Windows.Forms.Button();
            this.btn_rfid = new System.Windows.Forms.Button();
            this.btn_barcode = new System.Windows.Forms.Button();
            this.btn_text = new System.Windows.Forms.Button();
            this.btn_PrintFile = new System.Windows.Forms.Button();
            this.btn_SaveFile = new System.Windows.Forms.Button();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tb_status = new System.Windows.Forms.TextBox();
            this.print_panel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.main_panel = new System.Windows.Forms.Panel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDialog2 = new System.Windows.Forms.PrintDialog();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.main_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(4, 4);
            this.btn_exit.Margin = new System.Windows.Forms.Padding(4);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(120, 30);
            this.btn_exit.TabIndex = 6;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1081, 28);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dPIToolStripMenuItem,
            this.speedToolStripMenuItem,
            this.baseDeDatosToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // dPIToolStripMenuItem
            // 
            this.dPIToolStripMenuItem.Name = "dPIToolStripMenuItem";
            this.dPIToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.dPIToolStripMenuItem.Text = "Label";
            this.dPIToolStripMenuItem.Click += new System.EventHandler(this.dPIToolStripMenuItem_Click);
            // 
            // speedToolStripMenuItem
            // 
            this.speedToolStripMenuItem.Name = "speedToolStripMenuItem";
            this.speedToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.speedToolStripMenuItem.Text = "Speed";
            // 
            // baseDeDatosToolStripMenuItem
            // 
            this.baseDeDatosToolStripMenuItem.Name = "baseDeDatosToolStripMenuItem";
            this.baseDeDatosToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.baseDeDatosToolStripMenuItem.Text = "Base de datos";
            this.baseDeDatosToolStripMenuItem.Click += new System.EventHandler(this.baseDeDatosToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripMenuItem,
            this.manualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(141, 26);
            this.versionToolStripMenuItem.Text = "Version";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(141, 26);
            this.manualToolStripMenuItem.Text = "Manual";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(155, 4);
            this.btn_Connect.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(120, 30);
            this.btn_Connect.TabIndex = 6;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // btn_restart
            // 
            this.btn_restart.Location = new System.Drawing.Point(308, 4);
            this.btn_restart.Margin = new System.Windows.Forms.Padding(4);
            this.btn_restart.Name = "btn_restart";
            this.btn_restart.Size = new System.Drawing.Size(120, 30);
            this.btn_restart.TabIndex = 6;
            this.btn_restart.Text = "Restart";
            this.btn_restart.UseVisualStyleBackColor = true;
            this.btn_restart.Click += new System.EventHandler(this.btn_restart_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.btn_pic);
            this.panel1.Controls.Add(this.Btn_NewFile);
            this.panel1.Controls.Add(this.btn_rfid);
            this.panel1.Controls.Add(this.btn_barcode);
            this.panel1.Controls.Add(this.btn_text);
            this.panel1.Controls.Add(this.btn_PrintFile);
            this.panel1.Controls.Add(this.btn_SaveFile);
            this.panel1.Controls.Add(this.btn_OpenFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 56);
            this.panel1.TabIndex = 12;
            // 
            // btn_pic
            // 
            this.btn_pic.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_pic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_pic.BackgroundImage")));
            this.btn_pic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_pic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_pic.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_pic.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_pic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pic.Location = new System.Drawing.Point(678, 5);
            this.btn_pic.Name = "btn_pic";
            this.btn_pic.Size = new System.Drawing.Size(48, 48);
            this.btn_pic.TabIndex = 5;
            this.btn_pic.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_pic.UseVisualStyleBackColor = false;
            this.btn_pic.Click += new System.EventHandler(this.btn_pic_Click);
            // 
            // Btn_NewFile
            // 
            this.Btn_NewFile.BackColor = System.Drawing.SystemColors.MenuBar;
            this.Btn_NewFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Btn_NewFile.BackgroundImage")));
            this.Btn_NewFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Btn_NewFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_NewFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Wheat;
            this.Btn_NewFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightYellow;
            this.Btn_NewFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_NewFile.Location = new System.Drawing.Point(5, 5);
            this.Btn_NewFile.Name = "Btn_NewFile";
            this.Btn_NewFile.Size = new System.Drawing.Size(48, 48);
            this.Btn_NewFile.TabIndex = 1;
            this.Btn_NewFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Btn_NewFile.UseVisualStyleBackColor = false;
            this.Btn_NewFile.Click += new System.EventHandler(this.Btn_NewFile_Click);
            // 
            // btn_rfid
            // 
            this.btn_rfid.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_rfid.BackgroundImage = global::PrinterDEMO.Properties.Resources.RFID_icon;
            this.btn_rfid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_rfid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_rfid.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_rfid.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_rfid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rfid.Location = new System.Drawing.Point(772, 5);
            this.btn_rfid.Name = "btn_rfid";
            this.btn_rfid.Size = new System.Drawing.Size(48, 48);
            this.btn_rfid.TabIndex = 4;
            this.btn_rfid.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_rfid.UseVisualStyleBackColor = false;
            this.btn_rfid.Click += new System.EventHandler(this.btn_rfid_Click);
            // 
            // btn_barcode
            // 
            this.btn_barcode.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_barcode.BackgroundImage = global::PrinterDEMO.Properties.Resources.codebar_icon;
            this.btn_barcode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_barcode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_barcode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_barcode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_barcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_barcode.Location = new System.Drawing.Point(572, 5);
            this.btn_barcode.Name = "btn_barcode";
            this.btn_barcode.Size = new System.Drawing.Size(48, 48);
            this.btn_barcode.TabIndex = 4;
            this.btn_barcode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_barcode.UseVisualStyleBackColor = false;
            this.btn_barcode.Click += new System.EventHandler(this.btn_barcode_Click);
            // 
            // btn_text
            // 
            this.btn_text.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_text.BackgroundImage = global::PrinterDEMO.Properties.Resources.Text_icon;
            this.btn_text.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_text.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_text.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_text.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_text.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_text.Location = new System.Drawing.Point(480, 5);
            this.btn_text.Name = "btn_text";
            this.btn_text.Size = new System.Drawing.Size(48, 48);
            this.btn_text.TabIndex = 4;
            this.btn_text.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_text.UseVisualStyleBackColor = false;
            this.btn_text.Click += new System.EventHandler(this.btn_text_Click);
            // 
            // btn_PrintFile
            // 
            this.btn_PrintFile.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_PrintFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_PrintFile.BackgroundImage")));
            this.btn_PrintFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_PrintFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_PrintFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_PrintFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_PrintFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PrintFile.Location = new System.Drawing.Point(228, 5);
            this.btn_PrintFile.Name = "btn_PrintFile";
            this.btn_PrintFile.Size = new System.Drawing.Size(48, 48);
            this.btn_PrintFile.TabIndex = 4;
            this.btn_PrintFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_PrintFile.UseVisualStyleBackColor = false;
            this.btn_PrintFile.Click += new System.EventHandler(this.btn_PrintFile_Click);
            // 
            // btn_SaveFile
            // 
            this.btn_SaveFile.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_SaveFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_SaveFile.BackgroundImage")));
            this.btn_SaveFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_SaveFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_SaveFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_SaveFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_SaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveFile.Location = new System.Drawing.Point(153, 5);
            this.btn_SaveFile.Name = "btn_SaveFile";
            this.btn_SaveFile.Size = new System.Drawing.Size(48, 48);
            this.btn_SaveFile.TabIndex = 3;
            this.btn_SaveFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_SaveFile.UseVisualStyleBackColor = false;
            this.btn_SaveFile.Click += new System.EventHandler(this.btn_SaveFile_Click);
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btn_OpenFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_OpenFile.BackgroundImage")));
            this.btn_OpenFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_OpenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_OpenFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_OpenFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_OpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OpenFile.Location = new System.Drawing.Point(77, 5);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(48, 48);
            this.btn_OpenFile.TabIndex = 2;
            this.btn_OpenFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_OpenFile.UseVisualStyleBackColor = false;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.tb_status);
            this.panel4.Controls.Add(this.btn_Connect);
            this.panel4.Controls.Add(this.btn_restart);
            this.panel4.Controls.Add(this.btn_exit);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 556);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1081, 40);
            this.panel4.TabIndex = 15;
            // 
            // tb_status
            // 
            this.tb_status.BackColor = System.Drawing.Color.Red;
            this.tb_status.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_status.Location = new System.Drawing.Point(709, 7);
            this.tb_status.Margin = new System.Windows.Forms.Padding(1);
            this.tb_status.Name = "tb_status";
            this.tb_status.Size = new System.Drawing.Size(150, 27);
            this.tb_status.TabIndex = 8;
            this.tb_status.Text = "DESCONECTADO";
            this.tb_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_status.TextChanged += new System.EventHandler(this.tb_status_TextChanged);
            // 
            // print_panel
            // 
            this.print_panel.BackColor = System.Drawing.Color.White;
            this.print_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.print_panel.Location = new System.Drawing.Point(80, 60);
            this.print_panel.Margin = new System.Windows.Forms.Padding(0);
            this.print_panel.Name = "print_panel";
            this.print_panel.Size = new System.Drawing.Size(870, 343);
            this.print_panel.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 84);
            this.panel3.MaximumSize = new System.Drawing.Size(0, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1081, 10);
            this.panel3.TabIndex = 14;
            // 
            // main_panel
            // 
            this.main_panel.BackColor = System.Drawing.Color.Silver;
            this.main_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.main_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.main_panel.Controls.Add(this.print_panel);
            this.main_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_panel.Location = new System.Drawing.Point(0, 94);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(1081, 462);
            this.main_panel.TabIndex = 17;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDialog2
            // 
            this.printDialog2.UseEXDialog = true;
            // 
            // Main_init
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1081, 596);
            this.Controls.Add(this.main_panel);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main_init";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFID Printer - LABOTEC";
            this.Load += new System.EventHandler(this.Main_init_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.main_panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dPIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_restart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel print_panel;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Button Btn_NewFile;
        private System.Windows.Forms.Button btn_PrintFile;
        private System.Windows.Forms.Button btn_SaveFile;
        private System.Windows.Forms.Button btn_rfid;
        private System.Windows.Forms.Button btn_barcode;
        private System.Windows.Forms.Button btn_text;
        private System.Windows.Forms.ToolStripMenuItem baseDeDatosToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_status;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel main_panel;
        private System.Windows.Forms.Button btn_pic;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintDialog printDialog2;
    }
}


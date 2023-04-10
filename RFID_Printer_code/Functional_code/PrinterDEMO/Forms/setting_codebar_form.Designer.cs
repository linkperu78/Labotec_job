namespace PrinterDEMO
{
    partial class setting_codebar_form
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bc_btn_cancel = new System.Windows.Forms.Button();
            this.bc_btn_ok = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bc_tb_value = new System.Windows.Forms.TextBox();
            this.bc_cb_format = new System.Windows.Forms.ComboBox();
            this.bc_cb_source = new System.Windows.Forms.ComboBox();
            this.bc_cb_header = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_item = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 208);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PrinterDEMO.Properties.Resources.codebar_icon;
            this.pictureBox1.Location = new System.Drawing.Point(223, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(345, 142);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 47);
            this.panel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(132, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(559, 36);
            this.label3.TabIndex = 0;
            this.label3.Text = "Configuración - CODIGO DE BARRAS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(94, 307);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Valor :";
            // 
            // bc_btn_cancel
            // 
            this.bc_btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bc_btn_cancel.Location = new System.Drawing.Point(672, 400);
            this.bc_btn_cancel.Name = "bc_btn_cancel";
            this.bc_btn_cancel.Size = new System.Drawing.Size(116, 38);
            this.bc_btn_cancel.TabIndex = 5;
            this.bc_btn_cancel.Text = "CANCEL";
            this.bc_btn_cancel.UseVisualStyleBackColor = true;
            this.bc_btn_cancel.Click += new System.EventHandler(this.bc_btn_cancel_Click);
            // 
            // bc_btn_ok
            // 
            this.bc_btn_ok.Location = new System.Drawing.Point(531, 400);
            this.bc_btn_ok.Name = "bc_btn_ok";
            this.bc_btn_ok.Size = new System.Drawing.Size(116, 38);
            this.bc_btn_ok.TabIndex = 5;
            this.bc_btn_ok.Text = "OK";
            this.bc_btn_ok.UseVisualStyleBackColor = true;
            this.bc_btn_ok.Click += new System.EventHandler(this.bc_btn_ok_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(59, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Formato :";
            // 
            // bc_tb_value
            // 
            this.bc_tb_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bc_tb_value.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.bc_tb_value.Location = new System.Drawing.Point(209, 307);
            this.bc_tb_value.Name = "bc_tb_value";
            this.bc_tb_value.Size = new System.Drawing.Size(150, 30);
            this.bc_tb_value.TabIndex = 0;
            this.bc_tb_value.TextChanged += new System.EventHandler(this.tb_value_TextChanged);
            // 
            // bc_cb_format
            // 
            this.bc_cb_format.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bc_cb_format.FormattingEnabled = true;
            this.bc_cb_format.Items.AddRange(new object[] {
            "Code 128 A",
            "Code 128 B",
            "Code 128 C",
            "EAN-13",
            "EAN-8",
            "UPC Interleaved 2 of 5",
            "UPC-A",
            "UPC-E"});
            this.bc_cb_format.Location = new System.Drawing.Point(209, 253);
            this.bc_cb_format.Name = "bc_cb_format";
            this.bc_cb_format.Size = new System.Drawing.Size(185, 33);
            this.bc_cb_format.TabIndex = 6;
            this.bc_cb_format.Text = "Code 128 A";
            this.bc_cb_format.SelectedIndexChanged += new System.EventHandler(this.bc_cb_format_SelectedIndexChanged);
            // 
            // bc_cb_source
            // 
            this.bc_cb_source.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bc_cb_source.FormattingEnabled = true;
            this.bc_cb_source.Items.AddRange(new object[] {
            "Fijo",
            "Base de datos"});
            this.bc_cb_source.Location = new System.Drawing.Point(570, 254);
            this.bc_cb_source.Name = "bc_cb_source";
            this.bc_cb_source.Size = new System.Drawing.Size(154, 33);
            this.bc_cb_source.TabIndex = 6;
            this.bc_cb_source.Text = "Fijo";
            this.bc_cb_source.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // bc_cb_header
            // 
            this.bc_cb_header.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bc_cb_header.FormattingEnabled = true;
            this.bc_cb_header.Location = new System.Drawing.Point(570, 304);
            this.bc_cb_header.Name = "bc_cb_header";
            this.bc_cb_header.Size = new System.Drawing.Size(121, 33);
            this.bc_cb_header.TabIndex = 6;
            this.bc_cb_header.SelectedIndexChanged += new System.EventHandler(this.bc_cb_header_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(445, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fuente :";
            // 
            // label_item
            // 
            this.label_item.AutoSize = true;
            this.label_item.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_item.Location = new System.Drawing.Point(474, 304);
            this.label_item.Name = "label_item";
            this.label_item.Size = new System.Drawing.Size(76, 29);
            this.label_item.TabIndex = 3;
            this.label_item.Text = "Item :";
            // 
            // setting_codebar_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bc_btn_cancel;
            this.ClientSize = new System.Drawing.Size(802, 453);
            this.Controls.Add(this.bc_cb_header);
            this.Controls.Add(this.bc_cb_source);
            this.Controls.Add(this.bc_cb_format);
            this.Controls.Add(this.bc_btn_ok);
            this.Controls.Add(this.bc_btn_cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_item);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bc_tb_value);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(820, 500);
            this.Name = "setting_codebar_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bc_btn_cancel;
        private System.Windows.Forms.Button bc_btn_ok;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox bc_tb_value;
        private System.Windows.Forms.ComboBox bc_cb_format;
        private System.Windows.Forms.ComboBox bc_cb_source;
        private System.Windows.Forms.ComboBox bc_cb_header;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_item;
    }
}
using System.Windows.Forms;

namespace PrinterDEMO
{
    partial class setting_text_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(setting_text_form));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_inputText = new System.Windows.Forms.TextBox();
            this.cb_underline = new System.Windows.Forms.CheckBox();
            this.cb_cursive = new System.Windows.Forms.CheckBox();
            this.cb_bold = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label_header = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_header = new System.Windows.Forms.ComboBox();
            this.cb_reference = new System.Windows.Forms.ComboBox();
            this.cb_font = new System.Windows.Forms.ComboBox();
            this.cb_size = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_btn_cancel = new System.Windows.Forms.Button();
            this.txt_btn_ok = new System.Windows.Forms.Button();
            this.tb_previewText = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.tb_inputText);
            this.panel1.Controls.Add(this.cb_underline);
            this.panel1.Controls.Add(this.cb_cursive);
            this.panel1.Controls.Add(this.cb_bold);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label_header);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cb_header);
            this.panel1.Controls.Add(this.cb_reference);
            this.panel1.Controls.Add(this.cb_font);
            this.panel1.Controls.Add(this.cb_size);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_btn_cancel);
            this.panel1.Controls.Add(this.txt_btn_ok);
            this.panel1.Controls.Add(this.tb_previewText);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 426);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Font = new System.Drawing.Font("Arial", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(205, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(347, 29);
            this.label6.TabIndex = 7;
            this.label6.Text = "CONFIGURACION DEL TEXTO";
            // 
            // tb_inputText
            // 
            this.tb_inputText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_inputText.Location = new System.Drawing.Point(21, 164);
            this.tb_inputText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_inputText.Name = "tb_inputText";
            this.tb_inputText.Size = new System.Drawing.Size(401, 30);
            this.tb_inputText.TabIndex = 6;
            this.tb_inputText.TextChanged += new System.EventHandler(this.tb_inputText_TextChanged);
            // 
            // cb_underline
            // 
            this.cb_underline.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_underline.AutoSize = true;
            this.cb_underline.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cb_underline.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cb_underline.BackgroundImage")));
            this.cb_underline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cb_underline.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.cb_underline.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_underline.FlatAppearance.BorderSize = 3;
            this.cb_underline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_underline.Location = new System.Drawing.Point(179, 124);
            this.cb_underline.Margin = new System.Windows.Forms.Padding(5);
            this.cb_underline.MinimumSize = new System.Drawing.Size(25, 25);
            this.cb_underline.Name = "cb_underline";
            this.cb_underline.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_underline.Size = new System.Drawing.Size(25, 25);
            this.cb_underline.TabIndex = 5;
            this.cb_underline.UseVisualStyleBackColor = false;
            // 
            // cb_cursive
            // 
            this.cb_cursive.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_cursive.AutoSize = true;
            this.cb_cursive.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cb_cursive.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cb_cursive.BackgroundImage")));
            this.cb_cursive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cb_cursive.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.cb_cursive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_cursive.FlatAppearance.BorderSize = 3;
            this.cb_cursive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_cursive.Location = new System.Drawing.Point(143, 124);
            this.cb_cursive.Margin = new System.Windows.Forms.Padding(5);
            this.cb_cursive.MinimumSize = new System.Drawing.Size(25, 25);
            this.cb_cursive.Name = "cb_cursive";
            this.cb_cursive.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_cursive.Size = new System.Drawing.Size(25, 25);
            this.cb_cursive.TabIndex = 5;
            this.cb_cursive.UseVisualStyleBackColor = false;
            // 
            // cb_bold
            // 
            this.cb_bold.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_bold.AutoSize = true;
            this.cb_bold.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cb_bold.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cb_bold.BackgroundImage")));
            this.cb_bold.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cb_bold.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.cb_bold.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_bold.FlatAppearance.BorderSize = 3;
            this.cb_bold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_bold.Location = new System.Drawing.Point(107, 123);
            this.cb_bold.Margin = new System.Windows.Forms.Padding(5);
            this.cb_bold.MinimumSize = new System.Drawing.Size(25, 25);
            this.cb_bold.Name = "cb_bold";
            this.cb_bold.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_bold.Size = new System.Drawing.Size(25, 25);
            this.cb_bold.TabIndex = 5;
            this.cb_bold.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fuente";
            // 
            // label_header
            // 
            this.label_header.AutoSize = true;
            this.label_header.Location = new System.Drawing.Point(555, 174);
            this.label_header.Name = "label_header";
            this.label_header.Size = new System.Drawing.Size(43, 16);
            this.label_header.TabIndex = 4;
            this.label_header.Text = "Indice";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(524, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Referencia";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(349, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tamaño";
            // 
            // cb_header
            // 
            this.cb_header.FormattingEnabled = true;
            this.cb_header.Location = new System.Drawing.Point(613, 174);
            this.cb_header.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_header.Name = "cb_header";
            this.cb_header.Size = new System.Drawing.Size(121, 24);
            this.cb_header.TabIndex = 3;
            this.cb_header.SelectedIndexChanged += new System.EventHandler(this.cb_header_SelectedIndexChanged);
            // 
            // cb_reference
            // 
            this.cb_reference.FormattingEnabled = true;
            this.cb_reference.Location = new System.Drawing.Point(613, 114);
            this.cb_reference.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_reference.Name = "cb_reference";
            this.cb_reference.Size = new System.Drawing.Size(121, 24);
            this.cb_reference.TabIndex = 3;
            this.cb_reference.SelectedIndexChanged += new System.EventHandler(this.cb_reference_SelectedIndexChanged);
            // 
            // cb_font
            // 
            this.cb_font.FormattingEnabled = true;
            this.cb_font.Location = new System.Drawing.Point(212, 124);
            this.cb_font.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_font.Name = "cb_font";
            this.cb_font.Size = new System.Drawing.Size(125, 24);
            this.cb_font.TabIndex = 3;
            this.cb_font.SelectedIndexChanged += new System.EventHandler(this.cb_font_SelectedIndexChanged);
            // 
            // cb_size
            // 
            this.cb_size.FormattingEnabled = true;
            this.cb_size.Location = new System.Drawing.Point(353, 124);
            this.cb_size.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_size.Name = "cb_size";
            this.cb_size.Size = new System.Drawing.Size(71, 24);
            this.cb_size.TabIndex = 3;
            this.cb_size.SelectedIndexChanged += new System.EventHandler(this.cb_size_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 26);
            this.label4.TabIndex = 2;
            this.label4.Text = "Texto";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Preview :";
            // 
            // txt_btn_cancel
            // 
            this.txt_btn_cancel.Location = new System.Drawing.Point(635, 370);
            this.txt_btn_cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_btn_cancel.Name = "txt_btn_cancel";
            this.txt_btn_cancel.Size = new System.Drawing.Size(99, 34);
            this.txt_btn_cancel.TabIndex = 1;
            this.txt_btn_cancel.Text = "Cancelar";
            this.txt_btn_cancel.UseVisualStyleBackColor = true;
            this.txt_btn_cancel.Click += new System.EventHandler(this.txt_btn_cancel_Click);
            // 
            // txt_btn_ok
            // 
            this.txt_btn_ok.Location = new System.Drawing.Point(476, 370);
            this.txt_btn_ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_btn_ok.Name = "txt_btn_ok";
            this.txt_btn_ok.Size = new System.Drawing.Size(99, 34);
            this.txt_btn_ok.TabIndex = 1;
            this.txt_btn_ok.Text = "Aceptar";
            this.txt_btn_ok.UseVisualStyleBackColor = true;
            this.txt_btn_ok.Click += new System.EventHandler(this.txt_btn_ok_Click);
            // 
            // tb_previewText
            // 
            this.tb_previewText.Location = new System.Drawing.Point(21, 274);
            this.tb_previewText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_previewText.MinimumSize = new System.Drawing.Size(120, 70);
            this.tb_previewText.Name = "tb_previewText";
            this.tb_previewText.ReadOnly = true;
            this.tb_previewText.Size = new System.Drawing.Size(732, 22);
            this.tb_previewText.TabIndex = 0;
            // 
            // setting_text_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "setting_text_form";
            this.Text = "Setting Text";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_header;
        private System.Windows.Forms.ComboBox cb_reference;
        private System.Windows.Forms.ComboBox cb_font;
        private System.Windows.Forms.ComboBox cb_size;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button txt_btn_cancel;
        private System.Windows.Forms.Button txt_btn_ok;
        private System.Windows.Forms.TextBox tb_previewText;
        private System.Windows.Forms.Label label_header;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_bold;
        private System.Windows.Forms.CheckBox cb_underline;
        private System.Windows.Forms.CheckBox cb_cursive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_inputText;
        private System.Windows.Forms.Label label6;
    }
}
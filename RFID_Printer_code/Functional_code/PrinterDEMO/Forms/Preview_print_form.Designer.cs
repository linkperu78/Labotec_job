namespace PrinterDEMO
{
    partial class Preview_print_form
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.print_btn_preview = new System.Windows.Forms.Button();
            this.preview_panel = new System.Windows.Forms.Panel();
            this.Prev_button = new System.Windows.Forms.Button();
            this.Next_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_max = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tb_index_label = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_rfid = new System.Windows.Forms.Button();
            this.btn_bc = new System.Windows.Forms.Button();
            this.btn_txt = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(637, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 43);
            this.button1.TabIndex = 2;
            this.button1.Text = "Imprimir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(773, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancelar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // print_btn_preview
            // 
            this.print_btn_preview.Font = new System.Drawing.Font("Arial Narrow", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.print_btn_preview.Location = new System.Drawing.Point(12, 4);
            this.print_btn_preview.Name = "print_btn_preview";
            this.print_btn_preview.Size = new System.Drawing.Size(126, 42);
            this.print_btn_preview.TabIndex = 3;
            this.print_btn_preview.Text = "Preview";
            this.print_btn_preview.UseVisualStyleBackColor = true;
            this.print_btn_preview.Click += new System.EventHandler(this.print_btn_preview_Click);
            // 
            // preview_panel
            // 
            this.preview_panel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.preview_panel.Location = new System.Drawing.Point(107, 50);
            this.preview_panel.Name = "preview_panel";
            this.preview_panel.Size = new System.Drawing.Size(695, 282);
            this.preview_panel.TabIndex = 4;
            // 
            // Prev_button
            // 
            this.Prev_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prev_button.Location = new System.Drawing.Point(205, 8);
            this.Prev_button.Name = "Prev_button";
            this.Prev_button.Size = new System.Drawing.Size(40, 29);
            this.Prev_button.TabIndex = 5;
            this.Prev_button.Text = "<";
            this.Prev_button.UseVisualStyleBackColor = true;
            this.Prev_button.Click += new System.EventHandler(this.Prev_button_Click);
            // 
            // Next_button
            // 
            this.Next_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Next_button.Location = new System.Drawing.Point(413, 9);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(40, 29);
            this.Next_button.TabIndex = 6;
            this.Next_button.Text = ">";
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.Next_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(329, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 27);
            this.label1.TabIndex = 7;
            this.label1.Text = "/";
            // 
            // lb_max
            // 
            this.lb_max.AutoSize = true;
            this.lb_max.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_max.Location = new System.Drawing.Point(356, 10);
            this.lb_max.Name = "lb_max";
            this.lb_max.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lb_max.Size = new System.Drawing.Size(23, 25);
            this.lb_max.TabIndex = 9;
            this.lb_max.Text = "?";
            this.lb_max.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.tb_index_label);
            this.panel2.Controls.Add(this.print_btn_preview);
            this.panel2.Controls.Add(this.lb_max);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.Prev_button);
            this.panel2.Controls.Add(this.Next_button);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 411);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(904, 55);
            this.panel2.TabIndex = 10;
            // 
            // tb_index_label
            // 
            this.tb_index_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_index_label.Location = new System.Drawing.Point(294, 8);
            this.tb_index_label.Name = "tb_index_label";
            this.tb_index_label.Size = new System.Drawing.Size(29, 30);
            this.tb_index_label.TabIndex = 10;
            this.tb_index_label.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_index_label_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.btn_rfid);
            this.panel1.Controls.Add(this.btn_bc);
            this.panel1.Controls.Add(this.btn_txt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 32);
            this.panel1.TabIndex = 11;
            // 
            // btn_rfid
            // 
            this.btn_rfid.Location = new System.Drawing.Point(239, 7);
            this.btn_rfid.Name = "btn_rfid";
            this.btn_rfid.Size = new System.Drawing.Size(84, 22);
            this.btn_rfid.TabIndex = 13;
            this.btn_rfid.Text = "RFID";
            this.btn_rfid.UseVisualStyleBackColor = true;
            this.btn_rfid.Click += new System.EventHandler(this.btn_rfid_Click);
            // 
            // btn_bc
            // 
            this.btn_bc.Location = new System.Drawing.Point(128, 7);
            this.btn_bc.Name = "btn_bc";
            this.btn_bc.Size = new System.Drawing.Size(84, 22);
            this.btn_bc.TabIndex = 12;
            this.btn_bc.Text = "Barcodes";
            this.btn_bc.UseVisualStyleBackColor = true;
            // 
            // btn_txt
            // 
            this.btn_txt.Location = new System.Drawing.Point(13, 7);
            this.btn_txt.Name = "btn_txt";
            this.btn_txt.Size = new System.Drawing.Size(84, 22);
            this.btn_txt.TabIndex = 11;
            this.btn_txt.Text = "Texto";
            this.btn_txt.UseVisualStyleBackColor = true;
            this.btn_txt.Click += new System.EventHandler(this.btn_txt_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel3.Controls.Add(this.preview_panel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(904, 379);
            this.panel3.TabIndex = 12;
            // 
            // Preview_print_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 466);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Preview_print_form";
            this.Text = "Form_Print";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button print_btn_preview;
        private System.Windows.Forms.Panel preview_panel;
        private System.Windows.Forms.Button Prev_button;
        private System.Windows.Forms.Button Next_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_max;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tb_index_label;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_bc;
        private System.Windows.Forms.Button btn_txt;
        private System.Windows.Forms.Button btn_rfid;
        private System.Windows.Forms.Panel panel3;
    }
}
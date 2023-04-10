namespace PrinterDEMO
{
    partial class database_excel_form
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
            this.btn_upload = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.gv_excel_data = new System.Windows.Forms.DataGridView();
            this.db_cb_qty = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gv_excel_data)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(48, 52);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(100, 40);
            this.btn_upload.TabIndex = 0;
            this.btn_upload.Text = "Seleccionar Archivo";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(794, 410);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(136, 46);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(960, 410);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(117, 46);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // gv_excel_data
            // 
            this.gv_excel_data.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.gv_excel_data.AllowUserToAddRows = false;
            this.gv_excel_data.AllowUserToResizeRows = false;
            this.gv_excel_data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_excel_data.Location = new System.Drawing.Point(217, 32);
            this.gv_excel_data.Name = "gv_excel_data";
            this.gv_excel_data.ReadOnly = true;
            this.gv_excel_data.RowHeadersWidth = 51;
            this.gv_excel_data.RowTemplate.Height = 24;
            this.gv_excel_data.Size = new System.Drawing.Size(847, 372);
            this.gv_excel_data.TabIndex = 3;
            // 
            // db_cb_qty
            // 
            this.db_cb_qty.FormattingEnabled = true;
            this.db_cb_qty.Location = new System.Drawing.Point(48, 202);
            this.db_cb_qty.Name = "db_cb_qty";
            this.db_cb_qty.Size = new System.Drawing.Size(100, 24);
            this.db_cb_qty.TabIndex = 4;
            this.db_cb_qty.SelectedIndexChanged += new System.EventHandler(this.db_cb_qty_SelectedIndexChanged);
            // 
            // database_excel_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 485);
            this.Controls.Add(this.db_cb_qty);
            this.Controls.Add(this.gv_excel_data);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_upload);
            this.Name = "database_excel_form";
            this.Text = "database_excel_form";
            ((System.ComponentModel.ISupportInitialize)(this.gv_excel_data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridView gv_excel_data;
        private System.Windows.Forms.ComboBox db_cb_qty;
    }
}
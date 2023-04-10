using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterDEMO
{
    public partial class setting_rfid_form : Form
    {
        public static bool enable_rfid;
        public static int ref_UPC_header;
        private bool ref_show;
        public setting_rfid_form()
        {
            InitializeComponent();
            init_status();
        }

        private void setting_rfid_form_Load(object sender, EventArgs e)
        {
            this.Text = "Configuración del RFID/UHF Tag";

        }

        private void init_status() 
        {
            enable_rfid = false;
            ref_show = false;
            ref_UPC_header = 0;

            cb_type.SelectedIndex = 0;
            cb_type.Text = "";
            cb_header.Enabled = false;
            label_ref.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {

            ref_UPC_header = cb_header.SelectedIndex;
            enable_rfid = true;

            this.DialogResult= DialogResult.OK;
        }

        private void cb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_type.SelectedIndex == 1)
            {
                if (class_excel_data.this_exist)
                {
                    ref_show = true;
                    cb_header.Items.Clear();
                    add_header_list();
                }
                else
                {
                    MessageBox.Show("No hay base de datos cargada");
                    cb_type.SelectedIndex = 0;
                }

            }
            else
            {
                ref_show = false;
            }
            label_ref.Enabled = ref_show;
            cb_header.Enabled = ref_show;
        }

        private void add_header_list()
        {
            string[] db_headers = class_excel_data.header;
            for (int m = 0; m < db_headers.Length; m++)
            {
                cb_header.Items.Add(db_headers[m]);

            }
            cb_header.SelectedIndex = 0;
        }

        private void cb_header_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pos_ = cb_header.SelectedIndex;
            tb_text.Text = class_excel_data.data_excel[0][pos_];
        }
    }
}

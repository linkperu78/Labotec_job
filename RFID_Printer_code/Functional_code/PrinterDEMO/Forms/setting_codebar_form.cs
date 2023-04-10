using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterDEMO
{
    public partial class setting_codebar_form : Form
    {
        // Valores de paso
        private string[] format_names_barcode = {"1A","1B","1C","E30","E80","2U","UA0","UE0"};
        private string bc_format="";
        private int pos_lib_format = 0;
        private string bc_value="";
        private bool this_barcode_exists;


        // Almacenamiento interno de datos
        private string[] inner_values;
        public int index_bc;


        public setting_codebar_form(int index)
        {
            InitializeComponent();
            //this.ControlBox = false;
            this.Text = string.Empty;

            // Revision de datos segun indice en datalist_barcodes_format
            this.index_bc = index;
            int barcodes_existentes = Main_init.datalist_barcodes_format.Count;   

            this.Text = "CODIGO DE BARRAS [N°" + (index+1).ToString() + "]";  // Titulo
            //{"index","format","UPC_reference"};

            // Evaluamos si existe ya este codigo, para recuperar sus valores
            if (index < barcodes_existentes)
            {
                this_barcode_exists = true;                     // Ya se creo este codigo de barras
                inner_values = Main_init.
                         datalist_barcodes_format[index_bc];    // Recuperamos sus valores
                string actual_format = inner_values[0];         // Recuperamos el formato actual del UPC

                int index_format = 0;                           // Buscamos el equivalente de su formato
                                                                // simplificado
                for (int i = 0; i < format_names_barcode.Length; i++) {
                    if (format_names_barcode[i].Equals(actual_format)) 
                    {
                        index_format = i; 
                    }
                }
                bc_cb_format.SelectedIndex = index_format;      // Mostramos el formato extendido del UPC

                int pos_cb_source = int.Parse(inner_values[1]);
                pos_lib_format = Main_init.pos_format_lib[index_bc];

                bool state_db = false;

                bc_cb_source.SelectedIndex = pos_cb_source;     // Referencia del valor

                switch (pos_cb_source) 
                {
                    case 0:
                        bc_tb_value.Text = inner_values[2];
                        break;
                    case 1:
                        state_db = true;
                        int index_db_ref = int.Parse(inner_values[2]);
                        bc_cb_header.SelectedIndex = index_db_ref;
                        bc_tb_value.Text = class_excel_data.data_excel[0][index_db_ref];
                        break;
                }
                bc_cb_header.Visible = state_db;
                label_item.Visible = state_db;
            }

            // Si no existe este codigo de barras, iniciamos con valores vacios
            else {
                this_barcode_exists = false;                // Señalamos que no existe este BC
                inner_values = new string[3] { "", "", ""}; // Valores iniciales vacios
                bc_tb_value.Text = "";
                bc_cb_source.SelectedIndex=0;
                bc_cb_format.SelectedIndex = 6;
                pos_lib_format = 6;
                bc_format = "UA0";
                label_item.Visible = false;
                bc_cb_header.Visible = false;
            }

            // El usuario no puede modificar libremente estos ComboBox
            bc_cb_format.DropDownStyle = ComboBoxStyle.DropDownList;    
            bc_cb_source.DropDownStyle = ComboBoxStyle.DropDownList;
            bc_cb_header.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void bc_btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(bc_cb_source.SelectedIndex == 1)
            {
                bc_cb_header.Visible = true;
                label_item.Visible = true;
                if (class_excel_data.this_exist)
                { 
                    bc_cb_header.Items.Clear();
                    add_header_list();
                }
                bc_tb_value.Enabled = false;
            }
            else
            {
                bc_cb_header.Visible = false;
                label_item.Visible = false;
                bc_tb_value.Enabled = true;
            }
        }

        private void tb_value_TextChanged(object sender, EventArgs e)
        {
            bc_value = bc_tb_value.Text;
        }

        private void bc_cb_format_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = bc_cb_format.SelectedIndex;
            bc_type_select(j);
            bc_format = format_names_barcode[j];
            pos_lib_format = j;
        }

        private void bc_type_select(int barcode_type_index) 
        {
            switch (barcode_type_index)
            {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                case 4: bc_tb_value.MaxLength = 8; break;
                case 5: bc_tb_value.MaxLength = 13; break;
                case 6: bc_tb_value.MaxLength = 12; break;
                case 7: bc_tb_value.MaxLength = 12; break;
            }
        }

        private void bc_btn_ok_Click(object sender, EventArgs e)
        {
            string Error_message = "Falta:"+"\r\n";
            bool empty_value = false;
            if (bc_value.Length<1) {

                Error_message += "Valor de Codigo de Barras";
                empty_value = true;
                return;
            }

            if (empty_value) {
                MessageBox.Show(Error_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (bc_cb_source.SelectedIndex == 1 && !class_excel_data.this_exist)
            {
                Error_message = "No se ha ingresado una base de datos";
                MessageBox.Show(Error_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            inner_values[0] = bc_format;
            int ref_UPC_format = bc_cb_source.SelectedIndex;
            inner_values[1] = ref_UPC_format.ToString();
            switch (ref_UPC_format) 
            {
                case 0:
                    inner_values[2] = bc_tb_value.Text;
                    break;
                case 1:
                    inner_values[2] = bc_cb_header.SelectedIndex.ToString();
                    break;
            }
            


            if (this_barcode_exists) {
                Main_init.datalist_barcodes_format[index_bc] = inner_values;
                Main_init.pos_format_lib[index_bc] = pos_lib_format;
            }
            else {
                Main_init.datalist_barcodes_format.Add(inner_values);
                Main_init.pos_format_lib.Add(pos_lib_format);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void add_header_list() 
        {
            string[] db_headers = class_excel_data.header;
            for (int m = 0; m < db_headers.Length; m++) 
            {
                bc_cb_header.Items.Add(db_headers[m]);
                
            }
            bc_cb_header.SelectedIndex = 0;
        }

        private void bc_cb_header_SelectedIndexChanged(object sender, EventArgs e)
        {
            string previous_value = bc_tb_value.Text;

            int selected_index = bc_cb_header.SelectedIndex;
            string new_value = class_excel_data.data_excel[0][selected_index];
            int j_temp;

            bc_tb_value.Text = new_value;
            bool a_1, a_2 = true;
            if (new_value.Length > 10) 
            { 
                a_1 = int.TryParse(new_value.Substring(0,9), out j_temp);
                a_2 = int.TryParse(new_value.Substring(9), out j_temp);   
            }
            else
                a_1 = int.TryParse(new_value, out j_temp);


            if (a_1 == false || a_2 == false)
            {
                MessageBox.Show("Its no a number, pick another option","Error",MessageBoxButtons.OK);
                bc_tb_value.Text = previous_value;
                return;
            }
        }
    }
}

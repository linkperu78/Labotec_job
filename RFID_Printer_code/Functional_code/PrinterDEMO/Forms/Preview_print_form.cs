using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using PrinterDEMO.Class;

namespace PrinterDEMO
{
    public partial class Preview_print_form : Form
    {
        private string mensaje_total = "";

        private List<string[]> verified_data = new List<string[]>();
        private List<string[]> raw_data = new List<string[]>();

        private int total_print_number;
        int ref_number;

        int[] array_times;
        bool charged_labels = false;
        int actual_label_value;

        // Aqui guardamos los valores de los barcodes y textos variables
        // indice - format - valor_ref 
        List<string[]> var_barcode = new List<string[]>();
        // indice - texto_ref
        List<string[]> var_text = new List<string[]>();

        int count_barcode = 0;
        int count_text = 0;
        int pos_rfid_header;
        bool e_rfid;

        int delay_print = 0;
        
        public Preview_print_form(Main_init principal_form1)
        {
            InitializeComponent();
            preview_panel.Enabled = false;
            total_print_number = 0;
            ref_number = 0;

            var_barcode.Add(new string[] {"index","format","ref"});
            var_text.Add(new string[] { "index", "ref" });
            configure_label();
            hide_navigator();
            init_scan_number_index();
        }

        // Configuramos la impresora segun los datos ingresados
        private void configure_label()
        {
            float width      = (float)Main_init.label_width;
            float height     = (float)Main_init.label_height;
            float spacing    = (float)Main_init.label_spacing;
            float dpi        = (float)Main_init.dpi/25.4f;
            uint label_height    = (uint)(height * dpi + 0.5f);
            uint label_width     = (uint)(width * dpi + 0.5f);
            uint label_gap       = (uint)(spacing * dpi + 0.5f);
            ZMPCL.ZM_SetPrintSpeed(Main_init.speed);
            ZMPCL.ZM_SetDarkness(15);
            ZMPCL.ZM_SetLabelWidth(label_width);
            ZMPCL.ZM_SetLabelHeight(label_height, label_gap);

        }


        // Filtramos aquellos datos que no son numeros
        // en la columna de "repeticiones"
        // y obtenemos el array de numero de repeticiones
        private void init_scan_number_index()
        {
            ref_number = class_excel_data.pos_number_print;
            raw_data = class_excel_data.data_excel;

            if (raw_data == null || ref_number == null)
            {
                total_print_number = 1;
                lb_max.Text = total_print_number.ToString();
                charged_labels = true;
                return; 
            }

            int raw_number_data = raw_data.Count;
            array_times=new int[0];
            int temp_value = 0;
            int count = 0;

            for (int i = 0; i < raw_number_data; i++) 
            {
                string[] verified = raw_data[i];
                if (!(int.TryParse(verified[ref_number], out temp_value)))
                    continue;
                verified_data.Add(verified);
                count++;
                total_print_number += temp_value;
                Array.Resize(ref array_times, count);
                array_times[count - 1] = temp_value;
            }
            lb_max.Text = total_print_number.ToString();
            tb_index_label.MaxLength 
                        = Convert.ToInt32(Math.Log10(total_print_number))+1;
            charged_labels = true;

        }

        // Generamos la lista a imprimir
        // "    total_data_imprimir     "
        private void generate_preview_label(int pos_header) 
        {   // pos_header = posicion del N° solicitado para previsualizar
            List<string[]> bc_import_data = Main_init.barcode_parametros;
            List<string[]> txt_import_data = Main_init.texto_parametros;
            List<string[]> pic_import_data = Main_init.picture_parametros;

            // Mostramos los codigos de barra
            for (int j = 0; j < bc_import_data.Count; j++)
            {
                // pos header es el label N° pos_header de L_max labels
                generar_barcode(bc_import_data[j],pos_header);
            }

            // Mostramos los textos generados
            for (int j = 0; j < txt_import_data.Count; j++)
            {
                generar_textlabel(txt_import_data[j], pos_header);
            }

            for (int j = 0; j < pic_import_data.Count; j++)
            {
                generar_picturebox(pic_import_data[j], pos_header);
            }

        }

        private void update_preview_panel(int pos_header) 
        {
            int total_controls = this.preview_panel.Controls.Count;
            foreach (Control control in preview_panel.Controls) 
            {
                string name = control.Tag.ToString();
                if (!name.Contains("_"))
                    continue;
                string[] tag_name = name.Split('_');
                int number = int.Parse(tag_name[1]);
                if (tag_name[0].Equals("BC"))
                {
                    string[] data_upload = var_barcode[number];
                    int UPC_pos = int.Parse(data_upload[2]);
                    string UPC_number = verified_data[pos_header][UPC_pos];
                    PictureBox a = control as PictureBox;
                    a.Image = new class_img_barcode(int.Parse(data_upload[1]),
                                UPC_number).get_image(1,false);

                }
                else if (tag_name[0].Equals("TXT"))
                {
                    string[] data_upload = var_text[number];
                    int text_pos = int.Parse(data_upload[1]);
                    Label a = control as Label;
                    a.Text = verified_data[pos_header][text_pos];

                }
            }
        }

        private void generar_barcode(string[] input_data, int count_ref) 
        {
            int xLocation   = int.Parse(input_data[0])*
                            this.preview_panel.Width/Main_init.p_x_max;
            int yLocation   = int.Parse(input_data[1])*
                            this.preview_panel.Height/Main_init.p_y_max; ;
            int espesor     = int.Parse(input_data[2]);
            int altura      = int.Parse(input_data[3]);
            int pos_list_format = int.Parse(input_data[4]);
            string format   = input_data[5];

            string header_position = input_data[6];
            string UPC_value    = input_data[7];
            string UPC_number   = "";

            Point point = new Point(xLocation, yLocation);
            PictureBox pictureBox   = new PictureBox();
            pictureBox.Location     = point;
            pictureBox.Size         = new Size(100, 100);
            pictureBox.SizeMode     = PictureBoxSizeMode.StretchImage;

            if (header_position.Equals("0"))
            {
                UPC_number = UPC_value;
                pictureBox.Tag = "BC";
            }
            else 
            {
                count_barcode++;
                int position = int.Parse(UPC_value);
                UPC_number=(verified_data[count_ref][position]);
                string[] add_list = new string[] {count_barcode.ToString(),
                                        pos_list_format.ToString(),UPC_value};
                var_barcode.Add(add_list);
                pictureBox.Tag = "BC_" + count_barcode.ToString();
            }

            pictureBox.Image = 
                new class_img_barcode(pos_list_format,UPC_number).get_image(1,false);
            pictureBox.Size = new Size(espesor*Main_init.min_width_rising, altura);
            preview_panel.Controls.Add(pictureBox);
        }

        private void generar_textlabel(string[] input_data, int count_ref) 
        {
            int xLocation   = int.Parse(input_data[0])*
                            this.preview_panel.Width / Main_init.p_x_max;
            int yLocation   = int.Parse(input_data[1]) *
                            this.preview_panel.Height / Main_init.p_y_max;
            string Font     = input_data[2];
            int Size        = int.Parse(input_data[3]);
            bool enable_B   = bool.Parse(input_data[4]);
            bool enable_K   = bool.Parse(input_data[5]);
            bool enable_S   = bool.Parse(input_data[6]);
            string indexed  = input_data[7];
            string text_ref = input_data[8];
            string text_value;

            Point point = new Point(xLocation, yLocation);
            Label label = new Label();
            label.Location = point;
            label.AutoSize = true;

            if (indexed.Equals("0"))
            {
                text_value = text_ref;
                label.Tag = "TXT";
            }
            else
            {
                count_text++;
                int position = int.Parse(text_ref);
                text_value = (verified_data[count_ref][position]);
                string[] add_list = new string[] {count_text.ToString(),
                                                  text_ref};
                var_text.Add(add_list);
                label.Tag = "TXT_" + count_text.ToString();
            }

            FontStyle font_options = new FontStyle();
            if (enable_B)
                font_options = font_options | FontStyle.Bold;
            if (enable_K)
                font_options = font_options | FontStyle.Italic;
            if (enable_S)
                font_options = font_options | FontStyle.Underline;

            label.Font = new Font(Font, Size, font_options, GraphicsUnit.Point);
            label.Text = text_value;
            preview_panel.Controls.Add(label);
        }

        private void generar_picturebox(string[] input_data, int count_ref) 
        {
            int xLocation   = int.Parse(input_data[0]) *
                                this.preview_panel.Width / Main_init.p_x_max;
            int yLocation   = int.Parse(input_data[1]) *
                                this.preview_panel.Height / Main_init.p_y_max;
            int xSize       = int.Parse(input_data[2]);
            int ySize       = int.Parse(input_data[3]);
            string ref_path = input_data[4];
            Image img = Image.FromFile(ref_path);
            
            PictureBox pb_icon = new PictureBox();
            this.preview_panel.Controls.Add(pb_icon);
            pb_icon.Image = img;
            pb_icon.Location = new Point(xLocation, yLocation);
            pb_icon.Size = new Size(xSize, ySize);
        }

        private int get_index_times_array(int count)  
        {
            int position;
            int res = count;

            for (position = 0; position < array_times.Length; position++)
            {
                res -= array_times[position];
                if (res <= 0)
                    break;
            }
            return position;
        }
        private void print_btn_preview_Click(object sender, EventArgs e)
        {
            if (!class_excel_data.this_exist) {
                MessageBox.Show("No hay base de datos","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            generate_preview_label(0);
            show_navigator();
            tb_index_label.Text = "1";
            actual_label_value = 1;
        }

//  ---------------------------------------------------------------------------------------------------
//  ---------------------------------------------------------------------------------------------------
        private string upload_data_print_item(List<string[]> data_type)
        {
            string temp_mensaje_total = "";
            if (data_type.Count < 1) { return temp_mensaje_total; }
            for (int i = 0; i < data_type.Count; i++)
            {
                String[] temp_data = data_type[i];
                for (int m = 0; m < temp_data.Length; m++)
                {
                    temp_mensaje_total += temp_data[m] + ", ";
                }

                temp_mensaje_total += "\r\n";
            }
            temp_mensaje_total += "------------------\r\n";
            return temp_mensaje_total;
        }

        // When click on "IMPRIMIR" Button
        private void button1_Click(object sender, EventArgs e)
        {
            if(Main_init.RFID_parametros.Length>1)
                e_rfid = bool.Parse(Main_init.RFID_parametros[0]);
            if (e_rfid)
                pos_rfid_header = int.Parse(Main_init.RFID_parametros[1]);
            set_variables();                // Set actual pic variables
            int pos_row;
            for (int i = 1; i < total_print_number+1; i++)
            {
                pos_row = (raw_data == null) ? 0 : get_index_times_array(i);  
           
                ZMPCL.ZM_ClearBuffer();
                print_label_number_of(pos_row);
                print_icons();
                if (e_rfid)
                    add_rfid_value(i,pos_row);
                ZMPCL.ZM_PrintLabel(1, 1);
            }   
        }


        private void print_label_number_of(int n_label) 
        {
            int pos_row = n_label;
            // Los codigos de barra
            for (int i = 0; i < Main_init.barcode_parametros.Count; i++)
            {
                String[] temp_data = Main_init.barcode_parametros[i];
                uint px = uint.Parse(temp_data[0]);
                uint py = uint.Parse(temp_data[1]);
                uint espesor = uint.Parse(temp_data[2]);
                uint h = uint.Parse(temp_data[3]);
                string format = temp_data[5];

                string indexed = temp_data[6];
                string UPC_ref = temp_data[7];
                string UPC_value;

                if (indexed.Equals("0"))
                    UPC_value = UPC_ref;
                else
                    UPC_value = verified_data[pos_row][int.Parse(UPC_ref)];

                ZMPCL.ZM_DrawBarcode(px, py, 0, format, espesor, 
                                     espesor, h, 'N', UPC_value);
            }

            // Los textos
            for (int i = 0; i < Main_init.texto_parametros.Count; i++)
            {
                String[] temp_data = Main_init.texto_parametros[i];
                int px          = int.Parse(temp_data[0]);
                int py          = int.Parse(temp_data[1]);
                string format   = temp_data[2];
                int fH          = Convert.ToInt32(float.Parse(temp_data[3])*Main_init.dpmm*1.6);
                int w           = temp_data[4].Equals("True") ? 800 : 400;
                bool italic     = temp_data[5].Equals("True") ? true : false;
                bool under      = temp_data[6].Equals("True") ? true : false;

                string indexed  = temp_data[7];
                string txt_ref  = temp_data[8];
                string txt_value;

                if (indexed.Equals("0"))
                    txt_value = txt_ref;
                else
                    txt_value = verified_data[pos_row][int.Parse(txt_ref)];

                ZMPCL.ZM_DrawTextTrueTypeW(px, py, fH, 0, format, 1, w, italic, under, false, "A" + i.ToString(), txt_value);
            }

        }

        private void print_icons() 
        {
            
            int npic = Main_init.number_pic;
            if (npic == 0)
                return;
            for (int n = 0; n < npic; n++)
            {

                string[] temp_data = Main_init.picture_parametros[n];
                uint xpos = uint.Parse(temp_data[0]);
                uint ypos = uint.Parse(temp_data[1]);
                string variable = "PCX" + n.ToString();
                ZMPCL.ZM_DrawPcxGraphics(xpos,ypos,variable);
            }
            DateTime current = DateTime.Now;
            int print_delay = 100;
            while (current.AddMilliseconds(print_delay) > DateTime.Now)
            { }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void set_variables() 
        {
            int npic = Main_init.number_pic;
            if (npic == 0)
                return;
            for (int n = 0; n < npic; n++)
            { 
                string variable = "PCX" + n.ToString();
                string value = Main_init.picture_parametros[n][4];
                value = value.Split('.')[0] + ".pcx";
                ZMPCL.ZM_PcxGraphicsDownload(variable, value);
            }
        }

        private void hide_navigator() 
        {
            lb_max.Hide();
            Next_button.Hide();
            Prev_button.Hide();
            label1.Hide();
            tb_index_label.Hide();
        }
        private void show_navigator() 
        {
            lb_max.Show();
            Next_button.Show();
            Prev_button.Show();
            label1.Show();
            tb_index_label.Show();
        }

        private void tb_index_label_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.KeyCode == Keys.Enter) || !charged_labels ||
                                tb_index_label.Text.Length == 0)
                return;

            int k = 0;
            bool is_a_number = int.TryParse(tb_index_label.Text, out k);
            if (!is_a_number)
            {
                MessageBox.Show("Insert numbers ONLY", "Not a number",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }

            if (k > total_print_number)
                    k = total_print_number;

            tb_index_label.Text = k.ToString();
            actual_label_value = k;
            // Obtenemos el valor de la fila en que se encuentra
            // el indice ingresado para previsualizar
            k = get_index_times_array(k);
            if (preview_panel.Controls.Count > 0)
                update_preview_panel(k);
            else
                generate_preview_label(k);

        }

        private void Next_button_Click(object sender, EventArgs e)
        {
            actual_label_value++;
            if (actual_label_value > total_print_number) 
                actual_label_value = total_print_number;
            tb_index_label.Text = actual_label_value.ToString();
            update_preview_panel(get_index_times_array(actual_label_value));

        }

        private void Prev_button_Click(object sender, EventArgs e)
        {
            actual_label_value--;
            if (actual_label_value < 1)
                actual_label_value = 1;
            tb_index_label.Text = actual_label_value.ToString();
            update_preview_panel(get_index_times_array(actual_label_value));
        }

        private void btn_txt_Click(object sender, EventArgs e)
        {
            string msg = upload_data_print_item(Main_init.texto_parametros);
            MessageBox.Show(msg);
        }

        private string UPC_to_EPC(string UPC_value, int iteracion)
        {
            int high_EPC    = int.Parse(UPC_value.Substring(0, 6)) * 4;
            int low_EPC     = int.Parse(UPC_value.Substring(6, 5)) * 4;
            string EPC_h    = Convert.ToString(high_EPC, 16).PadLeft(6,'0');
            string EPC_l    = Convert.ToString(low_EPC, 16).PadLeft(5,'0');
            string EPC = "3034" + EPC_h + EPC_l + iteracion.ToString().PadLeft(9, '0');
            return EPC;
        }

        private void btn_rfid_Click(object sender, EventArgs e)
        {
            pos_rfid_header = int.Parse(Main_init.RFID_parametros[1]);
            int row = 0;
            int iteracion = 1;
            string UPC = raw_data[row][pos_rfid_header];
            string epc = UPC_to_EPC(UPC, iteracion);
            MessageBox.Show(epc);
        }

        private void add_rfid_value(int iteracion,int row)
        {
            string UPC = raw_data[row][pos_rfid_header];
            string epc = UPC_to_EPC(UPC, iteracion);
            int epc_length = epc.Length / 2;
            ZMPCL.ZM_RW_RfidFormat(1, 0, 0, epc_length, 1, epc);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace PrinterDEMO
{
    public partial class setting_text_form : Form
    {
        // Valores que vamos a almacenar para imprimir
        private bool    is_b_checked;
        private bool    is_k_checked;
        private bool    is_s_checked;
        private string  text_value;
        private string  text_font;
        private int     text_size;
        private int     index_text;
        private string  is_indexed;
        private string  ref_index;

        

        // Valores de inicializacion
        
        private bool this_text_exists;


        Color unchecked_color, checked_color;
        int unchecked_bordersize, checked_bordersize;
        InstalledFontCollection ifc;
        StringFormat sf;
        
        public setting_text_form(int index)
        { 
            InitializeComponent();
            this.index_text = index;

            initial_values_checkbox();  // valores de uncheck and check
            
            int text_existentes;
            this.AcceptButton = txt_btn_ok;
            this.CancelButton = txt_btn_cancel;
            
            // Set the start position of the form to the center of the screen.
            this.StartPosition = FormStartPosition.CenterScreen;
            ifc = new InstalledFontCollection();

            setting_cb_font_values();   // seteamos la lista de nombres de FONTS
            setting_cb_size_values();   // seteamos la lista de valores de SIZE
            setting_cb_index();         // valores de las opciones de header
            setting_txt_preview();      // Seteamos propiedades del preview textbox

            // Formato para centrar los nombres de la fuentes
            // al mostrar la lista de opciones
            sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;

            this.cb_font.DrawMode = DrawMode.OwnerDrawFixed;
            this.cb_font.Height = 18;
            this.cb_font.DrawItem += new DrawItemEventHandler(this.cb_font_draw_item);
            this.cb_font.MeasureItem += new MeasureItemEventHandler                 (this.cb_front_measure_item);
            this.cb_bold.MouseClick += new MouseEventHandler(change_check);
            this.cb_cursive.MouseClick += new MouseEventHandler(change_check);
            this.cb_underline.MouseClick += new MouseEventHandler(change_check);


            // Identificamos si este text_setting_form ya se establecio previamente

            text_existentes = Main_init.number_text;


            this.Text = "TEXTO [N°" + (index_text).ToString() + "]";
            this_text_exists = false;


            if (text_existentes > index_text)
                this_text_exists = true;

            // Valores iniciales al abrir el setting_text_form
            initial_status();
        }

        private void change_check(object sender, MouseEventArgs e)
        {
            CheckBox cb_option_text = sender as CheckBox;
            string option_name = cb_option_text.Name.Split('_')[1];
            bool state = cb_option_text.Checked;
            if (state) {
                cb_option_text.FlatAppearance.BorderColor = checked_color;
                cb_option_text.FlatAppearance.BorderSize = checked_bordersize;
            }
            else
            {
                cb_option_text.FlatAppearance.BorderColor = unchecked_color;
                cb_option_text.FlatAppearance.BorderSize = unchecked_bordersize;
            }

            if (option_name.Equals("bold"))
            {
                is_b_checked = !is_b_checked;
            }
            else if (option_name.Equals("cursive")) {
                is_k_checked = !is_k_checked;
            }
            else if (option_name.Equals("underline"))
            {
                is_s_checked = !is_s_checked;
            }
            upload_preview_text();
        }


        private void initial_values_checkbox()
        {
            unchecked_color = Color.DarkGray;
            checked_color = Color.DarkBlue;
            checked_bordersize = 2;
            unchecked_bordersize = 2;
        }

        private void initial_status() {
            cb_bold.FlatAppearance.BorderSize = unchecked_bordersize;
            cb_bold.FlatAppearance.BorderColor = unchecked_color;
            cb_cursive.FlatAppearance.BorderSize = unchecked_bordersize;
            cb_cursive.FlatAppearance.BorderColor = unchecked_color;
            cb_underline.FlatAppearance.BorderSize = unchecked_bordersize;
            cb_underline.FlatAppearance.BorderColor = unchecked_color;
            if (this_text_exists)
            {
                string[] inner_values = Main_init.datalist_text[index_text];
            //  { "index","font","size","bold","italic","underline","ref","value"}
                is_indexed = inner_values[6];
                if (inner_values[3].Equals("True"))
                {   is_b_checked = true;
                    cb_bold.FlatAppearance.BorderColor = checked_color;
                    cb_bold.FlatAppearance.BorderSize = checked_bordersize;
                }
                else
                    is_b_checked = false;
                
                if (inner_values[4].Equals("True"))
                {
                    is_k_checked = true;
                    cb_cursive.FlatAppearance.BorderColor = checked_color;
                    cb_cursive.FlatAppearance.BorderSize = checked_bordersize;
                }
                else
                    is_k_checked = false;
                
                if (inner_values[5].Equals("True"))
                {
                    is_s_checked = true;
                    cb_underline.FlatAppearance.BorderColor = checked_color;
                    cb_underline.FlatAppearance.BorderSize = checked_bordersize;
                }
                else
                    is_s_checked = false;

                cb_font.Text = inner_values[1];
                double mm_size = double.Parse(inner_values[2])*3.77952;
                int px_size = Convert.ToInt32(mm_size);

                cb_size.Text = px_size.ToString();

                cb_bold.Checked = is_b_checked;
                cb_cursive.Checked = is_k_checked;
                cb_underline.Checked = is_s_checked;

                if (is_indexed.Equals("0"))
                {
                    cb_reference.SelectedIndex = 0;
                    tb_inputText.Text = inner_values[7];
                }
                else
                {
                    cb_reference.SelectedIndex = 1;
                    cb_header.SelectedIndex = 1;
                    int pos_data = int.Parse(inner_values[7]);
                    cb_header.SelectedIndex = pos_data;
                    tb_inputText.Text = class_excel_data.data_excel[0][pos_data];

                }
                    change_visible_status(is_indexed);

            }
            else 
            {
                cb_bold.Checked = false;
                cb_cursive.Checked = false;
                cb_underline.Checked = false;
                cb_font.SelectedIndex = 4;
                cb_size.SelectedIndex = 7;
                tb_inputText.Text = "Test";
                is_indexed = "0";
                cb_reference.SelectedIndex = 0;
            }


            upload_preview_text();

        }
        private void setting_txt_preview() 
        {
            tb_previewText.MaximumSize = tb_previewText.Size;
            tb_previewText.TextAlign = HorizontalAlignment.Center;
            
        }

        // Actualizamos la vista previa del texto
        private void upload_preview_text() {
            if (tb_inputText.Text.Length < 1) { tb_previewText.Text = ""; return; }

            System.Drawing.FontStyle font_options = new FontStyle();
            if (is_b_checked) { font_options = font_options | FontStyle.Bold; }
            if (is_k_checked) { font_options = font_options | FontStyle.Italic; }
            if (is_s_checked) { font_options = font_options | FontStyle.Underline; }

            int size = int.Parse(cb_size.Text);
            tb_previewText.Text = tb_inputText.Text;
            tb_previewText.Font =
                    new System.Drawing.Font(cb_font.Text, float.Parse(cb_size.Text), font_options, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }

        // Funcion para rellenar las opciones del combobox
        // con los nomnbes de las columnas del excel
        private void setting_cb_index() 
        {
            cb_reference.Items.Add("Fijo");
            cb_reference.Items.Add("Base de datos");
            if (!class_excel_data.this_exist) 
                return;
            int count_data = class_excel_data.header.Length;
            for (int i = 0; i < count_data; i++) 
            {
                string add_me = class_excel_data.header[i];
                cb_header.Items.Add(add_me);
            }

        }

        // Funcion para rellenar las opciones del combobox
        // de opciones para "Font"
        private void setting_cb_font_values() 
        {
            FontFamily[] families = ifc.Families;
            bool add = false;
            for (int i = 0; i < families.Length; i++)
            {
                FontFamily fm = families[i];
                add = fm.Name.ToString().Length <= 16;

                if (!add) {continue; }
                string style = fm.Name.ToString();
                cb_font.Items.Add(style);
            }
        }

        // Funcion para agregar la lista del tamaño de letras
        private void setting_cb_size_values()
        {
            for (int i = 6; i < 12; i++)
            {
                string size = i.ToString();
                cb_size.Items.Add(size);
            }
            for (int i = 12; i < 29; i=i+2)
            {
                string size = i.ToString();
                cb_size.Items.Add(size);
            }
            for (int i = 30; i < 65; i=i+5)
            {
                string size = i.ToString();
                cb_size.Items.Add(size);
            }
            for (int i = 70; i < 110; i = i + 10)
            {
                string size = i.ToString();
                cb_size.Items.Add(size);
            }
        }
        private void cb_font_SelectedIndexChanged(object sender, EventArgs e)
        {
            upload_preview_text();
        }
        private void cb_size_SelectedIndexChanged(object sender, EventArgs e)
        {
            upload_preview_text();
        }
        private void tb_inputText_TextChanged(object sender, EventArgs e)
        {
            upload_preview_text();
        }
        private void cb_font_draw_item(object sender, DrawItemEventArgs e)
        {

            if (Object.ReferenceEquals(null, cb_font))
                return;

            e.DrawBackground();

            if (e.Index >= 0)
            {
                Graphics g = e.Graphics;
                string style = cb_font.Items[e.Index].ToString();
                Font font = new Font(style, 8, e.Font.Style);

                using (Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                                      ? new SolidBrush(SystemColors.Highlight)
                                      : new SolidBrush(e.BackColor))
                {
                    using (Brush textBrush = new SolidBrush(e.ForeColor))
                    {
                        g.FillRectangle(brush, e.Bounds);

                        g.DrawString(cb_font.Items[e.Index].ToString(),
                                     font,
                                     textBrush,
                                     e.Bounds,
                                     sf);
                    }
                }
            }

            e.DrawFocusRectangle();
        }
        private void cb_front_measure_item(object sender, MeasureItemEventArgs e) 
        {
            e.ItemHeight = 17;
        }

        private void cb_reference_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_reference.SelectedIndex == 0)
            {
                is_indexed = "0";
                disable_options();
            }
            else 
            {
                is_indexed = "1";
                enable_options();
            }

        }

        private void change_visible_status(string a) 
        {
            if (a.Equals("1"))
                enable_options();
            else
                disable_options();


        }

        private void disable_options() 
        {
            label_header.Hide();
            cb_header.Hide();
            tb_inputText.Enabled = true;
        }
        private void enable_options()
        {
            label_header.Show();
            cb_header.Show();
            tb_inputText.Enabled = false;

        }

        private void cb_header_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = get_value_from_data_excel(cb_header.SelectedIndex);
            tb_inputText.Text = value;
        }

        private string get_value_from_data_excel(int position) 
        {
            string name = class_excel_data.data_excel[0][position];
            return name;
        }
        private void txt_btn_ok_Click(object sender, EventArgs e)
        {
            if (tb_previewText.Text.Length < 1) { return; }
            text_value = tb_previewText.Text ;
            text_font=cb_font.Text;
            
            text_size=int.Parse(cb_size.Text);      // Tamaño de la letra en PCdots
            string text_size_mm = (text_size * 0.2645833).ToString("0.00");

            if (cb_reference.SelectedIndex == 0)
            {
                ref_index = "0";
                text_value = tb_inputText.Text;
            }
            else
            {
                ref_index = "1";
                text_value = cb_header.SelectedIndex.ToString();
            }


        //  {"index","font",,"size","bold","italic","underline","ref","value"}
            string[] data_text = { index_text.ToString(), text_font,
                                    text_size_mm,
                                    is_b_checked.ToString(),
                                    is_k_checked.ToString(), 
                                    is_s_checked.ToString(),
                                    ref_index,text_value };

            if (this_text_exists)
                Main_init.datalist_text[index_text] = data_text;
            else 
            {
                Main_init.datalist_text.Add(data_text);
                Main_init.number_text++;
            }
            
            this.DialogResult = DialogResult.OK;
        }

        private void txt_btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}

using System;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Serialization;
using BarcodeLib;
using System.Security.Cryptography;
using PrinterDEMO.Modified_class;
using System.Drawing.Printing;

namespace PrinterDEMO
{
    public partial class Main_init : Form
    {
        // Valores propios del programa
        public static int p_x_max;      // Ancho del print_panel en pixeles
        public static int p_y_max;      // Altura del print_panel en pixlees
        int p_x_min;                    // Margen x
        int p_y_min;                    // Margen y
        double b_max;                   // es el ANCHO en dots de la etiqueta RFID
        double h_max;                   // es la ALTURA en dots de la etiqueta RFID

        // Variable donde se guardara la posicion del archivo .exe
        public string project_path;

        // Variables donde se guardan los datos de
        // las dimensiones de etiqueta RFID
        public static int label_height;
        public static int label_width;
        public static int label_spacing;

        public static int dpi = 300;
        public static uint speed = 15;
        public static uint step = 4;
        public static float dpmm;

        public static String[] status_connect = {"Conectado","Desconectado"};

        // Aqui se guardan los datos de los codigos de barra generados

        // Int, de las ubicaciones de los formatos 
        public static List<int>         pos_format_lib;             // Para ubicar formato de libreria
        public static List<string[]>    datalist_barcodes_format;   // Para valores obtenidos del form
        public static List<int>         espesor_barcodes;           // Para resize
        public static int               number_barcode;             // Para search


        // Aqui se guardan los datos de los textos generados
        public static List<string[]>    datalist_text;
        public static List<string>      fixed_text;
        public static List<int>         db_text;
        public static int               number_text;

        // Aqui se guardan los datos de los pictures seleccionados
        public static List<string[]>    datalist_pic;
        public static int               number_pic;

        // Aqui se guardan los datos de BC,TXT, RFID
        // que se van a enviar a la impresora
        // para imprimirlos
        public static List<string[]>    barcode_parametros;
        public static List<string[]>    texto_parametros;
        public static List<string[]>    picture_parametros;
        public static string[]          RFID_parametros;

        // Variables donde se guardan las posiciones
        // de los elementos creados en el panel de creacion
        // segun su tipo (BC,TXT,RFID)
        public List<int> pos_barcode;
        public List<int> pos_text;
        public List<int> pos_pic;

        // Cuando hacemos click en el panel luego de seleccionar el tipo
        // de elemento a crear, usaremos estas banderas
        // para saber que elemento se va a crear al hacer click
        bool is_barcode_icon = false;
        bool is_text_icon = false;
        bool is_rfid_icon = false;

        // Banderas auxiliares para permitir o no activar eventos
        public bool enable_to_print;
        public static int min_width_rising;

        // Temporal Variables for Debugging
        

        public Main_init()
        {
            InitializeComponent();
            // Initial settings
            project_path = Directory.GetParent(Environment.CurrentDirectory).
                                  Parent.Parent.FullName;
            enable_to_print = false;
            LoadSettingJson();

            this.print_panel.MouseClick += new MouseEventHandler(this.print_panel_MouseClick);

            p_x_max = print_panel.Size.Width;
            p_y_max = print_panel.Size.Height;
            dpmm = float.Parse((dpi / 25.4).ToString("0.00"));
            p_x_min = 5;
            p_y_min = 5;
            min_width_rising =(int)get_min_width_barcode(13);

            // b_max    es el ANCHO    en dots de la etiqueta RFID
            b_max = dpmm * label_width;
            // h_max    es la ALTURA   en dots de la etiqueta RFID
            h_max = dpmm * label_height;


            center_panel();
            set_initial_values();

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZMPCL.ClosePort();
            Close();
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            int returncode = -1;

            if (btn_Connect.Text.Equals("Connect"))
            {
                returncode = ZMPCL.OpenPort(255);
                if (returncode != 0)
                {
                    MessageBox.Show("打印机端口打开失败，代码：" + returncode.ToString(), "端口打开失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int a = ZMPCL.ZM_PcxGraphicsDel("*");   // Clear all pic variables  
                btn_Connect.Text = "Disconnect";
                tb_status.BackColor = Color.FromArgb(0,125,0);
                tb_status.Text = status_connect[0];
                enable_to_print = true;
            }
            else {
                ZMPCL.ClosePort();
                btn_Connect.Text = "Connect";
                tb_status.BackColor = Color.FromArgb(125, 0, 0);
                tb_status.Text = status_connect[1];
                enable_to_print = false;
            }

        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            ZMPCL.ClosePort();
            Close();
        }
        private void btn_SaveFile_Click(object sender, EventArgs e)
        {

        }
        private void btn_restart_Click(object sender, EventArgs e)
        {
            restart_label();
        }

        private void btn_barcode_Click(object sender, EventArgs e)
        {
            is_barcode_icon = true;
            is_rfid_icon = false;
            is_text_icon = false;
            this.Cursor = Cursors.SizeAll;
            
        }
        private void btn_text_Click(object sender, EventArgs e)
        {
            is_barcode_icon = false;
            is_text_icon = true;
            is_rfid_icon = false;
            this.Cursor = Cursors.SizeAll;
        }
        private void btn_PrintFile_Click(object sender, EventArgs e)
        {
            check_pos_data_print();
            
            if ((print_panel.Controls.Count < 1))
            { MessageBox.Show("No hay datos para imprimir"); return; }
            if (!enable_to_print)
            { MessageBox.Show("Impresora no conectada"); return; }
            
            Preview_print_form form_print = new Preview_print_form(this);
            form_print.Show();
        }
        private void print_panel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            Point mouseClickLocation = new Point(e.X, e.Y);
            if (is_barcode_icon)
            {
                this.Cursor = Cursors.Default;
                is_barcode_icon = false;
                Form new_barcode_form = new setting_codebar_form(number_barcode);

                if (!(new_barcode_form.ShowDialog(this) == DialogResult.OK))
                    return;

                espesor_barcodes.Add(1);

                string[] data_barcode = datalist_barcodes_format[number_barcode];
                int value_length_UPC = data_barcode[2].Length; ;

                // Creamos un custom PictureBox donde alojar la imagen
                // de codigo de barras creada
                picturebox_barcode my_pic =
                    new picturebox_barcode(number_barcode,0);  // input: Index del barcode
               
                my_pic.Location = mouseClickLocation;
                print_panel.Controls.Add(my_pic);       // agreagamos el PictureBox al panel
                my_pic.BringToFront();

                number_barcode++;
            }

            else if (is_text_icon) 
            {
                this.Cursor = Cursors.Default;
                is_text_icon = false;
                Form new_text_form = new setting_text_form(number_text);
                if (!(new_text_form.ShowDialog(this) == DialogResult.OK))
                    return;
                
                string[] data_text = datalist_text[number_text-1];
                string text_add;
                int ref_text = 0;
            //  { "index","font","size","bold","italic","underline","ref","value"}
                Class_text my_text_ = new Class_text(number_text);
                my_text_.Location = mouseClickLocation;
                FontStyle font_options = new FontStyle();

                if (bool.Parse(data_text[3])) { font_options = font_options | FontStyle.Bold; }
                if (bool.Parse(data_text[4])) { font_options = font_options | FontStyle.Italic; }
                if (bool.Parse(data_text[5])) { font_options = font_options | FontStyle.Underline; }
                int size_px = Convert.ToInt32(float.Parse(data_text[2]) * p_x_max / label_height);

                my_text_.Font = new Font(data_text[1], size_px,font_options, GraphicsUnit.Pixel);
                if (data_text[6].Equals("0")) 
                    text_add = data_text[7];
                else
                {
                    ref_text = int.Parse(data_text[7]);
                    text_add = class_excel_data.data_excel[0][ref_text];
                }
                my_text_.Text = text_add;
                my_text_.AutoSize = true;
                print_panel.Controls.Add(my_text_);
            }
            else if (is_rfid_icon)
            {
            }
        }
        // Cada vez que se requiera imprimir
        // se analizara los elementos en la pantalla
        // Asi evitaremos tener que estar controlando los elementos
        // al momento de crearlos, modificarlos o eliminarlos
        public void check_pos_data_print() {
            pos_barcode.Clear(); pos_text.Clear();
            barcode_parametros.Clear(); texto_parametros.Clear();

            int j = this.print_panel.Controls.Count;

            int[] init_pos = new int[2];

            for (int i = 0; i < j; i++) {

                // Obtenemos el elemento "i" en el panel
                System.Windows.Forms.Control a = print_panel.Controls[i];
                Size aa = a.Size;
                string tag_name_no_split = a.Tag.ToString();
                string[] tag_name_split = tag_name_no_split.Split('_');
                string tag_name = tag_name_split[0];
                string tag_number = tag_name_split[1];


                init_pos[0] = size_px_to_dots(a.Location.X, 1) + p_x_min;

                init_pos[1] = size_px_to_dots(a.Location.Y, 2) + p_y_min;
                int test_comment = size_px_to_dots(8, 2) + p_x_min;

                // Al crear un barcode, se le asigna el tag "BC"
                // y el name "BC_#"
                if (tag_name.Equals("BC"))
                {
                    int pos_bc_list = int.Parse(tag_number) - 1;
                    // i_x, i_y, e, l_y, format_UPC, UPC_number
                    string[] ind_bc_param = new string[8];
                    ind_bc_param[0] = init_pos[0].ToString();
                    ind_bc_param[1] = init_pos[1].ToString();
                    ind_bc_param[2] = (espesor_barcodes[pos_bc_list]).ToString();
                    ind_bc_param[3] = get_barcode_height(a.Size.Height).ToString();
                    ind_bc_param[4] = pos_format_lib[pos_bc_list].ToString();
                    String[] data_inner_UPC = datalist_barcodes_format[pos_bc_list];
                    ind_bc_param[5] = data_inner_UPC[0];
                    ind_bc_param[6] = data_inner_UPC[1];

                    string UPC_print_value = "0";
                    string type_reference_UPC = data_inner_UPC[1];
                    if (type_reference_UPC.Equals("0"))         // Fixed
                    {
                        UPC_print_value = data_inner_UPC[2];
                    }
                    else if (type_reference_UPC.Equals("1"))    // Base de datos
                    {
                        //  int pos_header = int.Parse(data_inner_UPC[2]);
                        UPC_print_value = data_inner_UPC[2];
                    }
                    ind_bc_param[7] = UPC_print_value;
                    barcode_parametros.Add(ind_bc_param);

                }
                // Al crear un Texto, se le asigna el tag "TXT"
                else if (tag_name.Equals("TXT"))
                {
                    float esc_x = 0.048F;
                    float esc_y = 0.08667F;
                    float off_set_x = 4.0F;
                    float off_set_y = -5.0F;

                    int pos_text_list = int.Parse(tag_number);

                    // String[] list_headers_text =
                    // {"index","format","value","size","bold","italic","underline"};
                    string[] data_inner_text = datalist_text[pos_text_list - 1];

                    //     int i_x, int i_y , int fH, int fW,
                    //     String "Arial", int Weigth
                    //     bool Italic, bool Under, str data 
                    string[] ind_txt_param = new string[9];

                    int balanced = (int)(aa.Width * esc_x + off_set_x);
                    int pos_x_real = init_pos[0] + balanced;

                    balanced = (int)(aa.Height * esc_y + off_set_y);
                    int pos_y_real = init_pos[1] + balanced;

                    /*
                    if (a.Location.X + aa.Width > this.print_panel.Width)
                        pos_x_real = init_pos[0] - balanced;
                    if (a.Location.Y + aa.Height > this.print_panel.Height)
                        pos_y_real = init_pos[1] - balanced;
                    */

                    ind_txt_param[0] = pos_x_real.ToString();   // x
                    ind_txt_param[1] = pos_y_real.ToString();   // y
                    ind_txt_param[2] = data_inner_text[1];      // Font
                    ind_txt_param[3] = data_inner_text[2];      // Size
                    ind_txt_param[4] = data_inner_text[3];      // B
                    ind_txt_param[5] = data_inner_text[4];      // K
                    ind_txt_param[6] = data_inner_text[5];      // S
                    ind_txt_param[7] = data_inner_text[6];      // Indexed
                    ind_txt_param[8] = data_inner_text[7];      // Ref

                    texto_parametros.Add(ind_txt_param);

                }
                // Al crear un barcode, se le asigna el tag "QR"
                else if (tag_name.Equals("PIC"))
                {
                    int pos_text_list = int.Parse(tag_number);

                    // String[] list_headers_text =
                    // {"index","size_x","size_y","path_file"};
                    string[] data_inner_pic = datalist_pic[pos_text_list - 1];

                    // export data
                    // {pos_x,pos_y,size_x,size_y,path_file};
                    string[] ind_txt_param = new string[5];
                    //MessageBox.Show(real_path);

                    ind_txt_param[0] = init_pos[0].ToString();
                    ind_txt_param[1] = init_pos[1].ToString();
                    ind_txt_param[2] = data_inner_pic[1];
                    ind_txt_param[3] = data_inner_pic[2];
                    ind_txt_param[4] = data_inner_pic[3];
                    picture_parametros.Add(ind_txt_param);
                    

                }

            }
        }
        // Barra de navegacion: Config -> Label
        // Configuracion del tamaño de la etiqueta RFID
        // (Los datos se guardan en un archivo .json)
        private void dPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label_setting_form label_form = new label_setting_form();
            DialogResult dr = label_form.ShowDialog(this);

            if (!(dr == DialogResult.OK))
                return;
            List<rfid_label> items = new List<rfid_label>();
            items.Add(new rfid_label()
            {
                height = label_height,
                width = label_width,
                separation = label_spacing
            });
            string json = JsonSerializer.Serialize(items);
            File.WriteAllText(project_path + "\\setting.json", json);

            define_size_print_panel();

        }

        // Leemos los datos almacenados en el archivo .json
        // al activar este programa
        private void LoadSettingJson() {
            using (StreamReader r = new StreamReader(project_path + "\\setting.json")) {
                string json = r.ReadToEnd();
                List<rfid_label> items = JsonSerializer.Deserialize<List<rfid_label>>(json);
                rfid_label a = items[0];
                label_height = a.height;
                label_width = a.width;
                label_spacing = a.separation;

            }
        }

        // Funcion para escalar las posiciones de los elementos
        // visibles en el programa a las posiciones reales
        // de la etiqueta RFID
        private int size_px_to_dots(int longitud_en_px, int dimension_axis)
        {
            double dots = 0.0;
            switch (dimension_axis)
            {
                case 1:
                    dots = longitud_en_px * (b_max - 2 * p_x_min) / (p_x_max);
                    break;
                case 2:
                    dots = longitud_en_px * (h_max - 2 * p_y_min) / (p_y_max);
                    break;
            }
            return (int)dots;
        }

        private uint size_dots_to_px(uint longitud_en_dots,int dimension_axis)
        {   double px=0.0;
            switch (dimension_axis) {
                case 1:
                    px = longitud_en_dots * (p_x_max) / (b_max - 2 * p_x_min);
                    break;
                case 2:
                    px = longitud_en_dots * (p_y_max) / (h_max - 2 * p_y_min);
                    break;
            }
            return (uint)px;
        }

        private uint get_min_width_barcode(int characters_qty) {
            uint width = (uint) (5.5 * characters_qty + 35);
            return size_dots_to_px(width,1);
        }
        private uint get_barcode_height(int value_height_px) {
            double double_dots_height =
                value_height_px * (h_max - 2 * p_y_min) / (1.5* p_y_max);
            return (uint)double_dots_height;
        }

        private void baseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            database_excel_form my_excel_form = new database_excel_form();

            if (my_excel_form.ShowDialog() == DialogResult.OK) 
            {
                //MessageBox.Show("Database Updated");
            }
        }

        private void tb_status_TextChanged(object sender, EventArgs e)
        {

        }

        private void set_initial_values() 
        {
            pos_barcode     = new List<int>();
            pos_text        = new List<int>();
            pos_format_lib  = new List<int>();
            pos_pic         = new List<int>();

            datalist_barcodes_format    = new List<string[]>();
            espesor_barcodes            = new List<int>();
            number_barcode              = 0;

            datalist_text   = new List<string[]>();
            fixed_text      = new List<string>();
            db_text         = new List<int>();
            number_text     = 0;

            datalist_pic    = new List<string[]>();
            number_pic      = 0;

            barcode_parametros  = new List<string[]>();
            texto_parametros    = new List<string[]>();
            picture_parametros  = new List<string[]>();

            RFID_parametros     = new string[0];
        }

        private void restart_label() 
        {
            this.print_panel.Controls.Clear();
            set_initial_values();
        }

        private void center_panel() 
        {
            int x_center = (main_panel.Width - print_panel.Width) / 2;
            int y_center = (main_panel.Height - print_panel.Height) / 2;
            Point center = new Point(x_center,y_center);
            print_panel.Location = center;
        }

        private void Main_init_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            center_panel();
            define_size_print_panel();
        }

        private void define_size_print_panel() 
        {
            int base_e = 0;
            int altura_e = 0;
            double h = (double)label_height;
            double b = (double)label_width;
            double rel = h / b;
            double e_x = b / (double)main_panel.Size.Width;
            double e_y = h / (double)main_panel.Size.Height;
            if (e_x > 0.85)
                e_x = 0.85; 

            if (e_y > 0.85)
                e_y = 0.85;
            
            double e_e = 0;
            if (e_x >= e_y)
            {
                e_x = 0.85;
                base_e = (int) (e_x * main_panel.Size.Width);
                altura_e = (int) (base_e * rel);
            }
            else
            {
                e_y = 0.85;
                altura_e = (int)(e_y * main_panel.Size.Height);
                base_e = (int)(altura_e / rel);
            }

            Size size = new Size(base_e,altura_e);
            print_panel.Size = size;
            p_x_max = print_panel.Size.Width;
            p_y_max = print_panel.Size.Height;

            center_panel();
        }

        private void Btn_NewFile_Click(object sender, EventArgs e)
        {
            ZMPCL.ZM_ClearBuffer();
            string TagData = "99999999999999999999";
            int DataNum = TagData.Length / 2; // Length of written data (number of bytes), TagData is in hexadecimal format
            ZMPCL.ZM_RW_RfidFormat(1, 0, 0, DataNum, 1, TagData);
            ZMPCL.ZM_PrintLabel(1, 1);  
        }

        private void btn_pic_Click(object sender, EventArgs e)
        {
            string mensaje;
            string f_path = open_browser_file();
            if (f_path.Equals(""))
                return;
            number_pic++;
            Image image = Image.FromFile(f_path);
            
            int size_x = image.Width;
            int size_y = image.Height;

            class_picture my_picture_class = new class_picture(number_pic);
            my_picture_class.Image = image;

            Point my_location = new Point();
            my_location.X = 40;
            my_location.Y = 40;
            my_picture_class.Location = my_location;

            Size size = new Size();
            size.Width = size_x;
            size.Height = size_y;
            my_picture_class.Size = size;
            this.print_panel.Controls.Add(my_picture_class);

            mensaje = string.Format("{0},{1},{2},{3}",number_pic,size_x,size_y,f_path);
            datalist_pic.Add(mensaje.Split(','));
            //MessageBox.Show(string.Join(",", datalist_pic[0]));
        }

        private string open_browser_file()
        {
            string filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "D:\\";
                openFileDialog.Filter = "PNG files (*.png)|*.png|"  +
                                        "JPG files (*.jpg)|*.jpg|"  +                               
                                        "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
        }

        private void btn_rfid_Click(object sender, EventArgs e)
        {
            Form form = new setting_rfid_form();
            if (form.ShowDialog() == DialogResult.OK)
            {
                string[] rfid_data = new string[2];
                rfid_data[0] = setting_rfid_form.enable_rfid.ToString();
                rfid_data[1] = setting_rfid_form.ref_UPC_header.ToString();
                RFID_parametros = rfid_data;
            }
        }

        //----------------- FINAL DE LAS FUNCIONES -------------------
    }

    // ---------------------------------------------------
    // Aqui comenzamos a crear clases internas para su uso
    // ---------------------------------------------------


    // Clase modelo para el archivo .json
    public class rfid_label
    {
        public int height { get; set; }
        public int width { get; set; }
        public int separation { get; set; }
    }

    // Libreria de la impresora RFID para poder enviar
    // comandos de impresion
    public class ZMPCL
    {
        [DllImport("ZMPCL.dll")]
        public static extern int OpenPort(int portnum);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_SetPrintSpeed(uint px);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_SetDarkness(uint id);
        [DllImport("ZMPCL.dll")]
        public static extern int ClosePort();
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_PrintLabel(uint number, uint cpnumber);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawTextTrueTypeW
                                            (int x, int y, int FHeight,
                                            int FWidth, string FType,
                                            int Fspin, int FWeight,
                                            bool FItalic, bool FUnline,
                                            bool FStrikeOut,
                                            string id_name,
                                            string data);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawBarcode(uint px,
                                        uint py,
                                        uint pdirec,
                                        string pCode,
                                        uint pbright,
                                        uint pHorizontal,
                                        uint pVertical,
                                        char ptext,
                                        string pstr);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_SetLabelHeight(uint lheight, uint gapH);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_SetLabelWidth(uint lwidth);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_ClearBuffer();
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawLineOr(uint px, uint py, uint pLength, uint pH);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawBar2D_QR(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, string pstr);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawBar2D_Pdf417(uint x, uint y, uint w, uint v, uint s, uint c, uint px, uint py, uint r, uint l, uint t, uint o, string pstr);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_PcxGraphicsDel(string pid);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_PcxGraphicsDownload(string pcxname, string pcxpath);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawPcxGraphics(uint px, uint py, string gname);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_DrawText(uint px, uint py, uint pdirec, uint pFont, uint pHorizontal, uint pVertical, char ptext, string pstr);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_RW_RfidFormat(int RWMode, int Format, int StartBlock, int DataNum, int Area, string gname);
        [DllImport("ZMPCL.dll")]
        public static extern int ZM_ReadRFTagDataUSB(int usbPort, int DataBlock, int RFPower, int Feed, StringBuilder pUsbData, int timeout_ms);

        [DllImport("ZMPCL.dll")]
        public static extern int ZM_HF_RW_RfidData(uint RWFormat, uint FirstBlock, uint BlockNum, string pstr); //180626

        [DllImport("ZMPCL.dll")]
        public static extern int ZM_ErrorReportEx(int wPort, int rPort, uint BaudRate, bool HandShake, int TimeOut);

    }

    //----------------- FINAL DE LAS CLASES -------------------
}


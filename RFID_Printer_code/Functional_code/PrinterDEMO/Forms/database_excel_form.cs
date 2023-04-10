using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using System.Data.Common;

namespace PrinterDEMO
{
    public partial class database_excel_form : Form
    {
        public static string my_path;
        DataSet ds;
        private string[] header;
        private List<string[]> data_excel;
        string[] extension_allow = {"xls","xlsx"};
        bool seleceted_right_file;
        public database_excel_form()
        {
            InitializeComponent();
            this.FormClosing += this_FormClosing;   // Asociamos el close_event a la funcion
                                                    // "this_FormClosing"
            db_cb_qty.DropDownStyle = ComboBoxStyle.DropDownList;
            seleceted_right_file = false;
            if (!(class_excel_data.header == null))
            { 
                header = class_excel_data.header;
                data_excel = class_excel_data.data_excel;
                combobox_setlist();
                db_cb_qty.SelectedIndex = class_excel_data.pos_number_print;
                seleceted_right_file = true;
                show_data_excel();
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            my_path = open_browser_file();
            //my_path = "D:\\final.xlsx";
            read_excel_data(my_path);
            show_data_excel();
            combobox_setlist();
        }

        private void read_excel_data(string path_input)
        {
            seleceted_right_file = evaluate_extension(extension_allow, path_input);
            if (!seleceted_right_file)
            {
                MessageBox.Show("Archivo no válido");
                return;
            }
            FileStream fs = new FileStream(path_input, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            var config = new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            };

            // Interpretamos los datos del archivo Excel con AsDataSet
            ds = reader.AsDataSet(config);

            int[] pos_name = new int[] { };     // Guardamos las posiciones donde
                                                // estan las cabeceras en la fila

            header = new string[] { };          // Guardamos el nombre de las cabeceras

            data_excel = new List<string[]>();  // Guardamos los datos (sin cabeceras) que
                                                // obtengamos del reader.AsDataSet

            int number_header = 0;              // Guardamos el numero de cabeceras no vacias
                                                // que existan

            DataTable dt = ds.Tables[0];
            int len_colum = dt.Columns.Count;   // Obtenemos el numero de datos en la fila inicial

            bool column_no_empty = false;       // Bandera para determinar si hay datos
                                                // en la fila inicial


            
            // Comenzamos a leer datos de la primera fila
            for (int i = 0; i < len_colum; i++) 
            {
                object a = dt.Columns[i];
                if (a.ToString().Contains("Column"))    // SI esta vacia, el nombre por defecto es
                                                        // "Column + i"
                    continue;
                column_no_empty = true; break;
            }

            if (column_no_empty) 
            {
                for (int i = 0; i < len_colum; i++)
                {
                    string sa = dt.Columns[i].ToString();
                    if (sa.Contains("Column"))
                        continue;
                    number_header++;
                    Array.Resize(ref header, number_header);
                    Array.Resize(ref pos_name, number_header);
                    header[number_header - 1] = sa;
                    pos_name[number_header - 1] = i;
                }
            }

            object[] row_obj;
            int number_row_max = dt.Rows.Count;
            int row_size = 0;       // Obtenemos el tamaño de los datos (incluidos los vacios)
            int pos_init = 0;       // Obtenemos donde iniciara la lectura de datos

            // En caso la primera fila del Excel NO contenga datos
            if (!column_no_empty) 
            {
                for (int j = 0; j < number_row_max; j++)
                {
                    row_obj = dt.Rows[j].ItemArray;
                    row_size = row_obj.Length;
                    for (int i = 0; i < row_size; i++)
                    { 
                        string data_row = row_obj[i].ToString();
                        if (data_row.Length < 1)
                            continue;
                        pos_init = j; column_no_empty = true; break;
                        
                    }
                    if (column_no_empty)
                        break;
                }

                row_obj = dt.Rows[pos_init].ItemArray;
                for (int i = 0; i < row_size; i++) 
                {
                    string data_row = row_obj[i].ToString();
                    if (data_row.Length < 1)
                        continue;
                    number_header++;
                    Array.Resize(ref header, number_header);
                    Array.Resize(ref pos_name, number_header);
                    header[number_header - 1] = data_row;
                    pos_name[number_header - 1] = i;
                }
                pos_init++;     // Pasamos al siguiente row, donde iniciara la lectura de datos
            }
            // Ya tenemos el nombre de los headers

            for (int i = pos_init; i < number_row_max; i++)
            {
                string[] array_string_add_list = new string[number_header];
                for (int j = 0; j < number_header; j++)
                {
                    int pos_data = pos_name[j];
                    string string_data_list = dt.Rows[i].ItemArray[pos_data].ToString();
                    array_string_add_list[j] = string_data_list;
                }
                data_excel.Add(array_string_add_list);
            }
        }
        private string open_browser_file() 
        {
            string filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "D:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = openFileDialog.FileName;
            }

            return filePath;
        }
        private bool evaluate_extension(string[] extension,string path_input) 
        {
            int ext_num = extension.Length;
            string[] file_split = path_input.Split('.');
            string file_ext = file_split[1];                        // Extension del archivo
            string file_name = file_split[0].Split('\\').Last();    // Nombre del archivo
            bool result = false;
            for (int m = 0; m < ext_num; m++) 
            {
                if (extension[m].Equals(file_ext))
                {   result=true; 
                    //MessageBox.Show(file_name); 
                    break; 
                }
            }
            return result;
        }
        private void show_data_excel() 
        {
            if (header == null)
                return;

            
            this.gv_excel_data.Rows.Clear();
            this.gv_excel_data.Columns.Clear();

            for (int m = 0; m < header.Length; m++) 
            {
                this.gv_excel_data.Columns.Add(m.ToString(), header[m]);
            }

            foreach (DataGridViewColumn column in gv_excel_data.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int m = 0; m < data_excel.Count; m++) 
            {
                string[] temp_data_excel = data_excel[m];
                this.gv_excel_data.Rows.Add(temp_data_excel);
            }
        }
        private void combobox_setlist() 
        {
            db_cb_qty.Items.Clear();
            for (int m = 0; m < header.Length; m++)
            { 
                db_cb_qty.Items.Add(header[m]);
            }
            db_cb_qty.SelectedItem = 0;

        }

        private void this_FormClosing(Object sender, FormClosingEventArgs e)
        {
        }

        private void upload_rows_data() 
        {
            int re_rows_number = gv_excel_data.Rows.Count;
            data_excel.Clear();
            string[] re_temp_value;
            for (int m = 0; m < re_rows_number; m++) 
            {
                re_temp_value = new string[this.header.Length];
                var k = gv_excel_data.Rows[m];
                for (int i = 0; i < header.Length; i++)
                {
                    re_temp_value[i] = k.Cells[i].Value.ToString();
                }
                data_excel.Add(re_temp_value);
            }


        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (seleceted_right_file) {
                upload_rows_data();
                class_excel_data.header = this.header;
                class_excel_data.data_excel = this.data_excel;
                class_excel_data.this_exist = true;
                MessageBox.Show("Se ha actualizado la base de datos");
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }

        private void db_cb_qty_SelectedIndexChanged(object sender, EventArgs e)
        {
           class_excel_data.pos_number_print = db_cb_qty.SelectedIndex;
            
        }
    }
}

using PrinterDEMO.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterDEMO
{
    public class picturebox_barcode : PictureBox
    {
        // Valor referencial para recuperar valores
        int index;

        // Si el valor es referencial, solicitamos
        // la posicion 
        int pos_rows;

        // Imagen del picturebox
        class_img_barcode this_image_ref;

        // Parametros para rescalamiento
        private int margin_resize;
        public static bool draw_border;
        private int min_width_change;
        private int espesor;

        // Banderas para forma del puntero
        private bool is_right_position;
        private bool is_left_position;
        private bool is_top_position;
        private bool is_bottom_position;

        // Banderas para accion de mover o resize
        private bool allow_resize;
        private bool allow_move;

        // Banderas para saber el estado del mouse
        private bool isMousepressed;


        // Valores de la posicion previa del mouse
        // y tamaño previo del panel
        private Size startsize;
        private Point current_mouse_position;
        string UPC_number;
        private Image img;
        public picturebox_barcode(int index_temp,int pos_rows)
        {
            margin_resize = 15;
            draw_border = false;
            min_width_change = Main_init.min_width_rising;

            // Banderas para forma del puntero
            is_right_position   =   false;
            is_left_position    =   false;
            is_top_position     =   false;
            is_bottom_position  =   false;

            // Banderas para accion de mover o resize
            allow_resize    =   false ;
            allow_move      =   false ;

            // Banderas para saber el estado del mouse
            isMousepressed  =   false;
            allow_move      =   false;
            current_mouse_position = new Point();

            // Datos para saber a que codigo de barras se esta editando
            this.index      = index_temp;
            this.SizeMode   = PictureBoxSizeMode.StretchImage;
            this.Tag        = "BC_" + (Main_init.number_barcode+1).ToString();

            this.Cursor     = Cursors.SizeAll;
            this.BackColor  = Color.Transparent;

            UPC_number      = get_UPC_value(index);
            espesor         = Main_init.espesor_barcodes[index];
            
            int format_int  = Main_init.pos_format_lib[index];
            this_image_ref  = new class_img_barcode(format_int,UPC_number);
            actualizar_img();
        }

        /*
        * FUNCIONES PARA REESCALAR IMAGEN CON MOUSE
        */

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!isMousepressed)
            {
                // Acutalizamos: Forma del puntero
                Point position_mouse = e.Location;
                update_flags_pointer(position_mouse);
                update_pointer_form();
            }
            else if (isMousepressed && allow_move)
            {
                // Move programming
                this.Left += e.X - current_mouse_position.X;
                this.Top += e.Y - current_mouse_position.Y;
            }
            else if (isMousepressed && allow_resize) 
            {
                int change_x = e.X - current_mouse_position.X;
                int min_width_change = Main_init.min_width_rising;

                int change_y = e.Y - current_mouse_position.Y;
                int previous_width = startsize.Width;
                int previous_height = startsize.Height;
                // Resize programming
                if (is_right_position)
                {
                    if (change_x > min_width_change || change_x < -min_width_change)
                    {
                        int factor = change_x / min_width_change;
                        Main_init.espesor_barcodes[index] += factor;
                        current_mouse_position.X = e.X;
                        this.Width = this.startsize.Width + factor *            min_width_change;
                        startsize.Width = this.Width;
                    }

                    if (is_top_position)
                    {
                        this.Height -= change_y;
                        this.Top += change_y;
                    }
                    else if (is_bottom_position)
                    {
                        this.Height = change_y + previous_height;
                    }
                }

                else if (is_left_position)
                {
                    if (change_x > min_width_change || change_x < -min_width_change)
                    {
                        int factor = change_x / min_width_change;
                        change_x = factor * min_width_change;
                        Main_init.espesor_barcodes[index] -= factor;

                        this.Width = this.startsize.Width - change_x;
                        this.Left += change_x;
                        startsize.Width = this.Width;
                        current_mouse_position.X = e.X - change_x;
                    }

                    if (is_top_position)
                    {
                        this.Height -= change_y;
                        this.Top += change_y;
                    }
                    else if (is_bottom_position)
                    {
                        this.Height = change_y + previous_height;
                    }

                }

                else if (is_top_position)
                {
                    this.Height -= change_y;
                    this.Top += change_y;
                }
                else {
                    this.Height = change_y + previous_height;
                }

            }

        }

        // En esta funcion, el mouse esta sobre el panel
        // y SE ESTA presionando algun boton
        protected override void OnMouseDown(MouseEventArgs e)
        {
            isMousepressed = true;
            current_mouse_position = e.Location;
            this.Capture = true;
            bool is_center_position = is_top_position || is_bottom_position || is_right_position || is_left_position;
            is_center_position = !is_center_position;
            if (is_center_position)
            {
                allow_move = true;
                allow_resize = false;
            }
            else 
            {
                allow_move = false;
                allow_resize = true;
            }
            
            startsize = this.Size;

        }

        // En esta funcion, el mouse esta sobre el panel
        // y NO SE ESTA presionando algun boton
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Deshabilitamos: Mover image, resize
            isMousepressed = false;
            allow_move = false;
            allow_resize = false;
            this.Capture = false;
        }


        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Form new_barcode_form = new setting_codebar_form(index);
            if (!(new_barcode_form.ShowDialog(this) == DialogResult.OK))
                return;
            UPC_number = get_UPC_value(index);
            int format_int = Main_init.pos_format_lib[index];
            this_image_ref = new class_img_barcode(format_int, UPC_number);
            actualizar_img();
            
            
        }

        private string get_UPC_value(int UPC_index) 
        {
            string[] data_temp = Main_init.datalist_barcodes_format[UPC_index];
            int max_temp = data_temp.Length;
            string format = data_temp[max_temp - 2];
            if (format.Equals("0"))
            {
                return data_temp[max_temp - 1];
            }
            else
            {
                int pos_value = int.Parse(data_temp[max_temp - 1]);
                return class_excel_data.data_excel[pos_rows][pos_value];
            }
        }

        private void update_pointer_form() {
            if (is_left_position)
            {
                if (is_top_position)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (is_bottom_position)
                {
                    this.Cursor = Cursors.SizeNESW;
                }
                else
                {
                    this.Cursor = Cursors.SizeWE;
                }
            }
            else if (is_right_position)
            {
                if (is_top_position)
                {
                    this.Cursor = Cursors.SizeNESW;
                }
                else if (is_bottom_position)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    this.Cursor = Cursors.SizeWE;
                }
            }
            else if (is_top_position || is_bottom_position)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else
            {
                this.Cursor = Cursors.SizeAll;
            }
        }
        private void update_flags_pointer(Point pointer) 
        {
            int x_pos = pointer.X; int y_pos = pointer.Y;
            is_right_position = x_pos > this.Width-margin_resize;
            is_left_position = x_pos < margin_resize;
            is_top_position = y_pos < margin_resize;
            is_bottom_position = y_pos > this.Height-margin_resize;

        }

        private void actualizar_img()
        {
            img = this_image_ref.get_image(espesor, false);
            this.Image = img;
            this.Size = img.Size;
        }
    }
}


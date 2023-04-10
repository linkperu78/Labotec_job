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
    public class Class_text : Label
    {
        int index;

        // Parametros para rescalamiento
        private int margin_resize;
        public static bool draw_border;
        private int min_width_change;

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

        public Class_text(int index_a)
        {
            margin_resize = 0;
            draw_border = false;

            // Banderas para forma del puntero
            is_right_position = false;
            is_left_position = false;
            is_top_position = false;
            is_bottom_position = false;

            // Banderas para accion de mover o resize
            allow_resize = false;
            allow_move = false;

            // Banderas para saber el estado del mouse
            isMousepressed = false;
            allow_move = false;
            current_mouse_position = new Point();



            // Datos para saber a que codigo de barras se esta editando
            this.index = index_a;
            this.Tag = "TXT" + "_" + (Main_init.number_text).ToString();

            this.Cursor = Cursors.SizeAll;
            //this.BackColor = System.Drawing.Color.Transparent;
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
            Form new_form = new setting_text_form(index - 1);
            if (!(new_form.ShowDialog(this) == DialogResult.OK))
                return;
            //  { "index","font","size","bold","italic","underline","ref","value"}
            string[] data_text = Main_init.datalist_text[index - 1];
            FontStyle font_options = new FontStyle();
            string text_add;
            int ref_text = 0;

            if (bool.Parse(data_text[3])) { font_options = font_options | FontStyle.Bold; }
            if (bool.Parse(data_text[4])) { font_options = font_options | FontStyle.Italic; }
            if (bool.Parse(data_text[5])) { font_options = font_options | FontStyle.Underline; }


            int size_px = Convert.ToInt32(float.Parse(data_text[2]) * Main_init.p_x_max / 
                                                                       Main_init.label_height);

            // size_px => Tamaño en pixeles del texto
            this.Font = new Font(data_text[1], size_px, font_options, GraphicsUnit.Pixel);
            if (data_text[6].Equals("0"))
                text_add = data_text[7];
            else
            {
                ref_text = int.Parse(data_text[7]);
                text_add = class_excel_data.data_excel[0][ref_text];
            }
            this.Text = text_add;

        }
        private void update_pointer_form()
        {
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
            is_right_position = x_pos > this.Width - margin_resize;
            is_left_position = x_pos < margin_resize;
            is_top_position = y_pos < margin_resize;
            is_bottom_position = y_pos > this.Height - margin_resize;

        }
    }
}

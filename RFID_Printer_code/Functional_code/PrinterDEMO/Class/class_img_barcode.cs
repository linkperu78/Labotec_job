using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterDEMO.Class
{
    public class class_img_barcode
    {
        BarcodeLib.TYPE bc_type;    // Variable for BC_type
        private int max_length;     // max UPC length
        private int index_format;   // BC_type order
        string UPC_number;          // UPC value


        public class_img_barcode(int bc_type, string upc) 
        {
            this.index_format = bc_type;
            this.UPC_number = upc;
        }

        public Image get_image(int espesor, bool text_enable) 
        {
            bc_type_select(index_format);

            Font font               = new System.Drawing.Font("arial", 10F);
            BarcodeLib.Barcode b    = new BarcodeLib.Barcode();

            b.BarWidth      = espesor;
            b.IncludeLabel  = false;
            b.Alignment     = BarcodeLib.AlignmentPositions.LEFT;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.LabelFont     = font;
            int lon_str = UPC_number.Length;

            if(lon_str < max_length)
                UPC_number = UPC_number.PadLeft(max_length, '0');
            else
                UPC_number = UPC_number.Substring(lon_str-max_length,lon_str-1);

            return b.Encode(bc_type, UPC_number);
        }

        private void bc_type_select(int aindex)
        {
            switch (aindex)
            {
                case 0: 
                    bc_type = BarcodeLib.TYPE.CODE128A; max_length = 12;
                    break;
                case 1: 
                    bc_type = BarcodeLib.TYPE.CODE128B; max_length = 12;
                    break;
                case 2: 
                    bc_type = BarcodeLib.TYPE.CODE128C; max_length = 12;
                    break;
                case 3: 
                    bc_type = BarcodeLib.TYPE.EAN13; max_length = 13;
                    break;
                case 4: 
                    bc_type = BarcodeLib.TYPE.EAN8; max_length = 8; 
                    break;
                case 5: 
                    bc_type = BarcodeLib.TYPE.ISBN; max_length = 13; 
                    break;
                case 6: 
                    bc_type = BarcodeLib.TYPE.UPCA; max_length = 12; 
                    break;
                case 7: 
                    bc_type = BarcodeLib.TYPE.UPCE; max_length = 12; 
                    break;
            }
        }
    }
}

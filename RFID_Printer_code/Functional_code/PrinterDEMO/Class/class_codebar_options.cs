using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterDEMO
{
    public class class_codebar_options
    {
        private string[] lib_codes;
        private string[] printer_codes;

        public class_codebar_options() 
        {
            lib_codes = new string[] {"a","b"};

            printer_codes = new string[] { "1A","1B","1C","E30","E80",
                                           "2U","UA0","UE0"};


        }
        public string get_printer_code(int pos) 
        {
            return printer_codes[pos];
        }
        public string get_lib_code(int pos)
        {
            return lib_codes[pos];
        }

    }
}

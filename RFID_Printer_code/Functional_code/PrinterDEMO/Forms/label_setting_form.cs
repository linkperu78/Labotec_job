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
    public partial class label_setting_form : Form
    {
        public label_setting_form()
        {
            InitializeComponent();
            this.tb_height.Text = Main_init.label_height.ToString();
            this.tb_width.Text = Main_init.label_width.ToString();
            this.tb_spacing.Text = Main_init.label_spacing.ToString();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void tb_spacing_TextChanged(object sender, EventArgs e)
        {
            Main_init.label_spacing = int.Parse(tb_spacing.Text);
        }

        private void tb_height_TextChanged(object sender, EventArgs e)
        {
            Main_init.label_height = int.Parse(tb_height.Text);

        }

        private void tb_width_TextChanged(object sender, EventArgs e)
        {
            Main_init.label_width = int.Parse(tb_width.Text);

        }
    }
}

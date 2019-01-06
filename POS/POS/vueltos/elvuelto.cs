using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.vueltos
{
    public partial class elvuelto : Form
    {
        Form1 f1;
        public elvuelto(Form1 f)
        {
            InitializeComponent(); f1 = f;
        }

        private void elvuelto_Load(object sender, EventArgs e)
        {
            label2.Text = "₡" + f1.elvuelto;
        }

        private void elvuelto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Visible = false;
            }
        }
    }
}

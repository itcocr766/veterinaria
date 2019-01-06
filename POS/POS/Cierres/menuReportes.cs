using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.Reportes;
namespace POS.Cierres
{
    public partial class menuReportes : Form
    {
        public menuReportes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clientesrepor cre = new Clientesrepor();
            cre.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ventasrepor vre = new Ventasrepor();
            vre.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Productosrepor pre = new Productosrepor();
            pre.Show(this);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Vendedoresrepor vrep = new Vendedoresrepor();
            vrep.Show(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            detallesr dr = new detallesr();
            dr.Show(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.Reportes
{
    public partial class reporteventas : Form
    {
        public reporteventas()
        {
            InitializeComponent();
        }

        private void reporteventas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSet1.DataTable' Puede moverla o quitarla según sea necesario.
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DataTableTableAdapter.Fill(this.DataSet1.DataTable,dateTimePicker1.Value,dateTimePicker2.Value);

            this.reportViewer1.RefreshReport();
        }
    }
}

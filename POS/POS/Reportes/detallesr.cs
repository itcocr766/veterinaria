using MySql.Data.MySqlClient;
using POS.Modelo;
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
    public partial class detallesr : Form
    {
        public detallesr()
        {
            InitializeComponent();
        }

        private void detallesr_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reportViewer1.Clear();
            using (var mysql = new Mysql())
            {
                mysql.conexion();
                mysql.cadenasql = "SELECT items.Codigo,items.CodigoA,detalles.Cantidad,items.Descripcion,factura.Numero,detalles.Precio FROM factura,detalles,items WHERE (factura.Numero=detalles.NumeroFactura) AND (detalles.Item=items.Codigo OR detalles.Item=items.CodigoA) AND (factura.Fecha BETWEEN date('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND date('" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')) ORDER BY factura.Numero";
                MySqlDataAdapter adapt = new MySqlDataAdapter(mysql.cadenasql, mysql.con);
                adapt.Fill(dataSet1.detalles);
                mysql.Dispose();
            }
            this.reportViewer1.RefreshReport();
        }
    }
}

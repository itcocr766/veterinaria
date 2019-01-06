using MySql.Data.MySqlClient;
using POS.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.Cierres
{
    public partial class historico : Form
    {
        string formato;
        StreamWriter facturawr;
        StreamReader streamToPrint;
        Font printFont;
        public historico()
        {
            InitializeComponent();
        }

        private void historico_Load(object sender, EventArgs e)
        {
            cargarcierres();
        }

        public void cargarcierres()
        {
            try
            {

                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();



                    mysql.cadenasql = "SELECT Numero,Fecha,VentasContado,Fondo,VentasTarjeta FROM cierre";


                    MySqlDataAdapter mdaDatos = new MySqlDataAdapter(mysql.cadenasql, mysql.con);
                    mdaDatos.Fill(dtDatos);
                    dataGridView1.DataSource = dtDatos;

                    mysql.Dispose();

                }






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }


        public void formatodeactura()
        {

            formato = "";



            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select * from cierre where Numero='" + dataGridView1.CurrentRow.Cells[0].Value + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            formato = 
                            "CAJA                           ADMINISTRADOR\n" +
                            "NUMERO CIERRE            " + lee["Numero"] + "\n" +
                            "FECHA EMISIÓN         " + Convert.ToDateTime(lee["Fecha"].ToString()).ToShortDateString() + "\n" +
                            "FACTURA INICIAL F      " + lee["FacturaInicialf"] + "\n" +
                            "FACTURA FINAL F        " + lee["FacturaFinalf"] + "\n" +
                            "FACTURA INICIAL P      " + lee["FacturaInicialp"] + "\n" +
                            "FACTURA FINAL P        " + lee["FacturaFinalp"] + "\n" +
                            "ABONO INICIAL     ₡" + lee["abonoinicial"] + "\n" +
                            "ABONO FINAL        ₡" + lee["abonofinal"] + "\n" +
                            "VENTAS                       MONTO\n" +
                            "----------------------------------\n" +
                            "VENTAS CONTADO      ₡" + string.Format("{0:N2}", double.Parse(lee["VentasContado"].ToString())) + "\n" +
                            "ABONOS                       ₡" + string.Format("", double.Parse(lee["Abonos"].ToString())) + "\n" +
                            "VENTAS TARJETA          ₡" + string.Format("{0:N2}", double.Parse(lee["VentasTarjeta"].ToString())) + "\n" +
                            "ENTRADA CAJA             ₡" + string.Format("{0:N2}", double.Parse(lee["Fondo"].ToString())) + "\n" +
                            "SALIDA CAJA                 ₡0\n" +
                            "TOTAL CAJA                  ₡0\n" +
                            "FONDO CAJA                ₡0\n" +
                            "POR ENTREGAR            ₡" + string.Format("{0:N2}", (double.Parse(lee["VentasContado"].ToString()) + double.Parse(lee["VentasTarjeta"].ToString()) + double.Parse(lee["Fondo"].ToString()))) + "\n\n" +
                            "NOTAS\n\n" +
                            "________________________________________\n\n" +
                            "________________________________________\n\n" +
                            "FIRMA CAJERO           FIRMA ADMIN\n\n" +
                            "________________       _________________\n\n" +
                            "Impreso el " + DateTime.Now.ToString();

                        }



                    }
                    mysql.Dispose();
                }




                facturawr = new StreamWriter("Cierre.txt");
                facturawr.WriteLine(formato);
                facturawr.Flush();
                facturawr.Close();









            }
            catch (Exception err_0016)
            {
                MessageBox.Show(err_0016.ToString(), "329", MessageBoxButtons.OK, MessageBoxIcon.Hand);



            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            formatodeactura();

            imprimir();
            this.Visible = false;
        }


        public void imprimir()
        {

            try
            {



                streamToPrint = new StreamReader
                    ("Cierre.txt");


                try
                {
                    printFont = new Font("Segoe UI", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (this.printDocument1_PrintPage);
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception err_004)
            {
                Mensaje.Error(err_004, "128");

            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;


            float xPos = 0;

            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;





            string line = null;



            try
            {

                linesPerPage = e.MarginBounds.Height /
                   printFont.GetHeight(e.Graphics);


                while (count < linesPerPage &&
                   ((line = streamToPrint.ReadLine()) != null))
                {
                    yPos = (topMargin - 100) + (count *
                       printFont.GetHeight(e.Graphics));




                    e.Graphics.DrawString(line, printFont, Brushes.Black,
                    //leftMargin - 5, yPos, new StringFormat());
                    leftMargin - 80, yPos, new StringFormat());
                    count++;
                }


                if (line != null)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;

            }
            catch (Exception err_005)
            {
                Mensaje.Error(err_005, "186");

            }


        }

    }
}

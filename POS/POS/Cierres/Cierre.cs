using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.Modelo;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Drawing.Printing;
using System.Globalization;

namespace POS.Cierres
{
    public partial class Cierre : Form
    {

        string numero;
   
  
        bool impri;
         public string inicialff = "";
        public  string finalff = "";
        public string inicialpp = "";
        public string finalpp = "";
        Int64 finalf = 0;
        Int64 inicialf =0;

        double efectivof;
        double tarjetaf;
        double sumaf,sumatoria;
    
        public string abonoiniciall = "";
        public  string abonofinall = "";
         string formato;
        StreamWriter facturawr;
        StreamReader streamToPrint;
        Font printFont;
        public Cierre()
        {
            InitializeComponent();
        }

        private void Cierre_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            comboBox2.DataSource = cargarCajeros();
            comboBox2.DisplayMember = "Codigo";
            comboBox2.ValueMember = "Codigo";



        }

        public DataTable cargarCajeros()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql = new Mysql())
                {

                    mysql.conexion();

                    string query = "SELECT Codigo FROM registro";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();
                }

            }
            catch (Exception ekk)
            {

                Mensaje.Error(ekk, "1482");


            }

            return dt;
        }


        public void cargarFactura()
        {
            
            try
            {
               
                using (var mysql= new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                  
                        mysql.cadenasql = "SELECT * FROM `factura` where Fecha between '"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"' and '"+dateTimePicker2.Value.ToString("yyyy-MM-dd") +"'";
                  

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


        public void cargarFacturaporcajero()
        {

            try
            {

                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();

                    mysql.cadenasql = "SELECT * FROM `factura` where Fecha between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and CodigoCajero='" + comboBox2.Text + "'";


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



        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                  

                }
                else
                {

                    MessageBox.Show("No ha ingresado valores suficientes para realizar este proceso", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
               

            }
            catch (Exception er)
            {
                MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //sumatoria = 0;
           
            //try
            //{
            //    if (!string.IsNullOrEmpty(textBox2.Text)  &&
            //        !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrEmpty(textBox14.Text))
            //    {
            //        sumatoria += (double.Parse(textBox2.Text) +double.Parse(textBox5.Text));
            //        textBox7.Text = string.Format("{0:N2}", (double.Parse(textBox13.Text) + double.Parse(textBox14.Text)));
            //        textBox6.Text = string.Format("{0:N2}", (sumatoria));
            //    }
            //    else
            //    {

            //       DialogResult result= MessageBox.Show("No ha ingresado un fondo de caja." +
            //           "Desea que el programa calcule el cierre con un monto de cien mil colones como fondo?\n" +
            //           "Presione 'No' para salir y digitar un fondo de caja.","Faltan datos",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            //        if (result == DialogResult.Yes)
            //        {
            //            if (!string.IsNullOrEmpty(textBox2.Text)   &&
            //                 !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrEmpty(textBox14.Text))
            //            {
                            
                         
            //                sumatoria += (double.Parse(textBox2.Text) );
            //                textBox7.Text = string.Format("{0:N2}", (double.Parse(textBox13.Text) + double.Parse(textBox14.Text)));
            //                textBox5.Text = "100000.00";
            //                textBox6.Text = string.Format("{0:N2}",sumatoria);
            //            }
            //            else if (string.IsNullOrEmpty(textBox2.Text) 
            //                || string.IsNullOrEmpty(textBox13.Text) || string.IsNullOrEmpty(textBox14.Text))
            //            {
            //                MessageBox.Show("No ha ingresado valores suficientes para realizar el cierre", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //            }
                     

            //        }
                   
            //    }
                

            //}
            //catch (Exception errf)
            //{

            //    MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

    
      

        private void button5_Click(object sender, EventArgs e)
        {
            guardarcierre();
            if (impri)
            {
                formatodeactura();
                imprimir();
                button6.Enabled = true;
                limpieza();
            }
           
        }

        private void guardarcierre()
        {
            impri = false;
            numero = "";
            try
            {
                if (!string.IsNullOrEmpty(textBox2.Text)&&
                    !string.IsNullOrEmpty(textBox13.Text)&& !string.IsNullOrEmpty(textBox14.Text)&&
                    !string.IsNullOrEmpty(textBox7.Text)&& !string.IsNullOrEmpty(textBox5.Text)&&
                    !string.IsNullOrEmpty(textBox6.Text))
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        inicialf = Int64.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString());
                        finalf = Int64.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value.ToString());
                    }
                    else
                    {
                        inicialf = 0;
                        finalf = 0;
                    }

                  
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();

                        mysql.cadenasql = "INSERT INTO cierre(Fecha,FacturaInicialf,FacturaFinalf,VentasContado,Fondo,VentasTarjeta,Total) VALUES (" +
                            "'" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "'," +
                            "'" + inicialf + "'," +
                            "'" + finalf + "'," +
                          
                            "'" + double.Parse(textBox13.Text).ToString("0.00",CultureInfo.InvariantCulture) + "'," +

                            "'"+ double.Parse(textBox5.Text).ToString("0.00", CultureInfo.InvariantCulture) + "'" +
                            ",'"+ double.Parse(textBox14.Text).ToString("0.00", CultureInfo.InvariantCulture) + "'," +
                            "'"+ (double.Parse(textBox13.Text)+ double.Parse(textBox14.Text)).ToString("0.00", CultureInfo.InvariantCulture) + "')";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();

                        mysql.Dispose();
                        impri = true;
                    }


                    using (var mysql2 = new Mysql())
                    {
                        mysql2.conexion();
                        mysql2.cadenasql = "select Max(Numero) from cierre";
                        mysql2.comando = new MySqlCommand(mysql2.cadenasql, mysql2.con);
                        mysql2.comando.ExecuteNonQuery();
                        using (var lee2 = mysql2.comando.ExecuteReader())
                        {
                            while (lee2.Read())
                            {
                                numero = lee2["Max(Numero)"].ToString();
                            }
                        }
                        mysql2.Dispose();
                    }



                }
                else
                {
                    MessageBox.Show("Faltan datos","Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                }
                
            }
            catch (Exception evo)
            {
                MessageBox.Show(evo.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        #region metodos impresion
        public void formatodeactura()
        {

            formato = "";

            try
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    inicialff = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    finalff = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value.ToString();
                }
                else
                {
                    inicialff = "0";
                    finalff = "0";
                }



                formato = 
                              "CAJA                           ADMINISTRADOR\n" +
                              "NUMERO CIERRE            " + numero + "\n" +
                              "FECHA EMISIÓN         " + DateTime.Now.ToShortDateString() + "\n" +
                              "FACTURA INICIAL F      " + inicialff + "\n" +
                              "FACTURA FINAL F        " + finalff + "\n" +
                         
                              "VENTAS                       MONTO\n" +
                              "----------------------------------\n" +
                              "VENTAS CONTADO      ₡" + string.Format("{0:N2}", double.Parse(textBox13.Text)) + "\n" +
                            
                              "VENTAS TARJETA          ₡" + string.Format("{0:N2}", double.Parse(textBox14.Text)) + "\n" +
                              "ENTRADA CAJA             ₡" + string.Format("{0:N2}", double.Parse(textBox5.Text)) + "\n" +
                              "SALIDA CAJA                 ₡0\n" +
                              "TOTAL CAJA                  ₡0\n" +
                              "FONDO CAJA                ₡0\n" +
                              "POR ENTREGAR            ₡" + string.Format("{0:N2}", double.Parse(textBox6.Text)) + "\n\n" +
                              "NOTAS\n\n" +
                              "________________________________________\n\n" +
                              "________________________________________\n\n" +
                              "FIRMA CAJERO           FIRMA ADMIN\n\n" +
                              "________________       _________________\n\n" +
                              "Impreso el " + DateTime.Now.ToString();


                facturawr = new StreamWriter("Cierre.txt");
                facturawr.WriteLine(formato);
                facturawr.Flush();
                facturawr.Close();

                //}







            }
            catch (Exception err_0016)
            {
                Mensaje.Error(err_0016, "329");



            }

        }

        public void limpieza()
        {
           
            textBox2.Text = "";
          
            textBox13.Text = "";
            textBox14.Text = "";
            textBox7.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            dataGridView1.DataSource = null;
         
        }
        #region imprimir
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
        #endregion
        #endregion

        private void button6_Click(object sender, EventArgs e)
        {
            verificarcierre();

           
            cargarFactura();
        
            button6.Enabled = false;
 
        }

        public  void verificarcierre()
        {
           
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    
                    mysql.cadenasql = "select count(*),max(FacturaFinalf) from cierre";
                    mysql.comando = new MySqlCommand(mysql.cadenasql,mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee=mysql.comando.ExecuteReader())
                    {
                       
                            while (lee.Read())
                            {
                            if (Int64.Parse(lee["count(*)"].ToString())>0)
                            {

                                finalf = Int64.Parse(lee["max(FacturaFinalf)"].ToString());
                            }
                            else
                            {

                                finalf = 0;
                            }
                                
                            }
                           
                    

                        mysql.Dispose();
                    }
                   
                    mysql.Dispose();

                }

              



            }
            catch (Exception huy)
            {
                MessageBox.Show(huy.ToString());
            }
        }

     

        private void cambiarLaClaveDeAccesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cambiarlaclave ccc = new cambiarlaclave();
            ccc.Show(this);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            verificarcierre();


            cargarFactura();
        
   
            sumarefes();
        
            sumatoriatotal();
            button5.Enabled = true;
            button6.Enabled = false;
        }

        public void realizartotales()
        {

            try
            {
                if (!string.IsNullOrEmpty(textBox2.Text) 
                    )
                {
                   

                }
                else
                {

                    MessageBox.Show("No ha ingresado valores suficientes para realizar este proceso", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


            }
            catch (Exception er)
            {
                MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      


        public void sumatoriatotal()
        {
            sumatoria = 0;
          
            try
            {
                if (!string.IsNullOrEmpty(textBox2.Text)&&!string.IsNullOrEmpty(textBox5.Text)  
                    //!string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrEmpty(textBox14.Text))
                   )
                {
                    sumatoria += efectivof + tarjetaf;
                    //textBox7.Text = string.Format("{0:N2}", (double.Parse(textBox13.Text) + double.Parse(textBox14.Text)));
                    textBox6.Text =  (sumatoria).ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(textBox2.Text)&& !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrEmpty(textBox14.Text))
                    {
                        DialogResult result = MessageBox.Show("No ha ingresado un fondo de caja." +
                      "Desea ingresar uno?\n", "Faltan datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                               
                                
                                textBox7.Text = string.Format("{0:N2}", (efectivof+tarjetaf)); 
                                textBox5.Text = "0";
                                textBox6.Text = string.Format("{0:N2}", (efectivof + tarjetaf)); ;
                                button6.Enabled = false;
                                //button5.Enabled = true;
                            
                  
                        }
                        else
                        {
                            textBox7.Text = string.Format("{0:N2}", (efectivof + tarjetaf));
                            textBox5.Text = "0";
                            textBox6.Text = string.Format("{0:N2}", (efectivof + tarjetaf));
                            button6.Enabled = false;
                            button5.Enabled = true;

                            guardarcierre();
                            if (impri)
                            {
                                formatodeactura();
                                imprimir();
                                button6.Enabled = true;
                                limpieza();
                            }

                        }
                       


                    }
                    else
                    {
                        MessageBox.Show("No ha ingresado valores suficientes para realizar el cierre", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }

                }


            }
            catch (Exception errf)
            {

                MessageBox.Show(errf.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void sumarefes()
        {

            sumaf = 0;
            tarjetaf = 0;
            efectivof = 0;
            try
            {
                for (int h = 0; h < dataGridView1.Rows.Count; h++)
                {

                    sumaf += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());
                    if (dataGridView1.Rows[h].Cells[5].Value.ToString() == "Efectivo")
                    {

                        efectivof += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());
                    }
                    else if (dataGridView1.Rows[h].Cells[5].Value.ToString() == "Tarjeta")
                    {
                        tarjetaf += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());

                    }
                }
                textBox2.Text =  string.Format("{0:N2}",sumaf);
                textBox13.Text = string.Format("{0:N2}", efectivof);

                textBox14.Text = string.Format("{0:N2}", tarjetaf);
                textBox7.Text = string.Format("{0:N2}", (efectivof+tarjetaf));

            }
            catch (Exception trend)
            {
                MessageBox.Show("", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            historico his = new historico();
            his.Show(this);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            guardarcierre();
            if (impri)
            {
                formatodeactura();
                imprimir();
                button6.Enabled = true;
                limpieza();
            }
        }

       

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
         

         
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                double fon = double.Parse(textBox5.Text);
                textBox6.Text = string.Format("{0:N2}", (fon+ double.Parse(textBox6.Text)));
                textBox5.ReadOnly = true;
                button5.Enabled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) ||char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text =="Global")
            {
                button6.Visible = true;
                button2.Visible = false;
                comboBox2.Visible = false;
                label4.Visible = false;

            }
            else
            {
                button6.Visible = false;
                button2.Visible = true;
                comboBox2.Visible = true;
                label4.Visible = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            verificarcierre();


            cargarFacturaporcajero();


            sumarefes();

            sumatoriatotal();
            button5.Enabled = true;
            button6.Enabled = false;
            button2.Enabled = false;
            comboBox2.Visible = false;
            label4.Visible = false;
           
        }

        private void comboBox2_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sumaf = 0;
            tarjetaf = 0;
            efectivof = 0;
            try
            {
                for (int h = 0; h < dataGridView1.Rows.Count; h++)
                {

                    sumaf += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());
                    if (dataGridView1.Rows[h].Cells[5].Value.ToString() == "Efectivo")
                    {

                        efectivof += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());
                    }
                    else if (dataGridView1.Rows[h].Cells[5].Value.ToString() == "Tarjeta")
                    {
                        tarjetaf += double.Parse(dataGridView1.Rows[h].Cells[3].Value.ToString());

                    }
                }
                textBox2.Text = string.Format("{0:N2}", sumaf);
         
                
            }
            catch (Exception trend)
            {

                MessageBox.Show("","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}

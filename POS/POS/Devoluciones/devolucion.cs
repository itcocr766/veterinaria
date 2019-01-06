using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using POS.Modelo;
namespace POS.Devoluciones
{
    public partial class devolucion : Form
    {
        Anulaciones anu;
        int conta=0;
        string productos;
        string formato;
        StreamReader streamToPrint;
        Font printFont;
        StreamWriter facturawr;
        double datdouble;
        double pre;
        double devu;
        double resu;
        bool imprim;
        int desc;
        string num;
        double subtotal = 0;
        double descuento = 0;
        double total = 0;
        public devolucion(Anulaciones a)
        {
            InitializeComponent(); anu = a;
        }

        private void devolucion_Load(object sender, EventArgs e)
        {
                  
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        public void formatodeactura()
        {

            productos = "";
            formato = "";
            num = "";
            try
            {






                for (int da = 0; da < dataGridView1.Rows.Count; da++)
                {



                    if (dataGridView1.Rows[da].Cells[0].Value != null && dataGridView1.Rows[da].Cells[6].Value.ToString() == "1")
                    {

                        productos += "\n    " + dataGridView1.Rows[da].Cells[2].Value.ToString() +
                             "    " + dataGridView1.Rows[da].Cells[1].Value.ToString() + "    " +
                       dataGridView1.Rows[da].Cells[3].Value.ToString();

                    }
                    else if (dataGridView1.Rows[da].Cells[0].Value != null && dataGridView1.Rows[da].Cells[6].Value.ToString() == "1")
                    {

                        productos += "\n    " + dataGridView1.Rows[da].Cells[2].Value.ToString() +
                                 "    " + dataGridView1.Rows[da].Cells[1].Value.ToString() + "    " +
                           dataGridView1.Rows[da].Cells[3].Value.ToString();

                    }

                }

                
                using (var mysql=new Mysql())
                {
                    mysql.conexion2();
                    mysql.cadenasql = "select max(Id) from notascredito";
                    mysql.comando = new MySqlCommand(mysql.cadenasql,mysql.con);
                    using (MySqlDataReader lee=mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            num = lee["max(Id)"].ToString();
                        }
                    
                    }
                        mysql.rol();
                    mysql.Dispose();

                }




                    formato =
                       ConfigurationManager.AppSettings["cadena2"] + "\n" +
                   "\n    N.CREDITO : "+ num+"\n" +
                    "     FECHA  :" + DateTime.Now.ToString() + "\n" +
                   "      CAJERO: " + textBox4.Text + "\n" +
                   "      CLIENTE: " + textBox2.Text + "\n" +

                "\nCantidad        Artículo        Precio\n" +
               "----------------------------------------------------------\n" + productos +
                "\n----------------------------------------------------------\n" +
                "ARTICULOS=   ₡" + conta + "\n" +
                "SUBTOTAL=   ₡" + textBox6.Text + "\n" +
                "DESCUENTO=   ₡" + textBox5.Text + "\n" +
                "TOTAL=   ₡" + textBox1.Text.Trim() + "\n" +
                "----------------------------------------------------------\n" +
               "NOTAS : \n" +
               "----------------------------------------------------------\n" +
                 richTextBox1.Text + "\n" +

               "\n----Autorizada mediante resolución--------\n---------N° DGT‐R‐48‐2016 del 07---------\n----------de octubre de 2016.---------------\n----------¡Gracias por su compra!--------------\n********Esperamos servirle de nuevo ************";










                facturawr = new StreamWriter("Nota.txt");
                facturawr.WriteLine(formato);
                facturawr.Flush();
                facturawr.Close();






            }
            catch (Exception err_0016)
            {
                MessageBox.Show(err_0016.ToString());


            }

        }


        public void imprimir()
        {

            try
            {



                streamToPrint = new StreamReader
                    ("Nota.txt");


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
                MessageBox.Show(err_004.ToString());


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
                MessageBox.Show(err_005.ToString());

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            guardarnotacredito();
            if (imprim)
            {
                formatodeactura();
                imprimir();
                limpieza();
                this.Visible = false;
            }
            
        }


       

        private void limpieza()
        {
            dataGridView1.DataSource = null;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox1.Text = "";
        }

        private void guardarnotacredito()
        {
            desc = 0;
         
            imprim = false;
            try
            {
                if (dataGridView1.Rows.Count>0&&!string.IsNullOrEmpty(textBox1.Text)&& !string.IsNullOrEmpty(textBox2.Text)&&
                    !string.IsNullOrEmpty(textBox3.Text)&& !string.IsNullOrEmpty(richTextBox1.Text))
                {
                    for (int v=0;v<dataGridView1.Rows.Count;v++)
                    {
                        if (double.Parse(dataGridView1.Rows[v].Cells[4].Value.ToString())>0)
                        {
                            desc = 1;
                           
                         
                        }
                        

                    }

                    using (var mysql = new Mysql())
                    {
                        mysql.conexion2();
                        for (int x=0;x<dataGridView1.Rows.Count ; x++)
                        {
                            mysql.cadenasql = "UPDATE items SET OnHand=((SELECT OnHand)+" + Int32.Parse(dataGridView1.Rows[x].Cells[5].Value.ToString()) + ") where Barcode='" + dataGridView1.Rows[x].Cells[0].Value + "'";
                            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                            mysql.comando.ExecuteNonQuery();
                        }

                        mysql.cadenasql = "INSERT INTO `notascredito`(`Cliente`, `Monto`, `Detalle`, `Descuento`) VALUES ('" +Int32.Parse(textBox3.Text) + "','" +double.Parse(textBox1.Text) + "','" + richTextBox1.Text + "','" + desc + "')";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();

                        mysql.rol();
                        mysql.Dispose();
                        MessageBox.Show("Se guardo la nota de crédito para el cliente: "+textBox2.Text, "Solicitud procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                    //using (var mysql2=new Mysql())
                    //{
                    //    mysql2.conexion();
                    //    mysql2.cadenasql = "update item set OnHand='"++"'";
                    //    mysql2.comando = new MySqlCommand(mysql2.cadenasql,mysql2.con);
                    //    mysql2.comando.ExecuteNonQuery();

                    //    mysql2.Dispose();
                    //}

                        imprim = true;
                }
                else
                {
                    MessageBox.Show("Faltan datos...","Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
                
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString());
            }
        }

        private void dataGridView1_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            datdouble = 0;
            pre = 0;
            devu = 0;
            resu = 0;

          
            int contador = 0;
            try
            {

                for (int u = 0; u < dataGridView1.Rows.Count; u++)
                {
                    if (dataGridView1.Rows[u].Cells[6].Value.ToString() == "1")
                    {
                        contador++;
                  
                    }



                }

                if (contador>0)
                {
                     subtotal = 0;
                     descuento = 0;
                     total = 0;
                    for (int k=0;k<dataGridView1.Rows.Count;k++)
                    {
                        if (dataGridView1.Rows[k].Cells[6].Value.ToString() == "1")
                        {

                            descuento += double.Parse(dataGridView1.Rows[k].Cells[4].Value.ToString());
                            subtotal += (double.Parse(dataGridView1.Rows[k].Cells[3].Value.ToString()) / 1.13) - descuento;
                            total += (double.Parse(dataGridView1.Rows[k].Cells[3].Value.ToString()) - (((double.Parse(dataGridView1.Rows[k].Cells[4].Value.ToString())*13)/100)+double.Parse(dataGridView1.Rows[k].Cells[4].Value.ToString())));

                        }

                    }

                    textBox5.Text = string.Format("{0:N2}", descuento);
                    textBox6.Text = string.Format("{0:N2}", subtotal);
                    textBox1.Text = string.Format("{0:N2}", total);

                }
                else
                {
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                    textBox1.Text = "0";
                }

                //if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[6]&& dataGridView1.CurrentRow.Cells[6].Value.ToString() == "1")
                //{
                //    textBox5.Text =string.Format("{0:N2}", double.Parse(textBox5.Text)+ double.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()));
                //    textBox6.Text = string.Format("{0:N2}",((double.Parse(textBox6.Text) + (double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString()) * double.Parse(dataGridView1.CurrentRow.Cells[6].Value.ToString())))/1.13));
                //    textBox1.Text = string.Format("{0:N2}", (double.Parse(textBox1.Text) + double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString()) * double.Parse(dataGridView1.CurrentRow.Cells[6].Value.ToString())).ToString());

                //    conta+=1;
                    
                //}
                //else if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[6] && dataGridView1.CurrentRow.Cells[6].Value.ToString() == "0")
                //{
                  
                    
                //     datdouble = double.Parse(textBox1.Text);
                //     pre = double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                //     devu = 1.0;
                //     resu = datdouble - (pre * devu);
                  
                //    textBox1.Text = "";
                //    textBox1.Text = resu.ToString();


                //    textBox5.Text = string.Format("{0:N2}",double.Parse(textBox5.Text)- double.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()));
                //    textBox6.Text = string.Format("{0:N2}", ((double.Parse(textBox6.Text) - (double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString()) * double.Parse(dataGridView1.CurrentRow.Cells[6].Value.ToString()))) / 1.13));

                //}
                //else if (dataGridView1.CurrentCell==dataGridView1.CurrentRow.Cells[5]){

                //}
                //else
                //{
                //    string dat = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                //    MessageBox.Show("Ha ingresado un valor incorrecto!. Por favor vuelva a intentarlo", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    dataGridView1.CurrentRow.Cells[6].Value = dat;
                //}


              



            }
            catch (NullReferenceException nuh)
            {

            }
            catch (Exception eju)
            {
                MessageBox.Show(eju.ToString());

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var mysql=new Mysql())
                {
                    for (int j=0;j<dataGridView1.Rows.Count;j++)
                    {
                        mysql.conexion();
                        mysql.cadenasql = "UPDATE items SET OnHand=((SELECT OnHand)+" + Int32.Parse(dataGridView1.Rows[j].Cells[5].Value.ToString()) + ") where Barcode='" + dataGridView1.Rows[j].Cells[0].Value + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        mysql.Dispose();
                       
                    }
                    MessageBox.Show("Se actualizo el inventario de acuerdo a los datos suministrados", "Solicitud Procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Visible = false;
               
                }
            }
            catch (Exception ej)
            {
                MessageBox.Show(ej.Message,"No logramos conectar",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
        }
    }
}

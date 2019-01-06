
using POS.Control;
using POS.Modelo;
using POS.Vista;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Configuration;
using POS.Contrasena;
using POS.clientesprincipal;
using POS.cedulas;
using POS.vendedoresprincipal_;
using Newtonsoft.Json;
using POS.clientes;
using POS.Inicio2;
using System.Runtime.InteropServices;
using POS.HACIENDA;
using POS.Anular;
using POS.nc;
using Microsoft.CSharp.RuntimeBinder;
using System.Net;
using System.Globalization;
using POS.vueltos;

namespace POS
{
    public partial class Form1 : Form
    {
        bool bandera,bandera2;
        public string elvuelto;
        decimal cantited;
        decimal united;
        string naturaleza;
        decimal desctotalfe;
        List<DETAIL> detalles;
        string json;
        public Int64 idFac;
        public string clave;
        public string consecutivo;
       public static string descuentomayor = "";
        int cuenta = 0;
        string numeroRecibido = "";
        ConvertirANumeros canuj;
        ControlFactura c = new ControlFactura();   
        StreamReader  streamToPrint;
        Font printFont;
        string formato;
        string productos ;
        buscarcedula rClient;
        ENVIO enviarfactura;
        public static string cedula="";
        public static string nombrec = "";
        string len ;
        StreamWriter facturawr;
        ControlFactura crfa = new ControlFactura();
        double descuentosumado = 0;
        public static string cajero = "";
        double descuentos = 0;
        public static DataGridViewComboBoxCell combo; 
        conexionabasedatos cndb = new conexionabasedatos();
        public static bool apartado;
        public static bool impri;
        public string consecutivoFE;
        decimal impuestofe;
        decimal unitprice;
        TAX losimpuestos;
        decimal totalamountfe;
        decimal discountfe;
        decimal subtotalfe;
        decimal totallineamountfe;
        decimal totaltaxedgoodsfe;
        decimal totaltaxedfe;
        decimal totalexcemptgoodsfe;
        decimal totalexcemptfe;
        decimal totalnetsalesfe;
        decimal totalsalesfe;
        decimal impuestototal;
        static Respuesta respuesta;
        string comprobanteelectronico;
        Dictionary<string,string> diccionario,diccionario2;
        public Form1()
        {
            InitializeComponent();
            
            try
            {

                rClient = new buscarcedula();
                 canuj = new ConvertirANumeros();
                detalles = new List<DETAIL>();
                enviarfactura = new ENVIO();
                respuesta = new Respuesta();
      
                diccionario = new Dictionary<string, string>();
                diccionario2 = new Dictionary<string, string>();
            }
            catch (Exception err_25)
            {
                Mensaje.Error(err_25, "61");
              
            }


        }
       
        
        #region load
      


        private void Form1_Load(object sender, EventArgs e)
        {


            try
            {
            
                comboBox2.Focus();
                comboBox4.SelectedIndex = 0;
                this.StartPosition = FormStartPosition.CenterScreen;

                comboBox1.SelectedIndex = 0;

                timer1.Interval = 10;
                    timer1.Start();
            
                    //comboBox1.SelectedIndex = 0;
                   
                    label7.Text = DateTime.Now.ToShortDateString();
                    label8.Text = DateTime.Now.ToShortTimeString();


                comboBox2.DataSource = cargarVendedores();
                comboBox2.DisplayMember = "Codigo";
                comboBox2.ValueMember = "Codigo";

                comboBox3.DataSource = cargarClientes();
                comboBox3.DisplayMember = "Cedula";
                comboBox3.ValueMember = "Cedula";

                textBox1.DataSource = cargaitem();
                textBox1.DisplayMember = "CodigoA";
                textBox1.ValueMember = "CodigoA";

                comboBox5.DataSource = cargaitem2();
                comboBox5.DisplayMember = "Codigo";
                comboBox5.ValueMember = "Codigo";

                dictionary();
                dictionary2();
                descuent();
                //administracion();

            }
            catch (Exception err_005)
            {
                Mensaje.Error(err_005, "168");
               
                   
            }
        }

       

        private void descuent()
        {

            try
            {
                using (var mysql =new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select Montoad, Montoti from descuento";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader le=mysql.comando.ExecuteReader())
                    {
                        if (le.Read())
                        {
                            textBox18.Text = le["Montoad"].ToString();
                            textBox19.Text = le["Montoti"].ToString();
                        }
                        else
                        {
                            textBox18.Text = "50";
                            textBox19.Text = "50";
                        }
                       
                    }

                        mysql.Dispose();
                }
            }
            catch (Exception etc)
            {
                MessageBox.Show(etc.ToString());
            }


        }

        #endregion
        #region meterfactura
        public void meterFactura()
        {
   
            string type = "";
       
            try
            {
                desctotalfe = 0;
                 naturaleza="";
                impuestofe = 0;
                unitprice = 0;
                totalamountfe = 0;
                discountfe = 0;
                totallineamountfe = 0;
                subtotalfe = 0;
                totaltaxedgoodsfe = 0;
                totaltaxedfe= 0;
                totalexcemptfe = 0;
                totalexcemptgoodsfe = 0;
                totalnetsalesfe = 0;
                totalsalesfe = 0;
                impuestototal = 0;
                 cantited=0;
                 united=0;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                using (var mysql = new Mysql())
                {
                    mysql.conexion2();
                    mysql.cadenasql = "select count(*),Max(Numero) from factura where Numero like '"+ textBox4.Text + "%'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            if (Int64.Parse(lee["count(*)"].ToString())>0)
                            {
                                idFac = Int64.Parse(string.Concat(( textBox4.Text), (Int64.Parse(lee["Max(Numero)"].ToString().Substring(1)) + 1).ToString()));
                                //idFac = Int32.Parse(lee["Max(Numero)"].ToString()) + 1;
                            }
                            else
                            {
                                idFac = Int64.Parse(string.Concat( textBox4.Text, "1"));
                                //idFac = 1;

                            }


                        }

                




                    }

                    /////////////////////////////////////////////////////////////////////////////


                    mysql.cadenasql = "insert into factura(Numero,Fecha,Cliente,Total,CodigoCajero,TipoPago,NumerodeComprobante,CodigoVendedor,Tipo)values('"
                        + idFac + "','" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " "
                        + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "','" + comboBox3.Text.Trim() +
                        "','" + double.Parse(textBox5.Text.Trim()).ToString("0.00",CultureInfo.InvariantCulture) + "','" + textBox4.Text + "','" + comboBox1.Text +
                        "','" + textBox7.Text.Trim() + "','" + comboBox2.Text + "','Factura')";

                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    ///////////////////////////////////////////////////////////////////////////////////////////
                    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                    {
                        mysql.cadenasql = "INSERT INTO `detalles`(`NumeroFactura`, `Cliente`, `Item`, `Cantidad`, `Descuento`, `Precio`,`Impuesto`) VALUES ('" + idFac + "','" +
                            comboBox3.Text.Trim() + "','" + dataGridView1.Rows[count].Cells[0].Value + "','" +
                            dataGridView1.Rows[count].Cells[1].Value + "','" + double.Parse(dataGridView1.Rows[count].Cells[10].Value.ToString()).ToString("0.00", CultureInfo.InvariantCulture) + "','" +
                            double.Parse(dataGridView1.Rows[count].Cells[3].Value.ToString()).ToString("0.00", CultureInfo.InvariantCulture) + "','" + dataGridView1.Rows[count].Cells[9].Value + "')";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();

                        if (dataGridView1.Rows[count].Cells[11].Value.ToString()=="Codigo")
                        {
                            mysql.cadenasql = "UPDATE items SET Cantidad=((SELECT Cantidad)-" + double.Parse(dataGridView1.Rows[count].Cells[1].Value.ToString()).ToString("0.00", CultureInfo.InvariantCulture) + ") where Codigo='" + dataGridView1.Rows[count].Cells[0].Value + "'";
                            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                            mysql.comando.ExecuteNonQuery();
                        }
                        else
                        {
                            mysql.cadenasql = "UPDATE items SET Cantidad=((SELECT Cantidad)-" + double.Parse(dataGridView1.Rows[count].Cells[1].Value.ToString()).ToString("0.00", CultureInfo.InvariantCulture) + ") where CodigoA='" + dataGridView1.Rows[count].Cells[0].Value + "'";
                            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                            mysql.comando.ExecuteNonQuery();
                        }

                     


                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    mysql.cadenasql = "select Max(Numero),Fecha from factura where CodigoCajero='" + textBox4.Text + "' AND Cliente='" + comboBox3.Text.Trim() + "' AND Total='" + double.Parse(textBox5.Text.Trim()).ToString("0.00", CultureInfo.InvariantCulture) + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        if (lee.Read())
                        {
                            if (lee["Max(Numero)"].ToString() != "")
                            {
                                numeroRecibido = lee["Max(Numero)"].ToString();
                                //busquedafech = DateTime.Parse(lee["Fecha"].ToString());
                            }
                            else
                            {
                                numeroRecibido = "0";
                                //busquedafech = DateTime.Now;

                            }


                            //MessageBox.Show(numeroRecibido);

                        }

                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                       
                    }

                    //MessageBox.Show("Factura guardada con éxito");

                    FE f = new FE()
                    {

                        CompanyAPI = ConfigurationManager.AppSettings["apicomp"]
                        //CompanyAPI = "c6e8b995-a098-4ff5-9748-c8ff3e637e79"

                        //CompanyAPI = "749ad71a-8e08-48b4-a5c1-6a5de55b677f"

                    };

                    if (comboBox3.Text == "0")
                    {
                        type = "04";
                    }
                    else
                    {
                        type = "01";

                    }


                    f.Key = new KEY()
                    {
                        Branch = "002",
                        Terminal = "001",
                        Type = type,
                        Voucher = numeroRecibido,
                        Country = "506",
                        Situation = "1"

                    };

                    f.Header = new HEADER()
                    {
                        Date = DateTime.Now.Date,
                        TermOfSale = "01",
                        CreditTerm = 0,
                        PaymentMethod = "01"
                    };

                    if (comboBox3.Text == "0")
                    {

                        f.Receiver = new RECEIVER()
                        {
                            Name = textBox17.Text,
                            Identification = new IDENTIFICATION
                            {
                                Type = "01",
                                Number = "000000000"

                            },
                            Email = textBox10.Text

                        };
                    }
                    else
                    {

                        f.Receiver = new RECEIVER()
                        {
                            Name = textBox17.Text,
                            Identification = new IDENTIFICATION
                            {
                                Type = "01",
                                Number = comboBox3.Text

                            },
                            Email = textBox10.Text

                        };
                    }



                    detalles.Clear();
                    for (int r = 0; r < dataGridView1.Rows.Count; r++)
                    {
                        if (dataGridView1.Rows[r].Cells[9].Value.ToString() == "Impuesto")
                        {



                            cantited = decimal.Parse(dataGridView1.Rows[r].Cells[1].Value.ToString());
                            united = decimal.Parse(dataGridView1.Rows[r].Cells[3].Value.ToString());
                            discountfe = decimal.Parse(dataGridView1.Rows[r].Cells[10].Value.ToString());
                            unitprice = decimal.Round((united / 1.13m), 2,MidpointRounding.AwayFromZero);
                            impuestofe = decimal.Round((((unitprice *cantited)-discountfe) *13)/100, 2, MidpointRounding.AwayFromZero);
                            totaltaxedgoodsfe += decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                            totalsalesfe += decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe)), 2, MidpointRounding.AwayFromZero);
                            totalnetsalesfe += decimal.Round(((totalsalesfe - discountfe)), 2, MidpointRounding.AwayFromZero);
                            totaltaxedfe += decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                            impuestototal += decimal.Round(impuestofe, 2, MidpointRounding.AwayFromZero);

                          
                            losimpuestos = new TAX()
                            {
                                Code = "01",
                                Rate = 13.0m,
                                Amount = decimal.Round(impuestofe, 2, MidpointRounding.AwayFromZero),
                                Exoneration = null
                            };

                            totalamountfe = decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                            desctotalfe += decimal.Round(discountfe,2,MidpointRounding.AwayFromZero);
                            subtotalfe = decimal.Round(((totalamountfe - discountfe)), 2, MidpointRounding.AwayFromZero);
                            totallineamountfe = decimal.Round((subtotalfe + impuestofe), 2, MidpointRounding.AwayFromZero);
                           
                            if (discountfe > 0)
                            {
                                naturaleza = "descuento de " + discountfe.ToString();

                            }
                            else
                            {
                                naturaleza = "";
                            }
                            DETAIL detail = new DETAIL()
                            {
                                Number = r + 1,
                                Code = new CODE
                                {
                                    Type = "04",
                                    Code = dataGridView1.Rows[r].Cells[0].Value.ToString()
                                },
                                Tax = new List<TAX>()
                                {
                                    losimpuestos
                                },
                           

                                Quantity = decimal.Parse(dataGridView1.Rows[r].Cells[1].Value.ToString()),
                                UnitOfMeasure = "Otros",
                                CommercialUnitOfMeasure = null,
                                Detail = dataGridView1.Rows[r].Cells[2].Value.ToString(),
                                UnitPrice = unitprice,
                                TotalAmount = totalamountfe,
                                
                                Discount =decimal.Round(discountfe,2,MidpointRounding.AwayFromZero),
                                NatureOfDiscount = naturaleza,
                                SubTotal = subtotalfe,
                                TotalLineAmount = totallineamountfe

                            };
                            detalles.Add(detail);
                        }
                        else
                        {
                            cantited = decimal.Parse(dataGridView1.Rows[r].Cells[1].Value.ToString());
                            united = decimal.Parse(dataGridView1.Rows[r].Cells[3].Value.ToString());
                            discountfe = decimal.Parse(dataGridView1.Rows[r].Cells[10].Value.ToString());
                            impuestofe = 0;

                            unitprice = united;

                            totaltaxedgoodsfe += 0;


                            totaltaxedfe += 0;
                            totalamountfe = decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                           
                            totalexcemptgoodsfe += decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                            totalexcemptfe += decimal.Round(((unitprice * cantited)), 2, MidpointRounding.AwayFromZero);
                            totalsalesfe += decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe)), 2, MidpointRounding.AwayFromZero);
                            totalnetsalesfe += decimal.Round(((totalsalesfe - discountfe)), 2, MidpointRounding.AwayFromZero);
                            desctotalfe += decimal.Round(discountfe, 2,MidpointRounding.AwayFromZero);

                            subtotalfe = decimal.Round(((totalamountfe - discountfe)), 2, MidpointRounding.AwayFromZero);
                            totallineamountfe = decimal.Round((subtotalfe + impuestofe), 2, MidpointRounding.AwayFromZero);
                            
                            if (discountfe > 0)
                            {
                                naturaleza = "descuento de " + discountfe.ToString();

                            }
                            else
                            {
                                naturaleza = "";
                            }

                            DETAIL detail = new DETAIL()
                            {
                                Number = r + 1,
                                Code = new CODE
                                {
                                    Type = "04",
                                    Code = dataGridView1.Rows[r].Cells[0].Value.ToString()
                                },


                                Quantity = decimal.Parse(dataGridView1.Rows[r].Cells[1].Value.ToString()),
                                UnitOfMeasure = "Otros",
                                CommercialUnitOfMeasure = null,
                                Detail = dataGridView1.Rows[r].Cells[2].Value.ToString(),
                                UnitPrice = unitprice,
                                TotalAmount = totalamountfe,
                                Discount = decimal.Round(discountfe, 2,MidpointRounding.AwayFromZero),
                                NatureOfDiscount = naturaleza,
                                SubTotal = subtotalfe,
                                TotalLineAmount = totallineamountfe

                            };
                            detalles.Add(detail);
                        }






                    }
                    f.Detail = null;


                    f.Detail = detalles;


                    //f.Reference=new List<REFERENCE>()
                    //{

                    //};


                    f.Summary = new SUMMARY()
                    {
                        Currency = "CRC",
                        ExchangeRate = 1,
                        TotalTaxedService = 0,
                        TotalExemptService = 0,
                        TotalTaxedGoods = decimal.Round(totaltaxedgoodsfe, 2, MidpointRounding.AwayFromZero),
                        TotalExemptGoods = decimal.Round(totalexcemptgoodsfe, 2, MidpointRounding.AwayFromZero),
                        TotalTaxed = decimal.Round(totaltaxedfe, 2, MidpointRounding.AwayFromZero),
                        TotalExempt = decimal.Round(totalexcemptfe, 2, MidpointRounding.AwayFromZero),
                        TotalSale = decimal.Round((totaltaxedgoodsfe + totalexcemptgoodsfe), 2, MidpointRounding.AwayFromZero),
                        TotalDiscounts = decimal.Round((desctotalfe),2, MidpointRounding.AwayFromZero),
                        TotalNetSale = decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe)-desctotalfe), 2, MidpointRounding.AwayFromZero),
                        TotalTaxes = decimal.Round(impuestototal, 2, MidpointRounding.AwayFromZero),
                        TotalVoucher = decimal.Round((totaltaxedgoodsfe + totalexcemptgoodsfe + impuestototal)-desctotalfe, 2, MidpointRounding.AwayFromZero)
                    };

                    json = JsonConvert.SerializeObject(f, Newtonsoft.Json.Formatting.Indented);


                    string strJSON = string.Empty;


                    SendInvoicesAGC(f);

                    if (!string.IsNullOrEmpty(respuesta.codificacion.clave))
                    {

                        clave = respuesta.codificacion.clave;
                        consecutivo = respuesta.codificacion.consecutivo;
                        mysql.cadenasql = "INSERT INTO `hacienda`(`Clave`, `Consecutivo`, `Comprobante`) VALUES ('" + clave + "','" + consecutivo + "','" + numeroRecibido + "')";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();

                       
                        impri = true;
                        textBox1.Visible = true;
                    }
                    else
                    {
                        impri = false;
                        
                        textBox1.Visible = false;
                    
                        MessageBox.Show("No pudimos comunicarnos con el Ministerio de hacienda por favor verifique que la factura :" + numeroRecibido +
                            "no haya sido almacenada en la base de datos interna y anulela de ser necesario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }


                    mysql.rol();
                    mysql.Dispose();
                }



            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show(respuesta.status + respuesta.message + "\n" + desctotalfe);
            }
            catch (Exception ftr)
            {
                Mensaje.Error(ftr, "191");

               
                impri = false;

            }
            
               


          

           


            

          
        }





        public static Respuesta SendInvoicesAGC(FE factura)

        {




            //string urlAPI = "http://104.43.136.13:8080/api/invoice/";
            string urlAPI = ConfigurationManager.AppSettings["endpo"];

            string api = urlAPI + "/api/invoice";



            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlAPI);



            httpWebRequest.ContentType = "application/json";

            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 5000;
           

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))

            {

                string json = JsonConvert.SerializeObject(factura);



                //Logs.SaveJson(json, company, factura.numero);



                streamWriter.WriteAsync(json);

            }



            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))

            {

                var result = streamReader.ReadToEnd();

                respuesta = JsonConvert.DeserializeObject<Respuesta>(result);

                return respuesta;

            }

        }


        #endregion
        
        #region metodo llenar
        //public void llenar(string codigo)
        //{

        //    descuentos = 0;
        //    int cantidad = 0;
        //    double precio = 0;
        //    double totalsinimpuesto = 0;
        //    double totalconimpuesto = 0;
        //    double total = 0;


        //    bool existe = false;
        //    int existens = 0;
        //    int existen2 = 0;
        //    try
        //    {
        //        using (var mysql = new Mysql())
        //        {

        //            mysql.conexion();

        //            mysql.cadenasql = "select * from items where Barcode='" + codigo + "'";
        //            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
        //            mysql.comando.ExecuteNonQuery();
        //            mysql.lector = mysql.comando.ExecuteReader();

        //            if (mysql.lector.Read())
        //            {

        //                existens = Int32.Parse(mysql.lector["OnHand"].ToString());

        //                for (int y = 0; y < dataGridView1.Rows.Count; y++)
        //                {



        //                    if (textBox1.Text.Trim() == dataGridView1.Rows[y].Cells[0].Value.ToString())
        //                    {

        //                        existe = true;
        //                        existen2 = Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()) + Int32.Parse(textBox2.Text.Trim());

        //                    }






        //                }


        //                if (existe == true && existens >= existen2 && existens > 0)
        //                {
        //                    for (int y = 0; y < dataGridView1.Rows.Count; y++)
        //                    {
        //                        if (dataGridView1.Rows[y].Cells[0].Value.ToString() == textBox1.Text.Trim())
        //                        {

        //                            //if (mysql.lector["Impuesto"].ToString() == "Impuesto")
        //                            //{
        //                                dataGridView1.Rows[y].Cells[1].Value = Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()) + Int32.Parse(textBox2.Text.Trim());
        //                            calcularTotal();
        //                                //dataGridView1.Rows[y].Cells[8].Value = double.Parse(dataGridView1.Rows[y].Cells[3].Value.ToString()) * Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString());
        //                                ////dataGridView1.Rows[y].Cells[10].Value =(double.Parse(dataGridView1.Rows[y].Cells[10].Value.ToString())*Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()));
        //                                ////actualizargridgeneral();
        //                                ////agregarFila();
        //                                //calcularTotal();
        //                            //}
        //                            //else
        //                            //{
        //                            //    dataGridView1.Rows[y].Cells[1].Value = Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()) + Int32.Parse(textBox2.Text.Trim());

        //                            //    //dataGridView1.Rows[y].Cells[8].Value = double.Parse(dataGridView1.Rows[y].Cells[3].Value.ToString()) * Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString());
        //                            //    ////dataGridView1.Rows[y].Cells[10].Value =(double.Parse(dataGridView1.Rows[y].Cells[10].Value.ToString())*Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()));
        //                            //    ////actualizargridgeneral();
        //                            //    ////agregarFila();
        //                            //    //calcularTotal();

        //                            //}


        //                        }




        //                    }

        //                }

        //                else if (existe == false && existens >= Int32.Parse(textBox2.Text.Trim()) && existens > 0)
        //                {
        //                    if (mysql.lector["Impuesto"].ToString() == "Impuesto")
        //                    {
        //                        impuestoenespanol = "(G)";
        //                        cantidad = Int32.Parse(textBox2.Text.Trim());
        //                        precio = double.Parse(mysql.lector["Precio"].ToString());
        //                        totalsinimpuesto = (precio * cantidad) / 1.13;


        //                        totalconimpuesto = precio * cantidad;
        //                        total = totalconimpuesto * cantidad;
        //                        dataGridView1.Rows.Add(mysql.lector["Barcode"], cantidad, mysql.lector["Descripcion"], precio, canuj.enletras(precio.ToString()), "0", totalsinimpuesto, totalconimpuesto, total, impuestoenespanol, "0");
        //                        //agregarFila();
        //                        calcularTotal();
        //                    }
        //                    else if (mysql.lector["Impuesto"].ToString() == "Non")
        //                    {
        //                        impuestoenespanol = "(E)";
        //                        cantidad = Int32.Parse(textBox2.Text.Trim());
        //                        precio = double.Parse(mysql.lector["Precio"].ToString());
        //                        totalsinimpuesto = (precio * cantidad);


        //                        totalconimpuesto = precio * cantidad;
        //                        total = totalconimpuesto * cantidad;
        //                        dataGridView1.Rows.Add(mysql.lector["Barcode"], cantidad, mysql.lector["Descripcion"], precio, canuj.enletras(precio.ToString()), "0", totalsinimpuesto, totalconimpuesto, total, impuestoenespanol, "0");
        //                        //agregarFila();
        //                        calcularTotal();

        //                    }




        //                }
        //                else
        //                {

        //                    MessageBox.Show("Ya No hay suficiente inventario para este item");
        //                    existens = 0;
        //                }



        //                textBox1.Focus();
        //            }


        //            else
        //            {

        //                MessageBox.Show("No hemos podido encontrar este producto,verifique que sea un producto existente");
        //                textBox1.Text = "";
        //                textBox1.Focus();
        //            }
        //            // MessageBox.Show(re["Barcode"].ToString()+"  "+re["COGSAccount"].ToString());


        //            mysql.Dispose();

        //        }





        //    }
        //    catch (Exception err_006)
        //    {
        //        Mensaje.Error(err_006, "416");



        //    }


        //}



        public void llenar(string codigo)
        {
            //tipoCod = "";
            int cuenta=0;
            string cod = "";
            string codia = "";
            double existencia = 0;
            try
            {
                if (!string.IsNullOrEmpty(diccionario2.FirstOrDefault(x => x.Key == textBox1.Text).Value))
                {
                    label51.Text = diccionario2.FirstOrDefault(f => f.Key == textBox1.Text).Value;


                }
                else
                {
                    if (comboBox5.Items.Count > 0)
                    {
                        for (int a = 0; a < comboBox5.Items.Count; a++)
                        {
                            comboBox5.SelectedIndex = a;
                            //MessageBox.Show(comboBox5.SelectedValue.ToString());
                            if (comboBox5.SelectedValue.ToString() == textBox1.Text)
                            {
                                label51.Text = diccionario.FirstOrDefault(x => x.Key == comboBox5.SelectedValue.ToString()).Value;
                            }
                        }
                    }


                }


                using (var mysql = new Mysql())
                {
                    mysql.conexion();

                    mysql.cadenasql = "select count(*),Codigo,CodigoA,Descripcion,Cantidad,Precio,Impuesto from items where Codigo='" + textBox1.Text + "' AND length(Codigo)>0";

                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            if (Int64.Parse(lee["count(*)"].ToString()) > 0)
                            { existencia = double.Parse(lee["Cantidad"].ToString());


                                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                {
                                    if (textBox1.Text==dataGridView1.Rows[j].Cells[0].Value.ToString())
                                    {
                                        cuenta++;
                                    }
                                }

                                if (cuenta<existencia&&existencia>0)
                                {
                                    bandera = true;
                                    cod = lee["Codigo"].ToString();
                                    codia = lee["CodigoA"].ToString();
                                    dataGridView1.Rows.Add(textBox1.Text, textBox2.Text, lee["Descripcion"].ToString(), lee["Precio"].ToString(), lee["Cantidad"].ToString(), 0, 0, 0, 0, lee["Impuesto"].ToString(), 0, "Codigo");
                                    calcularTotal();
                                }
                                else
                                {
                                    MessageBox.Show("No hay existencia", "No hay existencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                }





                            }
                            else
                            {
                                bandera = false;
                            }
                        }
                    }

                    if (!bandera)
                    {
                        mysql.cadenasql = "select count(*),Codigo,CodigoA,Descripcion,Cantidad,Precio,Impuesto from items where CodigoA='" + textBox1.Text + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        using (MySqlDataReader lee2 = mysql.comando.ExecuteReader())
                        {
                            while (lee2.Read())
                            {
                                if (Int64.Parse(lee2["count(*)"].ToString()) > 0)
                                {


                                    existencia = double.Parse(lee2["Cantidad"].ToString());



                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                        if (textBox1.Text == dataGridView1.Rows[j].Cells[0].Value.ToString())
                                        {
                                            cuenta++;
                                        }
                                    }


                                    if (cuenta < existencia && existencia > 0)
                                    {
                                        bandera2 = true;
                                        cod = lee2["Codigo"].ToString();
                                        codia = "";
                                        dataGridView1.Rows.Add(textBox1.Text, textBox2.Text, lee2["Descripcion"].ToString(), lee2["Precio"].ToString(), lee2["Cantidad"].ToString(), 0, 0, 0, 0, lee2["Impuesto"].ToString(), 0, "CodigoA");
                                        calcularTotal();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No hay existencia", "No hay existencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }







                                }
                                else
                                {
                                    bandera2 = false;
                                }
                            }

                        }
                    }

                    if (!bandera && !bandera2)
                    {
                        MessageBox.Show("No pudimos encontrar el código ingresado", "Solicitud inválida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }



                    mysql.Dispose();
                }




                //using (var mysql=new Mysql())
                //{
                //    mysql.conexion();
                //    mysql.cadenasql = "select * from items where Codigo='" + codigo + "'";
                //    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                //    mysql.comando.ExecuteNonQuery();


                //    using (MySqlDataReader lec = mysql.comando.ExecuteReader())
                //    {
                //        if(lec.Read())
                //        {


                //            if (lec["Impuesto"].ToString() == "Impuesto")
                //            {
                //                impos = true;
                //            }
                //            else
                //            {
                //                impos = false;
                //            }

                //            for (int y = 0; y < dataGridView1.Rows.Count; y++)
                //            {



                //                if (textBox1.Text.Trim() == dataGridView1.Rows[y].Cells[0].Value.ToString())
                //                {

                //                    existe = true;


                //                }






                //            }


                //            if (existe == true)
                //            {
                //                for (int y = 0; y < dataGridView1.Rows.Count; y++)
                //                {
                //                    if (dataGridView1.Rows[y].Cells[0].Value.ToString() == textBox1.Text.Trim())
                //                    {
                //                        dataGridView1.Rows[y].Cells[1].Value = decimal.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()) + decimal.Parse(textBox2.Text.Trim());

                //                        dataGridView1.Rows[y].Cells[7].Value = (decimal.Parse(dataGridView1.Rows[y].Cells[6].Value.ToString()) * decimal.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()));
                //                        if (impos)
                //                        {
                //                            dataGridView1.Rows[y].Cells[8].Value = (decimal.Parse(dataGridView1.Rows[y].Cells[3].Value.ToString()) - (decimal.Parse(dataGridView1.Rows[y].Cells[3].Value.ToString()) / 1.13m)) * decimal.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString());
                //                        }
                //                        else
                //                        {
                //                            dataGridView1.Rows[y].Cells[8].Value = decimal.Parse(dataGridView1.Rows[y].Cells[3].Value.ToString()) * decimal.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString());

                //                        }
                //                        //dataGridView1.Rows[y].Cells[10].Value =(double.Parse(dataGridView1.Rows[y].Cells[10].Value.ToString())*Int32.Parse(dataGridView1.Rows[y].Cells[1].Value.ToString()));

                //                        agregarFila();

                //                    }




                //                }

                //            }

                //            else if (existe == false)
                //            {
                //                string totsin = "";
                //                if (impos)
                //                {
                //                    if (presesp)
                //                    {
                //                        precio = presespe;
                //                        totsin = string.Format("{0:N3}", (precio / 1.13m));
                //                        totalsinimpuesto = decimal.Parse(totsin);
                //                        cantidad = decimal.Parse(textBox2.Text.Trim());
                //                        impuestounidad = (precio - (precio / 1.13m)) * cantidad;
                //                        impuestoenespanol = "(G)";

                //                        totalconimpuesto = precio;
                //                        total = totalconimpuesto * cantidad;
                //                        dataGridView1.Rows.Add(lec["Codigo"], cantidad, lec["Nombre"], precio, canuj.enletras(precio.ToString()), totalsinimpuesto, totalconimpuesto, (totalconimpuesto * cantidad), impuestounidad, impuestoenespanol);
                //                        agregarFila();
                //                    }
                //                    else
                //                    {

                //                        precio = decimal.Parse(lec["Precio"].ToString());
                //                        totsin = string.Format("{0:N3}", (precio / 1.13m));
                //                        totalsinimpuesto = decimal.Parse(totsin);
                //                        cantidad = decimal.Parse(textBox2.Text.Trim());
                //                        impuestounidad = (precio - (precio / 1.13m)) * cantidad;
                //                        impuestoenespanol = "(G)";

                //                        totalconimpuesto = precio;
                //                        total = totalconimpuesto * cantidad;
                //                        dataGridView1.Rows.Add(lec["Codigo"], cantidad, lec["Nombre"], precio, canuj.enletras(precio.ToString()), totalsinimpuesto, totalconimpuesto, (totalconimpuesto * cantidad), impuestounidad, impuestoenespanol);
                //                        agregarFila();
                //                    }

                //                }
                //                else if (impos == false)
                //                {

                //                    if (presesp)
                //                    {

                //                        precio = presespe;
                //                        totalsinimpuesto = precio;
                //                        cantidad = decimal.Parse(textBox2.Text.Trim());
                //                        exentounidad = precio * cantidad;
                //                        impuestoenespanol = "(E)";
                //                        totalconimpuesto = precio;
                //                        total = totalconimpuesto * cantidad;
                //                        dataGridView1.Rows.Add(lec["Codigo"], cantidad, lec["Nombre"], precio, canuj.enletras(precio.ToString()), totalsinimpuesto, totalconimpuesto, (totalconimpuesto * cantidad), exentounidad, impuestoenespanol);
                //                        agregarFila();
                //                    }
                //                    else
                //                    {
                //                        precio = decimal.Parse(lec["Precio"].ToString());
                //                        totalsinimpuesto = precio;
                //                        cantidad = decimal.Parse(textBox2.Text.Trim());
                //                        exentounidad = precio * cantidad;
                //                        impuestoenespanol = "(E)";
                //                        totalconimpuesto = precio;
                //                        total = totalconimpuesto * cantidad;
                //                        dataGridView1.Rows.Add(lec["Codigo"], cantidad, lec["Nombre"], precio, canuj.enletras(precio.ToString()), totalsinimpuesto, totalconimpuesto, (totalconimpuesto * cantidad), exentounidad, impuestoenespanol);
                //                        agregarFila();
                //                    }
                //                }




                //            }
                //            else
                //            {

                //                MessageBox.Show("Ya No hay suficiente inventario para este item");

                //            }



                //            textBox1.Focus();
                //        }


                //        else
                //        {

                //            MessageBox.Show("No hemos podido encontrar este producto,verifique que sea un producto existente");
                //            textBox1.Text = "";
                //            textBox1.Focus();
                //        }


                //    }

                //    mysql.Dispose();

                //}












            }
            catch (Exception err_006)
            {
                Mensaje.Error(err_006, "416");



            }

            //descuentos = 0;
            //double cantidad = 0;
            //double precio = 0;
            //decimal totalsinimpuesto = 0;
            //decimal totalconimpuesto = 0;
            //double existencias = 0;



            //try
            //{



            //    using (var mysql = new Mysql())
            //    {

            //        mysql.conexion();

            //        mysql.cadenasql = "select * from items where Barcode='" + codigo + "'";
            //        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
            //        mysql.comando.ExecuteNonQuery();
            //        mysql.lector = mysql.comando.ExecuteReader();

            //        if (mysql.lector.Read())
            //        {



            //            if (mysql.lector["Impuesto"].ToString() == "Impuesto")
            //            {
            //                impuestoenespanol = "(G)";
            //                cantidad = double.Parse(textBox2.Text.Trim());
            //                precio = double.Parse(mysql.lector["Precio"].ToString());
            //                existencias = double.Parse(mysql.lector["OnHand"].ToString());
            //                dataGridView1.Rows.Add(mysql.lector["Barcode"], cantidad, mysql.lector["Descripcion"], precio,existencias, "0", totalsinimpuesto, totalconimpuesto, 10000, impuestoenespanol, "0");
            //                //agregarFila();
            //                actualizargridgeneral();
            //                calcularTotal();

            //            }
            //            else
            //            {
            //                impuestoenespanol = "(E)";
            //                cantidad = double.Parse(textBox2.Text.Trim());
            //                precio = double.Parse(mysql.lector["Precio"].ToString());
            //                existencias = double.Parse(mysql.lector["OnHand"].ToString());
            //                dataGridView1.Rows.Add(mysql.lector["Barcode"], cantidad, mysql.lector["Descripcion"], precio,existencias, "0", totalsinimpuesto, totalconimpuesto, 10000, impuestoenespanol, "0");
            //                //agregarFila();
            //                actualizargridgeneral();
            //                calcularTotal();
            //            }





            //        }







            //        else
            //        {

            //            MessageBox.Show("No hemos podido encontrar este producto,verifique que sea un producto existente");
            //            textBox1.Text = "";
            //            textBox1.Focus();
            //        }
            //        // MessageBox.Show(re["Barcode"].ToString()+"  "+re["COGSAccount"].ToString());


            //        mysql.Dispose();

            //    }


            //    textBox1.Focus();


            //}
            //catch (FormatException fe)
            //{

            //}
            //catch (Exception err_006)
            //{
            //    Mensaje.Error(err_006, "416");



            //}


        }


        #endregion
        #region calcular el total
        public void calcularTotal()
        {
            double sub = 0;
            double totalt = 0;
            double pric = 0;
            double descuen = 0;
            double can = 0;
            double impi = 0;
            double unita = 0;
            double totalsub = 0;
            double totalimp = 0;
            double totalvou = 0;
            double totaldescuen = 0;
            // MessageBox.Show(totalt.ToString());
            try
            {
                for (int restadescuento = 0; restadescuento < dataGridView1.Rows.Count; restadescuento++)
                {
                    can = 0;
                    pric = 0;
                    descuen = 0;

                    if (dataGridView1.Rows[restadescuento].Cells[9].Value.ToString() == "Impuesto")
                    {
                        can = double.Parse(dataGridView1.Rows[restadescuento].Cells[1].Value.ToString());
                        pric = double.Parse(dataGridView1.Rows[restadescuento].Cells[3].Value.ToString());
                        descuen = double.Parse(dataGridView1.Rows[restadescuento].Cells[10].Value.ToString());


                        unita = Math.Round(((pric * can) / 1.13), 2);
                        impi = Math.Round((((unita * can) - descuen) * 13) / 100, 2);

                        sub = Math.Round(((unita * can) - descuen), 2);
                        totalt = Math.Round((sub + impi), 2);



                        dataGridView1.Rows[restadescuento].Cells[6].Value = Math.Round((unita - descuen), 2);
                        dataGridView1.Rows[restadescuento].Cells[7].Value = Math.Round(pric, 2);
                        dataGridView1.Rows[restadescuento].Cells[8].Value = Math.Round(totalt, 2);


                    }
                    else
                    {
                        can = double.Parse(dataGridView1.Rows[restadescuento].Cells[1].Value.ToString());
                        pric = double.Parse(dataGridView1.Rows[restadescuento].Cells[3].Value.ToString());
                        descuen = double.Parse(dataGridView1.Rows[restadescuento].Cells[10].Value.ToString());


                        unita = Math.Round((pric * can), 2);
                        impi = 0;

                        sub = Math.Round(((pric * can) - descuen), 2);
                        totalt = Math.Round((sub + impi), 2);



                        dataGridView1.Rows[restadescuento].Cells[6].Value = Math.Round((unita - descuen), 2);
                        dataGridView1.Rows[restadescuento].Cells[7].Value = Math.Round(pric, 2);
                        dataGridView1.Rows[restadescuento].Cells[8].Value = Math.Round(totalt, 2);

                    }
                }

                for (int g = 0; g < dataGridView1.Rows.Count; g++)
                {

                    if (dataGridView1.Rows[g].Cells[9].Value.ToString() == "Impuesto")
                    {
                        totalsub += Math.Round(((double.Parse(dataGridView1.Rows[g].Cells[8].Value.ToString())) / 1.13), 2);
                        //totalimp += Math.Round(imp, 2);
                        totalvou += Math.Round(double.Parse(dataGridView1.Rows[g].Cells[8].Value.ToString()), 2);
                        totaldescuen += Math.Round(double.Parse(dataGridView1.Rows[g].Cells[10].Value.ToString()), 2);
                        totalimp += (double.Parse(dataGridView1.Rows[g].Cells[6].Value.ToString()) * 13) / 100;
                    }
                    else
                    {
                        totalsub += Math.Round((double.Parse(dataGridView1.Rows[g].Cells[8].Value.ToString())), 2);
                        //totalimp += Math.Round(imp, 2);
                        totalvou += Math.Round(double.Parse(dataGridView1.Rows[g].Cells[8].Value.ToString()), 2);
                        totaldescuen += Math.Round(double.Parse(dataGridView1.Rows[g].Cells[10].Value.ToString()), 2);
                        totalimp += 0;
                    }
                }

                textBox13.Text = string.Format("{0:N2}", totaldescuen);
                textBox3.Text = string.Format("{0:N2}", totalsub);
                textBox5.Text = string.Format("{0:N2}", totalvou);

                textBox6.Text = string.Format("{0:N2}", totalimp);


                //    dataGridView1.Rows[restadescuento].Cells[8].Value = double.Parse(dataGridView1.Rows[restadescuento].Cells[8].Value.ToString()) - (((double.Parse(dataGridView1.Rows[restadescuento].Cells[8].Value.ToString()) * Int32.Parse(dataGridView1.Rows[restadescuento].Cells[5].Value.ToString()))) / 100);
                //    totalt += double.Parse(dataGridView1.Rows[restadescuento].Cells[8].Value.ToString());
                //    descuentos += double.Parse(dataGridView1.Rows[restadescuento].Cells[10].Value.ToString());
                //    textBox13.Text = string.Format("{0:N2}", descuentos);
                //    if (dataGridView1.Rows[restadescuento].Cells[9].Value.ToString() == "(G)")
                //    {
                //        basesinimp = double.Parse(dataGridView1.Rows[restadescuento].Cells[3].Value.ToString()) / 1.13;
                //        can = Int32.Parse(dataGridView1.Rows[restadescuento].Cells[1].Value.ToString());
                //        totalimp += (((basesinimp * can) - double.Parse(dataGridView1.Rows[restadescuento].Cells[10].Value.ToString())) * 13) / 100;
                //        sub += basesinimp * can;
                //    }
                //    else
                //    {
                //        sub += (double.Parse(dataGridView1.Rows[restadescuento].Cells[3].Value.ToString())) * Int32.Parse(dataGridView1.Rows[restadescuento].Cells[1].Value.ToString());
                //    }

                //}
                //totalt = double.Parse(textBox3.Text) + double.Parse(textBox6.Text);

                //textBox3.Text = string.Format("{0:N2}", (sub - descuentos));
                //textBox5.Text = string.Format("{0:N2}", totalt);
                //textBox6.Text = string.Format("{0:N2}", totalimp);


            }
            catch (Exception err_0010)
            {
                Mensaje.Error(err_0010, "467");

                if (dataGridView1.Rows.Count > 0)
                {
                    textBox5.Text = string.Format("{0:N2}", (totalt - descuentos));
                    textBox6.Text = string.Format("{0:N2}", (totalimp - (totalimp / 1.13)));
                }
                else
                {
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                }


            }

        }
        #endregion
        
       
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            elvuelto = "";
            try
            {

                impri = false;

               if (comboBox1.Text != "" && comboBox4.Text != "" && comboBox3.Text.Trim() != "" && textBox17.Text != "" && textBox16.Text != "" && dataGridView1.Rows.Count > 0)
                {

                    meterFactura();
                    formatodeactura();

                    if (impri)
                    {
                        if (comboBox3.Text == "0" || checkBox1.Checked == true)
                        {
                            for (int imp = 0; imp < Int64.Parse(numericUpDown1.Value.ToString()); imp++)
                            {
                                imprimir();

                            }

                            elvuelto = textBox15.Text;
                            limpiar();
                            elvuelto vp = new elvuelto(this);
                            vp.ShowDialog();
                        }
                        else
                        {
                            elvuelto = textBox15.Text;
                            limpiar();
                            elvuelto vp = new elvuelto(this);
                            vp.ShowDialog();
                        }

                      

                    }
                    else
                    {

                        MessageBox.Show("Por favor vuelva  a intentar imprimir la factura", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }



                }
                else
                {
                    Mensaje_Warning("Parece que no todos los datos estan correctos. \nPor favor verifique que el tipo de pago y toda la informacion este correcta");
                }

               
                timer3.Stop();
            
               
            }
            catch (Exception err_0018)
            {


                MessageBox.Show("Hubo una colisión en la base de datos y se bloqueo el acceso para evitar duplicados. Por favor intente de nuevo "+err_0018.ToString());

                textBox15.BackColor = Color.WhiteSmoke;
              
                timer3.Stop();

            }
        }

       
      
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
            try
            {
                eventos(e);
                textBox14.Text = "0";
                textBox15.Text = "0";
                button3.Enabled = false;
            }
            catch (Exception err_0011)
            {
                Mensaje.Error(err_0011, "716");

               
                    
            }
        }
      

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
         
        }
     
  
        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {

                if (dataGridView1.Rows.Count>0)
                {

                    calcularTotal();

                }
                else
                {

                    textBox3.Text = "0";
                    textBox6.Text = "0";
                    textBox5.Text = "0";
                    textBox12.Text = "0";
                    textBox13.Text = "";
                }
              



            }
            catch (Exception err_0013)
            {
                Mensaje.Error(err_0013, "765");
               

            }
        }
       
        
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


           
          


        }

 
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F7)
            {
                clies cli = new clies(this);
                cli.Show(this);

            }else if (e.KeyCode == Keys.F3)
            {
                if (comboBox1.Text== "Efectivo")
                {
                    comboBox1.Text = "Tarjeta";
                }
                else
                {

                    comboBox1.Text = "Efectivo";
                }

            }
            else if (e.KeyCode == Keys.F6)
            {

                productos pro = new productos(this);
                pro.Show(this);
            }
            else if (e.KeyCode == Keys.F9)
            {

                textBox14.Focus();
            }
            else if (e.KeyCode == Keys.F7)
            {
                clies cl = new clies(this);
                cl.Show(this);

            }
            else if (e.KeyCode == Keys.F8)
            {

                vendedores vd = new vendedores(this);
                vd.Show(this);
            }
            else if (e.KeyCode == Keys.F5)
            {

                inicio2 in2 = new inicio2();
                in2.Show(this);
            }
            else if (e.KeyCode == Keys.F10)
            {
                button3.PerformClick();
               
            }
          

        }
      
        #region logs
        public string eventoLog(EventArgs eve)
        {
            return "El evento : " + eve.ToString() + " ocurrio a las " + DateTime.Now.ToShortTimeString() + " el dia:" +
                DateTime.Now.ToShortDateString() + " por el usuario: " + Environment.UserName.ToString() + "\n";

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            cuenta = dataGridView1.Rows.Count;
        }

        public string errorLog(Exception er)
        {
            return "El error : " + er.Message + "ocurrio a las " + DateTime.Now.ToShortTimeString() + " el dia:" +
                DateTime.Now.ToShortDateString() + " por el usuario: " + Environment.UserName.ToString() + "\n";

        }



        #endregion
        #region metodos impresion

        public void imprimir()
        {

            try
            {



                streamToPrint = new StreamReader
                    ("Factura.txt");

                
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
                Mensaje.Error(err_004, "955");
               

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
                Mensaje.Error(err_005, "1014");
              
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (e.CloseReason==CloseReason.UserClosing)
                    e.Cancel = true;

            }
            catch (Exception err)
            {
                Mensaje.Error(err, "1031");
              
            }
         
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
           

        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
          
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick_2(object sender, EventArgs e)
        {
          
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        public void formatodeactura()
        {

            productos = "";
            formato = "";
            len = "";
            comprobanteelectronico = "";
            try
            {


                if (comboBox3.Text=="0")
                {
                    comprobanteelectronico = "Tiquete Electrónico";
                }
                else
                {
                    comprobanteelectronico = "Factura Electrónica";
                }



                for (int da = 0; da < dataGridView1.Rows.Count; da++)
                {
                    len = dataGridView1.Rows[da].Cells[2].Value.ToString().Trim();
                  

                    if (dataGridView1.Rows[da].Cells[0].Value != null && len.Length >= 25)
                    {

                        productos += "\n      " + dataGridView1.Rows[da].Cells[1].Value.ToString() +
                            "                        ₡" + dataGridView1.Rows[da].Cells[3].Value.ToString() +
                             "\n" + dataGridView1.Rows[da].Cells[2].Value.ToString().Substring(0, 25)  +
                       "\n         ("+dataGridView1.Rows[da].Cells[5].Value.ToString()+ "%)  Descuento =   ₡"+ (string.Format("{0:N2}", decimal.Parse( dataGridView1.Rows[da].Cells[10].Value.ToString())))+"";

                    }
                    else if (dataGridView1.Rows[da].Cells[0].Value != null)
                    {

                        productos += "\n      " + dataGridView1.Rows[da].Cells[1].Value.ToString() +
                             "                        ₡" + dataGridView1.Rows[da].Cells[3].Value.ToString() +
                              "\n" + dataGridView1.Rows[da].Cells[2].Value.ToString()  +
                      "\n         (" + dataGridView1.Rows[da].Cells[5].Value.ToString()+ "%)  Descuento =   ₡" + (string.Format("{0:N2}", decimal.Parse(dataGridView1.Rows[da].Cells[10].Value.ToString())));

                    }

                }




                    formato = ConfigurationManager.AppSettings["cadena2"] +
                   "\n    Fecha:     " + DateTime.Now.ToString() + "\n" +
                    "                      No.Factura:\n" + "             " + consecutivo + "\n" +
                   "                        Clave:\n" + clave.Substring(0, 25) + "\n" +
                       clave.Substring(25) + "\n" +
                   "                Comprobante: " + numeroRecibido + "\n" +
                   "Tipo de Comprobante:\n"+comprobanteelectronico+"\n"+
                   "        Tipo de pago : " + comboBox1.SelectedItem.ToString() + "\n          Facturado por: " +
                   facturando.Text +
                   "\nCliente:\n" + textBox17.Text.Trim() +
                   "\n-----------------------------------------------\n" +
                "Cantidad           Precio\n" + "               Artículo\n" +
               "-----------------------------------------------\n" + productos +
               "\n         (E)=Exento            (G)=Gravado" +
                                              "\nARTICULOS=                      "
                 + dataGridView1.Rows.Count + "\nSUBTOTAL=                      ₡"
                 + textBox3.Text.Trim() + "\nI.V.A.=                               ₡"
                     + textBox6.Text.Trim() + "\nDESCUENTO=                   ₡"
                 + string.Format("{0:N2}", decimal.Parse(textBox13.Text))
                                            + "\nTOTAL=                             ₡"
               + textBox5.Text.Trim() +
               "\n------------------------------MONTO-------------" +
               "\nPAGA CON:                        ₡"
               + string.Format("{0:n0}", decimal.Parse(textBox14.Text.Trim())) +
               "\nVUELTO=                           ₡"
               + textBox15.Text.Trim() +
               "\n           ARTICULOS CON I.V.I." +
               "\n           VENDEDOR : " + textBox16.Text.Trim() +
               "\n----Autorizada mediante resolución--------\n---------N° DGT‐R‐48‐2016 del 07---------\n----------de octubre de 2016.---------------\n----------¡Gracias por su compra!--------------\n********Esperamos servirle de nuevo ************";



                





                //MessageBox.Show(formato);


                facturawr = new StreamWriter("Factura.txt");
                facturawr.WriteLine(formato);
                facturawr.Flush();
                facturawr.Close();






            }
            catch (NullReferenceException ne)
            {

            }
            catch (Exception err_0016)
            {
                MessageBox.Show(err_0016.ToString());
               

            }

        }



        #endregion
        #region consifguracion de mensajes
        public void Mensaje_Warning(string m)
        {
            MessageBox.Show(m,"Mensaje de error",MessageBoxButtons.OK,MessageBoxIcon.Warning);


        }
        public void Mensaje_Error(string m)
        {
            MessageBox.Show(m, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        #endregion
        #region limpiar

        public void limpiar()
        {
       

            try
            {


                while (dataGridView1.Rows.Count>=1)
                {

                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
                }

                 
                    textBox2.Text = "1";
                    textBox3.Text = "0";
                textBox6.Text = "0";
                    textBox5.Text = "0";
                textBox14.Text = "0";
                textBox15.Text = "0";
                textBox12.Text = "0";
                textBox13.Text = "0";
                textBox3.Text = "0";
                descuentos = 0;
                button3.Enabled = false;
                textBox12.Enabled = true;
                textBox1.Visible = true;
                //dataGridView1.ReadOnly = true;
                textBox1.Focus();
                timer2.Stop();
                timer3.Stop();
                textBox1.Text = "";
                comboBox3.Text = "";
                textBox17.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                comboBox2.Text = "";
                textBox16.Text = "";
                //dataGridView1.Enabled = true;
                numericUpDown1.Value = 1;
                textBox12.Enabled = true;
                textBox15.BackColor = Color.WhiteSmoke;
                textBox15.ForeColor = Color.White;
                textBox7.Text = "0";
                //comboBox1.SelectedIndex = -1;
                comboBox2.Focus();
                checkBox1.Checked = false;
                checkBox1.Visible = false;
                button3.Visible = false;
           
                label11.Visible = false;
           
  
                
            }
            catch (Exception err_20)
            {
                Mensaje.Error(err_20, "1419");
               
            }


        }
        #endregion
      
        public void eventos(KeyEventArgs er)
        {
            if (er.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    llenar(textBox1.Text.Trim());


                    //textBox14.Focus();
                    textBox14.BackColor = Color.FromArgb(192, 255, 192);
                    timer2.Interval = 100;
                    timer2.Start();
                    //llenar2(textBox1.Text.Trim());
                }


            }
            else if(er.KeyCode==Keys.F9)
            {
                
              
                timer2.Stop();
                textBox14.Visible = true;
                textBox14.Focus();
            }
            else if (er.KeyCode == Keys.F6)
            {


                productos pos = new productos(this);
                pos.Show();
            }
            else if (er.KeyCode == Keys.F2)
            {
                
            }
           





        }
     
        #region llenar combos
        public DataTable cargarVendedores()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql=new Mysql())
                {

                    mysql.conexion();

                    string query = "SELECT * FROM vendedores";
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

        public DataTable cargaritems()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql=new Mysql())
                {
                    mysql.conexion();

                    string query = "SELECT * FROM items";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();

                }
                   

            }
            catch (Exception   jskjd)
            {
                Mensaje.Error(jskjd, "1509");
             
                    

            }
          
            return dt;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
       
           
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        public DataTable cargarClientes()
        {
            DataTable dt = new DataTable();
            try
            {

                using (var mysql=new Mysql())
                {
                    mysql.conexion();

                    string query = "SELECT * FROM clientes";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();


                }
                    
            }
            catch (Exception klsk)
            {

                Mensaje.Error(klsk, "1569");
               
                    
            }
          
            return dt;
        }
        public DataTable cargarDescuentos()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql=new Mysql())
                {
                    mysql.conexion();

                    string query = "SELECT * FROM descuentos";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();

                }
                    

            }
            catch (Exception lsksk)
            {

                Mensaje.Error(lsksk, "1602");
               
                  
            }
           
            return dt;



        }
        public DataTable cargaitem()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql=new Mysql())
                {

                    mysql.conexion();

                    string query = "SELECT * FROM items where length(CodigoA)>0 and CodigoA<>'0'";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();

                }

                   

            }
            catch (Exception lop)
            {
                Mensaje.Error(lop, "1632");
              
                    


            }
          
            return dt;



        }


        public DataTable cargaitem2()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var mysql = new Mysql())
                {

                    mysql.conexion();

                    string query = "SELECT * FROM items where length(Codigo)>0";
                    MySqlCommand cmd = new MySqlCommand(query, mysql.con);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    adap.Fill(dt);
                    mysql.Dispose();

                }



            }
            catch (Exception lop)
            {
                Mensaje.Error(lop, "1632");




            }

            return dt;



        }



        private void comboBox4_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          


        }

        private void comboBox4_KeyDown_1(object sender, KeyEventArgs e)
        {


         
        }




        #endregion
      
        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return)
                {
                    descuentomayor = textBox12.Text.Trim();

                    if (textBox20.Text=="1")
                    {
                        if (Int32.Parse(textBox12.Text.Trim()) <= Int32.Parse(textBox18.Text) && Int32.Parse(textBox12.Text.Trim()) > 0)
                        {
                            segundoamitad();
                            actualizargridgeneral();
                            calcularTotal();

                            textBox14.Text = "0";
                            textBox15.Text = "0";
                            textBox12.Text = "0";
                            button3.Enabled = false;
                            textBox12.Enabled = false;
                            for (int f = 0; f < dataGridView1.Rows.Count; f++)
                            {
                                dataGridView1.Rows[f].Cells[4].ReadOnly = true;
                            }

                        }

                        else
                        {

                            MessageBox.Show("El monto debe ser un numero entero mayor a 0 y menor a "+textBox18.Text+"");
                            textBox12.Text = "0";
                            dataGridView1.Enabled = true;
                        }
                    }
                    else
                    {
                        if (Int32.Parse(textBox12.Text.Trim()) <= Int32.Parse(textBox19.Text) && Int32.Parse(textBox12.Text.Trim()) > 0)
                        {
                            segundoamitad();
                            actualizargridgeneral();
                            calcularTotal();

                            textBox14.Text = "0";
                            textBox15.Text = "0";
                            textBox12.Text = "0";
                            button3.Enabled = false;
                            textBox12.Enabled = false;
                            for (int f = 0; f < dataGridView1.Rows.Count; f++)
                            {
                                dataGridView1.Rows[f].Cells[4].ReadOnly = true;
                            }

                        }

                        else
                        {

                            MessageBox.Show("El monto debe ser un numero entero mayor a 0 y menor a "+textBox19.Text+"");
                            textBox12.Text = "0";
                            dataGridView1.Enabled = true;
                        }

                    }
                


                }
               

            }
            catch (Exception textBox12_KeyDown)
            {
                Mensaje.Error(textBox12_KeyDown, "1737");
                dataGridView1.Enabled = true;

                textBox12.Text = "0";
               
               

            }
          
            
          
        }



        #region descuentosdatagrid

        public void segundoamitad()
        {

            int entero;
            bool cntb;



            try
            {
                entero = 0;
                cntb = int.TryParse(textBox12.Text.Trim(), out entero);
                if (dataGridView1.Rows.Count > 0 && cntb)
                {

                    //filas = dataGridView1.Rows.Count % 2;

                    for (int x = 0; x < dataGridView1.Rows.Count; x++)
                    {
                        dataGridView1.Rows[x].Cells[5].Value = entero;

                    }
                }



                else
                {

                    MessageBox.Show("El valor digitado no es válido.Debe de agregar productos primero o asegurarse que sea un numero entero el ingresado");
                }





            }
            catch (Exception textBox12_KeyDown)
            {
                Mensaje.Error(textBox12_KeyDown, "1795");


            }
        }

        public void actualizargridgeneral()
        {

            descuentos = 0;
            double baseprice = 0;
            double canti = 0;
            double desc = 0;
            try
            {
                for (int hh = 0; hh < dataGridView1.Rows.Count; hh++)
                {
                    if (dataGridView1.Rows[hh].Cells[9].Value.ToString()=="Impuesto")
                    {
                        descuentosumado = 0;
                        baseprice = double.Parse(dataGridView1.Rows[hh].Cells[3].Value.ToString()) / 1.13;
                        canti = double.Parse(dataGridView1.Rows[hh].Cells[1].Value.ToString());
                        desc = double.Parse(dataGridView1.Rows[hh].Cells[5].Value.ToString());
                        if (double.Parse(dataGridView1.Rows[hh].Cells[1].Value.ToString()) > 1)
                        {
                            dataGridView1.Rows[hh].Cells[10].Value = ((baseprice * canti) * desc) / 100;
                        }

                        else
                        {

                            descuentosumado = (baseprice * desc) / 100;
                            dataGridView1.Rows[hh].Cells[10].Value = descuentosumado;
                        }
                    }
                    else
                    {
                        descuentosumado = 0;
                        baseprice = double.Parse(dataGridView1.Rows[hh].Cells[3].Value.ToString());
                        canti = double.Parse(dataGridView1.Rows[hh].Cells[1].Value.ToString());
                        desc = double.Parse(dataGridView1.Rows[hh].Cells[5].Value.ToString());
                        if (double.Parse(dataGridView1.Rows[hh].Cells[1].Value.ToString()) > 1)
                        {
                            dataGridView1.Rows[hh].Cells[10].Value = ((baseprice * canti) * desc) / 100;
                        }

                        else
                        {

                            descuentosumado = (baseprice * desc) / 100;
                            dataGridView1.Rows[hh].Cells[10].Value = descuentosumado;
                        }
                    }

                    

                    //descuentototal = double.Parse();
                    //dataGridView1.Rows[hh].Cells[8].Value = double.Parse(dataGridView1.Rows[hh].Cells[8].Value.ToString()) - descuentosumado;




                }






            }
            catch (Exception actualizargrid)
            {

                Mensaje.Error(actualizargrid, "1841");

            }
        }


        #endregion

        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource!=null&&e.KeyCode == Keys.Enter&& Int32.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) <= 50&&
                    dataGridView1.CurrentCell==dataGridView1.CurrentRow.Cells[5])
                {

                    actualizargridgeneral();
                    calcularTotal();
                  

                }
                //else if (e.KeyCode == Keys.Delete)
                //{


                //}
                //else if(dataGridView1.CurrentCell != dataGridView1.CurrentRow.Cells[5]&& dataGridView1.DataSource != null)
                //{

                    
                //}
                //else if (dataGridView1.DataSource==null)
                //{

                //}
                //else
                //{

                //    MessageBox.Show("El valor digitado no puede ser mayor a 50");
                //}

            }
            catch (Exception lope)
            {
                Mensaje.Error(lope, "1888");
               

            }

           
         
        }
     
       
        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {

                //textBox14.Text = string.Format("{0:N0}", double.Parse(textBox14.Text.Trim()));

                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Visible = false;
                   
                    textBox12.Enabled = false;
                   
                    if (decimal.Parse(textBox14.Text.Trim()) >= decimal.Parse(textBox5.Text) && decimal.Parse(textBox14.Text.Trim()) > 0)
                    {
                        if (comboBox3.Text!="0")
                        {
                            checkBox1.Visible = true;
                        }

                        button3.Enabled = true;
                        button3.Visible = true;
                        label11.Visible = true;
                        textBox15.BackColor = Color.Black;
                        textBox15.ForeColor = Color.White;

                        timer3.Interval = 100;
                        timer3.Start();
                        textBox15.Text = string.Format("{0:N2}", (decimal.Parse(textBox5.Text.Trim()) - decimal.Parse(textBox14.Text.Trim())));
                        //textBox1.Focus();

                    }
                    else
                    {
                        textBox15.BackColor = Color.WhiteSmoke; textBox15.ForeColor = Color.White;
                        MessageBox.Show("El monto recibido debe ser mayor o igual a el monto total");
                        timer3.Interval = 1000;
                        timer3.Start();
                    }

                    textBox14.Focus();


                }
                else if (e.KeyCode == Keys.F3)
                {



                    textBox7.Focus();






                }


                
            }
            catch (Exception textBox14_KeyDown)
            {

                Mensaje.Error(textBox14_KeyDown, "2022");
                timer3.Stop();
                textBox15.BackColor = Color.WhiteSmoke; textBox15.ForeColor = Color.White;
            }
          
        }
     
 
        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
               
                eventos(e);
              
       

            }
            catch (FormatException fe) { 

            }
            catch (Exception err_0011)
            {
                Mensaje.Error(err_0011, "2054");
              
                   

            }
        }
       
        #region debugoutout metodo
        private void debugOutput(string strDebugText)
        {
            try
            {
                dynamic results = JsonConvert.DeserializeObject<dynamic>(strDebugText);
                dynamic id = results.results;
                //System.Diagnostics.Debug.Write(strDebugText + Environment.NewLine);
                //txtResponse.Text = txtResponse.Text + strDebugText + Environment.NewLine;
                textBox17.Text = "";
                textBox17.Text = id[0].fullname;

                //txtResponse.SelectionStart = txtResponse.TextLength;
                //txtResponse.ScrollToCaret();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message, ToString() + Environment.NewLine);
            }
        }

        public void debug2(string strDebugText2)
        {

            try
            {
                dynamic res = JsonConvert.DeserializeObject<dynamic>(strDebugText2);
                dynamic cod = res.codificacion;
                //System.Diagnostics.Debug.Write(strDebugText2 + Environment.NewLine);
                //textBox17.Text = "";
                //textBox17.Text = id[0].fullname + Environment.NewLine;
                if (res.status == 1)
                {
                    clave = cod.clave;
                    consecutivo = cod.consecutivo;
                }
                else if (res.status==2)
                {
                    MessageBox.Show("Error del ministerio de hacienda. Contacte al administrador del sistema");
                }
                else
                {

                    MessageBox.Show("Resultado no valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                   
               


            }
            catch (RuntimeBinderException re)
            {

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message, ToString() + Environment.NewLine);
            }
        }
        #endregion
     
        private void comboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                using (var rClient= new buscarcedula())
                {
                    rClient.endPoint = "https://apis.gometa.org/cedulas/" + comboBox3.Text.Trim();
                    debugOutput("RESTClient Object created.");

                    string strJSON = string.Empty;

                    strJSON = rClient.makeRequest();

                    debugOutput(strJSON);
                    rClient.Dispose();

                }
                    

              

            }

                       

            
            catch (Exception comboBox3_KeyDown)
            {
                Mensaje.Error(comboBox3_KeyDown, "2117");
              
                   

            }
        }
        
      
        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int cun = 0;
                if (e.KeyCode == Keys.Return)
                {

                    using (var mysql = new Mysql())
                    {

                        mysql.conexion();
                        mysql.cadenasql = "select Nombre from vendedores where Codigo='" + comboBox2.Text.Trim() + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        mysql.lector = mysql.comando.ExecuteReader();
                        while (mysql.lector.Read())
                        {
                            cun += 1;

                        }
                        if (cun > 0)
                        {
                            textBox16.Text = mysql.lector["Nombre"].ToString();

                        }
                        else
                        {
                            MessageBox.Show("El vendedor que busca no existe");
                            textBox16.Text = "";

                        }
                        comboBox3.Focus();
                        mysql.Dispose();
                    }
                }
                else if (e.KeyCode == Keys.F8)
                {

                    vendedores vend = new vendedores(this);
                    vend.Show();

                }
                

                       
               
              
               
            }
            catch (Exception comboBox2_KeyDown)
            {
                Mensaje.Error(comboBox2_KeyDown, "2157");
             
                   

            }
         
        }
       
      
        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode==Keys.Return)
                {
                    using (var rClient = new buscarcedula())
                    {
                        rClient.endPoint = "https://apis.gometa.org/cedulas/" + comboBox3.Text.Trim();
                        debugOutput("RESTClient Object created.");

                        string strJSON = string.Empty;

                        strJSON = rClient.makeRequest();

                        debugOutput(strJSON);
                        rClient.Dispose();

                    }

                }
              



            }




            catch (Exception comboBox3_KeyDown)
            {
                Mensaje.Error(comboBox3_KeyDown, "2117");



            }
        }
     
   
        private void comboBox3_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    textBox17.Text = "";
                    if (comboBox3.Text=="0")
                    {
                        comboBox3.Text = "0";
                        textBox17.Text = "Contado";
                        textBox9.Text = "Contado";
                        textBox10.Text = "Contado@correo.com";
                        textBox1.Focus();
                    }
                    else
                    {
                        rClient.endPoint = "https://apis.gometa.org/cedulas/" + comboBox3.Text.Trim();
                        debugOutput("RESTClient Object created.");

                        string strJSON = string.Empty;

                        strJSON = rClient.makeRequest();

                        debugOutput(strJSON);

                        if (!string.IsNullOrEmpty(textBox17.Text))
                        {
                            using (var mysql=new Mysql())
                            {
                                mysql.conexion();
                                mysql.cadenasql = "SELECT * FROM `clientes` WHERE Cedula='"+comboBox3.Text.Trim()+"'";
                                mysql.comando =new MySqlCommand(mysql.cadenasql,mysql.con);
                                mysql.lector = mysql.comando.ExecuteReader();
                                if (mysql.lector.Read())
                                {
                                    textBox9.Text = mysql.lector["Telefono"].ToString();
                                    textBox10.Text = mysql.lector["Correo"].ToString();
                                    textBox1.Focus();
                                }
                                else
                                {
                                   DialogResult result= MessageBox.Show("Este cliente no existe, desea crearlo?",
                                       "",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                                    if (result == DialogResult.Yes)
                                    {
                                        cedula = comboBox3.Text;
                                        nombrec = textBox17.Text.Trim();
                                        clienform cfm = new clienform(this);
                                        cfm.textBox5.Text = cedula;
                                        cfm.textBox1.Text = nombrec;
                                        this.Visible = false;
                                        cfm.bandera = true;
                                        cfm.Show(this);
                                        //this.Visible = true;
                                        //this.SendToBack();
                                        cfm.Focus();
                                        cfm.textBox2.Focus();
                                        //SendKeys.Send("{%}{Tab}");
                                        //NativeMethods.SetWindowTop(cfm.Handle);

                                    }
                                   
                                  

                                }

                                mysql.Dispose();
                               
                            }

                        }

                    }

                  
                }


               

            }




            catch (Exception comboBox3_KeyDown)
            {
                Mensaje.Error(comboBox3_KeyDown, "2117");



            }
        }
       
      
        private void comboBox2_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                int cun = 0;
                if (e.KeyCode == Keys.Enter)
                {

                    using (var mysql = new Mysql())
                    {

                        mysql.conexion();
                        mysql.cadenasql = "select Nombre from vendedores where Codigo='" + comboBox2.Text.Trim() + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        mysql.lector = mysql.comando.ExecuteReader();
                        while (mysql.lector.Read())
                        {
                            cun += 1;

                        }
                        if (cun > 0)
                        {
                            textBox16.Text = mysql.lector["Nombre"].ToString();

                        }
                        else
                        {
                            MessageBox.Show("El vendedor que busca no existe");
                            textBox16.Text = "";

                        }
                        
                        mysql.Dispose();
                        comboBox3.Focus();
                    }
                }
                else if (e.KeyCode == Keys.F8)
                {

                    vendedores vend = new vendedores(this);
                    vend.Show();

                }




                

            }
            catch (Exception comboBox2_KeyDown)
            {
                MessageBox.Show(comboBox2_KeyDown.ToString());



            }
        }
       
      
        private void comboBox2_Enter(object sender, EventArgs e)
        {
           comboBox2.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            comboBox2.BackColor = Color.WhiteSmoke;
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            textBox16.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            comboBox3.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            textBox17.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            textBox9.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            textBox10.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            textBox12.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            textBox14.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            textBox16.BackColor = Color.WhiteSmoke;
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            comboBox3.BackColor = Color.WhiteSmoke;
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            textBox17.BackColor = Color.WhiteSmoke;
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            textBox9.BackColor = Color.WhiteSmoke;
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            textBox10.BackColor = Color.WhiteSmoke;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.WhiteSmoke;
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            textBox12.BackColor = Color.WhiteSmoke;
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            textBox14.BackColor = Color.WhiteSmoke;
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.WhiteSmoke;
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            limpiar();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.ExitThread();

        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private void timer1_Tick(object sender, EventArgs e)
        {


            label8.Text = DateTime.Now.ToShortTimeString();


        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.itcoint.com/");
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void agregarUnClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //cls.Show();
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {





        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //Form2 f34 = new Form2();
            //f34.Show();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // c.refrescarClientes();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
         
            //MessageBox.Show(cajero);

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
        private void button5_Click_3(object sender, EventArgs e)
        {
            imprimir();
        }
        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (pictureBox6.Visible == true)
            {

                pictureBox6.Visible = false;
            }
            else
            {
                pictureBox6.Visible = true;

            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text=="Tarjeta")
            {
                label10.Visible = true;

                textBox7.Visible = true;
              
                textBox7.Text = "0";
                textBox7.Focus();
            }
            else
            {

                label10.Visible = false;
               
                textBox7.Visible = false;
                textBox7.Text = "0";
                comboBox2.Focus();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Focus();
              
                

            }
            catch (Exception hsjs)
            {

                Mensaje.Error(hsjs, "2254");
              
                  
            }
          
        }

        private void timer2_Tick_3(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (textBox20.Text=="1")
                {
                    if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[4] && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentCell.Value
                  == dataGridView1.CurrentRow.Cells[4].Value && decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) > 0 &&
                  decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) <= Int32.Parse(textBox18.Text))
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[9].Value = (decimal.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString()) * Int32.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                        actualizargridgeneral();
                        calcularTotal();
                    }
                    else if (decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) == 0)
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[9].Value = 0;
                        dataGridView1.CurrentRow.Cells[7].Value = decimal.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()) * decimal.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                        calcularTotal();
                    }
                    else
                    {
                        MessageBox.Show("Los datos no son válidos, verifique los datos ingresados.\n" +
                            "El descuento no puede ser mayor a "+textBox18.Text+" y debe ingresarse como número entero", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dataGridView1.CurrentRow.Cells[4].Value = "0";
                    }
                }
                else
                {
                    if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[4] && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentCell.Value
                  == dataGridView1.CurrentRow.Cells[4].Value && decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) > 0 &&
                  decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) <= Int32.Parse(textBox19.Text))
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[9].Value = (decimal.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString()) * Int32.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                        actualizargridgeneral();
                        calcularTotal();
                    }
                    else if (decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString()) == 0)
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[9].Value = 0;
                        dataGridView1.CurrentRow.Cells[7].Value = decimal.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()) * decimal.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                        calcularTotal();
                    }
                    else
                    {
                        MessageBox.Show("Los datos no son válidos, verifique los datos ingresados.\n" +
                            "El descuento no puede ser mayor a "+textBox19.Text+" y debe ingresarse como número entero", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dataGridView1.CurrentRow.Cells[4].Value = "0";
                    }
                }

              
               

            }
            catch (Exception ehheh)
            {
                Mensaje.Error(ehheh, "803");




            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
          
            label11.Visible = true;
           
            button3.Visible = true;
       
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {

            button3.Visible = false;
      
            label11.Visible = false;
         
        }

        private void button4_Click_3(object sender, EventArgs e)
        {
           




        }

        private void formatoapartado()
        {
            productos = "";
            formato = "";
            len = "";
            try
            {







                for (int da = 0; da < dataGridView1.Rows.Count; da++)
                {
                    len = dataGridView1.Rows[da].Cells[2].Value.ToString().Trim();


                    if (dataGridView1.Rows[da].Cells[0].Value != null && len.Length >= 7)
                    {

                        productos += "\n      " + dataGridView1.Rows[da].Cells[1].Value.ToString() +
                             "          " + dataGridView1.Rows[da].Cells[2].Value.ToString().Substring(0, 7) + "        ₡" +
                       dataGridView1.Rows[da].Cells[3].Value.ToString() + "        " + dataGridView1.Rows[da].Cells[9].Value.ToString() + "\n         (" + dataGridView1.Rows[da].Cells[5].Value.ToString() + "%)  Descuento =   ₡" + (string.Format("{0:N2}", decimal.Parse(dataGridView1.Rows[da].Cells[10].Value.ToString()))) + "";

                    }
                    else if (dataGridView1.Rows[da].Cells[0].Value != null && len.Length < 7)
                    {

                        productos += "\n      " + dataGridView1.Rows[da].Cells[1].Value.ToString() +
                              "          " + dataGridView1.Rows[da].Cells[2].Value.ToString() + "        ₡" +
                       dataGridView1.Rows[da].Cells[3].Value.ToString() + "        " + dataGridView1.Rows[da].Cells[9].Value.ToString() + "\n         (" + dataGridView1.Rows[da].Cells[5].Value.ToString() + "%)  Descuento =   ₡" + (string.Format("{0:N2}", decimal.Parse(dataGridView1.Rows[da].Cells[10].Value.ToString())));

                    }

                }






                formato = ConfigurationManager.AppSettings["cadena2"] +
                   "\n    Fecha:     " + DateTime.Now.ToString() + "\n" +
                       "                      No.Factura:\n" + "            "  +
                   "                        \nClave:\n" +clave+
                   "\nConsecutivo \n"+consecutivo+
                    
                   "                \nComprobante: " + numeroRecibido + "\n" +
                   "        Tipo de pago : " + comboBox1.SelectedItem.ToString() + "\n          Facturado por: " +
                   facturando.Text +
                   "\nCliente: " + textBox17.Text.Trim() +
                "\nCantidad        Artículo        Precio        Imp.\n" +
               "----------------------------------------------------------" + productos +
               "\n         (E)=Exento              (G)=Gravado" +
                                              "\nARTICULOS=                      "
                 + dataGridView1.Rows.Count + "\nSUBTOTAL=                      ₡"
                 + textBox3.Text.Trim() + "\nI.V.A.=                               ₡"
                     + textBox6.Text.Trim() + "\nDESCUENTO=                   ₡"
                 + string.Format("{0:N2}", descuentos)
                                            + "\nTOTAL=                             ₡"
               + textBox5.Text.Trim() +
               "\n------------------------------MONTO-------------" +
               "\nPAGA CON:                        ₡"
               + string.Format("{0:n0}", decimal.Parse(textBox14.Text.Trim())) +
               "\nVUELTO=                           ₡"
               + textBox15.Text.Trim() +
               "\n           ARTICULOS CON I.V.I." +
               "\n           VENDEDOR : " + textBox16.Text.Trim() +
               "\n----Autorizada mediante resolución--------\n---------N° DGT‐R‐48‐2016 del 07---------\n----------de octubre de 2016.---------------\n----------¡Gracias por su compra!--------------\n********Esperamos servirle de nuevo ************";

               




                facturawr = new StreamWriter("Factura.txt");
                facturawr.WriteLine(formato);
                facturawr.Flush();
                facturawr.Close();






            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show(ne.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
            catch (Exception err_00188)
            {
                MessageBox.Show(err_00188.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //contr ctr = new contr();
            //ctr.Show(this);
            Anulaciones anul = new Anulaciones(this);
            anul.Show(this);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(diccionario2.FirstOrDefault(x=>x.Key==textBox1.Text).Value))
            {
                label51.Text = diccionario2.FirstOrDefault(f => f.Key == textBox1.Text).Value;

               
            }
            else 
            {
                if (comboBox5.Items.Count>0)
                {
                    for (int a = 0; a < comboBox5.Items.Count; a++)
                    {
                        comboBox5.SelectedIndex = a;
                        //MessageBox.Show(comboBox5.SelectedValue.ToString());
                        if (comboBox5.SelectedValue.ToString() == textBox1.Text)
                        {
                            label51.Text = diccionario.FirstOrDefault(x => x.Key == comboBox5.SelectedValue.ToString()).Value;
                        }
                    }
                }
               
                
            }
           
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) ||e.KeyChar=='.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        public void dictionary2()
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select  * from items where length(CodigoA)>0 and CodigoA<>'0' GROUP BY CodigoA";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            diccionario2.Add(lee["CodigoA"].ToString(), lee["Descripcion"].ToString());


                        }
                    }

                    mysql.Dispose();

                }
            }
            catch (Exception fre)
            {
                MessageBox.Show(fre.ToString());
            }
        }
        public void dictionary()
        {

            try
            {


                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select  Codigo,Descripcion from items where length(Codigo)>0 GROUP BY Codigo";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            diccionario.Add(lee["Codigo"].ToString(), lee["Descripcion"].ToString());


                        }
                    }

                    mysql.Dispose();

                }



            }
            catch (Exception ji)
            {
                MessageBox.Show(ji.ToString());
            }




        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox3.Text))
            {
                notascredito nota = new notascredito(this);
                nota.Show(this);
            }
            else{
                MessageBox.Show("Es necesario indicar un cliente de referencia,digite un número de cédula y vuelva a intentar","Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);

            }
                   
        }

        private void dataGridView1_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (textBox20.Text == "1")
                {
                    if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[5] && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentCell.Value
                  == dataGridView1.CurrentRow.Cells[5].Value && decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) > 0 &&
                  decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) <= Int64.Parse(textBox18.Text))
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[10].Value = (decimal.Parse(dataGridView1.CurrentRow.Cells[10].Value.ToString()) * Int64.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                        actualizargridgeneral();
                        calcularTotal();
                    }
                    else if (decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) == 0)
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[10].Value = 0;
                        //dataGridView1.CurrentRow.Cells[8].Value = double.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()) * double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                        calcularTotal();
                    }
                    else
                    {
                        MessageBox.Show("Los datos no son válidos, verifique los datos ingresados.\n" +
                            "El descuento no puede ser mayor a " + textBox18.Text + " y debe ingresarse como número entero", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dataGridView1.CurrentRow.Cells[5].Value = "0";
                    }
                }
                else
                {
                    if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[5] && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentCell.Value
                  == dataGridView1.CurrentRow.Cells[5].Value && decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) > 0 &&
                  double.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) <= Int64.Parse(textBox19.Text))
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[10].Value = (decimal.Parse(dataGridView1.CurrentRow.Cells[10].Value.ToString()) * Int64.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                        actualizargridgeneral();
                        calcularTotal();
                    }
                    else if (decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString()) == 0)
                    {
                        textBox12.Enabled = false;
                        dataGridView1.CurrentRow.Cells[10].Value = 0;
                        //dataGridView1.CurrentRow.Cells[8].Value = double.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()) * double.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                        //actualizargridgeneral();
                        calcularTotal();
                    }
                    else
                    {
                        MessageBox.Show("Los datos no son válidos, verifique los datos ingresados.\n" +
                            "El descuento no puede ser mayor a " + textBox19.Text + " y debe ingresarse como número entero", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dataGridView1.CurrentRow.Cells[5].Value = "0";
                    }
                }




            }
            catch (Exception ehheh)
            {
                Mensaje.Error(ehheh, "803");




            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowsRemoved_1(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            int conta = 0;
            try
            {
                for (int g = 0; g < dataGridView1.Rows.Count; g++)
                {
                    conta++;
                }

                if (conta > 0)
                {

                    calcularTotal();
                }
                else
                {

                    textBox3.Text = "0";
                    textBox6.Text = "0";
                    textBox5.Text = "0";
                    textBox12.Text = "0";
                    textBox13.Text = "";
                }




            }
            catch (Exception err_0013)
            {
                Mensaje.Error(err_0013, "765");


            }
        }

        private void dataGridView1_RowsAdded_1(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int conta = 0;
            try
            {
                for (int g = 0; g < dataGridView1.Rows.Count; g++)
                {
                    conta++;
                }

                if (conta > 0)
                {

                    calcularTotal();
                }
                




            }
            catch (Exception err_0013)
            {
                Mensaje.Error(err_0013, "765");


            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar) ||char.IsSymbol(e.KeyChar) ||char.IsSeparator(e.KeyChar))
            {
                e.Handled = true;

            }
            else
            {
                e.Handled = false ;
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;

            }
            else
            {
                e.Handled = true;
            }


        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           
        }

        private void dataGridView1_KeyDown_2(object sender, KeyEventArgs e)
        {
            
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex==5)
            {
                TextBox txb = e.Control as TextBox;

                txb.KeyPress -= new KeyPressEventHandler(dataGridView1_KeyPress);
                txb.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
              
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
    }
}

using Microsoft.CSharp.RuntimeBinder;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using POS.Devoluciones;
using POS.HACIENDA;
using POS.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class Anulaciones : Form
    {
        string naturaleza;
        decimal desctotalfe;
        string cedula = "";
        string nombre = "";
        string cajero = "";
        List<DETAIL> detalles;
        string eltipo;
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
        string json;
        ENVIO enviarfactura;
        static Respuesta respuesta;
        decimal price;
        decimal canti;
        Form1 ef;
        public Anulaciones(Form1 efe1)
        {
            InitializeComponent();detalles = new List<DETAIL>();enviarfactura = new ENVIO();
            ef = efe1;
            respuesta = new Respuesta();
        }
      
        public void cargarfacs()
        {

            //try
            //{
            //    using (var mysql = new Mysql())
            //    {
            //        mysql.conexion();
            //        DataTable dtDatos = new DataTable();
            //        mysql.cadenasql = "select * from factura";
            //        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(mysql.cadenasql, mysql.con);
            //        mdaDatos.Fill(dtDatos);
            //        dataGridView1.DataSource = dtDatos;
            //        mysql.Dispose();

            //    }

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        public void cargarfacs2()
        {

            //try
            //{
            //    using (var mysql = new Mysql())
            //    {
            //        mysql.conexion();
            //        DataTable dtDatos = new DataTable();
            //        mysql.cadenasql = "select * from sales";
            //        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(mysql.cadenasql, mysql.con);
            //        mdaDatos.Fill(dtDatos);
                    
            //        mysql.Dispose();

            //    }

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();

                    //mysql.cadenasql = "update sales set Total='0' where Numero='" + dataGridView2.CurrentRow.Cells[0].Value + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    mysql.Dispose();


                }
                cargarfacs2(); MessageBox.Show("La factura ha sido anulada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception es)
            {
                MessageBox.Show("Hubo un error a conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Anulaciones_Load(object sender, EventArgs e)
        {
           
        }



        public void buscarlineas()
        {
            devolucion dev = new devolucion(this);

            try
            {
                dev.Show(this);
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "SELECT  detalles.Precio,detalles.Cantidad,detalles.Item,detalles.Descuento,items.Descripcion FROM detalles,items WHERE (detalles.Item=items.Barcode) AND detalles.NumeroFactura='" + dataGridView1.CurrentRow.Cells[0].Value + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {

                            dev.dataGridView1.Rows.Add(lee["Item"].ToString(), lee["Descripcion"].ToString(), lee["Cantidad"].ToString(), lee["Precio"].ToString(),lee["Descuento"].ToString(), "0", "0");
                        }
                    }
                    dev.textBox2.Text = nombre;
                    dev.textBox3.Text = cedula;
                    dev.textBox4.Text = cajero;
                    mysql.Dispose();
                }


            }
            catch (NullReferenceException nui)
            {

            }
            catch (Exception eff)
            {
                MessageBox.Show(eff.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            naturaleza = "";
            impuestofe = 0;
            unitprice = 0;
            totalamountfe = 0;
            discountfe = 0;
            totallineamountfe = 0;
            subtotalfe = 0;
            totaltaxedgoodsfe = 0;
            totaltaxedfe = 0;
            totalexcemptfe = 0;
            totalexcemptgoodsfe = 0;
            totalnetsalesfe = 0;
            totalsalesfe = 0;
            impuestototal = 0;
          
            int conta = 0;
            price = 0;
            canti = 0;
            desctotalfe = 0;
            int admi = 0;
            try
            {
                //MessageBox.Show(ef.textBox4.Text);
                using (var mysql3= new Mysql())
                {
                    mysql3.conexion();
               
                    mysql3.cadenasql = "select Admin from registro where Codigo='"+ef.textBox4.Text+"'";
                    mysql3.comando = new MySqlCommand(mysql3.cadenasql, mysql3.con);
                    mysql3.comando.ExecuteNonQuery();
                    using (MySqlDataReader leec=mysql3.comando.ExecuteReader())
                    {
                        while (leec.Read())
                        {
                            admi = Int32.Parse(leec["Admin"].ToString());
                        }

                     
                    }
                        mysql3.Dispose();
                }

                switch (admi)
                {
                    case 0:
                        MessageBox.Show("No tiene suficiente permiso para anular facturas.\nContacte un administrador ", "No tiene permiso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case 1:
                        if (dataGridView1.DataSource!=null)
                        {

                                using (var mysql = new Mysql())
                                {
                                    mysql.conexion();

                                    mysql.cadenasql = "update factura set Total='0' where Numero='" + dataGridView1.CurrentRow.Cells[0].Value + "'";
                                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                                    mysql.comando.ExecuteNonQuery();

                                    mysql.Dispose();


                                }


                                DialogResult result = MessageBox.Show("Desea crear una nota de crédito para esta factura?", "Error",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {




                                    FE f = new FE()
                                    {
                                        //CompanyAPI = "45e7a866-381b-4a1a-81c7-47cdfee2f620"
                                        //CompanyAPI = "5419ed5c-7c67-49bf-9009-a03dfcacbe27"
                                        CompanyAPI = "749ad71a-8e08-48b4-a5c1-6a5de55b677f"

                                    };



                                    f.Key = new KEY()
                                    {
                                        Branch = "001",
                                        Terminal = "001",
                                        Type = "03",
                                        Voucher = string.Concat( "14", (Int64.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString())).ToString()),
                                   
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

                                    //hacer consultas de receiver


                                    if (dataGridView1.CurrentRow.Cells[2].Value.ToString() == "0")
                                    {
                                        f.Receiver = new RECEIVER()
                                        {
                                            Name = "Contado",
                                            Identification = new IDENTIFICATION
                                            {
                                                Type = "01",
                                                Number = "000000000"

                                            },
                                            Email = "Contado@correo.com"

                                        };

                                        eltipo = "04";

                                    }
                                    else
                                    {
                                        using (var mysql = new Mysql())
                                        {
                                            mysql.conexion();

                                            mysql.cadenasql = "select Cedula,Nombre,Correo from clientes where Cedula='" + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "'";
                                            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                                            mysql.comando.ExecuteNonQuery();
                                            mysql.lector = mysql.comando.ExecuteReader();
                                            while (mysql.lector.Read())
                                            {
                                                f.Receiver = new RECEIVER()
                                                {
                                                    Name = mysql.lector["Nombre"].ToString(),
                                                    Identification = new IDENTIFICATION
                                                    {
                                                        Type = "01",
                                                        Number = dataGridView1.CurrentRow.Cells[2].Value.ToString()

                                                    },
                                                    Email = mysql.lector["Correo"].ToString()

                                                };

                                            }
                                            eltipo = "01";
                                            mysql.Dispose();
                                        }

                                    }




                                    detalles.Clear();
                                    f.Detail = null;


                                    using (var mysql = new Mysql())
                                    {
                                        mysql.conexion();

                                        mysql.cadenasql = "select * from detalles where NumeroFactura='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                                        mysql.comando.ExecuteNonQuery();
                                        mysql.lector = mysql.comando.ExecuteReader();
                                        while (mysql.lector.Read())
                                        {

                                            if (mysql.lector["Impuesto"].ToString() == "(G)")
                                            {

                                                price = Convert.ToDecimal(mysql.lector["Precio"].ToString());
                                                canti = Convert.ToDecimal(mysql.lector["Cantidad"].ToString());
                                                discountfe = Convert.ToDecimal(mysql.lector["Descuento"].ToString());
                                                unitprice = decimal.Round((price / 1.13m), 2, MidpointRounding.AwayFromZero);
                                                impuestofe = decimal.Round((((unitprice * canti) - discountfe) * 13) / 100, 2, MidpointRounding.AwayFromZero);
                                                totaltaxedgoodsfe += decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                totalsalesfe += decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe)), 2, MidpointRounding.AwayFromZero);
                                                totalnetsalesfe += decimal.Round(((totalsalesfe - discountfe)), 2, MidpointRounding.AwayFromZero);
                                                totaltaxedfe += decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                impuestototal += decimal.Round(impuestofe, 2, MidpointRounding.AwayFromZero);


                                                losimpuestos = new TAX()
                                                {
                                                    Code = "01",
                                                    Rate = 13.0m,
                                                    Amount = decimal.Round(impuestofe, 2, MidpointRounding.AwayFromZero),
                                                    Exoneration = null
                                                };

                                                totalamountfe = decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                desctotalfe += decimal.Round(discountfe, 2, MidpointRounding.AwayFromZero);
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
                                                    Number = conta += 1,
                                                    Code = new CODE
                                                    {
                                                        Type = "04",
                                                        Code = mysql.lector["Item"].ToString()
                                                    },
                                                    Tax = new List<TAX>()
                                        {
                                            losimpuestos
                                        },

                                                    Quantity = decimal.Parse(mysql.lector["Cantidad"].ToString()),
                                                    UnitOfMeasure = "kg",
                                                    CommercialUnitOfMeasure = null,
                                                    Detail = dataGridView1.CurrentRow.Cells[2].Value.ToString(),
                                                    UnitPrice = unitprice,
                                                    TotalAmount = totalamountfe,
                                                    Discount = decimal.Round(discountfe, 2, MidpointRounding.AwayFromZero),
                                                    NatureOfDiscount = naturaleza,
                                                    SubTotal = subtotalfe,
                                                    TotalLineAmount = totallineamountfe

                                                };
                                                detalles.Add(detail);
                                            }
                                            else
                                            {
                                                price = Convert.ToDecimal(mysql.lector["Precio"].ToString());
                                                canti = Convert.ToDecimal(mysql.lector["Cantidad"].ToString());
                                                discountfe = Convert.ToDecimal(mysql.lector["Descuento"].ToString());
                                                impuestofe = 0;

                                                unitprice = price;
                                                totaltaxedgoodsfe += 0;
                                                totaltaxedfe += 0;


                                                totalamountfe = decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                desctotalfe += decimal.Round(discountfe, 2, MidpointRounding.AwayFromZero);
                                                totalexcemptgoodsfe += decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                totalexcemptfe += decimal.Round(((unitprice * canti)), 2, MidpointRounding.AwayFromZero);
                                                totalsalesfe += decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe)), 2, MidpointRounding.AwayFromZero);
                                                totalnetsalesfe += decimal.Round(((totalsalesfe - discountfe)), 2, MidpointRounding.AwayFromZero);
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
                                                    Number = conta += 1,
                                                    Code = new CODE
                                                    {
                                                        Type = "04",
                                                        Code = mysql.lector["Item"].ToString()
                                                    },


                                                    Quantity = decimal.Parse(mysql.lector["Cantidad"].ToString()),
                                                    UnitOfMeasure = "kg",
                                                    CommercialUnitOfMeasure = null,
                                                    Detail = dataGridView1.CurrentRow.Cells[2].Value.ToString(),
                                                    UnitPrice = unitprice,
                                                    TotalAmount = totalamountfe,
                                                    Discount = decimal.Round(discountfe, 2, MidpointRounding.AwayFromZero),
                                                    NatureOfDiscount = naturaleza,
                                                    SubTotal = subtotalfe,
                                                    TotalLineAmount = totallineamountfe

                                                };
                                                detalles.Add(detail);
                                            }

                                        }
                                        f.Detail = detalles;
                                        mysql.Dispose();

                                    }



                                    using (var mysql = new Mysql())
                                    {
                                        mysql.conexion();
                                        mysql.cadenasql = "select Clave from hacienda where Comprobante='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                                        mysql.comando.ExecuteNonQuery();
                                        mysql.lector = mysql.comando.ExecuteReader();

                                        while (mysql.lector.Read())
                                        {

                                            REFERENCE refe = new REFERENCE()
                                            {
                                                DocumentType = eltipo,
                                                DocumentNumber = mysql.lector["Clave"].ToString(),
                                                Code = "01",
                                                Reason = "El cliente quizo cambiar o devolver algunos productos"
                                            };


                                            f.Reference = new List<REFERENCE>()
                            {
                                refe
                            };

                                        }
                                        mysql.Dispose();
                                    }





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
                                        TotalDiscounts = decimal.Round(desctotalfe, 2, MidpointRounding.AwayFromZero),
                                        TotalNetSale = decimal.Round(((totaltaxedgoodsfe + totalexcemptgoodsfe) - desctotalfe), 2, MidpointRounding.AwayFromZero),
                                        TotalTaxes = decimal.Round(impuestototal, 2, MidpointRounding.AwayFromZero),
                                        TotalVoucher = decimal.Round((totaltaxedgoodsfe + totalexcemptgoodsfe + impuestototal) - desctotalfe, 2, MidpointRounding.AwayFromZero)
                                    };

                                    json = JsonConvert.SerializeObject(f, Newtonsoft.Json.Formatting.Indented);

                                 
                                    string strJSON = string.Empty;



                                    SendInvoicesAGC(f);



                                }


                                MessageBox.Show("La factura ha sido anulada", "Solicitud procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Visible = false;

                           






                        }
                        else
                        {
                            MessageBox.Show("Faltan datos", "Faltan Datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                       
                        break;
                    default:
                        MessageBox.Show("Un error inesperado ha ocurrido.\nContacte un administrador", "Error desconocido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }



                    
            }

            catch (NullReferenceException nue)
            {
                MessageBox.Show("Faltan datos" ,"Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }


            catch (Exception es)
            {
                MessageBox.Show("Hubo un error a conectar a la base de datos" + es.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

    
        }

        public static Respuesta SendInvoicesAGC(FE factura)

        {



            //string urlAPI = "http://104.43.243.208:8080/api/invoice/";
            string urlAPI = "http://104.43.136.13:8080/api/invoice/";

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



                //Logs.SaveAnswer(result, company,factura.numero);



                respuesta = JsonConvert.DeserializeObject<Respuesta>(result);

                //MessageBox.Show(respuesta.codificacion.clave+"...."+respuesta.codificacion.consecutivo);
                return respuesta;

            }

        }

        private void debug2(string strDebugText2)
        {
            try
            {
                dynamic res = JsonConvert.DeserializeObject<dynamic>(strDebugText2);
                dynamic cod = res.codificacion;
              
                if (res.status == 1 || res.status == 2)
                {
                    //MessageBox.Show(res.message);
                    //clave = cod.clave;
                    //consecutivo = cod.consecutivo;
                    //impri = true;
                }
                else
                {

                    MessageBox.Show("Resultado no válido por la siguiente razón \n" + res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //impri = false;
                }




            }
            catch (RuntimeBinderException re)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //try
            //{
                
            //    using (var mysql = new Mysql())
            //    {
            //        mysql.conexion();
            //        DataTable dtDatos = new DataTable();
            //        DataTable dtDatos2 = new DataTable();
            //        string query = "select * from factura where Numero like '" + textBox1.Text + "%'";
            //        string query2 = "select * from sales where Numero like '" + textBox1.Text + "%'";
            //        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(query, mysql.con);
            //        MySqlDataAdapter mdaDatos2 = new MySqlDataAdapter(query2, mysql.con);
            //        mdaDatos.Fill(dtDatos);
            //        mdaDatos2.Fill(dtDatos2);
            //        dataGridView1.DataSource = dtDatos;
                   
            //        mysql.Dispose();
   
                  

            //    }


            //}
            //catch (Exception euju)
            //{

            //    Mensaje.Error(euju, "71");


            //}
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count>0)
            {
                this.Visible = false;
                buscarcliente();
                buscarcajero();
                buscarlineas();
            } else {
                MessageBox.Show("Faltan datos", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
         
        }

        private string buscarcajero()
        {
            try
            {
                if (dataGridView1.Rows.Count>0)
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        mysql.cadenasql = "select registro.Nombre,factura.CodigoCajero from registro,factura where factura.CodigoCajero='" + dataGridView1.CurrentRow.Cells[4].Value + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                        {
                            if (lee.Read())
                            {

                                cajero = lee["Nombre"].ToString();
                            }

                        }
                        mysql.Dispose();

                    }

                }
                else
                {
                    MessageBox.Show("Faltan datos","Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }

              

            }
            catch (NullReferenceException nuew)
            {

            }
            catch (Exception fr)
            {
                MessageBox.Show(fr.ToString());
            }
            return cajero;
        }

        private string buscarcliente()
        { 
            try
            {
                using (var mysql=new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select Cedula, Nombre from clientes where Cedula='"+dataGridView1.CurrentRow.Cells[2].Value+"'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql,mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee=mysql.comando.ExecuteReader())
                    {
                        if (lee.Read())
                        {
                            cedula = lee["Cedula"].ToString();
                            nombre = lee["Nombre"].ToString();
                        }
                    
                    }
                        mysql.Dispose();

                }
            }
            catch (NullReferenceException nuu)
            {

            }
            catch (Exception efe)
            {
                MessageBox.Show(efe.ToString());

            }
            return cedula + nombre;
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    try
                    {
                        //while (dataGridView1.Rows.Count>=0)
                        //{
                        //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
                        //}
                        using (var mysql = new Mysql())
                        {
                            mysql.conexion();
                            DataTable dtDatos = new DataTable();

                            string query = "select * from factura where Numero='" + textBox1.Text + "'";

                            MySqlDataAdapter mdaDatos = new MySqlDataAdapter(query, mysql.con);

                            mdaDatos.Fill(dtDatos);
                            if (dtDatos.Rows.Count>0)
                            {
                                dataGridView1.DataSource = dtDatos;
                             
                            }
                            else
                            {
                                DataTable dtDatos2 = new DataTable();

                                string query2 = "select * from sales where Numero='" + textBox1.Text + "'";

                                MySqlDataAdapter mdaDatos2 = new MySqlDataAdapter(query2, mysql.con);

                                mdaDatos2.Fill(dtDatos2);
                              
                                    dataGridView1.DataSource = dtDatos2;
   

                            }
                            

                            mysql.Dispose();



                        }
                    }
                    catch (Exception efds)
                    {
                        MessageBox.Show(efds.ToString().Substring(5,10), "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                   
                }
                else
                {
                    MessageBox.Show("Faltan datos", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = new DataTable();
            try{
                if (e.KeyCode==Keys.Enter)
                {

                    using (var mysql = new Mysql())
                    {
                        
                        mysql.conexion();
                        mysql.cadenasql = "select * from factura where Numero='" + textBox1.Text + "'";
                        MySqlDataAdapter adapt = new MySqlDataAdapter(mysql.cadenasql,mysql.con);
                        adapt.Fill(dt);
                        dataGridView1.DataSource = dt;
                        mysql.Dispose();
                       
                    }
                }

              
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}

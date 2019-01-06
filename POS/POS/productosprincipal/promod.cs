using MySql.Data.MySqlClient;
using POS.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.productosprincipal
{
    public partial class promod : Form
    {
        public promod()
        {
            InitializeComponent();
        }

        private void promod_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            impuesto.SelectedIndex = 0;
            cargar();
        }

        public void cargar()
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                    string query = "select Codigo,CodigoA from items where length(Codigo)>0 or length(CodigoA)>0";
                    MySqlDataAdapter mdaDatos = new MySqlDataAdapter(query, mysql.con);
                    mdaDatos.Fill(dtDatos);
                    dataGridView1.DataSource = dtDatos;
                    mysql.Dispose();


                }


            }
            catch (Exception hju)
            {
                Mensaje.Error(hju, "42");


            }

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                    string query = "select Codigo from items where  Codigo like '" + barcode.Text + "%' and length(Codigo)>0";
                    MySqlDataAdapter mdaDatos = new MySqlDataAdapter(query, mysql.con);
                    mdaDatos.Fill(dtDatos);
                    dataGridView1.DataSource = dtDatos;
                    mysql.Dispose();

                }


            }
            catch (Exception euju)
            {

                Mensaje.Error(euju, "71");


            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            buscarprods();
           
            


        }

        public void buscarprods()
        {
            try
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        if (string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                        {
                            mysql.cadenasql = "select * from items where CodigoA='" + dataGridView1.CurrentRow.Cells[1].Value.ToString() + "'";
                        }
                        else if (string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                        {
                            mysql.cadenasql = "select * from items where Codigo='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                        }
                        else
                        {
                            mysql.cadenasql = "select * from items where Codigo='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "' and CodigoA='" + dataGridView1.CurrentRow.Cells[1].Value.ToString() + "'";
                        }
                       
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.lector = mysql.comando.ExecuteReader();
                        while (mysql.lector.Read())
                        {
                            descripcion.Text = mysql.lector["Descripcion"].ToString();
                            barcode.Text = mysql.lector["Codigo"].ToString();
                            impuesto.Text = mysql.lector["Impuesto"].ToString();
                            textBox1.Text = mysql.lector["Cantidad"].ToString();
                            textBox2.Text = mysql.lector["Precio"].ToString();
                            textBox3.Text = mysql.lector["Categoria"].ToString();
                            textBox4.Text = mysql.lector["CodigoA"].ToString();
                            comboBox1.Text = mysql.lector["Identificador"].ToString();

                        }
                        mysql.Dispose();
                        button1.Enabled = true;
                        descripcion.Focus();
                    }
                }



            }
            catch (Exception excep)
            {

                Mensaje.Error(excep, "102");
            }
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode==Keys.Up ||e.KeyCode==Keys.Down)
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        mysql.cadenasql = "select * from items where Codigo='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.lector = mysql.comando.ExecuteReader();
                        while (mysql.lector.Read())
                        {
                            descripcion.Text = mysql.lector["Descripcion"].ToString();
                           
                            impuesto.Text = mysql.lector["Impuesto"].ToString();
                           

                        }
                        mysql.Dispose();
                    }

                }
              


            }
            catch (Exception excep)
            {

                Mensaje.Error(excep, "102");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Columns.Count==2)
                {
                  
                    if ((!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString())
                    || !string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[1].Value.ToString())) &&
                    !string.IsNullOrEmpty(descripcion.Text) && !string.IsNullOrEmpty(impuesto.Text) &&
                    !string.IsNullOrEmpty(textBox1.Text) &&
                    !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) &&
                    !string.IsNullOrEmpty(comboBox1.Text)
                   )
                    {
                        using (var mysql = new Mysql())
                        {
                            mysql.conexion();
                            if (!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString())&&!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                            {
                                mysql.cadenasql = "update items set Descripcion='" + descripcion.Text.Trim() + "'" +
                               ",Impuesto='" + impuesto.Text.Trim() + "',Cantidad='" + double.Parse(textBox1.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Precio='" + double.Parse(textBox2.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Categoria='" + textBox3.Text + "',Identificador='" + comboBox1.Text + "'  where Codigo='" + dataGridView1.CurrentRow.Cells[0].Value + "' AND CodigoA='" + dataGridView1.CurrentRow.Cells[1].Value + "'";

                            }
                            else if (string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                            {
                                mysql.cadenasql = "update items set Descripcion='" + descripcion.Text.Trim() + "'" +
                               ",Impuesto='" + impuesto.Text.Trim() + "',Cantidad='" + double.Parse(textBox1.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Precio='" + double.Parse(textBox2.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Categoria='" + textBox3.Text + "',Identificador='" + comboBox1.Text + "'  where  CodigoA='" + dataGridView1.CurrentRow.Cells[1].Value + "'";

                            }
                            else if (string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                            {
                                mysql.cadenasql = "update items set Descripcion='" + descripcion.Text.Trim() + "'" +
                               ",Impuesto='" + impuesto.Text.Trim() + "',Cantidad='" + double.Parse(textBox1.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Precio='" + double.Parse(textBox2.Text).ToString("0.00", CultureInfo.InvariantCulture) + "',Categoria='" + textBox3.Text + "',Identificador='" + comboBox1.Text + "'  where Codigo='" + dataGridView1.CurrentRow.Cells[0].Value + "'";
                            }
                            mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                            mysql.comando.ExecuteNonQuery();
                            mysql.Dispose();
                            MessageBox.Show("Solicitud procesada correctamente", "Acción realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button1.Enabled = false;
                            barcode.Focus();

                        }

                    }
                    else
                    {
                        MessageBox.Show("Faltan datos", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Faltan datos", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                



            }
            catch (Exception excep)
            {

                Mensaje.Error(excep, "102");
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                    string query = "select CodigoA from items where CodigoA like '" + textBox4.Text + "%' and length(CodigoA)>0 and CodigoA<>'0'";
                    MySqlDataAdapter mdaDatos = new MySqlDataAdapter(query, mysql.con);
                    mdaDatos.Fill(dtDatos);
                    dataGridView1.DataSource = dtDatos;
                    mysql.Dispose();

                }


            }
            catch (Exception euju)
            {

                Mensaje.Error(euju, "71");


            }
        }

        private void barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) ||char.IsLetter(e.KeyChar) ||char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar))
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
            if (char.IsNumber(e.KeyChar)  || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(39) || e.KeyChar == '/')
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void descripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(39) || e.KeyChar == '/')
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}

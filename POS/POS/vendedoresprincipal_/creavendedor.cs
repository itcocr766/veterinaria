using POS.clientesprincipal;
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

namespace POS.vendedoresprincipal_
{
    public partial class creavendedor : Form
    {
        public creavendedor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && richTextBox1.Text != "")
            {
                guardarvendedor();
                

                textBox1.Text = "";
                textBox3.Text = "";
                textBox2.Text = "";
                richTextBox1.Text = "";
            }
            else
            {
                Errores.war();
            }
        }

       

        private void guardarvendedor()
        {
            int codigo = 0;
            try
            {

                using (var mysql = new Mysql())
                {

                    mysql.conexion2();
                    mysql.cadenasql = "select count(*) from vendedores";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    //mysql.lector = mysql.comando.ExecuteReader();
                    using (MySqlDataReader lee = mysql.comando.ExecuteReader())
                    {
                        if (lee.Read())
                        {
                            codigo = Int32.Parse(lee["count(*)"].ToString()) + 1;

                        }
                        else
                        {
                            codigo = 1;

                        }

                    }
                       

                   
                    mysql.cadenasql = "insert into vendedores(Codigo,Nombre,Telefono,Correo,Direccion)values('"+codigo+"','" + textBox1.Text.ToUpper().Trim() + "','" + textBox3.Text.ToUpper().Trim() + "','" + textBox2.Text.ToUpper().Trim() + "','" + richTextBox1.Text + "')";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                  



                 

                    mysql.rol();
                    mysql.Dispose();
                    Errores.inf();


                }
               
                   
              
                   

            }
            catch (Exception ef)
            {
                MessageBox.Show(ef.ToString());
                //Errores.err();
            }
        }

        private void creavendedor_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Focus();
                //cargarconsecutivo();

            }
            catch (Exception uiy)
            {

                Errores.err();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus();

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();

            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(39) || e.KeyChar == '/' ||char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar== Convert.ToChar(39) ||e.KeyChar=='/')
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

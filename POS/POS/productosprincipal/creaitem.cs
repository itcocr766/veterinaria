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
using MySql.Data.MySqlClient;
using POS.Modelo;
namespace POS.productosprincipal
{
    public partial class creaitem : Form
    {
        public creaitem()
        {
            InitializeComponent();
        }

        private void creaitem_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            promod prm = new promod();
            prm.Show(this);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            insertar();
            
        }

        private void limpiar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            richTextBox1.Text = "";
        }

        public void insertar()
        {
            try
            {
                if ((!string.IsNullOrEmpty(textBox1.Text) || !string.IsNullOrEmpty(textBox2.Text))&&


                    !string.IsNullOrEmpty(richTextBox1.Text) && !string.IsNullOrEmpty(comboBox1.Text)&&
                    !string.IsNullOrEmpty(textBox3.Text)&& !string.IsNullOrEmpty(textBox4.Text)&&
                    !string.IsNullOrEmpty(textBox5.Text)&& !string.IsNullOrEmpty(comboBox2.Text))
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        mysql.cadenasql = "INSERT INTO `items`(`Codigo`, `CodigoA`, `Descripcion`, `Precio`, `Cantidad`, `Categoria`, `Impuesto`, `Identificador`) VALUES ('"+textBox2.Text+"','"+textBox1.Text+"','"+richTextBox1.Text+"','"+double.Parse(textBox3.Text).ToString("0.00",CultureInfo.InvariantCulture)+"','"+ double.Parse(textBox4.Text).ToString("0.00", CultureInfo.InvariantCulture) + "','"+textBox5.Text+"','"+comboBox1.Text+"','"+comboBox2.Text+"')";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        mysql.Dispose();
                        MessageBox.Show("La solicitud se proceso correctamente", "Acción realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }


                }
                else
                {

                    MessageBox.Show("Falta información","Faltan datos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }


                textBox2.Focus();
            }
            catch (MySqlException myse)
            {
                MessageBox.Show("Parece que algo falló al intentar guardar en la base de datos.\n" +
                    "Por favor verifique que no este ingresando valores repetidos o existentes o comuniquese con el administrador del sistema","Datos incorrectos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            catch (Exception exec)
            {

                Mensaje.Error(exec,"67");

            }
           
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                comboBox1.Focus();

            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                richTextBox1.Focus();

            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();

            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                richTextBox1.Focus();

            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) ||char.IsControl(e.KeyChar) ||e.KeyChar=='.' )
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
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
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
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox4_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
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

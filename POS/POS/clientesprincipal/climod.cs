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

namespace POS.clientesprincipal
{
    public partial class climod : Form
    {
       
        public climod()
        {
            InitializeComponent();
        }
       
        private void climod_Load(object sender, EventArgs e)
        {
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
                    string query = "select Cedula from clientes where Cedula<>'0'";
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                    string query = "select * from clientes where Cedula like '" + cedula.Text + "%'";
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
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select * from clientes where Cedula='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.lector = mysql.comando.ExecuteReader();
                    while (mysql.lector.Read())
                    {
                        nombre.Text = mysql.lector["Nombre"].ToString();
                        telefono.Text = mysql.lector["Telefono"].ToString();
                        correo.Text = mysql.lector["Correo"].ToString();
                        direccion.Text = mysql.lector["Direccion"].ToString();
                        

                    }
                    mysql.Dispose();
                    button1.Enabled = true;
                    direccion.Focus();
                }


            }
            catch (Exception excep)
            {

                Mensaje.Error(excep, "102");
            }
        }

        private void barcode_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    DataTable dtDatos = new DataTable();
                    string query = "select Cedula from clientes where Cedula like '" + cedula.Text + "%'";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString()) &&
                    !string.IsNullOrEmpty(telefono.Text) && !string.IsNullOrEmpty(correo.Text)&& !string.IsNullOrEmpty(direccion.Text))
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        mysql.cadenasql = "update clientes set Telefono='" + telefono.Text.Trim() + "'" +
                            ",Correo='" + correo.Text.Trim() + "',Direccion='" + direccion.Text.Trim() +
                            "' where Cedula='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.comando.ExecuteNonQuery();
                        mysql.Dispose();
                        MessageBox.Show("Solicitud procesada correctamente", "Acción realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button1.Enabled = false;
                        cedula.Focus();
                    }

                }



            }
            catch (Exception excep)
            {

                Mensaje.Error(excep, "102");
            }
        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "select * from clientes where Cedula='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.lector = mysql.comando.ExecuteReader();
                    while (mysql.lector.Read())
                    {
                        nombre.Text = mysql.lector["Nombre"].ToString();
                        telefono.Text = mysql.lector["Telefono"].ToString();
                        correo.Text = mysql.lector["Correo"].ToString();
                        direccion.Text = mysql.lector["Direccion"].ToString();
                      

                    }
                    mysql.Dispose();
                    button1.Enabled = true;
                    
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
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    using (var mysql = new Mysql())
                    {
                        mysql.conexion();
                        mysql.cadenasql = "select * from clientes where Cedula='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                        mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                        mysql.lector = mysql.comando.ExecuteReader();
                        while (mysql.lector.Read())
                        {
                            nombre.Text = mysql.lector["Nombre"].ToString();
                            telefono.Text = mysql.lector["Descripcion"].ToString();
                            correo.Text = mysql.lector["Correo"].ToString();
                            direccion.Text = mysql.lector["Direccion"].ToString();
                           

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

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) ||char.IsNumber(e.KeyChar) ||char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void direccion_KeyPress(object sender, KeyPressEventArgs e)
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

        private void nombre_KeyPress(object sender, KeyPressEventArgs e)
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

        private void telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void correo_KeyPress(object sender, KeyPressEventArgs e)
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

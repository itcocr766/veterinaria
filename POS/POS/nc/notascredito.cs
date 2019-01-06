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
namespace POS.nc
{
    public partial class notascredito : Form
    {
        Form1 f1;
        public notascredito(Form1 fo1)
        {
            InitializeComponent();f1=fo1;
        }

        private void notascredito_Load(object sender, EventArgs e)
        {
           
            cargarnotas();
        }

        public void cargarnotas()
        {
          
            try
            {
                using (var mysql= new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql="select Cliente,Monto,Detalle from notascredito where Cliente='"+f1.comboBox3.Text+"'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql,mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    using (MySqlDataReader lee=mysql.comando.ExecuteReader())
                    {
                        while (lee.Read())
                        {
                            dataGridView1.Rows.Add(lee["Cliente"].ToString(),lee["Monto"].ToString(),lee["Detalle"].ToString());
                        }

                    }
                        mysql.Dispose();

                }

            }
            catch (Exception erft)
            {
                MessageBox.Show(erft.ToString());

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
           
            double mon = 0;
            try
            {
              
                mon = double.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "delete from notascredito where Cliente='"+ dataGridView1.CurrentRow.Cells[0].Value.ToString() + "' and Monto='"+ dataGridView1.CurrentRow.Cells[1].Value.ToString() + "'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    mysql.Dispose();
                }
                this.Visible = false;
                f1.Activate();
       
                f1.textBox5.ReadOnly = false;
                f1.textBox5.Focus();
                f1.textBox5.Text = (double.Parse(f1.textBox5.Text)-mon).ToString();
                MessageBox.Show("Se aplicó la nota de crédito correctamente", "Solicitud procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                f1.textBox5.ReadOnly=true;
                f1.textBox1.Focus();
           
            }
            catch (Exception ette)
            {
                MessageBox.Show(ette.ToString());
            }
        }
    }
}

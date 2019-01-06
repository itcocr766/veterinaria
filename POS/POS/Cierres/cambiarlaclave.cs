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

namespace POS.Cierres
{
    public partial class cambiarlaclave : Form
    {
        string result = "";
        public cambiarlaclave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cambiar();
        }


        public void cambiar()
        {
            try
            {
                byte[] encryted = System.Text.Encoding.Unicode.GetBytes(textBox2.Text);
                result = Convert.ToBase64String(encryted);
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "update registro set Contrasena='" + result + "' where Codigo='2'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    mysql.Dispose();
                }

                MessageBox.Show("La clave fue cambiada exitosamente!", "Solicitud procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Visible = false;
            }
            catch (Exception efes)
            {
                MessageBox.Show(efes.Message);
            }

        }

        private void cambiarlaclave_Load(object sender, EventArgs e)
        {

        }
    }
}

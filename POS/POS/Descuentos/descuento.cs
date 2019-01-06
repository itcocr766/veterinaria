using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using POS.Modelo;
namespace POS.Descuentos
{
    public partial class descuento : Form
    {
        public descuento()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var mysql=new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql="UPDATE `descuento` SET `Montoad`='"+textBox1.Text+"',`Montoti`='"+textBox2.Text+"'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql,mysql.con);
                    mysql.comando.ExecuteNonQuery();
                    mysql.Dispose();
                    MessageBox.Show("Descuentos actualizados","Proceso terminado",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }

            }
            catch (Exception efe)
            {
                MessageBox.Show(efe.ToString());
            }
        }
    }
}

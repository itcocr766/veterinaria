using POS.Cierres;
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

namespace POS.whatsapp
{
    public partial class autenticacion : Form
    {
        conexionabasedatos cnd;
        sendwhat enviarwhat;
        public autenticacion()
        {
            InitializeComponent(); enviarwhat = new sendwhat();
            cnd = new conexionabasedatos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cnd.consultar(textBox1, textBox2))
            {
                Cierre cir = new Cierre();
                cir.Show(this);

            }
            else
            {
                MessageBox.Show("Los datos suministrados no son válidos.Intente de nuevo", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.ForeColor = Color.CadetBlue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.White;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Desea que le enviemos una nueva clave por Whatsapp?", "Solicitud en proceso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                enviarwhat.enviarwhat();
                MessageBox.Show("Un mensaje se ha enviado a su whatsapp.\n" +
                    "Por favor espere unos segundos para recibir su nueva clave", "Solicitud procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

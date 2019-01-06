using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.Configuracion
{
    public partial class configuracion : Form
    {
        string cadena = "";
        string cadena2 = "";
        string apicomp = "";
        string endpo = "";
        string autoincrement="";
        public configuracion()
        {
            InitializeComponent();
        }

        private void configuracion_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        public void guardar()
        {

            autoincrement = textBox5.Text.Trim();
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings.Remove("autoincrement");
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            conf.AppSettings.Settings.Add("autoincrement", autoincrement);
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void button1_Click(object sender, EventArgs e)
        {

            guardar();

            cadena = "server=" + textBox1.Text.Trim() + ";" + "port=" + textBox2.Text.Trim() + ";username=" + textBox3.Text.Trim() +
                 ";password=" + textBox4.Text.Trim() + ";SslMode = none;database=" + textBox5.Text.Trim();

            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings.Remove("cadena");
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            conf.AppSettings.Settings.Add("cadena", cadena);
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Configuracion exitosa. Verifique la cadena de conexion  " + ConfigurationManager.AppSettings["cadena"]);

            //server = 127.0.0.1; port = 3306; username = root; password =; SslMode = none; database = posnew
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cadena2 = "               " + textBox6.Text.Trim() + "\n               " +  textBox7.Text.Trim() + "\n          " +  textBox8.Text.Trim() + "\n          " +
                 textBox9.Text.Trim()  + "\n               " + textBox10.Text.Trim();

            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings.Remove("cadena2");
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            conf.AppSettings.Settings.Add("cadena2", cadena2);
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Configuración exitosa. Verifique la cadena de conexión  " + ConfigurationManager.AppSettings["cadena2"]);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            companyapi();
            endopo();
            MessageBox.Show("Configuración exitosa. Verifique la cadena de conexión  " + 
                ConfigurationManager.AppSettings["apicomp"] +"\n"+
                 ConfigurationManager.AppSettings["endpo"]);
        }

        public void companyapi()
        {
            apicomp = comboBox1.Text;
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings.Remove("apicomp");
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            conf.AppSettings.Settings.Add("apicomp", apicomp);
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

        }
      
        public void endopo()
        {
            endpo = comboBox2.Text;
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings.Remove("endpo");
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            conf.AppSettings.Settings.Add("endpo", endpo);
            conf.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

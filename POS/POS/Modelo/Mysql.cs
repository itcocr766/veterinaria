using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace POS.Modelo
{
    public class Mysql : IDisposable
    {

        public MySqlConnection con;
        public MySqlCommand comando = null;
        public MySqlDataReader lector;
        public string cadenasql = "";
        public MySqlTransaction trans;
        public void conexion()
        {
            con = new MySqlConnection(ConfigurationManager.AppSettings["cadena"]);
            //con = new MySqlConnection("server=104.43.243.208;port=3306;username=pos;password=123;SslMode=none;database=generaleno");
            //con = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;SslMode=none;database=generaleno");
            con.Open();
            
        }
        public void conexion2()
        {
            con = new MySqlConnection(ConfigurationManager.AppSettings["cadena"]);
            //con = new MySqlConnection("server=104.43.243.208;port=3306;username=pos;password=123;SslMode=none;database=generaleno");
            //con = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;SslMode=none;database=generaleno");
            con.Open();
            trans=con.BeginTransaction();
        }
      


        public void Dispose()
        {
            cadenasql = null;
            comando = null;
            con.Close();
            
        }
      
        public void rol()
        {
            try
            {
                //Dispose();
                //con.Open();
                trans.Commit();
            }
            catch (Exception e)
            {
                
                trans.Rollback();
                MessageBox.Show("Hemos revertido el proceso debido a n error al momento de conectar a la bae de datos: Rollback\n", e.ToString());
            }
            
            
        }



    }
}



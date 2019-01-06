using MySql.Data.MySqlClient;
using POS.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace POS.whatsapp
{
   
    public class sendwhat
    {
        Random ran = new Random();
        string result;
        public void enviarwhat()
        {

            int clave = ran.Next(1400, 5000);

            string yourId = "9qPTDurPy0+9VX1RJFjSRXNtdWVzMjAxN19hdF9nbWFpbF9kb3RfY29t";
            string yourMobile = "+50686489451";
            string yourMessage = "Clave" + clave.ToString() + "";
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(yourMessage);
            result = Convert.ToBase64String(encryted);

            try
            {
                using (var mysql = new Mysql())
                {
                    mysql.conexion();
                    mysql.cadenasql = "update registro set Contrasena='" + result + "' where Codigo='2'";
                    mysql.comando = new MySqlCommand(mysql.cadenasql, mysql.con);
                    mysql.comando.ExecuteNonQuery();

                    mysql.Dispose();
                }


                string url = "https://NiceApi.net/API";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("X-APIId", yourId);
                request.Headers.Add("X-APIMobile", yourMobile);
                using (StreamWriter streamOut = new StreamWriter(request.GetRequestStream()))
                {
                    streamOut.Write(yourMessage);
                }
                using (StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    Console.WriteLine(streamIn.ReadToEnd());
                }
            }
            catch (SystemException se)
            {
                Console.WriteLine(se.Message);
            }
            Console.ReadLine();

        }
    }
}

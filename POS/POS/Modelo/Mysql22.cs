﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Modelo
{
    public class Mysql22:IDisposable
    {
        public MySqlConnection con22;
        public MySqlCommand comando22 = null;
        public MySqlDataReader lector22;
        public string cadenasql22 = "";
        public void conexion22()
        {

            con22 = new MySqlConnection(ConfigurationManager.AppSettings["cadena22"]);
            //con22 = new MySqlConnection("server=104.43.243.208;port=3306;username=pos;password=123;SslMode=none;database=generaleno");
            //con22 = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;SslMode=none;database=generaleno");
            con22.Open();
        }
        public void Dispose()
        {
            cadenasql22 = null;
            comando22 = null;
            con22.Close();

        }
    }
}

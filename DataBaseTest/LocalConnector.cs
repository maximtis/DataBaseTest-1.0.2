using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;

namespace DataBaseTest
{
    static class LocalConnector
    {
        static LocalConnector()
        {
            SP13Connection = new MySqlConnection(Properties.Settings.Default.studentConnectionString);
        }
        public static MySqlConnection SP13Connection { get; private set; }
        public static bool ConnectionState { get; private set; }
        public static bool Connect()
        {
            try
            {
                SP13Connection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            ConnectionState = true;
            return ConnectionState;
        }
        public static bool Disconnect()
        {
            SP13Connection.Close();
            ConnectionState = false;
            return true;
        }
        
    }
}

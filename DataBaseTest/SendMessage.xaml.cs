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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace DataBaseTest
{
    /// <summary>
    /// Логика взаимодействия для SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        public SendMessage()
        {
            InitializeComponent();
            Label_Date.Content += DateTime.Now.ToShortDateString();
            MouseDown += SendMessage_MouseDown;
        }

        void SendMessage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_SendMessage_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

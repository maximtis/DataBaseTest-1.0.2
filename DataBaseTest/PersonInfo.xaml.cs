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
    /// Логика взаимодействия для PersonInfo.xaml
    /// </summary>
    public partial class PersonInfo : Window
    {
        public Member MyMember;
        public Member UpdatedMember;
        public PersonInfo(Member x)
        {
            MyMember = x;
            InitializeComponent();
            MouseDown += PersonInfo_MouseDown;
            Login.Text = MyMember.Person.Login;
            FirstName.Text = MyMember.Person.FirstName;
            LastName.Text = MyMember.Person.LastName;
            Password.Text = MyMember.Person.Password;
            Mobile_Number.Text = MyMember.Person.Mobile_Number;
            Email.Text = MyMember.Person.Email;


        }

        void PersonInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public PersonInfo()
        {
            MyMember = new Member();
            InitializeComponent();
            Login.Text = MyMember.Person.Login;
            FirstName.Text = MyMember.Person.FirstName;
            LastName.Text = MyMember.Person.LastName;
            Password.Text = MyMember.Person.Password;
            Mobile_Number.Text = MyMember.Person.Mobile_Number;
            Email.Text = MyMember.Person.Email;


        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_SendInfo_Click(object sender, RoutedEventArgs e)
        {
            
            if(LocalConnector.Connect())
            {
                new MySqlCommand(
                    "UPDATE `sp_13` SET `first_name` = '" + FirstName.Text + 
                    "' , `last_name` = '" + LastName.Text + 
                    "' , `password` = '" + Password.Text + 
                    "' , `mobile_number` = '" + Mobile_Number.Text + 
                    "' , `email` = '" + Email.Text + 
                    "' WHERE `login` = '" + Login.Text + "'", 
                    LocalConnector.SP13Connection).ExecuteNonQuery();

                LocalConnector.Disconnect();
            }
                UpdatedMember = new Member(new Person(
                    Login.Text,
                    FirstName.Text,
                    LastName.Text,
                    Password.Text,
                    MyMember.Person.Personal_Info,
                    Mobile_Number.Text,
                    Email.Text,
                    MyMember.Person.Message
                    ));
                UpdatedMember.SaveToFile(@"bin/storage.bin");
        }
        private void Button_UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            if (LocalConnector.Connect())
            {
                MySqlDataReader Reader = new MySqlCommand("SELECT `personal_info` FROM sp_13 WHERE `login` = \"" + MyMember.Person.Login + "\"", LocalConnector.SP13Connection).ExecuteReader();
                if (Reader.Read())
                    TextBlock_Informator.Text = Reader["personal_info"].ToString();
                LocalConnector.Disconnect();
            }
        }

        private void Button_Update_PersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            if (LocalConnector.Connect())
            {
                new MySqlCommand(
                    "UPDATE `sp_13` SET `personal_info` = '" + TextBlock_Informator.Text +
                    "' WHERE `login` = '" + MyMember.Person.Login + "'",
                    LocalConnector.SP13Connection).ExecuteNonQuery();

                LocalConnector.Disconnect();
                MessageBox.Show("Отправлено!");
            }
        }

        
    }
}

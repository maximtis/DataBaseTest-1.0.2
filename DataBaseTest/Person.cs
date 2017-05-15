using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DataBaseTest
{
    [Serializable]
    public class Person
    {
        public Person( string login,string firstname,
                        string lastname,string password,string personal_info,
                            string mobilenumber,string email,string message)
        {
            Login = login;
            FirstName = firstname;
            LastName = lastname;
            Password = password;
            Personal_Info = personal_info;
            Mobile_Number = mobilenumber;
            Email = email;
            Message = message;
        }
        public Person()
        {
            Login = "Empty";
            FirstName = "Empty";
            LastName = "Empty";
            Password = "Empty";
            Personal_Info = "Empty";
            Mobile_Number = "Empty";
            Email = "Empty";
            Message = "Empty";
        }

        public Person DeepCopy()
        {
            return new Person(Login, FirstName, LastName, Password, Personal_Info, Mobile_Number, Email, Message);
        }
        public string Message { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Personal_Info { get; set; }
        public string Mobile_Number { get; set; }
        public string Email { get; set; }
    }
}

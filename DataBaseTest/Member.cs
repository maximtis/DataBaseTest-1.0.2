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
    public class Member
    {
        public Member()
        {
            Authorized = false;
            Person = new Person();
        }
        public Member(Person person_data)
        {
            Authorized = false;
            Person = person_data;
        }
        public bool Loaded { get; private set; }
        public Person Person{ get; set; }
        public bool Authorized { get; private set; }

        public event RoutedEventHandler TryAuthorization;
        public bool Autorization()
        {
            bool Result = false;
            if (LocalConnector.Connect())
            {
                MySqlDataReader Reader =
                new MySqlCommand("SELECT `password` FROM `sp_13` WHERE `login` = \"" + Person.Login + "\"",
                        LocalConnector.SP13Connection).ExecuteReader();
                if (Reader.Read())
                    Result = Reader["password"].ToString() == Person.Password;
                LocalConnector.Disconnect();
            }
            if (Result) { 
                Authorized = true;
                      return true; 
            }
            return false;
        }
        public void LoadFromBase()
        {
            if (Person.Login != string.Empty && Person.Password != string.Empty)
            if (LocalConnector.Connect())
            {
                MySqlDataReader Reader =
                new MySqlCommand("SELECT * FROM `sp_13` WHERE `login` = \"" + Person.Login + "\"",
                        LocalConnector.SP13Connection).ExecuteReader();
                if (Reader.Read())
                {
                    Person.FirstName = Reader["first_name"].ToString();
                    Person.LastName = Reader["last_name"].ToString();
                    Person.Personal_Info = Reader["personal_info"].ToString();
                    Person.Mobile_Number = Reader["Mobile_Number"].ToString();
                    Person.Email = Reader["email"].ToString();
                    Person.Message = Reader["message"].ToString();
                }
                Loaded = true;
            }
            LocalConnector.Disconnect();
        }
        public void SendPersonalInfo(string Message)
        {
            if (LocalConnector.Connect())
            {
                var Send = new MySqlCommand(
                    "UPDATE `sp_13` SET `personal_info` = '" + Person.Personal_Info + "' WHERE `sp_13`.`login` = \"" + Person.Login + "\"",
                LocalConnector.SP13Connection).ExecuteNonQuery();
                LocalConnector.Disconnect();
            }
        }
        public bool CanLoadFromFile(string path)
        {
            return File.Exists(path);
        }
        public void SendMessage(string Message)
        {
            if (LocalConnector.Connect())
            {
                var Send = new MySqlCommand("UPDATE `sp_13` SET `message` = '" + Message + "' WHERE `sp_13`.`login` = \"" + Person.Login + "\"",
                LocalConnector.SP13Connection).ExecuteNonQuery();
                LocalConnector.Disconnect();
            }         
        }
        public Member DeepCopy()
        {
            return new Member(new Person(Person.Login, 
                                Person.FirstName, 
                                Person.LastName, 
                                Person.Password, 
                                Person.Personal_Info,
                                Person.Mobile_Number,
                                Person.Email,
                                Person.Message));
        }
        public bool SaveToFile(string path)
        {
            bool success = true;
            //Сохраняем состояние объекта Person в двоичном формате
            Member Data = this.DeepCopy() as Member;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (var fStream = new FileStream(@path/*@"bin/storage.bin"*/, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(fStream, Data);
                }
            }
            catch { success = false; }
            return success;
        }
        public Member LoadFromFile(string path)
        {
            Member LoadedPerson = null;;
            BinaryFormatter formatter = new BinaryFormatter();
            //Восстановим состояние объекта
            try
            {
                using (var fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                {
                    Member Data = (Member)formatter.Deserialize(fStream);

                    if (Data.Person.FirstName == null) return null;

                    if (Data.Person.LastName == null) return null;

                    if (Data.Person.Login == null) return null;

                    if (Data.Person.Password == null) return null;

                    if (Data.Person.Email == null) return null;

                    if (Data.Person.Mobile_Number == null) return null;

                    if (Data.Person.Personal_Info == null) return null;
                    LoadedPerson = new Member(new Person(
                                            Data.Person.Login,
                                            Data.Person.FirstName,
                                            Data.Person.LastName,
                                            Data.Person.Password,
                                            Data.Person.Personal_Info,
                                            Data.Person.Mobile_Number,
                                            Data.Person.Email,
                                            Data.Person.Message
                        )
                        );
                }
            }
            catch (Exception e){
                MessageBox.Show(e.ToString());
            }
            Loaded = true;
            if (TryAuthorization != null)
                TryAuthorization(this,new RoutedEventArgs());
            return LoadedPerson;
        }
    }
}

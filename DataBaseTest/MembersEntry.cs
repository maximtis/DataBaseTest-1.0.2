using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataBaseTest
{
    class MembersEntry
    {
        public MembersEntry()
        {
            //constructor
            MyGroupMembers = new List<Member>();
        }
        List<Member> MyGroupMembers;

        public Member this [int index]
        {
            get
            {
                if (index < MyGroupMembers.Count)
                    return MyGroupMembers[index];
                else return null;
            }
            set
            {
                if (index < MyGroupMembers.Count)
                    MyGroupMembers[index] = value;
            }
        }
        public int Count
        {
            get
            {
                return MyGroupMembers.Count;
            }
        }
        public void InitializeFromFile(int countOfmembers)
        {
            var LObject = new Member();
            for(int i=0;i<countOfmembers;i++)
            if(LObject.CanLoadFromFile(@"members/member"+i.ToString()+".bin"))
            {
                MyGroupMembers.Add(LObject.LoadFromFile(@"members/member" + i.ToString() + ".bin"));
            }
        }
        public void InitializeFromBase()
        {
            if(LocalConnector.Connect())
            {
                MySqlDataReader Reader = new MySqlCommand("SELECT * FROM sp_13",LocalConnector.SP13Connection).ExecuteReader();
                while(Reader.Read())
                {
                    MyGroupMembers.Add(new Member(new Person(
                                                  Reader["login"].ToString(),
                                                  Reader["first_name"].ToString(),
                                                  Reader["last_name"].ToString(),
                                                  Reader["password"].ToString(),
                                                  Reader["personal_info"].ToString(),
                                                  Reader["mobile_number"].ToString(),
                                                  Reader["email"].ToString(),
                                                  Reader["message"].ToString()
                                                 )));
                }
                LocalConnector.Disconnect();
            }
        }
        public void SaveMembersToFile(int countOfmembers)
        {
            for(int i=0;i<countOfmembers;i++)
            {
                MyGroupMembers[i].SaveToFile(@"members/member" + i.ToString() + ".bin");
            }
        }
        public void ClearMembers()
        {
            MyGroupMembers.Clear();
        }
        public void SendPersonalInfo(string Message,int NumberOfMember)
        {
            MyGroupMembers[NumberOfMember - 1].SendPersonalInfo(Message);
        }
        public void PostNew(string Name,string Content,string Tag,string Author)
        {
            new MySqlCommand("INSERT INTO `student`.`datainfo` (`id`, `name`, `content`, `date`, `tag`, `author`) VALUES (NULL, '" + Name + "', '" + Content + "', CURRENT_TIMESTAMP, '" + Tag + "', '" + Author + "')", LocalConnector.SP13Connection).ExecuteNonQuery();
        }
    }
}

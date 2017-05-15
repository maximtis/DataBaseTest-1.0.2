using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace DataBaseTest
{
    class NewsFeeder
    {
        public NewsFeeder()
        {
            NewsFeed = new List<NewsNode>();
        }
        public List<NewsNode> NewsFeed;
        public void UpdateInfo()
        {
            
            if(LocalConnector.Connect())
            {
                NewsFeed.Clear();
                MySqlDataReader Reader = new MySqlCommand("SELECT * FROM datainfo",LocalConnector.SP13Connection).ExecuteReader();
                while(Reader.Read())
                {
                    NewsFeed.Add(
                                new NewsNode(Reader["name"].ToString(),
                                Reader["content"].ToString(),
                                Reader["author"].ToString(),
                                Reader["date"].ToString(),
                                Reader["imgsrc"].ToString())
                                );
                }
                LocalConnector.Disconnect();
            }
        }
        public static void PostNews(NewsNode x)
        {
            if (LocalConnector.Connect())
            {
                new MySqlCommand("INSERT INTO `datainfo` (`name`, `content`, `date`, `author`, `imgsrc`, `tag`) VALUES ('" + x.Name + "', '" + x.NewsContent + "', CURRENT_TIMESTAMP, '" + x.Author + "','"+x.ImgSource+"', 'Tag')", LocalConnector.SP13Connection).ExecuteNonQuery();
                
                LocalConnector.Disconnect();
            }
        }
    }
}

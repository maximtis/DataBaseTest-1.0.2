using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DataBaseTest
{
    class NewsNode
    {
        public NewsNode(string name, string content, string author, string date,string imgsource="Cosmic.jpg")
        {
            Name = name;
            NewsContent = content;
            Author = author;
            Date = date;
            if(imgsource!="")
                ImgSource = new BitmapImage(new Uri(imgsource,UriKind.RelativeOrAbsolute)); /*BitmapImage(new Uri(imgsource,UriKind.Relative));*/
            

        }
        public BitmapImage ImgSource;
        public string Name;
        public string NewsContent;
        public string Author;
        public string Date;
        
    }
}

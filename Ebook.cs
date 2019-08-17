using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Master
{
    public class Ebook
    {
        public int id;
        public string title;
        public string author;
        public string format;
        public string dateAdd;
        public int categoryID;
        public string keywords;
        public string path;

        public Ebook(int id ,string title, string author, string format, string dateAdd, int categoryID, string keywords, string path)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.format = format;
            this.dateAdd = dateAdd;
            this.categoryID = categoryID;
            this.keywords = keywords;
            this.path = path;
        }
    }
}

using System.Data.SQLite;
using System.IO;
using System;

namespace Bib_Master.db
{
    class DB
    {
        public SQLiteConnection sqlCon;

        public DB()
        {
            SetConnection();
        }

        private void SetConnection()
        {
            string path = Directory.GetCurrentDirectory();
            string toRepl = @"bin\Debug";

            if (path.Contains(toRepl))
                path = path.Replace(toRepl, @"db\DataBase.db");
            else
                path = path.Replace(@"bin\Release", @"db\DataBase.db");

            sqlCon = new SQLiteConnection("Data Source = " + path + "; Version = 3;");
            sqlCon.Open();
        }
    }
}

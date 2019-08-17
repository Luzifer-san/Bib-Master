using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace Bib_Master.db
{
    class CategoryDB : DB
    {
        public SQLiteCommand command;
        SQLiteDataReader reader;

        public CategoryDB()
        {
            this.command = new SQLiteCommand(sqlCon);            
        }

        public void AddRecord(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("A critical error has occured.\n" +
                                "The program will now shut down.", "Error Message");
                Application.Current.Shutdown();
            }

            command.Parameters.AddWithValue("@name", name);
            command.CommandText = "insert into CategoryDB (NAME) values(@name);";

            command.ExecuteNonQuery();
        }

        public List<string> GetAllCategoryNames()
        {            
            var names = new List<string>();

            command.CommandText = "Select NAME from CategoryDB;";

            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    names.Add(reader["NAME"].ToString());
                }
            }

            return names;
        }

        public string GetCategoryByID(int id)
        {
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "Select NAME from CategoryDB where ID = @id;";

            string name = "";
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    name = reader["NAME"].ToString();
                }
            }

            return name;
        }

        public int GetCategoryID(string name)
        {
            command.Parameters.AddWithValue("@name", name);
            command.CommandText = "Select ID from CategoryDB where NAME = @name;";

            int id = 0;
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["ID"]);
                }
            }

            return id;
        }

        public void Delete(int id)
        {
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "Delete from CategoryDB where ID = @id;";
            command.ExecuteNonQuery();
        }        

        public void Rename(int categoryID, string newName)
        {
            command.Parameters.AddWithValue("@id", categoryID);
            command.Parameters.AddWithValue("@name", newName);
            command.CommandText = "Update CategoryDB set NAME = @name where ID = @id;";

            command.ExecuteNonQuery();
        }
    }
}

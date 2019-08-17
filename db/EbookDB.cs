using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Bib_Master.db
{
    class EbookDB : DB
    {
        public SQLiteCommand command;
        private SQLiteDataReader reader;

        public EbookDB()
        {
            this.command = new SQLiteCommand(sqlCon);
        }

        public void AddRecord(Ebook ebook)
        {
            command.Parameters.AddWithValue("@title", ebook.title);
            command.Parameters.AddWithValue("@author", ebook.author);
            command.Parameters.AddWithValue("@format", ebook.format);
            command.Parameters.AddWithValue("@dateAdd", ebook.dateAdd);
            command.Parameters.AddWithValue("@categoryID", ebook.categoryID);
            command.Parameters.AddWithValue("@key", ebook.keywords);
            command.Parameters.AddWithValue("@path", ebook.path);

            command.CommandText = "insert into EbookDB (TITLE,AUTHOR,FORMAT,DATE_ADD,CATEGORY_ID,KEY_WORDS, PATH) " +
                                  "values(@title,@author,@format,@dateAdd,@categoryID,@key, @path);";
            command.ExecuteNonQuery();
        }

        public List<Ebook> GetAllEbooks()
        {            
            command.Parameters.Clear();
            command.CommandText = "Select * from EbookDB;";

            var books = new List<Ebook>();
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var book = new Ebook(
                        Convert.ToInt32(reader["ID"]),
                        reader["TITLE"].ToString(),
                        reader["AUTHOR"].ToString(),
                        reader["FORMAT"].ToString(),
                        reader["DATE_ADD"].ToString(),
                        Convert.ToInt32(reader["CATEGORY_ID"]),
                        reader["KEY_WORDS"].ToString(),
                        reader["PATH"].ToString());

                    books.Add(book);
                }
            }

            return books;
        }

        public int NextID()
        {
            int id = 0;
            command.CommandText = "Select ID from EbookDB order by id asc;";

            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["ID"]);
                }
            }            

            return ++id;
        }

        public void DeleteRow(int id)
        {
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "Delete from EbookDB where ID = @id;";
            command.ExecuteNonQuery();
        }

        public Ebook GetBookByID(int id)
        {
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "Select * from EbookDB where id = @id;";

            Ebook book = null;
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    book = new Ebook(
                        Convert.ToInt32(reader["ID"]),
                        reader["TITLE"].ToString(),
                        reader["AUTHOR"].ToString(),
                        reader["FORMAT"].ToString(),
                        reader["DATE_ADD"].ToString(),
                        Convert.ToInt32(reader["CATEGORY_ID"]),
                        reader["KEY_WORDS"].ToString(),
                        reader["PATH"].ToString());
                }
            }

            return book;
        }

        public List<Ebook> GetFiltered(string input)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@input", input);
            command.CommandText = "Select * from EbookDB where KEY_WORDS like '%" + input + "%';";

            var books = new List<Ebook>();
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var book = new Ebook(
                        Convert.ToInt32(reader["ID"]),
                        reader["TITLE"].ToString(),
                        reader["AUTHOR"].ToString(),
                        reader["FORMAT"].ToString(),
                        reader["DATE_ADD"].ToString(),
                        Convert.ToInt32(reader["CATEGORY_ID"]),
                        reader["KEY_WORDS"].ToString(),
                        reader["PATH"].ToString());

                    books.Add(book);
                }
            }
            return books;
        }

        public void AssignCategory(int categoryID, int bookID)
        {
            command.Parameters.AddWithValue("@catID", categoryID);
            command.Parameters.AddWithValue("@bookID", bookID);

            command.CommandText = "Update EbookDB set CATEGORY_ID = @catID where ID = @bookID;";
            command.ExecuteNonQuery();
        }

        // Reset all entries with category = targetID to zero
        public void ResetCategories(int categoryID)
        {
            command.Parameters.AddWithValue("@id", categoryID);
            command.CommandText = "Update EbookDB set CATEGORY_ID = 0 where CATEGORY_ID = @id;";
            command.ExecuteNonQuery();
        }

        public List<Ebook> FilterForCategory(int categoryID)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", categoryID);
            command.CommandText = "Select * from EbookDB where CATEGORY_ID = @id;";

            var books = new List<Ebook>();
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var book = new Ebook(
                        Convert.ToInt32(reader["ID"]),
                        reader["TITLE"].ToString(),
                        reader["AUTHOR"].ToString(),
                        reader["FORMAT"].ToString(),
                        reader["DATE_ADD"].ToString(),
                        Convert.ToInt32(reader["CATEGORY_ID"]),
                        reader["KEY_WORDS"].ToString(),
                        reader["PATH"].ToString());

                    books.Add(book);
                }
            }
            return books;
        }

        public void UpdateMetadata(int bookID, string title, string author, string keywords)
        {
            command.Parameters.AddWithValue("@id", bookID);
            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@author", author);
            command.Parameters.AddWithValue("@key", keywords);
            command.CommandText = "Update EbookDB set TITLE = @title, AUTHOR = @author, KEY_WORDS = @key where ID = @id;";

            command.ExecuteNonQuery();
        }
    }
}

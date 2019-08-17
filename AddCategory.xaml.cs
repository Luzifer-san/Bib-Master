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
using Bib_Master.db;
using System.Data.SQLite;

namespace Bib_Master
{
    /// <summary>
    /// Interaktionslogik für AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        MainWindow owner;
        private CategoryDB categoryDB;

        public AddCategory(MainWindow owner)
        {
            InitializeComponent();

            this.owner = owner;
            this.categoryDB = new CategoryDB();
        }

        private void NewCategory(string name)
        {
            if (IsUnique(name))
            {
                categoryDB.AddRecord(name);
                owner.ShowCategories();
                this.Close();
            }
            else
                MessageBox.Show("A category with this name already exists.", "Invalid input");            
        }

        // Checks for double names of categories
        private bool IsUnique(string name)
        {
            SQLiteDataReader reader;

            categoryDB.command.Parameters.AddWithValue("@name", name);
            categoryDB.command.CommandText = "Select NAME from CategoryDB where NAME = @name;";

            string found = null;
            using (reader = categoryDB.command.ExecuteReader())
            {
                while (reader.Read())
                {
                    found = reader["NAME"].ToString();
                }
            }

            if (found != name)
                return true;
            else
                return false;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string input = txtboxName.Text;
            if (!string.IsNullOrWhiteSpace(input))
                NewCategory(input);
            else            
                MessageBox.Show("You must enter a name.","Invalid Input");            
        }        
    }
}

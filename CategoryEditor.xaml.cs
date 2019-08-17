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

namespace Bib_Master
{
    /// <summary>
    /// Interaktionslogik für CategoryEditor.xaml
    /// </summary>
    public partial class CategoryEditor : Window
    {
        private MainWindow owner;
        private List<string> categories;
        private CategoryDB categoryDB;

        public CategoryEditor(MainWindow owner, List<string> categories)
        {
            InitializeComponent();

            this.owner = owner;
            this.categories = categories;
            this.categoryDB = new CategoryDB();

            SetUI();
        }

        private void SetUI()
        {
            foreach(string c in categories)
            {
                comboCategories.Items.Add(c);
            }

            comboCategories.SelectedIndex = 0;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string input = txtboxName.Text;
            int index = comboCategories.SelectedIndex;

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("You must enter a new name for your category.", "Invalid Input",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (IsUnique(input))
                SaveNewName(index, input);
            else
            {
                MessageBox.Show("A category with this name already exists.", "Invalid Input",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool IsUnique(string name)
        {
            return !categories.Contains(name);
        }
        private void SaveNewName(int comboIndex, string newName)
        {
            string oldName = comboCategories.Items[comboIndex].ToString();
            int categoryID = categoryDB.GetCategoryID(oldName);

            categoryDB.Rename(categoryID, newName);

            owner.ShowCategories();
            this.Close();
        }
    }
}

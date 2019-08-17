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
using System.IO;

namespace Bib_Master
{
    /// <summary>
    /// Interaktionslogik für MetadataEditor.xaml
    /// </summary>
    public partial class MetadataEditor : Window
    {
        MainWindow owner;
        Ebook book;
        string coverPath;
        string imgPath;
        private EbookDB ebookDB;

        public MetadataEditor(MainWindow owner, Ebook book, string coverPath)
        {
            InitializeComponent();

            this.owner = owner;
            this.book = book;
            this.coverPath = coverPath;
            this.imgPath = this.coverPath + book.title + ".png";
            this.ebookDB = new EbookDB();

            LoadUI();
        }

        private void LoadUI()
        {
            imgCover.Source = new BitmapImage(new Uri(imgPath));

            txtboxTitle.Text    = book.title;
            txtboxAuthor.Text   = book.author;
            txtboxKeywords.Text = book.keywords;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            CopyCover();
            SaveChanges();
        }

        private void CopyCover()
        {
            if (File.Exists(imgPath))
            {
                string destPath = imgPath.Replace(book.title, txtboxTitle.Text);
                if (!File.Exists(destPath))
                    File.Copy(imgPath, destPath);
            }
        }

        private void SaveChanges()
        {
            string title = txtboxTitle.Text;
            string author = txtboxAuthor.Text;
            string keywords = txtboxKeywords.Text;

            ebookDB.UpdateMetadata(book.id, title, author, keywords);
            owner.ShowBooks();
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Bib_Master.db;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using VersOne.Epub;
using Ghostscript.NET.Rasterizer;
using System.Diagnostics;


namespace Bib_Master
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CategoryDB categoryDB;
        private EbookDB ebookDB;
        public List<Ebook> books;
        private string coverPath;
        private string dataPath;
        private List<string> categories;

        public MainWindow()
        {
            InitializeComponent();

            CheckDependencies();

            this.categoryDB = new CategoryDB();
            this.ebookDB = new EbookDB();
            this.categories = categoryDB.GetAllCategoryNames();

            SetPaths();
            SetLayout();
        }

        private void CheckDependencies()
        {
            string ghostPath = @"C:\Program Files (x86)\gs\gs9.26\bin";
            if (!Directory.Exists(ghostPath))
            {
                MessageBoxResult result = MessageBox.Show("No version of GhostScript has been found.\n" +
                                "In order to use Bib-Master, you need to download and install\n" +
                                "GhostScript to the standard location. Wish to download it now?", "Missing files",
                                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)                
                    Process.Start("https://github.com/ArtifexSoftware/ghostpdl-downloads/releases/download/gs926/gs926aw32.exe");              

                Application.Current.Shutdown();
            }
        }

        private void SetPaths()
        {
            dataPath = Directory.GetCurrentDirectory();

            if (dataPath.Contains(@"bin\Debug"))
                dataPath = dataPath.Replace(@"bin\Debug", @"Data\icons\");
            else
                dataPath = dataPath.Replace(@"bin\Release", @"Data\icons\");

            coverPath = dataPath.Replace(@"icons", @"book_covers");
        }

        private void SetLayout()
        {
            string[] images = new string[7];

            images[0] = dataPath + "001-add.png";
            images[1] = dataPath + "002-cancel.png";
            images[2] = dataPath + "003-student.png";
            images[3] = dataPath + "004-paper.png";
            images[4] = dataPath + "005-list.png";
            images[5] = dataPath + "006-cross.png";
            images[6] = dataPath + "007-settings.png";

            imageAdd.Source = new BitmapImage(
                new Uri(images[0]));

            imageDelete.Source = new BitmapImage(
                new Uri(images[1]));

            imageRead.Source = new BitmapImage(
                new Uri(images[2]));

            imageEdit.Source = new BitmapImage(
                new Uri(images[3]));

            imageCategory.Source = new BitmapImage(
                new Uri(images[4]));

            imgClearFilter.Source = new BitmapImage(
                new Uri(images[5]));
            imgClearFilter.Stretch = Stretch.Uniform;

            //imageSettings.Source = new BitmapImage(
             //   new Uri(images[6]));


            ShowCategories();
            ShowBooks();
            BuildCategoryContextMenu();
        }

        private void BuildCategoryContextMenu()
        {
            contxtCategory.Items.Clear();

            var noneItem = new MenuItem();

            noneItem.Header = "None";
            noneItem.Name = "contxt_None";
            noneItem.Click += SubItem_Click;

            contxtCategory.Items.Add(noneItem);
            contxtCategory.Items.Add(new Separator());

            foreach (string c in categories)
            {
                var item = new MenuItem();
                item.Header = c;
                item.Name = "contxt_" + c;
                item.Click += SubItem_Click;
                contxtCategory.Items.Add(item);
            }
        }

        private void LoadCategories()
        {
            categories = categoryDB.GetAllCategoryNames();
        }

        private void SubItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            ListViewEntry entry = GetSelectedItem();
            string category = item.Header.ToString();

            if (categories.Contains(category) && entry != null)
            {
                int catID = categoryDB.GetCategoryID(category);
                ebookDB.AssignCategory(catID, entry.Id);
            }
            else if (category == "None")
            {
                ebookDB.AssignCategory(0, entry.Id);
            }

            ShowPreview(ref entry);
        }

        public void ShowCategories()
        {
            tvCategories.Items.Clear();
            LoadCategories();

            var item = new TreeViewItem();
            item.Header = "Categories";

            foreach (string c in categories)
            {
                var subItem = new TreeViewItem();
                subItem.Header = c;

                item.Items.Add(subItem);
            }

            item.IsExpanded = true;
            tvCategories.Items.Add(item);

            BuildCategoryContextMenu();
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategory = new AddCategory(this);
            addCategory.ShowDialog();
        }

        private void MenuitemAdd_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();

            fd.Filter = "eBook files (*.pdf;*.epub)|*.pdf;*.epub";
            fd.DefaultExt = ".pdf";
            fd.InitialDirectory = @"c:\";
            fd.Title = "Select eBook";

            Nullable<bool> result = fd.ShowDialog();

            string file = null;
            if (result == true)
            {
                file = fd.FileName;
                if (!ValidExtension(file))
                {
                    MessageBox.Show("The selected file must be a .pdf or .epub file.", "Invalid file",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(file))
                AddBook(file);
        }

        private bool ValidExtension(string file)
        {
            string extension = System.IO.Path.GetExtension(file);
            extension = extension.ToLower();

            if (extension == ".pdf" || extension == ".epub")
                return true;
            else
                return false;
        }

        private void AddBook(string file)
        {
            string path = file;
            string format = System.IO.Path.GetExtension(file);
            string dateAdd = DateTime.Today.ToShortDateString();
            string title = System.IO.Path.GetFileName(file);

            title = title.Replace(format, string.Empty);

            string author;
            string keywords;

            if (format.ToLower() == ".pdf")
            {
                GetDataPDF(file, out author, out keywords);
                SaveCoverToFolderPDF(path);
            }
            else
            {
                GetDataEPUB(file, out author, out keywords);
                SaveCoverToFolderEPUB(path);
            }

            var book = new Ebook(
                ebookDB.NextID(),
                title,
                author,
                format,
                dateAdd,
                0,
                keywords,
                file);

            ebookDB.AddRecord(book);
            ShowBooks();
        }

        private void SaveCoverToFolderPDF(string path)
        {
            using (var ras = new GhostscriptRasterizer())
            {
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    ras.Open(file);
                    string outPath = coverPath;
                    outPath += System.IO.Path.GetFileNameWithoutExtension(path) + ".png";

                    System.Drawing.Image img = ras.GetPage(100, 100, 1);
                    img.Save(outPath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void SaveCoverToFolderEPUB(string path)
        {
            EpubBook book = EpubReader.ReadBook(path);

            byte[] imgContent = book.CoverImage;
            if (imgContent != null)
            {
                string outPath = coverPath;
                outPath += System.IO.Path.GetFileNameWithoutExtension(path) + ".png";

                using (var imgStream = new MemoryStream(imgContent))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream);
                    img.Save(outPath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void GetDataPDF(string path, out string author, out string keywords)
        {
            using (PdfDocument file = PdfReader.Open(path))
            {
                author = file.Info.Author;
                keywords = file.Info.Keywords;
            }

            CheckKeyWords(ref keywords, ref author, path);
        }

        // epub files don't contain keywords -> manually assign name and author
        private void GetDataEPUB(string path, out string author, out string keywords)
        {
            EpubBook book = EpubReader.ReadBook(path);

            author = book.Author;
            keywords = System.IO.Path.GetFileName(path) + ";";

            if (!string.IsNullOrWhiteSpace(author))
                keywords += author + ";";
        }

        // Make sure the keywords always contain the book title and author (only pdf)
        private void CheckKeyWords(ref string keywords, ref string author, string file)
        {
            string name = System.IO.Path.GetFileName(file);

            if (string.IsNullOrWhiteSpace(keywords))
            {
                keywords = name + ";";
                if (!string.IsNullOrWhiteSpace(author))
                    keywords += author + ";";
            }
            else if (!keywords.Contains(name))
                keywords += name + ";";

            if (!keywords.Contains(author))
                keywords += author + ";";
        }

        public void LoadBooks()
        {
            books = ebookDB.GetAllEbooks();
        }

        public void ShowBooks(bool filter = false)
        {
            if (!filter)
                LoadBooks();

            listviewBooks.Items.Clear();
            foreach (Ebook book in books)
            {
                listviewBooks.Items.Add(new ListViewEntry
                {
                    Title = book.title,
                    Author = book.author,
                    Format = book.format,
                    DateAdd = book.dateAdd,
                    Id = book.id
                });
            }
        }

        private void ContxtDelete_Click(object sender, RoutedEventArgs e)
        {
            ListViewEntry book = GetSelectedItem();

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this entry?\n" +
                                                      "This step cannot be reversed.", "Confirm input",
                                                      MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                DeleteEntry(book.Id);
        }

        private void DeleteEntry(int dbID)
        {
            ebookDB.DeleteRow(dbID);
            ClearInfoView();
            ShowBooks();
        }

        private void ClearInfoView()
        {
            imageCover.Source = null;
            txtTitle.Text = "Title: -";
            txtCategory.Text = "Category: -";
            txtPath.Text = "Path: -";
        }

        // Show cover and stuff when a book is selected
        private void ShowPreview(ref ListViewEntry item)
        {
            if (item == null) return;

            int dbIndex = item.Id;
            Ebook book = ebookDB.GetBookByID(dbIndex);

            if (!File.Exists(book.path))
            {
                if (ConfirmDeletionMissingBook())
                    DeleteEntry(book.id);
            }

            try
            {
                string imgPath = coverPath + book.title + ".png";
                imageCover.Source = new BitmapImage(new Uri(imgPath));

                txtTitle.Text = "Title: " + book.title;

                string category = "Category: ";
                if (book.categoryID > 0)
                    category += categoryDB.GetCategoryByID(book.categoryID);
                else
                    category += "-";

                txtCategory.Text = category;

                txtPath.Text = "Path: " + book.path;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured.\n Error message: " + e.ToString(), "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ConfirmDeletionMissingBook()
        {
            MessageBoxResult result = MessageBox.Show("This book doesn't exist anymore.\n" +
                                          "Do you wish to delete it from the list?",
                                          "File not found",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                return true;
            else
                return false;
        }

        private void ButtonSeek_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtboxSeek.Text))
            {
                string input = txtboxSeek.Text;
                books = ebookDB.GetFiltered(input);
                ShowBooks(true);
            }
        }

        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtboxSeek.Text = "";
            ShowBooks();
        }

        private ListViewEntry GetSelectedItem()
        {
            if (listviewBooks.SelectedItems.Count == 0)
                return null;

            ListViewEntry item = listviewBooks.SelectedItems[0] as ListViewEntry;
            return item;
        }

        private void ListviewBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewEntry item = GetSelectedItem();

            if (item != null)
                ShowPreview(ref item);
        }

        private TreeViewItem GetSelectedCategory()
        {
            TreeViewItem item = tvCategories.SelectedItem as TreeViewItem;
            return item;
        }

        private void MenuitemDelete_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = GetSelectedCategory();

            if (item == null)
            {
                MessageBox.Show("You must select a category in order to delete it.",
                                "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string header = item.Header.ToString();
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the category \"" + header + "\"?\n" +
                                                      "This step cannot be reversed.", "Confirm Input", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int id = categoryDB.GetCategoryID(header);
                categoryDB.Delete(id);
                ResetCategories(id);
                ShowCategories();

                ListViewEntry selected = GetSelectedItem();
                if (selected == null) return;
                
                ShowPreview(ref selected);
            }
        }

        private void ResetCategories(int targetID)
        {
            ebookDB.ResetCategories(targetID);
        }

        private void TvCategories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = GetSelectedCategory();

            if (item == null) return;
            string header = item.Header.ToString();
            if (!categories.Contains(header)) return;

            int categoryID = categoryDB.GetCategoryID(header);
            books = ebookDB.FilterForCategory(categoryID);
            ShowBooks(true);
        }

        private void MenuitemRead_Click(object sender, RoutedEventArgs e)
        {
            ReadBook();
        }

        private void ReadBook()
        {
            ListViewEntry item = GetSelectedItem();

            if (item == null)
            {
                MessageBox.Show("You must select a book in order to read it.", "No book selected",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Ebook book = ebookDB.GetBookByID(item.Id);

            if (!File.Exists(book.path))
            {
                if (ConfirmDeletionMissingBook())
                    DeleteEntry(book.id);

                return;
            }

            Process.Start(book.path);
        }

        private void OpenPath()
        {
            ListViewEntry item = GetSelectedItem();

            if (item == null) return;

            Ebook book = ebookDB.GetBookByID(item.Id);
            if (!File.Exists(book.path))
            {
                if (ConfirmDeletionMissingBook())
                    DeleteEntry(book.id);
                return;
            }

            string arg = "/select, \"" + book.path + "\"";
            Process.Start("explorer.exe", arg);
        }

        private void ListviewBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ReadBook();
        }

        private void ContxtPath_Click(object sender, RoutedEventArgs e)
        {
            OpenPath();
        }

        private void MenuitemCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryEdit = new CategoryEditor(this, categories);
            categoryEdit.ShowDialog();
        }

        private void MenuitemEdit_Click(object sender, RoutedEventArgs e)
        {
            ListViewEntry entry = GetSelectedItem();
            if (entry == null) return;

            Ebook book = ebookDB.GetBookByID(entry.Id);

            var metaEdit = new MetadataEditor(this, book, coverPath);
            metaEdit.ShowDialog();
        }

        private void MenuitemSettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
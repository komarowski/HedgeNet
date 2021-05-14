using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HedgeNet.Models;
using Microsoft.Win32;

namespace HedgeNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<PasswordInfo> Passwords;
        public static List<string> TabNames;
        public static int currentID = 0;

        public void UpdateListView(bool isCheckFreshness = false)
        {
            var tabs = new ObservableCollection<PasswordTab>();
            TabNames = Passwords.Select(x => x.Categoty).Distinct().ToList();
            TabNames.Sort();
            foreach (var tabName in TabNames)
            {
                var tab = new PasswordTab() { Header = tabName };
                var categotyPasswords = Passwords.Where(x => x.Categoty == tabName).ToList();
                var rows = ConvertData.GetPasswordItems(categotyPasswords, isCheckFreshness);
                var orderRows = rows.OrderBy(x => x.website);
                foreach (var item in orderRows)
                {
                    tab.Data.Add(item);
                }
                tabs.Add(tab);
            }
            DataContext = tabs;
            tabControl.SelectedIndex = 0;
        }

        public MainWindow()
        {
            InitializeComponent();
            Passwords = ParseTxt.ReadPasswords();
            UpdateListView();
        }


        // Buttons Logic
        public PasswordItem GetPasswordItem(object sender)
        {
            var itemButton = sender as Button;
            return itemButton.DataContext as PasswordItem;
        }

        private void button_Click_CopyLogin(object sender, RoutedEventArgs e)
        {
            PasswordItem itemButton = GetPasswordItem(sender);
            Clipboard.SetText(itemButton.login);
            textBlock.Text = $"{itemButton.website} login saved to clipboard";
        }

        private void button_Click_CopyPassword(object sender, RoutedEventArgs e)
        {
            PasswordItem itemButton = GetPasswordItem(sender);
            if (itemButton.Password == "******")
            {
                Clipboard.SetText(Passwords.First(x => x.ID == itemButton.id).Password);
            }
            else Clipboard.SetText(itemButton.Password);
            textBlock.Text = $"{itemButton.website} password saved to clipboard";
        }

        private void button_Click_ShowPassword(object sender, RoutedEventArgs e)
        {
            PasswordItem itemButton = GetPasswordItem(sender);
            if (itemButton.Password == "******")
            {
                itemButton.Password = Passwords.First(x => x.ID == itemButton.id).Password;
            }
            else itemButton.Password = "******";
        }


        // ContextMenu Logic
        private void ListViewItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ListViewItem listItem = sender as ListViewItem;
            var a = (PasswordItem)listItem.DataContext;
            currentID = a.id;
        }

        private void ShowItem(object sender, RoutedEventArgs e)
        {
            if (currentID != 0)
            {
                var showWindow = new ShowWindow(currentID);
                showWindow.Show();
            }
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (currentID != 0)
            {
                var newWindow = new NewWindow(this, currentID);
                newWindow.Show();
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (currentID != 0)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to remove this password?", "Delete password", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    PasswordInfo removePasswordInfo = Passwords.First(x => x.ID == currentID);
                    textBlock.Text = $"{removePasswordInfo.Website} password has been deleted";
                    Passwords.Remove(removePasswordInfo);
                    ParseTxt.WritePasswords(Passwords);
                    UpdateListView();       
                }
            }
        }


        // MenuItems Logic
        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            var newWindow = new NewWindow(this);
            newWindow.Show();
        }

        private void MenuItem_Click_CheckFreshness(object sender, RoutedEventArgs e)
        {
            UpdateListView(true);
            textBlock.Text = "How many days have passed since the last password change";
        }

        private void MenuItem_Click_SaveDocx(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "passwords";
            saveFileDialog1.DefaultExt = "docx";
            saveFileDialog1.Filter = "Word File (.docx ,.doc)|*.docx;*.doc";
            if (saveFileDialog1.ShowDialog() == true)
            {
                try
                {
                    var path = saveFileDialog1.FileName;
                    GenerateDocx.GenerateDocxFile(path);
                    textBlock.Text = "Passwords successfully saved";
                }
                catch
                {
                    textBlock.Text = "Failed to save passwords";
                }
            }
        }

        private void MenuItem_Click_OpenHelp(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }


        // Search passwords Logic
        public void SearchPassword(string beginning)
        {
            textBlock.Text = $"Search results for {beginning}";
            beginning = beginning.Trim().ToLower();
            var tabs = new ObservableCollection<PasswordTab>();
            var tab = new PasswordTab() { Header = "Search" };
            var categotyPasswords = Passwords.Where(x => x.Website.ToLower().StartsWith(beginning)).ToList();
            var rows = ConvertData.GetPasswordItems(categotyPasswords, false);
            foreach (var item in rows)
            {
                tab.Data.Add(item);
            }
            tabs.Add(tab);
            DataContext = tabs;
            tabControl.SelectedIndex = 0;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = textBox.Text;
            if (searchText.Length > 0)
            {
                SearchPassword(searchText);
            }
            else
            {
                UpdateListView();
                textBlock.Text = "";
            }
        }
    }
}

using System;
using System.Windows;
using HedgeNet.Models;
using System.Linq;

namespace HedgeNet
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        private MainWindow mainWindow;
        private PasswordInfo editPasswordInfo = null;

        public bool CheckIsNullorEmpty()
        {
            var isNull = textBox.Text == null || textBox2.Text == null || comboBox2.Text == null || textBox3.Text == null;
            var isEmpty = textBox.Text == "" || (textBox2.Text == "" && comboBox2.Text == "") || textBox3.Text == "";
            return (isNull || isEmpty);
        }

        public bool CheckLetter(char s)
        {
            return textBox.Text.Contains(s) || comboBox.Text.Contains(s) || comboBox2.Text.Contains(s) || textBox2.Text.Contains(s) || textBox3.Text.Contains(s) || textBox4.Text.Contains(s);
        }

        public int GetNewId()
        {
            var newId = 0;
            if (MainWindow.Passwords.Count > 0) newId = MainWindow.Passwords.Select(x => x.ID).Last();
            return newId + 1;
        }

        public void InitializeCombobox()
        {
            var emails = MainWindow.Passwords.Select(x => x.Email).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            if (emails.Count > 0)
            {
                emails.ForEach(item => comboBox2.Items.Add(item));
                comboBox2.SelectedIndex = 0;
            }
            var categories = MainWindow.Passwords.Select(x => x.Categoty).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            if (categories.Count > 0)
            {
                categories.ForEach(item => comboBox.Items.Add(item));
                comboBox.SelectedIndex = 0;
            }  
        }

        public string GetCategory()
        {
            var category = "Unknown";
            if (comboBox.Text != "" && comboBox.Text != null) 
            {
                category = comboBox.Text;
            }
            if (!MainWindow.TabNames.Contains(category)) MainWindow.TabNames.Add(category);
            return category;
        }

        public void ChangeAddPassword()
        {
            var newPassword = new PasswordInfo
                {
                    Website = textBox.Text,
                    Categoty = GetCategory(),
                    Username = textBox2.Text,
                    Email = comboBox2.Text,
                    Password = textBox3.Text,
                    UpdateDate = DateTime.Now.ToString("dd.MM.yyyy"),
                    Note = textBox4.Text
                };
            if (editPasswordInfo != null)
            {
                var index = MainWindow.Passwords.IndexOf(editPasswordInfo);
                // Change date if password changed
                if (newPassword.Password == MainWindow.Passwords[index].Password) newPassword.UpdateDate = MainWindow.Passwords[index].UpdateDate;
                newPassword.ID = MainWindow.Passwords[index].ID;
                MainWindow.Passwords[index] = newPassword;
                mainWindow.textBlock.Text = $"{MainWindow.Passwords[index].Website} data has been changed";
            }
            else
            {
                newPassword.ID = GetNewId();
                MainWindow.Passwords.Add(newPassword);
                mainWindow.textBlock.Text = $"{newPassword.Website} password has been added";
            }
            ParseTxt.WritePasswords(MainWindow.Passwords);
            mainWindow.UpdateListView();
            this.Close();
        }

        public NewWindow(MainWindow window)
        {
            InitializeComponent();
            InitializeCombobox();
            mainWindow = window;
        }

        public NewWindow(MainWindow window, int id)
        {
            InitializeComponent();
            InitializeCombobox();
            mainWindow = window;
            editPasswordInfo = MainWindow.Passwords.First(x => x.ID == id);
            textBox.Text = editPasswordInfo.Website;
            comboBox.Text = editPasswordInfo.Categoty;
            textBox2.Text = editPasswordInfo.Username;
            comboBox2.Text = editPasswordInfo.Email;
            textBox3.Text = editPasswordInfo.Password;
            textBox4.Text = editPasswordInfo.Note;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBox3.Text = PasswordGenerator.GeneratePassword((int)slider.Value, (bool)checkBox.IsChecked);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBox3.Text = PasswordGenerator.GeneratePassword((int)slider.Value, (bool)checkBox.IsChecked);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLetter(ParseTxt.Separator))
            {
                MessageBox.Show("Symbol [~] cannot be used.", "Invalid letter");
                return;
            }
            if (CheckIsNullorEmpty())
            {
                MessageBox.Show("Fill in the field [Website name], [Password], [Username] or [Email]", "Empty field");
                return;
            }
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to save your changes?", "Save password", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                ChangeAddPassword();
            }
        }
    }
}

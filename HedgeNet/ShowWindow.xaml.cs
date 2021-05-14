using System.Windows;
using HedgeNet.Models;
using System.Linq;

namespace HedgeNet
{
    /// <summary>
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        public ShowWindow(int id)
        {
            InitializeComponent();
            PasswordInfo showPassword = MainWindow.Passwords.First(x => x.ID == id);
            textBox.Text = showPassword.Website;
            textBox2.Text = showPassword.Username;
            textBox3.Text = showPassword.Email;
            textBox4.Text = showPassword.Password;
            textBox5.Text = showPassword.Note;
            textBox6.Text = showPassword.UpdateDate;
        }
    }
}

using System;
using System.Windows;

namespace HedgeNet
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        const string PIN = "2020";

        public Authorization()
        {
            InitializeComponent();
            passwordBox.Focus();
        }

        void PasswordChangedHandler(Object sender, RoutedEventArgs args)
        {
            if (passwordBox.Password == PIN)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }    
        }
    }
}

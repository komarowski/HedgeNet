using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HedgeNet.Models
{
    public class PasswordInfo
    {
        public int ID { get; set; }
        public string Website { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UpdateDate { get; set; }
        public string Note { get; set; }
        public string Categoty { get; set; }
    }

    public class PasswordItem : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string website { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class PasswordTab
    {
        public string Header { get; set; }

        public ObservableCollection<PasswordItem> Data { get; } = new ObservableCollection<PasswordItem>();
    }
}

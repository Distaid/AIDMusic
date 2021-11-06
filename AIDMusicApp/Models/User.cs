using AIDMusicApp.Sql;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIDMusicApp.Models
{
    public class User : INotifyPropertyChanged
    {
        private int _id;

        private string _login;

        private string _password;

        private string _phone;

        private string _email;

        private Access _accessId;

        private byte[] _avatar;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public Access AccessId
        {
            get => _accessId;
            set
            {
                _accessId = value;
                OnPropertyChanged("AccessId");
            }
        }

        public byte[] Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
                OnPropertyChanged("Avatar");
            }
        }

        public void Delete()
        {
            SqlDatabase.Instance.UsersAdapter.Delete(Id);
        }

        public void Update(string login, string password, string phone, string email, Access accessId, byte[] avatar)
        {
            SqlDatabase.Instance.UsersAdapter.Update(Id, login, password, phone, email, accessId.Id, avatar);
            Login = login;
            Password = password;
            Phone = phone;
            Email = email;
            AccessId = accessId;
            Avatar = avatar;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

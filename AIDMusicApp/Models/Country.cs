using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIDMusicApp.Models
{
    public class Country : INotifyPropertyChanged
    {
        private int _id;

        private string _name;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Country Copy()
        {
            return new Country
            {
                Id = Id,
                Name = Name
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

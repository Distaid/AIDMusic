using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIDMusicApp.Models
{
    public class AlbumFormat : INotifyPropertyChanged
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

        public AlbumFormat Copy()
        {
            return new AlbumFormat
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

using AIDMusicApp.Sql;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIDMusicApp.Models
{
    public class Genre : INotifyPropertyChanged
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

        public void Delete()
        {
            SqlDatabase.Instance.GenresListAdapter.Delete(Id);
        }

        public void Update(string name)
        {
            SqlDatabase.Instance.GenresListAdapter.Update(Id, name);
            Name = name;
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

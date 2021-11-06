using AIDMusicApp.Sql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIDMusicApp.Models
{
    public class Musician : INotifyPropertyChanged
    {
        private int _id;

        private string _name;

        private byte _age;

        private Country _countryId;

        private bool _isDead;

        private ObservableCollection<Skill> _skills;

        private bool _isFormer;

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

        public byte Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public Country CountryId
        {
            get => _countryId;
            set
            {
                _countryId = value;
                OnPropertyChanged("CountryId");
            }
        }

        public bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                OnPropertyChanged("IsDead");
            }
        }

        public ObservableCollection<Skill> Skills
        {
            get => _skills;
            set
            {
                _skills = value;
                OnPropertyChanged("Skills");
            }
        }

        public bool IsFormer
        {
            get => _isFormer;
            set
            {
                _isFormer = value;
                OnPropertyChanged("IsFormer");
            }
        }

        public void Delete()
        {
            SqlDatabase.Instance.MusiciansAdapter.Delete(Id);
        }

        public void Update(string name, byte age, Country countryId, bool isDead)
        {
            SqlDatabase.Instance.MusiciansAdapter.Update(Id, name, age, countryId.Id, isDead);
            Name = name;
            Age = age;
            CountryId = countryId;
            IsDead = isDead;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

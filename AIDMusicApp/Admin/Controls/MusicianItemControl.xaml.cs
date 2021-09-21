using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для MusicianItemControl.xaml
    /// </summary>
    public partial class MusicianItemControl : UserControl
    {
        public Musician MusicianItem { get; private set; } = null;

        public MusicianItemControl(Musician musician)
        {
            InitializeComponent();

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;

            MusicianItem = musician;
            IdText.Text = MusicianItem.Id.ToString();
            NameText.Text = MusicianItem.Name;
            AgeText.Text = MusicianItem.Age.ToString();
            CountryText.Text = SqlDatabase.Instance.CountriesListAdapter.GetById(MusicianItem.CountryId).Name;
            if (!MusicianItem.IsDead)
                IsDead.Visibility = Visibility.Visible;

            foreach (var skill in MusicianItem.Skills)
            {
                var text = new TextBlock { Text = skill.Name };
                SkillsItems.Children.Add(text);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new MusiciansWindow(MusicianItem);
            if (editWindow.ShowDialog() == true)
            {
                MusicianItem = editWindow.MusicianItem;
                NameText.Text = MusicianItem.Name;
                AgeText.Text = MusicianItem.Age.ToString();
                CountryText.Text = SqlDatabase.Instance.CountriesListAdapter.GetById(MusicianItem.CountryId).Name;
                if (!MusicianItem.IsDead)
                    IsDead.Visibility = Visibility.Visible;

                SkillsItems.Children.Clear();
                foreach (var skill in MusicianItem.Skills)
                {
                    var text = new TextBlock { Text = skill.Name };
                    SkillsItems.Children.Add(text);
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var skill in MusicianItem.Skills)
            {
                var id = SqlDatabase.Instance.MusicianSkillsAdapter.GetIdByMusicianIdAndSkillId(MusicianItem.Id, skill.Id);
                SqlDatabase.Instance.MusicianSkillsAdapter.Delete(id);
            }
            SqlDatabase.Instance.MusiciansAdapter.Delete(MusicianItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

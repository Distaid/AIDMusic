using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Musicians
{
    /// <summary>
    /// Логика взаимодействия для MusicianItemControl.xaml
    /// </summary>
    public partial class MusicianItemControl : UserControl
    {
        public MusicianItemControl(Musician musician)
        {
            InitializeComponent();

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;

            MusicianItem.Id = musician.Id;
            MusicianItem.Name = musician.Name;
            MusicianItem.Age = musician.Age;
            MusicianItem.CountryId = musician.CountryId;
            MusicianItem.IsDead = musician.IsDead;
            MusicianItem.Skills = musician.Skills;
        }

        public Musician MusicianItem => (Musician)Resources["MusicianItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new MusiciansWindow(MusicianItem);
            editWindow.ShowDialog();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            MusicianItem.Delete();
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

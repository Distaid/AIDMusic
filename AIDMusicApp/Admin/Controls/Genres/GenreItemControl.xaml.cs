using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Genres
{
    /// <summary>
    /// Логика взаимодействия для GenreItemControl.xaml
    /// </summary>
    public partial class GenreItemControl : UserControl
    {
        public GenreItemControl(Genre genre)
        {
            InitializeComponent();

            GenreItem.Id = genre.Id;
            GenreItem.Name = genre.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        public Genre GenreItem => (Genre)Resources["GenreItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new GenresWindow(GenreItem);
            if (editWindow.ShowDialog() == true)
            {
                GenreItem.Name = editWindow.GenreItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.GenresListAdapter.Delete(GenreItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

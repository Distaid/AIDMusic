using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для GenreItemControl.xaml
    /// </summary>
    public partial class GenreItemControl : UserControl
    {
        public Genre GenreItem { get; private set; }

        public GenreItemControl(Genre genre)
        {
            InitializeComponent();

            GenreItem = genre;
            IdText.Text = GenreItem.Id.ToString();
            NameText.Text = GenreItem.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new GenresWindow(GenreItem);
            if (editWindow.ShowDialog() == true)
            {
                GenreItem = editWindow.GenreItem;
                NameText.Text = GenreItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.GenresListAdapter.Delete(GenreItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

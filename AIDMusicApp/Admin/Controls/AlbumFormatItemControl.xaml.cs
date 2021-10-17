using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для AlbumFormatItemControl.xaml
    /// </summary>
    public partial class AlbumFormatItemControl : UserControl
    {
        public AlbumFormatItemControl(AlbumFormat albumFormat)
        {
            InitializeComponent();

            AlbumFormatItem.Id = albumFormat.Id;
            AlbumFormatItem.Name = albumFormat.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        public AlbumFormat AlbumFormatItem => (AlbumFormat)Resources["AlbumFormatItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AlbumFormatsWindow(AlbumFormatItem);
            if (editWindow.ShowDialog() == true)
            {
                AlbumFormatItem.Name = editWindow.AlbumFormatItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.AlbumFormatsListAdapter.Delete(AlbumFormatItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

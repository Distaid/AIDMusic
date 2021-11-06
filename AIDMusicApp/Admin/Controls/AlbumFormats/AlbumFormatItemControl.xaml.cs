using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.AlbumFormats
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
            editWindow.ShowDialog();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumFormatItem.Delete();
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

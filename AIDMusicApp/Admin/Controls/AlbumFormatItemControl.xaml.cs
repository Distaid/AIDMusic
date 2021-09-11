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
        public AlbumFormat AlbumFormatItem { get; private set; }

        public AlbumFormatItemControl(AlbumFormat albumFormat)
        {
            InitializeComponent();

            AlbumFormatItem = albumFormat;
            IdText.Text = AlbumFormatItem.Id.ToString();
            NameText.Text = AlbumFormatItem.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AlbumFormatsWindow(AlbumFormatItem);
            if (editWindow.ShowDialog() == true)
            {
                AlbumFormatItem = editWindow.AlbumFormatItem;
                NameText.Text = AlbumFormatItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.AlbumFormatsListAdapter.Delete(AlbumFormatItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

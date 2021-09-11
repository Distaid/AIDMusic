using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для AlbumFormatsControl.xaml
    /// </summary>
    public partial class AlbumFormatsControl : UserControl
    {
        public AlbumFormatsControl()
        {
            InitializeComponent();

            AddItemButton.Click += AddItemButton_Click;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            SearchButton.Click += SearchButton_Click;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    AddItemButton.IsEnabled = false;
                    foreach (var albumFormat in SqlDatabase.Instance.AlbumFormatsListAdapter.GetAll())
                    {
                        var item = new AlbumFormatItemControl(albumFormat);
                        AlbumFormatsItems.Children.Insert(AlbumFormatsItems.Children.Count - 1, item);
                    }
                    AddItemButton.IsEnabled = true;
                });
            });
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
            {
                foreach (UIElement item in AlbumFormatsItems.Children)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            for (var i = 0; i < AlbumFormatsItems.Children.Count - 1; i++)
            {
                if ((AlbumFormatsItems.Children[i] as AlbumFormatItemControl).AlbumFormatItem.Name.Contains(SearchTextBox.Text))
                    AlbumFormatsItems.Children[i].Visibility = Visibility.Visible;
                else
                    AlbumFormatsItems.Children[i].Visibility = Visibility.Collapsed;
            }

            AlbumFormatsItems.Children[AlbumFormatsItems.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AlbumFormatsWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new AlbumFormatItemControl(addWindow.AlbumFormatItem);
                AlbumFormatsItems.Children.Insert(AlbumFormatsItems.Children.Count - 1, item);
            }
        }
    }
}

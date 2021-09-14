using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System;
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

            Task.Run(async () =>
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    AddItemButton.IsEnabled = false;
                }));
                await Task.Delay(1);

                foreach (var albumFormat in SqlDatabase.Instance.AlbumFormatsListAdapter.GetAll())
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var item = new AlbumFormatItemControl(albumFormat);
                        AlbumFormatsItems.Children.Add(item);
                    }));
                    await Task.Delay(1);
                }

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    AddItemButton.IsEnabled = true;
                }));
                await Task.Delay(1);
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

                AddItemButton.IsEnabled = true;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            foreach (AlbumFormatItemControl item in AlbumFormatsItems.Children)
            {
                if (item.AlbumFormatItem.Name.Contains(SearchTextBox.Text))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Collapsed;
            }

            AddItemButton.IsEnabled = false;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AlbumFormatsWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new AlbumFormatItemControl(addWindow.AlbumFormatItem);
                AlbumFormatsItems.Children.Add(item);
            }
        }
    }
}

using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Genres
{
    /// <summary>
    /// Логика взаимодействия для GenresControl.xaml
    /// </summary>
    public partial class GenresControl : UserControl
    {
        public GenresControl()
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
                    SearchTextBox.IsEnabled = false;
                    SearchButton.IsEnabled = false;
                }));
                await Task.Delay(1);

                foreach (var genre in SqlDatabase.Instance.GenresListAdapter.GetAll())
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var item = new GenreItemControl(genre);
                        GenresItems.Children.Add(item);
                    }));
                    await Task.Delay(1);
                }

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    AddItemButton.IsEnabled = true;
                    SearchTextBox.IsEnabled = true;
                    SearchButton.IsEnabled = true;
                }));
            });
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
            {
                foreach (UIElement item in GenresItems.Children)
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

            foreach (GenreItemControl item in GenresItems.Children)
            {
                if (item.GenreItem.Name.Contains(SearchTextBox.Text))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Collapsed;
            }

            AddItemButton.IsEnabled = false;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new GenresWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new GenreItemControl(addWindow.GenreItem);
                GenresItems.Children.Add(item);
            }
        }
    }
}

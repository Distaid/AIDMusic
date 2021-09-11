using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
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

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    AddItemButton.IsEnabled = false;
                    foreach (var genre in SqlDatabase.Instance.GenresListAdapter.GetAll())
                    {
                        var item = new GenreItemControl(genre);
                        GenresItems.Children.Insert(GenresItems.Children.Count - 1, item);
                    }
                    AddItemButton.IsEnabled = true;
                });
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
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            for (var i = 0; i < GenresItems.Children.Count - 1; i++)
            {
                if ((GenresItems.Children[i] as GenreItemControl).GenreItem.Name.Contains(SearchTextBox.Text))
                    GenresItems.Children[i].Visibility = Visibility.Visible;
                else
                    GenresItems.Children[i].Visibility = Visibility.Collapsed;
            }

            GenresItems.Children[GenresItems.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new GenresWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new GenreItemControl(addWindow.GenreItem);
                GenresItems.Children.Insert(GenresItems.Children.Count - 1, item);
            }
        }
    }
}

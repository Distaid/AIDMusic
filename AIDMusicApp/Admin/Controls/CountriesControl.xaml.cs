using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для CountriesControl.xaml
    /// </summary>
    public partial class CountriesControl : UserControl
    {
        public CountriesControl()
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
                    foreach (var country in SqlDatabase.Instance.CountriesListAdapter.GetAll())
                    {
                        var item = new CountryItemControl(country);
                        CountriesItems.Children.Insert(CountriesItems.Children.Count - 1, item);
                    }
                    AddItemButton.IsEnabled = true;
                });
            });
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
            {
                foreach (UIElement item in CountriesItems.Children)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            for (var i = 0; i < CountriesItems.Children.Count - 1; i++)
            {
                if ((CountriesItems.Children[i] as CountryItemControl).CountryItem.Name.Contains(SearchTextBox.Text))
                    CountriesItems.Children[i].Visibility = Visibility.Visible;
                else
                    CountriesItems.Children[i].Visibility = Visibility.Collapsed;
            }

            CountriesItems.Children[CountriesItems.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new CountriesWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new CountryItemControl(addWindow.CountryItem);
                CountriesItems.Children.Insert(CountriesItems.Children.Count - 1, item);
            }
        }
    }
}

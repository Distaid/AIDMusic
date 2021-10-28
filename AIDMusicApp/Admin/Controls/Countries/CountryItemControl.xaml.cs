using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Countries
{
    /// <summary>
    /// Логика взаимодействия для CountryItemControl.xaml
    /// </summary>
    public partial class CountryItemControl : UserControl
    {
        public CountryItemControl(Country country)
        {
            InitializeComponent();

            CountryItem.Id = country.Id;
            CountryItem.Name = country.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        public Country CountryItem => (Country)Resources["CountryItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new CountriesWindow(CountryItem);
            if (editWindow.ShowDialog() == true)
            {
                CountryItem.Name = editWindow.CountryItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.CountriesListAdapter.Delete(CountryItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

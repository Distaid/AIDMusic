using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
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
    /// Логика взаимодействия для CountryItemControl.xaml
    /// </summary>
    public partial class CountryItemControl : UserControl
    {
        public Country CountryItem { get; private set; }

        public CountryItemControl(Country country)
        {
            InitializeComponent();

            CountryItem = country;
            IdText.Text = CountryItem.Id.ToString();
            NameText.Text = CountryItem.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new CountriesWindow(CountryItem);
            if (editWindow.ShowDialog() == true)
            {
                CountryItem = editWindow.CountryItem;
                NameText.Text = CountryItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.CountriesListAdapter.Delete(CountryItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
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
using System.Windows.Shapes;

namespace AIDMusicApp.Admin.Windows
{
    /// <summary>
    /// Логика взаимодействия для CountriesWindow.xaml
    /// </summary>
    public partial class CountriesWindow : Window
    {
        public Country CountryItem = null;

        public CountriesWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameTextAdd.Focus();
            TitleText.Text = "Добавление Страны";
            AddPanel.Visibility = Visibility.Visible;

            AddButton.Click += AddButton_Click;
        }

        public CountriesWindow(Country country)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            CountryItem = country.Copy();
            NameTextEdit.Text = CountryItem.Name;
            NameTextEdit.Focus();
            NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
            TitleText.Text = "Изменение Страны";
            EditPanel.Visibility = Visibility.Visible;

            EditButton.Click += EditButton_Click;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Поле должно быть заполнено!");
                NameTextAdd.Text = "";
                NameTextAdd.Focus();
                return;
            }

            if (SqlDatabase.Instance.CountriesListAdapter.ContainsName(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            var id = SqlDatabase.Instance.CountriesListAdapter.Insert(NameTextAdd.Text);

            DialogResult = true;
            CountryItem = new Country { Id = id, Name = NameTextAdd.Text };
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Поле должно быть заполнено!");
                NameTextAdd.Text = "";
                NameTextAdd.Focus();
                return;
            }

            if (CountryItem.Name == NameTextEdit.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.CountriesListAdapter.ContainsName(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            SqlDatabase.Instance.CountriesListAdapter.Update(CountryItem.Id, NameTextEdit.Text);

            DialogResult = true;
            CountryItem.Name = NameTextEdit.Text;
        }
    }
}

using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System.Windows;
using System.Windows.Input;

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
            NameText.Focus();
            TitleText.Text = "Добавление Страны";

            ConfirmButton.Content = "Добавить";
            ConfirmButton.Click += AddButton_Click;
        }

        public CountriesWindow(Country country)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            CountryItem = country;
            NameText.Text = CountryItem.Name;
            NameText.Focus();
            NameText.CaretIndex = NameText.Text.Length;
            TitleText.Text = "Изменение Страны";

            ConfirmButton.Content = "Изменить";
            ConfirmButton.Click += EditButton_Click;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText.Text))
            {
                AIDMessageWindow.Show("Поле должно быть заполнено!");
                NameText.Text = "";
                NameText.Focus();
                return;
            }

            if (SqlDatabase.Instance.CountriesListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            CountryItem = SqlDatabase.Instance.CountriesListAdapter.Insert(NameText.Text);

            DialogResult = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText.Text))
            {
                AIDMessageWindow.Show("Поле должно быть заполнено!");
                NameText.Text = "";
                NameText.Focus();
                return;
            }

            if (CountryItem.Name == NameText.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.CountriesListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            CountryItem.Update(NameText.Text);

            DialogResult = true;
        }
    }
}

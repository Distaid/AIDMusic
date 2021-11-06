using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System.Windows;
using System.Windows.Input;

namespace AIDMusicApp.Admin.Windows
{
    /// <summary>
    /// Логика взаимодействия для GenresWindow.xaml
    /// </summary>
    public partial class GenresWindow : Window
    {
        public Genre GenreItem = null;

        public GenresWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameText.Focus();
            TitleText.Text = "Добавление Жанра";

            ConfirmButton.Content = "Добавить";
            ConfirmButton.Click += AddButton_Click;
        }

        public GenresWindow(Genre genre)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            GenreItem = genre;
            NameText.Text = GenreItem.Name;
            NameText.Focus();
            NameText.CaretIndex = NameText.Text.Length;
            TitleText.Text = "Изменение Жанра";

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

            if (SqlDatabase.Instance.GenresListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            GenreItem = SqlDatabase.Instance.GenresListAdapter.Insert(NameText.Text);

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

            if (GenreItem.Name == NameText.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.GenresListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            GenreItem.Update(NameText.Text);

            DialogResult = true;
        }
    }
}

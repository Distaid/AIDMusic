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
            NameTextAdd.Focus();
            TitleText.Text = "Добавление Жанра";
            AddPanel.Visibility = Visibility.Visible;

            AddButton.Click += AddButton_Click;
        }

        public GenresWindow(Genre genre)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            GenreItem = genre.Copy();
            NameTextEdit.Text = GenreItem.Name;
            NameTextEdit.Focus();
            NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
            TitleText.Text = "Изменение Жанра";
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

            if (SqlDatabase.Instance.GenresListAdapter.ContainsName(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            var id = SqlDatabase.Instance.GenresListAdapter.Insert(NameTextAdd.Text);

            DialogResult = true;
            GenreItem = new Genre { Id = id, Name = NameTextAdd.Text };
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

            if (GenreItem.Name == NameTextEdit.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.GenresListAdapter.ContainsName(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            SqlDatabase.Instance.GenresListAdapter.Update(GenreItem.Id, NameTextEdit.Text);

            DialogResult = true;
            GenreItem.Name = NameTextEdit.Text;
        }
    }
}

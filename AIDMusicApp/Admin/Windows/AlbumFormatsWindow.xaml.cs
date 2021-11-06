using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System.Windows;
using System.Windows.Input;

namespace AIDMusicApp.Admin.Windows
{
    /// <summary>
    /// Логика взаимодействия для AlbumFormatsWindow.xaml
    /// </summary>
    public partial class AlbumFormatsWindow : Window
    {
        public AlbumFormat AlbumFormatItem = null;

        public AlbumFormatsWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameText.Focus();
            TitleText.Text = "Добавление Формата альбома";

            ConfirmButton.Content = "Добавить";
            ConfirmButton.Click += AddButton_Click;
        }

        public AlbumFormatsWindow(AlbumFormat albumFormat)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            AlbumFormatItem = albumFormat;
            NameText.Text = AlbumFormatItem.Name;
            NameText.Focus();
            NameText.CaretIndex = NameText.Text.Length;
            TitleText.Text = "Изменение Формата альбома";

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

            if (SqlDatabase.Instance.AlbumFormatsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            AlbumFormatItem = SqlDatabase.Instance.AlbumFormatsListAdapter.Insert(NameText.Text);

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

            if (AlbumFormatItem.Name == NameText.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.AlbumFormatsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            AlbumFormatItem.Update(NameText.Text);

            DialogResult = true;
        }
    }
}

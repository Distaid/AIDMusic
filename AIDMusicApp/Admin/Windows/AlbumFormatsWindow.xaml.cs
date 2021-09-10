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
            NameTextAdd.Focus();
            TitleText.Text = "Добавление Формата альбома";
            AddPanel.Visibility = Visibility.Visible;

            AddButton.Click += AddButton_Click;
        }

        public AlbumFormatsWindow(AlbumFormat albumFormat)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            AlbumFormatItem = albumFormat.Copy();
            NameTextEdit.Text = AlbumFormatItem.Name;
            NameTextEdit.Focus();
            NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
            TitleText.Text = "Изменение Формата альбома";
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

            if (SqlDatabase.Instance.AlbumFormatsListAdapter.ContainsName(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            var id = SqlDatabase.Instance.AlbumFormatsListAdapter.Insert(NameTextAdd.Text);

            DialogResult = true;
            AlbumFormatItem = new AlbumFormat { Id = id, Name = NameTextAdd.Text };
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

            if (AlbumFormatItem.Name == NameTextEdit.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.AlbumFormatsListAdapter.ContainsName(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            SqlDatabase.Instance.AlbumFormatsListAdapter.Update(AlbumFormatItem.Id, NameTextEdit.Text);

            DialogResult = true;
            AlbumFormatItem.Name = NameTextEdit.Text;
        }
    }
}

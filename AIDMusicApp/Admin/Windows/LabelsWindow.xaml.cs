using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System.Windows;
using System.Windows.Input;

namespace AIDMusicApp.Admin.Windows
{
    /// <summary>
    /// Логика взаимодействия для LabelsWindow.xaml
    /// </summary>
    public partial class LabelsWindow : Window
    {
        public Models.Label LabelItem = null;

        public LabelsWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameText.Focus();
            TitleText.Text = "Добавление Лэйбла";

            ConfirmButton.Content = "Добавить";
            ConfirmButton.Click += AddButton_Click;
        }

        public LabelsWindow(Models.Label label)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            LabelItem = label;
            NameText.Text = LabelItem.Name;
            NameText.Focus();
            NameText.CaretIndex = NameText.Text.Length;
            TitleText.Text = "Изменение Лэйбла";

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

            if (SqlDatabase.Instance.LabelsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            LabelItem = SqlDatabase.Instance.LabelsListAdapter.Insert(NameText.Text);

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

            if (LabelItem.Name == NameText.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.LabelsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            LabelItem.Update(NameText.Text);

            DialogResult = true;
        }
    }
}

using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System.Windows;
using System.Windows.Input;

namespace AIDMusicApp.Admin.Windows
{
    /// <summary>
    /// Логика взаимодействия для SkillsWindow.xaml
    /// </summary>
    public partial class SkillsWindow : Window
    {
        public Skill SkillItem = null;

        public SkillsWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameTextAdd.Focus();
            TitleText.Text = "Добавление Жанра";
            AddPanel.Visibility = Visibility.Visible;

            AddButton.Click += AddButton_Click;
        }

        public SkillsWindow(Skill skill)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            SkillItem = skill.Copy();
            NameTextEdit.Text = SkillItem.Name;
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

            if (SqlDatabase.Instance.SkillsListAdapter.ContainsName(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            var id = SqlDatabase.Instance.SkillsListAdapter.Insert(NameTextAdd.Text);

            DialogResult = true;
            SkillItem = new Skill { Id = id, Name = NameTextAdd.Text };
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

            if (SkillItem.Name == NameTextEdit.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.SkillsListAdapter.ContainsName(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            SqlDatabase.Instance.SkillsListAdapter.Update(SkillItem.Id, NameTextEdit.Text);

            DialogResult = true;
            SkillItem.Name = NameTextEdit.Text;
        }
    }
}

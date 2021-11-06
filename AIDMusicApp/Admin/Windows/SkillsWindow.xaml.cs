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
            NameText.Focus();
            TitleText.Text = "Добавление Навыка";

            ConfirmButton.Content = "Добавить";
            ConfirmButton.Click += AddButton_Click;
        }

        public SkillsWindow(Skill skill)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            SkillItem = skill;
            NameText.Text = SkillItem.Name;
            NameText.Focus();
            NameText.CaretIndex = NameText.Text.Length;
            TitleText.Text = "Изменение Навыка";

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

            if (SqlDatabase.Instance.SkillsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            SkillItem = SqlDatabase.Instance.SkillsListAdapter.Insert(NameText.Text);

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

            if (SkillItem.Name == NameText.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.SkillsListAdapter.ContainsName(NameText.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameText.Focus();
                NameText.CaretIndex = NameText.Text.Length;
                return;
            }

            SkillItem.Update(NameText.Text);

            DialogResult = true;
        }
    }
}

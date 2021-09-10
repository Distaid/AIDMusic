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
    /// Логика взаимодействия для LabelsWindow.xaml
    /// </summary>
    public partial class LabelsWindow : Window
    {
        public Models.Label LabelItem = null;

        public LabelsWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            NameTextAdd.Focus();
            TitleText.Text = "Добавление Жанра";
            AddPanel.Visibility = Visibility.Visible;

            AddButton.Click += AddButton_Click;
        }

        public LabelsWindow(Models.Label label)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            LabelItem = label.Copy();
            NameTextEdit.Text = LabelItem.Name;
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

            if (SqlDatabase.Instance.LabelsListAdapter.ContainsName(NameTextAdd.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            var id = SqlDatabase.Instance.LabelsListAdapter.Insert(NameTextAdd.Text);

            DialogResult = true;
            LabelItem = new Models.Label { Id = id, Name = NameTextAdd.Text };
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

            if (LabelItem.Name == NameTextEdit.Text)
            {
                DialogResult = false;
                return;
            }

            if (SqlDatabase.Instance.LabelsListAdapter.ContainsName(NameTextEdit.Text))
            {
                AIDMessageWindow.Show("Страна с таким названием уже существует!");
                NameTextAdd.Focus();
                NameTextEdit.CaretIndex = NameTextEdit.Text.Length;
                return;
            }

            SqlDatabase.Instance.SkillsListAdapter.Update(LabelItem.Id, NameTextEdit.Text);

            DialogResult = true;
            LabelItem.Name = NameTextEdit.Text;
        }
    }
}

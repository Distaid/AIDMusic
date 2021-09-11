using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для SkillItemControl.xaml
    /// </summary>
    public partial class SkillItemControl : UserControl
    {
        public Skill SkillItem { get; private set; }

        public SkillItemControl(Skill skill)
        {
            InitializeComponent();

            SkillItem = skill;
            IdText.Text = SkillItem.Id.ToString();
            NameText.Text = SkillItem.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new SkillsWindow(SkillItem);
            if (editWindow.ShowDialog() == true)
            {
                SkillItem = editWindow.SkillItem;
                NameText.Text = SkillItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.SkillsListAdapter.Delete(SkillItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

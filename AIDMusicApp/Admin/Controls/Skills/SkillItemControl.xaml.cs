using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Skills
{
    /// <summary>
    /// Логика взаимодействия для SkillItemControl.xaml
    /// </summary>
    public partial class SkillItemControl : UserControl
    {
        public SkillItemControl(Skill skill)
        {
            InitializeComponent();

            SkillItem.Id = skill.Id;
            SkillItem.Name = skill.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        public Skill SkillItem => (Skill)Resources["SkillItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new SkillsWindow(SkillItem);
            editWindow.ShowDialog();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SkillItem.Delete();
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

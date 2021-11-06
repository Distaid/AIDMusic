using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Musicians
{
    /// <summary>
    /// Логика взаимодействия для MusicianSkillItemControl.xaml
    /// </summary>
    public partial class MusicianSkillItemControl : UserControl
    {
        public MusicianSkillItemControl()
        {
            InitializeComponent();

            RemoveButton.Click += RemoveButton_Click;

            foreach (var skill in SqlDatabase.Instance.SkillsListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = skill.Name, Tag = skill };
                SkillComboBox.Items.Add(item);
            }
        }

        public MusicianSkillItemControl(int skillId)
        {
            InitializeComponent();

            RemoveButton.Click += RemoveButton_Click;

            foreach (var skill in SqlDatabase.Instance.SkillsListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = skill.Name, Tag = skill };
                if (skill.Id == skillId)
                    item.IsSelected = true;
                SkillComboBox.Items.Add(item);
            }
        }

        public Skill SkillItem => (Skill)((ComboBoxItem)SkillComboBox.SelectedItem)?.Tag ?? null;

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (Parent as StackPanel).Children.Remove(this);
        }
    }
}

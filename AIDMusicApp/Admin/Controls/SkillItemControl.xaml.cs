﻿using AIDMusicApp.Admin.Windows;
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
            if (editWindow.ShowDialog() == true)
            {
                SkillItem.Name = editWindow.SkillItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.SkillsListAdapter.Delete(SkillItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

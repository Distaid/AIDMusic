using AIDMusicApp.Models;
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
    /// Логика взаимодействия для MusiciansWindow.xaml
    /// </summary>
    public partial class MusiciansWindow : Window
    {
        public Musician MusicianItem { get; private set; } = null;

        public MusiciansWindow()
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            AgeText.PreviewTextInput += AgeText_PreviewTextInput;
            TitleText.Text = "Добавление Музыканта";
            ConfirmButton.Content = "Добавить";

            AddSkill.Click += AddSkill_Click;
            RemoveSkill.Click += RemoveSkill_Click;
            ConfirmButton.Click += AddButton_Click;

            foreach (var country in SqlDatabase.Instance.CountriesListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = country.Name, Tag = country };
                CountryId.Items.Add(item);
            }

            foreach (var skill in SqlDatabase.Instance.SkillsListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = skill.Name, Tag = skill };
                SkillId.Items.Add(item);
            }
        }

        public MusiciansWindow(Musician musician)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            AgeText.PreviewTextInput += AgeText_PreviewTextInput;
            TitleText.Text = "Редактирование Музыканта";
            ConfirmButton.Content = "Изменить";
            MusicianItem = musician.Copy();
            NameText.Text = MusicianItem.Name;
            AgeText.Text = MusicianItem.Age.ToString();
            IsDeadText.SelectedIndex = Convert.ToInt32(MusicianItem.IsDead);

            AddSkill.Click += AddSkill_Click;
            RemoveSkill.Click += RemoveSkill_Click;
            ConfirmButton.Click += ChangeButton_Click;

            foreach (var country in SqlDatabase.Instance.CountriesListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = country.Name, Tag = country };
                CountryId.Items.Add(item);
                if (MusicianItem.CountryId == country.Id)
                    item.IsSelected = true;
            }

            foreach (var skill in SqlDatabase.Instance.SkillsListAdapter.GetAll())
            {
                if (MusicianItem.Skills.FindIndex(s => skill.Id == s.Id) != -1)
                {
                    var item = new ListBoxItem { Content = skill.Name, Tag = skill };
                    SkillsItems.Items.Add(item);
                }
                else
                {
                    var item = new ComboBoxItem { Content = skill.Name, Tag = skill };
                    SkillId.Items.Add(item);
                }
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void AgeText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            if (SkillId.SelectedIndex != -1)
            {
                var item = SkillId.SelectedItem as ComboBoxItem;
                SkillsItems.Items.Add(new ListBoxItem { Content = item.Content, Tag = item.Tag });
                SkillId.Items.Remove(item);
            }
        }

        private void RemoveSkill_Click(object sender, RoutedEventArgs e)
        {
            if (SkillsItems.SelectedIndex != -1)
            {
                var item = SkillsItems.SelectedItem as ListBoxItem;
                SkillId.Items.Add(new ComboBoxItem { Content = item.Content, Tag = item.Tag });
                SkillsItems.Items.Remove(item);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText.Text))
            {
                AIDMessageWindow.Show("Поле \"Имя\" должно быть заполнено!");
                return;
            }

            if (string.IsNullOrWhiteSpace(AgeText.Text))
            {
                AIDMessageWindow.Show("Поле \"Возраст\" должно быть заполнено!");
                return;
            }

            if (CountryId.SelectedIndex == -1)
            {
                AIDMessageWindow.Show("Поле \"Страна\" должно быть заполнено!");
                return;
            }

            if (SkillsItems.Items.Count == 0)
            {
                AIDMessageWindow.Show("Поле \"Навыки\" должно содержать хотя бы один элемент!");
                return;
            }

            MusicianItem = new Musician
            {
                Name = NameText.Text,
                Age = Convert.ToByte(AgeText.Text),
                CountryId = ((CountryId.SelectedItem as ComboBoxItem).Tag as Country).Id,
                IsDead = Convert.ToBoolean(IsDeadText.SelectedIndex),
                Skills = new List<Skill>()
            };

            MusicianItem.Id = SqlDatabase.Instance.MusiciansAdapter.Insert(MusicianItem.Name, MusicianItem.Age, MusicianItem.CountryId, MusicianItem.IsDead);

            foreach (ListBoxItem item in SkillsItems.Items)
            {
                var skill = item.Tag as Skill;
                MusicianItem.Skills.Add(skill);
                SqlDatabase.Instance.MusicianSkillsAdapter.Insert(MusicianItem.Id, skill.Id);
            }

            DialogResult = true;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText.Text))
            {
                AIDMessageWindow.Show("Поле \"Имя\" должно быть заполнено!");
                return;
            }

            if (string.IsNullOrWhiteSpace(AgeText.Text))
            {
                AIDMessageWindow.Show("Поле \"Возраст\" должно быть заполнено!");
                return;
            }

            if (SkillsItems.Items.Count == 0)
            {
                AIDMessageWindow.Show("Поле \"Навыки\" должно содержать хотя бы один элемент!");
                return;
            }

            SqlDatabase.Instance.MusiciansAdapter.Update(MusicianItem.Id, NameText.Text, Convert.ToByte(AgeText.Text), ((CountryId.SelectedItem as ComboBoxItem).Tag as Country).Id, Convert.ToBoolean(IsDeadText.SelectedIndex));

            foreach (ListBoxItem item in SkillsItems.Items)
            {
                var skill = item.Tag as Skill;
                var i = MusicianItem.Skills.FindIndex((s) => skill.Id == s.Id);

                if (i != -1)
                    MusicianItem.Skills.RemoveAt(i);
                else
                    SqlDatabase.Instance.MusicianSkillsAdapter.Insert(MusicianItem.Id, skill.Id);
            }

            foreach (var skill in MusicianItem.Skills)
            {
                var id = SqlDatabase.Instance.MusicianSkillsAdapter.GetIdByMusicianIdAndSkillId(MusicianItem.Id, skill.Id);
                SqlDatabase.Instance.MusicianSkillsAdapter.Delete(id);
            }
            MusicianItem.Skills.Clear();

            foreach (ListBoxItem item in SkillsItems.Items)
            {
                var skill = item.Tag as Skill;
                MusicianItem.Skills.Add(skill);
            }

            MusicianItem.Name = NameText.Text;
            MusicianItem.Age = Convert.ToByte(AgeText.Text);
            MusicianItem.CountryId = ((CountryId.SelectedItem as ComboBoxItem).Tag as Country).Id;
            MusicianItem.IsDead = Convert.ToBoolean(IsDeadText.SelectedIndex);

            DialogResult = true;
        }

    }
}

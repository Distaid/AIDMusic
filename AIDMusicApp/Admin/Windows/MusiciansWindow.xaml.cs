using AIDMusicApp.Admin.Controls.Musicians;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            ConfirmButton.Click += AddButton_Click;

            foreach (var country in SqlDatabase.Instance.CountriesListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = country.Name, Tag = country };
                CountryId.Items.Add(item);
            }
        }

        public MusiciansWindow(Musician musician)
        {
            InitializeComponent();

            TitleBar.MouseDown += TitleBar_MouseDown;
            AgeText.PreviewTextInput += AgeText_PreviewTextInput;
            TitleText.Text = "Редактирование Музыканта";
            ConfirmButton.Content = "Изменить";
            MusicianItem = musician;
            NameText.Text = MusicianItem.Name;
            AgeText.Text = MusicianItem.Age.ToString();
            IsDeadText.SelectedIndex = Convert.ToInt32(MusicianItem.IsDead);

            AddSkill.Click += AddSkill_Click;
            ConfirmButton.Click += ChangeButton_Click;

            foreach (var country in SqlDatabase.Instance.CountriesListAdapter.GetAll())
            {
                var item = new ComboBoxItem { Content = country.Name, Tag = country };
                CountryId.Items.Add(item);
                if (MusicianItem.CountryId.Id == country.Id)
                    item.IsSelected = true;
            }

            foreach (var skill in SqlDatabase.Instance.MusicianSkillsAdapter.GetSkillsByMusicianId(MusicianItem.Id))
            {
                var item = new MusicianSkillItemControl(skill.Id);
                SkillsItems.Children.Insert(SkillsItems.Children.Count - 1, item);
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void AgeText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !byte.TryParse(e.Text, out _);
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            if (SkillsItems.Children.Count != 1)
            {
                var skillItemControl = SkillsItems.Children[SkillsItems.Children.Count - 2] as MusicianSkillItemControl;
                if (skillItemControl.SkillItem == null)
                    return;
            }

            var item = new MusicianSkillItemControl();
            SkillsItems.Children.Insert(SkillsItems.Children.Count - 1, item);
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

            if (SkillsItems.Children.Count == 1)
            {
                AIDMessageWindow.Show("Поле \"Навыки\" должно содержать хотя бы один элемент!");
                return;
            }

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                for (var j = i + 1; j < SkillsItems.Children.Count - 1; j++)
                {
                    if ((SkillsItems.Children[i] as MusicianSkillItemControl).SkillItem.Id == (SkillsItems.Children[j] as MusicianSkillItemControl).SkillItem.Id)
                    {
                        AIDMessageWindow.Show("Навыки не должны повторяться!");
                        return;
                    }
                }
            }

            var countryId = (Country)(CountryId.SelectedItem as ComboBoxItem).Tag;

            MusicianItem = SqlDatabase.Instance.MusiciansAdapter.Insert(NameText.Text, Convert.ToByte(AgeText.Text), countryId.Id, Convert.ToBoolean(IsDeadText.SelectedIndex));

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                var skill = (SkillsItems.Children[i] as MusicianSkillItemControl).SkillItem;
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

            if (SkillsItems.Children.Count == 1)
            {
                AIDMessageWindow.Show("Поле \"Навыки\" должно содержать хотя бы один элемент!");
                return;
            }

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                for (var j = i + 1; j < SkillsItems.Children.Count - 1; j++)
                {
                    if ((SkillsItems.Children[i] as MusicianSkillItemControl).SkillItem.Id == (SkillsItems.Children[j] as MusicianSkillItemControl).SkillItem.Id)
                    {
                        AIDMessageWindow.Show("Навыки не должны повторяться!");
                        return;
                    }
                }
            }

            var countryId = (Country)(CountryId.SelectedItem as ComboBoxItem).Tag;

            MusicianItem.Update(NameText.Text, Convert.ToByte(AgeText.Text), countryId, Convert.ToBoolean(IsDeadText.SelectedIndex));

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                var skill = (SkillsItems.Children[i] as MusicianSkillItemControl).SkillItem;

                var j = 0;
                for (; j < MusicianItem.Skills.Count; j++)
                {
                    if (MusicianItem.Skills[i].Id == skill.Id)
                        break;
                }

                if (j != MusicianItem.Skills.Count)
                    MusicianItem.Skills.RemoveAt(j);
                else
                    SqlDatabase.Instance.MusicianSkillsAdapter.Insert(MusicianItem.Id, skill.Id);
            }

            foreach (var skill in MusicianItem.Skills)
            {
                var id = SqlDatabase.Instance.MusicianSkillsAdapter.GetIdByMusicianIdAndSkillId(MusicianItem.Id, skill.Id);
                SqlDatabase.Instance.MusicianSkillsAdapter.Delete(id);
            }

            MusicianItem.Skills.Clear();

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                var skill = (SkillsItems.Children[i] as MusicianSkillItemControl).SkillItem;
                MusicianItem.Skills.Add(skill);
            }

            DialogResult = true;
        }

    }
}

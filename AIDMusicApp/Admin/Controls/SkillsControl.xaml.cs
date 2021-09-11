using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для SkillsControl.xaml
    /// </summary>
    public partial class SkillsControl : UserControl
    {
        public SkillsControl()
        {
            InitializeComponent();

            AddItemButton.Click += AddItemButton_Click;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            SearchButton.Click += SearchButton_Click;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    AddItemButton.IsEnabled = false;
                    foreach (var skill in SqlDatabase.Instance.SkillsListAdapter.GetAll())
                    {
                        var item = new SkillItemControl(skill);
                        SkillsItems.Children.Insert(SkillsItems.Children.Count - 1, item);
                    }
                    AddItemButton.IsEnabled = true;
                });
            });
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
            {
                foreach (UIElement item in SkillsItems.Children)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            for (var i = 0; i < SkillsItems.Children.Count - 1; i++)
            {
                if ((SkillsItems.Children[i] as SkillItemControl).SkillItem.Name.Contains(SearchTextBox.Text))
                    SkillsItems.Children[i].Visibility = Visibility.Visible;
                else
                    SkillsItems.Children[i].Visibility = Visibility.Collapsed;
            }

            SkillsItems.Children[SkillsItems.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new SkillsWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new SkillItemControl(addWindow.SkillItem);
                SkillsItems.Children.Insert(SkillsItems.Children.Count - 1, item);
            }
        }
    }
}

using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для LabelsControl.xaml
    /// </summary>
    public partial class LabelsControl : UserControl
    {
        public LabelsControl()
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
                    foreach (var label in SqlDatabase.Instance.LabelsListAdapter.GetAll())
                    {
                        var item = new LabelItemControl(label);
                        LabelsItems.Children.Insert(LabelsItems.Children.Count - 1, item);
                    }
                    AddItemButton.IsEnabled = true;
                });
            });
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
            {
                foreach (UIElement item in LabelsItems.Children)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                return;

            for (var i = 0; i < LabelsItems.Children.Count - 1; i++)
            {
                if ((LabelsItems.Children[i] as LabelItemControl).LabelItem.Name.Contains(SearchTextBox.Text))
                    LabelsItems.Children[i].Visibility = Visibility.Visible;
                else
                    LabelsItems.Children[i].Visibility = Visibility.Collapsed;
            }

            LabelsItems.Children[LabelsItems.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new LabelsWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new LabelItemControl(addWindow.LabelItem);
                LabelsItems.Children.Insert(LabelsItems.Children.Count - 1, item);
            }
        }
    }
}

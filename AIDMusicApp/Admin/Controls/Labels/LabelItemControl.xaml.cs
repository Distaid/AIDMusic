using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls.Labels
{
    /// <summary>
    /// Логика взаимодействия для LabelItemControl.xaml
    /// </summary>
    public partial class LabelItemControl : UserControl
    {
        public LabelItemControl(Models.Label label)
        {
            InitializeComponent();

            LabelItem.Id = label.Id;
            LabelItem.Name = label.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        public Models.Label LabelItem => (Models.Label)Resources["LabelItem"];

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new LabelsWindow(LabelItem);
            editWindow.ShowDialog();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            LabelItem.Delete();
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

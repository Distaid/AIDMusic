using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для LabelItemControl.xaml
    /// </summary>
    public partial class LabelItemControl : UserControl
    {
        public Models.Label LabelItem { get; private set; }

        public LabelItemControl(Models.Label label)
        {
            InitializeComponent();

            LabelItem = label;
            IdText.Text = LabelItem.Id.ToString();
            NameText.Text = LabelItem.Name;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new LabelsWindow(LabelItem);
            if (editWindow.ShowDialog() == true)
            {
                LabelItem = editWindow.LabelItem;
                NameText.Text = LabelItem.Name;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.LabelsListAdapter.Delete(LabelItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Sql;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для MusiciansControl.xaml
    /// </summary>
    public partial class MusiciansControl : UserControl
    {
        public MusiciansControl()
        {
            InitializeComponent();

            AddItemButton.Click += AddItemButton_Click;

            Task.Run(async () =>
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    AddItemButton.IsEnabled = false;
                    SearchTextBox.IsEnabled = false;
                    SearchButton.IsEnabled = false;
                    //SearchField.IsEnabled = false;
                }));
                await Task.Delay(1);

                foreach (var musician in SqlDatabase.Instance.MusiciansAdapter.GetAll())
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var item = new MusicianItemControl(musician);
                        MusiciansItems.Children.Add(item);
                    }));
                    await Task.Delay(1);
                }

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    AddItemButton.IsEnabled = true;
                    SearchTextBox.IsEnabled = true;
                    SearchButton.IsEnabled = true;
                    //SearchField.IsEnabled = true;
                }));
                await Task.Delay(1);
            });
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new MusiciansWindow();
            if (addWindow.ShowDialog() == true)
            {
                var item = new MusicianItemControl(addWindow.MusicianItem);
                MusiciansItems.Children.Add(item);
            }
        }
    }
}

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
    /// Логика взаимодействия для AdminMainControl.xaml
    /// </summary>
    public partial class AdminMainControl : UserControl
    {
        public AdminMainControl()
        {
            InitializeComponent();

            CountriesButton.Click += CountriesButton_Click;
        }

        private void CountriesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainContent.Content is CountriesControl))
                MainContent.Content = new CountriesControl();
        }
    }
}

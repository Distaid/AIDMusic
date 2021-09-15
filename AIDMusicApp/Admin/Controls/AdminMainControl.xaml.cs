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

            CountriesButton.Click += ChangePanel;
            GenresButton.Click += ChangePanel;
            LabelsButton.Click += ChangePanel;
            SkillsButton.Click += ChangePanel;
            AlbumFormatsButton.Click += ChangePanel;
            UsersButton.Click += ChangePanel;
        }

        private void ChangePanel(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;

            switch (item.Name)
            {
                case "CountriesButton":
                    if (!(MainContent.Content is CountriesControl))
                        MainContent.Content = new CountriesControl();
                    break;

                case "GenresButton":
                    if (!(MainContent.Content is GenresControl))
                        MainContent.Content = new GenresControl();
                    break;

                case "LabelsButton":
                    if (!(MainContent.Content is LabelsControl))
                        MainContent.Content = new LabelsControl();
                    break;

                case "SkillsButton":
                    if (!(MainContent.Content is SkillsControl))
                        MainContent.Content = new SkillsControl();
                    break;

                case "AlbumFormatsButton":
                    if (!(MainContent.Content is AlbumFormatsControl))
                        MainContent.Content = new AlbumFormatsControl();
                    break;

                case "UsersButton":
                    if (!(MainContent.Content is UsersControl))
                        MainContent.Content = new UsersControl();
                    break;
            }
        }
    }
}

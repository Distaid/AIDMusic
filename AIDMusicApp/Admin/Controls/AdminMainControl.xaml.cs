using AIDMusicApp.Admin.Controls.AlbumFormats;
using AIDMusicApp.Admin.Controls.Countries;
using AIDMusicApp.Admin.Controls.Genres;
using AIDMusicApp.Admin.Controls.Labels;
using AIDMusicApp.Admin.Controls.Musicians;
using AIDMusicApp.Admin.Controls.Skills;
using AIDMusicApp.Admin.Controls.Users;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Admin.Controls
{
    /// <summary>
    /// Логика взаимодействия для AdminMainControl.xaml
    /// </summary>
    public partial class AdminMainControl : UserControl
    {
        public event Action ExitClick = null;

        public AdminMainControl()
        {
            InitializeComponent();

            ExitButton.Click += ExitButton_Click;

            CountriesButton.Click += ChangePanel;
            GenresButton.Click += ChangePanel;
            LabelsButton.Click += ChangePanel;
            SkillsButton.Click += ChangePanel;
            AlbumFormatsButton.Click += ChangePanel;
            UsersButton.Click += ChangePanel;
            MusiciansButton.Click += ChangePanel;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitClick?.Invoke();
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

                case "MusiciansButton":
                    if (!(MainContent.Content is MusiciansControl))
                        MainContent.Content = new MusiciansControl();
                    break;
            }
        }
    }
}

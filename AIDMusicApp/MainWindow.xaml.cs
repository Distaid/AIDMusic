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

namespace AIDMusicApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            TitleBar.MouseDown += TitleBar_MouseDown;
            TitleHideButton.Click += TitleHideButton_Click;
            TitleMaximizeButton.Click += TitleMaximizeButton_Click;
            TitleRestoreButton.Click += TitleRestoreButton_Click;
            TitleCloseButton.Click += TitleCloseButton_Click;
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                TitleRestoreButton.Visibility = Visibility.Visible;
                TitleMaximizeButton.Visibility = Visibility.Hidden;
            }
            else if (WindowState == WindowState.Normal)
            {
                TitleRestoreButton.Visibility = Visibility.Hidden;
                TitleMaximizeButton.Visibility = Visibility.Visible;
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TitleHideButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TitleMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void TitleRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void TitleCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

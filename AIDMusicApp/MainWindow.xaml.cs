using AIDMusicApp.Admin.Controls;
using AIDMusicApp.Controls;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private EnterControl _enterControl = null;

        private RegistrationControl _registrationControl = null;

        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MainContent.Content = new LoadingControl();

            TitleBar.MouseDown += TitleBar_MouseDown;
            TitleHideButton.Click += TitleHideButton_Click;
            TitleMaximizeButton.Click += TitleMaximizeButton_Click;
            TitleRestoreButton.Click += TitleRestoreButton_Click;
            TitleCloseButton.Click += TitleCloseButton_Click;
            StateChanged += MainWindow_StateChanged;
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
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
            if (e.ChangedButton == MouseButton.Left)
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(delegate
                    {
                        (MainContent.Content as LoadingControl).LoadingText.Text = "Загрузка файла SQL настроек и подключение к БД";
                    });
                    SqlDatabase.Initialize();

                    Dispatcher.Invoke(delegate
                    {
                        (MainContent.Content as LoadingControl).LoadingText.Text = "Загрузка файлов SQL команд";
                    });
                    SqlDatabase.Instance.LoadComands();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Dispatcher.Invoke(delegate
                    {
                        Application.Current.Shutdown();
                    });
                    return;
                }

                Dispatcher.Invoke(delegate
                {
                    _enterControl = new EnterControl();
                    _enterControl.LoginClick += _enterControl_LoginClick;
                    _enterControl.RegistrationClick += _enterControl_RegistrationClick;
                    MainContent.Content = _enterControl;
                });
            });
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            SqlDatabase.Instance.CloseConnection();
        }

        private void _enterControl_LoginClick(User user)
        {
            switch (user.AccessId.Name)
            {
                case "Администратор":
                    MainContent.Content = null;
                    MainContent.Content = new AdminMainControl();
                    (MainContent.Content as AdminMainControl).ExitClick += MainWindow_ExitClick;
                    break;
                default:
                    break;
            }
        }

        private void MainWindow_ExitClick()
        {
            MainContent.Content = null;
            MainContent.Content = _enterControl;
            _enterControl.LoginTextBox.Text = "";
            _enterControl.PasswordTextBox.Password = "";
        }

        private void _enterControl_RegistrationClick()
        {
            MainContent.Content = null;
            _registrationControl = new RegistrationControl();
            _registrationControl.RegisterClick += _registrationControl_RegisterClick;
            _registrationControl.BackClick += _registrationControl_BackClick; 
            MainContent.Content = _registrationControl;
        }

        private void _registrationControl_RegisterClick(string login, string password)
        {
            MainContent.Content = null;
            MainContent.Content = _enterControl;
            _enterControl.LoginTextBox.Text = login;
            _enterControl.PasswordTextBox.Password = password;
        }

        private void _registrationControl_BackClick()
        {
            MainContent.Content = null;
            MainContent.Content = _enterControl;
        }
    }
}

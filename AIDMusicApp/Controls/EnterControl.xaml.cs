using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для EnterControl.xaml
    /// </summary>
    public partial class EnterControl : UserControl
    {
        public event Action<User> LoginClick = null;

        public event Action RegistrationClick = null;

        public EnterControl()
        {
            InitializeComponent();

            LoginButton.Click += LoginButton_Click;
            RegistrationButton.Click += RegistrationButton_Click;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                AIDMessageWindow.Show("Поле \"Логин\" обязательно для заполнения!");
                LoginTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                AIDMessageWindow.Show("Поле \"Пароль\" обязательно для заполнения!");
                PasswordTextBox.Focus();
                return;
            }

            if (SqlDatabase.Instance.UsersAdapter.ContainsLogin(LoginTextBox.Text))
            {
                User user = SqlDatabase.Instance.UsersAdapter.GetByLogin(LoginTextBox.Text);

                if (user.Password == PasswordTextBox.Password)
                {
                    LoginClick?.Invoke(user);
                }
                else
                {
                    AIDMessageWindow.Show("Неверный пароль!");
                }
            }
            else
            {
                AIDMessageWindow.Show("Данного пользователя не существует!");
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationClick?.Invoke();
        }
    }
}

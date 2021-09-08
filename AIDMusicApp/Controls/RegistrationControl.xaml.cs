using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIDMusicApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для RegistrationControl.xaml
    /// </summary>
    public partial class RegistrationControl : UserControl
    {
        public event Action<string, string> RegisterClick = null;

        public event Action BackClick = null;

        public RegistrationControl()
        {
            InitializeComponent();

            RegisterButton.Click += RegisterButton_Click;
            BackButton.Click += BackButton_Click;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
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

            if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                if (!Regex.IsMatch(PhoneTextBox.Text, @"\+375\d{9}"))
                {
                    AIDMessageWindow.Show("Поле \"Номер\" не соответствует шаблону!");
                    PhoneTextBox.Focus();
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                if (!Regex.IsMatch(EmailTextBox.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$"))
                {
                    AIDMessageWindow.Show("Поле \"Почта\" не соответствует шаблону!");
                    EmailTextBox.Focus();
                    return;
                }
            }

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    LoginTextBox.IsEnabled = false;
                    PasswordTextBox.IsEnabled = false;
                    PhoneTextBox.IsEnabled = false;
                    EmailTextBox.IsEnabled = false;
                    RegisterButton.IsEnabled = false;
                    BackButton.IsEnabled = false;

                    if (SqlDatabase.Instance.UsersAdapter.ContainsLogin(LoginTextBox.Text) ||
                        SqlDatabase.Instance.UsersAdapter.ContainsPhone(PhoneTextBox.Text) ||
                        SqlDatabase.Instance.UsersAdapter.ContainsEmail(EmailTextBox.Text))
                    {
                        AIDMessageWindow.Show("Пользователь с такими данными уже существует!");
                        LoginTextBox.IsEnabled = true;
                        PasswordTextBox.IsEnabled = true;
                        PhoneTextBox.IsEnabled = true;
                        EmailTextBox.IsEnabled = true;
                        RegisterButton.IsEnabled = true;
                        BackButton.IsEnabled = true;
                        return;
                    }

                    SqlDatabase.Instance.UsersAdapter.Insert(LoginTextBox.Text, PasswordTextBox.Password, PhoneTextBox.Text, EmailTextBox.Text, 1, null);

                    AIDMessageWindow.Show("Пользователь успешно добавлен!");

                    RegisterClick?.Invoke(LoginTextBox.Text, PasswordTextBox.Password);
                });
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackClick?.Invoke();
        }
    }
}

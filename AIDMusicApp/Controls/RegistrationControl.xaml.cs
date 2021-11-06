using AIDMusicApp.Converters;
using AIDMusicApp.Sql;
using AIDMusicApp.Windows;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            if (SqlDatabase.Instance.UsersAdapter.ContainsLogin(LoginTextBox.Text))
            {
                AIDMessageWindow.Show("Пользователь с таким логином уже существует!");
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

                if (SqlDatabase.Instance.UsersAdapter.ContainsPhone(PhoneTextBox.Text))
                {
                    AIDMessageWindow.Show("Пользователь с таким номером телефона уже существует!");
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

                if (SqlDatabase.Instance.UsersAdapter.ContainsEmail(EmailTextBox.Text))
                {
                    AIDMessageWindow.Show("Пользователь с такой почтой уже существует!");
                    EmailTextBox.Focus();
                    return;
                }
            }
            BitmapImage image = (BitmapImage)Application.Current.Resources["DefaultImage"];
            var stream = Application.GetResourceStream(image.UriSource).Stream;
            byte[] avatar = new byte[stream.Length];
            stream.Read(avatar, 0, avatar.Length);
            SqlDatabase.Instance.UsersAdapter.Insert(LoginTextBox.Text, PasswordTextBox.Password, PhoneTextBox.Text, EmailTextBox.Text, 1, avatar);

            AIDMessageWindow.Show("Пользователь успешно добавлен!");

            RegisterClick?.Invoke(LoginTextBox.Text, PasswordTextBox.Password);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackClick?.Invoke();
        }
    }
}

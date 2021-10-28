using AIDMusicApp.Admin.Windows;
using AIDMusicApp.Models;
using AIDMusicApp.Sql;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AIDMusicApp.Admin.Controls.Users
{
    /// <summary>
    /// Логика взаимодействия для UserItemControl.xaml
    /// </summary>
    public partial class UserItemControl : UserControl
    {
        public User UserItem { get; private set; }

        public UserItemControl(User user)
        {
            InitializeComponent();

            UserItem = user;
            IdText.Text = UserItem.Id.ToString();
            LoginText.Text = UserItem.Login;
            EmailText.Text = UserItem.Email;
            PhoneText.Text = UserItem.Phone;
            AccessText.Text = UserItem.AccessId.Name;

            if (UserItem.Avatar != null)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(UserItem.Avatar); ;
                image.EndInit();
                AvatarImage.ImageSource = image;
            }

            if (UserItem.AccessId.Name == "Администратор")
                RemoveButton.IsEnabled = false;

            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new UsersWindow(UserItem);
            if (editWindow.ShowDialog() == true)
            {
                UserItem = editWindow.UserItem;
                LoginText.Text = UserItem.Login;
                EmailText.Text = UserItem.Email;
                PhoneText.Text = UserItem.Phone;
                AccessText.Text = UserItem.AccessId.Name;

                if (UserItem.Avatar != null)
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(UserItem.Avatar); ;
                    image.EndInit();
                    AvatarImage.ImageSource = image;
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SqlDatabase.Instance.UsersAdapter.Delete(UserItem.Id);
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}

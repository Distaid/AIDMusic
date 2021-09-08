using System.Windows;
using System.Windows.Input;

namespace AIDMusicApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string message)
        {
            InitializeComponent();

            TitleText.Text = "";
            MessageText.Text = message;

            MouseDown += MessageWindow_MouseDown;
        }

        public MessageWindow(string header, string message)
        {
            InitializeComponent();

            TitleText.Text = header;
            MessageText.Text = message;

            MouseDown += MessageWindow_MouseDown;
        }

        private void MessageWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}

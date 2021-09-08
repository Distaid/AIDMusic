namespace AIDMusicApp.Windows
{
    public class AIDMessageWindow
    {
        public static bool? Show(string message)
        {
            return new MessageWindow(message).ShowDialog();
        }

        public static bool? Show(string header, string message)
        {
            return new MessageWindow(header, message).ShowDialog();
        }
    }
}

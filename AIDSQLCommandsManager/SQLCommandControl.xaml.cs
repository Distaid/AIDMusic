using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIDSQLCommandsManager
{
    /// <summary>
    /// Логика взаимодействия для SQLCommandControl.xaml
    /// </summary>
    public partial class SQLCommandControl : UserControl
    {
        public string SQLCommandName
        {
            get { return CommandName.Text.Trim(); }
            set { CommandName.Text = value; }
        }

        public string SQLCommandText
        {
            get { return CommandText.Text.Trim(); }
            set { CommandText.Text = value; }
        }

        public SQLCommandControl()
        {
            InitializeComponent();

            RemoveCommand.Click += RemoveCommand_Click;
            DuplicateCommand.Click += DuplicateCommand_Click;
            CopyCommand.Click += CopyCommand_Click;
            PasteCommand.Click += PasteCommand_Click;
            MouseDown += SQLCommandControl_MouseDown;
        }

        private void DuplicateCommand_Click(object sender, RoutedEventArgs e)
        {
            var commands = Parent as StackPanel;

            var command = new SQLCommandControl { SQLCommandName = SQLCommandName, SQLCommandText = SQLCommandText };
            command.Tag = Tag;
            commands.Children.Add(command);
            (commands.Parent as ScrollViewer).ScrollToBottom();
            (Tag as SQLCommandsFileControl).CommandsCounter.Text = $"Команд: {(Tag as SQLCommandsFileControl).CommandsStack.Children.Count}";
        }

        private void CopyCommand_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText($"AIDSQLCommand:SQLCommandName:{SQLCommandName}|SQLCommandText:{SQLCommandText}", TextDataFormat.UnicodeText);
        }

        private void PasteCommand_Click(object sender, RoutedEventArgs e)
        {
            var data = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (data.StartsWith("AIDSQLCommand:"))
            {
                data = data.Substring(14);
                var sql = data.Split('|');

                SQLCommandName = sql[0].Split(':')[1];
                SQLCommandText = sql[1].Split(':')[1];

                CommandText.Focus();
                CommandText.CaretIndex = CommandText.Text.Length;
            }
        }

        private void SQLCommandControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                var data = Clipboard.GetText(TextDataFormat.UnicodeText);
                if (!data.StartsWith("AIDSQLCommand:"))
                    PasteCommand.IsEnabled = false;
                else
                    PasteCommand.IsEnabled = true;
            }
        }

        private void RemoveCommand_Click(object sender, RoutedEventArgs e)
        {
            var commands = Parent as StackPanel;
            commands.Children.Remove(this);
            (Tag as SQLCommandsFileControl).CommandsCounter.Text = $"Команд: {(Tag as SQLCommandsFileControl).CommandsStack.Children.Count}";
        }
    }
}

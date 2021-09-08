using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AIDSQLCommandsManager
{
    /// <summary>
    /// Логика взаимодействия для SQLCommandsFileControl.xaml
    /// </summary>
    public partial class SQLCommandsFileControl : UserControl
    {
        private string _currentFile = "";

        private string _fileCode = "AIDfsqlsecret";

        private bool _isDown;

        private bool _isDragging;

        private Point _startPoint;

        private SQLCommandControl _realDragSource;

        private SQLCommandControl _dummyDragSource = new SQLCommandControl();

        public SQLCommandsFileControl()
        {
            InitializeComponent();

            AddCommand.Click += AddCommand_Click;
            SaveFile.Click += SaveFile_Click;
            SaveAsFile.Click += SaveAsFile_Click;
            CloseFile.Click += CloseFile_Click;

            DuplicateFile.Click += DuplicateFile_Click;
            PasteFile.Click += PasteFile_Click;
            MouseDown += SQLCommandsFileControl_MouseDown;

            CommandsStack.PreviewMouseLeftButtonDown += CommandsStack_PreviewMouseLeftButtonDown;
            CommandsStack.PreviewMouseLeftButtonUp += CommandsStack_PreviewMouseLeftButtonUp;
            CommandsStack.PreviewMouseMove += CommandsStack_PreviewMouseMove;
            CommandsStack.DragEnter += CommandsStack_DragEnter;
            CommandsStack.Drop += CommandsStack_Drop;
        }

        public SQLCommandsFileControl(string filename, JArray array)
        {
            InitializeComponent();

            AddCommand.Click += AddCommand_Click;
            SaveFile.Click += SaveFile_Click;
            SaveAsFile.Click += SaveAsFile_Click;
            CloseFile.Click += CloseFile_Click;

            DuplicateFile.Click += DuplicateFile_Click;
            PasteFile.Click += PasteFile_Click;
            MouseDown += SQLCommandsFileControl_MouseDown;

            CommandsStack.PreviewMouseLeftButtonDown += CommandsStack_PreviewMouseLeftButtonDown;
            CommandsStack.PreviewMouseLeftButtonUp += CommandsStack_PreviewMouseLeftButtonUp;
            CommandsStack.PreviewMouseMove += CommandsStack_PreviewMouseMove;
            CommandsStack.DragEnter += CommandsStack_DragEnter;
            CommandsStack.Drop += CommandsStack_Drop;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var item in array)
                    {
                        var dataPart = new SQLCommandControl();
                        dataPart.SQLCommandName = item["SQL_Name"].ToString();
                        dataPart.SQLCommandText = item["SQL_Command"].ToString();
                        dataPart.Tag = this;
                        CommandsStack.Children.Add(dataPart);
                        CommandsScroll.ScrollToBottom();
                    }
                    CommandsCounter.Text = $"Команд: {CommandsStack.Children.Count}";

                    _currentFile = filename;
                    FileLabel.Text = "Открыт файл - " + _currentFile;

                    StateLabelTimer("Открыто успешно!");
                });
            });
        }

        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            var command = new SQLCommandControl();
            command.Tag = this;
            CommandsStack.Children.Add(command);
            CommandsScroll.ScrollToBottom();
            CommandsCounter.Text = $"Команд: {CommandsStack.Children.Count}";
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "")
            {
                SaveAsFile_Click(sender, e);
                return;
            }

            if (CommandsStack.Children.Count == 0)
            {
                MessageBox.Show("Пустой список!");
                return;
            }

            var jpart = new JObject();
            if (!FillJSON(jpart.CreateWriter())) return;

            using (var file = new BinaryWriter(File.Open(_currentFile, FileMode.Truncate, FileAccess.Write)))
            {
                var code = Crypto.Crypt(_fileCode);
                file.Write(code.Length);
                file.Write(code, 0, code.Length);

                var text = Crypto.Crypt(jpart.ToString());
                file.Write(text.Length);
                file.Write(text, 0, text.Length);
            }

            StateLabelTimer("Сохранено успешно!");
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            if (CommandsStack.Children.Count == 0)
            {
                MessageBox.Show("Пустой список!");
                return;
            }

            var jpart = new JObject();
            if (!FillJSON(jpart.CreateWriter())) return;

            var dialog = new SaveFileDialog();
            dialog.Filter = "AIDFile|*.aid";

            if (dialog.ShowDialog() == true)
            {
                using (var file = new BinaryWriter(File.OpenWrite(dialog.FileName)))
                {
                    var code = Crypto.Crypt(_fileCode);
                    file.Write(code.Length);
                    file.Write(code, 0, code.Length);

                    var text = Crypto.Crypt(jpart.ToString());
                    file.Write(text.Length);
                    file.Write(text, 0, text.Length);
                }

                _currentFile = dialog.FileName;
                FileLabel.Text = "Открыт файл - " + _currentFile;
                (Parent as TabItem).Header = Path.GetFileNameWithoutExtension(FileLabel.Text);
            }

            StateLabelTimer("Сохранено успешно!");
        }

        private void CloseFile_Click(object sender, RoutedEventArgs e)
        {
            var tab = Parent as TabItem;
            (tab.Parent as TabControl).Items.Remove(tab);
        }

        private void DuplicateFile_Click(object sender, RoutedEventArgs e)
        {
            var tabs = (Parent as TabItem).Parent as TabControl;
            var sqlCommandsFile = new SQLCommandsFileControl();

            foreach (SQLCommandControl command in CommandsStack.Children)
            {
                var commandCopy = new SQLCommandControl { SQLCommandName = command.SQLCommandName, SQLCommandText = command.SQLCommandText };
                commandCopy.Tag = sqlCommandsFile;
                sqlCommandsFile.CommandsStack.Children.Add(commandCopy);
            }
            sqlCommandsFile.CommandsScroll.ScrollToTop();
            sqlCommandsFile.CommandsCounter.Text = $"Команд: {sqlCommandsFile.CommandsStack.Children.Count}";
            var index = tabs.Items.Add(new TabItem { Header = "Untitled", Content = sqlCommandsFile });
            tabs.SelectedIndex = index;
        }

        private void PasteFile_Click(object sender, RoutedEventArgs e)
        {
            var data = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (data.StartsWith("AIDSQLCommand:"))
            {
                data = data.Substring(14);
                var sql = data.Split('|');
                var command = new SQLCommandControl { SQLCommandName = sql[0].Split(':')[1], SQLCommandText = sql[1].Split(':')[1] };
                command.Tag = this;
                CommandsStack.Children.Add(command);
                CommandsScroll.ScrollToBottom();
                CommandsCounter.Text = $"Команд: {CommandsStack.Children.Count}";
            }
        }

        private void SQLCommandsFileControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                var data = Clipboard.GetText(TextDataFormat.UnicodeText);
                if (!data.StartsWith("AIDSQLCommand:"))
                    PasteFile.IsEnabled = false;
                else
                    PasteFile.IsEnabled = true;
            }
        }

        private void StateLabelTimer(string msg)
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (a, b) => { StateLabel.Text = ""; (a as DispatcherTimer).Stop(); };
            StateLabel.Text = msg;
            timer.Start();
        }

        private bool FillJSON(JsonWriter writer)
        {
            writer.WritePropertyName("Data");
            writer.WriteStartArray();

            foreach (SQLCommandControl part in CommandsStack.Children)
            {
                if (part.SQLCommandName == "")
                {
                    MessageBox.Show("Наименование команды пустое!");
                    return false;
                }

                if (part.SQLCommandText == "")
                {
                    MessageBox.Show("Пустая команда!");
                    return false;
                }

                writer.WriteStartObject();
                writer.WritePropertyName("SQL_Name");
                writer.WriteValue(part.SQLCommandName.Trim());
                writer.WritePropertyName("SQL_Command");
                writer.WriteValue(part.SQLCommandText.Trim());
                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            return true;
        }

        private void CommandsStack_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source != CommandsStack)
            {
                _isDown = true;
                _startPoint = e.GetPosition(CommandsStack);
            }
        }

        private void CommandsStack_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDown = false;
            _isDragging = false;
            _realDragSource?.ReleaseMouseCapture();
        }

        private void CommandsStack_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) && ((Math.Abs(e.GetPosition(CommandsStack).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(CommandsStack).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)) &&
                    (e.OriginalSource is Border || e.OriginalSource is TextBlock || e.OriginalSource is Separator))
                {
                    _isDragging = true;
                    _realDragSource = e.Source as SQLCommandControl;
                    _realDragSource.CaptureMouse();
                    DragDrop.DoDragDrop(_dummyDragSource, new DataObject("SQLCommandControl", e.Source, true), DragDropEffects.Move);
                }
            }
        }

        private void CommandsStack_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SQLCommandControl"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void CommandsStack_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SQLCommandControl"))
            {
                var droptarget = e.Source as SQLCommandControl;
                int droptargetIndex = -1, i = 0;
                foreach (SQLCommandControl element in CommandsStack.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }
                if (droptargetIndex != -1)
                {
                    CommandsStack.Children.Remove(_realDragSource);
                    CommandsStack.Children.Insert(droptargetIndex, _realDragSource);
                }

                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }
    }
}

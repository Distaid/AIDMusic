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

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var item in array)
                    {
                        var dataPart = new SQLCommandControl();
                        dataPart.SQLCommandName = item["SQL_Name"].ToString();
                        dataPart.SQLCommandText = item["SQL_Command"].ToString();
                        CommandsStack.Children.Add(dataPart);
                        CommandsScroll.ScrollToBottom();
                    }

                    _currentFile = filename;
                    FileLabel.Text = "Открыт файл - " + _currentFile;

                    StateLabelTimer("Открыто успешно!");
                });
            });
        }

        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            var command = new SQLCommandControl();
            CommandsStack.Children.Add(command);
            CommandsScroll.ScrollToBottom();
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
                (Parent as TabItem).Header = System.IO.Path.GetFileNameWithoutExtension(FileLabel.Text);
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
                sqlCommandsFile.CommandsStack.Children.Add(commandCopy);
            }
            sqlCommandsFile.CommandsScroll.ScrollToTop();
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
                CommandsStack.Children.Add(command);
                CommandsScroll.ScrollToBottom();
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
    }
}

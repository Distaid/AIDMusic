using Microsoft.Win32;
using Newtonsoft.Json.Linq;
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

namespace AIDSQLCommandsManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fileCode = "AIDfsqlsecret";

        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            TitleBar.MouseDown += TitleBar_MouseDown;
            MinimizeButton.Click += (o, e) => { WindowState = WindowState.Minimized; };
            MaximizeButton.Click += (o, e) => { WindowState = WindowState.Maximized; MaximizeButton.Visibility = Visibility.Collapsed; RestoreButton.Visibility = Visibility.Visible; };
            RestoreButton.Click += (o, e) => { WindowState = WindowState.Normal; MaximizeButton.Visibility = Visibility.Visible; RestoreButton.Visibility = Visibility.Collapsed; };
            CloseButton.Click += (o, e) => { Close(); };

            NewFile.Click += NewFile_Click;
            OpenFile.Click += OpenFile_Click;

            foreach (var filename in App.Args)
            {
                if (System.IO.Path.GetExtension(filename) != ".aid")
                {
                    MessageBox.Show($"Файл {System.IO.Path.GetFileName(filename)} не является файлом фомата .aid\nФайл будет пропущен");
                    return;
                }

                LoadFile(filename);
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 1)
                {
                    DragMove();
                }
                else if (e.ClickCount == 2)
                {
                    if (WindowState == WindowState.Normal)
                    {
                        WindowState = WindowState.Maximized;
                        MaximizeButton.Visibility = Visibility.Collapsed;
                        RestoreButton.Visibility = Visibility.Visible;
                    }
                    else if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                        MaximizeButton.Visibility = Visibility.Visible;
                        RestoreButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            var sqlCommandsFile = new SQLCommandsFileControl();
            var item = new TabItem { Header = "Untitled", Content = sqlCommandsFile };
            var index = FileTabs.Items.Add(item);
            FileTabs.SelectedIndex = index;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "AIDFile|*.aid";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {
                foreach (var filename in dialog.FileNames)
                    LoadFile(filename);
            }
        }

        private void LoadFile(string filename)
        {
            using (var file = new BinaryReader(File.OpenRead(filename)))
            {
                var codeSize = file.ReadInt32();
                var codeData = file.ReadBytes(codeSize);
                if (Crypto.Encrypt(codeData) != _fileCode)
                {
                    MessageBox.Show($"Файл {System.IO.Path.GetFileName(filename)} не является файлом фомата .aid\nФайл будет пропущен");
                    return;
                }
                var textSize = file.ReadInt32();
                var data = file.ReadBytes(textSize);

                var jpart = JObject.Parse(Crypto.Encrypt(data));
                var dataParts = JArray.Parse(jpart["Data"].ToString());

                var sqlCommands = new SQLCommandsFileControl(filename, dataParts);
                var item = new TabItem() { Header = System.IO.Path.GetFileNameWithoutExtension(filename), Content = sqlCommands };
                var index = FileTabs.Items.Add(item);
                FileTabs.SelectedIndex = index;
            }
        }
    }
}

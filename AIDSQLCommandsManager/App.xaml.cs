using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AIDSQLCommandsManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Args = e.Args;
        }
    }
}

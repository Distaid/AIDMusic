using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDMusicApp.Sql
{
    public class SqlDatabase
    {
        public static SqlDatabase Instance = null;

        private SqlConnection _sqlConnection = null;

        public UsersAdapter UsersAdapter = null;

        protected SqlDatabase()
        {
            try
            {
                _sqlConnection = new SqlConnection(FileLoader.LoadSqlConnectionString());
                _sqlConnection.Open();
            }
            catch (SqlException)
            {
                throw new Exception("База данных не обнаружена!\nРабота приложения будет прекращена!");
            }
            catch
            {
                throw;
            }
        }

        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new SqlDatabase();
            }
        }

        public void CloseConnection()
        {
            _sqlConnection.Close();
        }

        public void LoadComands()
        {
            if (!Directory.Exists("SQLCommands"))
                throw new Exception("Папка с файлами команд не обнаружена!\nРабота приложения будет прекращена!");

            try
            {
                UsersAdapter = new UsersAdapter(_sqlConnection, "SQLCommands\\SQLUsers.aid");
            }
            catch
            {
                throw;
            }
        }
    }
}

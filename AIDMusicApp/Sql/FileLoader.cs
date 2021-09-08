using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace AIDMusicApp.Sql
{
    public static class FileLoader
    {
        public static string LoadSqlConnectionString()
        {
            if (!File.Exists("settings.json"))
                throw new FileNotFoundException("Файл настроек SQL не найден!\nРабота приложения будет прекращена!");

            var builder = new SqlConnectionStringBuilder();
            var jsettings = JObject.Parse(File.ReadAllText("settings.json"));

            try
            {
                builder.Add("Workstation ID", jsettings["Server"].ToString());
                builder.Add("Data Source", jsettings["Data Source"].ToString());
                builder.Add("Initial Catalog", jsettings["Initial Catalog"].ToString());
                builder.Add("Integrated Security", jsettings["Integrated Security"].ToString());
            }
            catch
            {
                throw new Exception("Ошибка в файле SQL настроек!\nРабота приложения будет прекращена!");
            }

            return builder.ConnectionString;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetFileData(string path)
        {
            using (var file = new BinaryReader(File.OpenRead(path)))
            {
                var codeSize = file.ReadInt32();
                var codeData = file.ReadBytes(codeSize);
                if (Crypto.Encrypt(codeData) != "AIDfsqlsecret")
                    throw new Exception($"Файл {Path.GetFileName(path)} не является файлом фомата .aid!\nРабота приложения будет прекращена!");

                var textSize = file.ReadInt32();
                var data = file.ReadBytes(textSize);

                var jpart = JObject.Parse(Crypto.Encrypt(data));
                var dataParts = JArray.Parse(jpart["Data"].ToString());
                foreach (var part in dataParts)
                {
                    var key = part["SQL_Name"].ToString();
                    var value = part["SQL_Command"].ToString();
                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace AIDMusicApp.Sql
{
    public abstract class BaseAdapter
    {
        protected Dictionary<string, string> _sqlComands = new Dictionary<string, string>();

        protected SqlConnection _sqlConnection = null;

        protected BaseAdapter(SqlConnection connection, string file)
        {
            _sqlConnection = connection;

            if (!File.Exists(file))
                throw new Exception($"Файл {Path.GetFileName(file)} не найден!\nРабота приложения будет прекращена!");

            foreach (var pair in FileLoader.GetFileData(file))
                _sqlComands.Add(pair.Key, pair.Value);
        }
    }
}

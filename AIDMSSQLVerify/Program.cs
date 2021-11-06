using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace AIDMSSQLVerify
{
    public class Program
    {
        private static List<string> _tables = new List<string>
        {
            "AccessRights",
            "CountriesList",
            "GenresList",
            "LabelsList",
            "SkillsList",
            "AlbumFormatsList",
            "Users",
            "Groups",
            "Albums",
            "Songs",
            "Musicians",
            "GroupGenres",
            "AlbumGenres",
            "SongGenres",
            "Contracts",
            "Members",
            "MusicianSkills",
            "Discography",
            "TrackLists",
            "Feats",
            "Covers"
        };

        private static Dictionary<string, string> _comands = new Dictionary<string, string>();

        private static string _connString = "Data Source={0};Initial Catalog={1};Integrated Security=True";

        private static SqlConnection _connection = null;

        public static void Main(string[] args)
        {
            Init();
            var initialSourse = GetInitialSource();
            Connect(initialSourse);

            var dbResult = CheckDB();
            if (dbResult)
            {
                Console.Clear();
                Console.WriteLine("Database already exists. Do you want to check tables? (Y/N)");
                var key = Console.ReadKey(true).KeyChar;

                if (key == 'n' || key == 'N')
                {
                    Console.WriteLine("\nApplication will be close.");
                    Console.ReadKey(true);
                    return;
                }
            }

            SwapDatabase(initialSourse);
            CreateTables();

            CreateAdmin();

            Close();
        }

        private static IEnumerable<KeyValuePair<string, string>> GetFileData(byte[] sqlFile)
        {
            using (var file = new BinaryReader(new MemoryStream(sqlFile)))
            {
                var codeSize = file.ReadInt32();
                var codeData = file.ReadBytes(codeSize);

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

        private static void Init()
        {
            Console.WriteLine("Resource initializating...");
            foreach (var pair in GetFileData(Resources.SQLVerifyCheck))
                _comands.Add(pair.Key, pair.Value);
            foreach (var pair in GetFileData(Resources.SQLVerifyCreate))
                _comands.Add(pair.Key, pair.Value);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Resource initializating Success!");
        }

        private static string GetInitialSource()
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("Oyherwise, please, install: https://aka.ms/ssmsfullsetup");
            Console.SetCursorPosition(0, 2);
            Console.Write("If MSQ SQL Management Studio already installed, enter Initial Source: ");
            return Console.ReadLine();
        }

        private static void Connect(string source)
        {
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("Connecting to MS SQL...");

            _connection = new SqlConnection(string.Format(_connString, source, "master"));
            _connection.Open();

            Console.SetCursorPosition(0, 5);
            Console.WriteLine("Connecting to MS SQL Success!");
            Thread.Sleep(2000);
        }

        private static bool CheckDB()
        {
            Console.Clear();
            Console.WriteLine("Checking and creating Database:\n");

            Console.WriteLine(" Checking for existence:");
            var result = ContainsDB();
            Console.WriteLine($"   Result: {result}");

            if (!result)
            {
                Console.WriteLine(" Creating:");
                CreateDB();
                Console.WriteLine($"   Result: Success");
                Thread.Sleep(2000);
                return false;
            }

            Thread.Sleep(1000);
            return true;
        }

        private static bool ContainsDB()
        {
            using (var adapter = new SqlCommand(_comands["SQL_Check_Database_MusicDB"], _connection))
            {
                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }

        private static void CreateDB()
        {
            using (var adapter = new SqlCommand(_comands["SQL_Create_Database_MusicDB"], _connection))
                adapter.ExecuteNonQuery();
        }

        private static void SwapDatabase(string source)
        {
            _connection.Close();
            _connection = new SqlConnection(string.Format(_connString, source, "MusicDB"));
            _connection.Open();
        }

        private static void CreateTables()
        {
            Console.Clear();
            Console.WriteLine("Checking and creating tables:\n");

            foreach (var table in _tables)
            {
                Console.WriteLine($" {table} table:");
                Console.WriteLine(" \tChecking for existence:");
                Thread.Sleep(250);
                var result = ContainsTable(table);
                Console.WriteLine($" \t\tResult: {result}");

                if (!result)
                {
                    Console.WriteLine(" \tCreating:");
                    CreateTable(table);
                    Thread.Sleep(250);
                    Console.WriteLine($" \t\tResult: Success");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Checking complete!");
        }

        private static bool ContainsTable(string table)
        {
            using (var adapter = new SqlCommand(_comands[$"SQL_Check_Table_{table}"], _connection))
            {
                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }

        private static void CreateTable(string table)
        {
            using (var adapter = new SqlCommand(_comands[$"SQL_Create_Table_{table}"], _connection))
                adapter.ExecuteNonQuery();
        }

        private static void CreateAdmin()
        {
            Console.WriteLine("\nDo you want to create admin user? (Y/N)");
            var key = Console.ReadKey(true).KeyChar;

            if (key == 'n' || key == 'N')
                return;

            CreateAdminUser();
            Console.WriteLine("\nLogin:    root");
            Console.WriteLine("Password: 12345");
        }

        private static void CreateAdminUser()
        {
            using (var adapter = new SqlCommand(_comands["SQL_Insert_Table_Admin"], _connection))
            {
                using (var stream = new MemoryStream())
                {
                    Resources.person_default.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    adapter.Parameters.AddWithValue("@avatar", stream.ToArray());
                }
                
                adapter.ExecuteNonQuery();
            }
        }

        private static void Close()
        {
            Console.WriteLine("\n\nVerify complete!");
            Console.ReadKey(true);
            _connection.Close();
        }

    }
}

using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class GenresListAdapter : BaseAdapter
    {
        public GenresListAdapter(SqlConnection connection, string file) : base(connection, file) { }

        public IEnumerable<Genre> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select_GenresList"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    yield return new Genre
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1])
                    };
                }
            }
        }

        public int Insert(string name)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Insert_GenresList"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@name", name);

                return Convert.ToInt32(adapter.ExecuteScalar());
            }
        }

        public void Update(int id, string name)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Update_GenresList"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);
                adapter.Parameters.AddWithValue("@name", name);

                adapter.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var adapter = new SqlCommand(_sqlComands[$"SQL_Delete_GenresList"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);

                adapter.ExecuteNonQuery();
            }
        }

        public bool ContainsName(string name)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Check_GenresList_Name"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@name", name);

                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }
    }
}

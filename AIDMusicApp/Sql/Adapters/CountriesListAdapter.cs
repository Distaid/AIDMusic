using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class CountriesListAdapter : BaseAdapter
    {
        public CountriesListAdapter(SqlConnection connection) : base(connection, "SQLCommands\\SQLCountriesList.aid") { }

        public IEnumerable<Country> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    yield return new Country
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1])
                    };
                }
            }
        }

        public Country Insert(string name)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Insert"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@name", name);

                return new Country
                {
                    Id = Convert.ToInt32(command.ExecuteScalar()),
                    Name = name
                };
            }
        }

        public void Update(int id, string name)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Update"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Delete"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        public bool ContainsName(string name)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Check_Name"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@name", name);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count != 0;
            }
        }

        public Country GetById(int id)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_ById"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var adapter = new SqlDataAdapter(command))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    var row = ds.Tables[0].Rows[0];

                    return new Country
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1]),
                    };
                }
            }
        }
    }
}

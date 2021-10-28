using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class MusiciansAdapter : BaseAdapter
    {
        public MusiciansAdapter(SqlConnection connection, string file) : base(connection, file) { }

        public IEnumerable<Musician> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select_Musicians"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var musician = new Musician
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1]),
                        Age = Convert.ToByte(row[2]),
                        CountryId = Convert.ToInt32(row[3]),
                        IsDead = Convert.ToBoolean(row[4]),
                        Skills = new List<Skill>()
                    };

                    foreach (var skill in SqlDatabase.Instance.MusicianSkillsAdapter.GetAllByMusicianId(musician.Id))
                        musician.Skills.Add(skill);

                    yield return musician;
                }
            }
        }

        public int Insert(string name, byte age, int countryId, bool isDead)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Insert_Musicians"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@name", name);
                adapter.Parameters.AddWithValue("@age", age);
                adapter.Parameters.AddWithValue("@country_id", countryId);
                adapter.Parameters.AddWithValue("@is_dead", isDead);

                return Convert.ToInt32(adapter.ExecuteScalar());
            }
        }

        public void Update(int id, string name, byte age, int countryId, bool isDead)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Update_Musicians"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);
                adapter.Parameters.AddWithValue("@name", name);
                adapter.Parameters.AddWithValue("@age", age);
                adapter.Parameters.AddWithValue("@country_id", countryId);
                adapter.Parameters.AddWithValue("@is_dead", isDead);

                adapter.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Delete_Musicians"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);

                adapter.ExecuteNonQuery();
            }
        }
    }
}

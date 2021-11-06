using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class MusiciansAdapter : BaseAdapter
    {
        public MusiciansAdapter(SqlConnection connection) : base(connection, "SQLCommands\\SQLMusicians.aid") { }

        public IEnumerable<Musician> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select"], _sqlConnection))
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
                        CountryId = SqlDatabase.Instance.CountriesListAdapter.GetById(Convert.ToInt32(row[3])),
                        IsDead = Convert.ToBoolean(row[4]),
                        Skills = new ObservableCollection<Skill>()
                    };

                    foreach (var skill in SqlDatabase.Instance.MusicianSkillsAdapter.GetSkillsByMusicianId(musician.Id))
                        musician.Skills.Add(skill);

                    yield return musician;
                }
            }
        }

        public Musician Insert(string name, byte age, int countryId, bool isDead)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Insert"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@country_id", countryId);
                command.Parameters.AddWithValue("@is_dead", isDead);

                return new Musician
                {
                    Id = Convert.ToInt32(command.ExecuteScalar()),
                    Name = name,
                    Age = age,
                    CountryId = SqlDatabase.Instance.CountriesListAdapter.GetById(countryId),
                    IsDead = isDead,
                    Skills = new ObservableCollection<Skill>()
                };
            }
        }

        public void Update(int id, string name, byte age, int countryId, bool isDead)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Update"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@country_id", countryId);
                command.Parameters.AddWithValue("@is_dead", isDead);

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
    }
}

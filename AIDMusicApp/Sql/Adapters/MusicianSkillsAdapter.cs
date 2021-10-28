using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class MusicianSkillsAdapter : BaseAdapter
    {
        public MusicianSkillsAdapter(SqlConnection connection, string file) : base(connection, file) { }

        public IEnumerable<MusicianSkill> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select_MusicianSkills"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    yield return new MusicianSkill
                    {
                        Id = Convert.ToInt32(row[0]),
                        MusicianId = Convert.ToInt32(row[1]),
                        SkillId = Convert.ToInt32(row[2])
                    };
                }
            }
        }

        public int Insert(int musicianId, int skillId)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Insert_MusicianSkills"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@musician_id", musicianId);
                adapter.Parameters.AddWithValue("@skill_id", skillId);

                return Convert.ToInt32(adapter.ExecuteScalar());
            }
        }

        public void Delete(int id)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Delete_MusicianSkills"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);

                adapter.ExecuteNonQuery();
            }
        }

        public IEnumerable<Skill> GetAllByMusicianId(int musicianId)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_MusicianSkills_GetSkillsByMusicianId"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", musicianId);

                using (var adapter = new SqlDataAdapter(command))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        yield return new Skill
                        {
                            Id = Convert.ToInt32(row[0]),
                            Name = Convert.ToString(row[1])
                        };
                    }
                }
            }
        }

        public MusicianSkill GetById(int id)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_MusicianSkills_ById"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var adapter = new SqlDataAdapter(command))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    DataRow row = ds.Tables[0].Rows[0];

                    return new MusicianSkill
                    {
                        Id = Convert.ToInt32(row[0]),
                        MusicianId = Convert.ToInt32(row[1]),
                        SkillId = Convert.ToInt32(row[2])
                    };
                }
            }
        }

        public int GetIdByMusicianIdAndSkillId(int musicianId, int skillId)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_MusicianSkills_GetIdByMusicianIdAndSkillId"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@musician_id", musicianId);
                command.Parameters.AddWithValue("@skill_id", skillId);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}

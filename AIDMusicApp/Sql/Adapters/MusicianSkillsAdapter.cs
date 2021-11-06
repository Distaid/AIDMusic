using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class MusicianSkillsAdapter : BaseAdapter
    {
        public MusicianSkillsAdapter(SqlConnection connection) : base(connection, "SQLCommands\\SQLMusicianSkills.aid") { }

        public int Insert(int musicianId, int skillId)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Insert"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@musician_id", musicianId);
                command.Parameters.AddWithValue("@skill_id", skillId);

                return Convert.ToInt32(command.ExecuteScalar());
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

        public IEnumerable<Skill> GetSkillsByMusicianId(int musicianId)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_SkillsByMusicianId"], _sqlConnection))
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

        public int GetIdByMusicianIdAndSkillId(int musicianId, int skillId)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_Id_ByMusicianIdAndSkillId"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@musician_id", musicianId);
                command.Parameters.AddWithValue("@skill_id", skillId);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}

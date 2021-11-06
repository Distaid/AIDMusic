using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class AccessAdapter: BaseAdapter
    {
        public AccessAdapter(SqlConnection connection) : base(connection, "SQLCommands\\SQLAccessRights.aid") { }

        public IEnumerable<Access> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    yield return new Access
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1])
                    };
                }
            }
        }

        public Access GetById(int id)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_ById"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var adapter = new SqlDataAdapter(command))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    var row = ds.Tables[0].Rows[0];

                    return new Access
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = Convert.ToString(row[1]),
                    };
                }
            }
        }
    }
}

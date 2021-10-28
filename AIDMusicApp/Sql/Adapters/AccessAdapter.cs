using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AIDMusicApp.Sql.Adapters
{
    public class AccessAdapter: BaseAdapter
    {
        public AccessAdapter(SqlConnection connection, string file) : base(connection, file) { }

        public IEnumerable<Access> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select_AccessRights"], _sqlConnection))
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
            var comand = _sqlComands["SQL_Select_AccessRights_ById"].Replace("@id", $"{id}");
            using (var adapter = new SqlDataAdapter(comand, _sqlConnection))
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

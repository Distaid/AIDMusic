using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace AIDMusicApp.Sql.Adapters
{
    public class UsersAdapter : BaseAdapter
    {
        public UsersAdapter(SqlConnection connection) : base(connection, "SQLCommands\\SQLUsers.aid") { }

        public IEnumerable<User> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select"], _sqlConnection))
            {
                var ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    yield return new User
                    {
                        Id = Convert.ToInt32(row[0]),
                        Login = Convert.ToString(row[1]),
                        Password = Convert.ToString(row[2]),
                        Phone = Convert.ToString(row[3]),
                        Email = Convert.ToString(row[4]),
                        AccessId = SqlDatabase.Instance.AccessAdapter.GetById(Convert.ToInt32(row[5])),
                        Avatar = row[6].Equals(DBNull.Value) ? null : (byte[])row[6]
                    };
                }
            }
        }

        public User GetByLogin(string login)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Select_ByLogin"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@login", login);

                using (var adapter = new SqlDataAdapter(command))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    var row = ds.Tables[0].Rows[0];

                    return new User
                    {
                        Id = Convert.ToInt32(row[0]),
                        Login = Convert.ToString(row[1]),
                        Password = Convert.ToString(row[2]),
                        Phone = Convert.ToString(row[3]),
                        Email = Convert.ToString(row[4]),
                        AccessId = SqlDatabase.Instance.AccessAdapter.GetById(Convert.ToInt32(row[5])),
                        Avatar = row[6].Equals(DBNull.Value) ? null : (byte[])row[6]
                    };
                }
            }
        }

        public User Insert(string login, string password, string phone, string email, int accessId, byte[] avatar)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Insert"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@phone", !string.IsNullOrWhiteSpace(phone) ? phone : SqlString.Null);
                command.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(email) ? email : SqlString.Null);
                command.Parameters.AddWithValue("@access_id", accessId);
                command.Parameters.AddWithValue("@avatar", avatar ?? SqlBinary.Null);

                return new User
                {
                    Id = Convert.ToInt32(command.ExecuteScalar()),
                    Login = login,
                    Password = password,
                    Phone = !string.IsNullOrWhiteSpace(phone) ? phone : "",
                    Email = !string.IsNullOrWhiteSpace(email) ? email : "",
                    AccessId = SqlDatabase.Instance.AccessAdapter.GetById(accessId),
                    Avatar = avatar
                };
            }
        }

        public void Update(int id, string login, string password, string phone, string email, int access_id, byte[] avatar)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Update"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@phone", !string.IsNullOrWhiteSpace(phone) ? phone : SqlString.Null);
                command.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(email) ? email : SqlString.Null);
                command.Parameters.AddWithValue("@access_id", access_id);
                command.Parameters.AddWithValue("@avatar", avatar ?? SqlBinary.Null);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var command = new SqlCommand(_sqlComands[$"SQL_Delete"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        public bool ContainsLogin(string login)
        {
            using (var command = new SqlCommand(_sqlComands["SQL_Check_Login"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@login", login);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count != 0;
            }
        }

        public bool ContainsPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            using (var command = new SqlCommand(_sqlComands["SQL_Check_Phone"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@phone", phone);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count != 0;
            }
        }

        public bool ContainsEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            using (var command = new SqlCommand(_sqlComands["SQL_Check_Email"], _sqlConnection))
            {
                command.Parameters.AddWithValue("@email", email);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count != 0;
            }
        }
    }
}

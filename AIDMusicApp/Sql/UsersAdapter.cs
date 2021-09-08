using AIDMusicApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDMusicApp.Sql
{
    public class UsersAdapter : BaseAdapter
    {
        public UsersAdapter(SqlConnection connection, string file) : base(connection, file) { }

        public IEnumerable<User> GetAll()
        {
            using (var adapter = new SqlDataAdapter(_sqlComands["SQL_Select_Users"], _sqlConnection))
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
                        AccessId = Convert.ToInt32(row[5]),
                        Avatar = row[6].Equals(DBNull.Value) ? null : (byte[])row[6]
                    };
                }
            }
        }

        public User GetByLogin(string login)
        {
            var comand = _sqlComands["SQL_Select_Users_ByLogin"].Replace("@login", $"\'{login}\'");
            using (var adapter = new SqlDataAdapter(comand, _sqlConnection))
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
                    AccessId = Convert.ToInt32(row[5]),
                    Avatar = row[6].Equals(DBNull.Value) ? null : (byte[])row[6]
                };
            }
        }

        public int Insert(string login, string password, string phone, string email, int access, byte[] avatar)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Insert_Users"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@login", login);
                adapter.Parameters.AddWithValue("@password", password);
                adapter.Parameters.AddWithValue("@phone", !string.IsNullOrWhiteSpace(phone) ? phone : SqlString.Null);
                adapter.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(email) ? email : SqlString.Null);
                adapter.Parameters.AddWithValue("@access", access);
                adapter.Parameters.AddWithValue("@avatar", avatar ?? SqlBinary.Null);


                return Convert.ToInt32(adapter.ExecuteScalar());
            }
        }

        public void Update(int id, string login, string password, string phone, string email, int access, byte[] avatar)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Update_Users"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);
                adapter.Parameters.AddWithValue("@login", login);
                adapter.Parameters.AddWithValue("@password", password);
                adapter.Parameters.AddWithValue("@phone", !string.IsNullOrWhiteSpace(phone) ? phone : SqlString.Null);
                adapter.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(email) ? email : SqlString.Null);
                adapter.Parameters.AddWithValue("@access", access);
                adapter.Parameters.AddWithValue("@avatar", avatar ?? SqlBinary.Null);

                adapter.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var adapter = new SqlCommand(_sqlComands[$"SQL_Delete_Users"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@id", id);

                adapter.ExecuteNonQuery();
            }
        }

        public bool ContainsLogin(string login)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Check_Users_Login"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@login", login);

                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }

        public bool ContainsPassword(string password)
        {
            using (var adapter = new SqlCommand(_sqlComands["SQL_Check_Users_Password"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@password", password);

                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }

        public bool ContainsPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            using (var adapter = new SqlCommand(_sqlComands["SQL_Check_Users_Phone"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@phone", phone);

                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }

        public bool ContainsEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            using (var adapter = new SqlCommand(_sqlComands["SQL_Check_Users_Email"], _sqlConnection))
            {
                adapter.Parameters.AddWithValue("@email", email);

                var count = Convert.ToInt32(adapter.ExecuteScalar());
                if (count == 0) return false;
            }

            return true;
        }
    }
}

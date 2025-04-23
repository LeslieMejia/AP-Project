using EarlyBirdAPI.Model.Entities;
using EarlyBirdAPI.Model.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBird.Model.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        // C - Insert a new user
        public bool InsertUser(User u)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            dbConn.Open();
            using var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO public.""user"" (name, email, passwordhash, phone, role)
                VALUES (@name, @email, @passwordhash, @phone, @role);
            ";
            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, u.Name);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, u.Email);
            cmd.Parameters.AddWithValue("@passwordhash", NpgsqlDbType.Text, u.PasswordHash ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, u.Phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@role", NpgsqlDbType.Text, u.Role.ToString());

            return InsertData(dbConn, cmd);
        }

        // R - Get a user by ID
        public User? GetUserById(int userId)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            dbConn.Open();
            using var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                SELECT id, name, email, passwordhash, phone, role
                FROM public.""user""
                WHERE id = @id;
            ";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, userId);

            using var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new User
                {
                    Id = Convert.ToInt32(data["id"]),
                    Name = data["name"] as string ?? string.Empty,
                    Email = data["email"] as string ?? string.Empty,
                    PasswordHash = data["passwordhash"] as string,
                    Phone = data["phone"] as string,
                    Role = Enum.Parse<UserRole>(data["role"].ToString()!)
                };
            }
            return null;
        }

        // R - Get all users
        public List<User> GetUsers()
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            dbConn.Open();
            using var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                SELECT id, name, email, passwordhash, phone, role
                FROM public.""user"";
            ";

            using var data = GetData(dbConn, cmd);
            var users = new List<User>();
            while (data != null && data.Read())
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(data["id"]),
                    Name = data["name"] as string ?? string.Empty,
                    Email = data["email"] as string ?? string.Empty,
                    PasswordHash = data["passwordhash"] as string,
                    Phone = data["phone"] as string,
                    Role = Enum.Parse<UserRole>(data["role"].ToString()!)
                });
            }
            return users;
        }

        // U - Update an existing user
        public bool UpdateUser(User u)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            dbConn.Open();
            using var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE public.""user""
                SET name = @name,
                    email = @email,
                    passwordhash = @passwordhash,
                    phone = @phone,
                    role = @role
                WHERE id = @id;
            ";

            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, u.Name);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, u.Email);
            cmd.Parameters.AddWithValue("@passwordhash", NpgsqlDbType.Text, u.PasswordHash ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, u.Phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@role", NpgsqlDbType.Text, u.Role.ToString());
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, u.Id);

            return UpdateData(dbConn, cmd);
        }

        // D - Delete a user
        public bool DeleteUser(int userId)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            dbConn.Open();
            using var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM public.""user""
                WHERE id = @id;
            ";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, userId);

            return DeleteData(dbConn, cmd);
        }
    }
}

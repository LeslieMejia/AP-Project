using EarlyBirdAPI;
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

        // C - Create a new user
        public bool InsertUser(User u)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO users (first_name, last_name, email, password_hash, phone, address, created_at, role)
                    VALUES (@first_name, @last_name, @email, @password_hash, @phone, @address, @created_at, @role)
                ";

                cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, u.FirstName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, u.LastName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, u.Email);
                cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Text, u.PasswordHash ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, u.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@address", NpgsqlDbType.Text, u.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@created_at", NpgsqlDbType.Timestamp, u.CreatedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@role", NpgsqlDbType.Text, u.Role.ToString());

                return InsertData(dbConn, cmd);
            }
        }

        // R - Get a user by id
        public User? GetUserById(int userId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE user_id = @user_id";
                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, userId);

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
                    return new User
                    {
                        UserId = Convert.ToInt32(data["user_id"]),
                        FirstName = data["first_name"] as string,
                        LastName = data["last_name"] as string,
                        Email = data["email"].ToString(),
                        PasswordHash = data["password_hash"] as string,
                        Phone = data["phone"] as string,
                        Address = data["address"] as string,
                        CreatedAt = data["created_at"] as DateTime?,
                        Role = Enum.Parse<UserRole>(data["role"].ToString())
                    };
                }
                return null;
            }
        }

        // R - Get all users
        public List<User> GetUsers()
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM users";
                
                var data = GetData(dbConn, cmd);
                var users = new List<User>();
                while (data != null && data.Read())
                {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(data["user_id"]),
                        FirstName = data["first_name"] as string,
                        LastName = data["last_name"] as string,
                        Email = data["email"].ToString(),
                        PasswordHash = data["password_hash"] as string,
                        Phone = data["phone"] as string,
                        Address = data["address"] as string,
                        CreatedAt = data["created_at"] as DateTime?,
                        Role = Enum.Parse<UserRole>(data["role"].ToString())
                    });
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.
                }
                return users;
            }
        }

        // U - Update an existing user
        public bool UpdateUser(User u)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE users SET
                        first_name = @first_name,
                        last_name = @last_name,
                        email = @email,
                        password_hash = @password_hash,
                        phone = @phone,
                        address = @address,
                        created_at = @created_at,
                        role = @role
                    WHERE user_id = @user_id
                ";

                cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, u.FirstName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, u.LastName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, u.Email);
                cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Text, u.PasswordHash ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, u.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@address", NpgsqlDbType.Text, u.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@created_at", NpgsqlDbType.Timestamp, u.CreatedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@role", NpgsqlDbType.Text, u.Role.ToString());
                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, u.UserId);

                return UpdateData(dbConn, cmd);
            }
        }

        // D - Delete a user
        public bool DeleteUser(int userId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "DELETE FROM users WHERE user_id = @user_id";
                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, userId);

                return DeleteData(dbConn, cmd);
            }
        }
    }
}


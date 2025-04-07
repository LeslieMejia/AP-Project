using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace EarlyBirdAPI.Model.Repositories
{
    public class BaseRepository
    {
        // Connection string for the database connection
        protected string ConnectionString { get; }

        // Constructor that retrieves the connection string from the configuration
        public BaseRepository(IConfiguration configuration)
        {
            // Using EBDatabase for the connection string key
            ConnectionString = configuration.GetConnectionString("EBDatabase") ?? throw new InvalidOperationException("Connection string not found.");
        }

        // Method to retrieve data from the database
        protected NpgsqlDataReader GetData(NpgsqlConnection conn, NpgsqlCommand cmd)
        {
            conn.Open();
            return cmd.ExecuteReader();
        }

        // Method to insert data into the database
        protected bool InsertData(NpgsqlConnection conn, NpgsqlCommand cmd)
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            return true; // Assuming no exceptions were thrown
        }

        // Method to update data in the database
        protected bool UpdateData(NpgsqlConnection conn, NpgsqlCommand cmd)
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            return true; // Assuming no exceptions were thrown
        }

        // Method to delete data from the database
        protected bool DeleteData(NpgsqlConnection conn, NpgsqlCommand cmd)
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            return true; // Assuming no exceptions were thrown
        }
    }
}

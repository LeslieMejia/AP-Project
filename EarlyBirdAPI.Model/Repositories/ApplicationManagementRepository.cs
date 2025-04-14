
using EarlyBirdAPI.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBirdAPI.Model.Repositories
{
    public class ApplicationManagementRepository : BaseRepository
    {
        public ApplicationManagementRepository(IConfiguration configuration) : base(configuration) { }

        // C - Create a new ApplicationManagement record
        public bool InsertManagement(ApplicationManagement m)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO application_management (application_id, user_id, status, notes)
                VALUES (@application_id, @user_id, @status, @notes)
            ";

            cmd.Parameters.AddWithValue("@application_id", NpgsqlDbType.Integer, m.ApplicationId);
            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, m.UserId);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, m.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", NpgsqlDbType.Text, m.Notes ?? (object)DBNull.Value);

            return InsertData(dbConn, cmd);
        }

        // R - Get by ID
        public ApplicationManagement? GetManagementById(int id)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM application_management WHERE management_id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new ApplicationManagement
                {
                    ManagementId = Convert.ToInt32(data["management_id"]),
                    ApplicationId = Convert.ToInt32(data["application_id"]),
                    UserId = Convert.ToInt32(data["user_id"]),
                    Status = data["status"] as string,
                    Notes = data["notes"] as string
                };
            }
            return null;
        }

        // R - Get all
        public List<ApplicationManagement> GetAllManagements()
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM application_management";

            var data = GetData(dbConn, cmd);
            var list = new List<ApplicationManagement>();
            while (data != null && data.Read())
            {
                list.Add(new ApplicationManagement
                {
                    ManagementId = Convert.ToInt32(data["management_id"]),
                    ApplicationId = Convert.ToInt32(data["application_id"]),
                    UserId = Convert.ToInt32(data["user_id"]),
                    Status = data["status"] as string,
                    Notes = data["notes"] as string
                });
            }
            return list;
        }

        // U - Update
        public bool UpdateManagement(ApplicationManagement m)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE application_management SET
                    application_id = @application_id,
                    user_id = @user_id,
                    status = @status,
                    notes = @notes
                WHERE management_id = @management_id
            ";

            cmd.Parameters.AddWithValue("@application_id", NpgsqlDbType.Integer, m.ApplicationId);
            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, m.UserId);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, m.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", NpgsqlDbType.Text, m.Notes ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@management_id", NpgsqlDbType.Integer, m.ManagementId);

            return UpdateData(dbConn, cmd);
        }

        // D - Delete
        public bool DeleteManagement(int id)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM application_management WHERE management_id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
    }
}
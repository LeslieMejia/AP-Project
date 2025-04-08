using EarlyBirdAPI.Model.Entities;
using EarlyBirdAPI.Model.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBird.Model.Repositories
{
    public class SavedJobRepository : BaseRepository
    {
        public SavedJobRepository(IConfiguration configuration) : base(configuration) { }

        // C - Create a new SavedJob record
        public bool InsertSavedJob(SavedJob savedJob)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO saved_jobs (user_id, job_id, saved_at)
                    VALUES (@user_id, @job_id, @saved_at)
                ";

                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, savedJob.UserId);
                cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, savedJob.JobId);
                cmd.Parameters.AddWithValue("@saved_at", NpgsqlDbType.Timestamp, savedJob.SavedAt ?? DateTime.UtcNow);

                return InsertData(dbConn, cmd);
            }
        }

        // R - Get a SavedJob by ID
        public SavedJob? GetSavedJobById(int savedJobId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM saved_jobs WHERE saved_job_id = @saved_job_id";
                cmd.Parameters.AddWithValue("@saved_job_id", NpgsqlDbType.Integer, savedJobId);

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new SavedJob
                    {
                        SavedJobId = Convert.ToInt32(data["saved_job_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        JobId = Convert.ToInt32(data["job_id"]),
                        SavedAt = data["saved_at"] as DateTime?
                    };
                }
                return null;
            }
        }

        // R - Get all SavedJobs for a specific user
        public List<SavedJob> GetSavedJobsByUserId(int userId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM saved_jobs WHERE user_id = @user_id";
                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, userId);

                var data = GetData(dbConn, cmd);
                var savedJobs = new List<SavedJob>();
                while (data != null && data.Read())
                {
                    savedJobs.Add(new SavedJob
                    {
                        SavedJobId = Convert.ToInt32(data["saved_job_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        JobId = Convert.ToInt32(data["job_id"]),
                        SavedAt = data["saved_at"] as DateTime?
                    });
                }
                return savedJobs;
            }
        }

        // D - Delete a SavedJob by ID
        public bool DeleteSavedJob(int savedJobId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "DELETE FROM saved_jobs WHERE saved_job_id = @saved_job_id";
                cmd.Parameters.AddWithValue("@saved_job_id", NpgsqlDbType.Integer, savedJobId);

                return DeleteData(dbConn, cmd);
            }
        }
    }
}
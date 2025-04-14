using EarlyBirdAPI;
using EarlyBirdAPI.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBirdAPI.Model.Repositories
{
    public class JobRepository : BaseRepository
    {
        public JobRepository(IConfiguration configuration) : base(configuration) { }

        // C - Create a new job
        public bool InsertJob(Job job)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO jobs (user_id, title, description, category_id, salary_range, location, status, posted_at)
                VALUES (@user_id, @title, @description, @category_id, @salary_range, @location, @status, @posted_at)
            ";

            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, job.UserId);
            cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, job.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, job.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@category_id", NpgsqlDbType.Integer, job.CategoryId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@salary_range", NpgsqlDbType.Text, job.SalaryRange ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, job.Location ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, job.Status.ToString());
            cmd.Parameters.AddWithValue("@posted_at", NpgsqlDbType.Timestamp, job.PostedAt ?? DateTime.UtcNow);

            return InsertData(dbConn, cmd);
        }

        // R - Get a job by ID
        public Job? GetJobById(int jobId)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM jobs WHERE job_id = @job_id";
            cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, jobId);

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new Job
                {
                    JobId = Convert.ToInt32(data["job_id"]),
                    UserId = Convert.ToInt32(data["user_id"]),
                    Title = data["title"] as string ?? "",
                    Description = data["description"] as string,
                    CategoryId = data["category_id"] as int?,
                    SalaryRange = data["salary_range"] as string,
                    Location = data["location"] as string,
                    Status = Enum.Parse<JobPostingStatus>(data["status"].ToString() ?? "Open"),
                    PostedAt = data["posted_at"] as DateTime?
                };
            }

            return null;
        }

        // R - Get all jobs
        public List<Job> GetJobs()
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM jobs";

            var data = GetData(dbConn, cmd);
            var jobs = new List<Job>();
            while (data != null && data.Read())
            {
                jobs.Add(new Job
                {
                    JobId = Convert.ToInt32(data["job_id"]),
                    UserId = Convert.ToInt32(data["user_id"]),
                    Title = data["title"] as string ?? "",
                    Description = data["description"] as string,
                    CategoryId = data["category_id"] as int?,
                    SalaryRange = data["salary_range"] as string,
                    Location = data["location"] as string,
                    Status = Enum.Parse<JobPostingStatus>(data["status"].ToString() ?? "Open"),
                    PostedAt = data["posted_at"] as DateTime?
                });
            }

            return jobs;
        }

        // U - Update an existing job
        public bool UpdateJob(Job job)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE jobs SET
                    user_id = @user_id,
                    title = @title,
                    description = @description,
                    category_id = @category_id,
                    salary_range = @salary_range,
                    location = @location,
                    status = @status,
                    posted_at = @posted_at
                WHERE job_id = @job_id
            ";

            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, job.UserId);
            cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, job.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, job.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@category_id", NpgsqlDbType.Integer, job.CategoryId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@salary_range", NpgsqlDbType.Text, job.SalaryRange ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, job.Location ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, job.Status.ToString());
            cmd.Parameters.AddWithValue("@posted_at", NpgsqlDbType.Timestamp, job.PostedAt ?? DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, job.JobId);

            return UpdateData(dbConn, cmd);
        }

        // D - Delete a job
        public bool DeleteJob(int jobId)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM jobs WHERE job_id = @job_id";
            cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, jobId);

            return DeleteData(dbConn, cmd);
        }
    }
}
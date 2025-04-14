using EarlyBirdAPI.Model.Entities;
using EarlyBirdAPI.Model.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBird.Model.Repositories
{
    public class JobApplicationRepository : BaseRepository
    {
        public JobApplicationRepository(IConfiguration configuration) : base(configuration) { }

        // C - Insert a new job application
        public bool InsertJobApplication(JobApplication app)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO job_applications (user_id, job_id, resume_id, cover_letter, status, applied_at)
                    VALUES (@user_id, @job_id, @resume_id, @cover_letter, @status, @applied_at)
                ";

                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, app.UserId);
                cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, app.JobId);
                cmd.Parameters.AddWithValue("@resume_id", NpgsqlDbType.Integer, app.ResumeId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@cover_letter", NpgsqlDbType.Text, app.CoverLetter ?? (object)DBNull.Value);
                // Convert enum to string since our SQL column is text (or use correct enum mapping)
                cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, app.Status.ToString());
                cmd.Parameters.AddWithValue("@applied_at", NpgsqlDbType.Timestamp, app.AppliedAt ?? DateTime.UtcNow);

                return InsertData(dbConn, cmd);
            }
        }

        // R - Get a job application by its ID
        public JobApplication? GetJobApplicationById(int applicationId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM job_applications WHERE application_id = @application_id";
                cmd.Parameters.AddWithValue("@application_id", NpgsqlDbType.Integer, applicationId);

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new JobApplication
                    {
                        ApplicationId = Convert.ToInt32(data["application_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        JobId = Convert.ToInt32(data["job_id"]),
                        ResumeId = data["resume_id"] != DBNull.Value ? Convert.ToInt32(data["resume_id"]) : null,
                        CoverLetter = data["cover_letter"] as string,
                        Status = Enum.Parse<ApplicationStatus>(data["status"].ToString() ?? "UnderReview"),
                        AppliedAt = data["applied_at"] as DateTime?
                    };
                }
                return null;
            }
        }

        // R - Get all job applications (or you could filter by user or job)
        public List<JobApplication> GetJobApplications()
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM job_applications";

                var data = GetData(dbConn, cmd);
                var apps = new List<JobApplication>();
                while (data != null && data.Read())
                {
                    apps.Add(new JobApplication
                    {
                        ApplicationId = Convert.ToInt32(data["application_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        JobId = Convert.ToInt32(data["job_id"]),
                        ResumeId = data["resume_id"] != DBNull.Value ? Convert.ToInt32(data["resume_id"]) : null,
                        CoverLetter = data["cover_letter"] as string,
                        Status = Enum.Parse<ApplicationStatus>(data["status"].ToString() ?? "UnderReview"),
                        AppliedAt = data["applied_at"] as DateTime?
                    });
                }
                return apps;
            }
        }

        // U - Update an existing job application
        public bool UpdateJobApplication(JobApplication app)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE job_applications SET
                        user_id = @user_id,
                        job_id = @job_id,
                        resume_id = @resume_id,
                        cover_letter = @cover_letter,
                        status = @status,
                        applied_at = @applied_at
                    WHERE application_id = @application_id
                ";

                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, app.UserId);
                cmd.Parameters.AddWithValue("@job_id", NpgsqlDbType.Integer, app.JobId);
                cmd.Parameters.AddWithValue("@resume_id", NpgsqlDbType.Integer, app.ResumeId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@cover_letter", NpgsqlDbType.Text, app.CoverLetter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, app.Status.ToString());
                cmd.Parameters.AddWithValue("@applied_at", NpgsqlDbType.Timestamp, app.AppliedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@application_id", NpgsqlDbType.Integer, app.ApplicationId);

                return UpdateData(dbConn, cmd);
            }
        }

        // D - Delete a job application by ID
        public bool DeleteJobApplication(int applicationId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "DELETE FROM job_applications WHERE application_id = @application_id";
                cmd.Parameters.AddWithValue("@application_id", NpgsqlDbType.Integer, applicationId);

                return DeleteData(dbConn, cmd);
            }
        }
    }
}
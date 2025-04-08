using EarlyBirdAPI.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace EarlyBird.Model.Repositories
{
    public class ResumeRepository : BaseRepository
    {
        public ResumeRepository(IConfiguration configuration) : base(configuration) { }

        // Create - Insert a new resume record
        public bool InsertResume(Resume r)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO resumes (user_id, file_path, uploaded_at, is_active)
                    VALUES (@user_id, @file_path, @uploaded_at, @is_active)
                ";

                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, r.UserId);
                cmd.Parameters.AddWithValue("@file_path", NpgsqlDbType.Text, r.FilePath);
                // Set uploaded_at to current time if not provided
                cmd.Parameters.AddWithValue("@uploaded_at", NpgsqlDbType.Timestamp, r.UploadedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@is_active", NpgsqlDbType.Boolean, r.IsActive ?? true);

                return InsertData(dbConn, cmd);
            }
        }

        // Read - Get resume by resume_id
        public Resume? GetResumeById(int resumeId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM resumes WHERE resume_id = @resume_id";
                cmd.Parameters.AddWithValue("@resume_id", NpgsqlDbType.Integer, resumeId);

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new Resume
                    {
                        ResumeId = Convert.ToInt32(data["resume_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        FilePath = data["file_path"].ToString() ?? "",
                        UploadedAt = data["uploaded_at"] as DateTime?,
                        IsActive = data["is_active"] as bool?
                    };
                }
                return null;
            }
        }

        // Read - Get all resumes for a given user
        public List<Resume> GetResumesByUserId(int userId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM resumes WHERE user_id = @user_id";
                cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, userId);

                var data = GetData(dbConn, cmd);
                var resumes = new List<Resume>();
                while (data != null && data.Read())
                {
                    resumes.Add(new Resume
                    {
                        ResumeId = Convert.ToInt32(data["resume_id"]),
                        UserId = Convert.ToInt32(data["user_id"]),
                        FilePath = data["file_path"].ToString() ?? "",
                        UploadedAt = data["uploaded_at"] as DateTime?,
                        IsActive = data["is_active"] as bool?
                    });
                }
                return resumes;
            }
        }

        // Update - Update an existing resume record
        public bool UpdateResume(Resume r)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE resumes SET
                        file_path = @file_path,
                        uploaded_at = @uploaded_at,
                        is_active = @is_active
                    WHERE resume_id = @resume_id
                ";

                cmd.Parameters.AddWithValue("@file_path", NpgsqlDbType.Text, r.FilePath);
                cmd.Parameters.AddWithValue("@uploaded_at", NpgsqlDbType.Timestamp, r.UploadedAt ?? DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@is_active", NpgsqlDbType.Boolean, r.IsActive ?? true);
                cmd.Parameters.AddWithValue("@resume_id", NpgsqlDbType.Integer, r.ResumeId);

                return UpdateData(dbConn, cmd);
            }
        }

        // Delete - Delete a resume record by its ID
        public bool DeleteResume(int resumeId)
        {
            using (var dbConn = new NpgsqlConnection(ConnectionString))
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "DELETE FROM resumes WHERE resume_id = @resume_id";
                cmd.Parameters.AddWithValue("@resume_id", NpgsqlDbType.Integer, resumeId);

                return DeleteData(dbConn, cmd);
            }
        }
    }
}
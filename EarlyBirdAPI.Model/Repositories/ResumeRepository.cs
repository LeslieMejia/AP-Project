using System;
using System.Collections.Generic;
using EarlyBirdAPI.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace EarlyBirdAPI.Model.Repositories
{
    public class ResumeRepository : BaseRepository
    {
        public ResumeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        // Read - Get resume by id
        public Resume? GetResumeById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                // create a new connection for database
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT id, jobseekerid, content FROM public.resume WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new Resume
                    {
                        Id = Convert.ToInt32(data["id"]),
                        JobseekerId = Convert.ToInt32(data["jobseekerid"]),
                        Content = data["content"].ToString() ?? string.Empty
                    };
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // Read - Get all resumes
        public List<Resume> GetResumes()
        {
            NpgsqlConnection dbConn = null;
            var resumes = new List<Resume>();
            try
            {
                // create a new connection for database
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT id, jobseekerid, content FROM public.resume";

                var data = GetData(dbConn, cmd);
                while (data != null && data.Read())
                {
                    resumes.Add(new Resume
                    {
                        Id = Convert.ToInt32(data["id"]),
                        JobseekerId = Convert.ToInt32(data["jobseekerid"]),
                        Content = data["content"].ToString() ?? string.Empty
                    });
                }
                return resumes;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // Create - Insert a new resume record
        public bool InsertResume(Resume r)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                // create a new connection for database
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO public.resume (jobseekerid, content)
                    VALUES (@jobseekerid, @content)
                ";

                cmd.Parameters.AddWithValue("@jobseekerid", NpgsqlDbType.Integer, r.JobseekerId);
                cmd.Parameters.AddWithValue("@content", NpgsqlDbType.Text, r.Content);

                return InsertData(dbConn, cmd);
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // Update - Update an existing resume record
        public bool UpdateResume(Resume r)
        {
            var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE public.resume SET
                    jobseekerid = @jobseekerid,
                    content = @content
                WHERE id = @id
            ";

            cmd.Parameters.AddWithValue("@jobseekerid", NpgsqlDbType.Integer, r.JobseekerId);
            cmd.Parameters.AddWithValue("@content", NpgsqlDbType.Text, r.Content);
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, r.Id);

            return UpdateData(dbConn, cmd);
        }

        // Delete - Delete a resume record by its ID
        public bool DeleteResume(int id)
        {
            var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM public.resume WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
    }
}

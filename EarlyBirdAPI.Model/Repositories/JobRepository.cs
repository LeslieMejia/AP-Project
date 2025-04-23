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

        // R - Get a job by ID
        public Job? GetJobById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM public.job WHERE id = @id";
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new Job
                    {
                        Id = Convert.ToInt32(data["id"]),
                        EmployerId = Convert.ToInt32(data["employerid"]),
                        Title = data["title"].ToString() ?? string.Empty,
                        Description = data["description"] as string,
                        Location = data["location"] as string,
                        SalaryRange = data["salaryrange"] as string,
                        Category = data["category"] as string,
                        Status = Enum.Parse<JobStatus>(data["status"].ToString()!)
                    };
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // R - Get all jobs
        public List<Job> GetJobs()
        {
            NpgsqlConnection dbConn = null;
            var jobs = new List<Job>();
            try
            {
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM public.job";

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    while (data.Read())
                    {
                        var job = new Job
                        {
                            Id = Convert.ToInt32(data["id"]),
                            EmployerId = Convert.ToInt32(data["employerid"]),
                            Title = data["title"].ToString() ?? string.Empty,
                            Description = data["description"] as string,
                            Location = data["location"] as string,
                            SalaryRange = data["salaryrange"] as string,
                            Category = data["category"] as string,
                            Status = Enum.Parse<JobStatus>(data["status"].ToString()!)
                        };
                        jobs.Add(job);
                    }
                }
                return jobs;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // C - Insert a new job
        public bool InsertJob(Job job)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(ConnectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO public.job
                    (employerid, title, description, location, salaryrange, category, status)
                    VALUES
                    (@employerid, @title, @description, @location, @salaryrange, @category, @status);
                ";

                cmd.Parameters.AddWithValue("@employerid", NpgsqlDbType.Integer, job.EmployerId);
                cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, job.Title);
                cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, job.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, job.Location ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@salaryrange", NpgsqlDbType.Text, job.SalaryRange ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@category", NpgsqlDbType.Text, job.Category ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, job.Status.ToString());

                bool result = InsertData(dbConn, cmd);
                return result;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // U - Update an existing job
        public bool UpdateJob(Job job)
        {
            var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE public.job SET
                    employerid = @employerid,
                    title = @title,
                    description = @description,
                    location = @location,
                    salaryrange = @salaryrange,
                    category = @category,
                    status = @status
                WHERE id = @id;
            ";

            cmd.Parameters.AddWithValue("@employerid", NpgsqlDbType.Integer, job.EmployerId);
            cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, job.Title);
            cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, job.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, job.Location ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@salaryrange", NpgsqlDbType.Text, job.SalaryRange ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@category", NpgsqlDbType.Text, job.Category ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, job.Status.ToString());
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, job.Id);

            bool result = UpdateData(dbConn, cmd);
            return result;
        }

        // D - Delete a job
        public bool DeleteJob(int id)
        {
            var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM public.job WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            bool result = DeleteData(dbConn, cmd);
            return result;
        }
    }
}
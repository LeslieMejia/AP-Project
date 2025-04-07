using Microsoft.EntityFrameworkCore;
using EarlyBirdAPI.Model.Entities;

namespace EarlyBirdAPI.Model
{
    public class EarlyBirdDbContext : DbContext
    {
        public EarlyBirdDbContext(DbContextOptions<EarlyBirdDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<ApplicationManagement> ApplicationManagements { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PostgreSQL enums
            modelBuilder
                .HasPostgresEnum("application_status", new[] { "under review", "interview", "hired", "rejected" })
                .HasPostgresEnum("jobposting_status", new[] { "open", "closed" })
                .HasPostgresEnum("user_role", new[] { "employer", "jobseeker" });

            // OPTIONAL: Custom constraints or indexes not handled by attributes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

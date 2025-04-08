using EarlyBirdAPI.Model.Entities;

namespace EarlyBirdAPI.Model
{
    public class EarlyBirdDbContext : DbContext
    {
        public EarlyBirdDbContext(DbContextOptions<EarlyBirdDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Resume> Resume { get; set; }
        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<ApplicationManagement> ApplicationManagement { get; set; }
        public DbSet<SavedJob> SavedJob { get; set; }

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

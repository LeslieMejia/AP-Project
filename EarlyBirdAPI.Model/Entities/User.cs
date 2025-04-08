using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("role")]
        public UserRole Role { get; set; }

        // Navigation properties
        public virtual ICollection<ApplicationManagement> ApplicationManagements { get; set; } = new List<ApplicationManagement>();

        public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

        public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

        public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    }
}

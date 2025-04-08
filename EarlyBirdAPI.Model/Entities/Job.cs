using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;

[Table("jobs")]
public class Job
{
    [Key]
    [Column("job_id")]
    public int JobId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("salary_range")]
    public string? SalaryRange { get; set; }

    [Column("location")]
    public string? Location { get; set; }

    [Column("status")]
    public JobPostingStatus Status { get; set; }

    [Column("posted_at")]
    public DateTime? PostedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
}

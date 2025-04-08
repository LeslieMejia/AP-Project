using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;

[Table("job_applications")]
public class JobApplication
{
    [Key]
    [Column("application_id")]
    public int ApplicationId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("job_id")]
    public int JobId { get; set; }

    [Column("resume_id")]
    public int? ResumeId { get; set; }

    [Column("cover_letter")]
    public string? CoverLetter { get; set; }

    [Column("status")]
    public ApplicationStatus Status { get; set; }

    [Column("applied_at")]
    public DateTime? AppliedAt { get; set; }

    public virtual ICollection<ApplicationManagement> ApplicationManagements { get; set; } = new List<ApplicationManagement>();

    [ForeignKey("JobId")]
    public virtual Job Job { get; set; } = null!;

    [ForeignKey("ResumeId")]
    public virtual Resume? Resume { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}

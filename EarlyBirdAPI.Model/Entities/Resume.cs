using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;

[Table("resumes")]
public class Resume
{
    [Key]
    [Column("resume_id")]
    public int ResumeId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("file_path")]
    public string FilePath { get; set; } = null!;

    [Column("uploaded_at")]
    public DateTime? UploadedAt { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}

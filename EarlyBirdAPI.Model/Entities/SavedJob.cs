using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;

[Table("saved_job")]
public class SavedJob
{
    [Key]
    [Column("saved_job_id")]
    public int SavedJobId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("job_id")]
    public int JobId { get; set; }

    [Column("saved_at")]
    public DateTime? SavedAt { get; set; }

    [ForeignKey("JobId")]
    public virtual Job Job { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}

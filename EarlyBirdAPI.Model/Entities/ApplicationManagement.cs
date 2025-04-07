using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;

[Table("application_management")]
public class ApplicationManagement
{
    [Key]
    [Column("management_id")]
    public int ManagementId { get; set; }

    [Column("application_id")]
    public int ApplicationId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [ForeignKey("ApplicationId")]
    public virtual JobApplication Application { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}

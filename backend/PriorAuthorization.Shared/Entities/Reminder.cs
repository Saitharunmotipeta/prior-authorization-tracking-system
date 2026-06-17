using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Reminder")]
public partial class Reminder
{
    [Key]
    [Column("reminder_id")]
    public int ReminderId { get; set; }

    [Column("auth_id")]
    public int AuthId { get; set; }

    [Column("payer_id")]
    public int PayerId { get; set; }

    [Column("category")]
    public byte Category { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("scheduled_at", TypeName = "datetime")]
    public DateTime? ScheduledAt { get; set; }

    [Column("completed_at", TypeName = "datetime")]
    public DateTime? CompletedAt { get; set; }

    [Column("remarks")]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("AuthId")]
    [InverseProperty("Reminders")]
    public virtual AuthorizationRequest Auth { get; set; } = null!;

    [ForeignKey("PayerId")]
    [InverseProperty("Reminders")]
    public virtual Payer Payer { get; set; } = null!;
}

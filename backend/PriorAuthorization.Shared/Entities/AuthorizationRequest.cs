using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("AuthorizationRequest")]
[Index("EncounterId", Name = "UQ__Authoriz__CDF1340ED8DB83D1", IsUnique = true)]
public partial class AuthorizationRequest
{
    [Key]
    [Column("auth_id")]
    public int AuthId { get; set; }

    [Column("encounter_id")]
    public int EncounterId { get; set; }

    [Column("payer_id")]
    public int PayerId { get; set; }

    [Column("priority")]
    public byte Priority { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("estimated_total_amount", TypeName = "decimal(18, 2)")]
    public decimal EstimatedTotalAmount { get; set; }

    [Column("approved_amount", TypeName = "decimal(18, 2)")]
    public decimal? ApprovedAmount { get; set; }

    [Column("submitted_at", TypeName = "datetime")]
    public DateTime? SubmittedAt { get; set; }

    [Column("reviewed_at", TypeName = "datetime")]
    public DateTime? ReviewedAt { get; set; }

    [Column("expiration_date")]
    public DateOnly? ExpirationDate { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Auth")]
    public virtual ICollection<AuditHistory> AuditHistories { get; set; } = new List<AuditHistory>();

    [InverseProperty("Auth")]
    public virtual ICollection<AuthorizationService> AuthorizationServices { get; set; } = new List<AuthorizationService>();

    [ForeignKey("EncounterId")]
    [InverseProperty("AuthorizationRequest")]
    public virtual Encounter Encounter { get; set; } = null!;

    [ForeignKey("PayerId")]
    [InverseProperty("AuthorizationRequests")]
    public virtual Payer Payer { get; set; } = null!;

    [InverseProperty("Auth")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}

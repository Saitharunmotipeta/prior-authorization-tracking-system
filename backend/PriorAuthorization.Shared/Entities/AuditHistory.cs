using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("AuditHistory")]
public partial class AuditHistory
{
    [Key]
    [Column("audit_id")]
    public int AuditId { get; set; }

    [Column("encounter_id")]
    public int? EncounterId { get; set; }

    [Column("auth_id")]
    public int? AuthId { get; set; }

    [Column("entity_id")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EntityId { get; set; }

    [Column("action_type")]
    public byte ActionType { get; set; }

    [Column("old_value")]
    [Unicode(false)]
    public string? OldValue { get; set; }

    [Column("new_value")]
    [Unicode(false)]
    public string? NewValue { get; set; }

    [Column("performed_by_role")]
    public byte PerformedByRole { get; set; }

    [Column("remarks")]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("AuthId")]
    [InverseProperty("AuditHistories")]
    public virtual AuthorizationRequest? Auth { get; set; }

    [ForeignKey("EncounterId")]
    [InverseProperty("AuditHistories")]
    public virtual Encounter? Encounter { get; set; }
}

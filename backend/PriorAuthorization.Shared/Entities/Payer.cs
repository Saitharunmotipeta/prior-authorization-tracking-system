using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Payer")]
public partial class Payer
{
    [Key]
    [Column("payer_id")]
    public int PayerId { get; set; }

    [Column("payer_name")]
    [StringLength(150)]
    [Unicode(false)]
    public string PayerName { get; set; } = null!;

    [Column("contact_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ContactNumber { get; set; }

    [Column("payer_email")]
    [StringLength(150)]
    [Unicode(false)]
    public string? PayerEmail { get; set; }

    [Column("normal_tat_days")]
    public byte NormalTatDays { get; set; }

    [Column("urgent_tat_days")]
    public byte UrgentTatDays { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Payer")]
    public virtual ICollection<AuthorizationRequest> AuthorizationRequests { get; set; } = new List<AuthorizationRequest>();

    [InverseProperty("Payer")]
    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    [InverseProperty("Payer")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}

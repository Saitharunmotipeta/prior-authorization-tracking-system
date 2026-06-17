using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Policy")]
public partial class Policy
{
    [Key]
    [Column("policy_id")]
    public int PolicyId { get; set; }

    [Column("patient_id")]
    public Guid PatientId { get; set; }

    [Column("payer_id")]
    public int PayerId { get; set; }

    [Column("policy_start_date")]
    public DateOnly PolicyStartDate { get; set; }

    [Column("policy_expiry_date")]
    public DateOnly PolicyExpiryDate { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [ForeignKey("PatientId")]
    [InverseProperty("Policies")]
    public virtual Patient Patient { get; set; } = null!;

    [ForeignKey("PayerId")]
    [InverseProperty("Policies")]
    public virtual Payer Payer { get; set; } = null!;
}

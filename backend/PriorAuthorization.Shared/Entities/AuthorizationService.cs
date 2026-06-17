using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("AuthorizationService")]
public partial class AuthorizationService
{
    [Key]
    [Column("service_id")]
    public int ServiceId { get; set; }

    [Column("auth_id")]
    public int AuthId { get; set; }

    [Column("cpt_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string CptCode { get; set; } = null!;

    [Column("icd_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string IcdCode { get; set; } = null!;

    [Column("estimated_cost", TypeName = "decimal(18, 2)")]
    public decimal EstimatedCost { get; set; }

    [Column("notes")]
    [Unicode(false)]
    public string? Notes { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("AuthId")]
    [InverseProperty("AuthorizationServices")]
    public virtual AuthorizationRequest Auth { get; set; } = null!;

    [ForeignKey("CptCode")]
    [InverseProperty("AuthorizationServices")]
    public virtual Cptcode CptCodeNavigation { get; set; } = null!;

    [ForeignKey("IcdCode")]
    [InverseProperty("AuthorizationServices")]
    public virtual Icdcode IcdCodeNavigation { get; set; } = null!;
}

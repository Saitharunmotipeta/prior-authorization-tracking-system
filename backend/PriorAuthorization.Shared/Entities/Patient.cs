using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Patient")]
[Index("MrnNumber", Name = "UQ__Patient__627085A816650500", IsUnique = true)]
public partial class Patient
{
    [Key]
    [Column("patient_id")]
    public Guid PatientId { get; set; }

    [Column("mrn_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string MrnNumber { get; set; } = null!;

    [Column("full_name")]
    [StringLength(150)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [Column("dob")]
    public DateOnly Dob { get; set; }

    [Column("age")]
    public byte Age { get; set; }

    [Column("gender")]
    [StringLength(20)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("identification_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string? IdentificationType { get; set; }

    [Column("identification_number")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdentificationNumber { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    [InverseProperty("Patient")]
    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
}

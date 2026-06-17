using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Facility")]
public partial class Facility
{
    [Key]
    [Column("facility_id")]
    public int FacilityId { get; set; }

    [Column("facility_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string FacilityName { get; set; } = null!;

    [Column("facility_location")]
    [StringLength(200)]
    [Unicode(false)]
    public string FacilityLocation { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Facility")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [InverseProperty("Facility")]
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}

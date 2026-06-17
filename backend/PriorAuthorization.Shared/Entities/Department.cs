using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("facility_id")]
    public int FacilityId { get; set; }

    [Column("department_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string DepartmentName { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    [ForeignKey("FacilityId")]
    [InverseProperty("Departments")]
    public virtual Facility Facility { get; set; } = null!;
}

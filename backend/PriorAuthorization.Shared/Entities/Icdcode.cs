using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("ICDCode")]
public partial class Icdcode
{
    [Key]
    [Column("icd_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string IcdCode1 { get; set; } = null!;

    [Column("icd_description")]
    [StringLength(500)]
    [Unicode(false)]
    public string IcdDescription { get; set; } = null!;

    [InverseProperty("IcdCodeNavigation")]
    public virtual ICollection<AuthorizationService> AuthorizationServices { get; set; } = new List<AuthorizationService>();
}

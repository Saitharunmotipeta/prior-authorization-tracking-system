using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("CPTCode")]
public partial class Cptcode
{
    [Key]
    [Column("cpt_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string CptCode1 { get; set; } = null!;

    [Column("cpt_description")]
    [StringLength(500)]
    [Unicode(false)]
    public string CptDescription { get; set; } = null!;

    [InverseProperty("CptCodeNavigation")]
    public virtual ICollection<AuthorizationService> AuthorizationServices { get; set; } = new List<AuthorizationService>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PriorAuthorization.Shared.Entities;

[Table("Encounter")]
public partial class Encounter
{
    [Key]
    [Column("encounter_id")]
    public int EncounterId { get; set; }

    [Column("patient_id")]
    public Guid PatientId { get; set; }

    [Column("facility_id")]
    public int FacilityId { get; set; }

    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("condition_type")]
    public byte ConditionType { get; set; }

    [Column("verification_status")]
    public byte VerificationStatus { get; set; }

    [Column("request_status")]
    public byte RequestStatus { get; set; }

    [Column("identification_verified")]
    public bool IdentificationVerified { get; set; }

    [Column("prescription_verified")]
    public bool PrescriptionVerified { get; set; }

    [Column("scan_verified")]
    public bool ScanVerified { get; set; }

    [Column("doctor_notes_verified")]
    public bool DoctorNotesVerified { get; set; }

    [Column("insurance_card_verified")]
    public bool InsuranceCardVerified { get; set; }

    [Column("remarks")]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [InverseProperty("Encounter")]
    public virtual ICollection<AuditHistory> AuditHistories { get; set; } = new List<AuditHistory>();

    [InverseProperty("Encounter")]
    public virtual AuthorizationRequest? AuthorizationRequest { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Encounters")]
    public virtual Department Department { get; set; } = null!;

    [ForeignKey("FacilityId")]
    [InverseProperty("Encounters")]
    public virtual Facility Facility { get; set; } = null!;

    [ForeignKey("PatientId")]
    [InverseProperty("Encounters")]
    public virtual Patient Patient { get; set; } = null!;
}

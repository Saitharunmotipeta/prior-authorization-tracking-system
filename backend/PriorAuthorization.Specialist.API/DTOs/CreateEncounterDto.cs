namespace PriorAuthorization.Specialist.API.DTOs;

public class CreateEncounterDto
{
    public Guid PatientId { get; set; }

    public int FacilityId { get; set; }

    public int DepartmentId { get; set; }

    public byte ConditionType { get; set; }
}
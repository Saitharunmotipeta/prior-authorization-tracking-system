export interface CreateEncounterRequest {
  patientId: string;
  facilityId: number;
  departmentId: number;
  conditionType: number;
}

export interface CreateEncounterResponse {
  encounterId: number;
  message: string;
}
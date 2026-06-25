export interface UpdateEncounterRequest {
  identificationVerified?: boolean;

  prescriptionVerified?: boolean;

  scanVerified?: boolean;

  doctorNotesVerified?: boolean;

  insuranceCardVerified?: boolean;

  remarks?: string;
}

export interface VerifyEncounterResponse {
  success: boolean;

  message: string;

  data: string | null;
}
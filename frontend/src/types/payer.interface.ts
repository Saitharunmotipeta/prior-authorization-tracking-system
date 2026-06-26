export interface ApiResponse<T> {
  success: boolean;

  message: string;

  data: T;
}

/* -------------------- Facilities -------------------- */

export interface PayerFacility {
  facilityId: number;

  facilityName: string;

  pendingCount: number;
}

/* ---------------- Authorization Requests ---------------- */

export interface AuthorizationRequestSummary {
  authId: number;

  encounterId: number;

  patientName: string;

  facilityName: string;

  conditionType: string;

  priority: string;

  status: string;

  estimatedAmount: number;

  submittedAt: string | null;
}

/* ---------------- Authorization Details ---------------- */

export interface AuthorizationDocumentStatus {
  identificationVerified: boolean;

  prescriptionVerified: boolean;

  scanVerified: boolean;

  doctorNotesVerified: boolean;

  insuranceCardVerified: boolean;
}

export interface AuthorizationServiceDetail {
  cptCode: string;

  icdCode: string;

  estimatedCost: number;

  notes: string;
}

export interface AuthorizationDetails {
  authId: number;

  encounterId: number;

  patientName: string;

  dob: string;

  gender: string;

  phoneNumber: string;

  facilityName: string;

  departmentName: string;

  conditionType: string;

  priority: string;

  status: string;

  estimatedAmount: number;

  approvedAmount: number | null;

  submittedAt: string | null;

  reviewedAt: string | null;

  documents: AuthorizationDocumentStatus;

  services: AuthorizationServiceDetail[];
}

export interface ReviewAuthorizationRequest {
  action: number;

  approvedAmount?: number;

  remarks?: string;
}
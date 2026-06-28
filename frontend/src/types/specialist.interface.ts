export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface Facility {
  facilityId: number;
  facilityName: string;
  facilityLocation: string;
}

export interface Department {
  departmentId: number;
  departmentName: string;
}

export interface PatientLookup {
  mrnNumber: string;
  patientId: string;
  patientName: string;
  age: number;
}

export interface EligibilityResult {
  isEligible: boolean;
  policyId: number;
  message: string;
  policyExpiryDate: string;
}

export interface AuthorizationRequest {
  authId: number;
  patientName: string;
  payerName: string;
  status: string;
  priority: string;
  estimatedAmount: number;
  createdAt: string;
  submittedAt: string | null;
}
export interface CptCode {
  cptCode: string;
  cptDescription: string;
}

export interface IcdCode {
  icdCode: string;
  icdDescription: string;
}
export interface SpecialistReminderDto {

  reminderId: number;

  authId: number;

  patientName: string;

  reminderType: string;

  status: string;


}
export interface AuthorizationDetails {
  authId: number;
  patientName: string;
  payerName: string;
  status: string;
  priority: string;
  estimatedAmount: number;
  approvedAmount?: number;
  submittedAt?: string;
  reviewedAt?: string;
  expirationDate?: string;
  createdAt: string;
}
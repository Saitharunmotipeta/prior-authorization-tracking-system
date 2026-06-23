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
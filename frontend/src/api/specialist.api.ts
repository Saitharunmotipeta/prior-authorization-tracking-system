import { specialistApiClient } from "./axios";

import type {
  ApiResponse,
  Facility,
  Department,
  PatientLookup,
  EligibilityResult
} from "../types/specialist.interface";

export const getFacilities = async () => {
  const response =
    await specialistApiClient.get<
      ApiResponse<Facility[]>
    >("/api/facilities");

  return response.data;
};

export const getDepartments = async (
  facilityId: number
) => {
  const response =
    await specialistApiClient.get<
      ApiResponse<Department[]>
    >(
      "/api/departments",
      {
        params: {
          facilityId
        }
      }
    );

  return response.data;
};

export const lookupPatient = async (
  mrnNumber: string
) => {
  const response =
    await specialistApiClient.get<
      PatientLookup
    >(
      `/api/PatientLookup/${mrnNumber}`
    );

  return response.data;
};

export const verifyEligibility = async (
  patientId: string
) => {
  const response =
    await specialistApiClient.get<
      ApiResponse<EligibilityResult>
    >(
      `/api/Eligibility/eligibility/${patientId}`
    );

  return response.data;
};
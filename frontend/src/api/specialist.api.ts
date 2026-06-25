import { specialistApiClient } from "./axios";

import type {
  ApiResponse,
  Facility,
  Department,
  PatientLookup,
  EligibilityResult,
} from "../types/specialist.interface";

import type {
  CreateEncounterRequest,
  CreateEncounterResponse
} from "../types/encounter.interface";

import type {
  UpdateEncounterRequest
} from "../types/documentVerification.interface";

import type {
  CreateAuthorizationRequest,
  CreateAuthorizationResponse
} from "../types/authorization.interface";

import type {
  AddAuthorizationServiceRequest
} from "../types/authorization-service.interface";

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

export const createEncounter = async (
  request:
    CreateEncounterRequest
) => {
  const response =
    await specialistApiClient.post(
      "/api/Encounter",
      request
    );

  return response.data;
};

export const updateEncounter = async (
  encounterId: number,
  request: UpdateEncounterRequest
) => {
  const response =
    await specialistApiClient.patch(
      `/api/Encounter/${encounterId}`,
      request
    );

  return response.data;
};

export const verifyEncounter = async (
  encounterId: number
) => {
  const response =
    await specialistApiClient.patch(
      `/api/Encounter/${encounterId}/verify`
    );

  return response.data;
};


  export const createAuthorizationRequest =
  async (
    request:
      CreateAuthorizationRequest
  ) => {
    const response =
      await specialistApiClient.post(
        "/api/Authorization",
        request
      );

    return response.data;
  };

  export const addAuthorizationService =
  async (
    authorizationRequestId: number,
    request: AddAuthorizationServiceRequest
  ) => {
    const response =
      await specialistApiClient.post(
        `/api/Authorization/${authorizationRequestId}/services`,
        request
      );

    return response.data;
  };
  export const getCptCodes =
  async () => {

    const response =
      await specialistApiClient.get(
        "/api/cptcodes"
      );

    return response.data;
  };
  export const getIcdCodes =
  async () => {

    const response =
      await specialistApiClient.get(
        "/api/icdcodes"
      );

    return response.data;
  };
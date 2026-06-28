import {
  specialistApiClient,
  managerApiClient,
  payerApiClient
} from "./axios";

import type {
  UpdateEncounterRequest
} from "../types/documentVerification.interface";

import type {
  CreateAuthorizationRequest,
  AuthorizationTimeline
} from "../types/authorization.interface";


import type {
  AddAuthorizationServiceRequest,
  AddAuthorizationServiceListRequest,
  AuthorizationService
} from "../types/authorization-service.interface";

import type {
  CreateEncounterRequest
} from "../types/encounter.interface";
import type {
  ApiResponse,
  Facility,
  Department,
  PatientLookup,
  EligibilityResult,
  AuthorizationRequest,
  SpecialistReminderDto
} from "../types/specialist.interface";
import type {
  AuthorizationDetails
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
    request: AddAuthorizationServiceListRequest
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
        "/api/icd-codes"
      );

    return response.data;
  };
  export const getPayers =
  async () => {

    const response =
      await payerApiClient.get(
        "/api/payers"
      );

    return response.data;
  };

export const getAuthorizationRequests = async (
  status?: number
) => {
  const response =
    await specialistApiClient.get<
      ApiResponse<AuthorizationRequest[]>
    >(
      "/api/Authorization",
      {
        params: {
          status
        }
      }
    );

  return response.data;
};
export const getReminders = async () => {

  const response =
    await specialistApiClient.get<
      ApiResponse<SpecialistReminderDto[]>
    >(
      "/api/Authorization/reminders"
    );

  return response.data;
};

export const getAuthorizationServices = async (
  authId: number
) => {
  const response =
    await specialistApiClient.get<
      ApiResponse<AuthorizationService[]>
    >(
      `/api/Authorization/${authId}/services`
    );

  return response.data;
};

export const getAuthorizationTimeline = async (
  authId: number
) => {
  const response =
    await specialistApiClient.get<
      ApiResponse<AuthorizationTimeline[]>
    >(
      `/api/Authorization/${authId}/timeline`
    );

  return response.data;
};
export const getAuthorizationDetails = async (
  authId: number
): Promise<AuthorizationDetails> => {
  const response =
    await specialistApiClient.get<
      ApiResponse<AuthorizationDetails>
    >(`/api/Authorization/${authId}`);

  return response.data.data;
};
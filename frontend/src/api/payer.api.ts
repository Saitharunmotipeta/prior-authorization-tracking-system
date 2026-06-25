import { payerApiClient } from "./axios";

import type {
  ApiResponse,
  PayerFacility,
  AuthorizationRequestSummary,
  AuthorizationDetails,
  ReviewAuthorizationRequest
} from "../types/payer.interface";

/* ---------------- Facilities ---------------- */

export const getFacilities =
  async () => {
    const response =
      await payerApiClient.get<
        ApiResponse<PayerFacility[]>
      >(
        "/api/payers/facilities"
      );

    return response.data;
  };

/* ------------ Authorization Requests ------------ */

export const getAuthorizationRequests =
  async (
    facilityId: number
  ) => {
    const response =
      await payerApiClient.get<
        ApiResponse<
          AuthorizationRequestSummary[]
        >
      >(
        `/api/payers/facilities/${facilityId}/authorization-requests`
      );

    return response.data;
  };

/* ------------ Authorization Details ------------ */

export const getAuthorizationDetails =
  async (
    authId: number
  ) => {
    const response =
      await payerApiClient.get<
        ApiResponse<
          AuthorizationDetails
        >
      >(
        `/api/payers/${authId}`
      );

    return response.data;
  };

  export const reviewAuthorization =
  async (
    authId: number,
    request: ReviewAuthorizationRequest
  ) => {

    const response =
      await payerApiClient.patch(
        `/api/payers/${authId}/review`,
        request
      );

    return response.data;
  };
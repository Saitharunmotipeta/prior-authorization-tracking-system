import { defineStore } from "pinia";

import {
  getFacilities,
  getAuthorizationRequests,
  getAuthorizationDetails,
  reviewAuthorization,
  getAuditHistory
} from "../api/payer.api";

import {
  getErrorMessage
} from "../utils/error-handler";

import type {
  PayerFacility,
  AuthorizationRequestSummary,
  AuthorizationDetails,
  ReviewAuthorizationRequest
} from "../types/payer.interface";

/* ✅ STATE INTERFACE */
interface PayerState {
  facilities: PayerFacility[];
  selectedFacilityId: number | null;
  authorizationRequests: AuthorizationRequestSummary[];
  selectedAuthorizationId: number | null;
  authorizationDetails: AuthorizationDetails | null;
  loading: boolean;
  error: string | null;
  auditHistory: any[]; 
}

/* ✅ STORE */
export const usePayerStore = defineStore("payer", {
  persist: true,

  /* ✅ STATE */
  state: (): PayerState => ({
    facilities: [],
    selectedFacilityId: null,
    authorizationRequests: [],
    selectedAuthorizationId: null,
    authorizationDetails: null,
    loading: false,
    error: null,
    auditHistory: [] 
  }),

  /* ✅ ACTIONS */
  actions: {

    async reviewAuthorization(
      authId: number,
      request: ReviewAuthorizationRequest
    ) {
      try {
        this.loading = true;
        this.error = null;

        const response =
          await reviewAuthorization(authId, request);

        if (this.selectedFacilityId) {
          await this.loadAuthorizationRequests(
            this.selectedFacilityId
          );
        }

        await this.loadAuthorizationDetails(authId);

        return response;
      } catch (error) {
        console.error(error);
        this.error = getErrorMessage(error);
        throw error;
      } finally {
        this.loading = false;
      }
    },

    async loadFacilities() {
      try {
        this.loading = true;
        this.error = null;

        const response = await getFacilities();

        this.facilities = response.data;
      } catch (error) {
        console.error(error);
        this.error = getErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },

    async selectFacility(facilityId: number) {
      this.selectedFacilityId = facilityId;
      this.selectedAuthorizationId = null;
      this.authorizationDetails = null;

      await this.loadAuthorizationRequests(facilityId);
    },

    async loadAuthorizationRequests(facilityId: number) {
      try {
        this.loading = true;
        this.error = null;

        const response =
          await getAuthorizationRequests(facilityId);

        this.authorizationRequests = response.data;
      } catch (error) {
        console.error(error);
        this.error = getErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },

    async loadAuthorizationDetails(authId: number) {
      try {
        this.loading = true;
        this.error = null;

        this.selectedAuthorizationId = authId;

        const response =
          await getAuthorizationDetails(authId);

        this.authorizationDetails = response.data;
      } catch (error) {
        console.error(error);
        this.error = getErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },

    /* ✅ NEW: LOAD AUDIT HISTORY */
    async loadAuditHistory() {
      try {
        this.loading = true;
        this.error = null;

        const response = await getAuditHistory();

        // ✅ latest 15 sorted
        this.auditHistory = response.data
          .sort(
            (a: any, b: any) =>
              new Date(b.createdAt).getTime() -
              new Date(a.createdAt).getTime()
          )
          .slice(0, 15);

      } catch (error) {
        console.error(error);
        this.error = getErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },

    clearSelection() {
      this.selectedAuthorizationId = null;
      this.authorizationDetails = null;
    },

    clearError() {
      this.error = null;
    },

    resetStore() {
      this.selectedFacilityId = null;
      this.selectedAuthorizationId = null;
      this.authorizationRequests = [];
      this.authorizationDetails = null;
      this.error = null;
    }
  },

  /* ✅ GETTERS */
  getters: {
    hasFacilities: (state) =>
      state.facilities.length > 0,

    hasRequests: (state) =>
      state.authorizationRequests.length > 0,

    hasSelectedAuthorization: (state) =>
      state.authorizationDetails !== null
  }
});
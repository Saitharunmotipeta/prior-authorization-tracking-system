import { defineStore } from "pinia";

import type {
  UpdateEncounterRequest
} from "../types/documentVerification.interface";

import {
  updateEncounter,
  verifyEncounter
} from "../api/specialist.api";

import {
  getErrorMessage
} from "../utils/error-handler";

import {
  useEncounterStore
} from "./encounter.store";

import {
  RequestStatus
} from "../enums/request-status.enum";

import {
  useAuthorizationStore
} from "./authorization.store";

export const useDocumentVerificationStore =
  defineStore(
    "documentVerification",
    {
      persist: true,

      state: () => ({
        identificationVerified: false,

        prescriptionVerified: false,

        scanVerified: false,

        doctorNotesVerified: false,

        insuranceCardVerified: false,

        remarks: "",

        verificationStatus:
          "Pending",

        loading: false,

        error:
          null as string | null
      }),

      actions: {
        async saveDocuments() {
          const encounterStore =
            useEncounterStore();

          if (
            !encounterStore.encounterId
          ) {
            this.error =
              "Encounter not found.";

            return;
          }

          try {
            this.loading = true;

            this.error = null;

            const request:
              UpdateEncounterRequest = {
              identificationVerified:
                this.identificationVerified,

              prescriptionVerified:
                this.prescriptionVerified,

              scanVerified:
                this.scanVerified,

              doctorNotesVerified:
                this.doctorNotesVerified,

              insuranceCardVerified:
                this.insuranceCardVerified,

              remarks:
                this.remarks
            };

            console.log(
              encounterStore.encounterId
            );

            await updateEncounter(
              encounterStore.encounterId,
              request
            );
          }
          catch (error) {
            console.error(error);

            this.error =
              getErrorMessage(error);
          }
          finally {
            this.loading = false;
          }
        },

  async verifyDocuments(): Promise<boolean> {
  const encounterStore = useEncounterStore();

  if (!encounterStore.encounterId) {
    this.error = "Encounter not found.";
    return false;
  }

  if (
    !this.identificationVerified ||
    !this.prescriptionVerified ||
    !this.scanVerified ||
    !this.doctorNotesVerified ||
    !this.insuranceCardVerified
  ) {
    this.error = "All documents must be verified before proceeding.";
    return false;
  }

  try {
    this.loading = true;
    this.error = null;

    await verifyEncounter(encounterStore.encounterId);

    this.verificationStatus = "Verified";

    const authorizationStore = useAuthorizationStore();
    authorizationStore.requestStatus = RequestStatus.ReadyForSubmission;

    return true; 
  } catch (error) {
    this.error = getErrorMessage(error);
    return false; 
  } finally {
    this.loading = false;
  }
}
  }});
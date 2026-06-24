import { defineStore } from "pinia";

import type {
  UpdateEncounterRequest
} from "../types/documentVerification.interface";

import {
  updateEncounter,
  verifyEncounter
} from "../api/specialist.api";

import { useEncounterStore }
from "./encounter.store";

export const useDocumentVerificationStore =
  defineStore(
    "documentVerification",
    {
      state: () => ({
        identificationVerified: false,

        prescriptionVerified: false,

        scanVerified: false,

        doctorNotesVerified: false,

        insuranceCardVerified: false,

        remarks: "",

        verificationStatus: "Pending",

        loading: false,

        error: null as string | null
      }),

      actions: {
        async saveDocuments() {
          const encounterStore =
            useEncounterStore();

          if (
            !encounterStore.encounterId
          )
            return;

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
            console.log(encounterStore.encounterId);

            await updateEncounter(
              encounterStore.encounterId,
              request
            );
          }
          catch (error) {
            console.error(error);

            this.error =
              "Failed to save documents";
          }
          finally {
            this.loading = false;
          }
        },

        async verifyDocuments() {
          const encounterStore =
            useEncounterStore();

          if (
            !encounterStore.encounterId
          )
            return;

          try {
            this.loading = true;

            this.error = null;

            await verifyEncounter(
              encounterStore.encounterId
            );

            this.verificationStatus =
              "Verified";
          }
          catch (error) {
            console.error(error);

            this.error =
              "Verification failed";
          }
          finally {
            this.loading = false;
          }
        }
      }
    }
  );
import { defineStore } from "pinia";

import {
  createEncounter
} from "../api/specialist.api";

import {
  getErrorMessage
} from "../utils/error-handler";

import type {
  CreateEncounterRequest
} from "../types/encounter.interface";

export const useEncounterStore =
  defineStore(
    "encounter",
    {
      persist: true,

      state: () => ({
        encounterId:
          null as number | null,

        conditionType: 0,

        loading: false,

        error:
          null as string | null
      }),

      actions: {
        async createEncounter(
          request:
            CreateEncounterRequest
        ) {
          try {
            this.loading = true;

            this.error = null;

            const response =
              await createEncounter(
                request
              );

            this.encounterId =
              response.data;

            return response.data;
          }
          catch (error) {
            console.error(error);

            this.error =
              getErrorMessage(error);

            throw error;
          }
          finally {
            this.loading = false;
          }
        },

        clearError() {
          this.error = null;
        },

        resetEncounter() {
          this.encounterId =
            null;

          this.conditionType = 0;

          this.error = null;
        }
      },

      getters: {
        hasEncounter:
          (state) =>
            state.encounterId !==
            null
      }
    }
  );
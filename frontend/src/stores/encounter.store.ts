import { defineStore } from "pinia";

import type {
  CreateEncounterRequest,
  CreateEncounterResponse
} from "../types/encounter.interface";

import {
  createEncounter
} from "../api/specialist.api";

export const useEncounterStore =
  defineStore(
    "encounter",
    {
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
              "Failed to create encounter";

            throw error;
          }
          finally {
            this.loading = false;
          }
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
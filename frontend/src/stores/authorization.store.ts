import { defineStore } from "pinia";

import {
  createAuthorizationRequest,
  addAuthorizationService
} from "../api/specialist.api";

import type {
  CreateAuthorizationRequest
} from "../types/authorization.interface";

import type {
  AddAuthorizationServiceRequest
} from "../types/authorization-service.interface";

export const useAuthorizationStore =
  defineStore(
    "authorization",
    {
      state: () => ({
        authorizationRequestId:
          null as number | null,

        payerId: 0,

        priority: 0,

        services:
          [] as AddAuthorizationServiceRequest[],

        loading: false,

        error:
          null as string | null
      }),

      actions: {
        async createAuthorization(
          request:
            CreateAuthorizationRequest
        ) {
          try {
            this.loading = true;

            this.error = null;

            const response =
              await createAuthorizationRequest(
                request
              );

            this.authorizationRequestId =
              response.data;

            return response.data;
          }
          catch (error) {
            console.error(error);

            this.error =
              "Failed to create authorization request";

            throw error;
          }
          finally {
            this.loading = false;
          }
        },

        async addService(
          request:
            AddAuthorizationServiceRequest
        ) {
          if (
            !this.authorizationRequestId
          ) {
            throw new Error(
              "Authorization Request Id not found."
            );
          }

          try {
            this.loading = true;

            this.error = null;

            await addAuthorizationService(
              this.authorizationRequestId,
              request
            );

            this.services.push(
              request
            );
          }
          catch (error) {
            console.error(error);

            this.error =
              "Failed to add service";

            throw error;
          }
          finally {
            this.loading = false;
          }
        },

        resetAuthorization() {
          this.authorizationRequestId =
            null;

          this.payerId = 0;

          this.priority = 0;

          this.services = [];

          this.error = null;
        }
      },

      getters: {
        hasAuthorization:
          (state) =>
            state.authorizationRequestId !==
            null,

        serviceCount:
          (state) =>
            state.services.length
      }
    }
  );
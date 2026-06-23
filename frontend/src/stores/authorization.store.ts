import { defineStore } from "pinia";

import {
  createAuthorizationRequest
} from "../api/specialist.api";

import type {
  CreateAuthorizationRequest
} from "../types/authorization.interface";

export const useAuthorizationStore =
  defineStore(
    "authorization",
    {
      state: () => ({
        authorizationRequestId:
          null as number | null,

        payerId: 0,

        priority: 0,

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

            throw error;
          }
          finally {
            this.loading = false;
          }
        }
      }
    }
  );
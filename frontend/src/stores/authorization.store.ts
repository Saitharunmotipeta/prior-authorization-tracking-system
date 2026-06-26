import { defineStore } from "pinia";

import {
  createAuthorizationRequest,
  addAuthorizationService
} from "../api/specialist.api";

import {
    submitAuthorization
} from "../api/payer.api";

import {
  getErrorMessage
} from "../utils/error-handler";

import {
  RequestStatus
} from "../enums/request-status.enum";

import type {
  CreateAuthorizationRequest
} from "../types/authorization.interface";

import type {
  AddAuthorizationServiceRequest,
  AddAuthorizationServiceListRequest
} from "../types/authorization-service.interface";

export const useAuthorizationStore =
  defineStore(
    "authorization",
    {
      persist: true,

      state: () => ({
        authorizationRequestId:
          null as number | null,

        payerId: 0,
        estimatedTotalAmount: 0,

        priority: 0,
        requestStatus:
            RequestStatus.Draft,

        services:
          [] as AddAuthorizationServiceRequest[],

        loading: false,

        error:
          null as string | null
      }),

      actions: {

async uploadServices() {

    try {

        this.loading = true;

        this.error = null;

        if (this.authorizationRequestId === null) {

            throw new Error(
                "Authorization Request Id not found."
            );

        }

        const response =
            await addAuthorizationService(
                this.authorizationRequestId,
                {
                    services: this.services
                }
            );

        this.estimatedTotalAmount =
            response.data;

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

async submitAuthorizationRequest() {

    try {

        this.loading = true;

        this.error = null;

        if (this.authorizationRequestId === null) {

            throw new Error(
                "Authorization Request Id not found."
            );

        }

        await submitAuthorization(
            this.authorizationRequestId
        );

        this.requestStatus =
            RequestStatus.Submitted;

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
              getErrorMessage(error);

            throw error;
          }
          finally {
            this.loading = false;
          }
        },

        async addService(
    request: AddAuthorizationServiceRequest
) {

    this.services.push(request);

},

        clearError() {
          this.error = null;
        },

        resetAuthorization() {
          this.authorizationRequestId =
            null;

          this.payerId = 0;

          this.estimatedTotalAmount = 0;

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
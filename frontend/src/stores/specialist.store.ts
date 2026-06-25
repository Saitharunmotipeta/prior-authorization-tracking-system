import { defineStore } from "pinia";

import {
  getFacilities,
  getDepartments,
  lookupPatient,
  verifyEligibility
} from "../api/specialist.api";

import {
  getErrorMessage
} from "../utils/error-handler";

import type {
  Facility,
  Department,
  PatientLookup,
  EligibilityResult
} from "../types/specialist.interface";

export const useSpecialistStore =
  defineStore(
    "specialist",
    {
      persist: true,

      state: () => ({
        facilities: [] as Facility[],

        departments: [] as Department[],

        selectedFacilityId:
          null as number | null,

        selectedDepartmentId:
          null as number | null,

        mrnNumber: "",

        patientLookup:
          null as PatientLookup | null,

        eligibilityResult:
          null as EligibilityResult | null,

        loading: false,

        error:
          null as string | null
      }),

      actions: {
        async resetWorkflow() {
  this.selectedFacilityId =
    null;

  this.selectedDepartmentId =
    null;

  this.mrnNumber = "";

  this.patientLookup =
    null;

  this.eligibilityResult =
    null;

  this.error = null;
},
        async loadFacilities() {
          try {
            this.loading = true;

            this.error = null;

            const response =
              await getFacilities();

            this.facilities =
              response.data;
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

        async loadDepartments(
          facilityId: number
        ) {
          try {
            this.loading = true;

            this.error = null;

            this.selectedFacilityId =
              facilityId;

            this.selectedDepartmentId =
              null;

            this.patientLookup =
              null;

            this.eligibilityResult =
              null;

            const response =
              await getDepartments(
                facilityId
              );

            this.departments =
              response.data;
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

        selectDepartment(
          departmentId: number
        ) {
          this.selectedDepartmentId =
            departmentId;

          this.patientLookup =
            null;

          this.eligibilityResult =
            null;
        },

        async searchPatient() {
          try {
            this.loading = true;

            this.error = null;

            this.patientLookup =
              await lookupPatient(
                this.mrnNumber
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

        async checkEligibility() {
          try {
            if (
              !this.patientLookup
            )
              return;

            this.loading = true;

            this.error = null;

            const response =
              await verifyEligibility(
                this.patientLookup
                  .patientId
              );

            this.eligibilityResult =
              response.data;
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

        clearError() {
          this.error = null;
        }
      }
    }
  );
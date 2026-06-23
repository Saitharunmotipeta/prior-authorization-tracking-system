import { defineStore } from "pinia";

import {
  getFacilities,
  getDepartments,
  lookupPatient,
  verifyEligibility
} from "../api/specialist.api";

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
        async loadFacilities() {
          try {
            this.loading = true;

            const response =
              await getFacilities();

            this.facilities =
              response.data;
          }
          catch (error) {
            console.error(error);

            this.error =
              "Failed to load facilities";
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
              "Failed to load departments";
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

            this.patientLookup =
              await lookupPatient(
                this.mrnNumber
              );
          }
          catch (error) {
            console.error(error);

            this.error =
              "Patient not found";
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
              "Eligibility verification failed";
          }
          finally {
            this.loading = false;
          }
        }
      }
    }
  );
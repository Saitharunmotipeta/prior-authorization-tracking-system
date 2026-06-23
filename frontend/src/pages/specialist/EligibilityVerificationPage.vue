<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

const specialistStore =
  useSpecialistStore();

const {
  facilities,
  departments,
  mrnNumber,
  patientLookup,
  eligibilityResult,
  selectedFacilityId,
  selectedDepartmentId
} = storeToRefs(
  specialistStore
);

onMounted(async () => {
  await specialistStore.loadFacilities();
});
</script>

<template>
  <div class="page">

    <h1 class="page-title">
      Eligibility Verification
    </h1>

    <div class="form-card">

      <div class="field">

        <label>
          Facility
        </label>

        <select
          @change="
            specialistStore.loadDepartments(
              Number(
                ($event.target as HTMLSelectElement).value
              )
            )
          "
        >
          <option value="">
            Select Facility
          </option>

          <option
            v-for="facility in facilities"
            :key="facility.facilityId"
            :value="facility.facilityId"
          >
            {{ facility.facilityName }}
          </option>

        </select>

      </div>

      <div class="field">

        <label>
          Department
        </label>

        <select
          @change="
            specialistStore.selectDepartment(
              Number(
                ($event.target as HTMLSelectElement).value
              )
            )
          "
        >
          <option value="">
            Select Department
          </option>

          <option
            v-for="department in departments"
            :key="department.departmentId"
            :value="department.departmentId"
          >
            {{ department.departmentName }}
          </option>

        </select>

      </div>

      <div class="field">

        <label>
          MRN Number
        </label>

        <input
          v-model="mrnNumber"
          placeholder="Enter MRN Number"
        />

      </div>

      <button
        class="primary-button"
        :disabled="
          !selectedFacilityId ||
          !selectedDepartmentId ||
          !mrnNumber
        "
        @click="
          specialistStore.searchPatient()
        "
      >
        Search Patient
      </button>

    </div>

    <div
      v-if="patientLookup"
      class="card"
    >
      <h2>
        Patient Information
      </h2>

      <p>
        <strong>Name:</strong>
        {{ patientLookup.patientName }}
      </p>

      <p>
        <strong>MRN:</strong>
        {{ patientLookup.mrnNumber }}
      </p>

      <p>
        <strong>Age:</strong>
        {{ patientLookup.age }}
      </p>

      <button
        class="primary-button"
        @click="
          specialistStore.checkEligibility()
        "
      >
        Verify Eligibility
      </button>

    </div>

    <div
      v-if="eligibilityResult"
      class="card"
    >
      <h2>
        Eligibility Result
      </h2>

      <p>
        <strong>Eligible:</strong>

        {{
          eligibilityResult.isEligible
            ? "Yes"
            : "No"
        }}
      </p>

      <p>
        <strong>Policy Id:</strong>

        {{
          eligibilityResult.policyId
        }}
      </p>

      <p>
        <strong>Message:</strong>

        {{
          eligibilityResult.message
        }}
      </p>

      <p>
        <strong>Expiry Date:</strong>

        {{
          eligibilityResult.policyExpiryDate
        }}
      </p>

      <button
        v-if="
          eligibilityResult.isEligible
        "
        class="success-button"
      >
        Create Encounter
      </button>

    </div>

  </div>
</template>

<style scoped>
.page {
  padding: 24px;
}

.page-title {
  margin-bottom: 24px;
}

.form-card,
.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  margin-bottom: 24px;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;

  margin-bottom: 16px;
}

input,
select {
  padding: 10px;

  border: 1px solid #d1d5db;

  border-radius: 8px;
}

.primary-button {
  padding: 10px 16px;

  border: none;

  border-radius: 8px;

  background: #2563eb;

  color: white;

  cursor: pointer;
}

.success-button {
  padding: 10px 16px;

  border: none;

  border-radius: 8px;

  background: #16a34a;

  color: white;

  cursor: pointer;
}
</style>
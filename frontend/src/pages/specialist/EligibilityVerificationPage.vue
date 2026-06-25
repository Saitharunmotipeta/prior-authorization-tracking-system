<script setup lang="ts">
import { onMounted } from "vue";

import { storeToRefs } from "pinia";

import {
  Building2,
  Search,
  User,
  ShieldCheck,
  ClipboardCheck
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const specialistStore =
  useSpecialistStore();

const authorizationStore =
  useAuthorizationStore();

const {
  facilities,
  departments,
  mrnNumber,
  patientLookup,
  eligibilityResult,
  selectedFacilityId,
  selectedDepartmentId,
  error,
  loading
} = storeToRefs(
  specialistStore
);

const {
  requestStatus
} = storeToRefs(
  authorizationStore
);

onMounted(async () => {
  await specialistStore
    .loadFacilities();
});
</script>

<template>
  <div class="page">

    <div class="page-header">

      <div>

        <h1>
          Eligibility Verification
        </h1>

        <p class="subtitle">
          Verify patient eligibility
          before creating an encounter.
        </p>

      </div>

      <AppStatusBadge
        :status="requestStatus"
      />

    </div>

    <AppError
      :message="error"
    />

    <div class="form-card">

      <div class="card-title">

        <Building2
          :size="20"
        />

        <span>
          Search Patient
        </span>

      </div>

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
            v-for="
              facility
              in facilities
            "
            :key="
              facility.facilityId
            "
            :value="
              facility.facilityId
            "
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
            v-for="
              department
              in departments
            "
            :key="
              department.departmentId
            "
            :value="
              department.departmentId
            "
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
          placeholder="
            Enter MRN Number
          "
        />

      </div>

      <button
        class="primary-button"
        :disabled="
          !selectedFacilityId ||
          !selectedDepartmentId ||
          !mrnNumber ||
          loading
        "
        @click="
          specialistStore.searchPatient()
        "
      >

        <Search
          :size="18"
        />

        Search Patient

      </button>

    </div>

    <div
      v-if="patientLookup"
      class="card"
    >

      <div class="card-title">

        <User
          :size="20"
        />

        <span>
          Patient Information
        </span>

      </div>

      <div class="patient-grid">

        <div
          class="info-item"
        >
          <label>
            Patient Name
          </label>

          <p>
            {{
              patientLookup.patientName
            }}
          </p>
        </div>

        <div
          class="info-item"
        >
          <label>
            MRN Number
          </label>

          <p>
            {{
              patientLookup.mrnNumber
            }}
          </p>
        </div>

        <div
          class="info-item"
        >
          <label>
            Age
          </label>

          <p>
            {{
              patientLookup.age
            }}
          </p>
        </div>

      </div>

      <button
        class="primary-button"
        @click="
          specialistStore.checkEligibility()
        "
      >

        <ShieldCheck
          :size="18"
        />

        Verify Eligibility

      </button>

    </div>

    <div
      v-if="eligibilityResult"
      class="card"
    >

      <div class="card-title">

        <ClipboardCheck
          :size="20"
        />

        <span>
          Eligibility Result
        </span>

      </div>

      <div class="result-grid">

        <div
          class="info-item"
        >
          <label>
            Eligible
          </label>

          <p>
            {{
              eligibilityResult.isEligible
                ? "Yes"
                : "No"
            }}
          </p>
        </div>

        <div
          class="info-item"
        >
          <label>
            Policy Id
          </label>

          <p>
            {{
              eligibilityResult.policyId
            }}
          </p>
        </div>

        <div
          class="info-item"
        >
          <label>
            Expiry Date
          </label>

          <p>
            {{
              eligibilityResult.policyExpiryDate
            }}
          </p>
        </div>

      </div>

      <div class="message-box">

        <strong>
          Message:
        </strong>

        {{
          eligibilityResult.message
        }}

      </div>

      <button
        v-if="
          eligibilityResult.isEligible
        "
        class="success-button"
        @click="
          $router.push(
            '/specialist/create-encounter'
          )
        "
      >
        Create Encounter
      </button>

    </div>

  </div>
</template>

<style scoped>
.page {
  padding: 24px;

  background: #f8fafc;

  min-height: 100vh;
}

.page-header {
  display: flex;

  justify-content:
    space-between;

  align-items: center;

  margin-bottom: 24px;
}

.subtitle {
  color: #64748b;

  margin-top: 6px;
}

.form-card,
.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  margin-bottom: 24px;

  box-shadow:
    0 1px 3px
    rgb(0 0 0 / 8%);
}

.card-title {
  display: flex;

  align-items: center;

  gap: 10px;

  margin-bottom: 20px;

  font-weight: 600;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;

  margin-bottom: 16px;
}

input,
select {
  padding: 12px;

  border: 1px solid #d1d5db;

  border-radius: 8px;
}

.patient-grid,
.result-grid {
  display: grid;

  grid-template-columns:
    repeat(3, 1fr);

  gap: 16px;

  margin-bottom: 20px;
}

.info-item {
  background: #f8fafc;

  border: 1px solid #e5e7eb;

  border-radius: 8px;

  padding: 12px;
}

.info-item label {
  display: block;

  font-size: 12px;

  color: #64748b;

  margin-bottom: 6px;
}

.info-item p {
  margin: 0;

  font-weight: 600;
}

.message-box {
  background: #eff6ff;

  border: 1px solid #bfdbfe;

  border-radius: 8px;

  padding: 14px;

  margin-bottom: 20px;
}

.primary-button,
.success-button {
  border: none;

  border-radius: 8px;

  padding: 12px 18px;

  cursor: pointer;

  display: flex;

  align-items: center;

  gap: 8px;

  font-weight: 600;
}

.primary-button {
  background: #2563eb;

  color: white;
}

.success-button {
  background: #16a34a;

  color: white;
}
</style>
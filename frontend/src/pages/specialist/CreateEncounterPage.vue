<script setup lang="ts">
import { ref } from "vue";

import { useRouter } from "vue-router";

import { storeToRefs } from "pinia";

import {
  User,
  Building2,
  ClipboardPlus
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

import {
  useEncounterStore
} from "../../stores/encounter.store";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const router = useRouter();

const specialistStore =
  useSpecialistStore();

const encounterStore =
  useEncounterStore();

const authorizationStore =
  useAuthorizationStore();

const {
  patientLookup,
  selectedFacilityId,
  selectedDepartmentId,
  error
} =
  storeToRefs(
    specialistStore
  );

const {
  requestStatus
} =
  storeToRefs(
    authorizationStore
  );

const conditionType =
  ref(0);

const createEncounter =
  async () => {
    if (!patientLookup.value)
      return;

    try {
      await encounterStore
        .createEncounter({
          patientId:
            patientLookup.value
              .patientId,

          facilityId:
            selectedFacilityId.value!,

          departmentId:
            selectedDepartmentId.value!,

          conditionType:
            conditionType.value
        });

      router.push({
        path:
          "/specialist/document-verification",

        query: {
          encounterId:
            encounterStore.encounterId
        }
      });
    }
    catch (error) {
      console.error(error);
    }
  };
</script>

<template>
  <div class="page">

    <div class="page-header">

      <div>

        <h1>
          Create Encounter
        </h1>

        <p class="subtitle">
          Create a patient encounter
          before authorization.
        </p>

      </div>

      <AppStatusBadge
        :status="requestStatus"
      />

    </div>

    <AppError
      :message="error"
    />

    <div class="card">

      <div class="card-title">

        <User :size="20" />

        <span>
          Patient Information
        </span>

      </div>

      <div
        v-if="patientLookup"
        class="patient-grid"
      >

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

        <div
          class="info-item"
        >
          <label>
            Facility Id
          </label>

          <p>
            {{
              selectedFacilityId
            }}
          </p>
        </div>

        <div
          class="info-item"
        >
          <label>
            Department Id
          </label>

          <p>
            {{
              selectedDepartmentId
            }}
          </p>
        </div>

      </div>

    </div>

    <div class="card">

      <div class="card-title">

        <ClipboardPlus
          :size="20"
        />

        <span>
          Encounter Details
        </span>

      </div>

      <div class="field">

        <label>
          Condition Type
        </label>

        <select
          v-model="
            conditionType
          "
        >

          <option
            :value="0"
          >
            Elective
          </option>

          <option
            :value="1"
          >
            Urgent
          </option>

          <option
            :value="2"
          >
            Emergency
          </option>

        </select>

      </div>

      <button
        class="primary-button"
        @click="
          createEncounter()
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

  font-size: 16px;
}

.patient-grid {
  display: grid;

  grid-template-columns:
    repeat(2, 1fr);

  gap: 20px;
}

.info-item {
  background: #f8fafc;

  padding: 12px;

  border-radius: 8px;

  border: 1px solid #e5e7eb;
}

.info-item label {
  display: block;

  font-size: 12px;

  color: #64748b;

  margin-bottom: 4px;
}

.info-item p {
  margin: 0;

  font-weight: 500;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

select {
  padding: 12px;

  border: 1px solid #d1d5db;

  border-radius: 8px;
}

.primary-button {
  margin-top: 20px;

  padding: 12px 20px;

  border: none;

  border-radius: 8px;

  background: #2563eb;

  color: white;

  cursor: pointer;

  font-weight: 600;
}

.primary-button:hover {
  opacity: 0.95;
}
</style>
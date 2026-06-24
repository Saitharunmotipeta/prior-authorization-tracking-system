<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";

import { useSpecialistStore } from "../../stores/specialist.store";
import { useEncounterStore } from "../../stores/encounter.store";

const router = useRouter();

const specialistStore =
  useSpecialistStore();

const encounterStore =
  useEncounterStore();

const {
  patientLookup,
  selectedFacilityId,
  selectedDepartmentId
} = storeToRefs(
  specialistStore
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

    <h1 class="page-title">
      Create Encounter
    </h1>

    <div class="card">

      <h2>
        Patient Information
      </h2>

      <div
        v-if="patientLookup"
        class="patient-grid"
      >
        <div>
          <strong>Name</strong>
          <p>
            {{ patientLookup.patientName }}
          </p>
        </div>

        <div>
          <strong>MRN Number</strong>
          <p>
            {{ patientLookup.mrnNumber }}
          </p>
        </div>

        <div>
          <strong>Age</strong>
          <p>
            {{ patientLookup.age }}
          </p>
        </div>

        <div>
          <strong>Facility Id</strong>
          <p>
            {{ selectedFacilityId }}
          </p>
        </div>

        <div>
          <strong>Department Id</strong>
          <p>
            {{ selectedDepartmentId }}
          </p>
        </div>
      </div>

    </div>

    <div class="card">

      <h2>
        Encounter Details
      </h2>

      <div class="field">

        <label>
          Condition Type
        </label>

        <select
          v-model="conditionType"
        >
          <option :value="0">
            Elective
          </option>

          <option :value="1">
            Urgent
          </option>

          <option :value="2">
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

.page-title {
  margin-bottom: 24px;
}

.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  margin-bottom: 24px;
}

.patient-grid {
  display: grid;

  grid-template-columns:
    repeat(2, 1fr);

  gap: 20px;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

select {
  padding: 10px;

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
}
</style>
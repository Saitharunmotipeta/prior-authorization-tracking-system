<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";

import { useEncounterStore }
from "../../stores/encounter.store";

import { useSpecialistStore }
from "../../stores/specialist.store";

import { useDocumentVerificationStore }
from "../../stores/documentVerification.store";

const router = useRouter();

const encounterStore =
  useEncounterStore();

const specialistStore =
  useSpecialistStore();

const documentStore =
  useDocumentVerificationStore();

const {
  patientLookup,
  selectedFacilityId,
  selectedDepartmentId
} = storeToRefs(
  specialistStore
);

const {
  identificationVerified,
  prescriptionVerified,
  scanVerified,
  doctorNotesVerified,
  insuranceCardVerified,
  remarks,
  verificationStatus,
  loading
} = storeToRefs(
  documentStore
);

const saveDocuments =
  async () => {
    await documentStore
      .saveDocuments();
  };

const verifyDocuments =
  async () => {
    await documentStore
      .verifyDocuments();
  };
</script>

<template>
  <div class="page">

    <h1 class="page-title">
      Document Verification
    </h1>

    <div class="card">

      <h2>
        Encounter Summary
      </h2>

      <p>
        <strong>Encounter Id:</strong>
        {{ encounterStore.encounterId }}
      </p>

      <p>
        <strong>Patient:</strong>
        {{ patientLookup?.patientName }}
      </p>

      <p>
        <strong>MRN:</strong>
        {{ patientLookup?.mrnNumber }}
      </p>

      <p>
        <strong>Facility Id:</strong>
        {{ selectedFacilityId }}
      </p>

      <p>
        <strong>Department Id:</strong>
        {{ selectedDepartmentId }}
      </p>

      <p>
        <strong>Status:</strong>
        {{ verificationStatus }}
      </p>

    </div>

    <div class="card">

      <h2>
        Documents
      </h2>

      <label>
        <input
          type="checkbox"
          v-model="
            identificationVerified
          "
        />
        Identification Document
      </label>

      <label>
        <input
          type="checkbox"
          v-model="
            prescriptionVerified
          "
        />
        Prescription
      </label>

      <label>
        <input
          type="checkbox"
          v-model="scanVerified"
        />
        Scan Report
      </label>

      <label>
        <input
          type="checkbox"
          v-model="
            doctorNotesVerified
          "
        />
        Doctor Notes
      </label>

      <label>
        <input
          type="checkbox"
          v-model="
            insuranceCardVerified
          "
        />
        Insurance Card
      </label>

      <textarea
        v-model="remarks"
        placeholder="Remarks"
      />

      <button
        class="save-btn"
        @click="saveDocuments"
      >
        Save Documents
      </button>

      <button
        class="verify-btn"
        @click="
          verifyDocuments()
        "
      >
        Verify Encounter
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

.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  margin-bottom: 24px;
}

label {
  display: block;

  margin-bottom: 12px;
}

textarea {
  width: 100%;

  min-height: 100px;

  margin-top: 16px;

  padding: 10px;
}

.save-btn,
.verify-btn {
  margin-top: 16px;

  margin-right: 12px;

  padding: 10px 16px;

  border: none;

  border-radius: 8px;

  cursor: pointer;
}

.save-btn {
  background: #2563eb;

  color: white;
}

.verify-btn {
  background: #16a34a;

  color: white;
}
</style>
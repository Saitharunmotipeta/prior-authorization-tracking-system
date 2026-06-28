<script setup lang="ts">
import { useRouter } from "vue-router";

import { storeToRefs } from "pinia";

import { ref } from "vue";

const showSnackbar = ref(false);
const snackbarMessage = ref("");
const snackbarType = ref("info");
// "info" | "error"





import {
  FileCheck,
  User
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useEncounterStore
} from "../../stores/encounter.store";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

import {
  useDocumentVerificationStore
} from "../../stores/documentVerification.store";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const router = useRouter();

const encounterStore =
  useEncounterStore();

const specialistStore =
  useSpecialistStore();

const documentStore =
  useDocumentVerificationStore();

const authorizationStore =
  useAuthorizationStore();

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
  loading,
  error
} = storeToRefs(
  documentStore
);

const {
  requestStatus
} = storeToRefs(
  authorizationStore
);

const saveDocuments =
  async () => {
    await documentStore
      .saveDocuments();
  };

// or your snackbar implementation

const verifyDocuments = async () => {
  const success = await documentStore.verifyDocuments();

  if (success) {
    router.push("/specialist/create-authorization");
  } else {
   
snackbarMessage.value =
    documentStore.error ||
    "All documents must be verified";

  snackbarType.value = "error";
  showSnackbar.value = true;


    
  }
};


</script>

<template>
  <div class="page">

 

    <div class="page-header">

      <div>

        <h1>
          Document Verification
        </h1>

        <p class="subtitle">
          Verify all supporting
          clinical documents before
          authorization creation.
        </p>

      </div>

      <AppStatusBadge
        :status="requestStatus"
      />

    </div>

   

    <div class="card">

  

      <div class="card-title">

        <User :size="20" />

        <span>
          Encounter Summary
        </span>

      </div>

      <div class="summary-grid">

        <div class="summary-item">
          <label>
            Encounter Id
          </label>

          <p>
            {{ encounterStore.encounterId }}
          </p>
        </div>

        <div class="summary-item">
          <label>
            Patient
          </label>

          <p>
            {{ patientLookup?.patientName }}
          </p>
        </div>

        <div class="summary-item">
          <label>
            MRN
          </label>

          <p>
            {{ patientLookup?.mrnNumber }}
          </p>
        </div>

        <div class="summary-item">
          <label>
            Facility Id
          </label>

          <p>
            {{ selectedFacilityId }}
          </p>
        </div>

        <div class="summary-item">
          <label>
            Department Id
          </label>

          <p>
            {{ selectedDepartmentId }}
          </p>
        </div>

        <div class="summary-item">
          <label>
            Verification Status
          </label>

          <p>
            {{ verificationStatus }}
          </p>
        </div>

      </div>

    </div>

    <div class="card">

      <div class="card-title">

        <FileCheck :size="20" />

        <span>
          Required Documents
        </span>

      </div>

      <div class="document-grid">

        <label class="document-item">

          <input
            type="checkbox"
            v-model="
              identificationVerified
            "
          />

          Identification Document

        </label>

        <label class="document-item">

          <input
            type="checkbox"
            v-model="
              prescriptionVerified
            "
          />

          Prescription

        </label>

        <label class="document-item">

          <input
            type="checkbox"
            v-model="
              scanVerified
            "
          />

          Scan Report

        </label>

        <label class="document-item">

          <input
            type="checkbox"
            v-model="
              doctorNotesVerified
            "
          />

          Doctor Notes

        </label>

        <label class="document-item">

          <input
            type="checkbox"
            v-model="
              insuranceCardVerified
            "
          />

          Insurance Card

        </label>

      </div>

      <textarea
        v-model="remarks"
        placeholder="
          Enter remarks...
        "
      />

      <div class="actions">

        <button
          class="save-btn"
          :disabled="
            loading
          "
          @click="
            saveDocuments()
          "
        >
          Save Documents
        </button>

        <button
          class="verify-btn"
          :disabled="
            loading
          "
          @click="
            verifyDocuments()
          "
        >
          Verify Encounter
        </button>

 



      </div><br>
   
       <AppError
      :message="error"
    />

      
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
.snackbar-text {
  font-weight: 600;
  font-size: 16px;
  letter-spacing: 0.3px;
}


.card-title {
  display: flex;

  align-items: center;

  gap: 10px;

  margin-bottom: 20px;

  font-weight: 600;
}

.summary-grid {
  display: grid;

  grid-template-columns:
    repeat(3, 1fr);

  gap: 16px;
}

.summary-item {
  background: #f8fafc;

  border: 1px solid #e5e7eb;

  border-radius: 8px;

  padding: 12px;
}

.summary-item label {
  display: block;

  font-size: 12px;

  color: #64748b;

  margin-bottom: 6px;
}

.summary-item p {
  margin: 0;

  font-weight: 600;
}

.document-grid {
  display: grid;

  grid-template-columns:
    repeat(2, 1fr);

  gap: 12px;

  margin-bottom: 20px;
}

.document-item {
  display: flex;

  align-items: center;

  gap: 10px;

  padding: 12px;

  border: 1px solid #e5e7eb;

  border-radius: 8px;

  cursor: pointer;
}

textarea {
  width: 100%;

  min-height: 120px;

  border: 1px solid #d1d5db;

  border-radius: 8px;

  padding: 12px;

  margin-bottom: 20px;
}

.actions {
  display: flex;

  gap: 12px;
}

.save-btn {
  background: #2563eb;

  color: white;

  border: none;

  border-radius: 8px;

  padding: 12px 18px;

  cursor: pointer;

  font-weight: 600;
}

.verify-btn {
  background: #16a34a;

  color: white;

  border: none;

  border-radius: 8px;

  padding: 12px 18px;

  cursor: pointer;

  font-weight: 600;
}

.custom-snackbar {
  background-color: #dc2626 !important; /* red */
  color: white !important;

  font-size: 16px;
  font-weight: 600;

  padding: 14px 18px;
  border-radius: 8px;

  box-shadow: 0 6px 18px rgba(0, 0, 0, 0.2);
}

.fixed-alert {
  position: fixed;         /* ✅ KEY: fixes to screen */
  top: 20px;               /* distance from top */
  left: 50%;               /* center horizontally */
  transform: translateX(-50%);

  width: 60%;              /* controls width */
  max-width: 700px;

  background: #fee2e2;     /* light red */
  border: 1px solid #fca5a5;
  border-radius: 10px;

  color: #b91c1c;          /* dark red text */

  padding: 14px 16px;

  font-weight: 600;
  font-size: 15px;

  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);

  z-index: 9999;           /* ✅ always on top */
}




</style>
<script setup lang="ts">
import { useRouter } from "vue-router";

import { storeToRefs } from "pinia";

import {
  ClipboardCheck,
  FileText,
  BadgeDollarSign,
  Send
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

import {
  useEncounterStore
} from "../../stores/encounter.store";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

import {
  useDocumentVerificationStore
} from "../../stores/documentVerification.store";

const router = useRouter();

const authorizationStore =
  useAuthorizationStore();

const encounterStore =
  useEncounterStore();

const specialistStore =
  useSpecialistStore();

const documentStore =
  useDocumentVerificationStore();

const {
  authorizationRequestId,
  payerId,
  priority,
  services,
  requestStatus,
  error,
  estimatedTotalAmount
} =
  storeToRefs(
    authorizationStore
  );
  console.log(estimatedTotalAmount);

const submitAuthorization =
  () => {

    specialistStore
      .resetWorkflow();

    encounterStore
      .resetEncounter();

    documentStore
      .resetDocuments();

    authorizationStore
      .resetAuthorization();

     localStorage.clear();

    router.push(
      "/specialist/eligibility"
    );
  };
</script>

<template>
  <div class="page">

    <div class="page-header">

      <div>

        <h1>
          Authorization Summary
        </h1>

        <p class="subtitle">
          Review authorization
          request before submission.
        </p>

      </div>

      <AppStatusBadge
        :status="requestStatus"
      />

    </div>

    <AppError
      :message="error"
    />

    <div class="summary-grid">

      <div class="card">

        <div class="card-title">

          <ClipboardCheck
            :size="20"
          />

          <span>
            Authorization Details
          </span>

        </div>

        <div class="detail-row">
          <span>
            Authorization Id
          </span>

          <strong>
            {{ authorizationRequestId }}
          </strong>
        </div>

        <div class="detail-row">
          <span>
            Encounter Id
          </span>

          <strong>
            {{ encounterStore.encounterId }}
          </strong>
        </div>

        <div class="detail-row">
          <span>
            Payer Id
          </span>

          <strong>
            {{ payerId }}
          </strong>
        </div>

        <div class="detail-row">
          <span>
            Priority
          </span>

          <strong>
            {{ priority }}
          </strong>
        </div>

      </div>

      <div class="card">

        <div class="card-title">

          <BadgeDollarSign
            :size="20"
          />

          <span>
            Financial Summary
          </span>

        </div>

        <div class="detail-row">
          <span>
            Status
          </span>

          <AppStatusBadge
  :status="requestStatus"
/>
        </div>

        <div class="detail-row">
          <span>
            Estimated Amount
          </span>

          <strong>
  ₹ {{
    estimatedTotalAmount
      .toLocaleString()
  }}
</strong>
        </div>

        <div class="detail-row">
          <span>
            Services Count
          </span>

          <strong>
            {{ services.length }}
          </strong>
        </div>

      </div>

    </div>

    <div class="card">

      <div class="card-title">

        <FileText
          :size="20"
        />

        <span>
          Authorization Services
        </span>

      </div>

      <div
        v-if="
          services.length === 0
        "
        class="empty-state"
      >
        No services added.
      </div>

      <div
        v-for="
          (
            service,
            index
          )
          in services
        "
        :key="index"
        class="service-item"
      >

        <div
          class="service-header"
        >
          Service
          {{ index + 1 }}
        </div>

        <p>

          <strong>
            CPT:
          </strong>

          {{ service.cptCode }}

        </p>

        <p>

          <strong>
            ICD:
          </strong>

          {{ service.icdCode }}

        </p>

        <p>

          <strong>
            Notes:
          </strong>

          {{ service.notes }}

        </p>

      </div>

    </div>

    <button
      class="submit-btn"
      @click="
        submitAuthorization()
      "
    >

      <Send
        :size="18"
      />

      Submit Authorization

    </button>

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

.summary-grid {
  display: grid;

  grid-template-columns:
    repeat(2, 1fr);

  gap: 20px;

  margin-bottom: 24px;
}

.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

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

.detail-row {
  display: flex;

  justify-content:
    space-between;

  margin-bottom: 14px;

  padding-bottom: 10px;

  border-bottom:
    1px solid #f1f5f9;
}

.service-item {
  border: 1px solid #e5e7eb;

  border-radius: 10px;

  padding: 16px;

  margin-bottom: 14px;

  background: #fafafa;
}

.service-header {
  font-weight: 600;

  margin-bottom: 10px;

  color: #1e293b;
}

.empty-state {
  text-align: center;

  padding: 20px;

  color: #64748b;
}

.submit-btn {
  margin-top: 24px;

  border: none;

  border-radius: 10px;

  background: #16a34a;

  color: white;

  padding: 14px 24px;

  cursor: pointer;

  display: flex;

  align-items: center;

  gap: 10px;

  font-weight: 600;
}

.submit-btn:hover {
  opacity: 0.95;
}
</style>
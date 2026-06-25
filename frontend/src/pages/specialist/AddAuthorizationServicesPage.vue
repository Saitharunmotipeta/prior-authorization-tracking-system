<script setup lang="ts">
import { ref } from "vue";

import { storeToRefs } from "pinia";

import { useRouter } from "vue-router";

import {
  Plus,
  FileText,
  ClipboardList
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const router = useRouter();

const authorizationStore =
  useAuthorizationStore();

const {
  authorizationRequestId,
  services,
  requestStatus,
  error
} =
  storeToRefs(
    authorizationStore
  );
const cptResults = ref([]);
const icdResults = ref([]);

const selectedCpt = ref(null);
const selectedIcd = ref(null);

const addService =
  async () => {
    await authorizationStore
      .addService({
        cptCode:
          cptCode.value,

        icdCode:
          icdCode.value,

        notes:
          notes.value
      });

    cptCode.value = "";
    icdCode.value = "";
    notes.value = "";
  };

const goToSummary =
  () => {
    router.push(
      "/specialist/authorization-summary"
    );
  };
</script>

<template>
  <div class="page">

    <div class="page-header">

      <div>

        <h1>
          Add Authorization Services
        </h1>

        <p class="subtitle">
          Add CPT and ICD services
          to the authorization request.
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

        <FileText :size="20" />

        <span>
          Service Details
        </span>

      </div>

      <p class="auth-id">

        Authorization Id :

        <strong>
          {{ authorizationRequestId }}
        </strong>

      </p>

      <input
        v-model="cptCode"
        placeholder="Enter CPT Code"
      />

      <input
        v-model="icdCode"
        placeholder="Enter ICD Code"
      />

      <textarea
        v-model="notes"
        placeholder="Clinical Notes"
      />

      <button
        class="primary-btn"
        @click="addService()"
      >

        <Plus :size="16" />

        Add Service

      </button>

    </div>

    <div class="card">

      <div class="card-title">

        <ClipboardList
          :size="20"
        />

        <span>
          Added Services
        </span>

      </div>

      <div
        v-if="
          services.length === 0
        "
        class="empty-state"
      >
        No services added yet.
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

      <button
        class="success-btn"
        @click="
          goToSummary()
        "
      >
        Review Authorization
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

.auth-id {
  margin-bottom: 20px;
}

input,
textarea {
  width: 100%;

  padding: 12px;

  margin-bottom: 14px;

  border: 1px solid #d1d5db;

  border-radius: 8px;

  font-size: 14px;
}

textarea {
  min-height: 120px;

  resize: vertical;
}

.primary-btn,
.success-btn {
  border: none;

  border-radius: 8px;

  padding: 12px 18px;

  cursor: pointer;

  display: flex;

  align-items: center;

  gap: 8px;

  font-weight: 600;
}

.primary-btn {
  background: #2563eb;

  color: white;
}

.success-btn {
  background: #16a34a;

  color: white;

  margin-top: 16px;
}

.service-item {
  border: 1px solid #e5e7eb;

  border-radius: 8px;

  padding: 16px;

  margin-bottom: 12px;

  background: #fafafa;
}

.empty-state {
  color: #64748b;

  padding: 20px;

  text-align: center;
}
</style>
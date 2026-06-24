<script setup lang="ts">
import { ref } from "vue";

import { useRouter } from "vue-router";

import { storeToRefs } from "pinia";

import {
  ShieldCheck,
  FilePlus,
  Building2
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useEncounterStore
} from "../../stores/encounter.store";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const router = useRouter();

const encounterStore =
  useEncounterStore();

const authorizationStore =
  useAuthorizationStore();

const {
  requestStatus,
  error,
  loading
} =
  storeToRefs(
    authorizationStore
  );

const payerId =
  ref(0);

const priority =
  ref(0);

const createAuthorization =
  async () => {
    try {
      await authorizationStore
        .createAuthorization({
          encounterId:
            encounterStore.encounterId!,

          payerId:
            payerId.value,

          priority:
            priority.value,

          services: []
        });

      router.push(
        "/specialist/add-services"
      );
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
          Create Authorization Request
        </h1>

        <p class="subtitle">
          Create a new prior
          authorization request
          from a verified encounter.
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

        <ShieldCheck
          :size="20"
        />

        <span>
          Authorization Details
        </span>

      </div>

      <div
        class="encounter-info"
      >

        <p>

          <strong>
            Encounter Id:
          </strong>

          {{
            encounterStore.encounterId
          }}

        </p>

      </div>

      <div class="field">

        <label>

          <Building2
            :size="16"
          />

          Payer Id

        </label>

        <input
          v-model.number="
            payerId
          "
          type="number"
          placeholder="
            Enter Payer Id
          "
        />

      </div>

      <div class="field">

        <label>

          <FilePlus
            :size="16"
          />

          Priority

        </label>

        <select
          v-model="
            priority
          "
        >

          <option
            :value="0"
          >
            Normal
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
        class="
          primary-button
        "
        :disabled="
          loading
        "
        @click="
          createAuthorization()
        "
      >
        Create Authorization
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
  margin-top: 6px;

  color: #64748b;
}

.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  max-width: 700px;

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

.encounter-info {
  margin-bottom: 24px;

  padding: 12px;

  background: #f8fafc;

  border-radius: 8px;

  border: 1px solid #e5e7eb;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;

  margin-bottom: 20px;
}

.field label {
  display: flex;

  align-items: center;

  gap: 8px;

  font-weight: 500;
}

input,
select {
  padding: 12px;

  border: 1px solid #d1d5db;

  border-radius: 8px;

  font-size: 14px;
}

.primary-button {
  padding: 12px 20px;

  border: none;

  border-radius: 8px;

  background: #2563eb;

  color: white;

  cursor: pointer;

  font-weight: 600;
}

.primary-button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}
</style>
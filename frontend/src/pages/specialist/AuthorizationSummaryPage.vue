<script setup lang="ts">
import { useRouter } from "vue-router";

import { storeToRefs } from "pinia";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

import {
  useEncounterStore
} from "../../stores/encounter.store";

const router = useRouter();

const authorizationStore =
  useAuthorizationStore();

const encounterStore =
  useEncounterStore();

const {
  authorizationRequestId,
  payerId,
  priority,
  services
} =
  storeToRefs(
    authorizationStore
  );

const submitAuthorization =
  () => {
    console.log(
      "Authorization Submitted"
    );

    router.push(
      "/specialist"
    );
  };
</script>

<template>
  <div class="page">

    <h1>
      Authorization Summary
    </h1>

    <div class="card">

      <p>
        <strong>
          Authorization Id:
        </strong>
        {{ authorizationRequestId }}
      </p>

      <p>
        <strong>
          Encounter Id:
        </strong>
        {{ encounterStore.encounterId }}
      </p>

      <p>
        <strong>
          Payer Id:
        </strong>
        {{ payerId }}
      </p>

      <p>
        <strong>
          Priority:
        </strong>
        {{ priority }}
      </p>

      <p>
        <strong>
          Status:
        </strong>
        Draft
      </p>

      <p>
        <strong>
          Estimated Amount:
        </strong>
        0
      </p>

      <p>
        <strong>
          Services Count:
        </strong>
        {{ services.length }}
      </p>

    </div>

    <div class="card">

      <h2>
        Services
      </h2>

      <div
        v-for="
          (service,index)
          in services
        "
        :key="index"
      >
        <p>
          CPT:
          {{ service.cptCode }}
        </p>

        <p>
          ICD:
          {{ service.icdCode }}
        </p>

        <p>
          Notes:
          {{ service.notes }}
        </p>

        <hr />
      </div>

    </div>

    <button
      class="submit-btn"
      @click="
        submitAuthorization()
      "
    >
      Submit Authorization
    </button>

  </div>
</template>

<style scoped>
.page {
  padding: 24px;
}

.card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
}

.submit-btn {
  padding: 12px 20px;
  border: none;
  border-radius: 8px;
  background: #16a34a;
  color: white;
  cursor: pointer;
}
</style>
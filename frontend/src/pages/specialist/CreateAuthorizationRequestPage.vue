<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

import { useEncounterStore }
from "../../stores/encounter.store";

import { useAuthorizationStore }
from "../../stores/authorization.store";

const router = useRouter();

const encounterStore =
  useEncounterStore();

const authorizationStore =
  useAuthorizationStore();

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

    <h1>
      Create Authorization Request
    </h1>

    <div class="card">

      <p>
        <strong>
          Encounter Id:
        </strong>

        {{
          encounterStore.encounterId
        }}
      </p>

      <div class="field">

        <label>
          Payer Id
        </label>

        <input
          v-model.number="payerId"
          type="number"
        />

      </div>

      <div class="field">

        <label>
          Priority
        </label>

        <select
          v-model="priority"
        >
          <option :value="0">
            Normal
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
}

.card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  max-width: 600px;
}

.field {
  display: flex;

  flex-direction: column;

  gap: 8px;

  margin-bottom: 20px;
}

input,
select {
  padding: 10px;

  border: 1px solid #d1d5db;

  border-radius: 8px;
}

.primary-button {
  padding: 12px 20px;

  border: none;

  border-radius: 8px;

  background: #2563eb;

  color: white;

  cursor: pointer;
}
</style>
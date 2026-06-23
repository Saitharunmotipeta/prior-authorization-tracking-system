<script setup lang="ts">
import { ref } from "vue";

import { storeToRefs } from "pinia";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";

const authorizationStore =
  useAuthorizationStore();

const {
  authorizationRequestId,
  services
} =
  storeToRefs(
    authorizationStore
  );

const cptCode =
  ref("");

const icdCode =
  ref("");

const notes =
  ref("");

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
</script>

<template>
  <div class="page">

    <h1>
      Add Authorization Services
    </h1>

    <div class="card">

      <p>
        <strong>
          Authorization Id:
        </strong>

        {{ authorizationRequestId }}
      </p>

      <input
        v-model="cptCode"
        placeholder="CPT Code"
      />

      <input
        v-model="icdCode"
        placeholder="ICD Code"
      />

      <textarea
        v-model="notes"
        placeholder="Notes"
      />

      <button
        @click="
          addService()
        "
      >
        Add Service
      </button>

    </div>

    <div class="card">

      <h2>
        Added Services
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

  </div>
</template>
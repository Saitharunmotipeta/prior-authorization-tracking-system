<script setup lang="ts">
import {
  computed
} from "vue";

import {
  RequestStatus
} from "../../enums/request-status.enum";

import {
  getStatusLabel
} from "../../utils/request-status";

const props =
  defineProps<{
    status: RequestStatus;
  }>();

const statusLabel =
  computed(() =>
    getStatusLabel(
      props.status
    )
  );
</script>

<template>
  <div
    class="status-badge"
  >
    {{ statusLabel }}
  </div>
</template>

<style scoped>
.status-badge {
  padding: 8px 16px;

  border-radius: 999px;

  background: #dbeafe;

  color: #1d4ed8;

  font-weight: 600;

  font-size: 14px;

  display: inline-block;
}
</style>
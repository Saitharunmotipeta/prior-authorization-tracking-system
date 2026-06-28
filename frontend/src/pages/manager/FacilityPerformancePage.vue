<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import FacilityComparisonTable
from "../../components/manager/FacilityComparisonTable.vue";

const dashboardStore = useManagerDashboardStore();

const {
  facilityComparison
} = storeToRefs(dashboardStore);

onMounted(async () => {
  if (!facilityComparison.value.length) {
    await dashboardStore.loadAllDashboardData();
  }
});
</script>

<template>
  <div class="table-section">

    <FacilityComparisonTable
      :facilities="facilityComparison"
    />

  </div>
</template>

<style scoped>
.table-section {
  margin-top: 24px;
}
</style>
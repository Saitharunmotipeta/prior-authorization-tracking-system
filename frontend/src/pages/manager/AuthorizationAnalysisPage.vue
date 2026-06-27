<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import StatusDistributionChart
from "../../components/manager/StatusDistributionChart.vue";

import DelayTrendChart
from "../../components/manager/DelayTrendChart.vue";

const dashboardStore = useManagerDashboardStore();

const {
  dashboard,
  delayTrends
} = storeToRefs(dashboardStore);

onMounted(async () => {
  if (!dashboard.value) {
    await dashboardStore.loadAllDashboardData();
  }
});
</script>

<template>
  <div
    v-if="dashboard"
    class="charts-grid"
  >

    <StatusDistributionChart
      :approved="dashboard.approvedRequests"
      :denied="dashboard.deniedRequests"
      :pending="dashboard.pendingRequests"
      :expired="dashboard.expiredRequests"
    />

    <DelayTrendChart
      :trends="delayTrends"
    />

  </div>
</template>

<style scoped>
.charts-grid {
  display: grid;
  gap: 24px;
}
</style>
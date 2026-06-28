<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import PayerPerformanceChart
from "../../components/manager/PayerPerformanceChart.vue";

import SlowPayersChart
from "../../components/manager/SlowPayersChart.vue";

import TopPerformingPayersTable
from "../../components/manager/TopPerformingPayersTable.vue";

import PoorPerformingPayersTable
from "../../components/manager/PoorPerformingPayersTable.vue";

const dashboardStore = useManagerDashboardStore();

const {
  payerPerformance,
  slowPayers,
  topPerformingPayers,
  poorPerformingPayers
} = storeToRefs(dashboardStore);

onMounted(async () => {
  if (!payerPerformance.value.length) {
    await dashboardStore.loadAllDashboardData();
  }
});
</script>

<template>
  <div>

    <div class="charts-grid">

      <PayerPerformanceChart
        :payer-performance="payerPerformance"
      />

      <SlowPayersChart
        :slow-payers="slowPayers"
      />

    </div>

    <div class="table-grid">

      <TopPerformingPayersTable
        :payers="topPerformingPayers"
      />

      <PoorPerformingPayersTable
        :payers="poorPerformingPayers"
      />

    </div>

  </div>
</template>

<style scoped>
.charts-grid,
.table-grid {
  display: grid;
  gap: 24px;
  margin-top: 24px;
}
</style>
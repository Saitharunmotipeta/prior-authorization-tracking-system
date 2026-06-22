<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import MetricCard from "../../components/manager/MetricCard.vue";
import StatusDistributionChart from "../../components/manager/StatusDistributionChart.vue";
import PayerPerformanceChart from "../../components/manager/PayerPerformanceChart.vue";
import SlowPayersChart
from "../../components/manager/SlowPayersChart.vue";

import RevenueRiskCard
from "../../components/manager/RevenueRiskCard.vue";

import FacilityComparisonTable from "../../components/manager/FacilityComparisonTable.vue";
import TopPerformingPayersTable from "../../components/manager/TopPerformingPayersTable.vue";
import PoorPerformingPayersTable from "../../components/manager/PoorPerformingPayersTable.vue";
import DelayTrendChart from "../../components/manager/DelayTrendChart.vue";

const dashboardStore = useManagerDashboardStore();

const {
  loading,
  error,
  dashboard,
  payerPerformance,
  slowPayers,
  revenueAtRisk,
  facilityComparison,
  topPerformingPayers,
  poorPerformingPayers,
  delayTrend
} = storeToRefs(dashboardStore);

onMounted(async () => {
  await dashboardStore.loadAllDashboardData();
});
</script>

<template>
  <div class="dashboard-page">
    <h1 class="page-title">
      Manager Dashboard
    </h1>

    <div
      v-if="loading"
      class="loading"
    >
      Loading Dashboard...
    </div>

    <div
      v-else-if="error"
      class="error"
    >
      {{ error }}
    </div>

    <div
      v-else-if="dashboard"
      class="dashboard-content"
    >
      <!-- KPI Cards -->
      <div class="metrics-grid">
        <MetricCard
          title="Total Encounters"
          :value="dashboard.totalEncounters"
        />

        <MetricCard
          title="Authorization Requests"
          :value="dashboard.totalAuthorizationRequests"
        />

        <MetricCard
          title="Approved Requests"
          :value="dashboard.approvedRequests"
        />

        <MetricCard
          title="Pending Requests"
          :value="dashboard.pendingRequests"
        />

        <MetricCard
          title="Approval Rate"
          :value="`${dashboard.approvalRate}%`"
        />

        <MetricCard
          title="Approved Revenue"
          :value="`₹${dashboard.approvedRevenue.toLocaleString()}`"
        />
      </div>

      <!-- Charts -->
      <div class="charts-grid">
        <StatusDistributionChart
          :approved="dashboard.approvedRequests"
          :denied="dashboard.deniedRequests"
          :pending="dashboard.pendingRequests"
          :expired="dashboard.expiredRequests"
        />

        <PayerPerformanceChart
          :payer-performance="payerPerformance"
        />

        <SlowPayersChart
            :slow-payers="slowPayers"
        />

        <RevenueRiskCard
            :revenue-at-risk="revenueAtRisk"
        />
      </div>

      <div class="table-section">
        <FacilityComparisonTable
            :facilities="facilityComparison"
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

        <div class="table-section">
        <DelayTrendChart
            :trends="delayTrends"
        />
        </div>
    </div>
  </div>
</template>

<style scoped>
.dashboard-page {
  padding: 24px;
  background: #f9fafb;
  min-height: 100vh;
}

.page-title {
  margin-bottom: 24px;
  font-size: 28px;
  font-weight: 600;
  color: #111827;
}

.metrics-grid {
  display: grid;
  grid-template-columns:
    repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
}

.charts-grid {
  margin-top: 24px;

  display: grid;

  grid-template-columns:
    repeat(2, minmax(0, 1fr));

  gap: 20px;
}

.loading,
.error {
  padding: 20px;
}

@media (max-width: 1024px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }
}

.table-grid {
  margin-top: 24px;

  display: grid;

  grid-template-columns: repeat(2, 1fr);

  gap: 20px;
}

.table-section {
  margin-top: 24px;
}
</style>
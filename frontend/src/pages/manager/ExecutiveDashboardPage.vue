<script setup lang="ts">
import { onMounted, ref } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import MetricCard from "../../components/manager/MetricCard.vue";
import StatusDistributionChart from "../../components/manager/StatusDistributionChart.vue";
import PayerPerformanceChart from "../../components/manager/PayerPerformanceChart.vue";
import SlowPayersChart from "../../components/manager/SlowPayersChart.vue";
import RevenueRiskCard from "../../components/manager/RevenueRiskCard.vue";
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

const activeTab = ref("overview");

onMounted(async () => {
  await dashboardStore.loadAllDashboardData();
});
</script>

<template>
  <div class="dashboard-page">
    <h1 class="page-title">
      Manager Dashboard
    </h1>

    <div class="dashboard-tabs">
      <button
        :class="{ active: activeTab === 'overview' }"
        @click="activeTab = 'overview'"
      >
        Overview
      </button>

      <button
        :class="{ active: activeTab === 'facility' }"
        @click="activeTab = 'facility'"
      >
        Facility Performance
      </button>

      <button
        :class="{ active: activeTab === 'payer' }"
        @click="activeTab = 'payer'"
      >
        Payer Analysis
      </button>

      <button
        :class="{ active: activeTab === 'authorization' }"
        @click="activeTab = 'authorization'"
      >
        Authorization Analysis
      </button>
    </div>

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
      <!-- OVERVIEW -->
      <template v-if="activeTab === 'overview'">
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

        <div class="charts-grid">
          <StatusDistributionChart
            :approved="dashboard.approvedRequests"
            :denied="dashboard.deniedRequests"
            :pending="dashboard.pendingRequests"
            :expired="dashboard.expiredRequests"
          />

          <RevenueRiskCard
            :revenue-at-risk="revenueAtRisk"
          />
        </div>
      </template>

      <!-- FACILITY PERFORMANCE -->
      <template v-if="activeTab === 'facility'">
        <div class="table-section">
          <FacilityComparisonTable
            :facilities="facilityComparison"
          />
        </div>
      </template>

      <!-- PAYER ANALYSIS -->
      <template v-if="activeTab === 'payer'">
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
      </template>

      <!-- AUTHORIZATION ANALYSIS -->
      <template v-if="activeTab === 'authorization'">
        <div class="charts-grid">
          <StatusDistributionChart
            :approved="dashboard.approvedRequests"
            :denied="dashboard.deniedRequests"
            :pending="dashboard.pendingRequests"
            :expired="dashboard.expiredRequests"
          />

          <DelayTrendChart
            :trends="delayTrend"
          />
        </div>
      </template>
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
  margin-bottom: 20px;
  font-size: 28px;
  font-weight: 700;
  color: #111827;
}

.dashboard-tabs {
  display: flex;
  gap: 12px;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 1px solid #e5e7eb;
}

.dashboard-tabs button {
  border: none;
  background: transparent;
  padding: 10px 18px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 600;
  color: #6b7280;
  transition: all 0.2s ease;
}

.dashboard-tabs button:hover {
  color: #2563eb;
}

.dashboard-tabs button.active {
  color: #2563eb;
  border-bottom: 2px solid #2563eb;
}

.metrics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  font-size: 100px;
}

.charts-grid {
  margin-top: 24px;
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 20px;
}

.table-grid {
  margin-top: 24px;
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 20px;
}

.table-section {
  margin-top: 24px;
}

.loading,
.error {
  padding: 20px;
  font-size: 16px;
}

@media (max-width: 1024px) {
  .charts-grid,
  .table-grid {
    grid-template-columns: 1fr;
  }

  .dashboard-tabs {
    flex-wrap: wrap;
  }
}
</style>
<script setup lang="ts">
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useManagerDashboardStore } from "../../stores/managerDashboard.store";

import MetricCard from "../../components/manager/MetricCard.vue";
import RevenueRiskCard from "../../components/manager/RevenueRiskCard.vue";

const dashboardStore = useManagerDashboardStore();

const {
  loading,
  error,
  dashboard,
  revenueAtRisk
} = storeToRefs(dashboardStore);

onMounted(async () => {
  if (!dashboard.value) {
    await dashboardStore.loadAllDashboardData();
  }
});
</script>

<template>
  <div class="dashboard-main">

    <div class="page-header">

      <h1>
        Manager Dashboard
      </h1>

      <p>
        Monitor authorization performance, payer analytics,
        facility metrics, and revenue insights.
      </p>

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

        <RevenueRiskCard
          :revenue-at-risk="revenueAtRisk"
        />

      </div>

    </div>

  </div>
</template>

<style scoped>
.dashboard-main {
  width: 100%;
}

.page-header {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 32px;
  margin-bottom: 28px;
  box-shadow: 0 2px 8px rgb(0 0 0 / 5%);
}

.page-header h1 {
  margin: 0;
  color: #1e293b;
}

.page-header p {
  margin-top: 8px;
  margin-bottom: 0;
  font-size: 14px;
  color: #64748b;
}

.metrics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 20px;
}

.charts-grid {
  margin-top: 24px;
  display: grid;
  gap: 24px;
}

.loading,
.error {
  padding: 20px;
  font-size: 18px;
}

@media (max-width: 768px) {
  .page-header {
    padding: 24px;
  }

  .metrics-grid {
    grid-template-columns: 1fr;
  }
}
</style>
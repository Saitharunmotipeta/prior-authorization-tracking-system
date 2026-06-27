<script setup lang="ts">
import { onMounted} from "vue";
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


onMounted(async () => {
  await dashboardStore.loadAllDashboardData();
});
</script>

<template>
  <div class="dashboard-page">
    <div class="dashboard-main">
    <div class="page-header">

      <h1>
        Manager Dashboard
      </h1>

      <p>
        Monitor authorization performance, payer analytics, facility metrics, and revenue insights.
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
      <div>
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
    </div>
  </div>
</template>

<style scoped>
.dashboard-layout {
  display: flex;
  min-height: 100vh;
  background: #eff6ff;
}

.sidebar {
  width: 260px;
  background: linear-gradient(
    180deg,
    #1e40af,
    #2563eb
  );
  padding: 24px 16px;
  position: sticky;
  top: 0;
  height: 100vh;
  display: flex;
  flex-direction: column;
  gap: 10px;
  box-shadow: 4px 0 16px rgba(37, 99, 235, 0.15);
}

.sidebar-header {
  text-align: center;
  margin-bottom: 24px;
}

.sidebar-header h2 {
  color: white;
  margin: 0;
  font-size: 26px;
  font-weight: 700;
}

.sidebar-header p {
  color: #dbeafe;
  margin-top: 6px;
  font-size: 14px;
}

.sidebar button {
  display: flex;
  align-items: center;
  gap: 12px;

  border: none;
  background: transparent;
  color: #dbeafe;
  padding: 14px 18px;
  border-radius: 10px;
  cursor: pointer;
  font-size: 15px;
  font-weight: 600;
}

.sidebar button:hover {
  background: rgba(255, 255, 255, 0.15);
  color: white;
}

.sidebar button.active {
  background: white;
  color: #2563eb;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.dashboard-main {
  flex: 1;
  padding: 30px;
}

.page-header {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 36px 32px;
  margin-bottom: 32px;
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
  grid-template-columns: 1fr;
  gap: 24px;
}

.table-grid {
  margin-top: 24px;
  display: grid;
  grid-template-columns: 1fr;
  gap: 24px;
}

.table-section {
  margin-top: 24px;
}

.loading,
.error {
  padding: 20px;
  font-size: 18px;
}

@media (max-width: 1024px) {
  .dashboard-layout {
    flex-direction: column;
  }

  .sidebar {
    width: 100%;
    height: auto;
    position: relative;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center;
  }

.dashboard-main {
  flex: 1;
  padding: 30px;
  overflow-x: hidden;
}

  .charts-grid,
  .table-grid {
    grid-template-columns: 1fr;
  }
}
</style>
<script setup lang="ts">
import type {
  TopPerformingPayer
} from "../../types/dashboard.interface";

interface Props {
  payers: TopPerformingPayer[];
}

defineProps<Props>();
</script>

<template>
  <div class="table-card">
    <h3 style="text-align: center;">
      Top Performing Payers
    </h3>

    <table>
      <thead>
        <tr>
          <th>Rank</th>
          <th>Payer</th>
          <th>Requests</th>
          <th>Approved</th>
          <th>Approval %</th>
        </tr>
      </thead>

      <tbody>
        <tr
          v-for="payer in payers"
          :key="payer.rank"
        >
        <td>
          <span class="rank-badge">
            {{ payer.rank }}
          </span>
        </td>
          <td>{{ payer.payerName }}</td>
          <td>{{ payer.totalRequests }}</td>
          <td>{{ payer.approvedRequests }}</td>
          <td>{{ payer.approvalRate }}%</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-card {
  background: white;
  padding: 24px;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
}

.table-card h3 {
  margin: 0 0 20px;
  font-size: 20px;
  font-weight: 600;
  color: #111827;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  padding: 14px 16px;
  text-align: left;
  font-size: 14px;
  font-weight: 600;
  color: #374151;
  border-bottom: 2px solid #e5e7eb;
}

tbody td {
  padding: 14px 16px;
  font-size: 14px;
  color: #111827;
  border-bottom: 1px solid #f3f4f6;
}

tbody tr:hover {
  background: #f9fafb;
}

tbody tr:last-child td {
  border-bottom: none;
}

/* Center numeric columns */
th:nth-child(1),
th:nth-child(3),
th:nth-child(4),
th:nth-child(5),
td:nth-child(1),
td:nth-child(3),
td:nth-child(4),
td:nth-child(5) {
  text-align: center;
}

/* Payer name column */
td:nth-child(2) {
  font-weight: 500;
}

.rank-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: 999px;
  background: #dbeafe;
  color: #2563eb;
  font-weight: 600;
}
</style>
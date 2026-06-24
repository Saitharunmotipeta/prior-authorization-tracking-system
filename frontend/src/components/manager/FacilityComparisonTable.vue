<script setup lang="ts">
import type {
  FacilityComparison
} from "../../types/dashboard.interface";

interface Props {
  facilities: FacilityComparison[];
}

defineProps<Props>();
</script>

<template>
  <div class="table-card">
    <h3>Facility Comparison</h3>

    <table>
      <thead>
        <tr>
          <th>Rank</th>
          <th>Facility</th>
          <th>Requests</th>
          <th>Approval %</th>
          <th>Revenue</th>
          <th>Avg Days</th>
        </tr>
      </thead>

      <tbody>
        <tr
          v-for="facility in facilities"
          :key="facility.rank"
        >
          <td>{{ facility.rank }}</td>
          <td>{{ facility.facilityName }}</td>
          <td>{{ facility.totalRequests }}</td>
          <td>{{ facility.approvalRate }}%</td>
          <td>
            ₹{{ facility.approvedRevenue.toLocaleString() }}
          </td>
          <td>{{ facility.averageResponseDays }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-card {
  background: white;
  padding: 20px;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
}

.table-card h3 {
  text-align: center;
  margin-bottom: 20px;
  font-size: 20px;
  font-weight: 600;
  color: #111827;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 12px;
  text-align: center;
  border-bottom: 1px solid #e5e7eb;
}

th {
  font-weight: 600;
  color: #374151;
}

td {
  color: #111827;
}

tbody tr:hover {
  background: #f9fafb;
}
</style>
<script setup lang="ts">
import type {
  PoorPerformingPayer
} from "../../types/dashboard.interface";

interface Props {
  payers: PoorPerformingPayer[];
}

defineProps<Props>();
</script>

<template>
  <div class="table-card">
    <h3>Poor Performing Payers</h3>

    <table>
      <thead>
        <tr>
          <th>Rank</th>
          <th>Payer</th>
          <th>Requests</th>
          <th>Denied</th>
          <th>Denial %</th>
        </tr>
      </thead>

      <tbody>
        <tr
          v-for="payer in payers"
          :key="payer.rank"
        >
          <td>{{ payer.rank }}</td>
          <td>{{ payer.payerName }}</td>
          <td>{{ payer.totalRequests }}</td>
          <td>{{ payer.deniedRequests }}</td>
          <td>{{ payer.denialRate }}%</td>
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
</style>
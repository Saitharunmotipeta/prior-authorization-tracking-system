<script setup lang="ts">
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend
} from "chart.js";

import { Doughnut } from "vue-chartjs";

ChartJS.register(
  ArcElement,
  Tooltip,
  Legend
);

interface Props {
  approved: number;
  denied: number;
  pending: number;
  expired: number;
}

const props = defineProps<Props>();

const chartData = {
  labels: [
    "Approved",
    "Denied",
    "Pending",
    "Expired"
  ],

  datasets: [
    {
      data: [
        props.approved,
        props.denied,
        props.pending,
        props.expired
      ],

      backgroundColor: [
        "#22c55e",
        "#ef4444",
        "#f59e0b",
        "#6b7280"
      ],

      borderWidth: 1
    }
  ]
};

const chartOptions = {
  responsive: true,

  maintainAspectRatio: false,

  plugins: {
    legend: {
      position: "bottom" as const
    }
  }
};
</script>

<template>
  <div class="chart-card">

  <h3 style="text-align: center;">
    Authorization Status Distribution
  </h3>

    <div class="chart-wrapper">
      <Doughnut
        :data="chartData"
        :options="chartOptions"
      />
    </div>

  </div>
</template>

<style scoped>
.chart-card {
  background: white;

  padding: 20px;

  border-radius: 12px;

  border: 1px solid #e5e7eb;
}

.chart-wrapper {
  height: 350px;
}
</style>
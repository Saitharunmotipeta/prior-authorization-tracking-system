<script setup lang="ts">
import { computed } from "vue";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
} from "chart.js";

import { Bar } from "vue-chartjs";

import type {
  SlowPayer
} from "../../types/dashboard.interface";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

interface Props {
  slowPayers: SlowPayer[];
}

const props = defineProps<Props>();

const chartData = computed(() => ({
  labels:
    props.slowPayers?.map(
      payer => payer.payerName
    ) ?? [],

  datasets: [
    {
      label: "Average Response Days",

      data:
        props.slowPayers?.map(
          payer => payer.averageResponseDays
        ) ?? [],

      backgroundColor: "#ef4444",

      borderRadius: 8
    }
  ]
}));

const chartOptions = {
  responsive: true,

  maintainAspectRatio: false,

  plugins: {
    title: {
      display: true,
      text: "Slowest Payers"
    },

    legend: {
      display: false
    }
  }
};
</script>

<template>
  <div class="chart-card">
    <div class="chart-wrapper">
      <Bar
        :data="chartData"
        :options="chartOptions"
      />
    </div>
  </div>
</template>

<style scoped>
.chart-card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 20px;
}

.chart-wrapper {
  height: 350px;
}
</style>
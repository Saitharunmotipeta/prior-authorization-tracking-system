<script setup lang="ts">
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
  PayerPerformance
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
  payerPerformance: PayerPerformance[];
}

const props = defineProps<Props>();

const chartData = {
  labels: props.payerPerformance.map(
    payer => payer.payerName
  ),

  datasets: [
    {
      label: "Approval Rate (%)",

      data: props.payerPerformance.map(
        payer => payer.approvalRate
      ),

      backgroundColor: "#2563eb",

      borderRadius: 8
    }
  ]
};

const chartOptions = {
  responsive: true,

  maintainAspectRatio: false,

  plugins: {
    legend: {
      display: false
    },

    title: {
      display: true,
      text: "Payer Performance"
    }
  },

  scales: {
    y: {
      beginAtZero: true,

      max: 100
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
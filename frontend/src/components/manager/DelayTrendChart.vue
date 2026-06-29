<script setup lang="ts">
import { computed } from "vue";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Tooltip,
  Legend
} from "chart.js";

import { Bar } from "vue-chartjs";

import type {
  DelayTrend
} from "../../types/dashboard.interface";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Tooltip,
  Legend
);

interface Props {
  trends: DelayTrend[];
}

const props = defineProps<Props>();

const chartData = computed(() => ({
  labels: props.trends?.map(x => x.payerName) ?? [],
  datasets: [
    {
      label: "0–2 Days",
      data: props.trends?.map(x => x.zeroToTwoDays) ?? [],
      backgroundColor: "#22c55e"
    },
    {
      label: "3–5 Days",
      data: props.trends?.map(x => x.threeToFiveDays) ?? [],
      backgroundColor: "#f59e0b"
    },
    {
      label: "6–10 Days",
      data: props.trends?.map(x => x.sixToTenDays) ?? [],
      backgroundColor: "#ef4444"
    },
    {
      label: ">10 Days",
      data: props.trends?.map(x => x.moreThanTenDays) ?? [],
      backgroundColor: "#7c3aed"
    }
  ]
}));
</script>

<template>
<div class="chart-card">

  <h3 class="chart-title">
    Authorization Processing Time by Payer
  </h3>
  <Bar
    :data="chartData"
  />
</div>
</template>

<style scoped>
.chart-card {
  background: white;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  padding: 20px;
}
.chart-title {
  margin: 0 0 20px;
  text-align: center;
  font-size: 20px;
  font-weight: 700;
  color: #1e293b;
}
</style>
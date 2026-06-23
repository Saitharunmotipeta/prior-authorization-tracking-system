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
  labels:
    props.trends?.map(
      x => x.payerName
    ) ?? [],

  datasets: [
    {
      label: "0-2 Days",
      data:
        props.trends?.map(
          x => x.zeroToTwoDays
        ) ?? []
    },
    {
      label: "3-5 Days",
      data:
        props.trends?.map(
          x => x.threeToFiveDays
        ) ?? []
    },
    {
      label: "6-10 Days",
      data:
        props.trends?.map(
          x => x.sixToTenDays
        ) ?? []
    },
    {
      label: ">10 Days",
      data:
        props.trends?.map(
          x => x.moreThanTenDays
        ) ?? []
    }
  ]
}));
</script>

<template>
  <div class="chart-card">
    <Bar :data="chartData" />
  </div>
</template>

<style scoped>
.chart-card {
  background: white;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  padding: 20px;
}
</style>
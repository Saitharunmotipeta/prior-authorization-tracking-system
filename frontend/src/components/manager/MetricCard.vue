<script setup lang="ts">
interface Props {
  title: string;
  value: string | number;
  subtitle?: string;
}

defineProps<Props>();
</script>

<template>
  <div class="metric-card">
    <div class="metric-title">
      {{ title }}
    </div>

    <div class="metric-value">
      {{ value }}
    </div>

    <div
      v-if="subtitle"
      class="metric-subtitle"
    >
      {{ subtitle }}
    </div>
  </div>
</template>

<style scoped>
.metric-card {
  background: white;
  border-radius: 12px;
  padding: 24px;
  text-align: center;
  border: 1px solid #dbeafe;
}

.metric-title {
  font-size: 16px;
  color: #1e293b;
  margin-bottom: 12px;
}

.metric-value {
  font-size: 36px;
  font-weight: 700;
  color: #1e293b;
}

.metric-subtitle {
  margin-top: 8px;
  font-size: 13px;
  color: #9ca3af;
}
</style>
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
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 20px;
  min-height: 120px;

  display: flex;
  flex-direction: column;
  justify-content: center;

  box-shadow:
    0 1px 2px rgba(0,0,0,0.05);
}

.metric-title {
  font-size: 14px;
  color: #6b7280;
  margin-bottom: 12px;
}

.metric-value {
  font-size: 32px;
  font-weight: 700;
  color: #111827;
}

.metric-subtitle {
  margin-top: 8px;
  font-size: 13px;
  color: #9ca3af;
}
</style>
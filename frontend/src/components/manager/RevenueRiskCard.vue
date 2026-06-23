<script setup lang="ts">
import type {
  RevenueAtRisk
} from "../../types/dashboard.interface";

interface Props {
  revenueAtRisk: RevenueAtRisk | null;
}

defineProps<Props>();
</script>

<template>
  <div class="risk-card">

    <div class="title">
      Revenue At Risk
    </div>

    <div class="amount">
      ₹{{
        revenueAtRisk?.revenueAtRisk?.toLocaleString()
        ?? 0
      }}
    </div>

    <div class="subtitle">
      {{
        revenueAtRisk?.deniedRequests ?? 0
      }}
      Denied Requests
    </div>

  </div>
</template>

<style scoped>
.risk-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  min-height: 350px;

  display: flex;

  flex-direction: column;

  justify-content: center;

  align-items: center;
}

.title {
  font-size: 18px;
  color: #6b7280;
}

.amount {
  font-size: 42px;
  font-weight: 700;
  color: #dc2626;

  margin: 16px 0;
}

.subtitle {
  font-size: 16px;
  color: #6b7280;
}
</style>
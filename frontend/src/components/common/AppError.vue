<script setup lang="ts">
defineProps<{
  message: string | null;
}>();
</script>

<template>
  <div
    v-if="message"
    class="error-container"
  >
    <div class="error-icon">
      ⚠
    </div>

    <div class="error-message">
      {{ message }}
    </div>
  </div>
</template>

<style scoped>
.error-container {
  display: flex;

  align-items: center;

  gap: 12px;

  padding: 14px 18px;

  margin-bottom: 20px;

  border-radius: 10px;

  border: 1px solid #fecaca;

  background: #fef2f2;

  color: #b91c1c;
}

.error-icon {
  font-size: 20px;

  font-weight: bold;
}

.error-message {
  flex: 1;

  font-size: 14px;

  font-weight: 500;
}
</style>
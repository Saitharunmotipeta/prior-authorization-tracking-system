<script setup lang="ts">
import { ref, onMounted } from 'vue'

// ✅ STATE
const reminders = ref<any[]>([])
const loading = ref(false)
const error = ref('')

// ✅ FETCH REMINDERS
const fetchReminders = async () => {
  try {
    loading.value = true

    const res = await fetch('https://localhost:7190/api/payers/reminders')
    const result = await res.json()

    if (!result.success) {
      throw new Error(result.message)
    }

    reminders.value = result.data.data
  } catch (err: any) {
    error.value = err.message || 'Error loading reminders'
  } finally {
    loading.value = false
  }
}

// ✅ LOAD DATA
onMounted(() => {
  fetchReminders()
})
</script>

<template>
  <div class="reminders-container">
    <h2>Reminders 🔔</h2>

    <!-- ✅ Loading -->
    <p v-if="loading">Loading...</p>

    <!-- ✅ Error -->
    <p v-else-if="error" class="error">{{ error }}</p>

    <!-- ✅ Data -->
    <div v-else>
      <div
        v-for="r in reminders"
        :key="r.reminderId"
        class="reminder-card"
      >
        <p><strong>Category:</strong> {{ r.category }}</p>
        <p><strong>Status:</strong> {{ r.status }}</p>
        <p><strong>Scheduled:</strong> {{ r.scheduledAt }}</p>
        <p><strong>Remarks:</strong> {{ r.remarks || '-' }}</p>
      </div>

      <p v-if="reminders.length === 0">No reminders found</p>
    </div>
  </div>
</template>

<style scoped>
.reminders-container {
  padding: 20px;
}

.reminder-card {
  border: 1px solid #ddd;
  padding: 12px;
  margin-bottom: 10px;
  border-left: 4px solid #0072CE;
  background: #fff;
}

.error {
  color: red;
}
</style>
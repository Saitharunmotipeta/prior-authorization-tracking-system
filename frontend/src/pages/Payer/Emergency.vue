<script setup lang="ts">
import { ref, onMounted } from 'vue'

const emergencyRequests = ref<any[]>([])
const loading = ref(false)
const error = ref('')

// ✅ Fetch emergency requests
const fetchEmergency = async () => {
  try {
    loading.value = true

    const res = await fetch('https://localhost:7190/api/payers/emergency')
    const result = await res.json()

    if (!result.success) {
      throw new Error(result.message)
    }

    emergencyRequests.value = result.data

  } catch (err: any) {
    error.value = err.message || 'Error fetching emergency requests'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchEmergency()
})
</script>

<template>
  <div>
    <h2 class="mb-3">🚨 Emergency Requests</h2>

    <div v-if="loading">Loading...</div>
    <div v-if="error" class="text-danger">{{ error }}</div>

    <div v-if="emergencyRequests.length === 0 && !loading">
      No emergency requests
    </div>

    <div v-for="e in emergencyRequests" :key="e.authId" class="card mb-2 shadow">
      <div class="card-body">
        <h5>{{ e.patientName }}</h5>

        <p>
          Facility: {{ e.facilityName }} <br />
          Status:
          <span
            class="badge"
            :style="{
              backgroundColor:
                e.status === 'Approved' ? 'green' :
                e.status === 'Denied' ? 'red' :
                '#F59E0B'
            }"
          >
            {{ e.status }}
          </span>
        </p>

        <strong>₹{{ e.estimatedAmount }}</strong>
      </div>
    </div>
  </div>
</template>
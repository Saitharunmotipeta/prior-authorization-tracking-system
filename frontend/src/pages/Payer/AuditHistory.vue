<script setup lang="ts">
import { ref, onMounted } from 'vue'

const auditHistory = ref<any[]>([])
const loading = ref(false)
const error = ref('')

// ✅ Fetch audit history
const fetchAudit = async () => {
  try {
    loading.value = true

    const res = await fetch('https://localhost:7190/api/payers/audit-history')
    const result = await res.json()

    if (!result.success) {
      throw new Error(result.message)
    }

    auditHistory.value = result.data

  } catch (err: any) {
    error.value = err.message || 'Error fetching audit history'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchAudit()
})
</script>

<template>
  <div>
    <h2 class="mb-3">📜 Audit History</h2>

    <div v-if="loading">Loading...</div>
    <div v-if="error" class="text-danger">{{ error }}</div>

    <div v-if="auditHistory.length === 0 && !loading">
      No audit records
    </div>

    <div v-for="a in auditHistory" :key="a.auditId" class="card mb-2 shadow">
      <div class="card-body">

        <h6>{{ a.actionType }}</h6>

        <p>
          Auth ID: {{ a.authId }} <br />
          Remarks: {{ a.remarks || '-' }}
        </p>

        <small class="text-muted">
          {{ a.createdAt }}
        </small>

      </div>
    </div>
  </div>
</template>
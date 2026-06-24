<script setup lang="ts">
import { ref, watch } from 'vue'

// ✅ props (receive authId from parent)
const props = defineProps<{
  authId: number | null
}>()

// ✅ state
const details = ref<any>(null)

const actionType = ref<number | null>(null)
const approvedAmount = ref('')
const remarks = ref('')

// ✅ fetch details when authId changes
watch(() => props.authId, async (newId) => {
  if (!newId) return

  const res = await fetch(`https://localhost:7190/api/payers/${newId}`)
  const result = await res.json()

  details.value = result.data
})

// ✅ button select
const setAction = (type: number) => {
  actionType.value = type
  approvedAmount.value = ''
  remarks.value = ''
}

// ✅ submit review
const submitReview = async () => {
  if (!props.authId) return

  const res = await fetch(
    `https://localhost:7190/api/payers/${props.authId}/review`,
    {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        action: actionType.value,
        approvedAmount: actionType.value === 1 ? Number(approvedAmount.value) : null,
        remarks: actionType.value !== 1 ? remarks.value : null
      })
    }
  )

  const result = await res.json()

  if (result.success) {
    alert('✅ Review submitted')

    // refresh details after update
    const refresh = await fetch(`https://localhost:7190/api/payers/${props.authId}`)
    const updated = await refresh.json()
    details.value = updated.data
  }
}
</script>

<template>
  <div class="panel">
    <h3>Details</h3>

    <div v-if="details">

      <p><strong>Patient:</strong> {{ details.patientName }}</p>
      <p><strong>Facility:</strong> {{ details.facilityName }}</p>
      <p><strong>Status:</strong> {{ details.status }}</p>

      <p><strong>Estimated:</strong> ₹{{ details.estimatedAmount }}</p>
      <p><strong>Approved:</strong> ₹{{ details.approvedAmount }}</p>

      <!-- ✅ ACTION BUTTONS -->
      <div class="actions">
        <button @click="setAction(1)">Approve</button>
        <button @click="setAction(2)">Deny</button>
        <button @click="setAction(3)">Additional Info</button>
      </div>

      <!-- ✅ INPUTS -->
      <div v-if="actionType === 1">
        <input v-model="approvedAmount" placeholder="Enter amount" />
      </div>

      <div v-if="actionType === 2 || actionType === 3">
        <textarea v-model="remarks" placeholder="Enter remarks"></textarea>
      </div>

      <button v-if="actionType" @click="submitReview">
        Submit
      </button>

    </div>

    <p v-else>Select a request</p>
  </div>
</template>

<style scoped>
.panel {
  padding: 15px;
}

.actions button {
  margin-right: 8px;
}
</style>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Review from './Review.vue'

// ✅ STATE
const facilities = ref<any[]>([])
const requests = ref<any[]>([])
const selectedFacilityId = ref<number | null>(null)
const selectedRequestId = ref<number | null>(null)

// ✅ FETCH FACILITIES
const fetchFacilities = async () => {
  const res = await fetch('https://localhost:7190/api/payers/facilities')
  const result = await res.json()
  facilities.value = result.data
}

// ✅ FETCH REQUESTS (on facility click)
const fetchRequests = async (facilityId: number) => {
  selectedFacilityId.value = facilityId
  selectedRequestId.value = null // reset right panel

  const res = await fetch(
    `https://localhost:7190/api/payers/facilities/${facilityId}/authorization-requests`
  )
  const result = await res.json()

  requests.value = result.data
}

// ✅ LOAD DATA ON PAGE LOAD
onMounted(() => {
  fetchFacilities()
})
</script>

<template>
  <div class="row">

    <!-- ✅ FACILITIES -->
    <div class="col-md-3">
      <div class="card shadow">
        <div class="card-header text-white" style="background-color:#003B8F;">
          Facilities
        </div>

        <ul class="list-group list-group-flush">
          <li
            v-for="f in facilities"
            :key="f.facilityId"
            @click="fetchRequests(f.facilityId)"
            class="list-group-item d-flex justify-content-between align-items-center"
            style="cursor:pointer;"
          >
            {{ f.facilityName }}

            <span class="badge" style="background-color:#F59E0B;">
              {{ f.pendingCount }}
            </span>
          </li>
        </ul>
      </div>
    </div>

    <!-- ✅ REQUESTS -->
    <div class="col-md-4">
      <div class="card shadow">
        <div class="card-header text-white" style="background-color:#003B8F;">
          Requests
        </div>

        <ul class="list-group list-group-flush">
          <li
            v-for="r in requests"
            :key="r.authId"
            @click="selectedRequestId = r.authId"
            class="list-group-item"
            style="cursor:pointer;"
          >
            <div><strong>{{ r.patientName }}</strong></div>

            <span
              class="badge mt-1"
              :style="{
                backgroundColor:
                  r.status === 'Approved' ? 'green' :
                  r.status === 'Denied' ? 'red' :
                  '#F59E0B'
              }"
            >
              {{ r.status }}
            </span>
          </li>
        </ul>
      </div>
    </div>

    <!-- ✅ REVIEW PANEL -->
    <div class="col-md-5">
      <div class="card shadow">
        <div class="card-header text-white" style="background-color:#003B8F;">
          Details
        </div>

        <div class="card-body">
          <Review :authId="selectedRequestId" />
        </div>
      </div>
    </div>

  </div>
</template>


<style scoped>
.container {
  display: flex;
  gap: 20px;
  padding: 20px;
}

.panel {
  flex: 1;
  border: 1px solid #ddd;
  padding: 15px;
  background: white;
}

li {
  cursor: pointer;
  padding: 8px;
}

li:hover {
  background: #E6F1FB;
}

.active {
  background: #cfe2ff;
}
</style>

<template>
  <div class="container">

    <!-- ✅ LEFT → FACILITIES -->
    <div class="panel">
      <h3>Facilities</h3>

      <p v-if="facilities.length === 0">Loading...</p>

      <ul>
        <li
          v-for="f in facilities"
          :key="f.facilityId"
          @click="fetchRequests(f.facilityId)"
        >
          {{ f.facilityName }} ({{ f.pendingCount }})
        </li>
      </ul>
    </div>

    <!-- ✅ MIDDLE → REQUESTS -->
    <div class="panel">
      <h3>Requests</h3>

      <p v-if="requests.length === 0">Select a facility</p>

      <ul>
        <li
          v-for="r in requests"
          :key="r.authId"
          @click="selectedRequestId = r.authId"
        >
          {{ r.patientName }} - {{ r.status }}
        </li>
      </ul>
    </div>

    <!-- ✅ RIGHT → REVIEW PANEL -->
    <div class="panel">
      <Review :authId="selectedRequestId" />
    </div>

  </div>
</template>

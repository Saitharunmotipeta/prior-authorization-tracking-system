<script setup lang="ts">
import {
  ref,
  onMounted,
  computed,
  watch
} from "vue";

import {
  storeToRefs
} from "pinia";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

const specialistStore =
  useSpecialistStore();

const {
  authorizationRequests,
  authorizationServices,
  authorizationTimeline,
  loading
} =
  storeToRefs(
    specialistStore
  );

const currentPage =
  ref(1);

const pageSize =
  8;

const searchText =
  ref("");

onMounted(() => {

  specialistStore
    .loadAuthorizationRequests();

});

const filteredRequests =
computed(() => {

  if (
    !searchText.value.trim()
  ) {

    return authorizationRequests.value;

  }

  const keyword =
    searchText.value
      .toLowerCase();

  return authorizationRequests.value.filter(
    request =>

      request.authId
        .toString()
        .includes(keyword)

      ||

      request.patientName
        .toLowerCase()
        .includes(keyword)

      ||

      request.payerName
        .toLowerCase()
        .includes(keyword)

      ||

      request.status
        .toLowerCase()
        .includes(keyword)

      ||

      request.priority
        .toLowerCase()
        .includes(keyword)

  );

});

const totalPages =
computed(() =>
  Math.max(
    1,
    Math.ceil(
      filteredRequests.value.length /
      pageSize
    )
  )
);

const paginatedRequests =
computed(() => {

  const start =
    (currentPage.value - 1) *
    pageSize;

  return filteredRequests.value.slice(
    start,
    start + pageSize
  );

});

watch(
  searchText,
  () => {

    currentPage.value = 1;

  }
);

const formatDate = (
  date: string | null
) => {

  if (!date)
    return "-";

  return new Date(date)
    .toLocaleDateString();

};

const showHistoryModal =
  ref(false);

const selectedAuthId =
  ref<number | null>(null);

const viewRequestHistory = async (
  authId: number
) => {

  console.log("Clicked:", authId);

  selectedAuthId.value = authId;

  showTimeline.value = false;

  authorizationTimeline.value = [];

  await specialistStore.loadAuthorizationServices(authId);

  console.log(
    "Services:",
    authorizationServices.value
  );

  showHistoryModal.value = true;
};

const closeHistoryModal =
() => {

  showHistoryModal.value =
    false;

  showTimeline.value =
    false;

  selectedAuthId.value =
    null;

  authorizationTimeline.value =
    [];

};

const showTimeline =
  ref(false);

const viewTimeline =
async () => {

  if (
    !selectedAuthId.value
  )
    return;

  await specialistStore
    .loadAuthorizationTimeline(
      selectedAuthId.value
    );

  showTimeline.value =
    true;

};
</script>

<template>

<div class="table-card">
<div class="table-header">

  <h3>
    Authorization Requests
  </h3>

  <div class="toolbar">

    <span class="count">

      {{ filteredRequests.length }}
      Requests

    </span>

    <input
      v-model="searchText"
      class="search-box"
      placeholder="Search Authorization Id, Patient, Payer, Status..."
    />

  </div>

</div>

<table
v-if="!loading"
class="table"
>

<thead>

<tr>

<th>ID</th>

<th>Patient</th>

<th>Payer</th>

<th>Status</th>

<th>Priority</th>

<th>Estimated Amount</th>

<th>Action</th>

</tr>

</thead>
<tbody>

<tr
v-for="request in paginatedRequests"
:key="request.authId"
>

<td>

{{ request.authId }}

</td>

<td>

{{ request.patientName }}

</td>

<td>

{{ request.payerName }}

</td>

<td>

<span
class="badge"
:class="'status-' + request.status.toLowerCase().replace(/\s+/g,'-')"
>

{{ request.status }}

</span>

</td>

<td>

<span
class="badge"
:class="'priority-' + request.priority.toLowerCase()"
>

{{ request.priority }}

</span>

</td>

<td>

{{ request.estimatedAmount }}

</td>

<td>

<button
class="view-button"
@click="viewRequestHistory(request.authId)"
>

<i class="pi pi-eye"></i>

View

</button>

</td>

</tr>

<tr
v-if="paginatedRequests.length===0"
>

<td
colspan="7"
class="empty-row"
>

No authorization requests found.

</td>

</tr>

</tbody>

</table>

<div
class="pagination"
v-if="filteredRequests.length"
>

<button
@click="currentPage--"
:disabled="currentPage===1"
>

Previous

</button>

<span>

Page

{{ currentPage }}

of

{{ totalPages }}

</span>

<button
@click="currentPage++"
:disabled="currentPage===totalPages"
>

Next

</button>

</div>

</div>

<div
  v-if="showHistoryModal"
  class="drawer-overlay"
  @click="closeHistoryModal"
>

  <aside
    class="drawer"
    @click.stop
  >

    <div class="drawer-header">

      <h2>Authorization Details</h2>

      <button
        class="close-btn"
        @click="closeHistoryModal"
      >
        ✕
      </button>

    </div>

    <div class="drawer-body">

      <div class="details-card">

        <p>
          <strong>Authorization ID:</strong>
          {{ selectedAuthId }}
        </p>

      </div>

      <div class="details-card">

        <h3>Services</h3>

        <table class="services-table">

          <thead>
            <tr>
              <th>CPT Code</th>
              <th>ICD Code</th>
              <th>Estimated Cost</th>
              <th>Notes</th>
            </tr>
          </thead>

          <tbody>

            <tr
              v-for="service in authorizationServices"
              :key="service.cptCode"
            >
              <td>{{ service.cptCode }}</td>
              <td>{{ service.icdCode }}</td>
              <td>${{ service.estimatedCost }}</td>
              <td>{{ service.notes }}</td>
            </tr>

            <tr
              v-if="authorizationServices.length === 0"
            >
              <td
                colspan="4"
                class="empty-row"
              >
                No services found.
              </td>
            </tr>

          </tbody>

        </table>

        <div class="timeline-section">
        <button
        class="timeline-button"
        @click="viewTimeline"
        >
        View Timeline
        </button>
        </div>

      </div>

      <div
      v-if="showTimeline"
      class="details-card"
      >

      <h3>
      Timeline
      </h3>

      <div
      v-for="(
      item,
      index
      ) in authorizationTimeline"
      :key="index"
      class="timeline-item"
      >

      <div class="timeline-dot"></div>

      <div class="timeline-content">

      <h4>

      {{ item.action }}

      </h4>

      <p>

      {{ item.remarks }}

      </p>

      <span>

      {{ formatDate(item.createdAt) }}

      </span>

      </div>

      </div>

      </div>

    </div>

  </aside>

</div>
</template>

<style scoped>
.page {
  padding: 30px;
  background: #f8fafc;
  min-height: 100vh;
}

.page h2 {
  margin-bottom: 24px;
  font-size: 30px;
  font-weight: 700;
  color: #1e293b;
}

.table-card {
  background: white;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  overflow: hidden;
  box-shadow: 0 6px 20px rgba(15, 23, 42, 0.06);
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 24px;
  border-bottom: 1px solid #e5e7eb;
}

.table-header h3 {
  margin: 0;
  font-size: 24px;
  font-weight: 600;
}

.count {
  background: #eef4ff;
  color: #2563eb;
  padding: 8px 18px;
  border-radius: 999px;
  font-weight: 600;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th {
  background: #f8fafc;
  color: #64748b;
  font-weight: 600;
  padding: 16px;
  border-bottom: 1px solid #e5e7eb;
}

.table td {
  padding: 18px 16px;
  border-bottom: 1px solid #edf2f7;
}

.table tbody tr:hover {
  background: #f8fbff;
}

.drawer-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.35);
  display: flex;
  justify-content: flex-end;
  z-index: 1000;
}

.drawer {
  width: 700px;
  max-width: 100%;
  height: 100vh;
  background: white;
  overflow-y: auto;
  box-shadow: -10px 0 30px rgba(0, 0, 0, 0.15);
  animation: slideIn 0.25s ease;
}

.drawer-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 28px;
  border-bottom: 1px solid #e5e7eb;
}

.drawer-header h2 {
  margin: 0;
  font-size: 40px;
  font-weight: 700;
}

.drawer-body {
  padding: 28px;
}

.close-btn {
  border: none;
  background: transparent;
  font-size: 34px;
  cursor: pointer;
  color: #64748b;
}

.close-btn:hover {
  color: #0f172a;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
  }

  to {
    transform: translateX(0);
  }
}

.view-button {
  padding: 9px 18px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: white;
  font-weight: 600;
  cursor: pointer;
  transition: .2s;
}

.view-button:hover {
  background: #1d4ed8;
}

.close-button{
border:none;
background:none;

font-size:20px;

cursor:pointer;
}

.badge{
    display:inline-flex;
    align-items:center;
    justify-content:center;
    padding:7px 16px;
    border-radius:999px;
    font-size:13px;
    font-weight:600;
    min-width:95px;
}

/* Priority */

.priority-emergency{
    background:#fde2e2;
    color:#dc2626;
}

.priority-urgent{
    background:#fff3cd;
    color:#d97706;
}

.priority-routine{
    background:#dcfce7;
    color:#15803d;
}

/* Status */

.status-submitted{
    background:#dbeafe;
    color:#2563eb;
}

.status-re-submitted{
    background:#dbeafe;
    color:#2563eb;
}

.status-approved{
    background:#dcfce7;
    color:#15803d;
}

.status-denied{
    background:#fee2e2;
    color:#dc2626;
}

.status-pending{
    background:#fef3c7;
    color:#b45309;
}

.status-expired{
    background:#e5e7eb;
    color:#4b5563;
}

.view-button{
    display:inline-flex;
    align-items:center;
    gap:8px;
    padding:10px 18px;
    border:none;
    border-radius:8px;
    background:#2563eb;
    color:#fff;
    font-size:14px;
    font-weight:600;
    cursor:pointer;
    transition:.2s;
}

.view-button:hover{
    background:#1d4ed8;
}

.view-button i{
    font-size:14px;
}
.badge{
    display:inline-flex;
    align-items:center;
    justify-content:center;
    min-width:120px;
    padding:7px 16px;
    border-radius:999px;
    font-size:13px;
    font-weight:600;
}

/* Priority */

.priority-normal{
    background:#dcfce7;
    color:#15803d;
}

.priority-urgent{
    background:#fef3c7;
    color:#b45309;
}

.priority-emergency{
    background:#fee2e2;
    color:#dc2626;
}

/* Status */

.status-draft{
    background:#f3f4f6;
    color:#4b5563;
}

.status-verificationfailed{
    background:#fee2e2;
    color:#dc2626;
}

.status-readyforsubmission{
    background:#ede9fe;
    color:#6d28d9;
}

.status-submitted{
    background:#dbeafe;
    color:#2563eb;
}

.status-underreview{
    background:#fef3c7;
    color:#b45309;
}

.status-additionalinforequired{
    background:#ffedd5;
    color:#c2410c;
}

.status-resubmitted{
    background:#dbeafe;
    color:#2563eb;
}

.status-approved{
    background:#dcfce7;
    color:#15803d;
}

.status-denied{
    background:#fee2e2;
    color:#dc2626;
}

.status-expired{
    background:#e5e7eb;
    color:#4b5563;
}

.details-card{
  background:#fff;
  border:1px solid #e5e7eb;
  border-radius:12px;
  padding:20px;
  margin-bottom:24px;
}

.details-card h3{
  margin:0 0 18px;
  font-size:20px;
  font-weight:600;
  color:#1e293b;
}

.services-table{
  width:100%;
  border-collapse:collapse;
}

.services-table th{
  background:#f8fafc;
  color:#64748b;
  font-weight:600;
  padding:12px;
  text-align:left;
  border-bottom:1px solid #e5e7eb;
}

.services-table td{
  padding:14px 12px;
  border-bottom:1px solid #e5e7eb;
  color:#1f2937;
}

.empty-row{
  text-align:center;
  color:#94a3b8;
  padding:24px;
}

.timeline-section{
  display:flex;
  justify-content:flex-end;
  margin-top:24px;
}

.timeline-button{
  background:#2563eb;
  color:white;
  border:none;
  border-radius:8px;
  padding:10px 20px;
  font-weight:600;
  cursor:pointer;
  transition:.2s;
}

.timeline-button:hover{
  background:#1d4ed8;
}

.timeline-item{
display:flex;
gap:18px;
position:relative;
padding-bottom:24px;
}

.timeline-item:not(:last-child)::before{
content:"";
position:absolute;
left:8px;
top:18px;
width:2px;
height:calc(100% - 6px);
background:#dbeafe;
}

.timeline-dot{
width:18px;
height:18px;
border-radius:50%;
background:#2563eb;
margin-top:3px;
flex-shrink:0;
}

.timeline-content{
flex:1;
}

.timeline-content h4{
margin:0;
font-size:16px;
font-weight:600;
color:#1e293b;
}

.timeline-content p{
margin:6px 0;
color:#475569;
}

.timeline-content span{
font-size:13px;
color:#94a3b8;
}

.pagination{

display:flex;

justify-content: center;

align-items:center;

gap:16px;

padding:20px;

border-top:1px solid #e5e7eb;

}

.pagination button{

padding:8px 16px;

border:none;

border-radius:8px;

background:#2563eb;

color:white;

cursor:pointer;

font-weight:600;

}

.pagination button:disabled{

background:#cbd5e1;

cursor:not-allowed;

}

.pagination span{

font-weight:600;

color:#475569;

}

.table-header{

display:flex;

justify-content:space-between;

align-items:center;

margin-bottom:20px;

gap:20px;

}

.title{

margin:0;

font-size:22px;

font-weight:600;

color:#1e293b;

}

.toolbar{

display:flex;

align-items:center;

gap:16px;

margin-left:auto;

}

.count{

font-size:14px;

font-weight:600;

color:#64748b;

white-space:nowrap;

}

.search-box{

width:320px;

padding:10px 14px;

border:1px solid #d1d5db;

border-radius:8px;

font-size:14px;

background:#fff;

transition:.2s;

}

.search-box:focus{

outline:none;

border-color:#2563eb;

box-shadow:
0 0 0 3px
rgb(37 99 235 / 15%);

}
</style>
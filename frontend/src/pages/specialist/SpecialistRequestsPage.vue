<script setup lang="ts">
import { useRoute } from "vue-router";
import {
  ref,
  onMounted,
  computed,
  watch
} from "vue";

import {
  storeToRefs
} from "pinia";

//const selectedRequest = ref<AuthorizationRequest | null>(null);
import {
  useSpecialistStore
} from "../../stores/specialist.store";

const route = useRoute();

const specialistStore =
  useSpecialistStore();

const {
  authorizationRequests,
 
  authorizationTimeline,
  loading
} =
  storeToRefs(
    specialistStore
  );

const showHistoryModal =
  ref(false);

const selectedAuthId =
  ref<number | null>(null);

const showTimeline =
  ref(false);
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
const viewRequestHistory = async (authId: number) => {

  selectedAuthId.value = authId;

  showTimeline.value = false;

  authorizationTimeline.value = [];

  await specialistStore.loadAuthorizationDetails(authId);

  await specialistStore.loadAuthorizationServices(authId);

  await specialistStore.loadAuthorizationTimeline(authId); // <-- Missing

  showHistoryModal.value = true;

};
const closeHistoryModal = () => {

  showHistoryModal.value = false;
  console.log("Clicked:", authId);

  selectedAuthId.value = authId;

  showTimeline.value = false;

  selectedAuthId.value = null;

  authorizationTimeline.value = [];

  await specialistStore.loadAuthorizationServices(authId);

  console.log(
    "Services:",
    authorizationServices.value
  );

};


onMounted(async () => {

  await specialistStore
    .loadAuthorizationRequests();

  const authId =
    Number(route.query.authId);

  if (authId) {

    await viewRequestHistory(
      authId
    );

  }

});
</script>
<template>

<div class="table-card">

  <div class="table-header">

    <h3>
      Authorization Requests
    </h3>

    <span class="count">
      {{ authorizationRequests.length }} Requests
    </span>

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
        v-for="request in authorizationRequests"
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
            :class="
              'status-' +
              request.status.toLowerCase().replace(/\s+/g,'-')
            "
          >
            {{ request.status }}
          </span>

        </td>


        <td>

          <span
            class="badge"
            :class="
              'priority-' +
              request.priority.toLowerCase()
            "
          >
            {{ request.priority }}
          </span>

        </td>


        <td>
          ₹ {{ request.estimatedAmount }}
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

    </tbody>


  </table>


</div>





<!-- DRAWER -->
<!-- DRAWER -->

<div
  v-if="showHistoryModal"
  class="drawer-overlay"
  @click="closeHistoryModal"
>


<aside
  class="drawer"
  @click.stop
>


<!-- HEADER -->

<div class="drawer-header">

<h2>
Authorization Details
</h2>


<button
  class="close-btn"
  @click="closeHistoryModal"
>
✕
</button>


</div>





<!-- BODY -->

<div class="drawer-body">





<!-- AUTHORIZATION INFORMATION -->


<div
  v-if="specialistStore.authorizationDetails"
  class="details-card"
>


<h3>
Authorization Information
</h3>



<div class="details-grid">



<div class="detail-item">

<span class="label">
Auth ID
</span>

<span class="value">
{{ specialistStore.authorizationDetails.authId }}
</span>

</div>




<div class="detail-item">

<span class="label">
Patient
</span>

<span class="value">
{{ specialistStore.authorizationDetails.patientName }}
</span>

</div>




<div class="detail-item">

<span class="label">
Payer
</span>

<span class="value">
{{ specialistStore.authorizationDetails.payerName }}
</span>

</div>




<div class="detail-item">

<span class="label">
Status
</span>

<span class="status-badge">
{{ specialistStore.authorizationDetails.status }}
</span>

</div>




<div class="detail-item">

<span class="label">
Priority
</span>

<span class="value">
{{ specialistStore.authorizationDetails.priority }}
</span>

</div>




<div class="detail-item">

<span class="label">
Estimated Amount
</span>

<span class="value">
₹ {{ specialistStore.authorizationDetails.estimatedAmount }}
</span>

</div>




<div class="detail-item">

<span class="label">
Approved Amount
</span>

<span class="value">

{{
specialistStore.authorizationDetails.approvedAmount !== null
?
'₹ ' + specialistStore.authorizationDetails.approvedAmount
:
'--'
}}

</span>

</div>




<div class="detail-item">

<span class="label">
Submitted
</span>

<span class="value">

{{formatDate(
specialistStore.authorizationDetails.submittedAt ?? null
)}}

</span>

</div>



<div class="detail-item">

<span class="label">
Reviewed
</span>

<span class="value">

{{formatDate(
specialistStore.authorizationDetails.reviewedAt ?? null
)}}

</span>

</div>



<div class="detail-item">

<span class="label">
Expiration
</span>

<span class="value">

{{formatDate(
specialistStore.authorizationDetails.expirationDate ?? null
)}}

</span>

</div>



</div>


</div>









<!-- SERVICES -->


<div class="details-card">


<h3>
Services
</h3>



<div
v-if="specialistStore.authorizationServices?.length"
class="services-container"
>



<div
v-for="service in specialistStore.authorizationServices"
:key="service.serviceId"
class="service-card"
>



<div class="service-row">

<span class="service-label">
CPT Code
</span>


<span class="cpt-badge">
{{service.cptCode}}
</span>


</div>


<div class="service-row">

  <span class="service-label">
    ICD Code
  </span>


  <span class="icd-badge">
    {{service.icdCode}}
  </span>

</div>


<div class="service-row">

<span class="service-label">
Estimated Cost
</span>


<span>
₹ {{service.estimatedCost.toLocaleString()}}
</span>


</div>





<div
v-if="service.notes"
class="service-row"
>

<span class="service-label">
Notes
</span>


<span>
{{service.notes}}
</span>


</div>



</div>



</div>




<div
v-else
class="empty-row"
>

No services available.

</div>



</div>









<!-- TIMELINE BUTTON -->


<button
class="timeline-btn"
@click="showTimeline=!showTimeline"
>


<i
:class="
showTimeline
?
'pi pi-chevron-up'
:
'pi pi-chevron-down'
"
></i>


{{showTimeline ? 'Hide Timeline':'View Timeline'}}


</button>









<!-- TIMELINE -->


<div
v-if="showTimeline"
class="timeline-card"
>


<h3>
Authorization Timeline
</h3>



<div
v-if="specialistStore.authorizationTimeline?.length"
class="timeline-list"
>

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

<div
v-for="(event,index) in specialistStore.authorizationTimeline"
:key="index"
class="timeline-item"
>



<div class="timeline-marker">

<div class="timeline-dot"></div>


<div
v-if="
index !== specialistStore.authorizationTimeline.length-1
"
class="timeline-line"
>
</div>


</div>





<div class="timeline-content">


<h4>
{{event.action}}
</h4>



<p
v-if="event.remarks"
>
{{event.remarks}}
</p>


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

<span>
{{formatDate(event.createdAt)}}
</span>



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



</div>




</div>




<div
v-else
class="empty-row"
>

No timeline available.

</div>



</div>





</div>
<!-- END BODY -->



</aside>


</div>
<!-- END DRAWER -->
<!-- drawer-overlay -->



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
.timeline-btn {

margin-top:20px;
padding:10px 16px;
border:none;
border-radius:8px;
background:#2563eb;
color:white;
cursor:pointer;

display:flex;
align-items:center;
gap:8px;

}

.services-container {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.service-card {
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 16px;
  background: #fff;
}

.service-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.service-row:last-child {
  margin-bottom: 0;
}

.service-label {
  font-weight: 600;
  color: #374151;
}

.cpt-badge {
  background: #dbeafe;
  color: #1d4ed8;
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}
.timeline-btn {

width:100%;
margin-top:20px;

padding:12px 16px;

border:none;

border-radius:10px;

background:#2563eb;

color:white;

font-weight:600;

display:flex;

justify-content:center;

align-items:center;

gap:10px;

cursor:pointer;

}



.timeline-card{

margin-top:20px;

border:1px solid #e5e7eb;

border-radius:12px;

padding:20px;

background:white;

}
.icd-badge {

  background:#dcfce7;

  color:#166534;

  padding:4px 10px;

  border-radius:20px;

  font-size:12px;

  font-weight:600;

}


.timeline-list{

display:flex;

flex-direction:column;

}



.timeline-item{

display:flex;

gap:18px;

position:relative;

padding-bottom:24px;

}



.timeline-marker{

display:flex;

flex-direction:column;

align-items:center;

}



.timeline-dot{

width:12px;

height:12px;

background:#2563eb;

border-radius:50%;

}



.timeline-line{

width:2px;

flex:1;

background:#d1d5db;

margin-top:4px;

}



.timeline-content{

flex:1;

}



.timeline-content h4{

margin:0;

font-size:15px;

font-weight:600;

}



.timeline-content p{

margin:6px 0;

color:#6b7280;

white-space:normal;

word-break:break-word;

}



.timeline-content span{

font-size:12px;

color:#9ca3af;

}
.drawer-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 28px;
  border-bottom: 1px solid #e5e7eb;
}
.details-card {
  background: #ffffff;
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 18px;
  border: 1px solid #e5e7eb;
}


.details-card h3 {
  margin-bottom: 18px;
  font-size: 18px;
  font-weight: 600;
}


.details-grid {

  display: grid;

  grid-template-columns:
    repeat(2, minmax(200px, 1fr));

  gap: 18px;

}


.detail-item {

  display: flex;

  flex-direction: column;

  gap: 5px;

}


.label {

  font-size: 13px;

  color: #6b7280;

}


.value {

  font-size: 15px;

  font-weight: 500;

  color: #111827;

}


.status-badge {

  width: fit-content;

  padding: 5px 12px;

  border-radius: 20px;

  background: #dbeafe;

  color: #1d4ed8;

  font-size: 13px;

  font-weight: 600;

}
.drawer-header h2 {
  margin: 0;
  font-size: 40px;
  font-weight: 700;
}

.drawer-body{

overflow-y:auto;

padding:24px;

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
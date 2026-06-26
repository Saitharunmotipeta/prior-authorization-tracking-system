<script setup lang="ts">
import {
  onMounted
} from "vue";

import {
  storeToRefs
} from "pinia";

import {
  useSpecialistStore
} from "../../stores/specialist.store";

import {
  ref
} from "vue";

const specialistStore =
  useSpecialistStore();

const {
  authorizationRequests,
  loading
} =
  storeToRefs(
    specialistStore
  );

onMounted(() => {
  specialistStore.loadAuthorizationRequests();
});

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

const viewRequestHistory = (
  authId: number
) => {
  selectedAuthId.value =
    authId;

  showHistoryModal.value =
    true;
};

const closeHistoryModal =
  () => {
    showHistoryModal.value =
      false;

    selectedAuthId.value =
      null;
  };
</script>

<template>

<div class="page">

<h2>
Authorization Requests
</h2>

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

<th>Created</th>

<th>Submitted</th>

<th>Action</th>

</tr>

</thead>

<tbody>

<tr
v-for="request in authorizationRequests"
:key="request.authId"
>

<td>{{ request.authId }}</td>

<td>{{ request.patientName }}</td>

<td>{{ request.payerName }}</td>

<td>{{ request.status }}</td>

<td>{{ request.priority }}</td>

<td>{{ request.estimatedAmount }}</td>

<td>{{ formatDate(request.createdAt) }}</td>

<td>{{ formatDate(request.submittedAt) }}</td>

<td>
  <button
    class="view-button"
    @click="
      viewRequestHistory(
        request.authId
      )
    "
  >
    View
  </button>
</td>

</tr>

</tbody>

</table>
<div
v-if="showHistoryModal"
class="modal-overlay"
>

<div class="modal">

<div class="modal-header">

<h3>
Request History
</h3>

<button
class="close-button"
@click="closeHistoryModal"
>
✕
</button>

</div>

<div class="modal-body">

<p>

Authorization ID:

<strong>
{{ selectedAuthId }}
</strong>

</p>

<p>
History implementation coming soon...
</p>

</div>

</div>

</div>

</div>

</template>

<style scoped>
.page{
padding:24px;
}

.table{
width:100%;
border-collapse:collapse;
background:white;
}

.table th,
.table td{
padding:12px;
border:1px solid #e5e7eb;
text-align:left;
}

.table th{
background:#f8fafc;
font-weight:600;
}
.view-button {
  padding: 6px 14px;

  border: none;

  border-radius: 6px;

  background: #2563eb;

  color: white;

  cursor: pointer;

  font-size: 13px;

  font-weight: 500;

  transition: background 0.2s;
}

.view-button:hover {
  background: #1d4ed8;
}
.modal-overlay{
position:fixed;
inset:0;
background:rgba(0,0,0,.45);

display:flex;
justify-content:center;
align-items:center;

z-index:1000;
}

.modal{
width:700px;
max-width:90%;

background:white;

border-radius:10px;

overflow:hidden;

box-shadow:0 10px 30px rgba(0,0,0,.2);
}

.modal-header{
display:flex;
justify-content:space-between;
align-items:center;

padding:18px 24px;

border-bottom:1px solid #e5e7eb;
}

.modal-body{
padding:24px;
}

.close-button{
border:none;
background:none;

font-size:20px;

cursor:pointer;
}

</style>
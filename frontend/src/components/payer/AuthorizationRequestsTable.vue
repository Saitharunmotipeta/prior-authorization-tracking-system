<script setup lang="ts">
import {
  Eye
} from "lucide-vue-next";

import AppStatusBadge
from "../common/AppStatusBadge.vue";

import type {
  AuthorizationRequestSummary
} from "../../types/payer.interface";

defineProps<{
  requests:
    AuthorizationRequestSummary[];
}>();

const emit =
  defineEmits<{
    (
      e: "view",
      authId: number
    ): void;
  }>();
</script>

<template>

  <div class="card">

    <div class="card-header">

      <h2>
        Authorization Requests
      </h2>

      <span class="count">
        {{ requests.length }}
        Requests
      </span>

    </div>

    <div
      v-if="
        requests.length === 0
      "
      class="empty-state"
    >
      No authorization requests found.
    </div>
    <div
      v-else
      class="table-wrapper"
    >
    <table
      class="request-table"
    >

      <thead>

        <tr>

          <th>
            Auth Id
          </th>

          <th>
            Patient
          </th>

          <th>
            Priority
          </th>

          <th>
            Status
          </th>

          <th>
            Submitted
          </th>

          <th>
            Action
          </th>

        </tr>

      </thead>

      <tbody>

        <tr
          v-for="
            request
            in requests
          "
          :key="
            request.authId
          "
        >

          <td>
            #{{ request.authId }}
          </td>

          <td>
            {{ request.patientName }}
          </td>
          
          <td>
          <span
            class="priority"
            :class="{
              normal: Number(request.priority) === 1,
              urgent: Number(request.priority) === 2,
              emergency: Number(request.priority) === 3
            }"
          >
            {{
              Number(request.priority) === 1
                ? "Normal"
                : Number(request.priority) === 2
                ? "Urgent"
                : "Emergency"
            }}
          </span>
        </td>
          <td>

            <AppStatusBadge
              :status="
                Number(
                  request.status
                )
              "
            />

          </td>

          <td>

            {{
              request.submittedAt
                ? new Date(
                    request.submittedAt
                  ).toLocaleDateString()
                : "--"
            }}

          </td>

          <td>

            <button
              class="view-btn"
              @click="
                emit(
                  'view',
                  request.authId
                )
              "
            >

              <Eye
                :size="16"
              />

              View

            </button>

          </td>

        </tr>

      </tbody>

    </table>
    </div>

  </div>

</template>

<style scoped>

.card{
  background:white;

  border:1px solid #e5e7eb;

  border-radius:14px;

  padding:24px;

  box-shadow:
    0 2px 6px
    rgb(0 0 0 / 6%);
}

.card-header{

  display:flex;

  justify-content:
    space-between;

  align-items:center;

  margin-bottom:20px;
}

.card-header h2{

  margin:0;

  font-size:20px;

  color:#1e293b;
}

.count{

  background:#eff6ff;

  color:#2563eb;

  padding:6px 14px;

  border-radius:20px;

  font-weight:600;
}

.request-table {
  width: 100%;
  border-collapse: collapse;
  table-layout: auto;
}

.request-table th{

  text-align:left;

  padding:14px;

  background:#f8fafc;

  color:#64748b;

  font-size:14px;
}

.request-table td{

  padding:16px 14px;

  border-top:
    1px solid #e2e8f0;
}

.request-table tr:hover{

  background:#f8fafc;
}

.priority {
  display: inline-flex;
  align-items: center;
  justify-content: center;

  min-width: 90px;
  height: 32px;

  padding: 0 14px;

  border-radius: 999px;

  font-size: 13px;
  font-weight: 600;
  line-height: 1;
}

.priority.normal {
  background: #dbeafe;
  color: #1d4ed8;
}

.priority.urgent {
  background: #fef3c7;
  color: #b45309;
}

.priority.emergency {
  background: #fee2e2;
  color: #dc2626;
}

.normal {
  background: #dbeafe;
  color: #1d4ed8;
}

.urgent {
  background: #fef3c7;
  color: #b45309;
}

.emergency {
  background: #fee2e2;
  color: #dc2626;
}

.view-btn{

  display:flex;

  align-items:center;

  gap:6px;

  padding:8px 14px;

  border:none;

  border-radius:8px;

  background:#2563eb;

  color:white;

  cursor:pointer;

  transition:.2s;
}

.view-btn:hover{

  background:#1d4ed8;
}

.empty-state{

  text-align:center;

  padding:40px;

  color:#64748b;
}
.table-wrapper {
  width: 100%;
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
}

.request-table {
  width: 100%;
  min-width: 850px;
  border-collapse: collapse;
}

.request-table th {
  white-space: nowrap;
}

.request-table td {
  word-break: break-word;
}

.request-table td:nth-child(2) {
  white-space: normal;
}
</style>
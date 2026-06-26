<script setup lang="ts">
import {
  ClipboardList,
  BadgeDollarSign
} from "lucide-vue-next";

import type {
  AuthorizationService
} from "../../types/payer.interface";

defineProps<{
  services:
    AuthorizationService[];
}>();
</script>

<template>

<div class="card">

  <div class="card-header">

    <ClipboardList
      :size="20"
    />

    <h3>
      Authorization Services
    </h3>

  </div>

  <div
    v-if="
      services.length === 0
    "
    class="empty-state"
  >
    No services available.
  </div>

  <table
    v-else
    class="table"
  >

    <thead>

      <tr>

        <th>
          CPT Code
        </th>

        <th>
          ICD Code
        </th>

        <th>
          Estimated Cost
        </th>

        <th>
          Notes
        </th>

      </tr>

    </thead>

    <tbody>

      <tr
        v-for="
          service
          in services
        "
        :key="
          service.cptCode
        "
      >

        <td>
          {{ service.cptCode }}
        </td>

        <td>
          {{ service.icdCode }}
        </td>

        <td>

          <div class="amount">

            <BadgeDollarSign
              :size="16"
            />

            ₹
            {{
              service
                .estimatedCost
                .toLocaleString()
            }}

          </div>

        </td>

        <td>
          {{ service.notes }}
        </td>

      </tr>

    </tbody>

  </table>

</div>

</template>

<style scoped>

.card{

background:white;

border:1px solid #e5e7eb;

border-radius:12px;

padding:20px;

margin-top:20px;

}

.card-header{

display:flex;

align-items:center;

gap:10px;

margin-bottom:18px;

font-weight:600;

color:#1e293b;

}

.table{

width:100%;

border-collapse:collapse;

}

th{

background:#f8fafc;

padding:12px;

text-align:left;

font-weight:600;

color:#475569;

}

td{

padding:12px;

border-top:1px solid #e5e7eb;

}

.amount{

display:flex;

align-items:center;

gap:6px;

color:#16a34a;

font-weight:600;

}

.empty-state{

padding:40px;

text-align:center;

color:#64748b;

}

</style>
<script setup lang="ts">
import {
  User,
  Building2,
  ShieldCheck,
  BadgeDollarSign,
  ClipboardList,
  Phone,
  Calendar,
  HeartPulse
} from "lucide-vue-next";

import AppStatusBadge
from "../common/AppStatusBadge.vue";

import ServicesTable
from "./ServicesTable.vue";

import DecisionPanel
from "./DecisionPanel.vue";

import type {
  AuthorizationDetails
} from "../../types/payer.interface";

defineProps<{
  authorization:
    AuthorizationDetails | null;
}>();

const emit =
defineEmits<{
(
e:"review",
payload:any
):void;
}>();
</script>

<template>

<div
  v-if="authorization"
  class="details-container"
>

  <!-- Patient Information -->

  <div class="card">

    <div class="section-title">

      <User :size="20"/>

      <span>
        Patient Information
      </span>

    </div>

    <div class="grid">

      <div>

        <label>Name</label>

        <p>
          {{ authorization.patientName }}
        </p>

      </div>

      <div>

        <label>DOB</label>

        <p>
          {{ authorization.dob }}
        </p>

      </div>

      <div>

        <label>Gender</label>

        <p>
          {{ authorization.gender }}
        </p>

      </div>

      <div>

        <label>

          <Phone
            :size="15"
          />

          Phone

        </label>

        <p>
          {{ authorization.phoneNumber }}
        </p>

      </div>

    </div>

  </div>

  <!-- Authorization -->

  <div class="card">

    <div class="section-title">

      <Building2
        :size="20"
      />

      <span>
        Authorization Information
      </span>

    </div>

    <div class="grid">

      <div>

        <label>
          Auth Id
        </label>

        <p>
          {{ authorization.authId }}
        </p>

      </div>

      <div>

        <label>
          Encounter
        </label>

        <p>
          {{ authorization.encounterId }}
        </p>

      </div>

      <div>

        <label>
          Facility
        </label>

        <p>
          {{ authorization.facilityName }}
        </p>

      </div>

      <div>

        <label>
          Department
        </label>

        <p>
          {{ authorization.departmentName }}
        </p>

      </div>

      <div>

        <label>
          Priority
        </label>

        <p>

          {{
            authorization.priority==="0"
            ? "Normal"
            : authorization.priority==="1"
            ? "Urgent"
            : "Emergency"
          }}

        </p>

      </div>

      <div>

        <label>

          <HeartPulse
            :size="15"
          />

          Condition

        </label>

        <p>

          {{
            authorization.conditionType==="0"
            ? "Elective"
            : authorization.conditionType==="1"
            ? "Urgent"
            : "Emergency"
          }}

        </p>

      </div>

      <div>

        <label>
          Status
        </label>

        <AppStatusBadge
          :status="
            Number(
              authorization.status
            )
          "
        />

      </div>

      <div>

        <label>

          <BadgeDollarSign
            :size="15"
          />

          Estimated

        </label>

        <p>

          ₹
          {{
            authorization
              .estimatedAmount
              .toLocaleString()
          }}

        </p>

      </div>

      <div>

        <label>
          Approved
        </label>

        <p>

          {{
            authorization.approvedAmount
            ?? "--"
          }}

        </p>

      </div>

      <div>

        <label>

          <Calendar
            :size="15"
          />

          Submitted

        </label>

        <p>

          {{
            authorization.submittedAt
            ?? "--"
          }}

        </p>

      </div>

    </div>

  </div>

  <!-- Documents -->

  <div class="card">

    <div class="section-title">

      <ShieldCheck
        :size="20"
      />

      <span>
        Document Verification
      </span>

    </div>

    <div class="documents">

      <div>
        {{
          authorization.documents.identificationVerified
          ? "✅"
          : "❌"
        }}
        Identification
      </div>

      <div>
        {{
          authorization.documents.prescriptionVerified
          ? "✅"
          : "❌"
        }}
        Prescription
      </div>

      <div>
        {{
          authorization.documents.scanVerified
          ? "✅"
          : "❌"
        }}
        Scan Report
      </div>

      <div>
        {{
          authorization.documents.doctorNotesVerified
          ? "✅"
          : "❌"
        }}
        Doctor Notes
      </div>

      <div>
        {{
          authorization.documents.insuranceCardVerified
          ? "✅"
          : "❌"
        }}
        Insurance Card
      </div>

    </div>

  </div>

  <!-- Services -->

  <div class="card">

    <div class="section-title">

      <ClipboardList
        :size="20"
      />

      <span>
        Authorization Services
      </span>

    </div>

    <ServicesTable
      :services="
        authorization.services
      "
    />

  </div>

  <!-- Decision Panel -->

  <DecisionPanel
  :authorization="authorization"
  @review="emit('review',$event)"
/>

</div>

<div
  v-else
  class="empty-card"
>

  <ClipboardList
    :size="40"
  />

  <h2>
    Select an Authorization Request
  </h2>

  <p>

    Choose a request
    from the table
    to review.

  </p>

</div>

</template>

<style scoped>

.details-container{

display:flex;

flex-direction:column;

gap:20px;

}

.card{

background:white;

border:1px solid #e5e7eb;

border-radius:14px;

padding:24px;

box-shadow:0 2px 6px rgb(0 0 0 / 6%);

}

.section-title{

display:flex;

align-items:center;

gap:10px;

margin-bottom:18px;

font-weight:600;

font-size:18px;

color:#1e293b;

}

.grid{

display:grid;

grid-template-columns:repeat(2,1fr);

gap:18px;

}

.grid label{

display:block;

font-size:13px;

color:#64748b;

margin-bottom:4px;

font-weight:600;

}

.grid p{

margin:0;

font-weight:500;

color:#1e293b;

}

.documents{

display:grid;

grid-template-columns:repeat(2,1fr);

gap:16px;

font-weight:500;

}

.empty-card{

display:flex;

flex-direction:column;

justify-content:center;

align-items:center;

height:100%;

padding:80px;

border:2px dashed #cbd5e1;

border-radius:14px;

color:#64748b;

background:white;

}

</style>
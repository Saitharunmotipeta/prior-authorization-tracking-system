<script setup lang="ts">
import { ref } from "vue";

import {
  CheckCircle2,
  XCircle,
  CircleAlert,
  Send
} from "lucide-vue-next";

import type {
  AuthorizationDetails
} from "../../types/payer.interface";

const props =
  defineProps<{
    authorization:
      AuthorizationDetails;
  }>();

const emit =
    defineEmits<{
    (
    e:"review",
    payload:{
    action:number;
    approvedAmount?:number;
    remarks?:string;
    }
    ):void;
    }>();

const selectedAction =
  ref<
    "approve"
    | "deny"
    | "moreInfo"
    | null
  >(null);

const approvedAmount =
  ref(
    props.authorization
      .estimatedAmount
  );

const remarks =
  ref("");

const submit = () => {
console.log("sunmitted the value");

  if (
    selectedAction.value ===
    "approve"
  ) {

    emit(
      "review",
      {
        action: 1,

        approvedAmount:
          approvedAmount.value,

        remarks:
          remarks.value
      }
    );
    console.log("review submitted approved");

    return;
  }

  if (
    selectedAction.value ===
    "deny"
  ) {

    emit(
      "review",
      {
        action: 2,

        remarks:
          remarks.value
      }
    );

    return;
  }

  emit(
    "review",
    {
      action: 3,

      remarks:
        remarks.value
    }
  );
};
</script>

<template>

<div class="card">

<div class="title">

Decision

</div>

<div class="actions">

<button
class="approve"
@click="
selectedAction =
'approve'
"
>

<CheckCircle2
:size="18"
/>

Approve

</button>

<button
class="deny"
@click="
selectedAction =
'deny'
"
>

<XCircle
:size="18"
/>

Deny

</button>

<button
class="info"
@click="
selectedAction =
'moreInfo'
"
>

<CircleAlert
:size="18"
/>

Request Info

</button>

</div>

<div
v-if="
selectedAction ===
'approve'
"
class="panel"
>

<label>

Approved Amount

</label>

<input
type="number"
v-model.number="
approvedAmount
"
/>

<button
class="submit approve"
@click="submit"
>

<Send
:size="16"
/>

Approve Request

</button>

</div>

<div
v-if="
selectedAction ===
'deny'
"
class="panel"
>

<label>

Remarks

</label>

<textarea
v-model="remarks"
rows="4"
/>

<button
class="submit deny"
@click="submit"
>

<Send
:size="16"
/>

Deny Request

</button>

</div>

<div
v-if="
selectedAction ===
'moreInfo'
"
class="panel"
>

<label>

Remarks

</label>

<textarea
v-model="remarks"
rows="4"
/>

<button
class="submit info"
@click="submit"
>

<Send
:size="16"
/>

Send Request

</button>

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

.title{

font-size:20px;

font-weight:600;

margin-bottom:20px;

color:#1e293b;

}

.actions{

display:flex;

gap:16px;

margin-bottom:24px;

}

.actions button{

display:flex;

align-items:center;

gap:8px;

padding:12px 20px;

border:none;

border-radius:10px;

cursor:pointer;

font-weight:600;

transition:.2s;

}

.approve{

background:#dcfce7;

color:#15803d;

}

.approve:hover{

background:#bbf7d0;

}

.deny{

background:#fee2e2;

color:#b91c1c;

}

.deny:hover{

background:#fecaca;

}

.info{

background:#dbeafe;

color:#2563eb;

}

.info:hover{

background:#bfdbfe;

}

.panel{

display:flex;

flex-direction:column;

gap:14px;

margin-top:10px;

}

label{

font-weight:600;

color:#475569;

}

input,
textarea{

padding:12px;

border:1px solid #cbd5e1;

border-radius:10px;

font-size:14px;

}

textarea{

resize:vertical;

}

.submit{

width:fit-content;

display:flex;

align-items:center;

gap:8px;

padding:12px 22px;

color:white;

border:none;

border-radius:10px;

cursor:pointer;

margin-top:6px;

}

.submit.approve{

background:#16a34a;

}

.submit.deny{

background:#dc2626;

}

.submit.info{

background:#2563eb;

}

</style>
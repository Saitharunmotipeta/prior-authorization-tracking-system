<script setup lang="ts">
import {
  Building2,
  ChevronDown
} from "lucide-vue-next";

import type {
  PayerFacility
} from "../../types/payer.interface";

defineProps<{
  facilities: PayerFacility[];

  selectedFacilityId:
    number | null;
}>();

const emit =
  defineEmits<{
    (
      e: "select",
      facilityId: number
    ): void;
  }>();

  const onFacilityChange = (
  event: Event
) => {
  const target =
    event.target as HTMLSelectElement;

  emit(
    "select",
    Number(target.value)
  );
};
</script>

<template>

<div class="facility-section">

  <label>
    Select Facility
  </label>

  <div class="select-wrapper">

    <Building2
      class="left-icon"
      :size="18"
    />

    <select
      :value="
        selectedFacilityId ?? ''
      "
      @change="onFacilityChange"
    >

      <option
        disabled
        value=""
      >
        Select Facility
      </option>

      <option
        v-for="
          facility
          in facilities
        "
        :key="
          facility.facilityId
        "
        :value="
          facility.facilityId
        "
      >
        {{ facility.facilityName }}
        ({{ facility.pendingCount }})
      </option>

    </select>

    <ChevronDown
      class="right-icon"
      :size="18"
    />

  </div>

</div>

</template>

<style scoped>

.facility-section{

display:flex;

flex-direction:column;

gap:8px;

max-width:420px;

}

label{

font-size:14px;

font-weight:600;

color:#334155;

}

.select-wrapper{

position:relative;

display:flex;

align-items:center;

}

select{

width:100%;

padding:12px 42px;

border:1px solid #cbd5e1;

border-radius:10px;

background:white;

font-size:15px;

font-weight:500;

appearance:none;

outline:none;

transition:.2s;

cursor:pointer;

}

select:hover{

border-color:#2563eb;

}

select:focus{

border-color:#2563eb;

box-shadow:
0 0 0 3px
rgb(37 99 235 / 15%);

}

.left-icon{

position:absolute;

left:14px;

color:#64748b;

pointer-events:none;

}

.right-icon{

position:absolute;

right:14px;

color:#64748b;

pointer-events:none;

}

</style>
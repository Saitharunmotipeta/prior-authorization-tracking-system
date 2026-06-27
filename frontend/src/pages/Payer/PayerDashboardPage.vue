<script setup lang="ts">
import {
  ref,
  onMounted
} from "vue";
import { useRoute } from "vue-router";

import {
  storeToRefs
} from "pinia";

import RequestDrawer
from "../../components/payer/RequestDrawer.vue";

import FacilitySelector
from "../../components/payer/FacilitySelector.vue";

import AuthorizationRequestsTable
from "../../components/payer/AuthorizationRequestsTable.vue";

import AppError
from "../../components/common/AppError.vue";

import {
  usePayerStore
} from "../../stores/payer.store";

const route = useRoute();
const payerStore =
  usePayerStore();

  const submitReview =
async (
payload:any
)=>{

if(
!authorizationDetails.value
)
return;

await payerStore
.reviewAuthorization(
authorizationDetails.value.authId,
payload
);

drawerOpen.value =
false;

};

const {
  facilities,
  selectedFacilityId,
  authorizationRequests,
  authorizationDetails,
  error
} =
  storeToRefs(
    payerStore
  );

const drawerOpen =
  ref(false);

onMounted(async () => {

  await payerStore.loadFacilities();

  const authId = Number(route.query.authId);

  if (authId) {
    await openDrawer(authId);
  }

});

const selectFacility =
  async (
    facilityId: number
  ) => {

    await payerStore
      .selectFacility(
        facilityId
      );

  };

const openDrawer =
  async (
    authId: number
  ) => {

    await payerStore
      .loadAuthorizationDetails(
        authId
      );

    drawerOpen.value =
      true;

  };

const approve =
(
  payload: any
) => {

  console.log(
    "Approve",
    payload
  );

};

const deny =
(
  payload: any
) => {

  console.log(
    "Deny",
    payload
  );

};

const requestMoreInfo =
(
  payload: any
) => {

  console.log(
    "Request More Info",
    payload
  );

};
</script>

<template>

<div class="page">

  <AppError
    :message="error"
  />

  <!-- Facility Selection -->

  <div class="section">

    <h2>
      Facilities
    </h2>

    <FacilitySelector

      :facilities="
        facilities
      "

      :selectedFacilityId="
        selectedFacilityId
      "

      @select="
        selectFacility
      "

    />

  </div>

  <!-- Workspace -->

  <div class="workspace">

    <!-- Left -->

    <div class="left-panel">

      <AuthorizationRequestsTable

        :requests="
          authorizationRequests
        "

        @view="
        openDrawer
        "

      />

    </div>

    <!-- Right -->

    <RequestDrawer
  :open="drawerOpen"
  :authorization="authorizationDetails"
  @close="
    drawerOpen=false
  "
  @review="
    submitReview
  "
/>

  </div>

</div>

</template>

<style scoped>

.page{

padding:24px;

background:#f8fafc;

min-height:100%;

display:flex;

flex-direction:column;

gap:24px;

}

.section{

display:flex;

flex-direction:column;

gap:18px;

}

.section h2{

margin:0;

font-size:22px;

font-weight:600;

color:#1e293b;

}

.workspace{

display:grid;

grid-template-columns:

480px
1fr;

gap:24px;

align-items:start;

}

.left-panel{

display:flex;

flex-direction:column;

}

.right-panel{

display:flex;

flex-direction:column;

}

@media
(max-width:1400px){

.workspace{

grid-template-columns:
1fr;

}

}

</style>
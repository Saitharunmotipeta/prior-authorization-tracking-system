<script setup lang="ts">
import {
  ref,
  onMounted
} from "vue";

import {
  storeToRefs
} from "pinia";

import {
  useRouter
} from "vue-router";

import {
  Plus,
  FileText,
  ClipboardList
} from "lucide-vue-next";

import AppError
from "../../components/common/AppError.vue";

import AppStatusBadge
from "../../components/common/AppStatusBadge.vue";

import {
  useAuthorizationStore
} from "../../stores/authorization.store";
import {
  getCptCodes,
  getIcdCodes
} from "../../api/specialist.api.ts";

const router = useRouter();

const authorizationStore =
  useAuthorizationStore();

const {
  authorizationRequestId,
  services,
  requestStatus,
  error
} =
  storeToRefs(
    authorizationStore
  );

const cptSearchText =
  ref("");

const icdSearchText =
  ref("");

const selectedCptCode =
  ref("");

const selectedIcdCode =
  ref("");

const notes =
  ref("");

const cptResults =
  ref<any[]>([]);

const icdResults =
  ref<any[]>([]);

const allCptCodes =
  ref<any[]>([]);

const allIcdCodes =
  ref<any[]>([]);

const searchCpt = () => {

  cptResults.value =
    allCptCodes.value.filter(x =>
      x.cptDescription
        .toLowerCase()
        .includes(cptSearchText.value.toLowerCase()) ||

      x.cptCode
        .toLowerCase()
        .includes(cptSearchText.value.toLowerCase())
    );

};
 
onMounted(
  async () => {

    const cptResponse =
      await getCptCodes();

    allCptCodes.value =
      cptResponse.data;

    const icdResponse =
      await getIcdCodes();

    allIcdCodes.value =
      icdResponse.data;
  }
);
const searchIcd = () => {

  icdResults.value =
    allIcdCodes.value.filter(x =>
      x.icdDescription
        .toLowerCase()
        .includes(icdSearchText.value.toLowerCase()) ||

      x.icdCode
        .toLowerCase()
        .includes(icdSearchText.value.toLowerCase())
    );

};
const selectCpt =
  (item: any) => {

    selectedCptCode.value =
      item.cptCode;

    cptSearchText.value =
      item.cptDescription;

    cptResults.value = [];
  };

const selectIcd =
  (item: any) => {

    selectedIcdCode.value =
      item.icdCode;

    icdSearchText.value =
      item.icdDescription;

    icdResults.value = [];
  };

const addService =
  async () => {

    await authorizationStore
      .addService({
        cptCode:
          selectedCptCode.value,

        icdCode:
          selectedIcdCode.value,

        notes:
          notes.value
      });

    cptSearchText.value = "";
    icdSearchText.value = "";

    selectedCptCode.value = "";
    selectedIcdCode.value = "";

    notes.value = "";

    cptResults.value = [];
    icdResults.value = [];
  };

const goToSummary =
async () => {

    try {

        await authorizationStore
            .uploadServices();

        router.push(
            "/specialist/authorization-summary"
        );

    }
    catch (error) {

        console.error(error);

    }

};
</script>
<template>

<div class="page">

<div class="page-header">

<div>

<h1>
Add Authorization Services
</h1>

<p class="subtitle">
Add CPT and ICD services
to the authorization request.
</p>

</div>

<AppStatusBadge
:status="requestStatus"
/>

</div>

<AppError
:message="error"
/>

<div class="card">

<div class="card-title">

<FileText :size="20"/>

<span>
Service Details
</span>

</div>

<p class="auth-id">

Authorization Id :

<strong>
{{ authorizationRequestId }}
</strong>

</p>

<!-- CPT -->

<div class="search-container">

<input
v-model="cptSearchText"
placeholder="Search or Select CPT Service"
@focus="cptResults = allCptCodes"
@input="searchCpt"
/>

<div
v-if="cptResults.length"
class="dropdown"
>

<div
v-for="item in cptResults"
:key="item.cptCode"
class="dropdown-item"
@click="selectCpt(item)"
>

<div class="code">

{{ item.cptCode }}

</div>

<div class="description">

{{ item.cptDescription }}

</div>

</div>

</div>

</div>

<!-- ICD -->

<div class="search-container">

<input
v-model="icdSearchText"
placeholder="Search or Select Diagnosis"
@focus="icdResults = allIcdCodes"
@input="searchIcd"
/>

<div
v-if="icdResults.length"
class="dropdown"
>

<div
v-for="item in icdResults"
:key="item.icdCode"
class="dropdown-item"
@click="selectIcd(item)"
>

<div class="code">

{{ item.icdCode }}

</div>

<div class="description">

{{ item.icdDescription }}

</div>

</div>

</div>

</div>

<textarea
v-model="notes"
placeholder="Clinical Notes"
/>

<button
class="primary-btn"
@click="addService()"
>

<Plus :size="16"/>

Add Service

</button>

</div>

<div class="card">

<div class="card-title">

<ClipboardList :size="20"/>

<span>
Added Services
</span>

</div>

<div
v-if="services.length===0"
class="empty-state"
>

No services added yet.

</div>

<div
v-for="(service,index) in services"
:key="index"
class="service-item"
>

<p>

<strong>CPT:</strong>

{{ service.cptCode }}

</p>

<p>

<strong>ICD:</strong>

{{ service.icdCode }}

</p>

<p>

<strong>Notes:</strong>

{{ service.notes }}

</p>

</div>

<button
class="success-btn"
@click="goToSummary()"
>

Review Authorization

</button>

</div>

</div>

</template>

<style scoped>

.page {
  padding: 24px;
  background: #f8fafc;
  min-height: 100vh;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.subtitle {
  color: #64748b;
  margin-top: 6px;
}

.card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
  box-shadow: 0 1px 3px rgb(0 0 0 / 8%);
}

.card-title {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
  font-weight: 600;
  font-size: 16px;
}

.auth-id {
  margin-bottom: 20px;
}

input,
textarea {
  width: 100%;
  padding: 12px;
  margin-bottom: 14px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  transition: .2s;
}

input:focus,
textarea:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgb(37 99 235 / 15%);
}

textarea {
  min-height: 120px;
  resize: vertical;
}

.primary-btn,
.success-btn {
  border: none;
  border-radius: 8px;
  padding: 12px 18px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  transition: .2s;
}

.primary-btn {
  background: #2563eb;
  color: white;
}

.primary-btn:hover {
  background: #1d4ed8;
}

.success-btn {
  background: #16a34a;
  color: white;
  margin-top: 16px;
}

.success-btn:hover {
  background: #15803d;
}

.service-item {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 12px;
  background: #fafafa;
}

.empty-state {
  color: #64748b;
  padding: 20px;
  text-align: center;
}

/* ================= Search ================= */

.search-container {
  position: relative;
  margin-bottom: 18px;
}

.dropdown {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  width: 100%;
  background: white;
  border: 1px solid #dbe3ef;
  border-radius: 10px;
  max-height: 260px;
  overflow-y: auto;
  z-index: 999;
  box-shadow: 0 8px 24px rgb(0 0 0 / 12%);
}

/* Scrollbar */

.dropdown::-webkit-scrollbar {
  width: 6px;
}

.dropdown::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 10px;
}

.dropdown-item {
  padding: 12px 16px;
  cursor: pointer;
  transition: .15s;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.dropdown-item:hover {
  background: #eff6ff;
}

.dropdown-item+.dropdown-item {
  border-top: 1px solid #f1f5f9;
}

.code {
  font-size: 13px;
  font-weight: 700;
  color: #2563eb;
}

.description {
  font-size: 14px;
  color: #475569;
}

</style>
<script setup lang="ts">
import {
  storeToRefs
} from "pinia";

import {
  useManagerDashboardStore
} from "../../stores/managerDashboard.store";

const managerDashboardStore =
  useManagerDashboardStore();

const {
  executiveReport,
  loading,
  error
} =
  storeToRefs(
    managerDashboardStore
  );

const generateReport =
  async () => {

    await managerDashboardStore
      .loadExecutiveReport();

  };
</script>

<template>

<div class="page">

  <div class="page-header">

    <div>

      <h1>
        AI Executive Report
      </h1>

      <p class="subtitle">
        Generate an AI-powered executive summary of organizational performance.
      </p>

    </div>

    <button
      class="generate-btn"
      @click="generateReport"
      :disabled="loading"
    >

      {{
        loading
          ? "Generating..."
          : "Generate Executive Report"
      }}

    </button>

  </div>

  <div
    v-if="executiveReport"
    class="report-card"
  >

    <div class="report-header">

      <h2>
        Executive Report
      </h2>

      <span class="generated-at">

        Generated

        {{
          new Date(
            executiveReport.generatedAt
          ).toLocaleString()
        }}

      </span>

    </div>

    <div
      v-for="(
        value,
        key
      ) in executiveReport.report"
      :key="key"
      class="report-section"
    >

      <h3>

        {{ key }}

      </h3>

      <p>

        {{ value }}

      </p>

    </div>

  </div>

  <div
    v-else
    class="empty-state"
  >

    Click

    <strong>

      Generate Executive Report

    </strong>

    to generate an AI-powered business report.

  </div>

</div>

</template>

<style scoped>

.page{

  padding:30px;

  background:#f8fafc;

  min-height:100vh;

}

.page-header{

  display:flex;

  justify-content:space-between;

  align-items:center;

  margin-bottom:28px;

}

.page-header h1{

  margin:0;

  color:#1e293b;

  font-size:32px;

}

.subtitle{

  margin-top:8px;

  color:#64748b;

}

.generate-btn{

  padding:14px 24px;

  border:none;

  border-radius:10px;

  background:#2563eb;

  color:white;

  font-weight:600;

  cursor:pointer;

  transition:.2s;

}

.generate-btn:hover{

  background:#1d4ed8;

}

.generate-btn:disabled{

  background:#94a3b8;

  cursor:not-allowed;

}

.error-card{

  padding:18px;

  margin-bottom:24px;

  border-radius:10px;

  background:#fee2e2;

  color:#b91c1c;

}

.report-card{

  background:white;

  border-radius:16px;

  padding:32px;

  border:1px solid #e5e7eb;

  box-shadow:
    0 4px 12px
    rgb(0 0 0 / 6%);

}

.report-header{

  display:flex;

  justify-content:space-between;

  align-items:center;

  margin-bottom:32px;

  padding-bottom:20px;

  border-bottom:1px solid #e5e7eb;

}

.generated-at{

  color:#64748b;

  font-size:14px;

}

.report-section{

  margin-bottom:30px;

  padding-left:18px;

  border-left:4px solid #2563eb;

}

.report-section h3{

  margin-bottom:12px;

  color:#1e293b;

  font-size:20px;

}

.report-section p{

  white-space:pre-line;

  line-height:1.8;

  color:#475569;

}

.empty-state{

  margin-top:80px;

  padding:60px;

  text-align:center;

  background:white;

  border-radius:16px;

  border:1px dashed #cbd5e1;

  color:#64748b;

}

</style>
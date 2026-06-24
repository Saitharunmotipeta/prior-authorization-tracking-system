<script setup lang="ts">
import { useRouter } from "vue-router";

import {
  ArrowRight,
  FilePlus2,
  ClipboardCheck,
  ShieldCheck,
  FileSearch
} from "lucide-vue-next";

const router = useRouter();

const startWorkflow =
  () => {
    router.push(
      "/specialist/eligibility"
    );
  };
</script>

<template>
  <div class="page">

    <div class="hero-card">

      <div class="hero-content">

        <div class="hero-icon">
          <FilePlus2 :size="40" />
        </div>

        <div>

          <h1>
            Create Prior Authorization
          </h1>

          <p>
            Start a new authorization request
            workflow for a patient encounter.
          </p>

        </div>

      </div>

      <button
        class="start-btn"
        @click="startWorkflow"
      >

        Start Workflow

        <ArrowRight
          :size="18"
        />

      </button>

    </div>

    <div class="workflow-section">

      <h2>
        Workflow Overview
      </h2>

      <div class="workflow-grid">

        <div class="workflow-card">

          <ShieldCheck
            :size="28"
          />

          <h3>
            Eligibility Verification
          </h3>

          <p>
            Verify patient eligibility
            and active insurance policy.
          </p>

        </div>

        <div class="workflow-card">

          <ClipboardCheck
            :size="28"
          />

          <h3>
            Encounter Creation
          </h3>

          <p>
            Create encounter and
            capture patient details.
          </p>

        </div>

        <div class="workflow-card">

          <FileSearch
            :size="28"
          />

          <h3>
            Document Verification
          </h3>

          <p>
            Validate required
            clinical documentation.
          </p>

        </div>

        <div class="workflow-card">

          <FilePlus2
            :size="28"
          />

          <h3>
            Authorization Request
          </h3>

          <p>
            Create and submit
            authorization request.
          </p>

        </div>

      </div>

    </div>

  </div>
</template>

<style scoped>
.page {
  padding: 24px;

  background: #f8fafc;

  min-height: 100vh;
}

.hero-card {
  background: white;

  border-radius: 16px;

  padding: 32px;

  display: flex;

  justify-content:
    space-between;

  align-items: center;

  border: 1px solid #e5e7eb;

  box-shadow:
    0 1px 3px
    rgb(0 0 0 / 8%);

  margin-bottom: 32px;
}

.hero-content {
  display: flex;

  align-items: center;

  gap: 20px;
}

.hero-icon {
  display: flex;

  align-items: center;

  justify-content: center;

  width: 72px;

  height: 72px;

  border-radius: 12px;

  background: #eff6ff;

  color: #2563eb;
}

.hero-content h1 {
  margin: 0;

  font-size: 28px;
}

.hero-content p {
  margin-top: 8px;

  color: #64748b;
}

.start-btn {
  display: flex;

  align-items: center;

  gap: 10px;

  border: none;

  border-radius: 10px;

  padding: 14px 22px;

  background: #2563eb;

  color: white;

  font-weight: 600;

  cursor: pointer;
}

.workflow-section h2 {
  margin-bottom: 20px;
}

.workflow-grid {
  display: grid;

  grid-template-columns:
    repeat(4, 1fr);

  gap: 20px;
}

.workflow-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  box-shadow:
    0 1px 3px
    rgb(0 0 0 / 8%);
}

.workflow-card h3 {
  margin-top: 16px;

  margin-bottom: 10px;

  font-size: 16px;
}

.workflow-card p {
  color: #64748b;

  font-size: 14px;

  line-height: 1.5;
}
</style>
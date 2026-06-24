<script setup lang="ts">
import { RouterLink } from "vue-router";

import {
  FilePlus2,
  ClipboardList,
  Bell,
  ArrowRight
} from "lucide-vue-next";
</script>

<template>
  <div class="dashboard-page">

    <div class="hero-card">

      <div>

        <h1>
          Specialist Dashboard
        </h1>

        <p>
          Manage prior authorization
          requests, document verification,
          and patient workflows.
        </p>

      </div>

    </div>

    <div class="stats-grid">

      <div class="stat-card">

        <h3>
          Active Requests
        </h3>

        <span>
          0
        </span>

      </div>

      <div class="stat-card">

        <h3>
          Pending Reviews
        </h3>

        <span>
          0
        </span>

      </div>

      <div class="stat-card">

        <h3>
          Approved Today
        </h3>

        <span>
          0
        </span>

      </div>

      <div class="stat-card">

        <h3>
          Reminders
        </h3>

        <span>
          0
        </span>

      </div>

    </div>

    <div class="actions-grid">

      <div class="action-card">

        <FilePlus2
          :size="32"
        />

        <h3>
          Create Entry
        </h3>

        <p>
          Start a new authorization
          workflow from eligibility
          verification.
        </p>

        <RouterLink
          to="/specialist/eligibility"
          class="action-button"
        >

          Open

          <ArrowRight
            :size="16"
          />

        </RouterLink>

      </div>

      <div class="action-card">

        <ClipboardList
          :size="32"
        />

        <h3>
          Requests
        </h3>

        <p>
          View authorization requests,
          encounter information and
          request status.
        </p>

        <RouterLink
          to="/specialist/requests"
          class="action-button"
        >

          Open

          <ArrowRight
            :size="16"
          />

        </RouterLink>

      </div>

      <div class="action-card">

        <Bell
          :size="32"
        />

        <h3>
          Reminders
        </h3>

        <p>
          View generated reminders
          and follow-up activities.
        </p>

        <RouterLink
          to="/specialist/reminders"
          class="action-button"
        >

          Open

          <ArrowRight
            :size="16"
          />

        </RouterLink>

      </div>

    </div>

  </div>
</template>

<style scoped>
.dashboard-page {
  padding: 24px;

  background: #f8fafc;

  min-height: 100vh;
}

.hero-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 16px;

  padding: 32px;

  margin-bottom: 24px;

  box-shadow:
    0 1px 3px
    rgb(0 0 0 / 8%);
}

.hero-card h1 {
  margin-bottom: 8px;
}

.hero-card p {
  color: #64748b;
}

.stats-grid {
  display: grid;

  grid-template-columns:
    repeat(4, 1fr);

  gap: 20px;

  margin-bottom: 24px;
}

.stat-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 20px;

  text-align: center;
}

.stat-card h3 {
  margin-bottom: 12px;

  font-size: 14px;

  color: #64748b;
}

.stat-card span {
  font-size: 28px;

  font-weight: 700;
}

.actions-grid {
  display: grid;

  grid-template-columns:
    repeat(3, 1fr);

  gap: 20px;
}

.action-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 24px;

  display: flex;

  flex-direction: column;

  gap: 14px;

  box-shadow:
    0 1px 3px
    rgb(0 0 0 / 8%);
}

.action-card h3 {
  margin: 0;
}

.action-card p {
  color: #64748b;

  flex: 1;
}

.action-button {
  display: flex;

  align-items: center;

  gap: 8px;

  width: fit-content;

  text-decoration: none;

  background: #2563eb;

  color: white;

  padding: 10px 16px;

  border-radius: 8px;

  font-weight: 600;
}
</style>
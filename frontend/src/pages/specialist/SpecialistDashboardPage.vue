<script setup lang="ts">
import { RouterLink } from "vue-router";
</script>

<template>
  <div class="dashboard-page">

    <h1 class="page-title">
      Specialist Dashboard
    </h1>

    <div class="cards-grid">

      <div class="dashboard-card">
        <h3>Eligibility Verification</h3>

        <p>
          Verify insurance eligibility using
          MRN number.
        </p>

        <RouterLink
          to="/specialist/eligibility"
          class="action-button"
        >
          Open
        </RouterLink>
      </div>

      <div class="dashboard-card">
        <h3>Create Encounter</h3>

        <p>
          Create encounter after eligibility
          verification.
        </p>

        <button disabled>
          Coming Soon
        </button>
      </div>

      <div class="dashboard-card">
        <h3>Authorization Requests</h3>

        <p>
          Manage authorization requests.
        </p>

        <button disabled>
          Coming Soon
        </button>
      </div>

    </div>

  </div>
</template>

<style scoped>
.dashboard-page {
  padding: 24px;
}

.page-title {
  margin-bottom: 24px;
}

.cards-grid {
  display: grid;

  grid-template-columns:
    repeat(auto-fit, minmax(300px, 1fr));

  gap: 20px;
}

.dashboard-card {
  background: white;

  border: 1px solid #e5e7eb;

  border-radius: 12px;

  padding: 20px;
}

.action-button,
button {
  margin-top: 16px;

  padding: 10px 16px;

  border: none;

  border-radius: 8px;

  cursor: pointer;

  background: #2563eb;

  color: white;

  text-decoration: none;

  display: inline-block;
}
</style>
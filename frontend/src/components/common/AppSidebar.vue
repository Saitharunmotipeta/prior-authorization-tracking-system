<script setup lang="ts">
import {
  LayoutDashboard,
  FilePlus2,
  ClipboardList,
  Bell
} from "lucide-vue-next";

import {
  RouterLink
} from "vue-router";
</script>

<template>
  <aside class="sidebar">

    <div class="sidebar-header">

      <div class="logo">
        HCA
      </div>

      <div>

        <h2>
          HCA Healthcare
        </h2>

        <p>
          Specialist Portal
        </p>

      </div>

    </div>

    <nav class="nav-menu">

      <RouterLink
        to="/specialist"
        class="nav-link"
      >

        <LayoutDashboard
          :size="18"
        />

        Dashboard

      </RouterLink>

      <RouterLink
        to="/specialist/eligibility"
        class="nav-link"
      >

        <FilePlus2
          :size="18"
        />

        Create Entry

      </RouterLink>

      <RouterLink
        to="/specialist/requests"
        class="nav-link"
      >

        <ClipboardList
          :size="18"
        />

        Requests

      </RouterLink>

      <RouterLink
        to="/specialist/reminders"
        class="nav-link"
      >

        <Bell
          :size="18"
        />

        Reminders

      </RouterLink>

    </nav>

    <div class="sidebar-footer">

      <div class="status-dot" />

      System Online

    </div>

  </aside>
</template>

<style scoped>
.sidebar {
  width: 260px;

  background: white;

  border-right:
    1px solid #e5e7eb;

  display: flex;

  flex-direction: column;

  justify-content:
    space-between;

  padding: 24px 16px;

  box-shadow:
    1px 0 2px
    rgb(0 0 0 / 4%);
}

.sidebar-header {
  display: flex;

  align-items: center;

  gap: 12px;

  margin-bottom: 40px;
}

.logo {
  width: 48px;

  height: 48px;

  border-radius: 12px;

  background: #2563eb;

  color: white;

  display: flex;

  align-items: center;

  justify-content: center;

  font-weight: 700;

  font-size: 14px;
}

.sidebar-header h2 {
  margin: 0;

  font-size: 16px;

  color: #1e293b;
}

.sidebar-header p {
  margin: 0;

  font-size: 12px;

  color: #64748b;
}

.nav-menu {
  display: flex;

  flex-direction: column;

  gap: 8px;

  flex: 1;
}

.nav-link {
  display: flex;

  align-items: center;

  gap: 12px;

  padding: 12px 14px;

  border-radius: 10px;

  text-decoration: none;

  color: #475569;

  font-weight: 500;

  transition:
    all 0.2s ease;
}

.nav-link:hover {
  background: #f1f5f9;

  color: #2563eb;
}

.nav-link.router-link-active {
  background: #eff6ff;

  color: #2563eb;

  font-weight: 600;
}

.sidebar-footer {
  display: flex;

  align-items: center;

  gap: 10px;

  padding-top: 20px;

  border-top:
    1px solid #e5e7eb;

  font-size: 13px;

  color: #64748b;
}

.status-dot {
  width: 10px;

  height: 10px;

  border-radius: 50%;

  background: #22c55e;
}
</style>
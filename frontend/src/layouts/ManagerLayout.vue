<script setup lang="ts">
import { RouterView } from "vue-router";

import ManagerSidebar
from "../components/common/ManagerSidebar.vue";
</script>

<template>
  <div class="layout">

    <ManagerSidebar />

    <div class="main-section">

      <header class="topbar">

        <div class="topbar-left">

          <h2>
            HCA Prior Authorization
          </h2>

          <span>
            Manager Portal
          </span>

        </div>

        <div class="topbar-right">

          <div class="profile">

            <div class="avatar">
              MG
            </div>

            <div>

              <div class="name">
                Manager User
              </div>

              <div class="role">
                Facility Manager
              </div>

            </div>

          </div>

        </div>

      </header>

      <main class="content">
        <RouterView />
      </main>

    </div>

  </div>
</template>

<style scoped>
.layout {
  display: flex;
  min-height: 100vh;
  background: #f8fafc;
}

.main-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.topbar {
  height: 72px;
  background: white;
  border-bottom: 1px solid #e5e7eb;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  position: sticky;
  top: 0;
  z-index: 100;
}

.topbar-left h2 {
  margin: 0;
  font-size: 18px;
  font-weight: 700;
  color: #1e293b;
}

.topbar-left span {
  font-size: 13px;
  color: #64748b;
}

.profile {
  display: flex;
  align-items: center;
  gap: 12px;
}

.avatar {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  background: #2563eb;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
}

.name {
  font-weight: 600;
  color: #1e293b;
}

.role {
  font-size: 12px;
  color: #64748b;
}

.content {
  flex: 1;
  overflow-y: auto;
  padding: 24px;
}
</style>
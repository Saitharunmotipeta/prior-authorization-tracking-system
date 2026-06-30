<script setup lang="ts">
import { RouterView } from "vue-router";

import AppSidebar
from "../components/common/AppSidebar.vue";
</script>

<template>

  <div class="layout">

    <AppSidebar />

    <div class="main-section">

      <header class="topbar">

        <div class="topbar-left">

          <h2>
            Specialist Portal
          </h2>

        </div>

        <div class="topbar-right">

          <div class="profile">

            <div class="avatar">
              ST
            </div>

            <div>

              <div class="name">
                Specialist User
              </div>

              <div class="role">
                Prior Authorization Specialist
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

  border-bottom:
    1px solid #e5e7eb;

  display: flex;

  align-items: center;

  justify-content:
    space-between;

  padding: 0 24px;

  box-shadow:
    0 1px 2px
    rgb(0 0 0 / 5%);

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

.topbar {
  height: 72px;

  background: white;

  border-bottom:
    1px solid #e5e7eb;

  display: flex;

  align-items: center;

  justify-content: space-between;

  padding: 0 24px;

  position: sticky;

  top: 0;

  z-index: 100;
}

.brand {
  font-size: 18px;

  font-weight: 700;

  color: #1e293b;

  min-width: 200px;
}

.page-name {
  flex: 1;

  text-align: center;

  font-size: 18px;

  font-weight: 600;

  color: #334155;
}

.profile {
  min-width: 200px;

  display: flex;

  justify-content: flex-end;
}

.profile img {
  width: 42px;

  height: 42px;

  border-radius: 50%;

  border: 2px solid #e5e7eb;

  cursor: pointer;
}
</style>
<script setup lang="ts">
import {
  LayoutDashboard,
  Building2,
  ShieldCheck,
  FileBarChart2,
  LogOut
} from "lucide-vue-next";
</script>

<template>
  <aside class="sidebar">

    <div class="logo">

      <div class="logo-icon">
        HCA
      </div>

      <div>

        <h3>HCA Healthcare</h3>

        <span>Manager Portal</span>

      </div>

    </div>

    <nav class="menu">

      <RouterLink
        to="/manager/dashboard"
        class="menu-item"
      >
        <LayoutDashboard :size="18" />
        <span>Dashboard</span>
      </RouterLink>

      <RouterLink
        to="/manager/facilities"
        class="menu-item"
      >
        <Building2 :size="18" />
        <span>Facility Performance</span>
      </RouterLink>

      <RouterLink
        to="/manager/payers"
        class="menu-item"
      >
        <ShieldCheck :size="18" />
        <span>Payer Analysis</span>
      </RouterLink>

      <RouterLink
        to="/manager/authorizations"
        class="menu-item"
      >
        <FileBarChart2 :size="18" />
        <span>Authorization Analysis</span>
      </RouterLink>

    </nav>

  </aside>
</template>

<style scoped>
.sidebar {
  width: 260px;
  background: white;
  border-right: 1px solid #e5e7eb;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 24px 16px;
  box-shadow: 1px 0 2px rgb(0 0 0 / 4%);
}

.logo {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 40px;
}

.logo-icon {
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
  flex-shrink: 0;
}

.logo h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 700;
  color: #1e293b;
  line-height: 1.2;
}

.logo span {
  display: block;
  margin-top: 2px;
  font-size: 12px;
  color: #64748b;
}

.menu {
  display: flex;
  flex-direction: column;
  gap: 8px;
  flex: 1;
}

.menu-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 14px;
  border-radius: 10px;
  text-decoration: none;
  color: #475569;
  font-size: 15px;
  font-weight: 500;
  transition: all 0.2s ease;
}

.menu-item:hover {
  background: #f1f5f9;
  color: #2563eb;
}

.menu-item.router-link-active {
  background: #eff6ff;
  color: #2563eb;
  font-weight: 600;
}

.menu-item svg {
  width: 18px;
  height: 18px;
  flex-shrink: 0;
}
</style>
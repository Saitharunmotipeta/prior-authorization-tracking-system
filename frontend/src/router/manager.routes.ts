import type { RouteRecordRaw } from "vue-router";

import ExecutiveDashboardPage from "../pages/manager/ExecutiveDashboardPage.vue";

const managerRoutes: RouteRecordRaw[] = [
  {
    path: "/manager",
    name: "ManagerDashboard",
    component: ExecutiveDashboardPage
  }
];

export default managerRoutes;
import type { RouteRecordRaw } from "vue-router";

import PayerDashboardPage from "../pages/Payer/PayerDashboardPage.vue";
import RemindersPage from "../pages/Payer/RemindersPage.vue";
import AuditHistoryPage from "../pages/Payer/AuditHistoryPage.vue";
import PayerLayout from "../layouts/PayerLayout.vue";

export const payerRoutes: RouteRecordRaw[] = [
  {
    path: "/payer",
    component: PayerLayout,

    children: [
      {
        path: "",
        redirect: "/payer/requests"
      },

      {
        path: "requests",
        name: "PayerDashboard",     // <-- ADD
        component: PayerDashboardPage
      },

      {
        path: "reminders",
        name: "PayerReminders",     // <-- ADD
        component: RemindersPage
      },

      {
        path: "history",
        name: "PayerHistory",       // <-- ADD
        component: AuditHistoryPage
      }
    ]
  }
];
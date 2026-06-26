import type { RouteRecordRaw } from "vue-router";

import PayerDashboardPage from "../pages/payer/PayerDashboardPage.vue";
import RemindersPage from "../pages/payer/RemindersPage.vue";
import AuditHistoryPage from "../pages/payer/AuditHistoryPage.vue";
import PayerLayout from "../layouts/payerLayout.vue";

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
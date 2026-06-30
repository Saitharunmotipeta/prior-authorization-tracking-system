import type { RouteRecordRaw } from "vue-router";

import ManagerLayout
from "../layouts/ManagerLayout.vue";

import ExecutiveDashboardPage
from "../pages/manager/ExecutiveDashboardPage.vue";

import FacilityPerformancePage from "../pages/manager/FacilityPerformancePage.vue";
import PayerAnalysisPage from "../pages/manager/PayerAnalysisPage.vue";
import AuthorizationAnalysisPage from "../pages/manager/AuthorizationAnalysisPage.vue";
import ExecutiveReportPage from "../pages/manager/ExecutiveReportPage.vue";

const managerRoutes: RouteRecordRaw[] = [
  {
    path: "/manager",
    component: ManagerLayout,
    children: [
      {
        path: "",
        redirect: "/manager/dashboard"
      },
      {
        path: "dashboard",
        name: "ManagerDashboard",
        component: ExecutiveDashboardPage
      },
      {
        path: "facilities",
        name: "FacilityPerformance",
        component: FacilityPerformancePage
      },
      {
        path: "payers",
        name: "PayerAnalysis",
        component: PayerAnalysisPage
      },
      {
        path: "authorizations",
        name: "AuthorizationAnalysis",
        component: AuthorizationAnalysisPage
      },
      {
        path: "/manager/executive-report",
        component: ExecutiveReportPage
      }
    ]
  }
];

export default managerRoutes;
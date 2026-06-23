import type { RouteRecordRaw } from "vue-router";

import SpecialistDashboardPage
from "../pages/specialist/SpecialistDashboardPage.vue";

import EligibilityVerificationPage
from "../pages/specialist/EligibilityVerificationPage.vue";

export const specialistRoutes: RouteRecordRaw[] = [
  {
    path: "/specialist",
    name: "SpecialistDashboard",
    component: SpecialistDashboardPage
  },

  {
    path: "/specialist/eligibility",
    name: "EligibilityVerification",
    component: EligibilityVerificationPage
  }
];
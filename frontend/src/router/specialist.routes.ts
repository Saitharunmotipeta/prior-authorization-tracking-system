import type { RouteRecordRaw } from "vue-router";

import SpecialistDashboardPage
from "../pages/specialist/SpecialistDashboardPage.vue";

import EligibilityVerificationPage
from "../pages/specialist/EligibilityVerificationPage.vue";

import CreateEncounterPage
from "../pages/specialist/CreateEncounterPage.vue";

import DocumentVerificationPage
from "../pages/specialist/DocumentVerificationPage.vue";

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
  },

  {
    path: "/specialist/create-encounter",
    name: "CreateEncounter",
    component: CreateEncounterPage
    },

    {
  path: "/specialist/document-verification",
  name: "DocumentVerification",
  component: DocumentVerificationPage
}
];
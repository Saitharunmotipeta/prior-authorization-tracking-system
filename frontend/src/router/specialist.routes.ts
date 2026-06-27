import type { RouteRecordRaw } from "vue-router";

import SpecialistLayout
from "../layouts/SpecialistLayout.vue";

import SpecialistDashboardPage
from "../pages/specialist/SpecialistDashboardPage.vue";

import EligibilityVerificationPage
from "../pages/specialist/EligibilityVerificationPage.vue";

import CreateEncounterPage
from "../pages/specialist/CreateEncounterPage.vue";

import DocumentVerificationPage
from "../pages/specialist/DocumentVerificationPage.vue";

import CreateAuthorizationRequestPage
from "../pages/specialist/CreateAuthorizationRequestPage.vue";

import AddAuthorizationServicesPage
from "../pages/specialist/AddAuthorizationServicesPage.vue";

import AuthorizationSummaryPage
from "../pages/specialist/AuthorizationSummaryPage.vue";

import SpecialistRequestsPage
from "@/pages/specialist/SpecialistRequestsPage.vue";
import RemindersPage
from "../pages/specialist/RemindersPage.vue";

export const specialistRoutes:
  RouteRecordRaw[] = [
  {
    path: "/specialist",

    component:
      SpecialistLayout,

    children: [
      {
        path: "",

        name:
          "SpecialistDashboard",

        component:
          SpecialistDashboardPage
      },

      {
        path:
          "eligibility",

        name:
          "EligibilityVerification",

        component:
          EligibilityVerificationPage
      },

      {
        path:
          "create-encounter",

        name:
          "CreateEncounter",

        component:
          CreateEncounterPage
      },

      {
        path:
          "document-verification",

        name:
          "DocumentVerification",

        component:
          DocumentVerificationPage
      },

      {
        path:
          "create-authorization",

        name:
          "CreateAuthorization",

        component:
          CreateAuthorizationRequestPage
      },

      {
        path:
          "add-services",

        name:
          "AddAuthorizationServices",

        component:
          AddAuthorizationServicesPage
      },

      {
        path:
          "authorization-summary",

        name:
          "AuthorizationSummary",

        component:
          AuthorizationSummaryPage
      },
      {
        path: "requests",
        component:
          SpecialistRequestsPage
      },
      {
    path: "reminders",

    name: "SpecialistReminders",

    component: RemindersPage
}
   
    ]
  }
];
import {
  createRouter,
  createWebHistory
} from "vue-router";

import managerRoutes
from "./manager.routes";

import {
  specialistRoutes
} from "./specialist.routes";

import {
  payerRoutes
} from "./payer.routes";

const router =
  createRouter({
    history:
      createWebHistory(),

    routes: [

      ...managerRoutes,

      ...specialistRoutes,

      ...payerRoutes

    ]
  });

export default router;
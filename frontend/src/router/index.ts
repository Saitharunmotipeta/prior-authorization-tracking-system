import {
  createRouter,
  createWebHistory
} from "vue-router";

import managerRoutes from "./manager.routes";

import {
  specialistRoutes
} from "./specialist.routes";

const router = createRouter({
  history: createWebHistory(),

  routes: [
    ...managerRoutes,

    ...specialistRoutes
  ]
});

export default router;
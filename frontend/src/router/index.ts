import { createRouter, createWebHistory } from "vue-router";

import managerRoutes from "./manager.routes";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    ...managerRoutes
  ]
});

export default router;
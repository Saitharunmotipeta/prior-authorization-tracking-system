import { createRouter, createWebHistory } from "vue-router";

import managerRoutes from "./manager.routes";
import { specialistRoutes } from "./specialist.routes";

// ✅ Use alias (BEST PRACTICE)
import PayerLayout from "../layouts/Payer/MainLayout.vue";

const routes = [
  // ✅ Existing routes
  ...managerRoutes,
  ...specialistRoutes,

  // ✅ Payer routes
  {
  path: '/payer',
  component: PayerLayout,
  children: [
    {
      path: '',
      component: () => import('../pages/Payer/Dashboard.vue'),
    }
  ]
}

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
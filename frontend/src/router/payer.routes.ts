import type {
  RouteRecordRaw
} from "vue-router";

import PayerDashboardPage
from "../pages/payer/PayerDashboardPage.vue";

import RemindersPage
from "../pages/payer/RemindersPage.vue";

import AuditHistoryPage
from "../pages/payer/AuditHistoryPage.vue";

import PayerLayout
from "../layouts/PayerLayout.vue";

export const payerRoutes:
RouteRecordRaw[] = [

{

path:"/payer",

component:PayerLayout,

children:[

{

path:"",

redirect:"/payer/requests"

},

{

path:"requests",

component:
PayerDashboardPage

},

{

path:"reminders",

component:
RemindersPage

},

{

path:"history",

component:
AuditHistoryPage

}

]

}

];
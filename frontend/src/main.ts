import { createApp } from "vue";
import { createPinia } from "pinia";

import 'bootstrap/dist/css/bootstrap.min.css';
import piniaPluginPersistedstate
from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";
import "primeicons/primeicons.css";

const app = createApp(App);

const pinia = createPinia();
app.use(pinia);
pinia.use(
  piniaPluginPersistedstate
);
app.use(router);

app.mount("#app");
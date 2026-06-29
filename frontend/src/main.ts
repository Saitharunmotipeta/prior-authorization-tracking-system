import { createApp } from "vue";
import { createPinia } from "pinia";
import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";

import "bootstrap/dist/css/bootstrap.min.css";
import "primeicons/primeicons.css";

import "vuetify/styles";
import { createVuetify } from "vuetify";

// 1. Initialize Vue App
const app = createApp(App);

// 2. Initialize Vuetify
const vuetify = createVuetify();
app.use(vuetify);

// 3. Initialize Pinia with Persistence Plugin
const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);
app.use(pinia);

// 4. Initialize Router
app.use(router);

// 5. Mount App
app.mount("#app");
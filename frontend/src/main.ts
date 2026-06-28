import { createApp } from "vue";

import { createPinia } from "pinia";
import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";

import "bootstrap/dist/css/bootstrap.min.css";
import "primeicons/primeicons.css";

import "vuetify/styles";
import { createVuetify } from "vuetify";


const app = createApp(App);


const vuetify = createVuetify();

app.use(vuetify);


const pinia = createPinia();

pinia.use(piniaPluginPersistedstate);

app.use(pinia);


app.use(router);


app.mount("#app");
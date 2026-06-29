import { createApp } from "vue";

import { createPinia } from "pinia";
import 'bootstrap/dist/css/bootstrap.min.css';

import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";

import "primeicons/primeicons.css";

import "vuetify/styles";
import { createVuetify } from "vuetify";

import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
})

/* ✅ App setup */
const app = createApp(App);


app.use(vuetify);


const pinia = createPinia();

pinia.use(piniaPluginPersistedstate);

app.use(pinia);


app.use(router);


app.mount("#app");

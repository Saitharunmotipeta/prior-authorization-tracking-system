import { createApp } from "vue";
import { createPinia } from "pinia";

import 'bootstrap/dist/css/bootstrap.min.css';
import piniaPluginPersistedstate
from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";
// import "primeicons/primeicons.css";




const app = createApp(App);

const pinia = createPinia();
app.use(pinia);
pinia.use(
  piniaPluginPersistedstate
);



import { createVuetify } from 'vuetify';
import 'vuetify/styles'; 

const vuetify = createVuetify();


// createApp(App)
//   .use(vuetify)
//   .mount('#app');

// app.use(router);

// app.mount("#app");


app.use(vuetify)
app.use(router)
app.mount('#app')



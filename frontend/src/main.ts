// import { createApp } from "vue";
// import { createPinia } from "pinia";

// import 'bootstrap/dist/css/bootstrap.min.css';
// import piniaPluginPersistedstate
// from "pinia-plugin-persistedstate";

// import App from "./App.vue";
// import router from "./router";
// // import "primeicons/primeicons.css";




// const app = createApp(App);

// const pinia = createPinia();
// app.use(pinia);
// pinia.use(
//   piniaPluginPersistedstate
// );



// import { createVuetify } from 'vuetify';
// import 'vuetify/styles'; 

// const vuetify = createVuetify();


// // createApp(App)
// //   .use(vuetify)
// //   .mount('#app');

// // app.use(router);

// // app.mount("#app");


// app.use(vuetify)
// app.use(router)
// app.mount('#app')



import { createApp } from "vue";
import { createPinia } from "pinia";

import 'bootstrap/dist/css/bootstrap.min.css';

import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";

/* ✅ Vuetify setup */
import { createVuetify } from 'vuetify'
import 'vuetify/styles'

import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
})

/* ✅ App setup */
const app = createApp(App);

const pinia = createPinia();
app.use(pinia);
pinia.use(piniaPluginPersistedstate);

app.use(vuetify)
app.use(router)

app.mount('#app')
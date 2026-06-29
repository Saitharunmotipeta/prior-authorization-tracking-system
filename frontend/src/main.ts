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

<<<<<<< HEAD
import { createPinia } from "pinia";
=======
import 'bootstrap/dist/css/bootstrap.min.css';

>>>>>>> 760f8237fdb9ee8c2ffed1e8aa518ba9828ee716
import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

import App from "./App.vue";
import router from "./router";
<<<<<<< HEAD

import "bootstrap/dist/css/bootstrap.min.css";
import "primeicons/primeicons.css";

import "vuetify/styles";
import { createVuetify } from "vuetify";
=======

/* ✅ Vuetify setup */
import { createVuetify } from 'vuetify'
import 'vuetify/styles'

import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
>>>>>>> 760f8237fdb9ee8c2ffed1e8aa518ba9828ee716

const vuetify = createVuetify({
  components,
  directives,
})

/* ✅ App setup */
const app = createApp(App);

<<<<<<< HEAD

const vuetify = createVuetify();

app.use(vuetify);


const pinia = createPinia();

pinia.use(piniaPluginPersistedstate);

app.use(pinia);


app.use(router);


app.mount("#app");
=======
const pinia = createPinia();
app.use(pinia);
pinia.use(piniaPluginPersistedstate);

app.use(vuetify)
app.use(router)

app.mount('#app')
>>>>>>> 760f8237fdb9ee8c2ffed1e8aa518ba9828ee716

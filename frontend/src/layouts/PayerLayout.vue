<script setup lang="ts">
import { RouterView } from "vue-router";

import AppNavbar
from "../components/payer/PayerNavbar.vue";

import PayerSidebar
from "../components/payer/PayerSidebar.vue";
</script>

<template>

<div class="layout">

  <PayerNavbar
    title="Payer Dashboard"
  />

  <div class="body">

    <PayerSidebar />

    <main class="content">

      <RouterView />

    </main>

  </div>

</div>

</template>

<style scoped>

.layout{

height:100vh;

display:flex;

flex-direction:column;

background:#f8fafc;

overflow:hidden;

}

.body{

flex:1;

display:flex;

overflow:hidden;

}

.content{

flex:1;

overflow-y:auto;

padding:24px;

}

</style>
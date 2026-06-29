<script setup lang="ts">
import { RouterLink } from "vue-router";

import {
  ClipboardList,
  Bell,
  History
} from "lucide-vue-next";
</script>

<template>

<aside class="sidebar">


<nav>

<RouterLink
to="/payer/requests"
class="item"
>

<ClipboardList
:size="20"
/>

<span>

Requests

</span>

</RouterLink>

<RouterLink
to="/payer/reminders"
class="item"
>

<Bell
:size="20"
/>

<span>

Reminders

</span>

</RouterLink>

<RouterLink
to="/payer/history"
class="item"
>

<History
:size="20"
/>

<span>

Recent Activity

</span>

</RouterLink>

</nav>

</aside>

</template>

<style scoped>

.sidebar{

width:260px;

background:white;

color:Blue;

padding:24px;

display:flex;

flex-direction:column;

gap:30px;

overflow-y:auto;

}

.logo{

font-size:22px;

font-weight:700;

padding-bottom:18px;

border-bottom:1px solid rgba(255,255,255,.15);

}

nav{

display:flex;

flex-direction:column;

gap:10px;

}

.item{

display:flex;

align-items:center;

gap:12px;

padding:14px;

border-radius:10px;

text-decoration:none;

color:Blue;

transition:.2s;

}

.item:hover{

background:#334155;
color:white;

}

.router-link-active{

background:#2563eb;

color:white;

}

</style>
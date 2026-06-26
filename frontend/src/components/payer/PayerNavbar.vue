<script setup lang="ts">
import {
  Building2,
  CircleUserRound
} from "lucide-vue-next";
</script>

<template>

<header class="navbar">

  <div class="left">

    <Building2
      :size="28"
    />

    <span class="logo">
      HCA Healthcare
    </span>

  </div>

  <div class="center">

    <h2>
      Payer Dashboard
    </h2>

  </div>

  <div class="right">

    <div class="profile">

      <CircleUserRound
        :size="34"
      />

      <div>

        <div class="name">
          John Doe
        </div>

        <div class="role">
          Payer
        </div>

      </div>

    </div>

  </div>

</header>

</template>

<style scoped>

.navbar{

height:72px;

display:flex;

justify-content:space-between;

align-items:center;

padding:0 28px;

background:white;

border-bottom:1px solid #e5e7eb;

box-shadow:
0 2px 8px
rgb(0 0 0 / 5%);

flex-shrink:0;

z-index:100;

}

.left{

display:flex;

align-items:center;

gap:12px;

color:#2563eb;

font-weight:700;

}

.logo{

font-size:22px;

}

.center h2{

margin:0;

font-size:24px;

font-weight:600;

color:#1e293b;

}

.right{

display:flex;

align-items:center;

}

.profile{

display:flex;

align-items:center;

gap:10px;

padding:8px 14px;

border-radius:12px;

background:#f8fafc;

border:1px solid #e2e8f0;

}

.name{

font-size:14px;

font-weight:600;

color:#1e293b;

}

.role{

font-size:12px;

color:#64748b;

}

</style>
<script setup lang="ts">
import AuthorizationDetailsCard
from "./AuthorizationDetailsCard.vue";

import type {
  AuthorizationDetails
} from "../../types/payer.interface";

defineProps<{
  open: boolean;

  authorization:
    AuthorizationDetails | null;
}>();

const emit =
defineEmits<{
(
e:"close"
):void;

(
e:"review",
payload:any
):void;
}>();
</script>

<template>

<transition name="drawer">

<div
  v-if="open"
  class="overlay"
>

  <div class="drawer">

    <div class="drawer-header">

      <h2>

        Authorization Details

      </h2>

      <button
        class="close-btn"
        @click="
          emit('close')
        "
      >
        ✕
      </button>

    </div>

    <div
      class="drawer-body"
    >

      <AuthorizationDetailsCard
  :authorization="authorization"
  @review="emit('review',$event)"
/>

    </div>

  </div>

</div>

</transition>

</template>

<style scoped>

.overlay{

position:fixed;

top:0;

right:0;

width:100vw;

height:100vh;

background:
rgb(0 0 0 / 30%);

display:flex;

justify-content:flex-end;

z-index:1000;

}

.drawer{

width:700px;

height:100vh;

background:white;

display:flex;

flex-direction:column;

box-shadow:
-8px 0 30px
rgb(0 0 0 / 15%);

}

.drawer-header{

display:flex;

justify-content:space-between;

align-items:center;

padding:20px;

border-bottom:1px solid #e5e7eb;

background:white;

position:sticky;

top:0;

z-index:10;

}

.drawer-body{

flex:1;

overflow-y:auto;

padding:20px;

background:#f8fafc;

}

.close-btn{

border:none;

background:none;

font-size:26px;

cursor:pointer;

}

.drawer-enter-active,
.drawer-leave-active{

transition:.25s ease;

}

.drawer-enter-from,
.drawer-leave-to{

opacity:0;

transform:
translateX(100%);

}

</style>
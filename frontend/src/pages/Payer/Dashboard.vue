<script setup lang="ts">
import { ref } from 'vue'
import Facilities from './Facilities.vue'
import Reminder from './Reminder.vue'
import Emergency from './Emergency.vue'
import AuditHistory from './AuditHistory.vue'

const view = ref('main')

const showReminders = ref(false)
</script>

<template>
  <div>

    <!-- ✅ HEADER -->
    <nav class="navbar navbar-dark" style="background-color:#003B8F;">
      <div class="container-fluid d-flex justify-content-between">
        <span class="navbar-brand mb-0 h1">Prior Authorization</span>

        <button 
          class="btn text-white"
          style="background-color:#0072CE;"
          @click="showReminders = !showReminders"
        >
          🔔 Reminders
        </button>
      </div>
    </nav>

    <!-- ✅ MAIN -->
    <div class="container-fluid mt-3">
      <Facilities />
    </div>

    <!-- ✅ REMINDER PANEL -->
    <div 
      v-if="showReminders"
      class="position-fixed top-0 end-0 bg-white shadow p-3"
      style="width:300px; height:100vh;"
    >
      <Reminder />
    </div>

    <div class="d-flex gap-2">

  <button class="btn btn-primary" @click="view = 'main'">
    Dashboard
  </button>

  <button class="btn btn-danger" @click="view = 'emergency'">
    Emergency
  </button>

  <button class="btn btn-secondary" @click="view = 'audit'">
    Audit
  </button>

</div>

  </div>
</template>
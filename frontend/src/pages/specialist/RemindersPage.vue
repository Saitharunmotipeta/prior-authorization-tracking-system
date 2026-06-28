<template>

  <div class="reminder-page">


    <div class="page-header">

      <h1>
        Reminders
      </h1>

      <p class="subtitle">
        Authorization status updates and payer notifications
      </p>

    </div>



    <div
      v-if="loading"
      class="loading"
    >

      Loading reminders...

    </div>




    <div
      v-else-if="reminders.length === 0"
      class="empty"
    >

      No reminders found.

    </div>




    <div v-else>


      <div
        v-for="reminder in reminders"
        :key="reminder.reminderId"
        class="reminder-card"
        @click="openAuthorization(reminder.authId)"
      >



        <!-- ICON -->

        <div class="icon">

          <BellRing :size="24" />

        </div>





        <!-- CONTENT -->

        <div class="content">


          <h3>
            Authorization Update
          </h3>



          <p class="authorization-id">

            Authorization ID:

            <strong>
              #{{ reminder.authId }}
            </strong>

          </p>



          <p class="payer">

            Payer:

            <strong>
              {{ reminder.payerName }}
            </strong>

          </p>




          <p class="message">

            Authorization status changed to:

            <strong>
              {{ reminder.status }}
            </strong>

          </p>




          <p class="date">

            Updated:

            {{ formatDate(reminder.updatedAt) }}

          </p>



        </div>






        <!-- STATUS -->

        <div
          class="status"
          :class="
            'status-' +
            reminder.status.toLowerCase()
          "
        >

          {{ reminder.status }}

        </div>




      </div>


    </div>


  </div>


</template>

<script setup lang="ts">

import { ref, onMounted } from "vue";
import { getReminders } from "../../api/specialist.api";
import { useRouter } from "vue-router";
import {
  BellRing
} from "lucide-vue-next";
const router = useRouter();

export interface Reminder {

  reminderId: number;

  authId: number;

  payerName: string;

  status: string;

  updatedAt: string;

}


const reminders = ref<Reminder[]>([]);

const loading = ref(false);

//const error = ref<string | null>(null);

const openAuthorization = (authId: number) => {

  router.push({
    path: "/specialist/requests",
    query: {
      authId: authId.toString()
    }
  });

};

const fetchReminders = async () => {

  try {

    loading.value = true;

    const response = await getReminders();

    console.log("Reminder response:", response);

    reminders.value = response.data;

  } 
  
  catch (error) {

    console.error(
      "Failed to fetch reminders:",
      error
    );

  } 
  
  finally {

    loading.value = false;

  }

};



const formatDate = (date:string)=>{

  if(!date)
    return "-";

  return new Date(date).toLocaleDateString();

};



onMounted(()=>{

  fetchReminders();

});


</script>



<style scoped>
.reminder-page {

  padding:24px;

  background:#f8fafc;

  min-height:100vh;

}


.page-header {

  margin-bottom:24px;

}


.subtitle {

  color:#64748b;

  margin-top:6px;

}


.reminder-card {

  display:flex;

  align-items:center;

  gap:18px;

  background:white;

  border:1px solid #e5e7eb;

  border-radius:12px;

  padding:18px;

  margin-bottom:15px;

  cursor:pointer;

  transition:0.2s;

}


.reminder-card:hover {

  transform:translateY(-2px);

  box-shadow:
    0 4px 12px
    rgba(0,0,0,0.08);

}


.icon {

  font-size:28px;

}


.content {

  flex:1;

}


.content h3 {

  margin:0 0 8px;

}


.payer {

  font-weight:600;

  margin:4px 0;

}


.message {

  color:#475569;

}


.date {

  color:#64748b;

  font-size:13px;

}


.status {

  padding:8px 14px;

  border-radius:20px;

  font-weight:600;

  background:#fef3c7;

}
</style>
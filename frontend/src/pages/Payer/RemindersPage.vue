<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const reminders = ref<any[]>([]);
const loading = ref(false);
const error = ref("");


const fetchReminders = async () => {
  try {
    loading.value = true;

    const res = await fetch("https://localhost:7190/api/payers/reminders");
    const result = await res.json();

    if (!result.success) {
      throw new Error(result.message);
    }

    reminders.value = result.data.data;
  } catch (err: any) {
    error.value = err.message;
  } finally {
    loading.value = false;
  }
};

const openReminder = (authId: number) => {
  router.push({
    name: "PayerDashboard",
    query: {
      authId: authId.toString()
    }
  });
};

const formatDate = (date: string) =>
  new Date(date).toLocaleString();

onMounted(fetchReminders);
</script>

<template>
  <div class="card">
   <div class="reminder-header">
  <h2>Reminders</h2>

  <v-icon
    size="20"
    color="grey-darken-1"
  >
    mdi-clock-outline
  </v-icon>
</div>



    <p v-if="loading">Loading...</p>

    <p v-else-if="error" class="error">
      {{ error }}
    </p>

    <div v-else>
      <div
        v-for="r in reminders"
        :key="r.reminderId"
        class="reminder-card"
        @click="openReminder(r.authId)"
      >
        <div class="reminder-header">
          <span class="auth-id">
            Authorization #{{ r.authId }}
          </span>

          <span class="date">
            {{ formatDate(r.scheduledAt) }}
          </span>
        </div>

        <p class="status">
  Status: <strong>{{ r.status }}</strong>
</p>
      </div>

      <p v-if="reminders.length === 0">
        No reminders available.
      </p>
    </div>
  </div>
</template>

<style scoped>
.card {
  background: white;
  padding: 24px;
  border-radius: 12px;
}

.reminder-card {
  border-left: 4px solid #0072ce;
  background: #fff;
  padding: 12px 16px;
  margin-bottom: 12px;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.2s;
}

.reminder-card:hover {
  background: #f8fafc;
}

.reminder-header {
  display: flex;
  align-items: center;
  gap: 8px;

  font-size: 20px;
  font-weight: 600;
  color: #1e293b;
}
.auth-id {
  font-weight: 600;
  color: #1e293b;
}

.date {
  font-size: 0.8rem;
  color: #64748b;
}

.status {
  margin: 0;
  font-size: 0.95rem;
  color: #475569;
}

.status strong {
  color: #0072ce;
}

.reminder-header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  font-size: 18px;
  font-weight: 600;

  margin-bottom: 12px;
}

.title {
  color: #1e293b;
}

.icon {
  margin-left: 6px;
}
</style>
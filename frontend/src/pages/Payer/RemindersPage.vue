<script setup lang="ts">
import { ref, onMounted , computed } from "vue";

import { useRouter } from "vue-router";
import type { Reminder } from "../../types/payer.interface";
const reminders = ref<Reminder[]>([]);
const router = useRouter();

const loading = ref(false);
const error = ref("");
const showEmergencyOnly = ref(false);

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
const displayedReminders = computed(() => {
  

  if (!showEmergencyOnly.value) {
    return reminders.value;
  }

  const filtered = reminders.value.filter(
    reminder => reminder.priority === "Emergency"
  );

  console.log("Emergency reminders:", filtered);

  return filtered;
});
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
<div class="reminder-controls">
  <label class="toggle">
    <input
      type="checkbox"
      v-model="showEmergencyOnly"
    />

    <span class="slider"></span>

    <span class="toggle-text">
      Show Emergency Only
    </span>
  </label>
</div>
  <div class="card">
    <h1>Reminders </h1>

    <p v-if="loading">Loading...</p>

    <p v-else-if="error" class="error">
      {{ error }}
    </p>

    <div v-else>
      <div
        v-for="r in displayedReminders"
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
  justify-content: space-between;
  align-items: center;
  margin-bottom: 6px;
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
.reminder-controls {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 20px;
}

.toggle {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
}

.toggle input {
  display: none;
}

.slider {
  position: relative;
  width: 42px;
  height: 22px;
  background-color: #ccc;
  border-radius: 20px;
  transition: 0.3s;
}

.slider::before {
  content: "";
  position: absolute;
  height: 18px;
  width: 18px;
  left: 2px;
  top: 2px;
  background-color: white;
  border-radius: 50%;
  transition: 0.3s;
}

.toggle input:checked + .slider {
  background-color: #dc2626;
}

.toggle input:checked + .slider::before {
  transform: translateX(20px);
}

.toggle-text {
  color: #374151;
}
</style>
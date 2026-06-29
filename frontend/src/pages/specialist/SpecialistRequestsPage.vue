<script setup lang="ts">
import { ref, onMounted, computed, watch } from "vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useSpecialistStore } from "../../stores/specialist.store";

const route = useRoute();
const specialistStore = useSpecialistStore();

const {
  authorizationRequests,
  authorizationTimeline,
  authorizationServices,
  loading
} = storeToRefs(specialistStore);

// Search & Pagination Logic
const currentPage = ref(1);
const pageSize = 8;
const searchText = ref("");

const filteredRequests = computed(() => {
  if (!searchText.value.trim()) {
    return authorizationRequests.value;
  }
  const keyword = searchText.value.toLowerCase();
  return authorizationRequests.value.filter(
    (request) =>
      request.authId.toString().includes(keyword) ||
      request.patientName.toLowerCase().includes(keyword) ||
      request.payerName.toLowerCase().includes(keyword) ||
      request.status.toLowerCase().includes(keyword) ||
      request.priority.toLowerCase().includes(keyword)
  );
});

const totalPages = computed(() =>
  Math.max(1, Math.ceil(filteredRequests.value.length / pageSize))
);

const paginatedRequests = computed(() => {
  const start = (currentPage.value - 1) * pageSize;
  return filteredRequests.value.slice(start, start + pageSize);
});

watch(searchText, () => {
  currentPage.value = 1;
});

// Modal & Timeline Logic
const showHistoryModal = ref(false);
const selectedAuthId = ref<number | null>(null);
const showTimeline = ref(false);

const formatDate = (date: string | null) => {
  if (!date) return "-";
  return new Date(date).toLocaleDateString();
};

const viewRequestHistory = async (authId: number) => {
  selectedAuthId.value = authId;
  showTimeline.value = false;
  
  // Clear previous loaded data
  specialistStore.authorizationDetails = null;
  specialistStore.authorizationServices = [];
  specialistStore.authorizationTimeline = [];
  
  // Eager load details, services, and timeline
  await specialistStore.loadAuthorizationDetails(authId);
  await specialistStore.loadAuthorizationServices(authId);
  await specialistStore.loadAuthorizationTimeline(authId);
  
  showHistoryModal.value = true;
};

const closeHistoryModal = () => {
  showHistoryModal.value = false;
  showTimeline.value = false;
  selectedAuthId.value = null;
  specialistStore.authorizationTimeline = [];
  specialistStore.authorizationServices = [];
  specialistStore.authorizationDetails = null;
};

onMounted(async () => {
  await specialistStore.loadAuthorizationRequests();
  
  const authId = Number(route.query.authId);
  if (authId) {
    await viewRequestHistory(authId);
  }
});
</script>

<template>
  <div class="table-card">
    <div class="table-header">
      <h3>Authorization Requests</h3>
      <div class="toolbar">
        <span class="count">
          {{ filteredRequests.length }} Requests
        </span>
        <input
          v-model="searchText"
          class="search-box"
          placeholder="Search Authorization Id, Patient, Payer, Status..."
        />
      </div>
    </div>

    <!-- LOADING STATE -->
    <div v-if="loading" class="loading-state">
      <i class="pi pi-spin pi-spinner"></i> Loading requests...
    </div>

    <!-- TABLE -->
    <table v-else-if="filteredRequests.length" class="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Patient</th>
          <th>Payer</th>
          <th>Status</th>
          <th>Priority</th>
          <th>Estimated Amount</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="request in paginatedRequests" :key="request.authId">
          <td>{{ request.authId }}</td>
          <td>{{ request.patientName }}</td>
          <td>{{ request.payerName }}</td>
          <td>
            <span
              class="badge"
              :class="'status-' + request.status.toLowerCase().replace(/\s+/g, '-')"
            >
              {{ request.status }}
            </span>
          </td>
          <td>
            <span
              class="badge"
              :class="'priority-' + request.priority.toLowerCase()"
            >
              {{ request.priority }}
            </span>
          </td>
          <td>₹ {{ request.estimatedAmount.toLocaleString() }}</td>
          <td>
            <button
              class="view-button"
              @click="viewRequestHistory(request.authId)"
            >
              <i class="pi pi-eye"></i>
              View
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- EMPTY STATE -->
    <div v-else class="empty-state">
      No authorization requests found.
    </div>

    <!-- PAGINATION -->
    <div v-if="filteredRequests.length" class="pagination">
      <button @click="currentPage--" :disabled="currentPage === 1">
        Previous
      </button>
      <span>
        Page {{ currentPage }} of {{ totalPages }}
      </span>
      <button @click="currentPage++" :disabled="currentPage === totalPages">
        Next
      </button>
    </div>
  </div>

  <!-- DRAWER OVERLAY -->
  <div
    v-if="showHistoryModal"
    class="drawer-overlay"
    @click="closeHistoryModal"
  >
    <aside class="drawer" @click.stop>
      <!-- HEADER -->
      <div class="drawer-header">
        <h2>Authorization Details</h2>
        <button class="close-btn" @click="closeHistoryModal">
          ✕
        </button>
      </div>

      <!-- BODY -->
      <div class="drawer-body">
        <!-- AUTHORIZATION INFORMATION -->
        <div v-if="specialistStore.authorizationDetails" class="details-card">
          <h3>Authorization Information</h3>
          <div class="details-grid">
            <div class="detail-item">
              <span class="label">Auth ID</span>
              <span class="value">{{ specialistStore.authorizationDetails.authId }}</span>
            </div>

            <div class="detail-item">
              <span class="label">Patient</span>
              <span class="value">{{ specialistStore.authorizationDetails.patientName }}</span>
            </div>

            <div class="detail-item">
              <span class="label">Payer</span>
              <span class="value">{{ specialistStore.authorizationDetails.payerName }}</span>
            </div>

            <div class="detail-item">
              <span class="label">Status</span>
              <span
                class="badge"
                :class="'status-' + specialistStore.authorizationDetails.status.toLowerCase().replace(/\s+/g, '-')"
              >
                {{ specialistStore.authorizationDetails.status }}
              </span>
            </div>

            <div class="detail-item">
              <span class="label">Priority</span>
              <span
                class="badge"
                :class="'priority-' + specialistStore.authorizationDetails.priority.toLowerCase()"
              >
                {{ specialistStore.authorizationDetails.priority }}
              </span>
            </div>

            <div class="detail-item">
              <span class="label">Estimated Amount</span>
              <span class="value">₹ {{ specialistStore.authorizationDetails.estimatedAmount.toLocaleString() }}</span>
            </div>

            <div class="detail-item">
              <span class="label">Approved Amount</span>
              <span class="value">
                {{
                  specialistStore.authorizationDetails.approvedAmount !== null && specialistStore.authorizationDetails.approvedAmount !== undefined
                    ? '₹ ' + specialistStore.authorizationDetails.approvedAmount.toLocaleString()
                    : '--'
                }}
              </span>
            </div>

            <div class="detail-item">
              <span class="label">Submitted</span>
              <span class="value">
                {{ formatDate(specialistStore.authorizationDetails.submittedAt ?? null) }}
              </span>
            </div>

            <div class="detail-item">
              <span class="label">Reviewed</span>
              <span class="value">
                {{ formatDate(specialistStore.authorizationDetails.reviewedAt ?? null) }}
              </span>
            </div>

            <div class="detail-item">
              <span class="label">Expiration</span>
              <span class="value">
                {{ formatDate(specialistStore.authorizationDetails.expirationDate ?? null) }}
              </span>
            </div>
          </div>
        </div>

        <!-- SERVICES -->
        <div class="details-card">
          <h3>Services</h3>
          <div
            v-if="authorizationServices?.length"
            class="services-container"
          >
            <div
              v-for="service in authorizationServices"
              :key="service.serviceId"
              class="service-card"
            >
              <div class="service-row">
                <span class="service-label">CPT Code</span>
                <span class="cpt-badge">{{ service.cptCode }}</span>
              </div>
              <div class="service-row">
                <span class="service-label">ICD Code</span>
                <span class="icd-badge">{{ service.icdCode }}</span>
              </div>
              <div class="service-row">
                <span class="service-label">Estimated Cost</span>
                <span class="service-value">₹ {{ service.estimatedCost.toLocaleString() }}</span>
              </div>
              <div v-if="service.notes" class="service-row">
                <span class="service-label">Notes</span>
                <span class="service-value">{{ service.notes }}</span>
              </div>
            </div>
          </div>
          <div v-else class="empty-row">
            No services available.
          </div>
        </div>

        <!-- TIMELINE BUTTON -->
        <button class="timeline-btn" @click="showTimeline = !showTimeline">
          <i
            :class="
              showTimeline
                ? 'pi pi-chevron-up'
                : 'pi pi-chevron-down'
            "
          ></i>
          {{ showTimeline ? 'Hide Timeline' : 'View Timeline' }}
        </button>

        <!-- TIMELINE -->
        <div v-if="showTimeline" class="timeline-card">
          <h3>Authorization Timeline</h3>
          <div
            v-if="authorizationTimeline?.length"
            class="timeline-list"
          >
            <div
              v-for="(event, index) in authorizationTimeline"
              :key="index"
              class="timeline-item"
            >
              <div class="timeline-marker">
                <div class="timeline-dot"></div>
                <div
                  v-if="index !== authorizationTimeline.length - 1"
                  class="timeline-line"
                ></div>
              </div>
              <div class="timeline-content">
                <h4>{{ event.action }}</h4>
                <p v-if="event.remarks">{{ event.remarks }}</p>
                <span>{{ formatDate(event.createdAt) }}</span>
              </div>
            </div>
          </div>
          <div v-else class="empty-row">
            No timeline available.
          </div>
        </div>
      </div>
    </aside>
  </div>
</template>

<style scoped>
.table-card {
  background: white;
  border-radius: 16px;
  border: 1px solid #e2e8f0;
  overflow: hidden;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03);
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 24px;
  border-bottom: 1px solid #e2e8f0;
  gap: 20px;
}

.table-header h3 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
  color: #0f172a;
}

.toolbar {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-left: auto;
}

.count {
  background: #f1f5f9;
  color: #475569;
  padding: 6px 12px;
  border-radius: 999px;
  font-size: 13px;
  font-weight: 600;
  white-space: nowrap;
}

.search-box {
  width: 320px;
  padding: 8px 14px;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  font-size: 14px;
  background: #ffffff;
  color: #0f172a;
  transition: all 0.2s ease;
}

.search-box::placeholder {
  color: #94a3b8;
}

.search-box:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 4px rgba(37, 99, 235, 0.1);
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th {
  background: #f8fafc;
  color: #64748b;
  font-weight: 600;
  font-size: 13px;
  text-align: left;
  padding: 14px 16px;
  border-bottom: 1px solid #e2e8f0;
}

.table td {
  padding: 16px;
  border-bottom: 1px solid #f1f5f9;
  color: #334155;
  font-size: 14px;
}

.table tbody tr {
  transition: background-color 0.2s ease;
}

.table tbody tr:hover {
  background: #f8fafc;
}

/* Loading & Empty States */
.loading-state,
.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 48px;
  color: #64748b;
  font-size: 15px;
  gap: 8px;
}

.loading-state i {
  color: #2563eb;
  font-size: 18px;
}

/* Pagination */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 16px;
  padding: 16px 24px;
  border-top: 1px solid #e2e8f0;
  background: #ffffff;
}

.pagination button {
  padding: 8px 16px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  background: #ffffff;
  color: #334155;
  cursor: pointer;
  font-size: 14px;
  font-weight: 600;
  transition: all 0.2s ease;
}

.pagination button:hover:not(:disabled) {
  background: #f1f5f9;
  border-color: #cbd5e1;
}

.pagination button:disabled {
  background: #f8fafc;
  color: #cbd5e1;
  cursor: not-allowed;
}

.pagination span {
  font-size: 14px;
  font-weight: 500;
  color: #64748b;
}

/* Badges */
.badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 110px;
  padding: 6px 14px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
  text-align: center;
}

/* Priority Badges */
.priority-normal,
.priority-routine {
  background: #ecfdf5;
  color: #059669;
}

.priority-urgent {
  background: #fffbeb;
  color: #d97706;
}

.priority-emergency {
  background: #fef2f2;
  color: #dc2626;
}

/* Status Badges */
.status-draft {
  background: #f3f4f6;
  color: #4b5563;
}

.status-verificationfailed {
  background: #fef2f2;
  color: #dc2626;
}

.status-readyforsubmission {
  background: #f5f3ff;
  color: #7c3aed;
}

.status-submitted,
.status-resubmitted,
.status-re-submitted {
  background: #eff6ff;
  color: #2563eb;
}

.status-underreview {
  background: #fffbeb;
  color: #d97706;
}

.status-additionalinforequired {
  background: #fff7ed;
  color: #ea580c;
}

.status-approved {
  background: #ecfdf5;
  color: #059669;
}

.status-denied {
  background: #fef2f2;
  color: #dc2626;
}

.status-expired {
  background: #f3f4f6;
  color: #6b7280;
}

.status-pending {
  background: #fffbeb;
  color: #d97706;
}

/* Buttons */
.view-button {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: white;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.view-button:hover {
  background: #1d4ed8;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.2);
}

.view-button i {
  font-size: 14px;
}

/* Drawer overlay */
.drawer-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.4);
  backdrop-filter: blur(4px);
  display: flex;
  justify-content: flex-end;
  z-index: 1000;
}

.drawer {
  width: 600px;
  max-width: 100%;
  height: 100vh;
  background: #ffffff;
  display: flex;
  flex-direction: column;
  box-shadow: -8px 0 30px rgba(15, 23, 42, 0.08);
  animation: slideIn 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.drawer-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 24px;
  border-bottom: 1px solid #e2e8f0;
  background: #ffffff;
}

.drawer-header h2 {
  margin: 0;
  font-size: 22px;
  font-weight: 700;
  color: #0f172a;
}

.close-btn {
  border: none;
  background: #f1f5f9;
  font-size: 18px;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  cursor: pointer;
  color: #64748b;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.close-btn:hover {
  background: #e2e8f0;
  color: #0f172a;
}

.drawer-body {
  flex: 1;
  overflow-y: auto;
  padding: 24px;
}

/* Details Card */
.details-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
}

.details-card h3 {
  margin: 0 0 20px 0;
  font-size: 18px;
  font-weight: 600;
  color: #0f172a;
}

.details-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

@media (max-width: 640px) {
  .details-grid {
    grid-template-columns: 1fr;
  }
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.label {
  font-size: 12px;
  font-weight: 500;
  color: #64748b;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.value {
  font-size: 14px;
  font-weight: 500;
  color: #0f172a;
}

/* Services */
.services-container {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.service-card {
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  padding: 16px;
  background: #f8fafc;
  transition: border-color 0.2s ease;
}

.service-card:hover {
  border-color: #cbd5e1;
}

.service-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.service-row:last-child {
  margin-bottom: 0;
}

.service-label {
  font-size: 14px;
  font-weight: 500;
  color: #475569;
}

.service-value {
  font-size: 14px;
  font-weight: 600;
  color: #0f172a;
}

.cpt-badge {
  background: #eff6ff;
  color: #2563eb;
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}

.icd-badge {
  background: #f0fdf4;
  color: #16a34a;
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}

.empty-row {
  text-align: center;
  color: #94a3b8;
  padding: 24px;
  font-size: 14px;
}

/* Timeline */
.timeline-btn {
  width: 100%;
  margin-top: 20px;
  padding: 12px 16px;
  border: none;
  border-radius: 10px;
  background: #2563eb;
  color: white;
  font-weight: 600;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.timeline-btn:hover {
  background: #1d4ed8;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.2);
}

.timeline-card {
  margin-top: 20px;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 24px;
  background: #ffffff;
}

.timeline-card h3 {
  margin: 0 0 20px 0;
  font-size: 16px;
  font-weight: 600;
  color: #0f172a;
}

.timeline-list {
  display: flex;
  flex-direction: column;
}

.timeline-item {
  display: flex;
  gap: 16px;
  position: relative;
}

.timeline-marker {
  display: flex;
  flex-direction: column;
  align-items: center;
  flex-shrink: 0;
}

.timeline-dot {
  width: 12px;
  height: 12px;
  background: #2563eb;
  border-radius: 50%;
  border: 2px solid #ffffff;
  box-shadow: 0 0 0 2px #2563eb;
  z-index: 1;
}

.timeline-line {
  width: 2px;
  flex-grow: 1;
  background: #e2e8f0;
  margin: 6px 0;
}

.timeline-content {
  flex: 1;
  padding-bottom: 24px;
}

.timeline-content h4 {
  margin: 0;
  font-size: 14px;
  font-weight: 600;
  color: #0f172a;
}

.timeline-content p {
  margin: 6px 0;
  font-size: 13px;
  color: #475569;
  line-height: 1.5;
}

.timeline-content span {
  font-size: 12px;
  color: #94a3b8;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}
</style>
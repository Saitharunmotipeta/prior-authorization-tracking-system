<script setup lang="ts">
import { onMounted } from "vue";
import { usePayerStore } from "../../stores/payer.store";
import { storeToRefs } from "pinia";

const payerStore = usePayerStore();

const { auditHistory, loading } = storeToRefs(payerStore);

const formatAction = (action: string) => {
  if (action === "RequestedMoreInfo") return "Requested More Info";
  return action;
};

const formatDate = (date: string) => {
  return new Date(date).toLocaleString();
};


onMounted(() => {
  payerStore.loadAuditHistory(); 
});
</script><template>

  <div>

    <div
      v-for="audit in auditHistory"
      :key="audit.auditId"
      class="audit-card"
    >

      <!-- ✅ CONTEXT LINE -->
     <div class="context">
  <span>AuthId: {{ audit.authId }}</span>
  <span>PatientId: {{ audit.patientId }}</span>
  <span>Facility: {{ audit.facilityName }}</span>
</div>

      <!-- ✅ STATUS + DATE -->
      <div class="audit-header">
        <span
          class="badge"
          :class="audit.actionType"
        >
          {{ formatAction(audit.actionType) }}
        </span>

        <span class="date">
          {{ formatDate(audit.createdAt) }}
        </span>
      </div>

    </div>

  </div>

</template>

<style>
.audit-item {
  background: #f8fafc;
  border-left: 4px solid #3b82f6;
  padding: 12px 16px;
  margin-bottom: 12px;
  border-radius: 6px;
}

.audit-header {
  display: flex;
  justify-content: space-between;
}

.action {
  font-weight: 600;
}

.date {
  font-size: 12px;
  color: #6b7280;
}

.remarks {
  margin-top: 6px;
}

.new-value {
  font-size: 13px;
  color: #2563eb;
}
.audit-card {
  background: white;
  border-radius: 10px;
  padding: 16px;
  margin-bottom: 14px;
  border-left: 5px solid #64748b;
  box-shadow: 0 2px 6px rgba(0,0,0,0.05);
}

.context {
  font-size: 13px;
  color: #475569;

  margin-bottom: 10px;

  display: flex;
  gap: 24px;

  font-weight: 500;
}

.audit-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.badge {
  font-weight: 600;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 14px;
}

/* ✅ Colors */
.badge.Approved {
  background: #dcfce7;
  color: #16a34a;
}

.badge.Denied {
  background: #fee2e2;
  color: #dc2626;
}

.badge.RequestedMoreInfo {
  background: #fef3c7;
  color: #d97706;
}

.date {
  font-size: 12px;
  color: #6b7280;
}
.context span {
  background: #f1f5f9;
  padding: 4px 8px;
  border-radius: 6px;
}
</style>
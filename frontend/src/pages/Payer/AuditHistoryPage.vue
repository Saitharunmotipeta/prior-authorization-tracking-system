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
  <div
  v-for="audit in auditHistory"
  :key="audit.auditId"
  class="audit-card"
>

  <!-- ✅ HEADER -->
  <div class="audit-header">
    <span class="badge" :class="audit.actionType">
      {{ formatAction(audit.actionType) }}
    </span>

    <span class="date">
      {{ formatDate(audit.createdAt) }}
    </span>
  </div>

  <!-- ✅ IMPORTANT: SHOW REASON -->
  <div class="remarks">
    {{ audit.remarks || "No additional details provided" }}
  </div>

  <!-- ✅ SHOW ONLY VALID CHANGE -->
  <div
    v-if="
      audit.oldValue &&
      audit.newValue &&
      audit.oldValue !== audit.newValue
    "
    class="change"
  >
    {{ audit.oldValue }} → {{ audit.newValue }}
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

.audit-top {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
}

.badge {
  font-weight: 600;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 14px;
}

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

.remarks {
  margin: 8px 0;
  font-size: 14px;
  color: #374151;
}

.change {
  font-size: 13px;
  color: #2563eb;
}
</style>

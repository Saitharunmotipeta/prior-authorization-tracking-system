export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface DashboardMetrics {
  totalEncounters: number;
  totalAuthorizationRequests: number;
  approvedRequests: number;
  deniedRequests: number;
  pendingRequests: number;
  expiredRequests: number;
  approvalRate: number;
  denialRate: number;
  approvedRevenue: number;
  totalReminders: number;
  successfulReminders: number;
  reminderSuccessRate: number;
}

export interface PayerPerformance {
  payerName: string;
  totalRequests: number;
  approvedRequests: number;
  deniedRequests: number;
  pendingRequests: number;
  approvalRate: number;
}

export interface RevenueAtRisk {
  deniedRequests: number;
  revenueAtRisk: number;
}

export interface SlowPayer {
  payerName: string;
  totalReviewedRequests: number;
  averageResponseDays: number;
}

export interface FacilityComparison {
  rank: number;
  facilityName: string;
  totalRequests: number;
  approvedRequests: number;
  deniedRequests: number;
  approvalRate: number;
  approvedRevenue: number;
  averageResponseDays: number;
}

export interface TopPerformingPayer {
  rank: number;
  payerName: string;
  totalRequests: number;
  approvedRequests: number;
  approvalRate: number;
}

export interface PoorPerformingPayer {
  rank: number;
  payerName: string;
  totalRequests: number;
  deniedRequests: number;
  denialRate: number;
}

export interface DelayTrend {
  payerName: string;
  zeroToTwoDays: number;
  threeToFiveDays: number;
  sixToTenDays: number;
  moreThanTenDays: number;
}
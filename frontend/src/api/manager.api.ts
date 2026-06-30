import { managerApiClient } from "./axios";

import type {
  ApiResponse,
  DashboardMetrics,
  PayerPerformance,
  SlowPayer,
  RevenueAtRisk,
  FacilityComparison,
  TopPerformingPayer,
  PoorPerformingPayer,
  DelayTrend
} from "../types/dashboard.interface";

export const getDashboard = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<DashboardMetrics>>(
      "/api/dashboard",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getPayerPerformance = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<PayerPerformance[]>>(
      "/api/analytics/payer-performance",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getSlowPayers = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<SlowPayer[]>>(
      "/api/analytics/slowest-payers",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getRevenueAtRisk = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<RevenueAtRisk>>(
      "/api/analytics/revenue-at-risk",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getFacilityComparison = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<FacilityComparison[]>>(
      "/api/analytics/facility-comparison",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getTopPerformingPayers = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<TopPerformingPayer[]>>(
      "/api/analytics/top-performing-payers",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getPoorPerformingPayers = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<PoorPerformingPayer[]>>(
      "/api/analytics/poor-performing-payers",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const getDelayTrends = async (facilityId?: number) => {
  const response =
    await managerApiClient.get<ApiResponse<DelayTrend[]>>(
      "/api/analytics/delay-trends",
      {
        params: facilityId ? { facilityId } : {}
      }
    );

  return response.data;
};

export const generateExecutiveReport =
async () => {

  const response =
    await managerApiClient.post(
      "/api/reports/executive"
    );

  return response.data;

};
import { defineStore } from "pinia";

import {
  getErrorMessage
} from "../utils/error-handler";

import type {
  DashboardMetrics,
  PayerPerformance,
  SlowPayer,
  RevenueAtRisk,
  FacilityComparison,
  TopPerformingPayer,
  PoorPerformingPayer,
  DelayTrend,
  ExecutiveReport
} from "../types/dashboard.interface";

import {
  getDashboard,
  getPayerPerformance,
  getSlowPayers,
  getRevenueAtRisk,
  getFacilityComparison,
  getTopPerformingPayers,
  getPoorPerformingPayers,
  getDelayTrends,
  generateExecutiveReport
} from "../api/manager.api";

interface ManagerDashboardState {
  selectedFacilityId: number | null;

  loading: boolean;

  error: string | null;

  dashboard: DashboardMetrics | null;

  payerPerformance: PayerPerformance[];

  slowPayers: SlowPayer[];

  revenueAtRisk: RevenueAtRisk | null;

  facilityComparison: FacilityComparison[];

  topPerformingPayers: TopPerformingPayer[];

  poorPerformingPayers: PoorPerformingPayer[];

  delayTrends: DelayTrend[];

  executiveReport: ExecutiveReport | null;
}

export const useManagerDashboardStore =
  defineStore(
    "managerDashboard",
    {
      persist: true,

      state:
        (): ManagerDashboardState => ({
          selectedFacilityId:
            null,

          loading: false,

          error: null,

          dashboard: null,

          payerPerformance: [],

          slowPayers: [],

          revenueAtRisk: null,

          facilityComparison: [],

          topPerformingPayers: [],

          poorPerformingPayers: [],

          delayTrends: [],

          executiveReport: null
        }),

      actions: {

        async loadExecutiveReport() {

  try {

    const response =
      await generateExecutiveReport();

    this.executiveReport = {

      generatedAt:
        response.data.generatedAt,

      report:
        JSON.parse(
          response.data.report
        )

    };

  }

  catch (error) {

    console.error(error);

    this.error =
      getErrorMessage(error);

  }

},
        async loadAllDashboardData(
          facilityId?: number
        ) {
          try {
            this.loading = true;

            this.error = null;

            this.selectedFacilityId =
              facilityId ?? null;

            const [
              dashboardResponse,

              payerPerformanceResponse,

              slowPayersResponse,

              revenueAtRiskResponse,

              facilityComparisonResponse,

              topPerformingPayersResponse,

              poorPerformingPayersResponse,

              delayTrendsResponse
            ] = await Promise.all([
              getDashboard(
                facilityId
              ),

              getPayerPerformance(
                facilityId
              ),

              getSlowPayers(
                facilityId
              ),

              getRevenueAtRisk(
                facilityId
              ),

              getFacilityComparison(
                facilityId
              ),

              getTopPerformingPayers(
                facilityId
              ),

              getPoorPerformingPayers(
                facilityId
              ),

              getDelayTrends(
                facilityId
              )
            ]);

            this.dashboard =
              dashboardResponse.data;

            this.payerPerformance =
              payerPerformanceResponse.data;

            this.slowPayers =
              slowPayersResponse.data;

            this.revenueAtRisk =
              revenueAtRiskResponse.data;

            this.facilityComparison =
              facilityComparisonResponse.data;

            this.topPerformingPayers =
              topPerformingPayersResponse.data;

            this.poorPerformingPayers =
              poorPerformingPayersResponse.data;

            this.delayTrends =
              delayTrendsResponse.data;
          }
          catch (error) {
            console.error(error);

            this.error =
              getErrorMessage(error);
          }
          finally {
            this.loading = false;
          }
        },

        async changeFacility(
          facilityId?: number
        ) {
          await this
            .loadAllDashboardData(
              facilityId
            );
        },

        clearError() {
          this.error = null;
        },

        resetDashboard() {
          this.selectedFacilityId =
            null;

          this.dashboard =
            null;

          this.payerPerformance =
            [];

          this.slowPayers =
            [];

          this.revenueAtRisk =
            null;

          this.facilityComparison =
            [];

          this.topPerformingPayers =
            [];

          this.poorPerformingPayers =
            [];

          this.delayTrends =
            [];

          this.executiveReport = null;

          this.error = null;
        }
      },

      getters: {
        approvalRate:
          (state) =>
            state.dashboard
              ?.approvalRate ?? 0,

        denialRate:
          (state) =>
            state.dashboard
              ?.denialRate ?? 0,

        approvedRevenue:
          (state) =>
            state.dashboard
              ?.approvedRevenue ?? 0,

        revenueAtRiskAmount:
          (state) =>
            state.revenueAtRisk
              ?.revenueAtRisk ?? 0,

        totalRequests:
          (state) =>
            state.dashboard
              ?.totalAuthorizationRequests ?? 0,

        pendingRequests:
          (state) =>
            state.dashboard
              ?.pendingRequests ?? 0
      }
    }
  );
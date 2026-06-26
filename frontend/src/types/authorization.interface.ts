export interface CreateAuthorizationRequest {
  encounterId: number;

  payerId: number;

  priority: number;

  services: [];
}

export interface CreateAuthorizationResponse {
  success: boolean;

  message: string;

  data: number;
}
export interface AuthorizationTimeline {
  action: string;
  remarks: string;
  createdAt: string;
}
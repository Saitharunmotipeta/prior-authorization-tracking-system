export interface AddAuthorizationServiceRequest {
  cptCode: string;

  icdCode: string;

  notes: string;
}

export interface AddAuthorizationServiceListRequest {
    services: AddAuthorizationServiceRequest[];
}

export interface AuthorizationService {
  serviceId: number;
  cptCode: string;
  icdCode: string;
  estimatedCost: number;
  notes: string;
}
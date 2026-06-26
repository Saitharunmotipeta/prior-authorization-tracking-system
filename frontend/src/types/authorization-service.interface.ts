export interface AddAuthorizationServiceRequest {
  cptCode: string;

  icdCode: string;

  notes: string;
}

export interface AddAuthorizationServiceListRequest {
    services: AddAuthorizationServiceRequest[];
}
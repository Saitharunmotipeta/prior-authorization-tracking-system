# Final Solution Structure

```text
PriorAuthorizationSystem.sln
│
├── Specialist.API
├── Payer.API
├── Manager.API
│
├── PriorAuthorization.Data
│
└── PriorAuthorization.Shared
```

---

# PriorAuthorization.Data

Shared by all APIs.

```text
PriorAuthorization.Data
│
├── Context
│   └── ApplicationDbContext.cs
│
├── Entities
│   ├── Facility.cs
│   ├── Department.cs
│   ├── Patient.cs
│   ├── Payer.cs
│   ├── Policy.cs
│   ├── Encounter.cs
│   ├── AuthorizationRequest.cs
│   ├── AuthorizationService.cs
│   ├── Reminder.cs
│   └── AuditHistory.cs
│
├── Configurations
│   ├── FacilityConfiguration.cs
│   ├── DepartmentConfiguration.cs
│   ├── PatientConfiguration.cs
│   ├── PayerConfiguration.cs
│   ├── PolicyConfiguration.cs
│   ├── EncounterConfiguration.cs
│   ├── AuthorizationRequestConfiguration.cs
│   ├── AuthorizationServiceConfiguration.cs
│   ├── ReminderConfiguration.cs
│   └── AuditHistoryConfiguration.cs
│
├── Migrations
│
└── SeedData
```

---

# PriorAuthorization.Shared

```text
PriorAuthorization.Shared
│
├── Enums
│   ├── ConditionType.cs
│   ├── RequestStatus.cs
│   ├── VerificationStatus.cs
│   ├── Priority.cs
│   ├── ReminderCategory.cs
│   ├── ReminderStatus.cs
│   ├── AuditActionType.cs
│   └── UserRole.cs
│
├── Helpers
│
├── Constants
│
├── Responses
│
└── Common
```

---

# Specialist.API

Runs on:

```text
https://localhost:5001
```

Responsibilities:

```text
Patient
Eligibility
Encounter
Authorization Creation
Services
```

Directory:

```text
Specialist.API
│
├── Controllers
│   ├── PatientController.cs
│   ├── EligibilityController.cs
│   ├── EncounterController.cs
│   ├── AuthorizationRequestController.cs
│   └── AuthorizationServiceController.cs
│
├── Services
│   │
│   ├── Interfaces
│   │   ├── IPatientService.cs
│   │   ├── IEligibilityService.cs
│   │   ├── IEncounterService.cs
│   │   ├── IAuthorizationRequestService.cs
│   │   └── IAuthorizationServiceService.cs
│   │
│   └── Implementations
│       ├── PatientService.cs
│       ├── EligibilityService.cs
│       ├── EncounterService.cs
│       ├── AuthorizationRequestService.cs
│       └── AuthorizationServiceService.cs
│
├── DTOs
│   ├── PatientDto.cs
│   ├── EligibilityResponseDto.cs
│   ├── CreateEncounterDto.cs
│   ├── EncounterDetailsDto.cs
│   ├── CreateAuthorizationRequestDto.cs
│   └── CreateAuthorizationServiceDto.cs
│
├── Program.cs
│
└── appsettings.json
```

---

# Payer.API

Runs on:

```text
https://localhost:5002
```

Responsibilities:

```text
Review Requests
Approve
Deny
Request More Info
Reminders
Notifications
Peer Review Calls
```

Directory:

```text
Payer.API
│
├── Controllers
│   ├── ReviewController.cs
│   └── ReminderController.cs
│
├── Services
│   │
│   ├── Interfaces
│   │   ├── IReviewService.cs
│   │   └── IReminderService.cs
│   │
│   └── Implementations
│       ├── ReviewService.cs
│       └── ReminderService.cs
│
├── DTOs
│   ├── ReviewRequestDto.cs
│   ├── ReviewResponseDto.cs
│   ├── CreateReminderDto.cs
│   └── ReminderDto.cs
│
├── Program.cs
│
└── appsettings.json
```

---

# Manager.API

Runs on:

```text
https://localhost:5003
```

Responsibilities:

```text
Dashboard
Audit
Analytics
Readiness
Risk
Insights
```

Directory:

```text
Manager.API
│
├── Controllers
│   ├── DashboardController.cs
│   ├── AuditController.cs
│   └── AnalyticsController.cs
│
├── Services
│   │
│   ├── Interfaces
│   │   ├── IDashboardService.cs
│   │   ├── IAuditService.cs
│   │   └── IAnalyticsService.cs
│   │
│   └── Implementations
│       ├── DashboardService.cs
│       ├── AuditService.cs
│       └── AnalyticsService.cs
│
├── DTOs
│   ├── DashboardDto.cs
│   ├── ProviderPerformanceDto.cs
│   ├── DenialAnalyticsDto.cs
│   ├── AuditHistoryDto.cs
│   ├── ReadinessScoreDto.cs
│   ├── RiskIndicatorDto.cs
│   └── InsightDto.cs
│
├── Program.cs
│
└── appsettings.json
```

PriorAuthorization.Manager.API
│
├── Controllers
│   ├── DashboardController.cs
│   └── AnalyticsController.cs
│
├── DTOs
│   │
│   ├── Dashboard
│   │   ├── DashboardFilterDto.cs
│   │   └── DashboardResponseDto.cs
│   │
│   ├── Analytics
│   │   ├── PayerPerformanceDto.cs
│   │   ├── TopPerformingPayerDto.cs
│   │   ├── PoorPerformingPayerDto.cs
│   │   ├── SlowPayerDto.cs
│   │   ├── RevenueAtRiskDto.cs
│   │   ├── DelayTrendDto.cs
│   │   └── FacilityComparisonDto.cs
│   │
│   └── Common
│       ├── ApiResponseDto.cs
│       └── PaginationDto.cs
│
├── Services
│   │
│   ├── Interfaces
│   │   ├── IDashboardService.cs
│   │   └── IAnalyticsService.cs
│   │
│   └── Implementations
│       ├── DashboardService.cs
│       └── AnalyticsService.cs
│
├── Validators
│   ├── DashboardValidator.cs
│   └── AnalyticsValidator.cs
│
├── Extensions
│   └── ServiceCollectionExtensions.cs
│
├── Constants
│
├── Helpers
│
├── Program.cs
│
├── appsettings.json
│
└── appsettings.Development.json
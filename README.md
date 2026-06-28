# 🏥 Prior Authorization Tracking & Payer Management System

A Modular Monolithic Healthcare Prior Authorization platform inspired by enterprise healthcare workflows. The application centralizes authorization requests, payer management, eligibility verification, approval tracking, reminders, audit history, and executive analytics into a unified system.

---

# 📌 Problem Statement

Healthcare providers process thousands of prior authorization requests every day. Manual authorization workflows introduce:

- Delayed approvals
- Revenue leakage
- Poor payer visibility
- Lack of centralized tracking
- High administrative workload
- Limited operational analytics

This project aims to streamline the complete authorization lifecycle while providing actionable insights through executive dashboards and payer analytics.

---

# 🎯 Solution

The system digitizes the complete authorization workflow by providing three dedicated portals:

- 👨‍⚕️ Specialist Portal
- 🏢 Payer Portal
- 📊 Manager Portal

All three applications communicate through ASP.NET Core Web APIs backed by a centralized SQL Server database.

---

# 🏗 Architecture

Modular Monolithic Architecture

```
                    Vue Frontend

       Specialist     Manager              Payer
             │            │                  │
             └────── ASP.NET Core APIs ──────┘
                          │
                    Shared Library
                          │
                 Entity Framework Core
                          │
                    Azure SQL Database
```

---

# 🚀 Key Features

## Specialist Portal

- Patient Eligibility Verification
- Encounter Management
- Authorization Request Creation
- CPT & ICD Management
- Authorization Timeline
- Document Verification
- Reminder Management

---

## Payer Portal

- Authorization Review
- Approval / Denial Workflow
- Additional Information Requests
- Authorization Details
- Reminder Tracking
- Audit History

---

## Manager Portal

Executive dashboard with operational insights including:

- Total Facilities
- Departments
- Patients
- Encounters
- Authorization Requests
- Pending Requests
- Approval Rate
- Denial Rate
- Revenue at Risk
- Reminder Success Rate

Additional analytics include:

- Facility Comparison
- Payer Performance
- Slowest Responding Payers
- Delay Trends
- Top Performing Payers
- Poor Performing Payers

---

# 📈 Executive Analytics

The application provides near real-time analytics to monitor operational efficiency.

Metrics include:

- Approval Percentage
- Denqueued Requests
- Pending Authorizations
- Revenue at Risk
- Average Response Time
- Reminder Success Rate
- Facility Performance
- Payer Performance

---

# 🗄 Synthetic Test Data

To simulate enterprise-scale healthcare operations, synthetic datasets were generated using **Bogus**.

Dataset Size:

- ~500 Patients
- ~2,000 Encounters
- Multiple Facilities
- Multiple Departments
- Authorization Requests
- Authorization Services
- Policies
- Reminders
- Audit History

These datasets were used to validate:

- Dashboard Metrics
- Analytics Queries
- Facility Comparisons
- Payer Performance
- Revenue Calculations
- Delay Trends

---

# ⚡ Performance & Diagnostics

The application includes several diagnostics to improve observability.

### Stopwatch Diagnostics

Every major service operation is timed using:

- Stopwatch
- Execution Duration Logging

This helps identify slow-running operations.

### Structured Logging

Microsoft ILogger is used throughout the APIs for:

- Request Logging
- Warning Logs
- Validation Errors
- Service Execution
- Exception Tracking

---

# 🐳 Docker Support

The entire application is containerized.

Containers include:

- Frontend
- Specialist API
- Manager API
- Payer API

Docker Compose orchestrates all services using a single command.

```

docker compose up --build

```

---

# ⚙ CI/CD Pipeline

GitHub Actions automates the complete validation pipeline.

Workflow includes:

```
Backend Build
        │
Frontend Build
        │
Docker Image Build
        │
Docker Compose
        │
Health Checks
        │
Smoke Tests
        │
Frontend Verification
        │
Cleanup
```

Smoke Tests verify:

- Specialist API
- Manager API
- Payer API

Health checks ensure:

- API Availability
- Database Connectivity

Code Quality:

- CodeQL Security Analysis
- Automated Builds
- Docker Validation

---

# 🛠 Technology Stack

## Frontend

- Vue 3
- TypeScript
- Pinia
- Axios
- Vue Router

---

## Backend

- ASP.NET Core 9
- Entity Framework Core
- SQL Server
- REST APIs

---

## Database

- Azure SQL Database
- Entity Framework Core

---

## DevOps

- Docker
- Docker Compose
- GitHub Actions
- CodeQL

---

## Logging & Diagnostics

- ILogger
- Stopwatch
- Health Checks

---

# 📁 Project Structure

```
backend/
    Specialist.API
    Manager.API
    Payer.API
    Shared

frontend/

docker-compose.yml

.github/workflows
```

---

# 🔒 Security

- Centralized Validation
- Exception Handling
- Health Monitoring
- CodeQL Security Scanning

---

# 📊 Testing

Validation includes:

- Health Checks
- API Smoke Tests
- Dashboard Analytics Validation
- Docker Validation

---

# 🚀 Future Enhancements

- JWT Authentication
- Role-Based Authorization
- Redis Caching
- SignalR Notifications
- Email Automation
- Kubernetes Deployment
- Azure DevOps Release Pipeline
- AI-powered Authorization Prediction

---

# 👨‍💻 Author

Sai Tharun Motipeta

B.Tech Information Technology

Full Stack Developer | ASP.NET Core | Vue.js | AI Integration

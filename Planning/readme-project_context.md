# PRIOR AUTHORIZATION TRACKING AND PAYER MANAGEMENT SYSTEM

## COMPLETE PROJECT CONTEXT DOCUMENT

---

# 1. PROJECT OVERVIEW

## Business Problem

Healthcare facilities spend significant administrative effort managing prior authorization requests with insurance payers.

Current challenges include:

* No standardized authorization tracking process
* Delayed payer responses
* High denial rates
* Missing documentation
* Lack of follow-up tracking
* No centralized visibility across facilities
* No performance analytics for payers

The objective of this system is to centralize authorization management, payer communication, status tracking, reminders, audit tracking, and analytics.

---

# 2. SYSTEM ACTORS

## Authorization Specialist

Hospital staff responsible for:

* Verifying patient documents
* Checking insurance eligibility
* Creating encounters
* Creating authorization requests
* Following up with payers
* Scheduling peer-to-peer reviews

---

## Payer Representative

Insurance provider representative responsible for:

* Reviewing requests
* Approving requests
* Denying requests
* Requesting additional information
* Managing peer-to-peer reviews

---

## Clinical Manager

Management user responsible for:

* Monitoring operational metrics
* Reviewing payer performance
* Tracking denials
* Reviewing audit history
* Viewing authorization analytics

---

# 3. ARCHITECTURE

## Architecture Style

Modular Monolith

Combined With:

* Layered Architecture
* Repository Pattern
* Entity Framework Core
* SQL Server

---

## Solution Structure

PriorAuthorizationSystem.sln

Projects:

* Specialist.API
* Payer.API
* Manager.API
* PriorAuthorization.Data
* PriorAuthorization.Shared

---

## API Hosting

Specialist.API

Responsibilities:

* Eligibility Verification
* Encounter Creation
* Authorization Creation
* Service Creation

Port:

5001

---

Payer.API

Responsibilities:

* Authorization Review
* Approval
* Denial
* Request More Information
* Reminder Management

Port:

5002

---

Manager.API

Responsibilities:

* Dashboard
* Analytics
* Audit
* Reporting

Port:

5003

---

## Shared Projects

PriorAuthorization.Data

Contains:

* DbContext
* Entities
* Configurations
* Migrations
* Seed Data

PriorAuthorization.Shared

Contains:

* Enums
* Helpers
* Constants
* Shared DTOs
* Common Responses

---

# 4. DATABASE DESIGN

## Tables

### Facility

Stores healthcare facility information.

### Department

Stores departments belonging to facilities.

### Patient

Stores patient demographic information.

Primary Key:

PatientId (GUID)

---

### Payer

Stores insurance provider information.

Includes:

* Contact Number
* Payer Email
* Normal TAT
* Urgent TAT

---

### Policy

Stores insurance coverage information.

Used for eligibility validation.

Eligibility Rule:

CurrentDate >= PolicyStartDate

AND

CurrentDate <= PolicyExpiryDate

AND

Policy Is Active

---

### CPTCode

Reference table.

Stores procedure codes.

---

### ICDCode

Reference table.

Stores diagnosis codes.

---

### Encounter

Represents hospital-side workflow.

Stores:

* Patient
* Facility
* Department
* Verification Status
* Condition Type
* Current Workflow Status

Important:

Encounter may exist even when authorization request does not exist.

Reason:

Verification may fail before submission.

---

### AuthorizationRequest

Represents payer-side workflow.

Stores:

* Payer
* Priority
* Estimated Amount
* Approved Amount
* Review Information

One Encounter can have only one AuthorizationRequest.

Relationship:

Encounter (1) -> AuthorizationRequest (1)

---

### AuthorizationService

Stores:

* CPT Code
* ICD Code
* Estimated Cost

Many services may belong to one authorization request.

---

### Reminder

Used for:

* Follow-up reminders
* Escalations
* Peer-to-peer review requests
* Notification tracking

Categories:

1 = Follow Up

2 = Escalation

3 = Peer Review

4 = Expiration Reminder

---

### AuditHistory

Stores complete activity history.

Tracks:

* Create
* Update
* Status Change
* Review Actions
* Reminder Actions

AuditHistory stores event history.

Encounter stores current state.

---

# 5. DATABASE RELATIONSHIPS

Facility (1) -> Department (M)

Facility (1) -> Encounter (M)

Department (1) -> Encounter (M)

Patient (1) -> Encounter (M)

Patient (1) -> Policy (M)

Payer (1) -> Policy (M)

Encounter (1) -> AuthorizationRequest (1)

Payer (1) -> AuthorizationRequest (M)

AuthorizationRequest (1) -> AuthorizationService (M)

AuthorizationRequest (1) -> Reminder (M)

Payer (1) -> Reminder (M)

Encounter (1) -> AuditHistory (M)

AuthorizationRequest (1) -> AuditHistory (M)

CPTCode (1) -> AuthorizationService (M)

ICDCode (1) -> AuthorizationService (M)

---

# 6. WORKFLOW

## Happy Path

Patient Visits Hospital

↓

Eligibility Verified

↓

Encounter Created

↓

Documents Verified

↓

Authorization Request Created

↓

Authorization Submitted

↓

Payer Reviews

↓

Approved

↓

Approved Amount Updated

↓

Dashboard Updated

---

## Verification Failure

Patient Visits Hospital

↓

Encounter Created

↓

Documents Missing

↓

Verification Failed

↓

Authorization Request Not Created

↓

Encounter Remains Open

↓

Patient Provides Missing Documents

↓

Authorization Request Created Later

---

## Request More Information

Authorization Submitted

↓

Payer Reviews

↓

Additional Information Required

↓

Request More Information

↓

Specialist Updates Documents

↓

Authorization Resubmitted

↓

Approved

---

## Delayed Response

Authorization Submitted

↓

No Response

↓

Reminder Created

↓

Follow-up Reminder Created

↓

Escalation Reminder Created

↓

Payer Responds

OR

Peer Review Triggered

---

## Peer Review Flow

Authorization Submitted

↓

No Response / Critical Condition

↓

Peer Review Reminder Created

↓

Call Scheduled

↓

Call Completed

↓

Outcome Recorded

↓

Approved OR Denied

---

# 7. AUDIT STRATEGY

Every mutation creates an audit record.

Examples:

Encounter Created

Authorization Created

Status Changed

Approved

Denied

Reminder Created

Peer Review Requested

Reminder Completed

Audit History acts as complete chain-of-events tracking.

---

# 8. API ENDPOINTS

## Specialist API

GET /patients/{id}

GET /policies/eligibility/{patientId}

POST /encounters

GET /encounters

GET /encounters/{id}

POST /authorization-requests

POST /authorization-services

---

## Payer API

GET /payers

PATCH /authorization-requests/{authId}/review

POST /reminders

GET /reminders

PATCH /reminders/{id}/status

---

## Manager API

GET /facilities

GET /departments

GET /cpt-codes

GET /icd-codes

GET /audit-history/{encounterId}

GET /dashboard

GET /analytics/provider-performance

GET /analytics/denial-reasons

GET /encounters/{encounterId}/readiness-score

GET /encounters/{encounterId}/risk-indicator

GET /encounters/{encounterId}/insights

---

# 9. INDEXING STRATEGY

## Clustered Indexes

FacilityId

DepartmentId

PatientId

PayerId

PolicyId

EncounterId

AuthId

ServiceId

ReminderId

AuditId

---

## Non-Clustered Indexes

Department(FacilityId)

Patient(MRNNumber)

Encounter(PatientId)

Encounter(FacilityId)

Encounter(DepartmentId)

Encounter(RequestStatus)

Encounter(RequestStatus, ConditionType)

AuthorizationRequest(EncounterId)

AuthorizationRequest(PayerId)

AuthorizationRequest(Status)

AuthorizationRequest(ExpirationDate, Status)

AuthorizationService(AuthId)

Reminder(AuthId)

Reminder(PayerId)

AuditHistory(EncounterId)

AuditHistory(AuthId)

AuditHistory(EncounterId, CreatedAt)

---

# 10. STORED PROCEDURES

## sp_GetEncounterDetails

Returns:

* Encounter
* Authorization
* Services
* Reminders
* Audit History

---

## sp_GetAuditTimeline

Returns:

Complete activity history.

---

## sp_GetDashboardMetrics

Returns:

* Approval Rate
* Denial Rate
* Pending Count
* Expired Count
* Average TAT
* Provider Performance
* Top Denial Reasons
* Critical Queue

---

# 11. RESILIENCE STRATEGY

Using Polly.

## Retry Pattern

Applied To:

* Database Operations
* Email Operations

Configuration:

3 Retries

---

## Circuit Breaker

Applied To:

* Email Service

Purpose:

Prevent repeated failures.

---

## Timeout Pattern

Applied To:

* Dashboard Queries
* Analytics Queries

Maximum Timeout:

5 Seconds

---

## Fallback Pattern

Example:

Email Failure

↓

Create Reminder

↓

Log Audit

↓

Continue Processing

---

# 12. READINESS ENGINE

Purpose:

Measure submission readiness.

Example:

5 Documents Required

5 Verified

Score = 100%

---

Missing Documents Reduce Score.

Used by Manager Dashboard.

---

# 13. RISK ENGINE

Purpose:

Identify likely delays or denials.

Examples:

Urgent + Missing Scan

=

High Risk

---

Missing Doctor Notes

=

Medium Risk

---

Fully Verified

=

Low Risk

---

# 14. FUTURE AI INTEGRATION

Potential Features:

* Approval Probability Prediction
* Denial Prediction
* Missing Documentation Recommendation
* Authorization Insights Generator
* LLM-Based Authorization Assistant

Current MVP uses rule-based intelligence.

---

# 15. SCALABILITY STRATEGY

Phase 1

Up To:

100K Patients

400K Authorization Requests

1M Audit Records

Current Architecture Sufficient.

---

Phase 2

Add:

* Redis Cache
* Background Jobs
* Read Replicas

---

Phase 3

Add:

* Database Partitioning
* CQRS
* Analytics Database

---

# 16. BUILD ORDER

1. Solution Structure

2. Enums

3. Entities

4. Configurations

5. DbContext

6. Migration

7. Seed Data

8. Repositories

9. Services

10. Resilience

11. Specialist API

12. Payer API

13. Audit Logic

14. Manager API

15. Readiness Engine

16. Risk Engine

17. Testing

18. Documentation

# 17. LOGGING STRATEGY

## Purpose

Logging is implemented to provide:

* Application Monitoring
* Debugging Support
* Error Tracking
* Performance Analysis
* Audit Validation
* Operational Visibility

Logging is different from AuditHistory.

---

## Audit History vs Logging

### Audit History

Tracks business events.

Examples:

```text
Encounter Created

Authorization Submitted

Authorization Approved

Reminder Created

Peer Review Requested
```

Stored in:

```text
AuditHistory Table
```

Purpose:

```text
Business Tracking
```

---

### Logging

Tracks system events.

Examples:

```text
API Request Started

Database Query Failed

Email Service Timeout

Unhandled Exception

Stored Procedure Execution Time
```

Stored in:

```text
Log Files

Console

Future Centralized Logging
```

Purpose:

```text
Technical Monitoring
```

---

# Logging Architecture

All APIs will use:

```text
ILogger<T>
```

Built-in ASP.NET Core Logging.

Future Upgrade:

```text
Serilog
```

---

# Logging Levels

## Information

Normal business operations.

Examples:

```text
Encounter Created

Authorization Submitted

Reminder Scheduled

Dashboard Loaded
```

---

## Warning

Unexpected but recoverable situations.

Examples:

```text
Policy Expiring Soon

Authorization Delayed

Reminder Overdue

Readiness Score Below Threshold
```

---

## Error

Operation failed.

Examples:

```text
Database Connection Failure

Stored Procedure Failure

Email Sending Failure

Analytics Query Failure
```

---

## Critical

System-wide failure.

Examples:

```text
SQL Server Unavailable

Application Startup Failure

Configuration Corruption
```

---

# Logging Middleware

Global Middleware:

```text
RequestLoggingMiddleware
```

Captures:

```text
Request Path

HTTP Method

Response Status Code

Execution Time

Correlation Id
```

Example:

```text
POST /encounters

Status Code: 201

Duration: 120 ms
```

---

# Correlation Id Strategy

Each request gets:

```text
CorrelationId
```

Example:

```text
CORR-123456
```

Used to trace requests across:

```text
Controller

Service

Repository
```

Flow:

```text
Request
↓
Controller
↓
Service
↓
Repository
↓
Database
```

All logs contain same CorrelationId.

---

# Stopwatch Performance Monitoring

Purpose:

Measure execution time of critical operations.

Used For:

```text
Dashboard Queries

Analytics Queries

Stored Procedures

Database Operations
```

Example:

```csharp
var stopwatch = Stopwatch.StartNew();

/* Execute Query */

stopwatch.Stop();

_logger.LogInformation(
    "Dashboard query completed in {Duration} ms",
    stopwatch.ElapsedMilliseconds);
```

---

# Performance Thresholds

## Database Query

Expected:

```text
< 200 ms
```

Warning:

```text
> 500 ms
```

---

## Dashboard Query

Expected:

```text
< 1000 ms
```

Warning:

```text
> 3000 ms
```

---

## Analytics Query

Expected:

```text
< 2000 ms
```

Warning:

```text
> 5000 ms
```

---

# Repository Logging

Log:

```text
Repository Started

Repository Completed

Repository Failed
```

Example:

```text
GetEncounterById Started

GetEncounterById Completed

Duration: 45 ms
```

---

# Service Logging

Log business actions.

Examples:

```text
Eligibility Validation Started

Eligibility Validation Completed

Authorization Review Started

Authorization Review Completed
```

---

# Controller Logging

Log:

```text
Request Received

Response Sent
```

Example:

```text
GET /dashboard

Request Received

Response Sent

Duration: 250 ms
```

---

# Resilience Logging

## Retry Logging

Example:

```text
SQL Timeout

Retry Attempt 1

Retry Attempt 2

Retry Successful
```

---

## Circuit Breaker Logging

Example:

```text
Email Service Failed

Circuit Opened

Requests Blocked

Circuit Closed
```

---

## Timeout Logging

Example:

```text
Dashboard Query Exceeded 5 Seconds

Request Aborted
```

---

## Fallback Logging

Example:

```text
Email Failed

Fallback Activated

Reminder Created Instead
```

---

# Future Enhancements

Future centralized logging stack:

```text
Serilog
↓
Seq

or

Serilog
↓
ElasticSearch
↓
Kibana
```

Current MVP:

```text
ILogger<T>

Console Logs

Application Logs

Exception Logs

Performance Logs
```

---

# Logging Coverage Requirements

Every Controller:
✓ Logging

Every Service:
✓ Logging

Every Repository:
✓ Logging

Every Stored Procedure Call:
✓ Logging

Every Exception:
✓ Logging

Every Retry:
✓ Logging

Every Fallback:
✓ Logging

Every Critical Workflow:
✓ Logging

Logging must provide complete traceability from API request initiation to database transaction completion.

```

This document represents the frozen architecture, workflow, database, APIs, resilience strategy, indexing strategy, scalability plan, and implementation roadmap for the Prior Authorization Tracking and Payer Management System.

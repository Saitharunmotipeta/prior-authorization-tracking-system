# Database Design Documentation

# Database Schemas

The database is logically organized into multiple business domains (schemas) to improve maintainability and separation of concerns.

## 1. Master Data Schema

Stores reference and configuration data used across the application.

Tables:

* Facility
* Department
* Patient
* CPTCode
* ICDCode

---

## 2. Payer Management Schema

Stores payer-related configuration and turnaround rules.

Tables:

* Payer

---

## 3. Authorization Workflow Schema

Stores authorization requests and operational workflow data.

Tables:

* Encounter
* AuthorizationRequest
* AuthorizationService

---

## 4. Audit Schema

Stores activity tracking and historical changes.

Tables:

* AuditHistory

---

## 5. Analytics Schema (Logical)

No physical tables required.

Analytics are generated from:

* AuthorizationRequest
* Encounter
* AuditHistory
* Payer

Examples:

* Approval Rate
* Denial Rate
* Average Turnaround Time
* Provider Ranking
* Readiness Score
* Risk Indicators

---

# Table Definitions

## Facility

Stores healthcare facility information.

| Column            | Description                |
| ----------------- | -------------------------- |
| facility_id       | Unique facility identifier |
| facility_name     | Name of facility           |
| facility_location | Facility address/location  |
| created_at        | Record creation timestamp  |
| is_active         | Soft delete flag           |

---

## Department

Stores departments belonging to a facility.

| Column          | Description                  |
| --------------- | ---------------------------- |
| department_id   | Unique department identifier |
| facility_id     | Associated facility          |
| department_name | Department name              |
| created_at      | Record creation timestamp    |
| is_active       | Soft delete flag             |

---

## Patient

Stores patient demographic details.

| Column                | Description               |
| --------------------- | ------------------------- |
| patient_id            | Unique patient identifier |
| mrn_number            | Medical Record Number     |
| full_name             | Patient full name         |
| dob                   | Date of birth             |
| gender                | Patient gender            |
| phone_number          | Contact number            |
| identification_type   | Aadhaar, PAN, etc.        |
| identification_number | Identification value      |
| created_at            | Record creation timestamp |
| is_active             | Soft delete flag          |

---

## Payer

Stores insurance provider configuration.

| Column          | Description                    |
| --------------- | ------------------------------ |
| payer_id        | Unique payer identifier        |
| payer_name      | Insurance provider name        |
| contact_number  | Provider contact               |
| portal_link     | Authorization portal URL       |
| normal_tat_days | Standard turnaround time       |
| urgent_tat_days | Urgent request turnaround time |
| created_at      | Record creation timestamp      |
| is_active       | Soft delete flag               |

---

## CPTCode

Stores procedure codes.

| Column          | Description               |
| --------------- | ------------------------- |
| cpt_code        | CPT code identifier       |
| cpt_description | Procedure description     |
| created_at      | Record creation timestamp |
| is_active       | Soft delete flag          |

---

## ICDCode

Stores diagnosis codes.

| Column          | Description               |
| --------------- | ------------------------- |
| icd_code        | ICD code identifier       |
| icd_description | Diagnosis description     |
| created_at      | Record creation timestamp |
| is_active       | Soft delete flag          |

---

## Encounter

Represents the authorization workflow entry point.

| Column                  | Description                         |
| ----------------------- | ----------------------------------- |
| encounter_id            | Unique encounter identifier         |
| patient_id              | Associated patient                  |
| facility_id             | Associated facility                 |
| department_id           | Associated department               |
| condition_type          | Normal or urgent case               |
| verification_status     | Verification completion status      |
| request_status          | Current authorization status        |
| identification_verified | ID verification completed           |
| prescription_verified   | Prescription verification completed |
| scan_verified           | Scan verification completed         |
| doctor_notes_verified   | Doctor notes verification completed |
| insurance_card_verified | Insurance verification completed    |
| remarks                 | Additional comments                 |
| created_at              | Record creation timestamp           |
| updated_at              | Last modification timestamp         |
| is_active               | Soft delete flag                    |

---

## AuthorizationRequest

Stores insurance authorization request information.

| Column                 | Description                     |
| ---------------------- | ------------------------------- |
| auth_id                | Unique authorization identifier |
| encounter_id           | Associated encounter            |
| payer_id               | Selected payer                  |
| priority               | Normal or Urgent                |
| status                 | Authorization status            |
| estimated_total_amount | Estimated treatment cost        |
| approved_amount        | Approved coverage amount        |
| denial_reason          | Reason for denial (recommended) |
| submitted_at           | Submission timestamp            |
| reviewed_at            | Review timestamp                |
| expiration_date        | Authorization expiry date       |
| created_at             | Record creation timestamp       |
| updated_at             | Last modification timestamp     |

---

## AuthorizationService

Stores services included in an authorization request.

| Column         | Description                      |
| -------------- | -------------------------------- |
| service_id     | Unique service identifier        |
| auth_id        | Associated authorization request |
| cpt_code       | Procedure code                   |
| icd_code       | Diagnosis code                   |
| estimated_cost | Expected procedure cost          |
| notes          | Service-specific notes           |
| created_at     | Record creation timestamp        |

---

## AuditHistory

Stores complete activity tracking.

| Column            | Description                           |
| ----------------- | ------------------------------------- |
| audit_id          | Unique audit identifier               |
| encounter_id      | Associated encounter                  |
| auth_id           | Associated authorization request      |
| entity_name       | Table/entity affected                 |
| entity_id         | Record identifier                     |
| action_type       | Create, Update, Status Change, Delete |
| old_value         | Previous value                        |
| new_value         | Updated value                         |
| performed_by_user | User who performed action             |
| performed_by_role | User role                             |
| remarks           | Additional comments                   |
| created_at        | Activity timestamp                    |

---

# Relationship Summary

Facility
→ Department

Facility
→ Encounter

Department
→ Encounter

Patient
→ Encounter

Encounter
→ AuthorizationRequest

Payer
→ AuthorizationRequest

AuthorizationRequest
→ AuthorizationService

CPTCode
→ AuthorizationService

ICDCode
→ AuthorizationService

Encounter
→ AuditHistory

AuthorizationRequest
→ AuditHistory

---

# Derived Analytics

The following metrics are calculated dynamically and do not require dedicated tables:

* Approval Rate
* Denial Rate
* Average Turnaround Time
* Provider Performance Ranking
* Authorization Readiness Score
* Authorization Risk Indicator
* SLA Breach Prediction
* Critical Request Queue
* Authorization Timeline
* Expiration Monitoring
* Denial Reason Analytics

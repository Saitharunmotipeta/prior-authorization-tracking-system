# Final Database Schema (Frozen Version)

## Facility

| Column            | Data Type                     |
| ----------------- | ----------------------------- |
| facility_id       | INT IDENTITY(1,1) PRIMARY KEY |
| facility_name     | VARCHAR(100)                  |
| facility_location | VARCHAR(200)                  |
| created_at        | DATETIME2(0)                  |
| is_active         | BIT                           |

---

## Department

| Column          | Data Type                     |
| --------------- | ----------------------------- |
| department_id   | INT IDENTITY(1,1) PRIMARY KEY |
| facility_id     | INT FOREIGN KEY               |
| department_name | VARCHAR(100)                  |
| created_at      | DATETIME2(0)                  |
| is_active       | BIT                           |

---

## Patient

| Column                | Data Type                     |
| --------------------- | ----------------------------- |
| patient_id            | INT IDENTITY(1,1) PRIMARY KEY |
| mrn_number            | VARCHAR(20) UNIQUE            |
| full_name             | VARCHAR(150)                  |
| dob                   | DATE                          |
| gender                | CHAR(1)                       |
| phone_number          | VARCHAR(15)                   |
| identification_type   | VARCHAR(20)                   |
| identification_number | VARCHAR(30)                   |
| created_at            | DATETIME2(0)                  |
| is_active             | BIT                           |

---

## Payer

| Column          | Data Type                     |
| --------------- | ----------------------------- |
| payer_id        | INT IDENTITY(1,1) PRIMARY KEY |
| payer_name      | VARCHAR(100)                  |
| contact_number  | VARCHAR(15)                   |
| portal_link     | VARCHAR(255)                  |
| normal_tat_days | SMALLINT                      |
| urgent_tat_days | SMALLINT                      |
| created_at      | DATETIME2(0)                  |
| is_active       | BIT                           |

---

## CPTCode

| Column          | Data Type               |
| --------------- | ----------------------- |
| cpt_code        | VARCHAR(10) PRIMARY KEY |
| cpt_description | VARCHAR(255)            |
| created_at      | DATETIME2(0)            |
| is_active       | BIT                     |

---

## ICDCode

| Column          | Data Type               |
| --------------- | ----------------------- |
| icd_code        | VARCHAR(15) PRIMARY KEY |
| icd_description | VARCHAR(255)            |
| created_at      | DATETIME2(0)            |
| is_active       | BIT                     |

---

## Encounter

| Column                  | Data Type                     |
| ----------------------- | ----------------------------- |
| encounter_id            | INT IDENTITY(1,1) PRIMARY KEY |
| patient_id              | INT FOREIGN KEY               |
| facility_id             | INT FOREIGN KEY               |
| department_id           | INT FOREIGN KEY               |
| condition_type          | TINYINT                       |
| verification_status     | TINYINT                       |
| request_status          | TINYINT                       |
| identification_verified | BIT                           |
| prescription_verified   | BIT                           |
| scan_verified           | BIT                           |
| doctor_notes_verified   | BIT                           |
| insurance_card_verified | BIT                           |
| remarks                 | VARCHAR(500)                  |
| created_at              | DATETIME2(0)                  |
| updated_at              | DATETIME2(0)                  |
| is_active               | BIT                           |

---

## AuthorizationRequest

| Column                 | Data Type                     |
| ---------------------- | ----------------------------- |
| auth_id                | INT IDENTITY(1,1) PRIMARY KEY |
| encounter_id           | INT FOREIGN KEY               |
| payer_id               | INT FOREIGN KEY               |
| priority               | TINYINT                       |
| status                 | TINYINT                       |
| estimated_total_amount | DECIMAL(12,2)                 |
| approved_amount        | DECIMAL(12,2)                 |
| denial_reason          | VARCHAR(255)                  |
| submitted_at           | DATETIME2(0)                  |
| reviewed_at            | DATETIME2(0)                  |
| expiration_date        | DATE                          |
| created_at             | DATETIME2(0)                  |
| updated_at             | DATETIME2(0)                  |

---

## AuthorizationService

| Column         | Data Type                     |
| -------------- | ----------------------------- |
| service_id     | INT IDENTITY(1,1) PRIMARY KEY |
| auth_id        | INT FOREIGN KEY               |
| cpt_code       | VARCHAR(10) FOREIGN KEY       |
| icd_code       | VARCHAR(15) FOREIGN KEY       |
| estimated_cost | DECIMAL(12,2)                 |
| notes          | VARCHAR(500)                  |
| created_at     | DATETIME2(0)                  |

---

## AuditHistory

| Column            | Data Type                     |
| ----------------- | ----------------------------- |
| audit_id          | INT IDENTITY(1,1) PRIMARY KEY |
| encounter_id      | INT FOREIGN KEY NULL          |
| auth_id           | INT FOREIGN KEY NULL          |
| entity_name       | TINYINT                       |
| entity_id         | INT                           |
| action_type       | TINYINT                       |
| old_value         | VARCHAR(200)                  |
| new_value         | VARCHAR(200)                  |
| performed_by_user | VARCHAR(50)                   |
| performed_by_role | TINYINT                       |
| remarks           | VARCHAR(500)                  |
| created_at        | DATETIME2(0)                  |

---

# Enum Mappings

## ConditionType

| Value | Meaning |
| ----- | ------- |
| 1     | Normal  |
| 2     | Urgent  |

---

## VerificationStatus

| Value | Meaning    |
| ----- | ---------- |
| 1     | Pending    |
| 2     | Verified   |
| 3     | Incomplete |

---

## RequestStatus

| Value | Meaning                  |
| ----- | ------------------------ |
| 1     | Pending                  |
| 2     | Approved                 |
| 3     | Denied                   |
| 4     | Request More Information |
| 5     | Expired                  |

---

## Priority

| Value | Meaning |
| ----- | ------- |
| 1     | Normal  |
| 2     | Urgent  |

---

## Audit Entity Types

| Value | Meaning              |
| ----- | -------------------- |
| 1     | Encounter            |
| 2     | AuthorizationRequest |
| 3     | AuthorizationService |
| 4     | Patient              |

---

## Audit Action Types

| Value | Meaning       |
| ----- | ------------- |
| 1     | Create        |
| 2     | Update        |
| 3     | Delete        |
| 4     | Status Change |

---

## User Roles

| Value | Meaning                  |
| ----- | ------------------------ |
| 1     | Authorization Specialist |
| 2     | Clinical Manager         |
| 3     | Revenue Cycle Director   |

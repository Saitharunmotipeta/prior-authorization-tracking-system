# Database Tables

## 1. facilities

Stores healthcare facility information.

Columns:

* facility_id (PK)
* facility_name
* facility_location
* created_at
* is_active

---

## 2. departments

Stores department information for facilities.

Columns:

* department_id (PK)
* facility_id (FK)
* department_name
* created_at
* is_active

---

## 3. patients

Stores patient and provider details.

Columns:

* patient_id (Pk) - guid
* MRN Number 
* patient_name(first name and last name )
* provider_id
//* policy_number
* aadhaar_number
* pan_number
* phone_number
* dob
* gender
* created_at
* is_active

---

## 4. encounters

Stores authorization workflow details and document verification status.

Columns:

* encounter_id (PK)
* patient_id (FK)
* facility_id (FK)
* department_id (FK)
* condition_type
* verification_status
* request_status
* aadhaar_verified
* pan_verified
* prescription_verified
* scan_verified
* doctor_notes_verified
* insurance_card_verified
* remarks
* status_updated_at
* created_at
* updated_at
* is_active

---

## 5. authorization_requests

Stores authorization request details and approval information.

Columns:

* auth_request_id (PK)
* encounter_id (FK)
* estimated_total_amount
* approval_amount
* provider_response
* approval_expiry_date
* submitted_at
* reviewed_at
* created_at
* updated_at
* is_active

---

## 6. authorization_procedures

Stores procedures associated with authorization requests.

Columns:

* procedure_id (PK)
* auth_request_id (FK)
* procedure_code
* procedure_name
* expected_amount
* created_at
* updated_at
* is_active

---

## 7. encounter_history

Stores all activity logs and status changes for encounters.

Columns:

* history_id (PK)
* encounter_id (FK)
* old_status
* new_status
* updated_by
* remarks
* event_type
* created_at

---

# Table Relationships

## 1. facilities → departments

Relationship: One-to-Many

Foreign Key:
departments.facility_id → facilities.facility_id

---

## 2. patients → encounters

Relationship: One-to-Many

Foreign Key:
encounters.patient_id → patients.patient_id

---

## 3. facilities → encounters

Relationship: One-to-Many

Foreign Key:
encounters.facility_id → facilities.facility_id

---

## 4. departments → encounters

Relationship: One-to-Many

Foreign Key:
encounters.department_id → departments.department_id

---

## 5. encounters → authorization_requests

Relationship: One-to-One

Foreign Key:
authorization_requests.encounter_id → encounters.encounter_id

---

## 6. authorization_requests → authorization_procedures

Relationship: One-to-Many

Foreign Key:
authorization_procedures.auth_request_id → authorization_requests.auth_request_id

---

## 7. encounters → encounter_history

Relationship: One-to-Many

Foreign Key:
encounter_history.encounter_id → encounters.encounter_id

# Final API Specification

## Master Data APIs

### GET /facilities

Purpose:
Retrieve all healthcare facilities.

---

### GET /departments

Purpose:
Retrieve all departments.

---

### GET /patients/{id}

Purpose:
Retrieve patient demographics and insurance details.

---

### GET /payers

Purpose:
Retrieve payer information, contact details, portal links, and turnaround times.

---

### GET /cpt-codes

Purpose:
Retrieve available CPT procedure codes.

---

### GET /icd-codes

Purpose:
Retrieve available ICD diagnosis codes.

---

# Authorization Workflow APIs

### Get/ Verify-Insurance-Eligibility

### POST /encounters

Purpose:
Create a new encounter.

Includes:

* Patient
* Facility
* Department
* Document Verification Checklist
* Condition Type

---

### GET /encounters

Purpose:
Retrieve authorization work queue.

Supports Filtering:

* Pending
* Approved
* Denied
* Request More Information
* Expired
* Urgent
* Normal

---

### GET /encounters/{id}

Purpose:
Retrieve complete encounter details.

Returns:

* Encounter Details
* Authorization Request
* Services
* Audit Timeline
* Readiness Score
* Risk Indicator

---

### PATCH /encounters/{id}/status

Purpose:
Update authorization workflow status.

Supported Statuses:

* Pending
* Approved
* Denied
* Request More Information
* Expired

Automatically:

* Updates timestamps
* Creates audit entry
* Updates timeline

---

# Authorization Request APIs

### POST /authorization-requests

Purpose:
Create authorization request.

Includes:

* Payer
* Priority
* Estimated Amount
* Submission Details

---

### POST /authorization-services

Purpose:
Add CPT, ICD, estimated cost and notes.

Supports:

* Multiple services per authorization request.

---

# Audit & Timeline APIs

### GET /audit-history/{encounterId}

Purpose:
Retrieve complete audit history.

Returns:

* Create Events
* Update Events
* Status Changes
* Approval Actions
* Denial Actions
* Expiration Events

---

# Dashboard & Analytics APIs

### GET /dashboard

Purpose:
Retrieve all dashboard metrics in a single response.

Returns:

Authorization Health Metrics:

* Total Requests
* Pending Requests
* Approved Requests
* Denied Requests
* Expired Requests

Operational Metrics:

* Approval Rate
* Denial Rate
* Average Turnaround Time

Readiness Metrics:

* Average Readiness Score
* High Risk Requests

Expiration Metrics:

* Expiring Today
* Expiring Within 3 Days

Provider Performance:

* Top Payers
* Slowest Payers

Denial Analytics:

* Top Denial Reasons

Critical Queue:

* Urgent Requests Pending Review

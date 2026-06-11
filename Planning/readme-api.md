# API Endpoints

## 1. GET /facilities

Purpose:
Fetch all healthcare facilities.

---

## 2. GET /departments

Purpose:
Fetch all departments.

---

## 3. GET /patients/{id}

Purpose:
Fetch patient details along with provider and policy information.

---

## 4. POST /encounters

Purpose:
Create a new encounter with document verification details and initial request status.

---

## 5. GET /encounters

Purpose:
Fetch all encounters for dashboard display and filtering.

---

## 6. GET /encounters/{id}

Purpose:
Fetch complete details of a specific encounter.

---

## 7. PATCH /encounters/{id}/status

Purpose:
Update encounter request status and automatically create encounter history logs.

---

## 8. POST /authorization-requests

Purpose:
Create and store authorization request details including estimated amount and provider response.

---

## 9. GET /authorization-requests/{id}

Purpose:
Fetch authorization request details for a specific encounter.

---

## 10. POST /authorization-procedures

Purpose:
Store procedure codes and expected amounts for authorization requests.

---

## 11. GET /encounter-history/{encounterId}

Purpose:
Fetch complete encounter activity and status history.

---

## 12. GET /analytics/dashboard

Purpose:
Fetch analytics metrics such as approvals, denials, pending requests, and turnaround time.

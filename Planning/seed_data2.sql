INSERT INTO Encounter
(
patient_id,
facility_id,
department_id,
condition_type,
verification_status,
request_status,
identification_verified,
prescription_verified,
scan_verified,
doctor_notes_verified,
insurance_card_verified,
remarks,
created_at,
updated_at,
is_active
)
SELECT TOP 25
patient_id,
((ROW_NUMBER() OVER(ORDER BY patient_id)-1)%3)+1,
((ROW_NUMBER() OVER(ORDER BY patient_id)-1)%15)+1,
((ROW_NUMBER() OVER(ORDER BY patient_id)-1)%3)+1,
1,
CASE
WHEN ROW_NUMBER() OVER(ORDER BY patient_id)%5=0 THEN 2
WHEN ROW_NUMBER() OVER(ORDER BY patient_id)%4=0 THEN 5
ELSE 3
END,
1,1,1,1,1,
'Seed Encounter',
DATEADD(DAY,-ABS(CHECKSUM(NEWID()))%90,GETDATE()),
GETDATE(),
1
FROM Patient;

INSERT INTO AuthorizationRequest
(
encounter_id,
payer_id,
priority,
status,
estimated_total_amount,
approved_amount,
submitted_at,
reviewed_at,
expiration_date,
created_at,
updated_at
)
VALUES

(4,1,1,8,12000,10000,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(5,2,2,8,25000,22000,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(6,3,1,5,32000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(7,1,2,5,45000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(8,2,3,6,67000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(9,3,1,6,15000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(10,1,2,9,34000,NULL,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(11,2,1,8,18000,15000,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(12,3,2,8,55000,51000,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(13,1,1,5,28000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(14,2,3,7,72000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(15,3,2,9,39000,NULL,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE());

INSERT INTO AuthorizationService
(auth_id,cpt_code,icd_code,estimated_cost,notes,created_at)
VALUES

(4,'93306','I25.10',12000,'Echocardiography',GETDATE()),
(4,'93000','R07.9',3000,'ECG',GETDATE()),

(5,'70553','G43.909',18000,'MRI Brain',GETDATE()),
(5,'70450','G40.909',7000,'CT Brain',GETDATE()),

(6,'27447','M17.9',22000,'Knee Replacement',GETDATE()),
(6,'73721','M17.9',10000,'MRI Knee',GETDATE()),

(7,'27130','M54.5',30000,'Hip Arthroplasty',GETDATE()),
(7,'73721','M54.5',15000,'MRI Hip',GETDATE()),

(8,'71260','C34.90',45000,'CT Chest',GETDATE()),
(8,'74177','C34.90',22000,'CT Abdomen',GETDATE()),

(9,'45378','Z12.11',10000,'Colonoscopy',GETDATE()),
(9,'99214','Z12.11',5000,'Consultation',GETDATE()),

(10,'71046','J18.9',8000,'Chest X-Ray',GETDATE()),
(10,'93000','R07.9',5000,'ECG',GETDATE()),

(11,'99213','I10',5000,'Office Visit',GETDATE()),
(11,'93000','I10',8000,'ECG',GETDATE()),

(12,'93458','I21.9',40000,'Coronary Angiography',GETDATE()),
(12,'93306','I21.9',15000,'Echo',GETDATE()),

(13,'70553','G43.909',18000,'MRI Brain',GETDATE()),
(13,'99214','G43.909',5000,'Neurology Review',GETDATE()),

(14,'71260','C50.919',42000,'CT Chest',GETDATE()),
(14,'74177','C50.919',30000,'CT Abdomen',GETDATE()),

(15,'47562','K80.20',28000,'Gallbladder Surgery',GETDATE()),
(15,'74177','K80.20',11000,'CT Abdomen',GETDATE());

INSERT INTO Reminder
(
auth_id,
payer_id,
category,
status,
scheduled_at,
completed_at,
remarks,
updated_at
)
VALUES

(4,1,1,1,GETDATE(),NULL,'Follow Up',GETDATE()),
(5,2,2,1,GETDATE(),NULL,'Escalation',GETDATE()),
(6,3,3,2,DATEADD(DAY,1,GETDATE()),NULL,'Peer Review Scheduled',GETDATE()),
(7,1,4,3,GETDATE(),GETDATE(),'Notification Sent',GETDATE()),
(8,2,1,1,GETDATE(),NULL,'Awaiting Review',GETDATE()),
(9,3,2,1,GETDATE(),NULL,'Escalated',GETDATE()),
(10,1,3,3,GETDATE(),GETDATE(),'Peer Review Complete',GETDATE()),
(11,2,4,3,GETDATE(),GETDATE(),'Reminder Sent',GETDATE());

INSERT INTO AuditHistory
(
encounter_id,
auth_id,
entity_id,
action_type,
old_value,
new_value,
performed_by_role,
remarks,
created_at
)
SELECT
e.encounter_id,
a.auth_id,
CAST(e.encounter_id AS VARCHAR(50)),
1,
NULL,
'Created',
1,
'Encounter Created',
DATEADD(DAY,-10,GETDATE())
FROM Encounter e
LEFT JOIN AuthorizationRequest a
ON e.encounter_id=a.encounter_id;

INSERT INTO AuditHistory
(
encounter_id,
auth_id,
entity_id,
action_type,
old_value,
new_value,
performed_by_role,
remarks,
created_at
)
SELECT
encounter_id,
auth_id,
CAST(auth_id AS VARCHAR(50)),
3,
'Draft',
'Submitted',
1,
'Authorization Submitted',
DATEADD(DAY,-5,GETDATE())
FROM AuthorizationRequest;

INSERT INTO AuditHistory
(
encounter_id,
auth_id,
entity_id,
action_type,
old_value,
new_value,
performed_by_role,
remarks,
created_at
)
SELECT
encounter_id,
auth_id,
CAST(auth_id AS VARCHAR(50)),
CASE
WHEN status=8 THEN 4
WHEN status=9 THEN 5
WHEN status=6 THEN 6
ELSE 2
END,
'UnderReview',
CAST(status AS VARCHAR(20)),
2,
'Review Action',
GETDATE()
FROM AuthorizationRequest;

SELECT 'Facility' AS TableName, COUNT(*) AS TotalRows FROM Facility
UNION ALL
SELECT 'Department', COUNT(*) FROM Department
UNION ALL
SELECT 'Patient', COUNT(*) FROM Patient
UNION ALL
SELECT 'Payer', COUNT(*) FROM Payer
UNION ALL
SELECT 'Policy', COUNT(*) FROM Policy
UNION ALL
SELECT 'CPTCode', COUNT(*) FROM CPTCode
UNION ALL
SELECT 'ICDCode', COUNT(*) FROM ICDCode
UNION ALL
SELECT 'Encounter', COUNT(*) FROM Encounter
UNION ALL
SELECT 'AuthorizationRequest', COUNT(*) FROM AuthorizationRequest
UNION ALL
SELECT 'AuthorizationService', COUNT(*) FROM AuthorizationService
UNION ALL
SELECT 'Reminder', COUNT(*) FROM Reminder
UNION ALL
SELECT 'AuditHistory', COUNT(*) FROM AuditHistory
ORDER BY TableName;
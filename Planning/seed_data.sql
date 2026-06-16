USE PriorAuthorizationDB;
GO

--------------------------------------------------
-- FACILITY
--------------------------------------------------

INSERT INTO Facility
(facility_name,facility_location,created_at,is_active)
VALUES
('TriStar Centennial Medical Center','Nashville, Tennessee',GETDATE(),1),
('Medical City Dallas','Dallas, Texas',GETDATE(),1),
('Research Medical Center','Kansas City, Missouri',GETDATE(),1);

--------------------------------------------------
-- DEPARTMENTS
--------------------------------------------------

INSERT INTO Department
(facility_id,department_name,created_at,is_active)
VALUES

(1,'Cardiology',GETDATE(),1),
(1,'Neurology',GETDATE(),1),
(1,'Orthopedics',GETDATE(),1),
(1,'Oncology',GETDATE(),1),
(1,'Emergency Medicine',GETDATE(),1),

(2,'Cardiology',GETDATE(),1),
(2,'Neurology',GETDATE(),1),
(2,'Orthopedics',GETDATE(),1),
(2,'Oncology',GETDATE(),1),
(2,'Emergency Medicine',GETDATE(),1),

(3,'Cardiology',GETDATE(),1),
(3,'Neurology',GETDATE(),1),
(3,'Orthopedics',GETDATE(),1),
(3,'Oncology',GETDATE(),1),
(3,'Emergency Medicine',GETDATE(),1);

--------------------------------------------------
-- PAYERS
--------------------------------------------------

INSERT INTO Payer
(
payer_name,
contact_number,
payer_email,
normal_tat_days,
urgent_tat_days,
created_at,
is_active
)
VALUES

('UnitedHealthcare',
'8001111111',
'authorizations@uhc.com',
3,
1,
GETDATE(),
1),

('Aetna',
'8002222222',
'authorizations@aetna.com',
4,
2,
GETDATE(),
1),

('Cigna Healthcare',
'8003333333',
'authorizations@cigna.com',
5,
2,
GETDATE(),
1);

--------------------------------------------------
-- CPT CODES
--------------------------------------------------

INSERT INTO CPTCode
VALUES
('99213','Office Visit Established Patient'),
('99214','Detailed Office Visit'),
('93000','Electrocardiogram ECG'),
('71046','Chest X-Ray'),
('74177','CT Abdomen and Pelvis'),
('70450','CT Head Brain'),
('71260','CT Chest'),
('93306','Echocardiography'),
('93458','Coronary Angiography'),
('70553','MRI Brain'),
('73721','MRI Lower Extremity'),
('27130','Total Hip Arthroplasty'),
('27447','Total Knee Arthroplasty'),
('47562','Laparoscopic Cholecystectomy'),
('45378','Diagnostic Colonoscopy');

--------------------------------------------------
-- ICD CODES
--------------------------------------------------

INSERT INTO ICDCode
VALUES
('I10','Essential Hypertension'),
('E11.9','Type 2 Diabetes Mellitus'),
('I25.10','Coronary Artery Disease'),
('I21.9','Acute Myocardial Infarction'),
('G40.909','Epilepsy'),
('G43.909','Migraine'),
('M17.9','Osteoarthritis Knee'),
('M54.5','Low Back Pain'),
('C50.919','Breast Cancer'),
('C34.90','Lung Cancer'),
('J18.9','Pneumonia'),
('N18.9','Chronic Kidney Disease'),
('K80.20','Gallstones'),
('R07.9','Chest Pain'),
('Z12.11','Colon Cancer Screening');



USE PriorAuthorizationDB;
GO

--------------------------------------------------
-- PATIENTS
--------------------------------------------------

INSERT INTO Patient
(
patient_id,
mrn_number,
full_name,
dob,
age,
gender,
phone_number,
identification_type,
identification_number,
created_at,
is_active
)
VALUES

(NEWID(),'MRN100001','James Wilson','1980-05-11',45,'Male','6155551001','SSN','111111111',GETDATE(),1),
(NEWID(),'MRN100002','Emma Johnson','1992-03-22',33,'Female','6155551002','SSN','111111112',GETDATE(),1),
(NEWID(),'MRN100003','Michael Davis','1978-09-14',47,'Male','6155551003','SSN','111111113',GETDATE(),1),

(NEWID(),'MRN100004','Olivia Brown','1988-11-09',37,'Female','6155551004','SSN','111111114',GETDATE(),1),
(NEWID(),'MRN100005','William Miller','1969-06-01',56,'Male','6155551005','SSN','111111115',GETDATE(),1),

(NEWID(),'MRN100006','Sophia Anderson','1990-01-18',35,'Female','6155551006','SSN','111111116',GETDATE(),1),
(NEWID(),'MRN100007','Benjamin Taylor','1977-02-12',48,'Male','6155551007','SSN','111111117',GETDATE(),1),
(NEWID(),'MRN100008','Charlotte Thomas','1986-10-10',39,'Female','6155551008','SSN','111111118',GETDATE(),1),

(NEWID(),'MRN100009','Lucas Moore','1983-04-02',42,'Male','6155551009','SSN','111111119',GETDATE(),1),
(NEWID(),'MRN100010','Amelia Jackson','1995-07-15',30,'Female','6155551010','SSN','111111120',GETDATE(),1),

(NEWID(),'MRN100011','Henry Martin','1972-12-19',53,'Male','6155551011','SSN','111111121',GETDATE(),1),
(NEWID(),'MRN100012','Evelyn White','1981-08-08',44,'Female','6155551012','SSN','111111122',GETDATE(),1),

(NEWID(),'MRN100013','Alexander Harris','1976-05-24',49,'Male','6155551013','SSN','111111123',GETDATE(),1),
(NEWID(),'MRN100014','Harper Thompson','1991-04-11',34,'Female','6155551014','SSN','111111124',GETDATE(),1),

(NEWID(),'MRN100015','Daniel Garcia','1970-01-01',55,'Male','6155551015','SSN','111111125',GETDATE(),1);

-- Continue same pattern till MRN100025


INSERT INTO Policy
(
patient_id,
payer_id,
policy_start_date,
policy_expiry_date,
is_active
)
SELECT
patient_id,
CASE
WHEN ROW_NUMBER() OVER(ORDER BY patient_id) <= 8 THEN 1
WHEN ROW_NUMBER() OVER(ORDER BY patient_id) <= 16 THEN 2
ELSE 3
END,
'2025-01-01',
'2027-12-31',
1
FROM Patient;


--------------------------------------------------
-- ENCOUNTERS
--------------------------------------------------

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
SELECT TOP 15
patient_id,
1,
1,
2,
1,
3,

1,
1,
1,
1,
1,

'Verified Encounter',
GETDATE(),
GETDATE(),
1
FROM Patient;

--------------------------------------------------
-- AUTH REQUESTS
--------------------------------------------------

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

(1,1,2,8,24500,22000,GETDATE(),GETDATE(),'2026-12-31',GETDATE(),GETDATE()),
(2,2,1,5,18000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE()),
(3,3,3,6,72000,NULL,GETDATE(),NULL,'2026-12-31',GETDATE(),GETDATE());


--------------------------------------------------
-- AUTH SERVICES
--------------------------------------------------

INSERT INTO AuthorizationService
(
auth_id,
cpt_code,
icd_code,
estimated_cost,
notes,
created_at
)
VALUES

(1,'93458','I25.10',18000,'Coronary Angiography',GETDATE()),
(1,'93000','R07.9',6500,'ECG',GETDATE()),

(2,'70553','G43.909',18000,'MRI Brain',GETDATE()),

(3,'71260','C34.90',45000,'CT Chest',GETDATE()),
(3,'74177','C34.90',27000,'CT Abdomen',GETDATE());



--------------------------------------------------
-- REMINDERS
--------------------------------------------------

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

(2,2,1,1,GETDATE(),NULL,'Follow Up Required',GETDATE()),
(3,3,3,2,DATEADD(DAY,1,GETDATE()),NULL,'Peer Review Scheduled',GETDATE());



--------------------------------------------------
-- AUDIT HISTORY
--------------------------------------------------

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
VALUES

(1,1,'1',1,NULL,'Encounter Created',1,'Initial Creation',GETDATE()),
(1,1,'1',3,'Draft','Submitted',1,'Submitted To Payer',GETDATE()),
(1,1,'1',4,'UnderReview','Approved',2,'Approved By Payer',GETDATE());



using Dapper;
using Microsoft.Data.SqlClient;
using PriorAuthorization.DataSeeder.Generators;

Console.WriteLine("✅ Data Seeder Started");

var connectionString =
"Server=localhost;Database=PriorAuthorizationDB;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);

connection.Open();

Console.WriteLine("✅ Database Connected Successfully!");

var payerIds = connection.Query<int>("SELECT payer_id FROM Payer").ToList();

// ✅ STEP 1 — Generate patients
var patients = PatientGenerator.Generate(200);

Console.WriteLine($"✅ Generated {patients.Count} patients");

// ✅ STEP 2 — Insert into DB
var sql = @"
INSERT INTO Patient
(patient_id, mrn_number, full_name, dob, age, gender,
 phone_number, identification_type, identification_number,
 created_at, is_active)
VALUES
(@PatientId, @MrnNumber, @FullName, @Dob, @Age, @Gender,
 @PhoneNumber, @IdentificationType, @IdentificationNumber,
 @CreatedAt, @IsActive)
";

connection.Execute(sql, patients);


Console.WriteLine("✅ Patients inserted into DB!");

var facilityIds = connection.Query<int>("SELECT facility_id FROM Facility").ToList();
var departmentIds = connection.Query<int>("SELECT department_id FROM Department").ToList();
var patientIds = connection.Query<Guid>("SELECT patient_id FROM Patient").ToList();

Console.WriteLine("✅ Master data fetched!");


// ✅ 4. Generate Encounters
var encounters = EncounterGenerator.Generate(patientIds, facilityIds, departmentIds);

Console.WriteLine($"✅ Generated {encounters.Count} encounters");


// ✅ 5. Insert Encounters
var encounterSql = @"
INSERT INTO Encounter
(patient_id, facility_id, department_id,
 condition_type, verification_status, request_status,
 identification_verified, prescription_verified, scan_verified,
 doctor_notes_verified, insurance_card_verified,
 remarks, created_at, updated_at, is_active)
VALUES
(@PatientId, @FacilityId, @DepartmentId,
 @ConditionType, @VerificationStatus, @RequestStatus,
 @IdentificationVerified, @PrescriptionVerified, @ScanVerified,
 @DoctorNotesVerified, @InsuranceCardVerified,
 @Remarks, @CreatedAt, @UpdatedAt, @IsActive)
";

connection.Execute(encounterSql, encounters);

Console.WriteLine("✅ Encounters inserted!");

var encounterIds = connection.Query<int>(@"
SELECT e.encounter_id
FROM Encounter e
LEFT JOIN AuthorizationRequest a 
    ON e.encounter_id = a.encounter_id
WHERE a.encounter_id IS NULL
").ToList();


var authRequests = AuthorizationRequestGenerator.Generate(encounterIds, payerIds);


Console.WriteLine($"✅ Generated {authRequests.Count} authorization requests");

var authSql = @"
INSERT INTO AuthorizationRequest
(encounter_id, payer_id, priority, status,
 estimated_total_amount, approved_amount,
 submitted_at, reviewed_at, expiration_date,
 created_at, updated_at)
VALUES
(@EncounterId, @PayerId, @Priority, @Status,
 @EstimatedTotalAmount, @ApprovedAmount,
 @SubmittedAt, @ReviewedAt, @ExpirationDate,
 @CreatedAt, @UpdatedAt)
";

connection.Execute(authSql, authRequests);

Console.WriteLine("✅ AuthorizationRequests inserted!");

var authIds = connection.Query<int>("SELECT auth_id FROM AuthorizationRequest").ToList();

var cptCodes = connection.Query<string>(
    "SELECT cpt_code FROM dbo.CPTCode"
).ToList();

var icdCodes = connection.Query<string>(
    "SELECT icd_code FROM dbo.ICDCode"
).ToList();


var services = AuthorizationServiceGenerator.Generate(authIds, cptCodes, icdCodes);

Console.WriteLine($"✅ Generated {services.Count} services");

var serviceSql = @"
INSERT INTO AuthorizationService
(auth_id, cpt_code, icd_code, estimated_cost, notes, created_at)
VALUES
(@AuthId, @CptCode, @IcdCode, @EstimatedCost, @Notes, @CreatedAt)
";

connection.Execute(serviceSql, services);

Console.WriteLine("✅ AuthorizationServices inserted!");

var authData = connection.Query<(int AuthId, int EncounterId, int Status)>(@"
SELECT auth_id AS AuthId, encounter_id AS EncounterId, status AS Status
FROM AuthorizationRequest
").ToList();

var audits = AuditHistoryGenerator.Generate(authData);

Console.WriteLine($"✅ Generated {audits.Count} audit records");

var auditSql = @"
INSERT INTO AuditHistory
(encounter_id, auth_id, entity_id,
 action_type, old_value, new_value,
 performed_by_role, remarks, created_at)
VALUES
(@EncounterId, @AuthId, @EntityId,
 @ActionType, @OldValue, @NewValue,
 @PerformedByRole, @Remarks, @CreatedAt)
";

connection.Execute(auditSql, audits);

Console.WriteLine("✅ AuditHistory inserted!");


var authStatusData = connection.Query<(int AuthId, int Status)>(@"
SELECT auth_id AS AuthId, status AS Status
FROM AuthorizationRequest
").ToList();

var reminders = ReminderGenerator.Generate(authStatusData, payerIds);

Console.WriteLine($"✅ Generated {reminders.Count} reminders");

var reminderSql = @"
INSERT INTO Reminder
(auth_id, payer_id, category, status,
 scheduled_at, completed_at, remarks, updated_at)
VALUES
(@AuthId, @PayerId, @Category, @Status,
 @ScheduledAt, @CompletedAt, @Remarks, @UpdatedAt)
";

connection.Execute(reminderSql, reminders);

Console.WriteLine("✅ Reminders inserted!");

// ✅ Get patient IDs (use already generated or DB)
var newPatientIds = patients.Select(p => p.PatientId).ToList();


// ✅ Generate policies
var policies = PolicyGenerator.Generate(patientIds, payerIds);


Console.WriteLine($"✅ Generated {policies.Count} policies");

// ✅ Insert
var policySql = @"
INSERT INTO Policy
(patient_id, payer_id, policy_start_date, policy_expiry_date, is_active)
VALUES
(@PatientId, @PayerId, @PolicyStartDate, @PolicyExpiryDate, @IsActive)
";

connection.Execute(policySql, policies);

Console.WriteLine("✅ Policies inserted!");

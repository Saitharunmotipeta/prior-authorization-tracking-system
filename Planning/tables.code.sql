CREATE DATABASE PriorAuthorizationDB;
GO

USE PriorAuthorizationDB;
GO

CREATE TABLE Facility
(
    facility_id INT IDENTITY(1,1) PRIMARY KEY,

    facility_name VARCHAR(100) NOT NULL,

    facility_location VARCHAR(200) NOT NULL,

    created_at DATETIME NOT NULL,

    is_active BIT NOT NULL
);

CREATE TABLE Department
(
    department_id INT IDENTITY(1,1) PRIMARY KEY,

    facility_id INT NOT NULL,

    department_name VARCHAR(100) NOT NULL,

    created_at DATETIME NOT NULL,

    is_active BIT NOT NULL,

    CONSTRAINT FK_Department_Facility
    FOREIGN KEY (facility_id)
    REFERENCES Facility(facility_id)
);

CREATE TABLE Patient
(
    patient_id UNIQUEIDENTIFIER PRIMARY KEY,

    mrn_number VARCHAR(50) NOT NULL UNIQUE,

    full_name VARCHAR(150) NOT NULL,

    dob DATE NOT NULL,

    age TINYINT NOT NULL,

    gender VARCHAR(20) NOT NULL,

    phone_number VARCHAR(20),

    identification_type VARCHAR(50),

    identification_number VARCHAR(100),

    created_at DATETIME NOT NULL,

    is_active BIT NOT NULL
);

CREATE TABLE Payer
(
    payer_id INT IDENTITY(1,1) PRIMARY KEY,

    payer_name VARCHAR(150) NOT NULL,

    contact_number VARCHAR(20),

    payer_email VARCHAR(150),

    normal_tat_days TINYINT NOT NULL,

    urgent_tat_days TINYINT NOT NULL,

    created_at DATETIME NOT NULL,

    is_active BIT NOT NULL
);

CREATE TABLE Policy
(
    policy_id INT IDENTITY(1,1) PRIMARY KEY,

    patient_id UNIQUEIDENTIFIER NOT NULL,

    payer_id INT NOT NULL,

    policy_start_date DATE NOT NULL,

    policy_expiry_date DATE NOT NULL,

    is_active BIT NOT NULL,

    CONSTRAINT FK_Policy_Patient
    FOREIGN KEY (patient_id)
    REFERENCES Patient(patient_id),

    CONSTRAINT FK_Policy_Payer
    FOREIGN KEY (payer_id)
    REFERENCES Payer(payer_id)
);

CREATE TABLE CPTCode
(
    cpt_code VARCHAR(20) PRIMARY KEY,

    cpt_description VARCHAR(500) NOT NULL
);

CREATE TABLE ICDCode
(
    icd_code VARCHAR(20) PRIMARY KEY,

    icd_description VARCHAR(500) NOT NULL
);

CREATE TABLE Encounter
(
    encounter_id INT IDENTITY(1,1) PRIMARY KEY,

    patient_id UNIQUEIDENTIFIER NOT NULL,

    facility_id INT NOT NULL,

    department_id INT NOT NULL,

    condition_type TINYINT NOT NULL,

    verification_status TINYINT NOT NULL,

    request_status TINYINT NOT NULL,

    identification_verified BIT NOT NULL,

    prescription_verified BIT NOT NULL,

    scan_verified BIT NOT NULL,

    doctor_notes_verified BIT NOT NULL,

    insurance_card_verified BIT NOT NULL,

    remarks VARCHAR(MAX),

    created_at DATETIME NOT NULL,

    updated_at DATETIME NOT NULL,

    is_active BIT NOT NULL,

    CONSTRAINT FK_Encounter_Patient
    FOREIGN KEY (patient_id)
    REFERENCES Patient(patient_id),

    CONSTRAINT FK_Encounter_Facility
    FOREIGN KEY (facility_id)
    REFERENCES Facility(facility_id),

    CONSTRAINT FK_Encounter_Department
    FOREIGN KEY (department_id)
    REFERENCES Department(department_id)
);

CREATE TABLE AuthorizationRequest
(
    auth_id INT IDENTITY(1,1) PRIMARY KEY,

    encounter_id INT NOT NULL UNIQUE,

    payer_id INT NOT NULL,

    priority TINYINT NOT NULL,

    status TINYINT NOT NULL,

    estimated_total_amount DECIMAL(18,2) NOT NULL,

    approved_amount DECIMAL(18,2),

    submitted_at DATETIME,

    reviewed_at DATETIME,

    expiration_date DATE,

    created_at DATETIME NOT NULL,

    updated_at DATETIME NOT NULL,

    CONSTRAINT FK_AuthorizationRequest_Encounter
    FOREIGN KEY (encounter_id)
    REFERENCES Encounter(encounter_id),

    CONSTRAINT FK_AuthorizationRequest_Payer
    FOREIGN KEY (payer_id)
    REFERENCES Payer(payer_id)
);

CREATE TABLE AuthorizationService
(
    service_id INT IDENTITY(1,1) PRIMARY KEY,

    auth_id INT NOT NULL,

    cpt_code VARCHAR(20) NOT NULL,

    icd_code VARCHAR(20) NOT NULL,

    estimated_cost DECIMAL(18,2) NOT NULL,

    notes VARCHAR(MAX),

    created_at DATETIME NOT NULL,

    CONSTRAINT FK_AuthorizationService_Auth
    FOREIGN KEY (auth_id)
    REFERENCES AuthorizationRequest(auth_id),

    CONSTRAINT FK_AuthorizationService_CPT
    FOREIGN KEY (cpt_code)
    REFERENCES CPTCode(cpt_code),

    CONSTRAINT FK_AuthorizationService_ICD
    FOREIGN KEY (icd_code)
    REFERENCES ICDCode(icd_code)
);

CREATE TABLE Reminder
(
    reminder_id INT IDENTITY(1,1) PRIMARY KEY,

    auth_id INT NOT NULL,

    payer_id INT NOT NULL,

    category TINYINT NOT NULL,

    status TINYINT NOT NULL,

    scheduled_at DATETIME,

    completed_at DATETIME,

    remarks VARCHAR(MAX),

    updated_at DATETIME NOT NULL,

    CONSTRAINT FK_Reminder_Auth
    FOREIGN KEY (auth_id)
    REFERENCES AuthorizationRequest(auth_id),

    CONSTRAINT FK_Reminder_Payer
    FOREIGN KEY (payer_id)
    REFERENCES Payer(payer_id)
);

CREATE TABLE AuditHistory
(
    audit_id INT IDENTITY(1,1) PRIMARY KEY,

    encounter_id INT NULL,

    auth_id INT NULL,

    entity_id VARCHAR(100),

    action_type TINYINT NOT NULL,

    old_value VARCHAR(MAX),

    new_value VARCHAR(MAX),

    performed_by_role TINYINT NOT NULL,

    remarks VARCHAR(MAX),

    created_at DATETIME NOT NULL,

    CONSTRAINT FK_AuditHistory_Encounter
    FOREIGN KEY (encounter_id)
    REFERENCES Encounter(encounter_id),

    CONSTRAINT FK_AuditHistory_Auth
    FOREIGN KEY (auth_id)
    REFERENCES AuthorizationRequest(auth_id)
);
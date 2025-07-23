/**************************************************************
  Database : MedicalCare
  Purpose  : Core schema for the technical test
  Notes    :
    • Table‑name prefix for *own* columns (e.g., Patient_FirstName)
    • Foreign‑key columns keep the referenced table name (e.g., DoctorId)
    • Basic auditing on every table (CreatedBy/At, ModifiedBy/At)
**************************************************************/
IF DB_ID(N'FichaClinica') IS NULL
    CREATE DATABASE FichaClinica;
GO
USE FichaClinica;
GO

/*------------------------------*
 | 1. Speciality                |
 *------------------------------*/
CREATE TABLE Speciality (
    Speciality_Id           INT             IDENTITY(1,1)     CONSTRAINT PK_Speciality PRIMARY KEY,
    Speciality_Name         NVARCHAR(100)   NOT NULL UNIQUE,
    Speciality_Description  NVARCHAR(250)   NULL,

    -- Audit
    Speciality_CreatedBy    NVARCHAR(128)   NOT NULL,
    Speciality_CreatedAt    DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    Speciality_ModifiedBy   NVARCHAR(128)   NULL,
    Speciality_ModifiedAt   DATETIME2       NULL
);
GO

/*------------------------------*
 | 2. Patient                   |
 *------------------------------*/
CREATE TABLE Patient (
    Patient_Id              INT             IDENTITY(1,1)     CONSTRAINT PK_Patient PRIMARY KEY,
    Patient_FirstName       NVARCHAR(100)   NOT NULL,
    Patient_LastName        NVARCHAR(100)   NOT NULL,
    Patient_RUT             VARCHAR(12)     NOT NULL CONSTRAINT UQ_Patient_RUT UNIQUE,
    Patient_DateOfBirth     DATE            NOT NULL,
    Patient_Gender          CHAR(1)         NULL, -- M/F/O
    Patient_Phone           NVARCHAR(50)    NULL,
    Patient_Email           NVARCHAR(255)   NULL,
    Patient_AddressLine1    NVARCHAR(150)   NULL,
    Patient_AddressLine2    NVARCHAR(150)   NULL,
    Patient_City            NVARCHAR(100)   NULL,
    Patient_State           NVARCHAR(100)   NULL,
    Patient_PostalCode      NVARCHAR(20)    NULL,

    -- Audit
    Patient_CreatedBy       NVARCHAR(128)   NOT NULL,
    Patient_CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    Patient_ModifiedBy      NVARCHAR(128)   NULL,
    Patient_ModifiedAt      DATETIME2       NULL
);
GO

/*------------------------------*
 | 3. Doctor                    |
 *------------------------------*/
CREATE TABLE Doctor (
    Doctor_Id               INT             IDENTITY(1,1)     CONSTRAINT PK_Doctor PRIMARY KEY,
    Doctor_FirstName        NVARCHAR(100)   NOT NULL,
    Doctor_LastName         NVARCHAR(100)   NOT NULL,
    Doctor_Email            NVARCHAR(255)   NULL,
    Doctor_Phone            NVARCHAR(50)    NULL,
    Doctor_LicenseNumber    NVARCHAR(50)    NOT NULL UNIQUE,

    -- FK
    SpecialityId            INT             NOT NULL
        CONSTRAINT FK_Doctor_Speciality
        REFERENCES Speciality (Speciality_Id),

    -- Audit
    Doctor_CreatedBy        NVARCHAR(128)   NOT NULL,
    Doctor_CreatedAt        DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    Doctor_ModifiedBy       NVARCHAR(128)   NULL,
    Doctor_ModifiedAt       DATETIME2       NULL
);
GO

/*------------------------------*
 | 4. Appointment               |
 *------------------------------*/
CREATE TABLE Appointment (
    Appointment_Id              INT           IDENTITY(1,1)   CONSTRAINT PK_Appointment PRIMARY KEY,

    -- FK
    PatientId                   INT           NOT NULL
        CONSTRAINT FK_Appointment_Patient
        REFERENCES Patient (Patient_Id),
    DoctorId                    INT           NOT NULL
        CONSTRAINT FK_Appointment_Doctor
        REFERENCES Doctor (Doctor_Id),

    -- Own columns
    Appointment_StartUtc        DATETIME2     NOT NULL,
    Appointment_EndUtc          DATETIME2     NOT NULL,
    Appointment_Diagnosis       NVARCHAR(500) NULL,
    Appointment_Room            NVARCHAR(30)  NULL,
    Appointment_Status          NVARCHAR(20)  NULL, -- e.g. Scheduled/Completed/Cancelled

    -- Audit
    Appointment_CreatedBy       NVARCHAR(128) NOT NULL,
    Appointment_CreatedAt       DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    Appointment_ModifiedBy      NVARCHAR(128) NULL,
    Appointment_ModifiedAt      DATETIME2     NULL,

    -- Constraint: no overlap for same doctor
    CONSTRAINT CK_Appointment_NoOverlap
        CHECK (Appointment_EndUtc > Appointment_StartUtc)
);
GO

/*------------------------------------------*
 | Optional: Helper index for overlap check |
 *------------------------------------------*/
CREATE UNIQUE INDEX IX_Appointment_Doctor_Time
    ON Appointment (DoctorId, Appointment_StartUtc, Appointment_EndUtc);
GO

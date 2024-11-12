/***********************************************************************************************************************
* Database
***********************************************************************************************************************/
-- Drop DB
--USE tempdb;
--go
--DECLARE @SQL nvarchar(1000);
--IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'CASCETracker')
--BEGIN
--    SET @SQL = N'USE [CASCETracker];

--                 ALTER DATABASE CASCETracker SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- disconnect all other users
--                 use [tempdb];

--                 DROP DATABASE CASCETracker;';
--    EXEC (@SQL);
--END;

---- Create DB
--create database CASCETracker;
--use CASCETracker;
--go

/***********************************************************************************************************************
 * Schemas
 **********************************************************************************************************************/

if not exists (select * from sys.schemas where name = 'core')
exec('create schema core')

if not exists (select * from sys.schemas where name = 'ce')
exec('create schema ce')

/***********************************************************************************************************************
 * Tables
 **********************************************************************************************************************/

/******
* Account Status
******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[AccountStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.AccountStatus
(
  -- primary key
  AccountStatusId int not null identity(1,1)

  -- data
  ,Name varchar(20) not null default ('')
  ,IsActive bit not null default(0)

  ,Constraint PK_AccountStatus Primary Key Clustered (AccountStatusId)
)
go

/*******
* Gender
*******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[Gender]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.Gender
(
  -- primary key
  GenderId int not null identity(1,1)

  -- data
  ,Name varchar(50) not null default ('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Gender Primary Key Clustered (GenderId)
)
go

/*******
* Organization
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'core.Organization') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.Organization
(
  -- primary key
  OrganizationId int not null identity(1,1)

  -- foreign keys
  ,CountryId int not null default(0)

  -- data
  ,LongName varchar(100) not null default('')
  ,ShortName varchar(50) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Organization Primary Key Clustered (OrganizationId)
)
GO

/*******
* Credential
*******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[Credential]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.[Credential]
(
  -- primary key
  CredentialId int not null identity(1,1)

  -- foreign key
  ,OrganizationId int not null default(0)

  -- data
  ,LongName varchar(100) not null default ('')
  ,ShortName varchar(50) not null default ('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Credential Primary Key Clustered (CredentialId)
  ,Constraint FK_Credential_OrganizationId Foreign Key (OrganizationId) References core.[Organization](OrganizationId)
)
GO

/*******
* National Standard
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.NationalStandard') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.NationalStandard
(
  -- primary key
  NationalStandardId int not null identity(1,1)

  -- foreign keys
  ,CountryId int not null default(0)
  ,OwningOrganizationId int not null default(0)

  -- data
  ,LongName varchar(500) not null default('')
  ,ShortName varchar(200) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_NationalStandard Primary Key Clustered (NationalStandardId)
)
GO

/********
* Role
********/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.[Role]
(
  -- primary key
  RoleId int not null identity(1,1)

  -- foreign key
  ,NationalStandardId int not null default(0)

  -- data
  ,LongName varchar(100) not null default ('')
  ,ShortName varchar(50) not null default ('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Role Primary Key Clustered (RoleId)
  ,Constraint FK_Role_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
)
GO

/*****
* User
******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table [core].[User]
(
  -- primary key
  [UserId] int not null identity(1,1)

  -- foreign keys
  ,[GenderId] int not null default(0)
  ,[CredentialId] int not null default(0)
  ,[RoleId] int not null default(0)
  ,[AccountStatusId] int not null default(0)
  
  -- data
  ,[FirstName] varchar(100) not null default('')
  ,[LastName] varchar(100) not null default('')
  ,[Title] varchar(20) not null default('')
  ,[Email] varchar(100) not null default('')
  ,[Image] varchar(100) not null default('')
  ,[Password] varchar(100) not null default('')
  ,[PasswordSalt] varchar(50) not null default('')
  ,[ForcePasswordChange] bit not null default(0)
  ,[DateRegistered] datetime not null

  ,Constraint PK_User Primary Key Clustered (UserId)
  ,Constraint FK_User_GenderId Foreign Key (GenderId) References core.Gender(GenderId)
  ,Constraint FK_User_AccountStatusId Foreign Key (AccountStatusId) References core.AccountStatus(AccountStatusId)
)
go

if not exists (select * from sys.indexes where name = N'User_FirstName' and object_id = OBJECT_ID(N'[core].[User]'))
create nonclustered index User_FirstName on [core].[User](FirstName)
go

if not exists (select * from sys.indexes where name = N'User_LastName' and object_id = OBJECT_ID(N'[core].[User]'))
create nonclustered index User_LastName on [core].[User](LastName)
go

/******
* User History
******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[UserHist]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table [core].[UserHist]
(
  -- primary key
  [UniqueifierId] int not null identity(1,1)

  -- foreign keys
  ,[UserId] int not null default(0)
  ,[GenderId] int not null default(0)
  ,[CredentialId] int not null default(0)
  ,[RoleId] int not null default(0)
  ,[AccountStatusId] int not null default(0)
  
  -- data
  ,[UpdateUserId] int not null default(0)
  ,[UpdateUserName] int not null default(0)
  ,[UpdateDateUTC] datetime not null
  ,IsDeleted bit not null
  ,[FirstName] varchar(100) not null default('')
  ,[LastName] varchar(100) not null default('')
  ,[Title] varchar(20) not null default('')
  ,[Email] varchar(100) not null default('')
  ,[Image] varchar(100) not null default('')
  ,[Password] varchar(100) not null default('')
  ,[PasswordSalt] varchar(50) not null default('')
  ,[ForcePasswordChange] bit not null default(0)
  ,[DateRegistered] datetime not null

  ,Constraint PK_UserHistory Primary Key Clustered (UniqueifierId, UserId)
)
go

/*******
* LoginHistory
*******/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[LoginHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.LoginHistory
(
  -- primary key
  LoginHistoryId int not null identity(1,1)

  -- foreign keys
  ,UserId int not null default(0)

  -- data
  ,Token varchar(255) not null default('')
  ,RefreshToken varchar(255) not null default('')
  ,ExpiresIn int not null default(0)
  ,[IPAddress] varchar(50) not null default('')
  ,[Message] varchar(500) not null default('')
  ,IsSuccessful bit not null default(0)
  ,AttemptedOn datetime null
  ,AttemptedUsername varchar(100) not null default('')
  ,AttemptedPassword varchar(100) not null default('')
  ,Details varchar(max) not null default(0)

  ,Constraint PK_LoginHistory Primary Key Clustered (LoginHistoryId)
)
GO

if not exists (select * from sys.indexes where name = N'LoginHistory_UserId' and object_id = OBJECT_ID(N'[core].[LoginHistory]'))
create nonclustered index LoginHistory_UserId on [core].[LoginHistory](UserId)

/*********
* Work
*********/
if not exists (select * from dbo.sysobjects where ID = object_id(N'[core].[Work]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.Work
(
  -- primary key
  WorkId int not null identity(1,1)

  -- foreign keys
  ,UserId int not null default(0)

  -- data
  ,CompanyName varchar(100) not null default('')
  ,JobTitle varchar(100) not null default('')
  ,StartDate datetime null
  ,EndDate datetime null

  ,Constraint PK_Work Primary Key Clustered (WorkId)
  ,Constraint FK_Work_UserId Foreign Key (UserId) References core.[User](UserId)
)
GO

if not exists (select * from sys.indexes where name = N'Work_UserId' and object_id = OBJECT_ID(N'[core].[Work]'))
create nonclustered index Work_UserId on [core].[Work](UserId)
go

/********
* CE Unit
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Unit') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.Unit
(
  -- primary key
  UnitId int not null identity(1,1)

  -- data
  ,LongNamePlural varchar(50) not null default('')
  ,ShortNamePlural varchar(20) not null default('')
  ,LongNameSingular varchar(50) not null default('')
  ,ShortNameSingular varchar(20) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Unit Primary Key Clustered (UnitId)
)
GO

/********
* Location
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Location') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.[Location]
(
  -- primary key
  LocationId int not null identity(1,1)

  -- data
  ,Name varchar(20) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Location Primary Key Clustered (LocationId)
)
GO

/*******
* CE Experience
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Experience') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.Experience
(
  -- primary key
  ExperienceId int not null identity(1,1)

  -- foreign keys
  ,UserId int not null default(0)
  ,LocationId int not null default(0)

  -- data
  ,CarryForward bit not null default(0)
  ,ProgramTitle varchar(200) not null default('')
  ,EventName varchar(200) not null default('')
  ,StartDate datetime not null
  ,EndDate datetime not null
  ,Description varchar(500) null
  ,Notes varchar(max) null

  ,Constraint PK_Experience Primary Key Clustered (ExperienceId)
  ,Constraint FK_Experience_UserId Foreign Key (UserId) References core.[User](UserId)
  ,Constraint FK_Experience_LocationId Foreign Key (LocationId) References ce.[Location](LocationId)
)
GO

if not exists (select * from sys.indexes where name = N'Experience_UserId' and object_id = OBJECT_ID(N'[ce].[Experience]'))
create nonclustered index Experience_UserId on [ce].[Experience](UserId)
GO

/*******
* Experience History
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.ExperienceHist') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table [ce].ExperienceHist
(
  -- primary key
  UniqueifierId int not null identity(1,1)

  -- foreign keys
  ,ExperienceId int not null default(0)
  ,UserId int not null default(0)
  ,LocationId int not null default(0)

  -- data
  ,[UpdateUserId] int not null default(0)
  ,[UpdateDateUTC] datetime not null
  ,CarryForward bit not null default(0)
  ,ProgramTitle varchar(200) not null default('')
  ,EventName varchar(200) not null default('')
  ,StartDate datetime not null
  ,EndDate datetime not null
  ,Description varchar(500) null 
  ,Notes varchar(max) null
  ,IsDeleted bit not null

  ,Constraint PK_ExperienceHist Primary Key Clustered (UniqueifierId, ExperienceId)
)
GO

/*******
* CategoryList
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.CategoryList') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.CategoryList
(
  -- primary key
  CategoryListId int not null identity(1,1)

  -- data
  ,NationalStandardId int not null default (0)
  ,[Name] varchar(50) not null default('')
  ,DisplayQuestion varchar(200) not null default('')
  ,DisplayOrder int not null default(0)
  ,IsActive bit not null default(0)

  ,Constraint PK_CategoryList Primary Key Clustered (CategoryListId)
)
GO

/*******
* Category
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Category') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.Category
(
  -- primary key
  CategoryId int not null identity(1,1)

  -- foreign keys
  ,NationalStandardId int not null default(0)
  ,CategoryListId int not null default(0)

  -- data
  ,[Name] varchar(50) not null default('')
  ,DisplayName varchar(50) not null default('')
  ,[Description] varchar(500) not null default('')
  ,StartYear int not null default(0)
  ,EndYear int not null default(0)
  ,IsProgressShown bit not null default(0)
  ,IsOverallTotal bit not null default(0)
  ,IsActive bit not null default(0)

  ,Constraint PK_Category Primary Key Clustered (CategoryId)
  ,Constraint FK_Category_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
)
GO

/********
* ExperienceCategory
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.ExperienceCategory') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.ExperienceCategory
(
  -- composite key
  ExperienceId int not null default(0)
  ,CategoryId int not null default(0)

  ,Constraint PK_ExperienceCategory Primary Key clustered (ExperienceId, CategoryId)
  ,Constraint FK_ExperienceCategory_ExperienceId Foreign Key (ExperienceId) References ce.Experience(ExperienceId)
  ,Constraint FK_ExperienceCategory_CategoryId Foreign Key (CategoryId) References ce.Category(CategoryId)
)
GO

if not exists (select * from sys.indexes where name = N'ExperienceCategory_ExperienceId' and object_id = OBJECT_ID(N'[ce].[ExperienceCategory]'))
create nonclustered index ExperienceCategory_ExperienceId on [ce].[ExperienceCategory](ExperienceId)
GO

/*******
* Experience Cat Hist
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.ExperienceCategoryHist') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.ExperienceCategoryHist
(
  -- primary key
  Uniqueifier int not null identity(1,1)

  ,[UpdateUserId] int not null default(0)
  ,[UpdateDateUTC] datetime not null
  ,IsDeleted bit not null
  ,ExperienceId int not null default(0)
  ,CategoryId int not null default(0)

  ,Constraint PK_ExperienceCategoryHist Primary Key Clustered (ExperienceId, CategoryId, Uniqueifier)
)
GO

/*******
* ExperienceAmount
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.ExperienceAmount') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.ExperienceAmount
(
   ExperienceId int not null default(0)
  ,UnitId int not null default(0)

  -- data
  ,Amount decimal(4,1) not null default(000.0)

  ,Constraint PK_ExperienceAmount_ExperienceId Primary Key Clustered (ExperienceId, UnitId)
  ,Constraint FK_ExperienceAmount_ExperienceId Foreign Key (ExperienceId) References ce.Experience(ExperienceId)
  ,Constraint FK_ExperienceCategory_UnitId Foreign Key (UnitId) References ce.Unit(UnitId)
)
GO

if not exists (select * from sys.indexes where name = N'ExperienceAmount_ExperienceId' and object_id = OBJECT_ID(N'[ce].[ExperienceAmount]'))
create nonclustered index ExperienceAmount_ExperienceId on [ce].[ExperienceAmount](ExperienceId)
GO

/*******
* Experience Amount Hist
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.ExperienceAmountHist') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.ExperienceAmountHist
(
  -- primary key
  Uniqueifier int not null identity(1,1)

  ,[UpdateUserId] int not null default(0)
  ,[UpdateDateUTC] datetime not null
  ,IsDeleted bit not null
  ,ExperienceId int not null
  ,UnitId int not null
  ,Amount decimal(4,1) not null default(000.0)

  ,Constraint PK_ExperienceAmountHist Primary Key Clustered (ExperienceId, UnitId, Uniqueifier)
)
GO

-- /*******
-- * CEDataGraphicField
-- *******/
-- if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.DataGraphicField') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
-- create table ce.DataGraphicField
-- (
--   -- primary key
--   DataGraphicFieldId int not null identity(1,1)

--   -- foreign keys
--   ,NationalStandardId int not null default(0)

--   -- data
--   ,DisplayName varchar(50) not null default('')
--   ,DisplayOrder int not null default(0)
--   ,IsActive bit not null default(0)

--   ,Constraint PK_DataGraphicField Primary Key Clustered (DataGraphicFieldId)
--   ,Constraint FK_DataGraphicField_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
-- )
-- GO

-- /*******
-- * DataGraphicFieldCategory
-- *******/
-- if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.DataGraphicFieldCategory') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
-- create table ce.DataGraphicFieldCategory
-- (
--   -- primary key
--   DataGraphicFieldCategoryId int not null identity(1,1)

--   -- foreign keys
--   ,DataGraphicFieldId int not null default(0)
--   ,CategoryId int not null default(0)

--   -- data
--   ,IsActive bit not null default(0)

--   ,Constraint PK_DataGraphicFieldCategory Primary Key Clustered (DataGraphicFieldCategoryId)
--   ,Constraint FK_DataGraphicFieldCategory_DataGraphicFieldId Foreign Key (DataGraphicFieldId) References ce.DataGraphicField(DataGraphicFieldId)
--   ,Constraint FK_DataGraphicFieldCategory_CategoryId Foreign Key (CategoryId) References ce.Category(CategoryId)
-- )
-- GO

/*******
* Compliance Status
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Compliance') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.Compliance
(
  -- primary key
  ComplianceId int not null identity(1,1)

  -- data
  ,[Name] varchar(50) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Compliance Primary Key Clustered (ComplianceId)
)
GO

/*******
* UserComplianceStatus
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.UserCompliance') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.UserCompliance
(
  -- Composite PK
  UserId int not null default(0)
  ,ComplianceId int not null default(0)

  -- data
  ,[Year] int not null default(0)

  ,Constraint PK_UserCompliance Primary Key Clustered (UserId, ComplianceId)
  ,Constraint FK_UserCompliance_UserId Foreign Key (UserId) References core.[User](UserId)
  ,Constraint FK_UserCompliance_ComplianceId Foreign Key (ComplianceId) References ce.Compliance(ComplianceId)
)
GO

/******
* User National Standard
******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.UserNationalStandard') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.UserNationalStandard
(
  -- Composite PK
  UserId int not null default(0)
  ,NationalStandardId int not null default(0)

  -- data
  ,IsActive bit not null default(0)

  ,Constraint PK_UserNationalStandard Primary Key Clustered (UserId, NationalStandardId)
  ,Constraint FK_UserNationalStandard_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
  ,Constraint FK_UserNationalStandard_UserId Foreign Key (UserId) References core.[User](UserId)
)
GO

/*******
* User Organization
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'core.UserOrganization') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.UserOrganization
(
  -- Composite PK
  UserId int not null default(0)
  ,OrganizationId int not null default(0)

  -- data
  ,IsActive bit not null default(0)

  ,Constraint PK_UserOrganization Primary Key Clustered (UserId, OrganizationId)
  ,Constraint FK_UserOrganization_UserId Foreign Key (UserId) References core.[User](UserId)
  ,Constraint FK_UserOrganization_OrganizationId Foreign Key (OrganizationId) References core.Organization(OrganizationId)
)
GO

/*******
* Country
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'core.Country') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.Country
(
  -- primary key
  CountryId int not null identity(1,1)

  -- data
  ,LongName varchar(100) not null default('')
  ,ShortName varchar(100) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_Country Primary Key Clustered (CountryId)
)
GO

/*******
* Rule
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.Rule') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.[Rule]
(
  -- primary key
  RuleId int not null identity(1,1)

  -- foreign keys
  ,NationalStandardId int not null default(0)
  ,UnitId int not null default(0)

  -- data
  ,Name varchar(50) not null default('')
  ,Description varchar(200) not null default('')
  ,Goal int not null default(0)
  ,StartYear int not null default(0)
  ,EndYear int not null default(0)
  ,YearSpan int not null default(0)
  ,IsActive bit not null default(0)

  ,Constraint PK_Rule Primary Key Clustered (RuleId)
  ,Constraint FK_Rule_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
  ,Constraint FK_Rule_UnitId Foreign Key (UnitId) References ce.[Unit](UnitId)
)
GO

/*********
* Rule Condition
*********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.RuleCondition') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.RuleCondition
(
  -- primary key
  RuleConditionId int not null identity(1,1)

  -- foreign keys
  ,RuleId int not null default(0)

  -- data
  ,Goal int not null default(0)
  ,MaxAmount int not null default(0)
  ,IsDoubleCounted bit not null default(0)
  ,IsTask bit not null default(0)
  ,YearSpan int not null default(0)
  ,StartYear int not null default(0)
  ,EndYear int not null default(0)
  ,IsActive bit not null default(0)

  ,Constraint PK_RuleCondition Primary Key Clustered (RuleConditionId)
  ,Constraint FK_Rule_RuleId Foreign Key (RuleId) References ce.[Rule](RuleId)
)
GO

/*******
* Rule Condition Category
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.RuleConditionCategory') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.RuleConditionCategory
(
  -- Composite PK
  RuleConditionId int not null default(0)
  ,CategoryId int not null default(0)

  -- data
  ,IsActive bit not null default(0)

  ,Constraint PK_RuleConditionCategory Primary Key Clustered (RuleConditionId, CategoryId)
  ,Constraint FK_RuleConditionCategory_RuleConditionId Foreign Key (RuleConditionId) References ce.[RuleCondition](RuleConditionId)
  ,Constraint FK_RuleConditionCategory_CategoryId Foreign Key (CategoryId) References ce.Category(CategoryId)
)
GO

/*******
* NatlStandardUnit
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.NatlStandardUnit') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.NatlStandardUnit
(
  -- Composite PK
  NationalStandardId int not null default(0)
  ,UnitId int not null default(0)
 
  -- foreign keys
  ,ParentUnitId int not null default(0)

  -- data
  ,ConversionFormula varchar(100) not null default('')
  ,IsComplianceUnit bit not null default(0)
  ,IsEditable bit not null default(0)
  ,IsActive bit not null default(0)

  ,Constraint PK_NatlStandardUnit Primary Key Clustered (NationalStandardId, UnitId)
  ,Constraint FK_NatlStandardUnit_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
  ,Constraint FK_NatlStandardUnit_UnitId Foreign Key (UnitId) References ce.Unit(UnitId)
)
GO

/*******
* National Standard Organization
*******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'ce.NatlStandardOrg') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table ce.NatlStandardOrg
(
  -- Composite PK
  NationalStandardId int not null default(0)
  ,OrganizationId int not null default(0)

  -- data
  ,StartYear int not null default(0)
  ,EndYear int not null default(0)

  ,Constraint PK_NatlStandardOrg Primary Key Clustered (NationalStandardId, OrganizationId)
  ,Constraint FK_NatlStandardOrg_NationalStandardId Foreign Key (NationalStandardId) References ce.NationalStandard(NationalStandardId)
  ,Constraint FK_NatlStandardOrg_OrganizationId Foreign Key (OrganizationId) References core.Organization(OrganizationId)
)
GO

/******
* Message Type
******/
if not exists (select * from dbo.sysobjects where ID=object_id(N'core.MessageType') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table core.MessageType
(
  -- primary key
  MessageTypeId int not null identity(1,1)

  -- data
  ,[Name] varchar(20) not null default('')
  ,IsActive bit not null default(0)

  ,Constraint PK_MessageType Primary Key Clustered (MessageTypeId)
)
GO

/********
* Log
********/
if not exists (select * from dbo.sysobjects where ID=object_id(N'[core].[Log]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
create table [core].[Log]
(
  -- primary key
  [LogId] int not null identity(1,1)

  -- foreign keys
  ,[MessageTypeId] int not null default(0)

  -- data
  ,[Timestamp] datetime null
  ,[Code] varchar(25) not null default('')
  ,[Method] varchar(150) not null default('')
  ,[Message] varchar(600) not null default('')

  ,Constraint PK_Log Primary Key Clustered (LogId)
  ,Constraint FK_Log_MessageTypeId Foreign Key (MessageTypeId) References core.MessageType(MessageTypeId)
)
GO

/******************
* Login for IISAppPool
******************/
IF NOT EXISTS 
    (SELECT 1  
     FROM master.sys.server_principals
     WHERE name = 'IIS APPPOOL\CETracker')
BEGIN
    CREATE LOGIN [IIS APPPOOL\CETracker] FROM WINDOWS;
END
GO

Use CASCETracker
GO

IF NOT EXISTS
    (SELECT 1
     FROM sys.database_principals
     WHERE name='CETRACKER_SVCACCT')
BEGIN
    CREATE USER [CETRACKER_SVCACCT] FOR LOGIN [IIS APPPOOL\CETracker];
END
GO

IF NOT EXISTS
    (SELECT 1
     FROM sys.database_principals
     WHERE name='CETRACKER_EXECROLE'
     and type_desc='DATABASE_ROLE')
BEGIN
    CREATE ROLE [CETRACKER_EXECROLE];
    ALTER ROLE [CETRACKER_EXECROLE] ADD MEMBER [CETRACKER_SVCACCT];
END
GO

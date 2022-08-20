
-- Gender
if not exists (select 1 from core.Gender where Name=N'Female') insert into core.Gender values (N'Female',1)
if not exists (select 1 from core.Gender where Name=N'Male') insert into core.Gender values (N'Male',1)
if not exists (select 1 from core.Gender where Name=N'Other/Do not wish to answer') insert into core.Gender values (N'Other/Do not wish to answer',1)

-- Country
if not exists (select 1 from core.Country where ShortName=N'USA') insert into core.Country values (N'United States of America', N'USA',1)
if not exists (select 1 from core.Country where ShortName=N'Canada') insert into core.Country values (N'Canada', N'Canada',0)
if not exists (select 1 from core.Country where ShortName=N'UK') insert into core.Country values (N'United Kingdom', N'UK',0)
if not exists (select 1 from core.Country where ShortName=N'Australia') insert into core.Country values (N'Australia', N'Australia',0)
if not exists (select 1 from core.Country where ShortName=N'Hong Kong') insert into core.Country values (N'Hong Kong', N'Hong Kong',0)
if not exists (select 1 from core.Country where ShortName=N'Malaysia') insert into core.Country values (N'Malaysia', N'Malaysia',0)
if not exists (select 1 from core.Country where ShortName=N'Mexico') insert into core.Country values (N'Mexico', N'Mexico',0)
if not exists (select 1 from core.Country where ShortName=N'Taipei') insert into core.Country values (N'Chinese Taipei', N'Taipei',0)
GO

-- CEUnit
if not exists (select 1 from ce.Unit where LongNamePlural=N'Hours') insert into ce.Unit values (N'Hours', N'Hrs.',N'Hour', N'Hr.',1)
if not exists (select 1 from ce.Unit where LongNamePlural=N'Points') insert into ce.Unit values (N'Points', N'Pts.',N'Point', N'Pt.',1)
if not exists (select 1 from ce.Unit where LongNamePlural=N'Credits') insert into ce.Unit values (N'Credits', N'Cr.',N'Credit', N'Cr.',1)
if not exists (select 1 from ce.Unit where LongNamePlural=N'Minutes') insert into ce.Unit values (N'Minutes', N'Min.',N'Minute', N'Min.',1)
go

-- Account Status
if not exists (select 1 from core.AccountStatus where Name=N'Active') insert into core.AccountStatus values (N'Active',1)
if not exists (select 1 from core.AccountStatus where Name=N'Inactive') insert into core.AccountStatus values (N'Inactive',1)
if not exists (select 1 from core.AccountStatus where Name=N'Locked') insert into core.AccountStatus values (N'Locked',1)
GO

-- Compliance Status
if not exists (select 1 from ce.Compliance where Name=N'Compliant') insert into ce.Compliance values (N'Compliant',1)
if not exists (select 1 from ce.Compliance where Name=N'Compliant – NAIC Statement of Actuarial Opinion') insert into ce.Compliance values (N'Compliant – NAIC Statement of Actuarial Opinion',1)
if not exists (select 1 from ce.Compliance where Name=N'Not Currently Providing Actuarial Services') insert into ce.Compliance values (N'Not Currently Providing Actuarial Services',1)
if not exists (select 1 from ce.Compliance where Name=N'Non-Compliant') insert into ce.Compliance values (N'Non-Compliant',1)
GO

-- Organization
if not exists (select 1 from core.Organization where LongName=N'Casualty Actuarial Society') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'USA') , N'Casualty Actuarial Society', N'CAS',1)
if not exists (select 1 from core.Organization where LongName=N'American Academy of Actuaries') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'USA') , N'American Academy of Actuaries', N'AAA',1)
if not exists (select 1 from core.Organization where LongName=N'Canadian Institute of Actuaries') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'Canada') , N'Canadian Institute of Actuaries', N'CIA',0)
if not exists (select 1 from core.Organization where LongName=N'Institute and Faculty of Actuaries') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'UK') , N'Institute and Faculty of Actuaries', N'IFoA',0)
if not exists (select 1 from core.Organization where LongName=N'The Institute of Actuaries of Australia') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'Australia') , N'The Institute of Actuaries of Australia', N'IAAustralia',0)
if not exists (select 1 from core.Organization where LongName=N'Actuarial Society of Hong Kong') insert into core.Organization values ((select CountryId from core.Country where ShortName=N'Hong Kong') , N'Actuarial Society of Hong Kong', N'ASHK',0)
GO

-- CELocation
if not exists (select 1 from ce.Location where Name=N'Home') insert into ce.Location (Name, IsActive) values (N'Home',1)
if not exists (select 1 from ce.Location where Name=N'Online') insert into ce.Location (Name, IsActive) values (N'Online',1)
if not exists (select 1 from ce.Location where Name=N'Work') insert into ce.Location (Name, IsActive) values (N'Work',1)
if not exists (select 1 from ce.Location where Name=N'Other') insert into ce.Location (Name, IsActive) values (N'Other',1)
GO

-- National Standard
if not exists (select 1 from ce.NationalStandard where LongName=N'United States General Qualification Standard') insert into ce.NationalStandard values ((select CountryId from core.Country where ShortName=N'USA'), (select OrganizationId from core.Organization where ShortName=N'AAA'), N'United States General Qualification Standard', N'USQS General' ,1)
if not exists (select 1 from ce.NationalStandard where LongName=N'United States Specific Qualification Standard') insert into ce.NationalStandard values ((select CountryId from core.Country where ShortName=N'USA'), (select OrganizationId from core.Organization where ShortName=N'AAA'), N'United States Specific Qualification Standard', N'USQS Specific' ,1)
GO

-- CE Category List
if not exists (select 1 from core.CECategoryList where Name=N'generalCategories') insert into core.CECategoryList values (N'generalCategories', N'Please indicate the CE Category',1 , 1)
if not exists (select 1 from core.CECategoryList where Name=N'bias') insert into core.CECategoryList values (N'bias', N'Does this CE include a Bias topic?', 2 , 1)
if not exists (select 1 from core.CECategoryList where Name=N'organized') insert into core.CECategoryList values (N'organized', N'Is this CE Organized?',3 , 1)
if not exists (select 1 from core.CECategoryList where Name=N'specificCategories') insert into core.CECategoryList values (N'specificCategories', N'Does this meet USQS Specific Education Requirements under Section 3.3?',4 , 1)
GO

-- CE Category
if not exists (select 1 from core.CECategory where Name=N'Total CE') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), 0, N'Total CE', N'Total CE',N'' ,1900, 9999, 1, 1, 1)
if not exists (select 1 from core.CECategory where Name=N'Professionalism') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'Professionalism', N'Professionalism',N'' ,1900, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Bias') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'),(select CECategoryListId from core.CECategoryList where Name='Bias'), N'Bias', N'Bias Topics',N'' ,2022, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'General Business') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'General Business', N'General Business',N'' ,1900, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Other Relevant') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'Other Relevant', N'Other Relevant',N'' ,1900, 9999, 0, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Organized') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'),(select CECategoryListId from core.CECategoryList where Name='Organized'), N'Organized', N'Organized',N'' ,1900, 9999, 0, 0, 1)

if not exists (select 1 from core.CECategory where Name=N'Total CE' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), 0, N'Total CE', N'Total CE',N'' ,1900, 9999, 1, 1, 1)
if not exists (select 1 from core.CECategory where Name=N'Professionalism' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'Professionalism', N'Professionalism',N'' ,1900, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Bias' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='Bias'), N'Bias', N'Bias Topics',N'' ,2022, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'General Business' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'General Business', N'General Business',N'' ,1900, 9999, 1, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Other Relevant' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='General Categories'), N'Other Relevant', N'Other Relevant',N'' ,1900, 9999, 0, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Organized' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='Organized'), N'Organized', N'Organized',N'' ,1900, 9999, 0, 0, 1)
if not exists (select 1 from core.CECategory where Name=N'Specific' and NationalStandardId='3') insert into core.CECategory values (0, (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'),(select CECategoryListId from core.CECategoryList where Name='Specific Categories'), N'Specific', N'Specific',N'Total for Specific CE Credit' ,1900, 9999, 1, 0, 1)
GO

-- CE Data Graphic Field
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Total CE' and NationalStandardId = ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'))) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), N'Total CE', N'1', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Professionalism' and NationalStandardId = ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'))) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), N'Professionalism', N'2', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Bias' and NationalStandardId = ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'))) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), N'Bias', N'3', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'General Business' and NationalStandardId = ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'))) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), N'General Business', N'4', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Organized' and NationalStandardId = ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'))) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS General'), N'Organized', N'5', 1)

if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Total CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Total CE', N'1', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Professionalism', N'2', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Bias', N'3', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'General Business', N'4', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Organized', N'5', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Specific CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Specific CE', N'6', 1)
if not exists (select 1 from core.CEDataGraphicField where DisplayName=N'Specific Organized CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific')) insert into core.CEDataGraphicField values ((select NationalStandardId from core.NationalStandard where ShortName=N'USQS Specific'), N'Specific Organized CE', N'7', 1)
GO

--CE Data Graphic Category
insert into core.CEDataGraphicFieldCategory values 
(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Total CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,(select CECategoryId from core.CECategory where Name='Total CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,(select CECategoryId from core.CECategory where Name='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,(select CECategoryId from core.CECategory where Name='General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,(select CECategoryId from core.CECategory where Name='Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Total CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Total CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
),(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Specific CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Specific' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Specific Organized CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
),(
	(select CEDataGraphicFieldId from core.CEDataGraphicField where DisplayName='Specific Organized CE' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,(select CECategoryId from core.CECategory where Name='Specific' and NationalStandardId = (select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)

-- Message Type
if not exists (select 1 from core.MessageType where Name=N'Info') insert into core.MessageType values (N'Info', 1)
if not exists (select 1 from core.MessageType where Name=N'Warning') insert into core.MessageType values (N'Warning', 1)
if not exists (select 1 from core.MessageType where Name=N'Error') insert into core.MessageType values (N'Error', 1)
GO

-- National standard CEunit
insert into core.NatlStandardCEUnit values 
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS General')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Hours')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Minutes')
	,'/50' -- divide minutes by 50 to get the hours
	,1
	,0
	,1
)
,(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS General')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Minutes')
	,0 
	,''
	,0
	,1
	,1
)
,(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Hours')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Minutes')
	,'/50' -- divide minutes by 50 to get the hours
	,1
	,0
	,1
)
,(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific')
	,(select CEUnitId from core.CEUnit where LongNamePlural=N'Minutes')
	,0 
	,''
	,0
	,1
	,1
)

-- General Rule and conditions
insert into core.CERule values
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS General')
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,'Total CE'
	,'Total CE hours for the USQS General Requirement'
	,30
	,1900
	,9999
	,1
	,1
)
go

-- Rule for USQS Specific Requirement
insert into core.CERule values
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific')
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,'Total CE'
	,'Total CE hours for the USQS Specific Requirement'
	,30
	,1900
	,9999
	,1
	,1
)
go

-- Rule conditions
-- general
insert into core.CERuleCondition values
(
	-- professionalism
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,3
	,0
	,0
	,0
	,1
	,1900
	,9999
	,1
)
,(
	-- bias
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
	,0
	,1
	,0
	,1
	,2022
	,9999
	,1
)
,(
	-- general business
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,0
	,3
	,0
	,0
	,1
	,1900
	,9999
	,1
)
,(	-- organized
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,6
	,0
	,1
	,0
	,1
	,1900
	,9999
	,1
)

-- specific
insert into core.CERuleCondition values
(
	-- prof
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,3
	,0
	,0
	,0
	,1
	,1900
	,9999
	,1
)
,(
	-- bias
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
	,0
	,1
	,0
	,1
	,2022
	,9999
	,1
)
,(
	-- general business
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,0
	,3
	,0
	,0
	,1
	,1900
	,9999
	,1
)
,(
	-- organized
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,6
	,0
	,1
	,0
	,1
	,1900
	,9999
	,1
)
,(
	-- specific
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,15
	,0
	,1
	,0
	,1
	,1900
	,9999
	,1
)
,(
	-- organized specific
	(select CERuleId from core.CERule where Name='Total CE' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,6
	,0
	,1
	,0
	,1
	,1900
	,9999
	,1
)

-- Rule Condition category linker
insert into core.CERuleConditionCategory values
(
	1
	,(select CECategoryId from core.CECategory where Name='Professionalism' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	2
	,(select CECategoryId from core.CECategory where Name='Bias' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	3
	,(select CECategoryId from core.CECategory where Name='General Business' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	4
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS General'))
	,1
)
,(
	5
	,(select CECategoryId from core.CECategory where Name='Professionalism' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	6
	,(select CECategoryId from core.CECategory where Name='Bias' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	7
	,(select CECategoryId from core.CECategory where Name='General Business' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	8
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	9
	,(select CECategoryId from core.CECategory where Name='Specific' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	10
	,(select CECategoryId from core.CECategory where Name='Specific' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)
,(
	10
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId=(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific'))
	,1
)

-- sample users
insert into core.[User] values
(
	2 -- male
	,0 -- credential
	,0 -- no role
	,1 -- active account
	,'Bob'
	,'Terwilliger'
	,'Sir'
	,'sideshow.bob@simpsons.org'
	,''
	,'test'
	,''
	,0
	,SYSDATETIME()
	,1
	,0
	,0
	,SYSDATETIME()
	,null
	,null
),
(
	1  --female
	,0
	,0
	,1
	,'Helen'
	,'Lovejoy'
	,'Ms.'
	,'helen@simpsons.org'
	,''
	,'test'
	,''
	,0
	,SYSDATETIME()
	,2
	,0
	,0
	,SYSDATETIME()
	,null
	,null
)

-- Link sample users to national standard
insert into core.UserNationalStandard values 
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS General')
	,1 -- sideshow bob
	,1
	,0
	,0
	,0
	,null
	,null
	,null
),
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS General')
	,2 -- helen lovejoy
	,1
	,0
	,0
	,0
	,null
	,null
	,null
),
(
	(select NationalStandardId from core.NationalStandard where ShortName='USQS Specific')
	,2 -- helen lovejoy
	,1
	,0
	,0
	,0
	,null
	,null
	,null
)

-- Experience
-- first experience

insert into core.CEExperience values
(
	1 -- sideshow bob
	,(select celocationid from core.CELocation where name='Other')
	,2022 -- needed?
	,0
	,'Super Learning Day'
	,'Actuary Conference'
	,'2022-02-14'
	,'2022-02-14'
	,'A bunch of classes'
	,''
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	1 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Minutes')
	,150 -- amount
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	1 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,3
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	1
	,(select CECategoryid from core.CECategory where Name='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

-- second experience
insert into core.CEExperience values
(
	1 -- sideshow bob
	,(select celocationid from core.CELocation where name='Other')
	,2022 -- needed?
	,0
	,'Super Learning Day'
	,'Actuary Conference'
	,'2022-02-14'
	,'2022-02-14'
	,'A bunch of classes'
	,''
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	2 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Minutes')
	,60 -- amount
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	2 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,1.2
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	2
	,(select CECategoryid from core.CECategory where Name='General Business' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)
,
(
	2
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

-- third experience
insert into core.CEExperience values
(
	1 -- sideshow bob
	,(select celocationid from core.CELocation where name='Work')
	,2022 -- needed?
	,0
	,'Work convo'
	,'Convo'
	,'2022-01-16'
	,'2022-01-16'
	,'Boooring'
	,''
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	3 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Minutes')
	,30 -- amount
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	3 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,0.6
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	3
	,(select CECategoryId from core.CECategory where Name='Other Relevant' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	3
	,(select CECategoryId from core.CECategory where Name='Bias' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	3
	,(select CECategoryId from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

-- fourth experience
insert into core.CEExperience values
(
	1 -- sideshow bob
	,(select celocationid from core.CELocation where name='Online')
	,2021 -- needed?
	,1 -- Carry forward to 2022 to count for 2023 CE
	,'Actuaries Unite'
	,'Online Class'
	,'2021-06-14'
	,'2021-06-14'
	,'Mathy stuff'
	,''
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	4 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Minutes')
	,50 -- amount
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceAmount values
(
	4 -- experience id
	,(select CEUnitId from core.CEUnit where LongNamePlural='Hours')
	,1
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)

insert into core.CEExperienceCategory values
(
	4 --experience id
	,(select CECategoryid from core.CECategory where Name='Professionalism' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)
,(
	2
	,(select CECategoryid from core.CECategory where Name='Organized' and NationalStandardId = (select NationalStandardId from core.NationalStandard where shortname='USQS General'))
	,1
	,0
	,0
	,'2022-03-01'
	,null
	,null
)
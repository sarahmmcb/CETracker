USE CasCETracker;
GO

IF OBJECT_ID('core.ufn_Select_Experiences_By_User_And_Year', N'IF') IS NOT NULL  
   DROP function core.ufn_Select_Experiences_By_User_And_Year;  
GO 

create function core.ufn_Select_Experiences_By_User_And_Year(
	@UserId int
	,@NationalStandardId int
	,@Year int
)
returns table
as
return
(
	select 
		ex.CEExperienceId
		,cat.CECategoryId
		,ex.StartDate
		,ex.EndDate
		,ca.DisplayName
		,am.Amount
	from core.CEExperience ex
	inner join core.CEExperienceAmount am on am.CEExperienceId = ex.CEExperienceId
	inner join core.CEExperienceCategory cat on cat.CEExperienceId = ex.CEExperienceId
	inner join core.CECategory ca on ca.CECategoryId = cat.CECategoryId
	where 
		(
			(
				ex.StartDate <= DATEFROMPARTS(@Year, 12, 31) 
				and ex.StartDate >= DATEFROMPARTS(@Year, 1, 1) 
				and ex.CarryForward = 0
			)
			OR
			(
				ex.StartDate <= DATEFROMPARTS(@Year-1, 12, 31) 
				and ex.StartDate >= DATEFROMPARTS(@Year-1, 1, 1) 
				and ex.CarryForward = 1
			)
		)
	and
		(ex.UserId = @UserId)
	and
		(
		 am.CEUnitId = (
			select u.CEUnitId from core.CEUnit u
			inner join core.NatlStandardCEUnit n on u.CEUnitId = n.CEUnitId
			where n.NationalStandardId = @NationalStandardId and n.IsComplianceUnit = 1
		 )
		)
);
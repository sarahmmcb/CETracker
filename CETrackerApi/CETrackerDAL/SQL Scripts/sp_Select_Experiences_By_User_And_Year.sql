USE CasCETracker;
GO

IF OBJECT_ID('ce.sp_Select_Experiences_By_User_And_Year', 'P') IS NOT NULL  
   DROP PROCEDURE ce.sp_Select_Experiences_By_User_And_Year;  
GO 

create procedure ce.sp_Select_Experiences_By_User_And_Year
	@UserId int
	,@NationalStandardId int
	,@Year int
as

begin
	select 
		ex.ExperienceId
		,cat.CategoryId
		,ex.StartDate
		,ex.EndDate
		,ca.DisplayName
		,am.Amount
	from ce.Experience ex
	inner join ce.ExperienceAmount am on am.ExperienceId = ex.ExperienceId
	inner join ce.ExperienceCategory cat on cat.ExperienceId = ex.ExperienceId
	inner join ce.Category ca on ca.CategoryId = cat.CategoryId
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
		 am.UnitId = (
			select u.UnitId from ce.Unit u
			inner join ce.NatlStandardUnit n on u.UnitId = n.UnitId
			where n.NationalStandardId = @NationalStandardId and n.IsComplianceUnit = 1
		 )
		)
	order by ex.ExperienceId;
end

RETURN;

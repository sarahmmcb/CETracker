USE CasCETracker;
GO

IF OBJECT_ID('ce.Experiences_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Experiences_S;  
GO 

create procedure ce.Experiences_S
	@UserId int
	,@NationalStandardId int
	,@Year int
as

begin
	select 
		ex.ExperienceId
		,ex.StartDate
		,ex.EndDate
		,ex.ProgramTitle
		,ex.EventName
		,ex.[Description]
		,ex.Notes
		,ca.CategoryId
		,ca.NationalStandardId
		,ca.CategoryListId
		,ca.[Name] as CategoryName
		,ca.DisplayName as CategoryDisplayName
		,am.UnitId
		,am.Amount
		,ex.UserId
		,loc.LocationId
		,loc.[Name] as LocationName
	from ce.Experience ex
	inner join ce.ExperienceAmount am on am.ExperienceId = ex.ExperienceId
	inner join ce.ExperienceCategory cat on cat.ExperienceId = ex.ExperienceId
	inner join ce.Category ca on ca.CategoryId = cat.CategoryId
	inner join ce.[Location] loc on loc.LocationId = ex.LocationId
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
	order by ex.ExperienceId;
end
go

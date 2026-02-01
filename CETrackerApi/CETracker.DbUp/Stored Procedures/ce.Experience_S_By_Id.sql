USE CasCETracker;
GO

IF OBJECT_ID('ce.Experiences_S_By_Id', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Experiences_S_By_Id;  
GO 

create procedure ce.Experiences_S_By_Id
	@ExperienceId INT
as

begin
	select 
		ex.ExperienceId
		,ex.StartDate
		,ex.ProgramTitle
		,ex.EventName
		,ex.CarryForward
		,ex.[Description]
		,ex.Notes
		,cat.CategoryId
		,ca.NationalStandardId
		,ca.CategoryListId
		,ca.[Name] as CategoryName
		,ca.DisplayName as CategoryDisplayName
		,am.UnitId
		,am.Amount
		,nsu.IsComplianceUnit
		,ex.UserId
		,loc.LocationId
		,loc.[Name] as LocationName
	from ce.Experience ex
	inner join ce.ExperienceAmount am on am.ExperienceId = ex.ExperienceId
	inner join ce.ExperienceCategory cat on cat.ExperienceId = ex.ExperienceId
	inner join ce.Category ca on ca.CategoryId = cat.CategoryId
	inner join ce.[Location] loc on loc.LocationId = ex.LocationId
	left join ce.UserData ud on ud.UserId = ex.UserId
	left join ce.NatlStandardUnit nsu on nsu.NationalStandardId = ud.NationalStandardId
	and nsu.UnitId = am.UnitId
	where 
		ex.ExperienceId = @ExperienceId;
end
GO

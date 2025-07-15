USE CasCETracker;
GO

IF OBJECT_ID('ce.UserData_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.UserData_S
GO 

create procedure ce.UserData_S
	@UserId int
as

begin
Select
	ud.UserId
	,Title
	,CanSignSAO
	,ns.NationalStandardId
	,ns.OwningOrganizationId
	,ns.LongName
	,ns.ShortName
	,ns.IsActive
from ce.UserData ud
left join ce.NationalStandard ns on ns.NationalStandardId = ud.NationalStandardId
where
  ud.UserId = @UserId
end
GO

GRANT EXECUTE ON ce.UserData_S TO [CETRACKER_EXECROLE];
GO

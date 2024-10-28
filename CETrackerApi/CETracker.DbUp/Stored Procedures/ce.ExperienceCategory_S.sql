USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceCategory_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceCategory_S;  
GO 

create procedure ce.ExperienceCategory_S
as

begin
	select
		ExperienceId,
		CategoryId
	from
		ce.ExperienceCategory_S
end
GO

GRANT EXECUTE ON ce.ExperienceCategory_S TO [CETRACKER_EXECROLE];
GO

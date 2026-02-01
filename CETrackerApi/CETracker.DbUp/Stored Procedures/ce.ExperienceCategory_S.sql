USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceCategory_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceCategory_S;  
GO 

create procedure ce.ExperienceCategory_S
	@ExperienceId int
as

begin
	select
		ExperienceId,
		CategoryId
	from
		ce.ExperienceCategory
	where ExperienceId = @ExperienceId
end
GO


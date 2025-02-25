USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceCategory_D', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceCategory_D;  
GO

CREATE PROCEDURE ce.ExperienceCategory_D
	@ExperienceId INT
	,@CategoryID INT
	,@UpdateUserId INT
AS

DELETE FROM
	ce.ExperienceCategory
OUTPUT
	@UpdateUserId
	,GETDATE()
	,1 --IsDeleted
	,deleted.ExperienceId
	,deleted.CategoryId
INTO
	ce.ExperienceCategoryHist
WHERE
	ExperienceId = @ExperienceId
	AND
	CategoryId = @CategoryId
GO

GRANT EXECUTE ON ce.ExperienceCategory_D TO [CETRACKER_EXECROLE];
GO
USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceCategory_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceCategory_I;  
GO

CREATE PROCEDURE ce.ExperienceCategory_I
	@ExperienceId INT
	,@CategoryId INT
	,@UpdateUserId INT
AS

INSERT INTO
	ce.ExperienceCategory
OUTPUT
	@UpdateUserId
	,GETDATE()
	,0 --IsDeleted
	,inserted.ExperienceId
	,inserted.CategoryId
INTO
	ce.ExperienceCategoryHist
VALUES
	(
		@ExperienceId
		,@CategoryId
	)

GO

GRANT EXECUTE ON ce.ExperienceCategory_I TO [CETRACKER_EXECROLE];
GO
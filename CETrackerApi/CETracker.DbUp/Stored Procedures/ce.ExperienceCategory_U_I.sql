USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceCategory_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceCategory_U_I;  
GO

CREATE PROCEDURE ce.ExperienceCategory_U_I
	@ExperienceCategoryId INT = NULL
	,@ExperienceId INT
	,@CategoryId INT
	,@UpdateUserId INT
AS

IF @ExperienceCategoryId IS NULL
BEGIN
	INSERT INTO
		ce.ExperienceCategory
	OUTPUT
		@UpdateUserId
		,GETDATE()
		,0 --IsDeleted
		,inserted.ExperienceCategoryId
		,inserted.ExperienceId
		,inserted.CategoryId
	INTO
		ce.ExperienceCategoryHist
	VALUES
		(
			@ExperienceId
			,@CategoryId
		)
END
ELSE
BEGIN
	UPDATE
		ce.ExperienceCategory
	SET
		ExperienceId = @ExperienceId
		,CategoryId = @CategoryId
	OUTPUT
		@UpdateUserId
		,GETDATE()
		,0 --IsDeleted
		,inserted.ExperienceCategoryId
		,inserted.ExperienceId
		,inserted.CategoryId
	INTO
		ce.ExperienceCategoryHist
	WHERE
		ExperienceCategoryId = @ExperienceCategoryId
END

GO

GRANT EXECUTE ON ce.ExperienceCategory_U_I TO [CETRACKER_EXECROLE];
GO
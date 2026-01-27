USE CasCETracker;
GO

IF OBJECT_ID('ce.Experiences_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Experiences_U_I;  
GO

CREATE PROCEDURE ce.Experiences_U_I
	@ExperienceId INT = NULL
	,@UserId INT
	,@LocationId INT
	,@CarryForward TINYINT
	,@ProgramTitle VARCHAR(200)
	,@EventName VARCHAR(200)
	,@StartDate DATETIME
	,@Description VARCHAR(500)
	,@Notes VARCHAR(MAX)
	,@UpdateUserId INT
AS



IF (@ExperienceId IS NULL OR @ExperienceId = 0)
BEGIN
	DECLARE @experience_output TABLE 
	(
		ExperienceId INT
		,UserId INT
		,LocationId INT
		,UpdateUserId INT
		,UpdateDate DATETIME
		,CarryForward BIT
		,ProgramTitle VARCHAR(200)
		,EventName VARCHAR(200)
		,StartDate DATETIME
		,[Description] VARCHAR(500)
		,Notes VARCHAR(MAX)
		,IsDeleted BIT
	);

	INSERT INTO
		ce.Experience
	OUTPUT
		inserted.ExperienceId
		,inserted.UserId
		,inserted.LocationId
		,@UpdateUserId
		,GETDATE()
		,inserted.CarryForward
		,inserted.ProgramTitle
		,inserted.EventName
		,inserted.StartDate
		,inserted.[Description]
		,inserted.Notes
		,0 --IsDeleted
	INTO
		@experience_output
	VALUES
		(@UserId
		,@LocationId
		,@CarryForward
		,@ProgramTitle
		,@EventName
		,@StartDate
		,@Description
		,@Notes)

	INSERT INTO
		ce.ExperienceHist
		SELECT * FROM @experience_output

	SELECT ExperienceId FROM @experience_output

	DELETE @experience_output
END
ELSE
BEGIN
	UPDATE
		ce.Experience
	SET
		UserId = @UserId
		,LocationId = @LocationId
		,CarryForward = @CarryForward
		,ProgramTitle = @ProgramTitle
		,EventName = @EventName
		,StartDate = @StartDate
		,[Description] = @Description
		,Notes = @Notes
	OUTPUT
		@ExperienceId
		,inserted.UserId
		,inserted.LocationId
		,@UpdateUserId
		,GETDATE()
		,inserted.CarryForward
		,inserted.ProgramTitle
		,inserted.EventName
		,inserted.StartDate
		,inserted.[Description]
		,inserted.Notes
		,0
	INTO
		ce.ExperienceHist
	WHERE
		ExperienceId = @ExperienceId

	SELECT @ExperienceId
END
	
GO


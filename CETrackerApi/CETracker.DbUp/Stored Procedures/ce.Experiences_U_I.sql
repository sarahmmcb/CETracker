USE CasCETracker;
GO

IF OBJECT_ID('ce.Experiences_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Experiences_U_I;  
GO

CREATE PROCEDURE ce.Experiences_U_I
	@ExperienceId INT
	,@UserId INT
	,@LocationId INT
	,@CarryForward TINYINT
	,@ProgramTitle VARCHAR(200)
	,@EventName VARCHAR(200)
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@Description VARCHAR(500)
	,@Notes VARCHAR(MAX)
	,@UpdateUserId INT
AS

UPDATE
	ce.Experience
SET
	UserId = @UserId
	,LocationId = @LocationId
	,CarryForward = @CarryForward
	,ProgramTitle = @ProgramTitle
	,EventName = @EventName
	,StartDate = @StartDate
	,EndDate = @EndDate
	,[Description] = @Description
	,Notes = @Notes
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
	,inserted.EndDate
	,inserted.[Description]
	,inserted.Notes
	,0
INTO
	ce.ExperienceHist
WHERE
	ExperienceId = @ExperienceId

IF (@@ROWCOUNT = 0)
BEGIN
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
		,inserted.EndDate
		,inserted.[Description]
		,inserted.Notes
		,0
	INTO
		ce.ExperienceHist
	VALUES
		(@UserId
		,@LocationId
		,@CarryForward
		,@ProgramTitle
		,@EventName
		,@StartDate
		,@EndDate
		,@Description
		,@Notes)
END

SELECT inserted.ExperienceId

GO

GRANT EXECUTE ON ce.Experiences_U_I TO [CETRACKER_EXECROLE];
GO
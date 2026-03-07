USE CASCETracker;
GO

IF OBJECT_ID('ce.Experiences_D', 'P') IS NOT NULL
	DROP PROCEDURE ce.Experiences_D;
GO

CREATE PROCEDURE ce.Experiences_D
	@UpdateUserId INT
	,@ExperienceId INT
AS

SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- delete dependencies of the experience first to avoid DB constraint violations
DELETE
FROM
	ce.ExperienceAmount
OUTPUT
	@UpdateUserId
	,GETDATE()
	,1 --IsDeleted
	,deleted.ExperienceId
	,deleted.UnitId
	,deleted.Amount
INTO
	ce.ExperienceAmountHist
WHERE
	ExperienceId = @ExperienceId

DELETE
FROM
	ce. ExperienceCategory
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

DELETE
FROM
	ce.Experience
OUTPUT
	deleted.ExperienceId
	,deleted.UserId
	,deleted.LocationId
	,@UpdateUserId
	,GETDATE()
	,deleted.CarryForward
	,deleted.ProgramTitle
	,deleted.EventName
	,deleted.StartDate
	,deleted.[Description]
	,deleted.Notes
	,1 --IsDeleted
INTO
	ce.ExperienceHist
WHERE
	ExperienceId = @ExperienceId

COMMIT TRANSACTION;

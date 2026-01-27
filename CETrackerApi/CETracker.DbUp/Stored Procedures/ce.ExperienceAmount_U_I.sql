USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceAmount_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceAmount_U_I;  
GO

CREATE PROCEDURE ce.ExperienceAmount_U_I
	@ExperienceId INT
	,@UnitId INT
	,@Amount DECIMAL(4,1)
	,@UpdateUserId INT
AS

UPDATE
	ce.ExperienceAmount
SET
	Amount = @Amount
OUTPUT
	@UpdateUserId
	,GETDATE()
	,0 --IsDeleted
	,@ExperienceId
	,@UnitId
	,inserted.Amount
INTO
	ce.ExperienceAmountHist
WHERE
	ExperienceId = @ExperienceId
	AND
	UnitId = @UnitId

IF @@ROWCOUNT = 0
BEGIN
	INSERT INTO
		ce.ExperienceAmount
	OUTPUT
		@UpdateUserId
		,GETDATE()
		,0 --IsDeleted
		,inserted.ExperienceId
		,inserted.UnitId
		,inserted.Amount
	INTO
		ce.ExperienceAmountHist
	VALUES
		(
			@ExperienceId
			,@UnitId
			,@Amount
		)
END

GO


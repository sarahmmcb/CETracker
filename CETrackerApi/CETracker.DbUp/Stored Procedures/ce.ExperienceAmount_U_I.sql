USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceAmount_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceAmount_U_I;  
GO

CREATE PROCEDURE ce.ExperienceAmount_U_I
	@ExperienceAmountId INT = NULL
	,@ExperienceId INT
	,@UnitId INT
	,@Amount INT
	,@UpdateUserId INT
AS

IF @ExperienceAmountId IS NULL
BEGIN
	INSERT INTO
		ce.ExperienceAmount
	OUTPUT
		@UpdateUserId
		,GETDATE()
		,0 --IsDeleted
		,inserted.ExperienceAmountId
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
ELSE
BEGIN
	UPDATE
		ce.ExperienceAmount
	SET
		ExperienceId = @ExperienceId
		,UnitId = @UnitId
		,Amount = @Amount
	OUTPUT
		@UpdateUserId
		,GETDATE()
		,0 --IsDeleted
		,inserted.ExperienceAmountId
		,inserted.ExperienceId
		,inserted.UnitId
		,inserted.Amount
	INTO
		ce.ExperienceAmountHist
	WHERE
		ExperienceAmountId = @ExperienceAmountId
END

GO

GRANT EXECUTE ON ce.ExperienceAmount_U_I TO [CETRACKER_EXECROLE];
GO

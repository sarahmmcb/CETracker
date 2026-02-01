USE CasCETracker;
GO

IF OBJECT_ID('ce.ExperienceAmount_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.ExperienceAmount_S;  
GO

CREATE PROCEDURE ce.ExperienceAmount_S
	@ExperienceId INT
AS

SELECT
	ExperienceId
	,ea.UnitId
	,Amount
	,nu.ParentUnitId
FROM
	ce.ExperienceAmount ea
	INNER JOIN ce.NatlStandardUnit nu
		on ea.UnitId = nu.UnitId
WHERE
	ExperienceId = @ExperienceId

GO

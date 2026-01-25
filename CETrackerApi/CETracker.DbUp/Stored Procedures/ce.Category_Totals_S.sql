USE CasCETracker;
GO

IF OBJECT_ID('ce.Category_Totals_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Category_Totals_S;  
GO 

create procedure ce.Category_Totals_S
	@UserId int
	,@NationalStandardId int
	,@Year int
as

WITH preLim AS (
Select
cat.CategoryId
,MAX(cat.DisplayName) AS DisplayName
,SUM(exAm.Amount) AS CategoryTotal
from 
ce.Experience ex
inner join ce.ExperienceCategory exCat on ex.ExperienceId = exCat.ExperienceId
inner join ce.Category cat on cat.CategoryId = exCat.CategoryId
inner join ce.ExperienceAmount exAm on ex.ExperienceId = exAm.ExperienceId
inner join ce.NatlStandardUnit nsu on nsu.UnitId = exAm.UnitId
where
nsu.IsComplianceUnit = 1
AND
nsu.NationalStandardId = @NationalStandardId
and
ex.UserId = @UserId
AND
ex.StartDate <= DATEFROMPARTS(@Year, 12, 31) 
and ex.StartDate >= DATEFROMPARTS(@Year, 1, 1) 
and ex.CarryForward = 0
GROUP BY
	cat.CategoryId
UNION ALL
Select
cat.CategoryId
,MAX(cat.DisplayName) AS DisplayName
,SUM(exAm.Amount) AS CategoryTotal
from 
ce.Experience ex
inner join ce.ExperienceCategory exCat on ex.ExperienceId = exCat.ExperienceId
inner join ce.Category cat on cat.CategoryId = exCat.CategoryId
inner join ce.ExperienceAmount exAm on ex.ExperienceId = exAm.ExperienceId
inner join ce.NatlStandardUnit nsu on nsu.UnitId = exAm.UnitId
where
nsu.IsComplianceUnit = 1
AND
nsu.NationalStandardId = @NationalStandardId
and
ex.UserId = @UserId
AND
ex.StartDate <= DATEFROMPARTS(@Year-1, 12, 31) 
and ex.StartDate >= DATEFROMPARTS(@Year-1, 1, 1) 
and ex.CarryForward = 1
GROUP BY
	cat.CategoryId
)
Select * INTO #tempAmounts
from preLim;

Select
CategoryId
,MAX(DisplayName) AS DisplayName
,SUM(CategoryTotal) AS CategoryTotal
From
#tempAmounts
Group by CategoryId

DROP TABLE #tempAmounts

GRANT EXECUTE ON ce.Category_Totals_S TO [CETRACKER_EXECROLE];
GO


USE CasCETracker;
GO

IF OBJECT_ID('ce.CEData_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.CEData_S;  
GO 

create procedure ce.CEData_S
	@UserId int
	,@NationalStandardId int
	,@Year int
as

IF OBJECT_ID('tempdb..#tempAmounts') IS NOT NULL 
DROP TABLE #tempAmounts;

IF OBJECT_ID('tempdb..#finalAmounts') IS NOT NULL 
DROP TABLE #finalAmounts;

IF OBJECT_ID('tempdb..#rules') IS NOT NULL 
DROP TABLE #rules;

WITH preLim AS (
Select
cat.CategoryId
,MAX(cat.DisplayName) AS DisplayName
,SUM(exAm.Amount) AS CategoryTotal
,MAX(u.ShortNamePlural) AS UnitShortNamePlural
,MAX(u.ShortNameSingular) AS UnitShortNameSingular
from 
ce.Experience ex
left join ce.ExperienceCategory exCat on ex.ExperienceId = exCat.ExperienceId
left join ce.Category cat on cat.CategoryId = exCat.CategoryId
inner join ce.ExperienceAmount exAm on ex.ExperienceId = exAm.ExperienceId
inner join ce.NatlStandardUnit nsu on nsu.UnitId = exAm.UnitId
left join ce.Unit u on u.UnitId = nsu.UnitId
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
,MAX(u.ShortNamePlural) AS UnitShortNamePlural
,MAX(u.ShortNameSingular) AS UnitShortNameSingular
from 
ce.Experience ex
inner join ce.ExperienceCategory exCat on ex.ExperienceId = exCat.ExperienceId
inner join ce.Category cat on cat.CategoryId = exCat.CategoryId
inner join ce.ExperienceAmount exAm on ex.ExperienceId = exAm.ExperienceId
inner join ce.NatlStandardUnit nsu on nsu.UnitId = exAm.UnitId
left join ce.Unit u on u.UnitId = nsu.UnitId
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

WITH final AS (
Select
CategoryId
,MAX(DisplayName) AS DisplayName
,SUM(CategoryTotal) AS CategoryTotal
,MAX(UnitShortNamePlural) AS UnitShortNamePlural
,MAX(UnitShortNameSingular) AS UnitShortNameSingular
From
#tempAmounts
Group by CategoryId
)
Select * into #finalAmounts
from final;

WITH ruleData AS (
Select
r.RuleId
,r.[Name]
,r.Goal
,r.MaxAmount
,r.IsMainGoal
,r.IsAdditionalCategory
,rc.CategoryId
from ce.[Rule] r
left join ce.RuleCategory rc on r.RuleId = rc.RuleId
left join ce.Category cat on cat.CategoryId = rc.CategoryId
where r.NationalStandardId = 1
and r.IsActive = 1
)
Select * into #rules
from ruleData

Select
COALESCE(r.RuleId,0) as RuleId
,COALESCE(r.[Name], 'No Rule') as RuleName
,COALESCE(r.Goal,0) as Goal
,COALESCE(r.MaxAmount, 0) as MaxAmount
,COALESCE(r.IsMainGoal, 0) as IsMainGoal
,COALESCE(r.IsAdditionalCategory, 0) as IsAdditionalCategory
,f.CategoryId
,f.DisplayName
,f.CategoryTotal
,f.UnitShortNamePlural
,f.UnitShortNameSingular
from
#rules r
full outer join #finalAmounts f on f.CategoryId = r.CategoryId

DROP TABLE #tempAmounts
DROP TABLE #finalAmounts
DROP TABLE #rules


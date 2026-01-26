USE CasCETracker;
GO

IF OBJECT_ID('ce.Rule_Data_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Rule_Data_S;  
GO 

create procedure ce.Rule_Data_S
	@NationalStandardId int
as

Select
	r.RuleId
	,r.[Name] as RuleName
	,r.Goal as RuleGoal
	,rc.RuleConditionId
	,rc.Goal
	,rc.MaxAmount
	,rc.IsDoubleCounted
	,rcc.CategoryId
	,cat.[Name] as CategoryName
from ce.[Rule] r
inner join ce.RuleCondition rc on rc.RuleId = r.RuleId
inner join ce.RuleConditionCategory rcc on rcc.RuleConditionId = rc.RuleConditionId
left join ce.Category cat on cat.CategoryId = rcc.CategoryId
where 
	r.NationalStandardId = @NationalStandardId
	and
	rc.IsActive = 1

GRANT EXECUTE ON ce.Rule_Data_S TO [CETRACKER_EXECROLE];
GO
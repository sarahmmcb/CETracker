select
	con.CERuleConditionId
	,rul.[Name] as RuleName
	,rul.Goal as RuleGoal
	,con.Goal as ConditionGoal
	,cat.CECategoryId
	,con.MaxAmount
	,con.IsDoubleCounted
	,con.IsTask
	,con.YearSpan
from core.CERuleCondition con
inner join core.CERule rul on rul.CERuleId = con.CERuleId
inner join core.CeRuleConditionCategory cat on cat.CERuleConditionId = con.CERuleConditionId
where
	rul.NationalStandardId=2
	and con.IsActive = 1
	and
	(
		2022 >= con.StartYear
		and
		2022 <= con.EndYear
	)
		and
	(
		2022 >= rul.StartYear
		and
		2022 <= rul.EndYear
	)
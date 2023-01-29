USE CasCETracker;
GO

IF OBJECT_ID('core.ufn_Select_Rules_By_Year_And_Standard', N'IF') IS NOT NULL  
   DROP function core.ufn_Select_Experiences_By_User_And_Year;  
GO

create function core.ufn_Select_Rules_By_Year_And_Standard(
		@Year int,
		@NationalStandardId int
)
returns table
as
return
(
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
	inner join core.CERuleConditionCategory cat on cat.CERuleConditionId = con.CERuleConditionId
	where
		rul.NationalStandardId=@NationalStandardId
		and con.IsActive = 1
		and
		(
			@Year >= con.StartYear
			and
			@Year <= con.EndYear
		)
			and
		(
			@Year >= rul.StartYear
			and
			@Year <= rul.EndYear
		)
);
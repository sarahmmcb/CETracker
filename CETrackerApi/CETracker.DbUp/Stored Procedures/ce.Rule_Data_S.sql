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
	,r.[Name]
	,r.Goal
	,r.MaxAmount
	,r.IsMainGoal
	,r.IsAdditionalCategory
	,rc.CategoryId
	,cat.[Name] as CategoryName
from ce.[Rule] r
left join ce.RuleCategory rc on r.RuleId = rc.RuleId
left join ce.Category cat on cat.CategoryId = rc.CategoryId
where 
	r.NationalStandardId = @NationalStandardId
	and 
	r.IsActive = 1

GO
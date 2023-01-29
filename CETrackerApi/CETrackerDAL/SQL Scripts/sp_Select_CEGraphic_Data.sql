USE CasCETracker;
GO

IF OBJECT_ID('core.sp_Select_CEGraphic_Data', 'P') IS NOT NULL  
   DROP PROCEDURE core.sp_Select_CEGraphic_Data;  
GO

/*********************
*  Selects Data needed for backend to construct JSON object for frontend data graphic
*  Date Created: 2022-3-21
*  Parameters: UserId, NationalStandardId, Year
**********************/
create procedure core.sp_Select_CEGraphic_Data
	@UserId int
	,@NationalStandardId int
	,@Year int
as

select 
	ex.CEExperienceId
	,ex.UserId
	,cat.CECategoryId
	,ex.StartDate
	,ex.EndDate
	,ca.DisplayName
	,am.Amount
	,con.CERuleConditionId
	,rul.[Name] as RuleName
	,rul.Goal as RuleGoal
	,con.Goal as ConditionGoal
	,con.MaxAmount
	,con.IsDoubleCounted
	,con.IsTask
	,con.YearSpan
from core.ceExperience ex
inner join core.ceexperienceAmount am on am.ceexperienceid = ex.ceexperienceid
inner join core.ceexperiencecategory cat on cat.ceexperienceid = ex.ceexperienceid
inner join core.CECategory ca on ca.CECategoryId = cat.CECategoryId
left join core.CERuleConditionCategory rcat on rcat.CECategoryId = ca.CECategoryId
left join core.CERuleCondition con on con.CERuleConditionId = rcat.CERuleConditionId
left join core.CERule rul on rul.CERuleId = con.CERuleId
where 
(
	(
		ex.StartDate <= DATEFROMPARTS(@Year, 12, 31)  
		and ex.StartDate >= DATEFROMPARTS(@Year, 1, 1) 
		and ex.CarryForward = 0
	)
	OR
	(
		ex.StartDate <= DATEFROMPARTS(@Year, 12, 31)  
		and ex.StartDate >= DATEFROMPARTS(@Year, 1, 1) 
		and ex.CarryForward = 1
	)
)
and
	(ex.userId = @UserId)
and
	(
		 am.ceunitid = (
			select u.ceunitid from core.ceunit u
			inner join core.NatlStandardceUnit n on u.ceunitid = n.ceunitid
			where n.nationalstandardid = @NationalStandardId and n.iscomplianceunit = 1
		 )
	)
and
	rul.NationalStandardId = @NationalStandardId
	and rul.IsActive = 1
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
order by ex.CEExperienceId;

return;

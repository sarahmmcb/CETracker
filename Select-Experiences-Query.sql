
select 
	ex.ceexperienceId
	,ex.userId
	,cat.CECategoryId
	--,typ.DisplayName
	--,ex.programtitle
	--,ex.eventname
	,ex.startdate
	,ex.enddate
	,ca.DisplayName
	,am.Amount
from core.ceExperience ex
inner join core.ceexperienceAmount am on am.ceexperienceid = ex.ceexperienceid
inner join core.ceexperiencecategory cat on cat.ceexperienceid = ex.ceexperienceid
inner join core.CECategory ca on ca.CECategoryId = cat.CECategoryId
where 
(
	(
		ex.StartDate <= '2022-12-31' 
		and ex.StartDate >= '2022-01-01'
		and ex.CarryForward = 0
	)
	OR
	(
		ex.StartDate <= '2021-12-31' 
		and ex.StartDate >= '2021-01-01'
		and ex.CarryForward = 1
	)
)
and
(ex.userId = 1)
and
(
 am.ceunitid = (
	select u.ceunitid from core.ceunit u
	inner join core.NatlStandardceUnit n on u.ceunitid = n.ceunitid
	where n.nationalstandardid = 2 and n.iscomplianceunit = 1
 )
)
order by ex.CEExperienceId

select 
	ex.ExperienceId
	,ex.UserId
	,cat.CategoryId
	,ex.ProgramTitle
	,ex.EventName
	,ex.StartDate
	,ex.EndDate
	,ca.DisplayName
	,am.Amount
from ce.Experience ex
inner join ce.ExperienceAmount am on am.Experienceid = ex.Experienceid
inner join ce.ExperienceCategory cat on cat.Experienceid = ex.Experienceid
inner join ce.Category ca on ca.CategoryId = cat.CategoryId
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
(ex.UserId = 1)
and
(
 am.ceunitid = (
	select u.ceunitid from core.ceunit u
	inner join core.NatlStandardceUnit n on u.ceunitid = n.ceunitid
	where n.nationalstandardid = 2 and n.iscomplianceunit = 1
 )
)
order by ex.CEExperienceId
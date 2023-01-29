if object_id('tempdb..#tempExp', 'U') is not null
drop table #tempExp
go
if object_id('tempdb..#tempRul', 'U')  is not null
drop table #tempRul
go
 
select 
	*
into #tempExp
from core.ufn_Select_Experiences_By_User_And_Year(1, 2, 2022)

select
	*
into #tempRul
from core.ufn_Select_Rules_By_Year_And_Standard(2022, 2)

select * from 
#tempExp e
left join #tempRul r on e.CECategoryId = r.CECategoryId
order by e.CEExperienceId
USE CasCETracker;
GO

IF OBJECT_ID('ce.CategoryLists_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.CategoryLists_S;  
GO 

create procedure ce.CategoryLists_S
	@NationalStandardId int
	,@Year int
as

begin
	select
	 cl.CategoryListId
	 ,cl.[Name]
	 ,cl.DisplayQuestion
	 ,cl.DisplayOrder
	 ,ca.CategoryId
	 ,ca.[Name] as CategoryName
	 ,ca.DisplayName
	 ,ca.NationalStandardId
	 from ce.CategoryList cl
	 inner join ce.Category ca on ca.CategoryListId = cl.CategoryListId
	 where ca.NationalStandardId = @NationalStandardId
	 and ca.IsActive = 1
	 and cl.IsActive = 1
	 and ca.StartYear <= @Year
end

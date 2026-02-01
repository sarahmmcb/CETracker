USE CasCETracker;
GO

IF OBJECT_ID('ce.Units_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Units_S;  
GO 

create procedure ce.Units_S
	@NationalStandardId int
as

begin
	select
      u.UnitId
	  ,nu.NationalStandardId
	  ,nu.ParentUnitId
      ,ShortNameSingular as unitSingular
      ,ShortNamePlural as unitPlural
      ,nu.IsDisabled
	  ,nu.ConversionFormula
  from ce.[Unit] u
  inner join ce.NatlStandardUnit nu on u.UnitId = nu.UnitId
  where
	nu.NationalStandardId = @NationalStandardId
	and
	u.IsActive = 1	
end
GO

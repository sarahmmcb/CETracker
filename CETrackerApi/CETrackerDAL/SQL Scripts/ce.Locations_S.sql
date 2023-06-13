USE CasCETracker;
GO

IF OBJECT_ID('ce.Locations_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Locations_S;  
GO 

create procedure ce.Locations_S
	@NationalStandardId int
as

begin
	select
      l.LocationId
      ,[Name]
  from ce.[Location] l
  inner join ce.NatlStandardLocation nl on l.LocationId = nl.LocationId
  where
	nl.NationalStandardId = @NationalStandardId
	and
	l.IsActive = 1	
end

-- TODO: add ce.NatlStandardLocation table

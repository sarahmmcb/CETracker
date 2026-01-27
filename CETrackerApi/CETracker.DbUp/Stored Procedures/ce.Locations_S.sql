USE CasCETracker;
GO

IF OBJECT_ID('ce.Locations_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Locations_S;  
GO 

create procedure ce.Locations_S
as

begin
	select
      l.LocationId
      ,[Name]
  from ce.[Location] l
  where
	l.IsActive = 1	
end
GO


USE CasCETracker;
GO

IF OBJECT_ID('ce.Units_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.Units_S;  
GO 

create procedure ce.Units_S
as

begin
	select
       UnitId
      ,LongNamePlural
      ,ShortNamePlural
      ,LongNameSingular
      ,ShortNameSingular
      ,IsActive
  FROM ce.[Unit]
end

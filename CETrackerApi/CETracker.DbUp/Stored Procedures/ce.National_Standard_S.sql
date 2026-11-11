USE CasCETracker;
GO

IF OBJECT_ID('ce.National_Standard_S', 'P') IS NOT NULL  
   DROP PROCEDURE ce.National_Standard_S;  
GO 

create procedure ce.National_Standard_S
as

begin
  SELECT [NationalStandardId]
      ,[OrganizationId]
      ,[LongName]
      ,[ShortName]
  FROM [ce].[NationalStandard]
  WHERE isActive = 1
end
GO
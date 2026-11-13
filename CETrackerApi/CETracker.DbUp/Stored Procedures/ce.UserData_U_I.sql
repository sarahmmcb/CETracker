USE CasCETracker;
GO

IF OBJECT_ID('ce.UserData_U_I', 'P') IS NOT NULL  
   DROP PROCEDURE ce.UserData_U_I
GO 

create procedure ce.UserData_U_I
	@UserId int
	,@NationalStandardId int
	,@Title VARCHAR(20)
	,@CanSignSAO bit
	,@UpdateUserId int
	,@IsDeleted bit
as

DECLARE @UpdateDate DATETIMEOFFSET = SYSDATETIMEOFFSET()

begin

UPDATE
 ce.UserData
SET
	NationalStandardId = @NationalStandardId
	,Title = @Title
	,CanSignSAO = @CanSignSAO
OUTPUT
	@UserId
	,inserted.NationalStandardId
	,inserted.Title
	,inserted.CanSignSAO
	,@UpdateUserId
	,@UpdateDate
	,@IsDeleted
INTO
	ce.UserDataHistory
WHERE
	UserId = @UserId

IF @@ROWCOUNT = 0
BEGIN
	INSERT INTO
		ce.UserData
	OUTPUT
		inserted.UserId
		,inserted.NationalStandardId
		,inserted.Title
		,inserted.CanSignSAO
		,@UpdateUserId
		,@UpdateDate
		,0 --IsDeleted
	INTO
		ce.UserDataHistory
	VALUES
		(
			@UserId
			,@NationalStandardId
			,@Title
			,@CanSignSAO
		)
END

end
GO

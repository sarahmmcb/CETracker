SET DBServer="Luo-PC\SQLEXPRESS"
SET CETrackerSchemaScript="C:\Dev\Web\CETracker\CETrackerApi\SQL Scripts\Init-Core.sql"
SET CETrackerDataScript="C:\Dev\Web\CETracker\CETrackerApi\SQL Scripts\Init-Data.sql"

SQLCMD -S %DBServer% -E -i %CETrackerSchemaScript%
SQLCMD -S %DBServer% -E -i %CETrackerDataScript% -d CASCETracker

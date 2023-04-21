using CETracker.Contracts.DataContracts;
using CETrackerDAL.DAL;

namespace CETrackerDAL.Data;

public interface IUnitData
{
    Task<IEnumerable<Unit>> GetUnits(int nationalStandardId);
}
public class UnitData : IUnitData
{
    private readonly IDataAccess _dataAccess;

    public UnitData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<IEnumerable<Unit>> GetUnits(int nationalStandardId)
    {
     var result =   _dataAccess.LoadData<Unit, dynamic>(
            "ce.Units_S", 
            new
            {
                nationalStandardId
            });

        return result;
    }
}

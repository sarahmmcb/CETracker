using CETracker.Contracts.DataContracts;
using CETrackerDAL.DAL;

namespace CETrackerDAL.Data;

public interface IUnitData
{
    Task<IEnumerable<Unit>> GetUnits();
}
public class UnitData : IUnitData
{
    private readonly IDataAccess _dataAccess;

    public UnitData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<IEnumerable<Unit>> GetUnits() =>
        _dataAccess.LoadData<Unit, dynamic>(
            "ce.Units_S", null);
}

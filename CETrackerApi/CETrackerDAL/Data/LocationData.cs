using CETracker.Contracts.DataContracts;
using CETrackerDAL.DAL;

namespace CETrackerDAL.Data;

public interface ILocationData
{
    Task<IEnumerable<Location>> GetLocations(int nationalStandardId);
}
public class LocationData : ILocationData
{
    private readonly IDataAccess _dataAccess;

    public LocationData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<IEnumerable<Location>> GetLocations(int nationalStandardId) =>
        _dataAccess.LoadData<Location, dynamic>(
            "ce.Locations_S",
            new
            {
                nationalStandardId
            }
        );
}

using CETrackerDAL.Data;

namespace CETrackerApi.Logic;

public interface ILocationService
{
    Task<LocationResponse> GetLocations(int nationalStandardId);
}
public class LocationService : ILocationService
{
    private readonly ILocationData _locationData;

    public LocationService(ILocationData locationData)
    {
        _locationData = locationData;
    }

    public async Task<LocationResponse> GetLocations(int nationalStandardId)
    {
        var locations = await _locationData.GetLocations(nationalStandardId);

        return new LocationResponse
        {
            Locations = locations
        };
    }
}

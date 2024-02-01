using CETrackerDAL.Data;

namespace CETrackerApi.Logic;

public interface ILocationService
{
    Task<LocationResponse> GetLocations();
}
public class LocationService : ILocationService
{
    private readonly ILocationData _locationData;

    public LocationService(ILocationData locationData)
    {
        _locationData = locationData;
    }

    public async Task<LocationResponse> GetLocations()
    {
        var locations = await _locationData.GetLocations();

        return new LocationResponse
        {
            Locations = locations
        };
    }
}

namespace CETrackerApi.Logic;

public interface ILocationService
{
    Task<LocationResponse> GetLocations();
}
public class LocationService : ILocationService
{
    private readonly ICeDataProvider _ceDataProvider;

    public LocationService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<LocationResponse> GetLocations()
    {
        var locations = await _ceDataProvider.GetLocations();

        return new LocationResponse
        {
            Locations = locations
        };
    }
}

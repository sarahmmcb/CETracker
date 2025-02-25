namespace CETrackerApi.Logic;

public interface ILocationService
{
    Task<LocationResponse> GetLocations(CancellationToken token);
}
public class LocationService : ILocationService
{
    private readonly ICeDataProvider _ceDataProvider;

    public LocationService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<LocationResponse> GetLocations(CancellationToken token)
    {
        var locations = await _ceDataProvider.GetLocations(token);

        return new LocationResponse
        {
            Locations = locations
        };
    }
}

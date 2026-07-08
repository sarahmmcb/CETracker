using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Locations
{
    public static void ConfigureLocations(this WebApplication app)
    {
        app.MapGet("/api/locations", GetLocations)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetLocations(
        ICeDataProvider ceDataProvider,
        CancellationToken token = default)
    {
        var locations = (await ceDataProvider.GetLocations(token)).ToList() ?? [];

        var result = new LocationResponse
        {
            Locations = locations
        };

        return Results.Ok(result);
    }
}

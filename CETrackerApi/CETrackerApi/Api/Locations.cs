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
        ILocationService locationService,
        CancellationToken token = default)
    {
        var result = await locationService.GetLocations(token);
        if (result == null)
            return Results.NotFound();
        return Results.Ok(result);
    }
}

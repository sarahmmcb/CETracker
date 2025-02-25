using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Locations
{
    public static void ConfigureLocations(this WebApplication app)
    {
        app.MapGet("/api/locations", GetLocations);
    }

    private static async Task<IResult> GetLocations(
        ILocationService locationService,
        CancellationToken token = default)
    {
        try
        {
            var result = await locationService.GetLocations(token);
            if (result == null)
                return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}

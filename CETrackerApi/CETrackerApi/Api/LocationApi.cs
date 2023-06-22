using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class LocationApi
{
    public static void ConfigureLocationApi(this WebApplication app)
    {
        app.MapGet("/api/locations/nationalStandardId/{nationalStandardId}", GetLocations);
    }

    private static async Task<IResult> GetLocations(
        int nationalStandardId,
        ILocationService locationService)
    {
        try
        {
            var result = await locationService.GetLocations(nationalStandardId);
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

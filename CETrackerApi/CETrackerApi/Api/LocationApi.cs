using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public class LocationApi
{
    public static void ConfigureLocationApi(this WebApplication app)
    {
        app.MapGet("/api/locations/nationalStandardId/{nationalStandardId}", GetLocations);
    }

    private static async GetLocations(int nationalStandardId)
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

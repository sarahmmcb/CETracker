using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Units
{
    public static void ConfigureUnits(this WebApplication app)
    {
        app.MapGet("/api/units/nationalStandardId/{nationalStandardId}", GetUnits);
    }

    private static async Task<IResult> GetUnits(int nationalStandardId, IUnitService unitService)
    {
        try
        {
            var result = await unitService.GetUnits(nationalStandardId).ConfigureAwait(false);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}

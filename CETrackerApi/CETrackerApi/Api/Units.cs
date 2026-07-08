using CETracker.Contracts.DataContracts;
using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Units
{
    public static void ConfigureUnits(this WebApplication app)
    {
        app.MapGet("/api/units/nationalStandardId/{nationalStandardId}", GetUnits)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetUnits(
        int nationalStandardId,
        ICeDataProvider ceDataProvider,
        CancellationToken token = default)
    {
        var units = (await ceDataProvider.GetUnits(nationalStandardId, token)).ToList() ?? [];

        var result = new UnitResponse
        {
            Units = units
        };

        return Results.Ok(result);
    }
}

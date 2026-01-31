using System.Threading;
using CETrackerApi.Logic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CETrackerApi.Api;

public static class CEData
{
    public static void ConfigureCeData(this WebApplication app)
    {
        app.MapGet("/api/cedata/year/{year}/userId/{userId}/nationalStandardId/{nationalStandardId}", GetUserCeDataByYear)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetUserCeDataByYear(
        int year,
        int userId,
        int nationalstandardId,
        ICeDataService ceDataService,
        CancellationToken token = default)
    {
        try
        {
            var ceDataResponse = await ceDataService.GetUserCeDataByYear(year, userId, nationalstandardId, token);

            return Results.Ok(ceDataResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

}

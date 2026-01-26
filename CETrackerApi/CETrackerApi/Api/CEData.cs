using System.Threading;
using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class CEData
{
    public static void ConfigureExperiences(this WebApplication app)
    {
        app.MapGet("/api/cedata/year/{year}/userId/{userId}/nationalStandardId/{nationalStandardId}", GetUserCeDataByYear)
            .RequireAuthorization();
    }

    private async Task<IResult> GetUserCeDataByYear(
        int year,
        int userId,
        int nationalstandardId,
        ICeDataService ceDataService,
        CancellationToken token = default)
    {
        try
        {

        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

}

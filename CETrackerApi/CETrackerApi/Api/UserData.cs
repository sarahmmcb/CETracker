using CETracker.Contracts.DataContracts;
using CETrackerApi.Logic;
using static CETrackerApi.Security.TokenAccessor;

namespace CETrackerApi.Api;

public static class UserData
{
    public static void ConfigureUserData(this WebApplication app)
    {
        app.MapGet("/api/userData/userId/{userId}", GetUserData)
            .RequireAuthorization();
    }

    public static async Task<IResult> GetUserData(
        int userId,
        ICeDataProvider ceDataProvider,
        CancellationToken token)
    {
        var result = await ceDataProvider.GetUserData(userId, token);

        if (result == null || !result.Any())
        {
            return Results.NotFound("User not found");
        }

        var userData = result.FirstOrDefault();

        var responseData = new UserDataResponse
        {
            UserId = userData!.UserId,
            Title = userData.Title,
            CanSignSAO = userData.CanSignSAO,
            NationalStandard = new NationalStandard
            {
                NationalStandardId = userData.NationalStandardId,
                OrganizationId = userData.OrganizationId,
                LongName = userData.LongName,
                ShortName = userData.ShortName,
                IsActive = userData.IsActive
            }
        };

        return Results.Ok(responseData);
    }
}

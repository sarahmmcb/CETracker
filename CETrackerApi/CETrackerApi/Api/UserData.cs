using CETrackerApi.Logic;

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
        IUserDataService _userDataService,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userDataService.GetUserData(userId, cancellationToken);

            return Results.Ok(response);
        }
        catch (ApplicationException ex)
        {
            return Results.NotFound(ex);
        }
    }
}

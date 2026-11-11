using CETracker.Contracts.DataContracts;
using CETrackerApi.Logic;
using CETrackerApi.Security;

namespace CETrackerApi.Api;

public static class User
{
    public static void ConfigureUserData(this WebApplication app)
    {
        app.MapGet("/api/userData/userId/{userId}", GetUserData)
            .RequireAuthorization();
        app.MapGet("/api/nationalStandards", GetNationalStandards)
            .RequireAuthorization();
        app.MapPut("/api/userData", UpdateUserData)
            .RequireAuthorization();
        app.MapPost("/api/user", RegisterUser);
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
                LongName = userData.LongName,
                ShortName = userData.ShortName
            }
        };

        return Results.Ok(responseData);
    }

    public static async Task<IResult> GetNationalStandards(
        ICeDataProvider ceDataProvider,
        CancellationToken token)
    {
        var result = await ceDataProvider.GetNationalStandards(token);

        if (result == null || !result.Any())
        {
            return Results.NotFound("No National Standards Found");
        }

        var nationalStandardResponse = new NationalStandardResponse
        {
            NationalStandards = result
        };

        return Results.Ok(nationalStandardResponse);
    }

    public static async Task<IResult> UpdateUserData(
        UpdateUserDataRequest request,
        TokenAccessor tokenAccessor,
        IUserService userService,
        CancellationToken token
    )
    {
        // TODO: Move this into a service class (<-- why did I write this?)
        var userId = tokenAccessor.GetProperty("UserId");
        var parsedUserId = int.TryParse(userId, out var updateUserId);
        if (parsedUserId)
        {
            try
            {
                await userService.UpdateUserData(request, updateUserId, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when trying to update user data for userid: {request.UserId}, {ex.Message}");
                return Results.Problem("An unexpected error occurred, please try again later");
            }
        }
        else
        {
            Console.WriteLine("Error parsing update user id");
            return Results.Problem("An unexpected error ocurred, please try again later");
        }

        return Results.Ok();
    }

    /*
     * 
{
  "firstName": "Moe",
  "lastName": "Syslak",
  "email": "moe.syslak@simpsons.org",
  "password": "MoesPassword",
  "nationalStandardId": 1,
  "title": "Mr.",
  "canSignSAO": false
}
     * */
    public static async Task<IResult> RegisterUser(
        RegisterUserRequest request,
        IUserService userService,
        CancellationToken token)
    {
        try
        {
            var result = await userService.RegisterUser(request, token);

            return Results.Ok(result);
        }
        catch (ApplicationException ex)
        {
            return Results.Problem(ex.Message);
        }

    }
}

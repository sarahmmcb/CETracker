using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Experiences
{
    public static void ConfigureExperiences(this WebApplication app)
    {
        app.MapGet("/api/experiences/year/{year}/userId/{userId}/nationalStandardId/{nationalStandardId}", GetExperiencesByYear);
        //app.MapPost("/api/experiences", UpdateExperience);
        app.MapPut("/api/experiences", UpdateExperience);
    }

    private static async Task<IResult> GetExperiencesByYear(
        int year,
        int userId,
        int nationalStandardId,
        IExperienceService experienceService,
        CancellationToken token = default)
    {
        try
        {
            var result = await experienceService.GetExperiencesByYear(year, userId, nationalStandardId, token).ConfigureAwait(false);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateExperience(
        UpdateExperienceRequest request,
        IExperienceService experienceService,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var experienceId = await experienceService.UpdateExperience(request, cancellationToken);
            return Results.Ok(experienceId);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}

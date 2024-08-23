using CETrackerApi.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CETrackerApi.Api;

public static class Experiences
{
    public static void ConfigureExperiences(this WebApplication app)
    {
        app.MapGet("/api/experiences/year/{year}/userId/{userId}/nationalStandardId/{nationalStandardId}", GetExperiencesByYear);
        app.MapPost("/api/experiences", UpdateExperience);
    }

    private static async Task<IResult> GetExperiencesByYear(
        int year,
        int userId,
        int nationalStandardId,
        IExperienceService experienceService)
    {
        try
        {
            var result = await experienceService.GetExperiencesByYear(year, userId, nationalStandardId).ConfigureAwait(false);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult UpdateExperience(
        UpdateExperienceRequest request,
        IExperienceService experienceService,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var experienceId = experienceService.UpdateExperience(request, cancellationToken);
            return Results.Ok(experienceId);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}

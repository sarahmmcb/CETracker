using System.Security.Claims;
using System.Threading.Tasks;
using CETrackerApi.Helpers;
using CETrackerApi.Logic;
using CETrackerApi.Security;

namespace CETrackerApi.Api;

public static class Experiences
{
    public static void ConfigureExperiences(this WebApplication app)
    {
        app.MapGet("/api/experiences/year/{year}/userId/{userId}/nationalStandardId/{nationalStandardId}", GetExperiencesByYear)
            .RequireAuthorization();
        app.MapPut("/api/experiences", UpdateExperience)
            .RequireAuthorization();
        app.MapDelete("/api/experiences/{experienceId}", DeleteExperience)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetExperiencesByYear(
        int year,
        int userId,
        int nationalStandardId,
        IExperienceService experienceService,
        CancellationToken token = default)
    {
            var result = await experienceService.GetExperiencesByYear(year, userId, nationalStandardId, token).ConfigureAwait(false);
            if (result == null) return Results.NotFound();
            var response = Results.Ok(result);
            return Results.Ok(result);
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

    private static async Task<IResult> DeleteExperience(
        HttpContext context,
        int experienceId,
        IExperienceService experienceService,
        CancellationToken token)
    {
        // TODO: Error message if Experience not found?
        await experienceService.DeleteExperience(experienceId, token);
        return Results.Ok();
    }



}

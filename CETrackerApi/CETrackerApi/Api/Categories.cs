using CETrackerApi.Logic;

namespace CETrackerApi.Api;

public static class Categories
{
    public static void ConfigureCategories(this WebApplication app)
    {
        app.MapGet("/api/categoryLists/nationalStandardId/{nationalStandardId}/year/{year}", GetCategoryLists);
    }

    private static async Task<IResult> GetCategoryLists(
        int nationalStandardId,
        int year,
        ICategoryService categoryService,
        CancellationToken token = default)
    {
        try
        {
            var result = await categoryService.GetCategoryLists(nationalStandardId, year, token).ConfigureAwait(false);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}

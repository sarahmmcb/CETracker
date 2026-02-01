using System.Data;
using CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface ICeDataService
{
    public Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token);
}
public class CeDataService : ICeDataService
{
    private readonly ICeDataProvider _ceDataProvider;

    public CeDataService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
        var ceData = await _ceDataProvider.GetCeData(year, userId, nationalStandardId, token);
        
        if (ceData is null || !ceData.Any())
        {
            throw new ApplicationException("CE Data or Rule Data missing");
        }

        var mainGoal = ceData.Where(c => c.IsMainGoal).FirstOrDefault();
        var categoryTotals = new List<CategoryData>();
        var isCompliant = true;
        var totalCredits = 0m;

        foreach(CeData row in ceData)
        {
            if (row.IsMainGoal)
            {
                continue;
            }

            var categoryData = new CategoryData
            {
                CategoryId = row.CategoryId,
                DisplayName = row.DisplayName,
                Minimum = row.Goal,
                Maximum = row.MaxAmount,
                AmountCompleted = row.CategoryTotal
            };

            categoryTotals.Add(categoryData);

            if (row.CategoryTotal < row.Goal)
            {
                isCompliant = false;
            }

            if (!row.IsAdditionalCategory)
            {
                totalCredits += row.MaxAmount > 0 ? Math.Min(row.MaxAmount, row.CategoryTotal) : row.CategoryTotal;
            }
        }

        if (mainGoal != null && totalCredits < mainGoal.Goal)
        {
            isCompliant = false;
        }

        if (mainGoal != null)
        {
            categoryTotals.Add(new CategoryData
            {
                CategoryId = mainGoal.CategoryId,
                DisplayName = mainGoal.RuleName,
                Minimum = mainGoal.Goal,
                Maximum = mainGoal.MaxAmount,
                AmountCompleted = totalCredits
            });
        }

        var ceDataResponse = new CeDataResponse
        {   
            ComplianceStatus = mainGoal is null ? "Unknown" : isCompliant.ToString(),
            UnitShortNamePlural = mainGoal?.UnitShortNamePlural,
            UnitShortNameSingular = mainGoal?.UnitShortNameSingular,
            CategoryData = categoryTotals,
        };

        return ceDataResponse;
    }

}

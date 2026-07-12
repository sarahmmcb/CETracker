using System.Data;
using CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface ICeDataService
{
    public Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token);
}
public class CeDataService(ICeDataProvider ceDataProvider) : ICeDataService
{
    public async Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
        var ceData = await ceDataProvider.GetCeData(year, userId, nationalStandardId, token);
        
        if (ceData is null || !ceData.Any())
        {
            throw new ApplicationException("CE Data or Rule Data Missing");
        }

        var mainGoal = ceData.Where(c => c.IsMainGoal).FirstOrDefault();
        var categoryTotals = new List<CategoryData>();
        var isCompliant = true;
        var totalCredits = 0m;
        var unitShortNameSingular = "";
        var unitShortNamePlural = "";

        foreach (CeData row in ceData)
        {
            if (row.IsMainGoal)
            {
                continue;
            }

            // TODO: fix SP to not return NULL for these for Total CE row
            if (string.IsNullOrEmpty(unitShortNameSingular) && !string.IsNullOrEmpty(row.UnitShortNameSingular))
            {
                unitShortNameSingular = row.UnitShortNameSingular;
            }

            if (string.IsNullOrEmpty(unitShortNamePlural) && !string.IsNullOrEmpty(row.UnitShortNamePlural))
            {
                unitShortNamePlural = row.UnitShortNamePlural;
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
            UnitShortNamePlural = unitShortNamePlural,
            UnitShortNameSingular = unitShortNameSingular,
            CategoryData = categoryTotals,
        };

        return ceDataResponse;
    }

}

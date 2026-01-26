namespace CETrackerApi.Logic;

public interface ICeDataService
{
    public Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token);
}
public class CeDataService : ICeDataService
{
    private CeDataProvider _ceDataProvider;

    public CeDataService(CeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<CeDataResponse> GetUserCeDataByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
        var categoryTotals = await _ceDataProvider.GetCategoryTotals(year, userId, nationalStandardId, token);
        var ruleData = await _ceDataProvider.GetRuleData(nationalStandardId, token);

    }

}

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
        
        if (ruleData is null
            || ruleData.Count() == 0
            || categoryTotals is null
            || categoryTotals.Count() == 0)
        {
            throw new ApplicationException("CE Data or Rule Data missing");
        }


        var mainGoal = ruleData.First().MainGoal;
        var ceDataResponse = new CeDataResponse();



    }

}

namespace CETrackerApi.Logic;

public interface IUnitService
{
    Task<UnitResponse> GetUnits(int nationalStandardId, CancellationToken token);
}

public class UnitService : IUnitService
{
    private readonly ICeDataProvider _ceDataProvider;

    public UnitService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<UnitResponse> GetUnits(int nationalStandardId, CancellationToken token)
    {
        var result = await _ceDataProvider.GetUnits(nationalStandardId, token);

        return new UnitResponse
        {
            Units = result
        };
    }
}

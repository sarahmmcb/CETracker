namespace CETrackerApi.Logic;

public interface IUnitService
{
    Task<UnitResponse> GetUnits(int nationalStandardId);
}

public class UnitService : IUnitService
{
    private readonly ICeDataProvider _ceDataProvider;

    public UnitService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<UnitResponse> GetUnits(int nationalStandardId)
    {
        var result = await _ceDataProvider.GetUnits(nationalStandardId);

        return new UnitResponse
        {
            Units = result
        };
    }
}

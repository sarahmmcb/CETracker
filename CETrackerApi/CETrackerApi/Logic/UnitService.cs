namespace CETrackerApi.Logic;

public interface IUnitService
{
    Task<UnitResponse> GetUnits();
}

public class UnitService : IUnitService
{
    private readonly IUnitData _unitData;

    public UnitService(IUnitData unitData)
    {
        _unitData = unitData;
    }

    public async Task<UnitResponse> GetUnits()
    {
        var result = await _unitData.GetUnits();

        return new UnitResponse
        {
            Units = result
        };
    }
}

using CETracker.Contracts.DataContracts;

namespace CETrackerApi.Logic;

public interface IUnitService
{
    Task<UnitResponse> GetUnits(int nationalStandardId);
}

public class UnitService : IUnitService
{
    private readonly IUnitData _unitData;

    public UnitService(IUnitData unitData)
    {
        _unitData = unitData;
    }

    public async Task<UnitResponse> GetUnits(int nationalStandardId)
    {
        var result = await _unitData.GetUnits(nationalStandardId);

        return new UnitResponse
        {
            Units = result
        };
    }
}

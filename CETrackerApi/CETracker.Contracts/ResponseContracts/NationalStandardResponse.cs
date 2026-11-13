using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;

public class NationalStandardResponse
{
    public IEnumerable<NationalStandard> NationalStandards { get; set; }
}

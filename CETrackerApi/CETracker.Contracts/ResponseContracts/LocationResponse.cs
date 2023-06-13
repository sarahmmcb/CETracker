using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;
public class LocationResponse
{
    public IEnumerable<Location> Locations { get; set; }
}
